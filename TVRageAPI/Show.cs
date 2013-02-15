using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace TVRageAPI
{
    public class Show : IEnumerable<Season>
    {
        public string Name { get; private set; }
        public int ShowId { get; private set; }
        public int TotalSeasons { get; private set; }
        public ConcurrentBag<Season> Seasons { get; private set; }

        internal Show(int showId, XElement xml)
        {
            ShowId = showId;
            Name = xml.ToString("name");
            TotalSeasons = xml.ToInt32("totalseasons");
            var episodeList = xml.Element("Episodelist");
            if (episodeList != null)
            {
                Seasons = new ConcurrentBag<Season>(
                    (from season in episodeList.Elements().AsParallel()
                     select new Season(season)));
            }
            else
            {
                Seasons = new ConcurrentBag<Season>();
            }
        }

        public Season this[int seasonNum]
        {
            get
            {
                return (from season in Seasons.AsParallel()
                        where season.SeasonNumber == seasonNum
                        select season).FirstOrDefault();
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder(string.Format("{0}:{1}", Name, Environment.NewLine));
            foreach (var season in Seasons)
            {
                sb.AppendLine(season.ToString());
            }
            return sb.ToString();
        }

        #region IEnumerable<Season> Members

        public IEnumerator<Season> GetEnumerator()
        {
            return Seasons.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Seasons.GetEnumerator();
        }

        #endregion
    }
}

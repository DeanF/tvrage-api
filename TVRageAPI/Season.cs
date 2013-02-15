using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace TVRageAPI
{
    public class Season : IEnumerable<Episode>, IComparable<Season>
    {
        public int SeasonNumber { get; private set; }

        public ConcurrentBag<Episode> Episodes { get; private set; }

        public int TotalEpisodes
        {
            get { return Episodes.Count; }
        }

        internal Season(XElement xml)
        {
            if (xml.Attribute("no") != null)
            {
                int num;
                bool res = int.TryParse(xml.Attribute("no").Value, out num);
                if (res)
                {
                    SeasonNumber = num;
                }
            }
            Episodes = new ConcurrentBag<Episode>(
                (from episode in xml.Elements("episode").AsParallel()
                 select new Episode(episode))
                );
        }

        public Episode this[int episodeNum]
        {
            get
            {
                return (from episode in Episodes.AsParallel()
                where episode.EpisodeNumber == episodeNum
                select episode).FirstOrDefault();
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder(string.Format("Season #{0}:{1}", SeasonNumber, Environment.NewLine));
            foreach (var episode in Episodes)
            {
                sb.AppendLine(episode.ToString());
            }
            return sb.ToString();
        }

        #region IEnumerable<Episode> Members

        public IEnumerator<Episode> GetEnumerator()
        {
            return Episodes.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IComparable<Season> Members

        public int CompareTo(Season other)
        {
            return SeasonNumber - other.SeasonNumber;
        }

        #endregion
    }
}

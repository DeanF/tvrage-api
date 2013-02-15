using System;
using System.Xml.Linq;

namespace TVRageAPI
{
    public class Episode : IComparable<Episode>
    {
        public int EpisodeNumber { get; private set; }

        public DateTime AirDate { get; private set; }

        public string Title { get; private set; }

        internal Episode(XElement xml)
        {
            EpisodeNumber = xml.ToInt32("seasonnum");
            DateTime temp;
            string airdate = xml.ToString("airdate");
            bool res = DateTime.TryParse(airdate, out temp);
            if (res)
            {
                AirDate = temp;
            }
            Title = xml.ToString("title");
        }

        public override string ToString()
        {
            return string.Format("Episode #{0}: {1} - {2}", EpisodeNumber, Title, AirDate);
        }

        #region IComparable<Episode> Members

        public int CompareTo(Episode other)
        {
            return EpisodeNumber - other.EpisodeNumber;
        }

        #endregion
    }
}

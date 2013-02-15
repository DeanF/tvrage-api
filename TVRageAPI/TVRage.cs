using System.Collections.Concurrent;
using System.Linq;
using System.Xml.Linq;
using System.Xml;

namespace TVRageAPI
{
    /// <summary>
    /// TVRage API class
    /// </summary>
    public class TVRage
    {
        #region Constants
        private const string SHOW_LOOKUP = @"http://www.tvrage.com/feeds/episode_list.php?sid=";
        private const string SID_LOOKUP = @"http://services.tvrage.com/feeds/search.php?show=";
        #endregion

        /// <summary>
        /// Singleton instance
        /// </summary>
        private static volatile TVRage _Instance;

        /// <summary>
        /// sync object
        /// </summary>
        private static readonly object LockObj = new object();

        /// <summary>
        /// cache of shows
        /// </summary>
        private readonly ConcurrentBag<Show> _Cache = new ConcurrentBag<Show>();

        private TVRage()
        {
        }

        /// <summary>
        /// Gets the instance of the api
        /// </summary>
        public static TVRage Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (LockObj)
                    {
                        if (_Instance == null)
                        {
                            _Instance = new TVRage();
                        }
                    }
                }
                return _Instance;
            }
        }

        /// <summary>
        /// get show id
        /// </summary>
        /// <param name="showName"></param>
        /// <returns></returns>
        private static int GetSid(string showName)
        {
            var document = XDocument.Load(SID_LOOKUP + showName);
            var xml = document.Element("Results");
            if (xml != null)
            {
                return xml.Element("show").ToInt32("showid");
            }
            return -1;
        }

        /// <summary>
        /// find a show
        /// </summary>
        /// <param name="showId"></param>
        /// <param name="checkCache"></param>
        /// <returns></returns>
        private Show FindShow(int showId, bool checkCache = true)
        {
            if (showId <= 0) return null;
            if (checkCache)
            {
                var result = (from show in _Cache.AsParallel()
                              where show.ShowId == showId
                              select show).FirstOrDefault();
                if (result != null)
                {
                    return result;
                }
            }
            try
            {
                var document = XDocument.Load(SHOW_LOOKUP + showId);
                var show = new Show(showId, document.Element("Show"));
                _Cache.Add(show);
                return show;
            }
            catch (XmlException)
            {
            }
            return null;
        }

        /// <summary>
        /// Find a show by name
        /// </summary>
        /// <param name="showName"></param>
        /// <returns></returns>
        public Show FindShow(string showName)
        {
            int sid = GetSid(showName);
            return FindShow(sid);
        }
    }
}

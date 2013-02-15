using System.Xml.Linq;

namespace TVRageAPI
{
    static class Extentions
    {
        public static string ToString(this XElement xml, string elementName)
        {
            var element = xml.Element(elementName);
            return element != null ? element.Value : "";
        }
        public static int ToInt32(this XElement xml, string elementName)
        {
            var element = xml.Element(elementName);
            if (element != null)
            {
                int num;
                bool success = int.TryParse(element.Value, out num);
                if (success)
                {
                    return num;
                }
            }
            return -1;
        }
    }
}

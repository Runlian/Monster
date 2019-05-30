using System.Configuration;
using System.Web;
using System.Web.Configuration;
using System.Xml;

namespace Monster.Common
{
    public class ConfigHelper
    {
        public static string DbConnectionString
        {
            get
            {
                var connection = ConfigurationManager.ConnectionStrings["DbContext"].ConnectionString;
                //connection = DESEncrypt.Decrypt(connection);
                return connection;
            }
        }

        public static string GetValue(string key)
        {
            return ConfigurationManager.AppSettings[key]?.Trim();
        }
        public static void SetValue(string key, string value)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(HttpContext.Current.Server.MapPath("~/system.config"));
            var xNode = xDoc.SelectSingleNode("//appSettings");

            var xElem1 = (XmlElement)xNode.SelectSingleNode("//add[@key='" + key + "']");
            if (xElem1 != null) xElem1.SetAttribute("value", value);
            else
            {
                var xElem2 = xDoc.CreateElement("add");
                xElem2.SetAttribute("key", key);
                xElem2.SetAttribute("value", value);
                xNode.AppendChild(xElem2);
            }
            xDoc.Save(HttpContext.Current.Server.MapPath("~/system.config"));
        }
    }
}
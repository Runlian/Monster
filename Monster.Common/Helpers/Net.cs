using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Web;

namespace Monster.Common
{
    /// <summary>
    /// 网络操作
    /// </summary>
    public class Net
    {
        #region Ip(获取Ip)

        /// <summary>
        /// 获取Ip
        /// </summary>
        public static string Ip
        {
            get
            {
                var result = string.Empty;
                if (HttpContext.Current != null)
                    result = GetWebClientIp();
                if (string.IsNullOrEmpty(result))
                    result = GetLanIp();
                return result;
            }
        }

        /// <summary>
        /// 获取Web客户端的Ip
        /// </summary>
        private static string GetWebClientIp()
        {
            var ip = GetWebRemoteIp();
            foreach (var hostAddress in Dns.GetHostAddresses(ip))
            {
                if (hostAddress.AddressFamily == AddressFamily.InterNetwork)
                    return hostAddress.ToString();
            }

            return string.Empty;
        }

        /// <summary>
        /// 获取Web远程Ip
        /// </summary>
        private static string GetWebRemoteIp()
        {
            return HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ??
                   HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        }

        /// <summary>
        /// 获取局域网IP
        /// </summary>
        private static string GetLanIp()
        {
            foreach (var hostAddress in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (hostAddress.AddressFamily == AddressFamily.InterNetwork)
                    return hostAddress.ToString();
            }

            return string.Empty;
        }

        #endregion

        #region Host(获取主机名)

        /// <summary>
        /// 获取主机名
        /// </summary>
        public static string Host
        {
            get { return HttpContext.Current == null ? Dns.GetHostName() : GetWebClientHostName(); }
        }

        /// <summary>
        /// 获取Web客户端主机名
        /// </summary>
        private static string GetWebClientHostName()
        {
            if (!HttpContext.Current.Request.IsLocal)
                return string.Empty;
            var ip = GetWebRemoteIp();
            var result = Dns.GetHostEntry(IPAddress.Parse(ip)).HostName;
            if (result == "localhost.localdomain")
                result = Dns.GetHostName();
            return result;
        }

        #endregion

        #region 获取mac地址

        /// <summary>
        /// 返回描述本地计算机上的网络接口的对象(网络接口也称为网络适配器)。
        /// </summary>
        /// <returns></returns>
        public static NetworkInterface[] NetCardInfo()
        {
            return NetworkInterface.GetAllNetworkInterfaces();
        }

        ///<summary>
        /// 通过NetworkInterface读取网卡Mac
        ///</summary>
        ///<returns></returns>
        public static List<string> GetMacByNetworkInterface()
        {
            List<string> macs = new List<string>();
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface ni in interfaces)
            {
                macs.Add(ni.GetPhysicalAddress().ToString());
            }

            return macs;
        }

        #endregion

        #region Ip城市(获取Ip城市)

        /// <summary>
        /// 获取IP地址信息
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static Taobao GetTaobaoLocation(string ip)
        {
            try
            {
                string url = "http://ip.taobao.com/service/getIpInfo.php?ip=" + ip;
                var res = HttpHelper.HttpGet(url);
                return res.ToObject<Taobao>();
            }
            catch
            {
                return null;
            }

        }

        /// <summary>
        /// 淘宝接口
        /// </summary>
        public class Taobao
        {
            public int code { get; set; }
            public TaobaoItem data { get; set; }
        }

        public class TaobaoItem
        {
            /// <summary>
            /// 国家
            /// </summary>
            public string country { get; set; }

            /// <summary>
            /// 国家编码
            /// </summary>
            public string country_id { get; set; }

            /// <summary>
            /// 地区
            /// </summary>
            public string area { get; set; }

            /// <summary>
            /// 地区编码
            /// </summary>
            public string area_id { get; set; }

            /// <summary>
            /// 省
            /// </summary>
            public string region { get; set; }

            /// <summary>
            /// 省份编码
            /// </summary>
            public string region_id { get; set; }

            /// <summary>
            /// 市
            /// </summary>
            public string city { get; set; }

            /// <summary>
            /// 市编码
            /// </summary>
            public string city_id { get; set; }

            /// <summary>
            /// 区/县
            /// </summary>
            public string county { get; set; }

            /// <summary>
            /// 区/县编码
            /// </summary>
            public string county_id { get; set; }

            /// <summary>
            /// 营运商
            /// </summary>
            public string isp { get; set; }

            /// <summary>
            /// 营运商编码
            /// </summary>
            public string isp_id { get; set; }

            /// <summary>
            /// IP地址
            /// </summary>
            public string ip { get; set; }

        }

        /// <summary>
        /// 新浪接口
        /// </summary>
        public class Sina
        {
            public string ret { get; set; }
            public string desc { get; set; }
            public string start { get; set; }
            public string isp { get; set; }
            public string province { get; set; }
            public string type { get; set; }
            public string district { get; set; }
            public string end { get; set; }
            public string city { get; set; }
            public string country { get; set; }
        }



        #endregion

        #region Browser(获取浏览器信息)

        /// <summary>
        /// 获取浏览器信息
        /// </summary>
        public static string Browser
        {
            get
            {
                if (HttpContext.Current == null)
                    return string.Empty;
                var browser = HttpContext.Current.Request.Browser;
                return string.Format("{0} {1}", browser.Browser, browser.Version);
            }
        }

        #endregion
    }
}

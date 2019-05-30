﻿using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Monster.Common
{
    public static partial class Extensions
    {
        private static readonly Regex b = new Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
        private static readonly Regex v = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);

        /// <summary>
        /// 判断是否手机访问
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool IsMobile(this HttpContextBase context)
        {
            try
            {
                if (context.Request.Browser.IsMobileDevice)
                {
                    return true;
                }

                if (!string.IsNullOrEmpty(context.Request.UserAgent))
                {
                    var userAgent = context.Request.UserAgent.ToLower();
                    if (b.IsMatch(userAgent) || v.IsMatch(userAgent.Substring(0, 4)))
                    {
                        return true;
                    }

                    return false;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }



        public static bool IsSearchEngine(this HttpContextBase context, string url = "")
        {
            if (string.IsNullOrEmpty(url))
            {
                url = context.Request.UrlReferrer == null ? "" : context.Request.UrlReferrer.ToString();
            }
            string[] engineArray = { "Google", "Baidu", "yisou", "MSN", "Yahoo", "live", "tom", "163", "TMCrawler", "iask", "Sogou", "soso", "youdao", "zhongsou", "so", "openfind", "alltheweb", "lycos", "bing", "118114" };
            url = url.ToLower();
            return engineArray.Any(t => url.IndexOf(t.ToLower(), StringComparison.Ordinal) >= 0);
        }

        public static string GetSearchEngine(this HttpContextBase context, string url = "")
        {
            if (string.IsNullOrEmpty(url))
            {
                url = context.Request.UrlReferrer == null ? "" : context.Request.UrlReferrer.ToString();
            }
            var engine = new SearchEngineProvider(url);
            return engine.GetEngineName();
        }

        public static string GetSearchKeyword(this HttpContextBase context, string url = "")
        {
            if (string.IsNullOrEmpty(url))
            {
                url = context.Request.UrlReferrer == null ? "" : context.Request.UrlReferrer.ToString();
            }
            var engine = new SearchEngineProvider(url);
            return engine.GetSearchKeyword();
        }

    }

    public class SearchEngineProvider
    {
        private readonly string[][] _enginers = {
            new[]{"google","utf8","q"},
            new[]{"baidu", "gb2312", "wd"},
            new[]{"yahoo","utf8","p"},
            new[]{"yisou","utf8","search"},
            new[]{"live","utf8","q"},
            new[]{"tom","gb2312","word"},
            new[]{"163","gb2312","q"},
            new[]{"iask","gb2312","k"},
            new[]{"soso","gb2312","w"},
            new[]{"sogou","gb2312","query"},
            new[]{"zhongsou","gb2312","w"},
            new[]{"so", "utf8", "q"},
            new[]{"openfind","utf8","q"},
            new[]{"alltheweb","utf8","q"},
            new[]{"lycos","utf8","query"},
            new[]{"onseek","utf8","q"},
            new[]{"youdao","utf8","q"},
            new[]{"bing","utf8","q"},
            new[]{"118114","gb2312","kw"}
        };

        private string _engineName = "";
        private string _coding = "utf8";
        private string _regexWord = "";
        private string _regex = @"(";
        private readonly string _url;

        public SearchEngineProvider(string url)
        {
            _url = url;
        }

        public void EngineRegex()
        {
            for (int i = 0, j = _enginers.Length; i < j; i++)
            {
                if (_url.Contains(_enginers[i][0]))
                {
                    _engineName = _enginers[i][0];
                    _coding = _enginers[i][1];
                    _regexWord = _enginers[i][2];
                    _regex += _engineName + @".+.*[?/ &]" + _regexWord + @"[=:])(?<key>[^&]*)";
                    break;
                }
            }
        }

        public string GetEngineName()
        {
            EngineRegex();
            return _engineName;
        }

        public string GetSearchKeyword()
        {
            var result = "";
            EngineRegex();
            if (!string.IsNullOrEmpty(_engineName))
            {
                var regex = new Regex(_regex, RegexOptions.IgnoreCase);
                var matche = regex.Match(_url);
                result = matche.Groups["key"].Value;
                result = result.Replace("+", " ");
                result = _coding == "gb2312" ? HttpUtility.UrlDecode(result) : Uri.UnescapeDataString(result);
            }

            return result;
        }


    }
}
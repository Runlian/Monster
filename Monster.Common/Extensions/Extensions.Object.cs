using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Monster.Common
{
    public static partial class Extensions
    {
        public static string ToJson(this object obj)
        {
            var time = new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
            return JsonConvert.SerializeObject(obj, time);
        }

        public static string ToJson(this object obj, JsonSerializerSettings settings)
        {
            return JsonConvert.SerializeObject(obj, settings);
        }

        public static string UnicodeTo(this string input)
        {
            var regex = new Regex(@"(?i)\\u([0-9a-f]{4})");
            var result = regex.Replace(input, match => ((char)Convert.ToInt32(match.Groups[1].Value, 16)).ToString());
            return result;
        }

        public static T ToObject<T>(this string data)
        {
            if (string.IsNullOrEmpty(data))
                return default(T);
            var settings = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            return JsonConvert.DeserializeObject<T>(data, settings);
        }
    }
}

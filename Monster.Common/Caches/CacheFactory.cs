using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Monster.Common
{
    public class CacheFactory
    {
        /// <summary>
        /// 定义通用的Repository
        /// </summary>
        /// <returns></returns>
        public static ICache Cache()
        {
            //修改为支持Redis
            string cacheType = ConfigHelper.GetValue("CacheType");
            switch (cacheType)
            {
                case "Redis":
                    return new Redis.Cache();
                    break;
                case "WebCache":
                    return new Cache();
                    break;
                default:
                    return new Cache();
                    break;
            }
        }
    }
}
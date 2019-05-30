using System.Web.Mvc;
using Monster.Common;
using Monster.Data;
using Newtonsoft.Json;
using SqlSugar;

namespace Monster.Web
{
    public class BaseController : Controller
    {
        public SqlSugarClient Context;
        public BaseController()
        {
            Context = DbConfig.GetDbInstance();
        }

        public virtual ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 请求成功统一返回
        /// </summary>
        /// <returns></returns>
        protected virtual ActionResult Success()
        {
            return Content(new ReturnData<string>(CustomStatusCode.Success, "请求成功").ToJson());
        }

        /// <summary>
        /// 请求成功统一返回
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        protected virtual ActionResult Success(string msg)
        {
            return Content(new ReturnData<string>(CustomStatusCode.Success, msg).ToJson());
        }

        /// <summary>
        /// 请求成功统一返回
        /// </summary>
        /// <returns></returns>
        protected virtual ActionResult Success<T>(T data, int count)
        {
            return Content(new ReturnData<T>(CustomStatusCode.Success, "请求成功", count, data).ToJson());
        }
        /// <summary>
        /// 请求成功统一返回
        /// </summary>
        /// <returns></returns>
        protected virtual ActionResult Success<T>(string msg, T data, int count)
        {
            return Content(new ReturnData<T>(CustomStatusCode.Success, msg, count, data).ToJson());
        }
        /// <summary>
        /// 请求成功统一返回
        /// </summary>
        /// <returns></returns>
        protected virtual ActionResult Success<T>(T data, int count, JsonSerializerSettings settings)
        {
            return Content(new ReturnData<T>(CustomStatusCode.Success, "请求成功", count, data).ToJson(settings));
        }

        /// <summary>
        /// 请求失败统一返回
        /// </summary>
        /// <returns></returns>
        protected virtual ActionResult Error(string msg)
        {
            return Content(new ReturnData<string>(CustomStatusCode.Error, msg).ToJson());
        }

    }
}
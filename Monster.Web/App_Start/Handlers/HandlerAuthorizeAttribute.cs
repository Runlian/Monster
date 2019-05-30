using System.Web;
using System.Web.Mvc;
using Monster.Common;

namespace Monster.Web
{
    public class HandlerAuthorizeAttribute : ActionFilterAttribute
    {
        public bool Verify { get; set; }
        public HandlerAuthorizeAttribute(bool verify = true)
        {
            Verify = verify;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var area = (filterContext.RouteData.DataTokens["area"] ?? "").ToString();
            if (OperatorProvider.Provider.GetCurrent() == null)
            {
                if (string.IsNullOrEmpty(area))
                {
                    filterContext.HttpContext.Response.Write("<script>top.location.href = '/Home/Index';</script>");
                }
                else
                {
                    filterContext.HttpContext.Response.Write("<script>top.location.href = '/" + area + "/Login/Index';</script>");
                }
                return;
            }
            if (OperatorProvider.Provider.GetCurrent().IsAdmin)
            {
                return;
            }
            if (Verify == false)
            {
                return;
            }

            if (!Authorize(filterContext))
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    if (string.IsNullOrEmpty(area))
                    {
                        filterContext.HttpContext.Response.Write("<script>top.location.href = '/Home/Index';</script>");
                    }
                    else
                    {
                        filterContext.HttpContext.Response.Write("<script>top.location.href = '/" + area + "/Login/Index';</script>");
                    }
                    return;
                }
                filterContext.Result = new ContentResult() { Content = new ReturnData<string>(CustomStatusCode.TimeOut, "您没有访问权限").ToJson() };
            }
        }

        private bool Authorize(ActionExecutingContext context)
        {
            var action = HttpContext.Current.Request.ServerVariables["SCRIPT_NAME"];
            var provider = OperatorProvider.Provider.GetCurrent();
            foreach (var module in provider.Modules)
            {
                if (!string.IsNullOrEmpty(module.Url))
                {
                    string[] url = module.Url.Split('?');
                    if (url[0] == action) return true;
                }
            }

            return false;
        }
    }
}
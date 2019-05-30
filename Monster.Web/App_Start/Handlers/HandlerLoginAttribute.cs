using System;
using System.Security.Policy;
using System.Web.Mvc;

namespace Monster.Web
{
    public class HandlerLoginAttribute : AuthorizeAttribute
    {
        public bool Ignore { get; set; }
        public HandlerLoginAttribute(bool ignore = true)
        {
            Ignore = ignore;
        }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (Ignore == false)
            {
                return;
            }
            if (OperatorProvider.Provider.GetCurrent() == null)
            {
                 var area = (filterContext.RouteData.DataTokens["area"] ?? "").ToString();
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
        }
    }
}
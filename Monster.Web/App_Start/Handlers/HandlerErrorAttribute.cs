using System.Web.Mvc;
using Monster.Common;

namespace Monster.Web
{
    public class HandlerErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            base.OnException(context);
            if (context.HttpContext.Request.IsAjaxRequest())
            {
                context.ExceptionHandled = true;
                context.HttpContext.Response.StatusCode = 200;

                CustomStatusCode code = CustomStatusCode.Error;
                if (OperatorProvider.Provider.GetCurrent() == null)
                    code = CustomStatusCode.TimeOut;

                context.Result = new ContentResult { Content = new ReturnData<string>(code, context.Exception.Message).ToJson() };
            }
        }
    }
}
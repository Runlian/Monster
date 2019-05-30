using System.Web.Mvc;

namespace Monster.Web.Areas.Backadmin
{
    public class BackadminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Backadmin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Backadmin_default",
                "Backadmin/{controller}/{action}/{id}",
                new { controller = "Login", action = "Index", id = UrlParameter.Optional },
                new[] { "" }
            );
        }
    }
}
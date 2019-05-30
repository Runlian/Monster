﻿using System.Web.Mvc;
using System.Web.Routing;
using Monster.Data;

namespace Monster.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            DbInit.Init();
        }
    }
}

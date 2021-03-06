﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace TresEmpanadas
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            // Session["IdUsuario"] = string.Empty ;
        }

        protected void Application_Error(object sender, EventArgs e)
            {
            Exception exception = Server.GetLastError();
            if (exception != null)
            {
                //Log
                if (HttpContext.Current.Server != null)
                {
                    //HttpContext.Current.Server.Transfer("/siteerror.aspx");
                    // clear error on server
                    //Server.ClearError();
                    //Response.Redirect("~/Error/Index");
                }
            }
        }
    }
}

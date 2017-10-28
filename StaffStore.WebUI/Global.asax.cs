using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using StaffStore.Domain.Entities;
using StaffStore.WebUI.Infrastructure.Binders;
using System.Web.Optimization;
using StaffStore.WebUI.App_Start;

namespace StaffStore.WebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            ModelBinders.Binders.Add(typeof(Cart), new CartModelBinder());
            BundleConfig.RegisterBundles(BundleTable.Bundles);

        }
    }
}

using System.Web.Mvc;
using System.Web.Routing;
using RouteMagic;
using RouteMagicDemo.Web.HttpHandlers;

namespace RouteMagicDemo.Web {
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Map HTTP Handlers
            routes.MapHttpHandler<HelloWorldHttpHandler>("hello-world", "handlers/helloworld");
            routes.MapHttpHandler("hello-world2", new HelloWorldHttpHandler(), "handlers/helloworld2");

            RouteCollection someRoutes = new RouteCollection();
            someRoutes.MapHttpHandler<HelloWorldHttpHandler>("hello-world3", "handlers/helloworld3");
            someRoutes.MapHttpHandler("hello-world4", new HelloWorldHttpHandler(), "handlers/helloworld4");
            var groupRoute = new GroupRoute("~/section", someRoutes);
            routes.Add("group", groupRoute);

            var mvcRoutes = new RouteCollection();
            mvcRoutes.Map("foo1", "foo/{controller}", new { action = "index" });
            mvcRoutes.Map("foo2", "foo2/{controller}", new { action = "index" });
            routes.Add("group2", new GroupRoute("~/group2sec", mvcRoutes));

            var defaultRoute = routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            ).SetRouteName("Default");
        }

        protected void Application_Start() {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}
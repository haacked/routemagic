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

            // Redirect From Old Route to New route
            var targetRoute = routes.Map("target", "yo/{id}/{action}", new { controller = "Home" });
            routes.Redirect(r => r.MapRoute("legacy", "foo/{id}/baz/{action}")).To(targetRoute, new { id = "123", action = "index" });
            routes.Redirect(r => r.MapRoute("legacy2", "foo/baz")).To(targetRoute, new { id = "123", action = "index" });

            // Map Delegate
            routes.MapDelegate("map-delegate", "this-is-a-test", rc => rc.HttpContext.Response.Write("Yeah, it's a test"));

            // Map HTTP Handlers
            routes.MapHttpHandler<HelloWorldHttpHandler>("hello-world", "handlers/helloworld");
            routes.MapHttpHandler("hello-world2", "handlers/helloworld2", new HelloWorldHttpHandler());

            RouteCollection someRoutes = new RouteCollection();
            someRoutes.MapHttpHandler<HelloWorldHttpHandler>("hello-world3", "handlers/helloworld3");
            someRoutes.MapHttpHandler("hello-world4", "handlers/helloworld4", new HelloWorldHttpHandler());
            var groupRoute = new GroupRoute("~/section", someRoutes);
            routes.Add("group", groupRoute);

            var mvcRoutes = new RouteCollection();
            mvcRoutes.Map("foo1", "foo/{controller}", new { action = "index" });
            mvcRoutes.Map("foo2", "foo2/{controller}", new { action = "index" });
            routes.Add("group2", new GroupRoute("~/group2sec", mvcRoutes));

            var defaultRoute = routes.Map(
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
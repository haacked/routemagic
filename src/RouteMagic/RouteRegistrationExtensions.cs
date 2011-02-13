using System;
using System.Web.Compilation;
using System.Web.Routing;
using RouteMagic.Internals;

namespace RouteMagic {
    public static class RouteRegistrationExtensions {
        // This method requires full trust.
        // See http://haacked.com/archive/2010/01/17/editable-routes.aspx for more details.
        public static void RegisterRoutes(this RouteCollection routes,
            string virtualPath) {
            if (String.IsNullOrEmpty(virtualPath)) {
                throw new ArgumentNullException("virtualPath");
            }

            ConfigFileChangeNotifier.Listen(virtualPath,
              vp => routes.ReloadRoutes(vp));
        }

        // MEDIUM TRUST: Use this method. Place Routes.cs in the ~/App_Code directory.
        // See http://haacked.com/archive/2010/01/18/editable-routes-in-medium-trust.aspx for more info.
        public static void RegisterAppCodeRoutes(this RouteCollection routes) {
            var type = BuildManager.GetType("Routes", false/*throwOnError*/);
            if (type == null) {
                throw new InvalidOperationException("Could not find a type Routes in the App_Code directory");
            }

            var registrar = Activator.CreateInstance(type) as IRouteRegistrar;
            if (registrar == null) {
                throw new InvalidOperationException("Could not find an instance of IRouteRegistrar");
            }

            registrar.RegisterRoutes(RouteTable.Routes);
        }

        static void ReloadRoutes(this RouteCollection routes, string virtualPath) {
            var assembly = BuildManager.GetCompiledAssembly(virtualPath);
            var registrar = assembly.CreateInstance("Routes") as IRouteRegistrar;
            using (routes.GetWriteLock()) {
                routes.Clear();
                registrar.RegisterRoutes(routes);
            }
        }
    }
}

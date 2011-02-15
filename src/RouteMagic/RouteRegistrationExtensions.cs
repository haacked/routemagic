using System;
using System.Web.Compilation;
using System.Web.Routing;
using RouteMagic.Internals;

namespace RouteMagic {
    public static class RouteRegistrationExtensions {
        public static void RegisterRoutes(this RouteCollection routes,
            string virtualPath) {
            if (String.IsNullOrEmpty(virtualPath)) {
                throw new ArgumentNullException("virtualPath");
            }
            routes.ReloadRoutes(virtualPath);
            ConfigFileChangeNotifier.Listen(virtualPath, vp => routes.ReloadRoutes(vp));
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

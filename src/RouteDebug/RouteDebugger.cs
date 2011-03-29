using System.Web.Routing;

namespace RouteDebug
{
    public static class RouteDebugger
    {
        public static void RewriteRoutesForTesting(RouteCollection routes)
        {
            using (routes.GetReadLock())
            {
                bool foundDebugRoute = false;
                foreach (RouteBase routeBase in routes)
                {
                    Route route = routeBase as Route;
                    if (route != null)
                    {
                        route.RouteHandler = new DebugRouteHandler();
                    }

                    if (route == DebugRoute.Singleton)
                        foundDebugRoute = true;

                }
                if (!foundDebugRoute)
                {
                    routes.Add(DebugRoute.Singleton);
                }
            }

            
        }

    }
}

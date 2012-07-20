using System.Web.Routing;

namespace RouteDebug
{
    public class DebugRoute : Route
    {
        static readonly DebugRoute singleton = new DebugRoute();

        public static DebugRoute Singleton
        {
            get { return singleton; }
        }

        private DebugRoute()
            : base("{*catchall}", new DebugRouteHandler()) { }
    }
}

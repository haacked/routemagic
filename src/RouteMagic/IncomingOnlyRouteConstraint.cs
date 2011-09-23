using System.Web;
using System.Web.Routing;

namespace RouteMagic {
    /// <summary>
    /// Simple constraint that only matches incoming requests.
    /// </summary>
    public class IncomingOnlyRouteConstraint : IRouteConstraint {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection) {
            return routeDirection == RouteDirection.IncomingRequest;
        }
    }
}

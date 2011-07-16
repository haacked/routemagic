using System;
using System.Web;
using System.Web.Routing;

namespace RouteMagic.Internals {
    public class NormalizeRoute : RouteBase {
        public NormalizeRoute(RouteBase route)
            : this(route, requireLowerCase: true, appendTrailingSlash: false) {
        }

        public NormalizeRoute(RouteBase route, bool requireLowerCase, bool appendTrailingSlash) {
            if (route == null) {
                throw new ArgumentNullException("route");
            }
            OriginalRoute = route;
            AppendTrailingSlash = appendTrailingSlash;
            RequireLowerCase = requireLowerCase;
        }

        public RouteBase OriginalRoute {
            get;
            private set;
        }

        public bool AppendTrailingSlash {
            get;
            private set;
        }

        public bool RequireLowerCase {
            get;
            private set;
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values) {
            var vpd = OriginalRoute.GetVirtualPath(requestContext, values);
            if (vpd != null) {
                var virtualPath = vpd.VirtualPath;
                if (RequireLowerCase) {
                    virtualPath = virtualPath.ToLowerInvariant();
                }
                if (AppendTrailingSlash) {
                    var queryIndex = virtualPath.IndexOf('?');
                    string queryPart = string.Empty;
                    if (queryIndex > -1) {
                        queryPart = virtualPath.Substring(queryIndex);
                        virtualPath = virtualPath.Substring(0, queryIndex);
                    }
                    if (!virtualPath.EndsWith("/")) {
                        virtualPath = virtualPath + "/";
                    }
                    virtualPath += queryPart;
                }
                vpd.VirtualPath = virtualPath;
            }
            return vpd;
        }

        public override RouteData GetRouteData(HttpContextBase httpContext) {
            return OriginalRoute.GetRouteData(httpContext);
        }
    }
}
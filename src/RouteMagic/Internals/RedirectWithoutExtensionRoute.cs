using System;
using System.IO;
using System.Web;
using System.Web.Routing;
using RouteMagic.HttpHandlers;

namespace RouteMagic.Internals {
    public class RedirectWithoutExtensionRoute : RouteBase {
        public RedirectWithoutExtensionRoute(string extensionToRemove, bool permanent) {
            ExtensionToRemove = extensionToRemove;
            Permanent = permanent;
        }

        public string ExtensionToRemove {
            get;
            private set;
        }

        public bool Permanent {
            get;
            private set;
        }

        public override RouteData GetRouteData(HttpContextBase httpContext) {
            string path = httpContext.Request.AppRelativeCurrentExecutionFilePath;
            string extension = Path.GetExtension(path);
            if (ExtensionToRemove == ".*" || extension.Equals(ExtensionToRemove, StringComparison.OrdinalIgnoreCase)) {
                string virtualPath = path.Substring(0, path.Length - extension.Length);
                return new RouteData(this, new RedirectHttpHandler(virtualPath, true, false));
            }
            return null;
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values) {
            // Never matches for URL generation
            return null;
        }
    }
}

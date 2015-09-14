xxxxusing System;
using System.Web;
using System.Web.Routing;
using RouteMagic.Internals;

namespace RouteMagic
{
    public class GroupRoute : RouteBase
    {
        const string ApplicationRootPath = "~/";
        readonly string _path;
        readonly IVirtualPathResolver _virtualPathResolver;

        public GroupRoute(RouteCollection childRoutes)
            : this(ApplicationRootPath, childRoutes, null)
        {
        }

        public GroupRoute(RouteCollection childRoutes, IVirtualPathResolver virtualPathResolver)
            : this(ApplicationRootPath, childRoutes, virtualPathResolver)
        {
        }

        public GroupRoute(string directoryPath, RouteCollection childRoutes)
            : this(directoryPath, childRoutes, null)
        {
        }

        public GroupRoute(string directoryPath, RouteCollection childRoutes, IVirtualPathResolver virtualPathResolver)
        {
            if (!directoryPath.StartsWith("~/"))
            {
                throw new ArgumentException("Directory Path must start with '~/'", "directoryPath");
            }
            _virtualPathResolver = virtualPathResolver ?? VirtualPathResolver.Instance;
            VirtualPath = directoryPath;
            _path = _virtualPathResolver.ToAbsolute(VirtualPath);
            ChildRoutes = childRoutes;

        }

        public string VirtualPath
        {
            get;
            private set;
        }

        public RouteCollection ChildRoutes
        {
            get;
            private set;
        }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            if (!httpContext.Request.AppRelativeCurrentExecutionFilePath.StartsWith(VirtualPath, StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }
            // some trickery here to strip off DirectoryPath from the Request URL in httpContext if needed
            HttpContextBase childHttpContext = VirtualPath != ApplicationRootPath ? new ChildHttpContextWrapper(httpContext, VirtualPath, _path) : null;

            return ChildRoutes.GetRouteData(childHttpContext ?? httpContext);
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            string routeName = values.GetRouteName();

            var virtualPath = ChildRoutes.GetVirtualPath(requestContext, routeName, values.WithoutRouteName());
            if (virtualPath != null)
            {
                string rewrittenVirtualPath = virtualPath.VirtualPath.WithoutApplicationPath(requestContext);
                string directoryPath = VirtualPath.WithoutTildePrefix(); // remove tilde
                rewrittenVirtualPath = rewrittenVirtualPath.Insert(0, directoryPath.WithoutTrailingSlash());

                virtualPath.VirtualPath = rewrittenVirtualPath.Remove(0, 1);
            }

            return virtualPath;
        }
    }
}
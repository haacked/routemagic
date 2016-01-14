using System;
using System.Web;
using System.Web.Routing;
using RouteBasics;

namespace RouteMagic.Internals
{
	public class NormalizeRoute : RouteWrapper
	{
		public readonly bool AppendTrailingSlash;
		public readonly bool RequireLowerCase;

		public NormalizeRoute(RouteBase route)
			: this(route, requireLowerCase: true, appendTrailingSlash: false)
		{
		}

		public NormalizeRoute(RouteBase route, bool requireLowerCase, bool appendTrailingSlash)
			: base(route)
		{
			if (route == null) {
				throw new ArgumentNullException("route");
			}
			AppendTrailingSlash = appendTrailingSlash;
			RequireLowerCase = requireLowerCase;
		}

		public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
		{
			var internalRoute = MyRoute;
			if (internalRoute != null && !internalRoute.Url.Contains("{")) {
				return null;
			}

			var vpd = base.GetVirtualPath(requestContext, values);

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
	}
}
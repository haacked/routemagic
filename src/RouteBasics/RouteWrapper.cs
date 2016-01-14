using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;

namespace RouteBasics
{
	public class RouteWrapper : RouteBase, IDebugRoute
	{
		private readonly RouteBase __DebugRoute;
		public RouteWrapper(RouteBase route)
		{
			__DebugRoute = route;
		}

		protected Route MyRoute
		{
			get
			{
				return __DebugRoute as Route;
			}
		}

		public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
		{
			return __DebugRoute == null ? null : __DebugRoute.GetVirtualPath(requestContext, values);
		}

		#region IRoute Members

		public IRouteHandler RouteHandler
		{
			get
			{
				return MyRoute == null ? null : MyRoute.RouteHandler;
			}
		}

		public string Url
		{
			get
			{
				return MyRoute == null ? null : MyRoute.Url;
			}
		}

		public RouteValueDictionary Constraints
		{
			get
			{
				return MyRoute == null ? null : MyRoute.Constraints;
			}
		}

		public RouteValueDictionary DataTokens
		{
			get
			{
				return MyRoute == null ? null : MyRoute.DataTokens;
			}
			set
			{
				if (MyRoute != null) {
					MyRoute.DataTokens = value;
				}
			}
		}

		public RouteValueDictionary Defaults
		{
			get
			{
				return MyRoute == null ? null : MyRoute.Defaults;
			}
		}
		public string RouteName
		{
			get
			{
				return MyRoute == null ? null : MyRoute.GetRouteName();
			}
			set
			{
				if (MyRoute != null) {
					MyRoute.SetRouteName(value);
				}
			}
		}


		public override RouteData GetRouteData(HttpContextBase httpContextBase)
		{
			return __DebugRoute == null ? null : __DebugRoute.GetRouteData(httpContextBase);
		}

		#endregion
	}
}

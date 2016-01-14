using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Web.Routing;

namespace RouteBasics
{
	//	http://www.khalidabuhakmeh.com/get-all-route-names-from-a-routecollection-webapi
	public static class RouteExtensions
	{
		public class RouteNameInfo
		{
			public readonly RouteBase Route;
			public readonly string Name;
			public RouteNameInfo(RouteBase InRoute, string InName = null)
			{
				Route = InRoute;
				Name = InName;
			}
		}
		/// <summary>
		/// Returns the RouteNameInfos in the correct order
		/// </summary>
		/// <param name="routeCollection"></param>
		/// <returns></returns>
		public static IEnumerable<RouteNameInfo> getRouteMap(this RouteCollection routeCollection)
		{
			IList<RouteNameInfo> _Result = new List<RouteNameInfo>();
			IDictionary<string, RouteBase> _RouteNames = routeCollection.getNamedMap();
			foreach (RouteBase _Route in routeCollection) {
				_Result.Add(new RouteNameInfo(_Route, findRouteName(_Route, _RouteNames)));
			}
			return _Result;
		}
		public static string findRouteName(RouteBase _Route, IDictionary<string, RouteBase> _RouteNames, bool remove = true)
		{
			string _Result = "";
			foreach (KeyValuePair<string, RouteBase> _RoutePair in _RouteNames) {
				if (_RoutePair.Value == _Route) {
					_Result = _RoutePair.Key;
					_RouteNames.Remove(_RoutePair);
					break;
				}
			}
			return _Result;
		}
		public static IDictionary<string, RouteBase> getNamedMap(this RouteCollection routeCollection)
		{
			object routes = routeCollection;
			var fieldName = "_dictionary";

			if (routes == null)
				return null;

			var type = routes.GetType();

			if (type.FullName == "System.Web.Http.WebHost.Routing.HostedHttpRouteCollection") {
				var hostedField = type.GetField("_routeCollection", BindingFlags.NonPublic | BindingFlags.Instance);
				type = hostedField.FieldType;
				routes = hostedField.GetValue(routes);
			}
			fieldName = "_namedMap";

			var field = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);

			return field.GetValue(routes) as IDictionary<string, RouteBase>;
		}

		public static string GetRouteName(this Route route)
		{
			if (route == null) {
				return null;
			}
			if (route.DataTokens.GetRouteName() == null) {
				BuildRouteNames();
			}
			return route.DataTokens.GetRouteName();
		}

		public static string GetRouteName(this RouteData routeData)
		{
			if (routeData == null) {
				return null;
			}
			return routeData.DataTokens.GetRouteName();
		}


		public static Route SetRouteName(this Route route, string routeName)
		{
			if (route == null) {
				throw new ArgumentNullException("route");
			}
			if (route.DataTokens == null) {
				route.DataTokens = new RouteValueDictionary();
			}
			route.DataTokens.SetRouteName(routeName);
			return route;
		}

		public static void BuildRouteNames()
		{
				IDictionary<string, RouteBase> _RouteNames = RouteTable.Routes.getNamedMap();
				foreach (RouteBase routeBase in RouteTable.Routes) {
					routeBase.CastRoute().RouteName = findRouteName(routeBase, _RouteNames);
				}
		}
		public static IDebugRoute CastRoute(this RouteBase routeBase)
		{
			if (routeBase is IDebugRoute) {
				return routeBase as IDebugRoute;
			}
			return new RouteWrapper(routeBase);
		}
	}
}

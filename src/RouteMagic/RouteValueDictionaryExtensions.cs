using System.Web.Routing;

namespace RouteMagic {
    public static class RouteValueDictionaryExtensions {
        private const string RouteNameKey = "__RouteName";

        public static RouteValueDictionary SetRouteName(this RouteValueDictionary routeValues, string routeName) {
            if (routeValues == null) {
                return null;
            }
            routeValues[RouteNameKey] = routeName;
            return routeValues;
        }

        public static RouteValueDictionary WithoutRouteName(this RouteValueDictionary routeValues) {
            routeValues = new RouteValueDictionary(routeValues);
            routeValues.Remove(RouteNameKey);
            return routeValues;
        }

        public static string GetRouteName(this RouteValueDictionary routeValues) {
            if (routeValues == null) {
                return null;
            }
            object routeName = null;
            routeValues.TryGetValue(RouteNameKey, out routeName);
            return routeName as string;
        }

        public static RouteValueDictionary Merge(this RouteValueDictionary routeValues, RouteValueDictionary targetRouteValues) {
            if (routeValues == null) {
                return new RouteValueDictionary(targetRouteValues);
            }

            var mergedRouteValues = new RouteValueDictionary(routeValues);
            // Target values take precedence.
            foreach (var key in targetRouteValues.Keys) {
                mergedRouteValues[key] = targetRouteValues[key];
            }
            return mergedRouteValues;
        }

        public static RouteValueDictionary ConvertToRouteValueDictionary(object routeValues) {
            if (routeValues == null) {
                return null;
            }
            var routeValueDictionary = routeValues as RouteValueDictionary;
            if (routeValueDictionary != null) {
                return routeValueDictionary;
            }

            return new RouteValueDictionary(routeValues);
        }
    }
}

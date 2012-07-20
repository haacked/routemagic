using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using RouteMagic.Internals;

namespace RouteMagic
{
    public static class MvcRouteExtenstions
    {
        // The Map methods map to the MvcRouteHandler
        public static Route Map(this RouteCollection routes, string name, string url)
        {
            return routes.Map(name, url, null, null, null);
        }

        public static Route Map(this RouteCollection routes, string name, string url, object defaults)
        {
            return routes.Map(name, url, defaults, null, null);
        }

        public static Route Map(this RouteCollection routes, string name, string url, object defaults, object constraints)
        {
            return routes.Map(name, url, defaults, constraints, null);
        }

        public static Route Map(this RouteCollection routes, string name, string url, object defaults, object constraints, string[] namespaces)
        {
            var route = routes.MapRoute(name, url, defaults, constraints, namespaces);
            route.SetRouteName(name);
            var normalized = new NormalizeRoute(route);
            routes.Remove(route);
            routes.Add(name, normalized);

            return route;
        }

        public static IHtmlString HandlerLink(this HtmlHelper htmlHelper, string linkText, string routeName)
        {
            return htmlHelper.HandlerLink(linkText, routeName, routeValues: null, htmlAttributes: null);
        }

        public static IHtmlString HandlerLink(this HtmlHelper htmlHelper, string linkText, string routeName, object routeValues, object htmlAttributes)
        {
            var routeValueDictionary = routeValues as RouteValueDictionary ?? new RouteValueDictionary(routeValues);
            IDictionary<string, object> attributes = null;
            if (htmlAttributes != null)
            {
                attributes = htmlAttributes as IDictionary<string, object> ?? new RouteValueDictionary(htmlAttributes);
            }
            return htmlHelper.RouteLink(linkText, routeName, routeValueDictionary, attributes);
        }

        public static string HandlerUrl(this UrlHelper urlHelper, string name)
        {
            return urlHelper.HandlerUrl(name, routeValues: null);
        }

        public static string HandlerUrl(this UrlHelper urlHelper, string routeName, object routeValues)
        {
            var routeValueDictionary = new RouteValueDictionary(routeValues);
            routeValueDictionary.SetRouteName(routeName);
            return urlHelper.RouteUrl(routeName, routeValues);
        }
    }
}

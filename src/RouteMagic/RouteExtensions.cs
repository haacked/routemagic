using System;
using System.Web;
using System.Web.Routing;
using RouteMagic.HttpHandlers;
using RouteMagic.Internals;
using RouteMagic.RouteHandlers;

namespace RouteMagic
{
    public static class RouteExtensions
    {
        public static Route MapHttpHandler<THandler>(this RouteCollection routes, string name, string url) where THandler : IHttpHandler, new()
        {
            return routes.MapHttpHandler(name, url, null, null, r => new THandler());
        }

        public static Route MapHttpHandler<THandler>(this RouteCollection routes, string name, string url, Func<RequestContext, THandler> handlerFactory) where THandler : IHttpHandler
        {
            return routes.MapHttpHandler(name, url, null, null, handlerFactory);
        }

        public static Route MapHttpHandler<THandler>(this RouteCollection routes, string name, string url, object defaults, object constraints, Func<RequestContext, THandler> handlerFactory) where THandler : IHttpHandler
        {
            var route = new Route(url, new HttpHandlerRouteHandler<THandler>(handlerFactory))
                        {
                            Defaults = new RouteValueDictionary(defaults),
                            Constraints = new RouteValueDictionary(constraints)
                        };
            routes.Add(name, new NormalizeRoute(route));
            route.SetRouteName(name);
            return route;
        }

        public static Route MapHttpHandler(this RouteCollection routes, string name, string url, IHttpHandler httpHandler)
        {
            return routes.MapHttpHandler(httpHandler, name, url, defaults: null, constraints: null);
        }

        public static Route MapHttpHandler(this RouteCollection routes, IHttpHandler httpHandler, string name, string url, object defaults, object constraints)
        {
            var route = new Route(url, new HttpHandlerRouteHandler(httpHandler))
                        {
                            Defaults = new RouteValueDictionary(defaults),
                            Constraints = new RouteValueDictionary(constraints)
                        };
            routes.Add(name, new NormalizeRoute(route));
            route.SetRouteName(name);
            return route;
        }

        public static Route Map(this RouteCollection routes, string name, string url, IRouteHandler routeHandler)
        {
            var route = new Route(url, routeHandler);
            routes.Add(name, new NormalizeRoute(route));
            route.SetRouteName(name);
            return route;
        }

        public static Route MapDelegate(this RouteCollection routes, string name, string url, Action<RequestContext> handler)
        {
            return routes.MapHttpHandler(name, url, null, null, requestContext => new DelegateHttpHandler(handler, requestContext.RouteData, false));
        }

        public static Route MapDelegate(this RouteCollection routes, string name, string url, object constraints, Action<RequestContext> handler)
        {
            return routes.MapHttpHandler(name, url, null, constraints, requestContext => new DelegateHttpHandler(handler, requestContext.RouteData, false));
        }

        public static string GetRouteName(this Route route)
        {
            if (route == null)
            {
                return null;
            }
            return route.DataTokens.GetRouteName();
        }

        public static string GetRouteName(this RouteData routeData)
        {
            if (routeData == null)
            {
                return null;
            }
            return routeData.DataTokens.GetRouteName();
        }


        public static Route SetRouteName(this Route route, string routeName)
        {
            if (route == null)
            {
                throw new ArgumentNullException("route");
            }
            if (route.DataTokens == null)
            {
                route.DataTokens = new RouteValueDictionary();
            }
            route.DataTokens.SetRouteName(routeName);
            return route;
        }
    }
}

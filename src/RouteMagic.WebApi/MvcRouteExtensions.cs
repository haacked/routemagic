using System.Web.Routing;
using RouteMagic.WebApi;

namespace RouteMagic
{
    public static class HttpRouteExtenstions
    {
        public static Route MapHttp(this RouteCollection routes, string name, string domain, string url)
        {
            return routes.MapHttp(name, domain, url, null);
        }

        public static Route MapHttp(this RouteCollection routes, string name, string domain, string url, object defaults)
        {
            var route = new HttpDomainRoute(domain, url, defaults);
            return route;
        }
    }
}

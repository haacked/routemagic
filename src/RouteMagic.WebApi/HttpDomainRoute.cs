using System.Web.Http.WebHost;
using System.Web.Routing;

namespace RouteMagic.WebApi
{
    public class HttpDomainRoute
        : DomainRoute
    {
        public HttpDomainRoute(string domain, string url, RouteValueDictionary defaults)
            : base(domain, url, defaults, HttpControllerRouteHandler.Instance)
        {
        }

        public HttpDomainRoute(string domain, string url, object defaults)
            : base(domain, url, new RouteValueDictionary(defaults), HttpControllerRouteHandler.Instance)
        {
        }
    }
}

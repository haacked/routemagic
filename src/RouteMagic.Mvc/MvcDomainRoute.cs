using System.Web.Routing;
using System.Web.Mvc;

namespace RouteMagic.Mvc
{
    public class MvcDomainRoute
        : DomainRoute
    {
        public MvcDomainRoute(string domain, string url, RouteValueDictionary defaults)
            : base(domain, url, defaults, new MvcRouteHandler())
        {
        }

        public MvcDomainRoute(string domain, string url, object defaults)
            : base(domain, url, new RouteValueDictionary(defaults), new MvcRouteHandler())
        {
            Domain = domain;
        }
    }
}

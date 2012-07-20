using System.Web;
using System.Web.Routing;

namespace RouteMagic.RouteHandlers
{
    public class HttpHandlerRouteHandler : IRouteHandler
    {
        public IHttpHandler HttpHandler
        {
            get;
            private set;
        }

        public HttpHandlerRouteHandler(IHttpHandler httpHandler)
        {
            HttpHandler = httpHandler;
        }

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return HttpHandler;
        }
    }
}

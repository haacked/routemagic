using System;
using System.Web;
using System.Web.Routing;

namespace RouteMagic.RouteHandlers
{
    /// <summary>
    /// An implementation of <see cref="IRouteHandler">IRouteHandler</see> 
    /// specifically for routing to http handlers
    /// </summary>
    public class HttpHandlerRouteHandler<THttpHandler> : IRouteHandler where THttpHandler : IHttpHandler
    {
        readonly Func<RequestContext, THttpHandler> _handlerFactory;

        public HttpHandlerRouteHandler(Func<RequestContext, THttpHandler> handlerFactory)
        {
            _handlerFactory = handlerFactory;
        }

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return _handlerFactory(requestContext);
        }
    }
}

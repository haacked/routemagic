using System.Web;
using System.Web.Mvc;

namespace RouteMagic.RouteHandlers
{
    public class DependencyResolvingRouteHandler<THandler> : HttpHandlerRouteHandler<THandler> where THandler : IHttpHandler
    {
        public DependencyResolvingRouteHandler()
            : base(requestContext => DependencyResolver.Current.GetService<THandler>())
        {
        }
    }
}

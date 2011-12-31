using System.Web.Routing;

namespace RouteMagic
{
    public interface IRouteRegistrar
    {
        void RegisterRoutes(RouteCollection routes);
    }
}
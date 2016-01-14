using System.Web.Routing;

namespace RouteBasics
{
    public interface IRouteRegistrar
    {
        void RegisterRoutes(RouteCollection routes);
    }
}
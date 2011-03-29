using System.Collections.Generic;
using System.Web;
using System.Web.Routing;
using Moq;

namespace UnitTests
{
    public class RouteEvaluator
    {
        RouteCollection routes;
        
        public RouteEvaluator(RouteCollection routes)
        {
            this.routes = routes;
        }

        public IList<RouteData> GetMatches(string virtualPath)
        {
            return GetMatches(virtualPath, "GET");
        }

        public IList<RouteData> GetMatches(string virtualPath, string httpMethod)
        {
            List<RouteData> matchingRouteData = new List<RouteData>();

            foreach (var route in this.routes)
            {
                var context = new Mock<HttpContextBase>();
                var request = new Mock<HttpRequestBase>();

                context.Setup(ctx => ctx.Request).Returns(request.Object);
                request.Setup(req => req.PathInfo).Returns(string.Empty);
                request.Setup(req => req.AppRelativeCurrentExecutionFilePath).Returns(virtualPath);
                if (!string.IsNullOrEmpty(httpMethod))
                {
                    request.Setup(req => req.HttpMethod).Returns(httpMethod);
                }

                RouteData routeData = this.routes.GetRouteData(context.Object);
                if (routeData != null)
                    matchingRouteData.Add(routeData);
            }

            return matchingRouteData;
        }
    }
}

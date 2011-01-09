using System.Web;
using System.Web.Routing;
using Moq;
using PowerAssert;
using RouteMagic;
using Xunit;

namespace UnitTests {
    public class RouteExtensionsTest {
        [Fact]
        public void MapHttpHandlerSetsRouteNameAndHttpHandlerRouteHandler() {
            // Arrange
            var obj = new RouteValueDictionary { { "foo", "bar" } };
            var routeValues = new RouteValueDictionary(obj);
            PAssert.IsTrue(() => (string)routeValues["foo"] == "bar");

            var httpHandler = new Mock<IHttpHandler>().Object;
            var routes = new RouteCollection();

            // Act
            var route = routes.MapHttpHandler("route-name", httpHandler, "url");

            // Assert
            PAssert.IsTrue(() => route.GetRouteName() == "route-name");
            PAssert.IsTrue(() => route.RouteHandler.GetHttpHandler(null) == httpHandler);
        }

        [Fact]
        public void MapHttpHandlerWithFuncSetsRouteNameAndHttpHandlerRouteHandler() {
            // Arrange
            var httpHandler = new Mock<IHttpHandler>().Object;
            var routes = new RouteCollection();

            // Act
            var route = routes.MapHttpHandler<IHttpHandler>("route-name", "url", r => httpHandler);

            // Assert
            PAssert.IsTrue(() => route.GetRouteName() == "route-name");
            PAssert.IsTrue(() => route.RouteHandler.GetHttpHandler(null) == httpHandler);
        }
    }
}

using System.Web;
using System.Web.Routing;
using Moq;
using RouteTesterDemoWeb;
using Xunit;

namespace UnitTests {
    public class RouteTests {
        [Fact]
        public void CanMatchUsingRouteEvaluator() {
            var routes = new RouteCollection();
            GlobalApplication.RegisterRoutes(routes);

            var evaluator = new RouteEvaluator(routes);
            var matchingRouteData = evaluator.GetMatches("~/foo/bar");
            Assert.True(matchingRouteData.Count > 0);
            matchingRouteData = evaluator.GetMatches("~/foo/bar/baz/quux/yadda/billy");
            Assert.Equal(0, matchingRouteData.Count);
        }

        [Fact]
        public void CanMatchRouteTheShortWay() {
            // Arrange
            var routes = new RouteCollection();
            GlobalApplication.RegisterRoutes(routes);
            var context = RoutingMockHelpers.FakeHttpContext("~/foo/bar");

            // Act
            var routeData = routes.GetRouteData(context);

            // Assert
            Assert.Equal("bar", routeData.Values["id"]);
            Assert.Equal("Test", routeData.Values["controller"]);
            Assert.Equal("Index", routeData.Values["action"]);
            Assert.Equal("foo-route", routeData.DataTokens["routeName"]);
        }

        [Fact]
        public void CanMatchRouteTheLongWay() {
            // Arrange
            var routes = new RouteCollection();
            GlobalApplication.RegisterRoutes(routes);

            var contextMock = new Mock<HttpContextBase>();
            var requestMock = new Mock<HttpRequestBase>();

            contextMock.Setup(ctx => ctx.Request).Returns(requestMock.Object);
            requestMock.Setup(req => req.PathInfo).Returns(string.Empty);
            requestMock.Setup(req => req.AppRelativeCurrentExecutionFilePath).Returns("~/foo/bar");

            // Act
            var routeData = routes.GetRouteData(contextMock.Object);

            // Assert
            Assert.Equal("bar", routeData.Values["id"]);
            Assert.Equal("Test", routeData.Values["controller"]);
            Assert.Equal("Index", routeData.Values["action"]);
            Assert.Equal("foo-route", routeData.DataTokens["routeName"]);
        }

        [Fact]
        public void CanMatchEmptyUrl() {
            RouteCollection routes = new RouteCollection();
            routes.Add(new Route(string.Empty, new Mock<IRouteHandler>().Object) { Defaults = new RouteValueDictionary(new { controller = "Home" }) });

            var context = RoutingMockHelpers.FakeHttpContext("~/");
            var routeData = routes.GetRouteData(context);
            Assert.NotNull(routeData);
            Assert.Equal("Home", routeData.Values["controller"]);
        }
    }
}

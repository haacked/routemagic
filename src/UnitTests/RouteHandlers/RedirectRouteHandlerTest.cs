using System.Web;
using System.Web.Routing;
using Moq;
using PowerAssert;
using RouteMagic.HttpHandlers;
using RouteMagic.Internals;
using Xunit;

namespace UnitTests.RouteHandlers {
    // The RedirectRoute class is also a route handler. These tests test that specific 
    // aspect of the RedirectRoute class. Hence they are in their own test class.
    public class RedirectRouteHandlerTest {
        [Fact]
        public void GetHttpHandler_WithTargetRoute_ReturnsHandlerFromTargetRoute() {
            // Arrange
            var httpContext = new Mock<HttpContextBase>();
            httpContext.Setup(c => c.Request.Path).Returns("/qux");
            httpContext.Setup(c => c.Request.AppRelativeCurrentExecutionFilePath).Returns("~/qux");
            httpContext.Setup(c => c.Request.ApplicationPath).Returns("/");
            httpContext.Setup(c => c.Response.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(s => s);
            var routeData = new RouteData();
            routeData.Values.Add("bar", "the-value-of-bar");
            var requestContext = new RequestContext(httpContext.Object, routeData);
            var targetRoute = new Route("foo/{bar}", null);
            var redirectRouteHandler = new RedirectRoute(null, targetRoute, permanent: false, additionalRouteValues: null);

            // Act
            var httpHandler = redirectRouteHandler.GetHttpHandler(requestContext) as RedirectHttpHandler;

            // Assert
            PAssert.IsTrue(() => httpHandler.IsReusable == false);
            PAssert.IsTrue(() => httpHandler.TargetUrl == "~/foo/the-value-of-bar");
            PAssert.IsTrue(() => httpHandler.Permanent == false);
        }

        [Fact]
        public void GetHttpHandler_WithTargetRouteAndAdditionalRouteData_MergesAdditionalRouteValues() {
            // Arrange
            var httpContext = new Mock<HttpContextBase>();
            httpContext.Setup(c => c.Request.Path).Returns("/qux");
            httpContext.Setup(c => c.Request.AppRelativeCurrentExecutionFilePath).Returns("~/qux");
            httpContext.Setup(c => c.Request.ApplicationPath).Returns("/");
            httpContext.Setup(c => c.Response.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(s => s);
            var routeData = new RouteData();
            routeData.Values.Add("bar", "the-value-of-bar");
            var requestContext = new RequestContext(httpContext.Object, routeData);
            var targetRoute = new Route("foo/{bar}/{baz}", null);
            var additionalRouteValues = new RouteValueDictionary();
            additionalRouteValues.Add("baz", "bizaz");
            var redirectRouteHandler = new RedirectRoute(null, targetRoute, false, additionalRouteValues);

            // Act
            var httpHandler = redirectRouteHandler.GetHttpHandler(requestContext) as RedirectHttpHandler;

            // Assert
            PAssert.IsTrue(() => httpHandler.TargetUrl == "~/foo/the-value-of-bar/bizaz");
        }

        [Fact]
        public void GetHttpHandler_WithTargetRouteAndAdditionalRouteData_GivesRequestRouteDataPrecedence() {
            // Arrange
            var httpContext = new Mock<HttpContextBase>();
            httpContext.Setup(c => c.Request.Path).Returns("/qux");
            httpContext.Setup(c => c.Request.AppRelativeCurrentExecutionFilePath).Returns("~/qux");
            httpContext.Setup(c => c.Request.ApplicationPath).Returns("/");
            httpContext.Setup(c => c.Response.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(s => s);
            var routeData = new RouteData();
            routeData.Values.Add("baz", "the-value-of-baz");
            var requestContext = new RequestContext(httpContext.Object, routeData);
            var targetRoute = new Route("foo/{bar}/{baz}", null);
            var additionalRouteValues = new RouteValueDictionary();
            additionalRouteValues.Add("bar", "bar");
            additionalRouteValues.Add("baz", "bizaz");
            var redirectRouteHandler = new RedirectRoute(null, targetRoute, false, additionalRouteValues);

            // Act
            var httpHandler = redirectRouteHandler.GetHttpHandler(requestContext) as RedirectHttpHandler;

            // Assert
            PAssert.IsTrue(() => httpHandler.TargetUrl == "~/foo/bar/the-value-of-baz");
        }

        [Fact]
        public void GetHttpHandler_WithTargetRoute_ButRequestDoesNotMatchTargetRouteReturnsDelegateHttpHandler() {
            // Arrange
            var httpContext = new Mock<HttpContextBase>();
            httpContext.Setup(c => c.Request.Path).Returns("/qux");
            httpContext.Setup(c => c.Request.AppRelativeCurrentExecutionFilePath).Returns("~/qux");
            httpContext.Setup(c => c.Request.ApplicationPath).Returns("/");
            httpContext.Setup(c => c.Response.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(s => s);
            var routeData = new RouteData();
            var requestContext = new RequestContext(httpContext.Object, new RouteData());
            var targetRoute = new Route("foo/{bar}", null);
            var redirectRouteHandler = new RedirectRoute(null, targetRoute, permanent: false, additionalRouteValues: null);

            // Act
            var httpHandler = redirectRouteHandler.GetHttpHandler(requestContext) as DelegateHttpHandler;

            // Assert
            PAssert.IsTrue(() => httpHandler != null);
        }
    }
}

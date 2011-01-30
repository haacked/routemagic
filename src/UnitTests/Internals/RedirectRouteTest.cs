using System;
using System.Web;
using System.Web.Routing;
using Moq;
using PowerAssert;
using RouteMagic.Internals;
using Xunit;

namespace UnitTests.Internals {
    public class RedirectRouteTest {
        [Fact]
        public void GetRouteData_WithNonMatchingSourceRoute_ReturnsNull() {
            // Arrange
            var sourceRouteMock = new Mock<RouteBase>();
            var targetRoute = new Mock<RouteBase>().Object;
            sourceRouteMock.Setup(r => r.GetRouteData(It.IsAny<HttpContextBase>())).Returns((RouteData)null);
            var redirectRoute = new RedirectRoute(sourceRouteMock.Object, targetRoute, permanent: true);

            // Act
            var routeData = redirectRoute.GetRouteData(new Mock<HttpContextBase>().Object);

            // Assert
            PAssert.IsTrue(() => routeData == null);
            // These next few asserts are "laziness" asserts.
            PAssert.IsTrue(() => redirectRoute.Permanent == true);
            PAssert.IsTrue(() => redirectRoute.SourceRoute == sourceRouteMock.Object);
            PAssert.IsTrue(() => redirectRoute.TargetRoute == targetRoute);
        }

        [Fact]
        public void GetRouteData_WithMatchingSourceRoute_SwapsRouteHandler() {
            // Arrange
            var routeData = new RouteData();
            var routeHandler = new Mock<IRouteHandler>().Object;
            routeData.RouteHandler = routeHandler;
            var sourceRouteMock = new Mock<RouteBase>();
            sourceRouteMock.Setup(r => r.GetRouteData(It.IsAny<HttpContextBase>())).Returns(routeData);
            var redirectRoute = new RedirectRoute(sourceRouteMock.Object, new Mock<RouteBase>().Object, permanent: false);

            // Act
            var redirectRouteData = redirectRoute.GetRouteData(new Mock<HttpContextBase>().Object);

            // Assert
            PAssert.IsTrue(() => redirectRouteData.RouteHandler != routeHandler);
            PAssert.IsTrue(() => redirectRouteData.RouteHandler is RedirectRoute);
            PAssert.IsTrue(() => redirectRouteData == routeData);
            PAssert.IsTrue(() => redirectRoute.Permanent == false);
        }

        [Fact]
        public void GetVirtualPath_WithMatchingRequest_AlwaysReturnsNull() {
            // Arrange
            var httpContext = new Mock<HttpContextBase>();
            httpContext.Setup(c => c.Request.Path).Returns("/foo");
            httpContext.Setup(c => c.Request.AppRelativeCurrentExecutionFilePath).Returns("~/foo");
            httpContext.Setup(c => c.Request.ApplicationPath).Returns("/");
            var requestContext = new RequestContext(httpContext.Object, new RouteData());

            var sourceRouteMock = new Mock<RouteBase>();
            sourceRouteMock.Setup(r => r.GetVirtualPath(It.IsAny<RequestContext>(), It.IsAny<RouteValueDictionary>())).Returns(new VirtualPathData(sourceRouteMock.Object, "/foo"));
            var targetRoute = new Mock<RouteBase>().Object;
            var redirectRoute = new RedirectRoute(sourceRouteMock.Object, targetRoute, permanent: true);

            // Act
            var virtualPath = redirectRoute.GetVirtualPath(requestContext, new RouteValueDictionary());

            // Assert
            PAssert.IsTrue(() => virtualPath == null);
        }

        [Fact]
        public void To_WithTargetRoute_SetsTargetRoute() {
            // Arrange
            var sourceRoute = new Mock<RouteBase>().Object;
            var targetRoute = new Mock<RouteBase>().Object;
            var redirectRoute = new RedirectRoute(sourceRoute, null, false);

            // Act
            redirectRoute.To(targetRoute);

            // Assert
            PAssert.IsTrue(() => redirectRoute.TargetRoute == targetRoute);
        }

        [Fact]
        public void To_WithNullTargetRoute_ThrowsException() {
            // Arrange
            var redirectRoute = new RedirectRoute(new Mock<RouteBase>().Object, null, false);

            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => redirectRoute.To(null));
        }

        [Fact]
        public void To_WhenTargetRouteIsAlreadySet_ThrowsException() {
            // Arrange
            var redirectRoute = new RedirectRoute(new Mock<RouteBase>().Object, new Mock<RouteBase>().Object, false);

            // Act, Assert
            Assert.Throws<InvalidOperationException>(() => redirectRoute.To(new Mock<RouteBase>().Object));
        }

        [Fact]
        public void To_WhenAdditionalRouteValuesAlreadySet_ThrowsException() {
            // Arrange
            var redirectRoute = new RedirectRoute(new Mock<RouteBase>().Object, null, false, new RouteValueDictionary());

            // Act, Assert
            Assert.Throws<InvalidOperationException>(() => redirectRoute.To(new Mock<RouteBase>().Object));
        }

    }
}

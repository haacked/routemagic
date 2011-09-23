using System;
using System.Web.Routing;
using Moq;
using PowerAssert;
using RouteMagic;
using RouteMagic.Internals;
using Xunit;

namespace UnitTests {
    public class RedirectRouteExtensionsTest {
        [Fact]
        public void Redirect_SourceAndTarget_ReturnsRedirectRoute() {
            // Arrange
            var routes = new RouteCollection();
            var sourceRoute = new Mock<RouteBase>().Object;
            var targetRoute = new Mock<RouteBase>().Object;

            // Act
            var redirectRoute = routes.Redirect(r => sourceRoute, permanent: false).To(targetRoute);

            // Assert
            PAssert.IsTrue(() => redirectRoute.SourceRoute == sourceRoute);
            PAssert.IsTrue(() => redirectRoute.TargetRoute == targetRoute);
            PAssert.IsTrue(() => redirectRoute.Permanent == false);
        }

        [Fact]
        public void Redirect_WithRouteMappingFunc_DoesNotAddRouteToMainRoutes() {
            // Arrange
            var routes = new RouteCollection();
            var targetRoute = new Mock<RouteBase>().Object;

            // Act
            var redirectRoute = routes.Redirect(r => r.Map("test", "testurl")).To(targetRoute);

            // Assert
            PAssert.IsTrue(() => routes.Count == 1);
            PAssert.IsTrue(() => routes[0] is NormalizeRoute);
        }

        [Fact]
        public void Redirect_SourceRouteButNoTarget_ReturnsPartialRedirectRoute() {
            // Arrange
            var routes = new RouteCollection();
            var sourceRoute = new Mock<RouteBase>().Object;

            // Act
            var redirectRoute = routes.Redirect(r => sourceRoute, permanent: true);

            // Assert
            PAssert.IsTrue(() => redirectRoute.SourceRoute == sourceRoute);
            PAssert.IsTrue(() => redirectRoute.TargetRoute == null);
            PAssert.IsTrue(() => redirectRoute.Permanent == true);
        }


        [Fact]
        public void Redirect_WithNullRoutes_ThrowsArgumentNullException() {
            // arrange
            RouteCollection routes = null;

            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => routes.Redirect(r => new Mock<RouteBase>().Object));
        }

        [Fact]
        public void Redirect_WithNullSourceRouteMapping_ThrowsArgumentNullException() {
            // arrange
            var targetRoute = new Mock<RouteBase>().Object;
            RouteCollection routes = new RouteCollection();

            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => routes.Redirect(null));

        }
    }
}

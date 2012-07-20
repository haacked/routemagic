using System.Web.Routing;
using Moq;
using RouteMagic.Internals;
using Xunit;

namespace UnitTests.Internals {
    public class NormalizeRouteTest {
        [Fact]
        public void GetVirtualPath_WithDefaultNormalizeRoute_ReturnsLowerCaseUrl() {
            // Arrange
            var originalRoute = new Mock<RouteBase>();
            originalRoute.Setup(r => r.GetVirtualPath(null, null)).Returns(new VirtualPathData(originalRoute.Object, "/Foo/Bar"));
            var normalizeRoute = new NormalizeRoute(originalRoute.Object);

            // Act
            var url = normalizeRoute.GetVirtualPath(null, null);

            // Assert
            Assert.Equal("/foo/bar", url.VirtualPath);
        }

        [Fact]
        public void GetVirtualPath_WithLowerCasingFalse_ReturnsOriginalCasing() {
            // Arrange
            var originalRoute = new Mock<RouteBase>();
            originalRoute.Setup(r => r.GetVirtualPath(null, null)).Returns(new VirtualPathData(originalRoute.Object, "/Foo/Bar"));
            var normalizeRoute = new NormalizeRoute(originalRoute.Object, requireLowerCase: false, appendTrailingSlash: false);

            // Act
            var url = normalizeRoute.GetVirtualPath(null, null);

            // Assert
            Assert.Equal("/Foo/Bar", url.VirtualPath);
        }

        [Fact]
        public void GetVirtualPath_WithAppendTrailingSlashAndQueryString_ReturnsTrailingSlashBeforeQuery() {
            // Arrange
            var originalRoute = new Mock<RouteBase>();
            originalRoute.Setup(r => r.GetVirtualPath(null, null)).Returns(new VirtualPathData(originalRoute.Object, "/Foo/Bar?Baz"));
            var normalizeRoute = new NormalizeRoute(originalRoute.Object, requireLowerCase: false, appendTrailingSlash: true);

            // Act
            var url = normalizeRoute.GetVirtualPath(null, null);

            // Assert
            Assert.Equal("/Foo/Bar/?Baz", url.VirtualPath);
        }

        [Fact]
        public void GetVirtualPath_WithAppendTrailingSlashWithoutQueryString_ReturnsTrailingSlash() {
            // Arrange
            var originalRoute = new Mock<RouteBase>();
            originalRoute.Setup(r => r.GetVirtualPath(null, null)).Returns(new VirtualPathData(originalRoute.Object, "/Foo/Bar"));
            var normalizeRoute = new NormalizeRoute(originalRoute.Object, requireLowerCase: false, appendTrailingSlash: true);

            // Act
            var url = normalizeRoute.GetVirtualPath(null, null);

            // Assert
            Assert.Equal("/Foo/Bar/", url.VirtualPath);
        }

        [Fact]
        public void GetVirtualPath_WithAppendTrailingSlashForRoot_ReturnsSingleSlash() {
            // Arrange
            var originalRoute = new Mock<RouteBase>();
            originalRoute.Setup(r => r.GetVirtualPath(null, null)).Returns(new VirtualPathData(originalRoute.Object, "/"));
            var normalizeRoute = new NormalizeRoute(originalRoute.Object, requireLowerCase: false, appendTrailingSlash: true);

            // Act
            var url = normalizeRoute.GetVirtualPath(null, null);

            // Assert
            Assert.Equal("/", url.VirtualPath);
        }

        [Fact]
        public void GetVirtualPath_WithAppendTrailingSlashForRootAndQueryString_ReturnsSingleSlash() {
            // Arrange
            var originalRoute = new Mock<RouteBase>();
            originalRoute.Setup(r => r.GetVirtualPath(null, null)).Returns(new VirtualPathData(originalRoute.Object, "/?foo=bar"));
            var normalizeRoute = new NormalizeRoute(originalRoute.Object, requireLowerCase: false, appendTrailingSlash: true);

            // Act
            var url = normalizeRoute.GetVirtualPath(null, null);

            // Assert
            Assert.Equal("/?foo=bar", url.VirtualPath);
        }
    }
}

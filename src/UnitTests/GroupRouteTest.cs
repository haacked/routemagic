using System.Web;
using System.Web.Routing;
using Moq;
using PowerAssert;
using RouteMagic;
using RouteMagic.Internals;
using Xunit;

namespace UnitTests {
    public class GroupRouteTest {
        [Fact]
        public void GetRouteDataMatchesChildRouteRequest() {
            // Arrange
            var httpContext = new Mock<HttpContextBase>();
            httpContext.Setup(c => c.Request.Path).Returns("/blog/foo/bar");
            httpContext.Setup(c => c.Request.AppRelativeCurrentExecutionFilePath).Returns("~/blog/foo/bar");

            var routeHandler = new Mock<IRouteHandler>().Object;
            var childRoutes = new RouteCollection();
            childRoutes.Add(new Route("no-match", new Mock<IRouteHandler>().Object));
            childRoutes.Add(new Route("foo/bar", routeHandler));
            var virtualPathResolver = new Mock<IVirtualPathResolver>();
            virtualPathResolver.Setup(r => r.ToAbsolute("~/blog")).Returns("/blog");
            var groupRoute = new GroupRoute("~/blog", childRoutes, virtualPathResolver.Object);

            // Act
            var routeData = groupRoute.GetRouteData(httpContext.Object);

            // Assert
            PAssert.IsTrue(() => routeData != null);
            PAssert.IsTrue(() => routeHandler == routeData.RouteHandler);
        }

        [Fact]
        public void GetRouteDataMatchesChildRouteRequestWithParentAtRoot() {
            // Arrange
            var httpContext = new Mock<HttpContextBase>();
            httpContext.Setup(c => c.Request.Path).Returns("/foo/bar");
            httpContext.Setup(c => c.Request.AppRelativeCurrentExecutionFilePath).Returns("~/foo/bar");

            var routeHandler = new Mock<IRouteHandler>().Object;
            var childRoutes = new RouteCollection();
            childRoutes.Add(new Route("no-match", new Mock<IRouteHandler>().Object));
            childRoutes.Add(new Route("foo/bar", routeHandler));
            var virtualPathResolver = new Mock<IVirtualPathResolver>();
            virtualPathResolver.Setup(r => r.ToAbsolute("~/")).Returns("/");
            var groupRoute = new GroupRoute(childRoutes, virtualPathResolver.Object);

            // Act
            var routeData = groupRoute.GetRouteData(httpContext.Object);

            // Assert
            PAssert.IsTrue(() => routeData != null);
            PAssert.IsTrue(() => routeHandler == routeData.RouteHandler);
        }

        [Fact]
        public void GetRouteDataGetsMatchingChildRouteData() {
            // Arrange
            var httpContext = new Mock<HttpContextBase>();
            httpContext.Setup(c => c.Request.Path).Returns("/foo/bar/uno/dos/tres");
            httpContext.Setup(c => c.Request.AppRelativeCurrentExecutionFilePath).Returns("~/foo/bar/uno/dos/tres");

            var routeHandler = new Mock<IRouteHandler>().Object;
            var childRoutes = new RouteCollection();
            childRoutes.Add(new Route("no-match", new Mock<IRouteHandler>().Object));
            childRoutes.Add(new Route("uno/{seg}/{seg2}", routeHandler));
            var virtualPathResolver = new Mock<IVirtualPathResolver>();
            virtualPathResolver.Setup(r => r.ToAbsolute("~/foo/bar")).Returns("/foo/bar");
            var groupRoute = new GroupRoute("~/foo/bar", childRoutes, virtualPathResolver.Object);

            // Act
            var routeData = groupRoute.GetRouteData(httpContext.Object);

            // Assert
            PAssert.IsTrue(() => routeData != null);
            PAssert.IsTrue(() => routeData.Values["seg"].Equals("dos"));
            PAssert.IsTrue(() => routeData.Values["seg2"].Equals("tres"));
        }

        [Fact]
        public void GetVirtualPathWithNameSuppliedReturnsPathToChildRoute() {
            // Arrange
            var childRoutes = new RouteCollection();
            childRoutes.Add("r1", new Route("no-match", new Mock<IRouteHandler>().Object));
            childRoutes.Add("r2", new Route("uno/{seg}/{seg2}", new Mock<IRouteHandler>().Object));
            var virtualPathResolver = new Mock<IVirtualPathResolver>();
            virtualPathResolver.Setup(r => r.ToAbsolute("~/foo/bar")).Returns("/foo/bar");
            var groupRoute = new GroupRoute("~/foo/bar", childRoutes, virtualPathResolver.Object);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.Setup(c => c.Request.Path).Returns("/qux");
            httpContext.Setup(c => c.Request.AppRelativeCurrentExecutionFilePath).Returns("~/qux");
            httpContext.Setup(c => c.Request.ApplicationPath).Returns("/");
            httpContext.Setup(c => c.Response.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(s => s);
            var requestContext = new RequestContext(httpContext.Object, new RouteData());

            // Act
            var routeValues = new RouteValueDictionary { { "seg", "dos" }, { "seg2", "three" } }.SetRouteName("r2");
            var virtualPath = groupRoute.GetVirtualPath(requestContext, routeValues);

            // Assert
            PAssert.IsTrue(() => virtualPath != null);
            PAssert.IsTrue(() => virtualPath.VirtualPath == "foo/bar/uno/dos/three");
        }

        [Fact]
        public void GetVirtualPathWithParentInApplicationRootWithNameSuppliedReturnsPathToChildRoute() {
            // Arrange
            var childRoutes = new RouteCollection();
            childRoutes.Add("r1", new Route("no-match", new Mock<IRouteHandler>().Object));
            childRoutes.Add("r2", new Route("uno/{seg}/{seg2}", new Mock<IRouteHandler>().Object));
            var virtualPathResolver = new Mock<IVirtualPathResolver>();
            virtualPathResolver.Setup(r => r.ToAbsolute("~/")).Returns("/");
            var groupRoute = new GroupRoute(childRoutes, virtualPathResolver.Object);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.Setup(c => c.Request.Path).Returns("/qux");
            httpContext.Setup(c => c.Request.AppRelativeCurrentExecutionFilePath).Returns("~/qux");
            httpContext.Setup(c => c.Request.ApplicationPath).Returns("/");
            httpContext.Setup(c => c.Response.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(s => s);
            var requestContext = new RequestContext(httpContext.Object, new RouteData());

            // Act
            var routeValues = new RouteValueDictionary { { "seg", "dos" }, { "seg2", "three" } }.SetRouteName("r2");
            var virtualPath = groupRoute.GetVirtualPath(requestContext, routeValues);

            // Assert
            PAssert.IsTrue(() => virtualPath != null);
            PAssert.IsTrue(() => virtualPath.VirtualPath == "uno/dos/three");
        }

        [Fact]
        public void GetVirtualPathWithApplicationRootReturnsVirtualPath() {
            // Arrange
            var childRoutes = new RouteCollection();
            childRoutes.Map("r1", "no-match", new Mock<IRouteHandler>().Object);
            childRoutes.Map("r2", "uno/{seg}/{seg2}", new Mock<IRouteHandler>().Object);
            var virtualPathResolver = new Mock<IVirtualPathResolver>();
            virtualPathResolver.Setup(r => r.ToAbsolute("~/")).Returns("/");
            var groupRoute = new GroupRoute(childRoutes, virtualPathResolver.Object);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.Setup(c => c.Request.Path).Returns("/qux");
            httpContext.Setup(c => c.Request.AppRelativeCurrentExecutionFilePath).Returns("~/qux");
            httpContext.Setup(c => c.Request.ApplicationPath).Returns("/Foo/Bar");
            httpContext.Setup(c => c.Response.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(s => s.Remove(0, "/Foo/Bar".Length));
            var requestContext = new RequestContext(httpContext.Object, new RouteData());

            // Act
            var routeValues = new RouteValueDictionary { { "seg", "dos" }, { "seg2", "three" } }.SetRouteName("r2");
            var virtualPath = groupRoute.GetVirtualPath(requestContext, routeValues);

            // Assert
            PAssert.IsTrue(() => virtualPath != null);
            PAssert.IsTrue(() => virtualPath.VirtualPath == "uno/dos/three");
        }
    }
}

using System.Web;
using System.Web.Routing;
using Moq;
using PowerAssert;
using RouteMagic.RouteHandlers;
using Xunit;

namespace UnitTests.RouteHandlers {
    public class HttpHandlerRouteHandlerTest {
        [Fact]
        public void GetHttpHandlerReturnsHttpHandler() {
            // Arrange
            var expectedHttpHandler = new Mock<IHttpHandler>().Object;
            var routeHandler = new HttpHandlerRouteHandler(expectedHttpHandler);

            // Act
            var httpHandler = routeHandler.GetHttpHandler(new RequestContext());

            // Assert
            PAssert.IsTrue(() => httpHandler == expectedHttpHandler);
        }

        [Fact]
        public void GetHttpHandlerReturnsHttpHandlerFromFunc() {
            // Arrange
            var expectedHttpHandler = new Mock<IHttpHandler>().Object;
            bool funcCalled = false;
            var routeHandler = new HttpHandlerRouteHandler<IHttpHandler>(r => { funcCalled = true; return expectedHttpHandler; });

            // Act
            var httpHandler = routeHandler.GetHttpHandler(new RequestContext());

            // Assert
            PAssert.IsTrue(() => funcCalled);
            PAssert.IsTrue(() => httpHandler == expectedHttpHandler);
        }
    }
}

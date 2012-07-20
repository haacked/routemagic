using System;
using System.Web;
using System.Web.Routing;
using Moq;
using PowerAssert;
using RouteMagic.RouteHandlers;
using Xunit;

namespace UnitTests.RouteHandlers {
    public class DelegateRouteHandlerTest {
        [Fact]
        public void GetHttpHandler_CallsFunc_ToReturnHttpHandler() {
            // arrange
            var httpHandler = new Mock<IHttpHandler>().Object;
            Func<RequestContext, IHttpHandler> action = (context) => httpHandler;
            var handler = new DelegateRouteHandler(action);

            // act
            var returnedHttpHandler = handler.GetHttpHandler(new RequestContext());

            // assert
            PAssert.IsTrue(() => handler.HttpHandlerAction == action);
            PAssert.IsTrue(() => httpHandler == returnedHttpHandler);
        }

        [Fact]
        public void GetHttpHandler_WithNullAction_ThrowsInvalidOperationException() {
            // Arrange
            var requestContext = new RequestContext();
            var handler = new DelegateRouteHandler(null);


            // Act, Assert
            Assert.Throws<InvalidOperationException>(() => handler.GetHttpHandler(requestContext));
        }
    }
}

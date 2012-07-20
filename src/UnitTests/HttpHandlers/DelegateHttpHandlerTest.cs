using System;
using System.IO;
using System.Web;
using System.Web.Routing;
using Moq;
using PowerAssert;
using RouteMagic.HttpHandlers;
using Xunit;

namespace UnitTests.HttpHandlers {
    public class DelegateHttpHandlerTest {
        [Fact]
        public void ProcessRequest_WithAction_CallsAction() {
            // arrange
            bool actionCalled = false;
            var httpRequest = new HttpRequest("foo", "http://foo.com/", "");
            var httpResponse = new HttpResponse(new Mock<TextWriter>().Object);
            var httpContext = new HttpContext(httpRequest, httpResponse);
            var handler = new DelegateHttpHandler((c) => actionCalled = true, new RouteData(), false);

            // act
            handler.ProcessRequest(httpContext);

            // assert
            PAssert.IsTrue(() => handler.IsReusable == false);
            PAssert.IsTrue(() => actionCalled == true);
        }

        [Fact]
        public void Ctor_WithNullAction_ThrowsArgumentNullException() {
            // arrange, act, assert
            var exception = Assert.Throws<ArgumentNullException>(
                () => new DelegateHttpHandler(null, new RouteData(), true)
            );
            PAssert.IsTrue(() => exception.ParamName == "action");
        }

        [Fact]
        public void Ctor_WithNullRouteData_ThrowsArgumentNullException() {
            // arrange, act, assert
            var exception = Assert.Throws<ArgumentNullException>(
                () => new DelegateHttpHandler(c => c.ToString(), null, true)
            );
            PAssert.IsTrue(() => exception.ParamName == "routeData");
        }
    }
}

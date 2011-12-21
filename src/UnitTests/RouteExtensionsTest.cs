using System;
using System.IO;
using System.Web;
using System.Web.Routing;
using Moq;
using PowerAssert;
using RouteMagic;
using RouteMagic.HttpHandlers;
using Xunit;

namespace UnitTests
{
    public class RouteExtensionsTest
    {
        [Fact]
        public void MapHttpHandlerSetsRouteNameAndHttpHandlerRouteHandler()
        {
            // Arrange
            var obj = new RouteValueDictionary { { "foo", "bar" } };
            var routeValues = new RouteValueDictionary(obj);
            PAssert.IsTrue(() => (string)routeValues["foo"] == "bar");

            var httpHandler = new Mock<IHttpHandler>().Object;
            var routes = new RouteCollection();

            // Act
            var route = routes.MapHttpHandler("route-name", "url", httpHandler);

            // Assert
            PAssert.IsTrue(() => route.GetRouteName() == "route-name");
            PAssert.IsTrue(() => route.RouteHandler.GetHttpHandler(null) == httpHandler);
        }

        [Fact]
        public void MapHttpHandlerWithFuncSetsRouteNameAndHttpHandlerRouteHandler()
        {
            // Arrange
            var httpHandler = new Mock<IHttpHandler>().Object;
            var routes = new RouteCollection();

            // Act
            var route = routes.MapHttpHandler<IHttpHandler>("route-name", "url", r => httpHandler);

            // Assert
            PAssert.IsTrue(() => route.GetRouteName() == "route-name");
            PAssert.IsTrue(() => route.RouteHandler.GetHttpHandler(null) == httpHandler);
        }

        [Fact]
        public void MapDelegateSetsRouteNameAndHttpHandlerRouteHandler()
        {
            // Arrange
            var httpHandler = new Mock<IHttpHandler>().Object;
            var routes = new RouteCollection();
            bool isSet = false;
            var httpRequest = new HttpRequest("foo", "http://foo.com/", "");
            var httpResponse = new HttpResponse(new Mock<TextWriter>().Object);
            var httpContext = new HttpContext(httpRequest, httpResponse);
            var requestContext = new RequestContext(new HttpContextWrapper(httpContext), new RouteData());

            // Act
            var route = routes.MapDelegate("route-name", "url", c => isSet = true);
            route.RouteHandler.GetHttpHandler(requestContext).ProcessRequest(httpContext);

            // Assert
            PAssert.IsTrue(() => route.GetRouteName() == "route-name");
            PAssert.IsTrue(() => route.RouteHandler.GetHttpHandler(requestContext).GetType() == typeof(DelegateHttpHandler));
            PAssert.IsTrue(() => isSet == true);
        }

        [Fact]
        public void MapDelegateWithStaticUrlDoesNotMatchForUrlGeneration()
        {
            // Arrange
            var routes = new RouteCollection();
            var httpRequest = new HttpRequest("foo", "http://foo.com/", "");
            var httpResponse = new HttpResponse(new Mock<TextWriter>().Object);
            var httpContext = new HttpContext(httpRequest, httpResponse);
            var requestContext = new RequestContext(new HttpContextWrapper(httpContext), new RouteData());

            // Act
            routes.MapDelegate("route-name", "url", c => { });
            var vp = routes[0].GetVirtualPath(requestContext, new RouteValueDictionary());

            // Assert
            PAssert.IsTrue(() => vp == null);
        }

        [Fact]
        public void MapDelegateWithParametersInUrlMatchesForUrlGeneration()
        {
            // Arrange
            var routes = new RouteCollection();
            var httpRequest = new HttpRequest("foo", "http://foo.com/", "");
            var httpResponse = new HttpResponse(new Mock<TextWriter>().Object);
            var httpContext = new HttpContext(httpRequest, httpResponse);
            var requestContext = new RequestContext(new HttpContextWrapper(httpContext), new RouteData());

            // Act
            routes.MapDelegate("route-name", "{url}", c => { });
            var vp = routes[0].GetVirtualPath(requestContext, new RouteValueDictionary { { "url", "blah" } });

            // Assert
            PAssert.IsTrue(() => vp != null);
        }

        [Fact]
        public void GetRouteName_WithNullRouteData_ReturnsNull()
        {
            // Arrange
            var routeData = (RouteData)null;

            // Act
            var result = routeData.GetRouteName();

            // Assert
            PAssert.IsTrue(() => result == null);
        }

        [Fact]
        public void GetRouteName_WithNullRoute_ReturnsNull()
        {
            // Arrange
            var route = (Route)null;

            // Act
            var result = route.GetRouteName();

            // Assert
            PAssert.IsTrue(() => result == null);
        }

        [Fact]
        public void SetRouteName_WithNullRoute_ThrowsArgumentNullException()
        {
            // Arrange
            var route = (Route)null;

            // Act, Assert
            var exception = Assert.Throws<ArgumentNullException>(() => route.SetRouteName("whatever"));
            PAssert.IsTrue(() => exception.ParamName == "route");
        }

    }
}

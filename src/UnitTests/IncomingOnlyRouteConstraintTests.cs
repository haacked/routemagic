using System.Web.Routing;
using RouteMagic;
using Xunit;

namespace UnitTests {
    public class IncomingOnlyRouteConstraintTests {
        public class TheMatchMethod {
            [Fact]
            public void ReturnsTrueForIncomingRequest() {
                var constraint = new IncomingOnlyRouteConstraint();

                var match = constraint.Match(null, null, null, null, RouteDirection.IncomingRequest);

                Assert.True(match);
            }

            [Fact]
            public void ReturnsFalseForUrlGeneration() {
                var constraint = new IncomingOnlyRouteConstraint();

                var match = constraint.Match(null, null, null, null, RouteDirection.UrlGeneration);

                Assert.False(match);
            }
        }
    }
}

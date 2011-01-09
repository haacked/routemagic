using System.Web.Routing;
using PowerAssert;
using RouteMagic;
using Xunit;

namespace UnitTests {
    public class RouteValueDictionaryExtensionsTest {
        [Fact]
        public void Merge_WithRouteValueDictionaries_GivesTargetPrecedence() {
            // Arrange
            var source = new RouteValueDictionary() { { "foo", "foovalue" }, { "bar", "barvalue" } };
            var target = new RouteValueDictionary() { { "foo", "foovalue2" }, { "baz", "bazvalue" } };

            // Act
            var merged = source.Merge(target);

            // Assert
            PAssert.IsTrue(() => merged.Count == 3);
            PAssert.IsTrue(() => (string)merged["foo"] == "foovalue2");
            PAssert.IsTrue(() => (string)merged["bar"] == "barvalue");
            PAssert.IsTrue(() => (string)merged["baz"] == "bazvalue");
        }

        [Fact]
        public void ConvertToRouteValueDictionary_WithObjectThatIsDictionary_JustReturnsObjectCastToDictionary() {
            // Arrange
            object routeValues = new RouteValueDictionary();

            // Act
            var dictionary = RouteValueDictionaryExtensions.ConvertToRouteValueDictionary(routeValues);

            // Assert
            PAssert.IsTrue(() => routeValues == dictionary);
        }

        [Fact]
        public void ConvertToRouteValueDictionary_WithNull_ReturnsNull() {
            // Arrange, Act
            var dictionary = RouteValueDictionaryExtensions.ConvertToRouteValueDictionary(null);

            // Assert
            PAssert.IsTrue(() => dictionary == null);
        }

        [Fact]
        public void ConvertToRouteValueDictionary_WithObjectDictionary_ReturnsPopulatedDictionary() {
            // Arrange
            var routeValues = new { foo = "bar" };

            // Act
            var dictionary = RouteValueDictionaryExtensions.ConvertToRouteValueDictionary(routeValues);

            // Assert
            PAssert.IsTrue(() => (string)dictionary["foo"] == "bar");
        }
    }
}

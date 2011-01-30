using PowerAssert;
using RouteMagic.HttpHandlers;
using Xunit;

namespace UnitTests.HttpHandlers {
    // No tests of ProcessRequest because IHttpHandler.ProcessRequest is not testable.
    // We could work around that, but in this case, the logic is very simple and not worth 
    // testing.
    public class RedirectHttpHandlerTest {
        // This isn't really worth testing either, but I tend to make 
        // typos when setting ctor properties.
        [Fact]
        public void Ctor_SetsCorrectProperties() {
            // Arrange, Act
            var handler = new RedirectHttpHandler(targetUrl: "~/foo", permanent: true, isReusable: false);

            // Assert
            PAssert.IsTrue(() => handler.TargetUrl == "~/foo");
            PAssert.IsTrue(() => handler.Permanent);
            PAssert.IsTrue(() => handler.IsReusable == false);
        }
    }
}

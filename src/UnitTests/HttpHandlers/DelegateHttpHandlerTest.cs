using PowerAssert;
using RouteMagic.HttpHandlers;
using Xunit;

namespace UnitTests.HttpHandlers
{
    public class DelegateHttpHandlerTest
    {
        [Fact]
        public void ProcessRequest_WithAction_CallsAction()
        {
            // arrange
            bool actionCalled = false;
            var handler = new DelegateHttpHandler((c) => actionCalled = true, false);

            // act
            handler.ProcessRequest(null);

            // assert
            PAssert.IsTrue(() => handler.IsReusable == false);
            PAssert.IsTrue(() => actionCalled == true);
        }

        [Fact]
        public void Ctor_WithNullAction_DoesNotThrowException()
        {
            // arrange
            var handler = new DelegateHttpHandler(null, true);

            // act
            handler.ProcessRequest(null);

            // assert
            PAssert.IsTrue(() => handler.IsReusable == true);
        }
    }
}

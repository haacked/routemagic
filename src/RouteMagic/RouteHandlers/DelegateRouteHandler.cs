using System;
using System.Web;
using System.Web.Routing;

namespace RouteMagic.RouteHandlers {
    public class DelegateRouteHandler : IRouteHandler {
        public DelegateRouteHandler(Func<RequestContext, IHttpHandler> action) {
            HttpHandlerAction = action;
        }

        public Func<RequestContext, IHttpHandler> HttpHandlerAction {
            get;
            private set;
        }

        public IHttpHandler GetHttpHandler(RequestContext requestContext) {
            var action = HttpHandlerAction;
            if (action == null) {
                throw new InvalidOperationException("No action specified");
            }

            return action(requestContext);
        }
    }
}

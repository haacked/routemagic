using System;
using System.Web;
using System.Web.Routing;

namespace RouteMagic.HttpHandlers {
    public class DelegateHttpHandler : IHttpHandler {
        private Action<RequestContext> _action;
        private RouteData _routeData;

        public DelegateHttpHandler(Action<RequestContext> action, RouteData routeData, bool isReusable) {
            if (action == null) {
                throw new ArgumentNullException("action");
            }
            if (routeData == null) {
                throw new ArgumentNullException("routeData");
            }

            IsReusable = isReusable;
            _action = action;
            _routeData = routeData;
        }

        public bool IsReusable {
            get;
            private set;
        }

        public void ProcessRequest(HttpContext context) {
            _action(new RequestContext(new HttpContextWrapper(context), _routeData));
        }
    }
}

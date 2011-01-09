using System.Web;

// This class is marked as internal since it does some things that could be incorrect in other contexts.
// But this code is open source so if you need it public, make it so at your own risk.
namespace RouteMagic.Internals {
    internal class ChildHttpContextWrapper : HttpContextBase {
        private HttpContextBase _httpContext;
        private HttpRequestBase _request;

        public ChildHttpContextWrapper(HttpContextBase httpContext, string parentVirtualPath, string parentPath) {
            _httpContext = httpContext;
            _request = new ChildHttpRequestWrapper(httpContext.Request, parentVirtualPath, parentPath);
        }

        public override HttpRequestBase Request {
            get {
                return _request;
            }
        }

        public override void AddError(System.Exception errorInfo) {
            _httpContext.AddError(errorInfo);
        }

        public override System.Exception[] AllErrors {
            get {
                return _httpContext.AllErrors;
            }
        }

        public override HttpApplicationStateBase Application {
            get {
                return _httpContext.Application;
            }
        }

        public override HttpApplication ApplicationInstance {
            get {
                return _httpContext.ApplicationInstance;
            }
            set {
                _httpContext.ApplicationInstance = value;
            }
        }

        public override System.Web.Caching.Cache Cache {
            get {
                return _httpContext.Cache;
            }
        }

        public override void ClearError() {
            _httpContext.ClearError();
        }

        public override IHttpHandler CurrentHandler {
            get {
                return _httpContext.CurrentHandler;
            }
        }

        public override RequestNotification CurrentNotification {
            get {
                return _httpContext.CurrentNotification;
            }
        }

        public override bool Equals(object obj) {
            return _httpContext.Equals(obj);
        }

        public override System.Exception Error {
            get {
                return _httpContext.Error;
            }
        }

        public override object GetGlobalResourceObject(string classKey, string resourceKey) {
            return _httpContext.GetGlobalResourceObject(classKey, resourceKey);
        }

        public override object GetGlobalResourceObject(string classKey, string resourceKey, System.Globalization.CultureInfo culture) {
            return _httpContext.GetGlobalResourceObject(classKey, resourceKey, culture);
        }

        public override int GetHashCode() {
            return _httpContext.GetHashCode();
        }

        public override object GetLocalResourceObject(string virtualPath, string resourceKey) {
            return _httpContext.GetLocalResourceObject(virtualPath, resourceKey);
        }

        public override object GetLocalResourceObject(string virtualPath, string resourceKey, System.Globalization.CultureInfo culture) {
            return _httpContext.GetLocalResourceObject(virtualPath, resourceKey, culture);
        }

        public override object GetSection(string sectionName) {
            return _httpContext.GetSection(sectionName);
        }

        public override object GetService(System.Type serviceType) {
            return _httpContext.GetService(serviceType);
        }

        public override IHttpHandler Handler {
            get {
                return _httpContext.Handler;
            }
            set {
                _httpContext.Handler = value;
            }
        }

        public override bool IsCustomErrorEnabled {
            get {
                return _httpContext.IsCustomErrorEnabled;
            }
        }

        public override bool IsDebuggingEnabled {
            get {
                return _httpContext.IsDebuggingEnabled;
            }
        }

        public override bool IsPostNotification {
            get {
                return _httpContext.IsPostNotification;
            }
        }

        public override System.Collections.IDictionary Items {
            get {
                return _httpContext.Items;
            }
        }

        public override IHttpHandler PreviousHandler {
            get {
                return _httpContext.PreviousHandler;
            }
        }

        public override System.Web.Profile.ProfileBase Profile {
            get {
                return _httpContext.Profile;
            }
        }

        public override void RemapHandler(IHttpHandler handler) {
            _httpContext.RemapHandler(handler);
        }

        public override HttpResponseBase Response {
            get {
                return _httpContext.Response;
            }
        }

        public override void RewritePath(string filePath, string pathInfo, string queryString) {
            _httpContext.RewritePath(filePath, pathInfo, queryString);
        }

        public override void RewritePath(string filePath, string pathInfo, string queryString, bool setClientFilePath) {
            _httpContext.RewritePath(filePath, pathInfo, queryString, setClientFilePath);
        }

        public override void RewritePath(string path) {
            _httpContext.RewritePath(path);
        }

        public override void RewritePath(string path, bool rebaseClientPath) {
            _httpContext.RewritePath(path, rebaseClientPath);
        }

        public override HttpServerUtilityBase Server {
            get {
                return _httpContext.Server;
            }
        }

        public override HttpSessionStateBase Session {
            get {
                return _httpContext.Session;
            }
        }

        public override void SetSessionStateBehavior(System.Web.SessionState.SessionStateBehavior sessionStateBehavior) {
            _httpContext.SetSessionStateBehavior(sessionStateBehavior);
        }

        public override bool SkipAuthorization {
            get {
                return _httpContext.SkipAuthorization;
            }
            set {
                _httpContext.SkipAuthorization = value;
            }
        }

        public override System.DateTime Timestamp {
            get {
                return _httpContext.Timestamp;
            }
        }

        public override string ToString() {
            return _httpContext.ToString();
        }

        public override TraceContext Trace {
            get {
                return _httpContext.Trace;
            }
        }

        public override System.Security.Principal.IPrincipal User {
            get {
                return _httpContext.User;
            }
            set {
                _httpContext.User = value;
            }
        }
    }
}
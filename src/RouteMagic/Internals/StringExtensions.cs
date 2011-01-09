using System;
using System.Web.Routing;

namespace RouteMagic.Internals {
    internal static class StringExtensions {
        public static string WithoutTrailingSlash(this string value) {
            if (value == null) {
                return null;
            }
            if (value.EndsWith("/", StringComparison.Ordinal)) {
                return value.Substring(0, value.Length - 1);
            }
            return value;
        }

        public static string WithoutApplicationPath(this string path, RequestContext requestContext) {
            string applicationPath = requestContext.HttpContext.Request.ApplicationPath;
            if (applicationPath != "/" && path.StartsWith(applicationPath)) {
                return path.Remove(0, applicationPath.Length);
            }
            return path;
        }

        public static string WithoutTildePrefix(this string s) {
            if (s.StartsWith("~")) {
                return s.Substring(1);
            }
            return s;
        }
    }
}

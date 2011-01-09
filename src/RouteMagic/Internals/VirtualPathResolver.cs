using System.Web;

namespace RouteMagic.Internals {
    public class VirtualPathResolver : IVirtualPathResolver {
        public static readonly VirtualPathResolver Instance = new VirtualPathResolver();

        private VirtualPathResolver() {
        }

        public string ToAbsolute(string virtualPath) {
            return VirtualPathUtility.ToAbsolute(virtualPath);
        }
    }
}

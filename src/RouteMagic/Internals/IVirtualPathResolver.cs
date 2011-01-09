
namespace RouteMagic.Internals {
    public interface IVirtualPathResolver {
        string ToAbsolute(string virtualPath);
    }
}

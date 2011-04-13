using System.Web;
using RouteDebug;

[assembly: PreApplicationStartMethod(typeof(PreApplicationStart), "Start")]

namespace RouteDebug {
    public class PreApplicationStart {
        public static void Start() {
            Microsoft.Web.Infrastructure.DynamicModuleHelper.DynamicModuleUtility.RegisterModule(typeof(RouteDebuggerHttpModule));
        }
    }
}

using System.Configuration;
using System.Web;
using RouteDebug;
using System;

[assembly: PreApplicationStartMethod(typeof(PreApplicationStart), "Start")]

namespace RouteDebug {
    public class PreApplicationStart {
        public static void Start() {
            bool enabled = Convert.ToBoolean(ConfigurationManager.AppSettings["RouteDebugger:Enabled"]);
            if (enabled) {
                Microsoft.Web.Infrastructure.DynamicModuleHelper.DynamicModuleUtility.RegisterModule(typeof(RouteDebuggerHttpModule));
            }
        }
    }
}

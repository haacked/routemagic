using System.Linq;
using System.Web;
using System.Web.Routing;

namespace RouteDebug
{
    public class RouteDebuggerHttpModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.EndRequest += OnEndRequest;
            context.BeginRequest += OnBeginRequest;
        }

        static void OnBeginRequest(object sender, System.EventArgs e)
        {
            if (RouteTable.Routes.Last() != DebugRoute.Singleton)
            {
                RouteTable.Routes.Add(DebugRoute.Singleton);
            }
        }

        static void OnEndRequest(object sender, System.EventArgs e)
        {
            var handler = new DebugHttpHandler();

            handler.ProcessRequest(HttpContext.Current);
        }

        public void Dispose()
        {
        }
    }
}

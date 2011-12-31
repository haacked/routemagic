using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using System.Web.Hosting;
using System.Web.Routing;

namespace RouteDebug
{
    public class DebugHttpHandler : IHttpHandler
    {
        readonly VirtualPathProvider _virtualPathProvider;

        public DebugHttpHandler() : this(null) { }

        public DebugHttpHandler(VirtualPathProvider virtualPathProvider)
        {
            _virtualPathProvider = virtualPathProvider ?? HostingEnvironment.VirtualPathProvider;
        }

        public void ProcessRequest(HttpContext context)
        {
            var request = context.Request;

            if (!IsRoutedRequest(request) || !request.ContentType.Equals("text/html", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            string generatedUrlInfo = string.Empty;
            var requestContext = request.RequestContext;

            if (request.QueryString.Count > 0)
            {
                var rvalues = new RouteValueDictionary();
                foreach (string key in request.QueryString.Keys)
                {
                    if (key != null)
                    {
                        rvalues.Add(key, request.QueryString[key]);
                    }
                }

                var vpd = RouteTable.Routes.GetVirtualPath(requestContext, rvalues);
                if (vpd != null)
                {
                    generatedUrlInfo = "<p><label style=\"font-weight: bold; font-size: 1.1em;\">Generated URL</label>: ";
                    generatedUrlInfo += "<strong style=\"color: #00a;\">" + vpd.VirtualPath + "</strong>";
                    var vpdRoute = vpd.Route as Route;
                    if (vpdRoute != null)
                    {
                        generatedUrlInfo += " using the route \"" + vpdRoute.Url + "\"</p>";
                    }
                }
            }

            const string htmlFormat = @"<html>
<div id=""haackroutedebugger"" style=""background-color: #fff;"">
    <style>
        #haackroutedebugger, #haackroutedebugger td, #haackroutedebugger th {{background-color: #fff; font-family: verdana, helvetica, san-serif; font-size: small;}}
        #haackroutedebugger tr.header td, #haackroutedebugger tr.header th {{background-color: #ffc;}}
    </style>
    <hr style=""width: 100%; border: solid 1px #000; margin:0; padding:0;"" />
    <h1 style=""margin: 0; padding: 4px; border-bottom: solid 1px #bbb; padding-left: 10px; font-size: 1.2em; background-color: #ffc;"">Route Debugger</h1>
    <div id=""main"" style=""margin-top:0; padding-top:0"">
        <p style=""font-size: .9em; padding-top:0"">
            Type in a url in the address bar to see which defined routes match it. 
            A {{*catchall}} route is added to the list of routes automatically in 
            case none of your routes match.
        </p>
        <p style=""font-size: .9em;"">
            To generate URLs using routing, supply route values via the query string. example: <code>http://localhost:14230/?id=123</code>
        </p>
        <p><label style=""font-weight: bold; font-size: 1.1em;"">Matched Route</label>: {1}</p>
        {5}
        <div style=""float: left;"">
            <table border=""1"" cellpadding=""3"" cellspacing=""0"" width=""300"">
                <caption style=""font-weight: bold;"">Route Data</caption>
                <tr class=""header""><th>Key</th><th>Value</th></tr>
                {0}
            </table>
        </div>
        <div style=""float: left; margin-left: 10px;"">
            <table border=""1"" cellpadding=""3"" cellspacing=""0"" width=""300"">
                <caption style=""font-weight: bold;"">Data Tokens</caption>
                <tr class=""header""><th>Key</th><th>Value</th></tr>
                {4}
            </table>
        </div>
        <hr style=""clear: both;"" />
        <table border=""1"" cellpadding=""3"" cellspacing=""0"">
            <caption style=""font-weight: bold;"">All Routes</caption>
            <tr class=""header"">
                <th>Matches Current Request</th>
                <th>Url</th>
                <th>Defaults</th>
                <th>Constraints</th>
                <th>DataTokens</th>
            </tr>
            {2}
        </table>
        <hr />
        <h3>Current Request Info</h3>
        <p>
            AppRelativeCurrentExecutionFilePath is the portion of the request that Routing acts on.
        </p>
        <p><strong>AppRelativeCurrentExecutionFilePath</strong>: {3}</p>
    </div>
</div>";
            string routeDataRows = string.Empty;

            RouteData routeData = requestContext.RouteData;
            RouteValueDictionary routeValues = routeData.Values;
            RouteBase matchedRouteBase = routeData.Route;

            string routes = string.Empty;
            using (RouteTable.Routes.GetReadLock())
            {
                foreach (RouteBase routeBase in RouteTable.Routes)
                {
                    bool matchesCurrentRequest = (routeBase.GetRouteData(requestContext.HttpContext) != null);
                    string matchText = string.Format(@"<span{0}>{1}</span>", BoolStyle(matchesCurrentRequest), matchesCurrentRequest);
                    string url = "n/a";
                    string defaults = "n/a";
                    string constraints = "n/a";
                    string dataTokens = "n/a";

                    Route route = CastRoute(routeBase);

                    if (route != null)
                    {
                        url = route.Url;
                        defaults = FormatDictionary(route.Defaults);
                        constraints = FormatDictionary(route.Constraints);
                        dataTokens = FormatDictionary(route.DataTokens);
                    }

                    routes += string.Format(@"<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td></tr>"
                            , matchText
                            , url
                            , defaults
                            , constraints
                            , dataTokens);
                }
            }

            string matchedRouteUrl = "n/a";

            string dataTokensRows = "";

            if (!(matchedRouteBase is DebugRoute))
            {
                foreach (string key in routeValues.Keys)
                {
                    routeDataRows += string.Format("\t<tr><td>{0}</td><td>{1}&nbsp;</td></tr>", key, routeValues[key]);
                }

                foreach (string key in routeData.DataTokens.Keys)
                {
                    dataTokensRows += string.Format("\t<tr><td>{0}</td><td>{1}&nbsp;</td></tr>", key, routeData.DataTokens[key]);
                }

                var matchedRoute = matchedRouteBase as Route;

                if (matchedRoute != null)
                    matchedRouteUrl = matchedRoute.Url;
            }
            else
            {
                matchedRouteUrl = string.Format("<strong{0}>NO MATCH!</strong>", BoolStyle(false));
            }

            context.Response.Write(string.Format(htmlFormat
                , routeDataRows
                , matchedRouteUrl
                , routes
                , request.AppRelativeCurrentExecutionFilePath
                , dataTokensRows
                , generatedUrlInfo));
        }

        private Route CastRoute(RouteBase routeBase)
        {
            var route = routeBase as Route;
            if (route == null)
            {
                // cheat!
                // TODO: Create an interface for self reporting routes.
                var type = routeBase.GetType();
                var property = type.GetProperty("__DebugRoute", BindingFlags.NonPublic | BindingFlags.Instance);
                if (property != null)
                {
                    route = property.GetValue(routeBase, null) as Route;
                }
            }
            return route;
        }

        private static string FormatDictionary(IDictionary<string, object> values)
        {
            if (values == null)
                return "(null)";

            if (values.Count == 0)
            {
                return "(empty)";
            }

            string display = string.Empty;
            foreach (string key in values.Keys)
            {
                display += string.Format("{0} = {1}, ", key, FormatObject(values[key]));
            }
            if (display.EndsWith(", "))
                display = display.Substring(0, display.Length - 2);
            return display;
        }

        private static string FormatObject(object value)
        {
            if (value == null)
            {
                return "(null)";
            }

            var values = value as object[];
            if (values != null)
            {
                return string.Join(", ", values);
            }

            var dictionaryValues = value as IDictionary<string, object>;
            if (dictionaryValues != null)
            {
                return FormatDictionary(dictionaryValues);
            }

            if (value.GetType().Name == "UrlParameter")
            {
                return "UrlParameter.Optional";
            }

            return value.ToString();
        }

        private static string BoolStyle(bool boolean)
        {
            if (boolean) return " style=\"color: #0c0\"";
            return " style=\"color: #c00\"";
        }

        private bool IsRoutedRequest(HttpRequest request)
        {
            string path = request.AppRelativeCurrentExecutionFilePath;
            if (path != "~/" && (_virtualPathProvider.FileExists(path) || _virtualPathProvider.DirectoryExists(path)))
            {
                return false;
            }
            return true;
        }

        public bool IsReusable
        {
            get { return true; }
        }
    }
}

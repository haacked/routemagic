using System.Web;
using System.Web.Routing;

namespace RouteDebug {
    public class DebugHttpHandler : IHttpHandler {
        public bool IsReusable {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context) {
            string generatedUrlInfo = string.Empty;
            var requestContext = HttpContext.Current.Request.RequestContext;

            if (context.Request.QueryString.Count > 0) {
                var rvalues = new RouteValueDictionary();
                foreach (string key in context.Request.QueryString.Keys) {
                    rvalues.Add(key, context.Request.QueryString[key]);
                }

                var vpd = RouteTable.Routes.GetVirtualPath(requestContext, rvalues);
                if (vpd != null) {
                    generatedUrlInfo = "<p><label style=\"font-weight: bold; font-size: 1.1em;\">Generated URL</label>: ";
                    generatedUrlInfo += "<strong style=\"color: #00a;\">" + vpd.VirtualPath + "</strong>";
                    var vpdRoute = vpd.Route as Route;
                    if (vpdRoute != null) {
                        generatedUrlInfo += " using the route \"" + vpdRoute.Url + "\"</p>";
                    }
                }
            }

            string htmlFormat = @"<html>
<head>
    <style>
        #haackroutedebugger, #haackroutedebugger td, #haackroutedebugger th {{background-color: #fff; font-family: verdana, helvetica, san-serif; font-size: small;}}
        #haackroutedebugger tr.header td, #haackroutedebugger tr.header th {{background-color: #ffc;}}
    </style>
</head>
<body id=""haackroutedebugger"" style=""background-color: #fff;"">
<hr style=""width: 100%; border: solid 1px #000;"" />
<h1>Route Debugger</h1>
<div id=""main"">
    <p style=""font-size: .9em;"">
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
</body>
</html>";
            string routeDataRows = string.Empty;

            RouteData routeData = requestContext.RouteData;
            RouteValueDictionary routeValues = routeData.Values;
            RouteBase matchedRouteBase = routeData.Route;

            string routes = string.Empty;
            using (RouteTable.Routes.GetReadLock()) {
                foreach (RouteBase routeBase in RouteTable.Routes) {
                    bool matchesCurrentRequest = (routeBase.GetRouteData(requestContext.HttpContext) != null);
                    string matchText = string.Format(@"<span{0}>{1}</span>", BoolStyle(matchesCurrentRequest), matchesCurrentRequest);
                    string url = "n/a";
                    string defaults = "n/a";
                    string constraints = "n/a";
                    string dataTokens = "n/a";

                    Route route = routeBase as Route;
                    if (route != null) {
                        url = route.Url;
                        defaults = FormatRouteValueDictionary(route.Defaults);
                        constraints = FormatRouteValueDictionary(route.Constraints);
                        dataTokens = FormatRouteValueDictionary(route.DataTokens);
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

            if (!(matchedRouteBase is DebugRoute)) {
                foreach (string key in routeValues.Keys) {
                    routeDataRows += string.Format("\t<tr><td>{0}</td><td>{1}&nbsp;</td></tr>", key, routeValues[key]);
                }

                foreach (string key in routeData.DataTokens.Keys) {
                    dataTokensRows += string.Format("\t<tr><td>{0}</td><td>{1}&nbsp;</td></tr>", key, routeData.DataTokens[key]);
                }

                Route matchedRoute = matchedRouteBase as Route;

                if (matchedRoute != null)
                    matchedRouteUrl = matchedRoute.Url;
            }
            else {
                matchedRouteUrl = string.Format("<strong{0}>NO MATCH!</strong>", BoolStyle(false));
            }

            context.Response.Write(string.Format(htmlFormat
                , routeDataRows
                , matchedRouteUrl
                , routes
                , context.Request.AppRelativeCurrentExecutionFilePath
                , dataTokensRows
                , generatedUrlInfo));
        }

        private static string FormatRouteValueDictionary(RouteValueDictionary values) {
            if (values == null || values.Count == 0)
                return "(null)";

            string display = string.Empty;
            foreach (string key in values.Keys)
                display += string.Format("{0} = {1}, ", key, values[key]);
            if (display.EndsWith(", "))
                display = display.Substring(0, display.Length - 2);
            return display;
        }

        private static string BoolStyle(bool boolean) {
            if (boolean) return " style=\"color: #0c0\"";
            return " style=\"color: #c00\"";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;

namespace RouteBasics
{
    public interface IDebugRoute
    {
		 IRouteHandler RouteHandler
		 {
			 get;
		 }

		 string Url
		 {
			 get;
		 }
		 // Summary:
		 //     Gets or sets a dictionary of expressions that specify valid values for a
		 //     URL parameter.
		 //
		 // Returns:
		 //     An object that contains the parameter names and expressions.
		 RouteValueDictionary Constraints
		 {
			 get;
		 }
		 //
		 // Summary:
		 //     Gets or sets custom values that are passed to the route handler, but which
		 //     are not used to determine whether the route matches a URL pattern.
		 //
		 // Returns:
		 //     An object that contains custom values.
		 RouteValueDictionary DataTokens
		 {
			 get;
			 set;
		 }
		 //
		 // Summary:
		 //     Gets or sets the values to use if the URL does not contain all the parameters.
		 //
		 // Returns:
		 //     An object that contains the parameter names and default values.
		 RouteValueDictionary Defaults
		 {
			 get;
		 }
		 string RouteName
		 {
			 get;
			 set;
		 }
		 RouteData GetRouteData(HttpContextBase httpContextBase);
	 }
}

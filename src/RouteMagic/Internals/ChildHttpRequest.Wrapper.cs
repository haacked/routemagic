using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Security.Authentication.ExtendedProtection;
using System.Security.Principal;
using System.Web;

// This class is marked as internal since it does some things that could be incorrect in other contexts.
// But this code is open source so if you need it public, make it so at your own risk.
namespace RouteMagic.Internals
{
    internal class ChildHttpRequestWrapper : HttpRequestBase
    {
        readonly HttpRequestBase _httpRequest;
        readonly string _path;
        readonly string _appRelativeCurrentExecutionFilePath;

        public ChildHttpRequestWrapper(HttpRequestBase httpRequest, string parentVirtualPath, string parentPath)
        {
            if (!parentVirtualPath.StartsWith("~/"))
            {
                throw new InvalidOperationException("parentVirtualPath must start with ~/");
            }

            if (!httpRequest.AppRelativeCurrentExecutionFilePath.StartsWith(parentVirtualPath, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("This request is not valid for the current path.");
            }

            _path = httpRequest.Path.Remove(0, parentPath.Length);
            _appRelativeCurrentExecutionFilePath = httpRequest.AppRelativeCurrentExecutionFilePath.Remove(1, parentVirtualPath.Length - 1);
            Debug.Assert(_appRelativeCurrentExecutionFilePath == "~" + _path, "AppRelativePath and Path Mismatch!");
            _httpRequest = httpRequest;
        }

        public override string Path
        {
            get
            {
                return _path;
            }
        }

        public override string AppRelativeCurrentExecutionFilePath
        {
            get
            {
                return _appRelativeCurrentExecutionFilePath;
            }
        }

        public override string[] AcceptTypes
        {
            get
            {
                return _httpRequest.AcceptTypes;
            }
        }

        public override string AnonymousID
        {
            get
            {
                return _httpRequest.AnonymousID;
            }
        }

        public override string ApplicationPath
        {
            get
            {
                return _httpRequest.ApplicationPath;
            }
        }

        public override byte[] BinaryRead(int count)
        {
            return _httpRequest.BinaryRead(count);
        }

        public override HttpBrowserCapabilitiesBase Browser
        {
            get
            {
                return _httpRequest.Browser;
            }
        }

        public override HttpClientCertificate ClientCertificate
        {
            get
            {
                return _httpRequest.ClientCertificate;
            }
        }

        public override System.Text.Encoding ContentEncoding
        {
            get
            {
                return _httpRequest.ContentEncoding;
            }
            set
            {
                _httpRequest.ContentEncoding = value;
            }
        }

        public override int ContentLength
        {
            get
            {
                return _httpRequest.ContentLength;
            }
        }

        public override string ContentType
        {
            get
            {
                return _httpRequest.ContentType;
            }
            set
            {
                _httpRequest.ContentType = value;
            }
        }

        public override HttpCookieCollection Cookies
        {
            get
            {
                return _httpRequest.Cookies;
            }
        }

        public override string CurrentExecutionFilePath
        {
            get
            {
                return _httpRequest.CurrentExecutionFilePath;
            }
        }

        public override bool Equals(object obj)
        {
            return _httpRequest.Equals(obj);
        }

        public override string FilePath
        {
            get
            {
                return _httpRequest.FilePath;
            }
        }

        public override HttpFileCollectionBase Files
        {
            get
            {
                return _httpRequest.Files;
            }
        }

        public override System.IO.Stream Filter
        {
            get
            {
                return _httpRequest.Filter;
            }
            set
            {
                _httpRequest.Filter = value;
            }
        }

        public override NameValueCollection Form
        {
            get
            {
                return _httpRequest.Form;
            }
        }

        public override int GetHashCode()
        {
            return _httpRequest.GetHashCode();
        }

        public override NameValueCollection Headers
        {
            get
            {
                return _httpRequest.Headers;
            }
        }

        public override ChannelBinding HttpChannelBinding
        {
            get
            {
                return _httpRequest.HttpChannelBinding;
            }
        }

        public override string HttpMethod
        {
            get
            {
                return _httpRequest.HttpMethod;
            }
        }

        public override System.IO.Stream InputStream
        {
            get
            {
                return _httpRequest.InputStream;
            }
        }

        public override bool IsAuthenticated
        {
            get
            {
                return _httpRequest.IsAuthenticated;
            }
        }

        public override bool IsLocal
        {
            get
            {
                return _httpRequest.IsLocal;
            }
        }

        public override bool IsSecureConnection
        {
            get
            {
                return _httpRequest.IsSecureConnection;
            }
        }

        public override WindowsIdentity LogonUserIdentity
        {
            get
            {
                return _httpRequest.LogonUserIdentity;
            }
        }

        public override int[] MapImageCoordinates(string imageFieldName)
        {
            return _httpRequest.MapImageCoordinates(imageFieldName);
        }

        public override string MapPath(string virtualPath)
        {
            return _httpRequest.MapPath(virtualPath);
        }

        public override string MapPath(string virtualPath, string baseVirtualDir, bool allowCrossAppMapping)
        {
            return _httpRequest.MapPath(virtualPath, baseVirtualDir, allowCrossAppMapping);
        }

        public override NameValueCollection Params
        {
            get
            {
                return _httpRequest.Params;
            }
        }

        public override string PathInfo
        {
            get
            {
                return _httpRequest.PathInfo;
            }
        }

        public override string PhysicalApplicationPath
        {
            get
            {
                return _httpRequest.PhysicalApplicationPath;
            }
        }

        public override string PhysicalPath
        {
            get
            {
                return _httpRequest.PhysicalPath;
            }
        }

        public override NameValueCollection QueryString
        {
            get
            {
                return _httpRequest.QueryString;
            }
        }

        public override string RawUrl
        {
            get
            {
                return _httpRequest.RawUrl;
            }
        }

        public override System.Web.Routing.RequestContext RequestContext
        {
            get
            {
                return _httpRequest.RequestContext;
            }
        }

        public override string RequestType
        {
            get
            {
                return _httpRequest.RequestType;
            }
            set
            {
                _httpRequest.RequestType = value;
            }
        }

        public override void SaveAs(string filename, bool includeHeaders)
        {
            _httpRequest.SaveAs(filename, includeHeaders);
        }

        public override NameValueCollection ServerVariables
        {
            get
            {
                return _httpRequest.ServerVariables;
            }
        }

        public override string this[string key]
        {
            get
            {
                return _httpRequest[key];
            }
        }

        public override string ToString()
        {
            return _httpRequest.ToString();
        }

        public override int TotalBytes
        {
            get
            {
                return _httpRequest.TotalBytes;
            }
        }

        public override Uri Url
        {
            get
            {
                return _httpRequest.Url;
            }
        }

        public override Uri UrlReferrer
        {
            get
            {
                return _httpRequest.UrlReferrer;
            }
        }

        public override string UserAgent
        {
            get
            {
                return _httpRequest.UserAgent;
            }
        }

        public override string UserHostAddress
        {
            get
            {
                return _httpRequest.UserHostAddress;
            }
        }

        public override string UserHostName
        {
            get
            {
                return _httpRequest.UserHostName;
            }
        }

        public override string[] UserLanguages
        {
            get
            {
                return _httpRequest.UserLanguages;
            }
        }

        public override void ValidateInput()
        {
            _httpRequest.ValidateInput();
        }
    }
}
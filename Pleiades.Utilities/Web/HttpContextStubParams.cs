using System.Collections.Generic;
using System.Web;
using System.Security.Principal;
using System.Collections.Specialized;

namespace Pleiades.Framework.TestHelpers.Web
{
    public class HttpContextStubParams
    {
        public HttpContextStubParams()
        {
            Form = new NameValueCollection();
            QueryString = new NameValueCollection();
            Cookies = new HttpCookieCollection();
            ServerVariables = new NameValueCollection();
            Headers = new NameValueCollection();
            IsInRoles = new List<string>();
            SessionVariables = new NameValueCollection();
        }

        // Request Stuff
        public string Url { get; set; }
        public string AppRelativeCurrentExecutionFilePath { get; set; }
        public string HttpMethod { get; set; }
        public NameValueCollection Form { get; set; }
        public NameValueCollection QueryString { get; set; }
        public HttpCookieCollection Cookies { get; set; }
        public NameValueCollection ServerVariables { get; set; }
        public NameValueCollection Headers { get; set; }

        // Session Stuff
        public NameValueCollection SessionVariables { get; set; }

        // IPrincipal Stuff
        public string AuthenticatedName { get; set; }
        public string AuthenticationType { get; set; }
        public bool IsAuthenticated { get; set; }
        public List<string> IsInRoles { get; set; }
    }
}

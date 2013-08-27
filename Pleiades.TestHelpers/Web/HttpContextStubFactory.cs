using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.IO;
using System.Security.Principal;
using System.Web;
using Rhino.Mocks;
using HttpSessionStateBase = System.Web.HttpSessionStateBase;

namespace Pleiades.TestHelpers.Web
{
    /// <summary>
    /// My good enough HttpContext Stub
    /// </summary>
	public static class HttpContextStubFactory
    {
        public static HttpContextBase Create(
                string Url = "http://go.com",
                string AppRelativeCurrentExecutionFilePath = "",
                string HttpMethod = "GET",
			    NameValueCollection Form = null, 
                NameValueCollection QueryString = null,
                HttpCookieCollection Cookies = null,
                NameValueCollection ServerVariables = null,
                NameValueCollection Headers = null, 
                List<string> IsInRoles = null,
                NameValueCollection SessionVariables = null,
                string AuthenticatedName = "",
                string AuthenticationType = "",
                bool IsAuthenticated = false)
        {
            var args = new HttpContextStubParams
            {
                Url = Url,
                AppRelativeCurrentExecutionFilePath = AppRelativeCurrentExecutionFilePath,
                HttpMethod = HttpMethod,
                Form = Form ?? new NameValueCollection(),
                QueryString = QueryString ?? new NameValueCollection(),
                Cookies = Cookies ?? new HttpCookieCollection(),
                ServerVariables = ServerVariables ?? new NameValueCollection(),
                Headers = Headers ?? new NameValueCollection(),
                IsInRoles = IsInRoles ?? new List<string>(),
                SessionVariables = SessionVariables ?? new NameValueCollection(),
                AuthenticatedName = AuthenticatedName,
                AuthenticationType = AuthenticationType,
                IsAuthenticated = IsAuthenticated,
            };

            var httpcontext = MockRepository.GenerateMock<HttpContextBase>();
            httpcontext.Stub(x => x.User).Return(UserStub(args));
            httpcontext.Stub(x => x.Request).Return(HttpRequestStub(args));
            httpcontext.Stub(x => x.Response).Return(HttpResponseStub(args));
            httpcontext.Stub(x => x.Session).Return(HttpSessionStub(args));
            httpcontext.Stub(x => x.Server).Return(HttpServerUtilityMock());
            var items = new Dictionary<string, object>();
            httpcontext.Stub(x => x.Items).Return(items);
            return httpcontext;
        }

        public static HttpRequestBase HttpRequestStub(HttpContextStubParams args)
		{
            var request = MockRepository.GenerateStub<HttpRequestBase>();
            request.Stub(x => x.Url).Return(new Uri(args.Url));
            request.Stub(x => x.RawUrl).Return(args.Url);
            request.Stub(x => x.HttpMethod).Return(args.HttpMethod);
            request.Stub(x => x.AppRelativeCurrentExecutionFilePath).Return(args.AppRelativeCurrentExecutionFilePath);
            request.Stub(x => x.IsAuthenticated).Return(args.IsAuthenticated);
            request.Stub(x => x.Browser).Return(MockRepository.GenerateMock<HttpBrowserCapabilitiesBase>());
            request.Stub(x => x.Form).Return(args.Form);
            request.Stub(x => x.QueryString).Return(args.QueryString);
            request.Stub(x => x.Cookies).Return(args.Cookies);
            request.Stub(x => x.ServerVariables).Return(args.ServerVariables);
            request.Stub(x => x.Headers).Return(args.Headers);

            // Combine QueryString, Form, Cookies and Server Variables into single collection
            NameValueCollection parms = new NameValueCollection(48);
            parms.Add(args.QueryString);
            parms.Add(args.Form);
            for (var i = 0; i < args.Cookies.Count; i++)
            {
                var cookie = args.Cookies.Get(i);
                parms.Add(cookie.Name, cookie.Value);
            }
            parms.Add(args.ServerVariables);

            request.Stub(x => x.Params).Do((Func<NameValueCollection>)(() => parms));            
            return request;
		}

        public static HttpResponseBase HttpResponseStub(HttpContextStubParams args)
		{
            var response = MockRepository.GenerateStub<HttpResponseBase>();
            response.Stub(x => x.OutputStream).Return(new MemoryStream());
            response.Output = new StringWriter();
            
            /**
            response.Stub(x => x.ContentType).PropertyBehavior();
            response.Stub(x => x.StatusCode).PropertyBehavior();
            response.Stub(x => x.RedirectLocation).PropertyBehavior();
            **/

            // Without this, all the Routing Tests will fail to function properly
            response.Stub(x => x.ApplyAppPathModifier(null))
                .Do((Func<string, string>)delegate(string x) { return x; })
                .IgnoreArguments();

            return response;
		}

		public static HttpSessionStateBase HttpSessionStub(HttpContextStubParams args)
		{
            var session = MockRepository.GenerateStub<HttpSessionStateBase>();
            foreach (KeyValuePair<string, string> variable in args.SessionVariables)
                session.Stub(x => x[variable.Key]).Return(variable.Value);

			return session;
		}

		public static HttpServerUtilityBase HttpServerUtilityMock()
		{
            var server = MockRepository.GenerateStub<HttpServerUtilityBase>();
			return server;
		}
        
        public static IPrincipal UserStub(HttpContextStubParams args)
		{
            var identity = MockRepository.GenerateStub<IIdentity>();
            identity.Stub(x => x.AuthenticationType).Return(args.AuthenticationType);
            identity.Stub(x => x.IsAuthenticated).Return(args.IsAuthenticated);
            identity.Stub(x => x.Name).Return(args.AuthenticatedName);
            return new PrincipalStub(args.IsInRoles, identity);
		}

        public class PrincipalStub : IPrincipal
        {
            private List<string> roles;
            private IIdentity identity;

            public PrincipalStub(List<string> roles, IIdentity identity)
            {
                this.roles = roles;
                this.identity = identity;
            }

            public bool IsInRole(string role)
            {
                return roles.Contains(role);
            }

            public IIdentity Identity
            {
                get
                {
                    return identity;
                }
            }
        }
    }
}

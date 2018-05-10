using System.Linq;
using System.Web.Http;
using DAL.Interfaces;
using System.Web.Http.Controllers;
using System.Net.Http;
using System.Collections.Generic;
using System;

namespace Security
{
    public class ClientAuthorizeAttribute : AuthorizeAttribute
    {
        public IDSecurity GetDALInstance()
        {
            bool UseStub = false;
            IDSecurity lobjDSecurity;

            if (System.Configuration.ConfigurationManager.AppSettings["UseStubs"] != null)
                UseStub = bool.Parse(System.Configuration.ConfigurationManager.AppSettings["UseStubs"].ToString());

            if (UseStub)
                lobjDSecurity = new DAL.Stubs.DSecurityStub();
            else
                lobjDSecurity = new DAL.DSecurity();

            return lobjDSecurity;
        }
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (SkipAuthorization(actionContext)) return;

            bool isAuthenticated = false;
            IEnumerable<string> apiKey;
            if (actionContext.Request.Headers.TryGetValues("APIKEY", out apiKey))
            {
                IEnumerable<string> origin;
                string originHost = string.Empty;
                if (actionContext.Request.Headers.TryGetValues("Referrer", out origin))
                    originHost = (new Uri(origin.FirstOrDefault())).Host;

                string controllerName = actionContext.ControllerContext.ControllerDescriptor.ControllerName;
                string actionName = actionContext.ActionDescriptor.ActionName;

                isAuthenticated = (apiKey != null && Authenticate(apiKey.FirstOrDefault(), originHost, controllerName, actionName));
            }

            if (!isAuthenticated)
                HandleUnauthorizedRequest(actionContext);
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateResponse(System.Net.HttpStatusCode.Unauthorized, "Unauthorized usage, Invalid API Key or API Key not found or Access restricted. Please contact API administrator.");
        }

        private bool AuthorizeRequest(HttpActionContext actionContext)
        {
            //Write your code here to perform authorization
            return true;
        }

        private bool Authenticate(string pstrApiKey, string pstrReferrer, string pstrController, string pstrAction)
        {
            return GetDALInstance().AuthenticateClient(pstrApiKey, pstrReferrer, pstrController, pstrAction);
        }

        private static bool SkipAuthorization(HttpActionContext actionContext)
        {
            if (actionContext != null)
            {
                return actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any()
                           || actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any();
            }
            else
                return false;
        }
    }
}

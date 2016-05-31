using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WebAPIAuth
{
    public class HeaderAuthActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            IEnumerable<string> headers;
            if (!actionContext.Request.Headers.TryGetValues("x-auth-header", out headers))
            {
                actionContext.Response = BuildUnauthorizedResponseMessage();
            }
            else if (headers.All(h => h != "secretmessage"))
            {
                actionContext.Response = BuildUnauthorizedResponseMessage();
            }
            else
            {
                base.OnActionExecuting(actionContext);
            }
        }

        private HttpResponseMessage BuildUnauthorizedResponseMessage()
        {
            var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            response.Content = new StringContent(JsonConvert.SerializeObject(new { message = "Unauthorized" }), Encoding.UTF8, "application/json");
            return response;
        }
    }

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.Filters.Add(new HeaderAuthActionFilter());
        }
    }
}

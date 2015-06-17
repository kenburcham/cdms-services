using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using services.Resources;

namespace services
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "RpcAction",
                routeTemplate: "action/{action}/{id}",
                defaults: new { id = RouteParameter.Optional, controller = "Action", action="Get" }
            );
            config.Routes.MapHttpRoute(
                name: "RpcDataAction",
                routeTemplate: "data/{action}/{id}",
                defaults: new { id = RouteParameter.Optional, controller = "DataAction", action = "Get" }
            );
            config.Routes.MapHttpRoute(
                name: "LoginAction",
                routeTemplate: "account/{action}/{id}",
                defaults: new { id = RouteParameter.Optional, controller = "Account", action = "Get" }
            );
            config.Routes.MapHttpRoute(
                name: "ScriptAction",
                routeTemplate: "script/{action}/{id}",
                defaults: new { id = RouteParameter.Optional, controller = "Script", action = "Get" }
            );
            // Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable<T> return type.
            // To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.
            // For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.
            //config.EnableQuerySupport();

            // To disable tracing in your application, please comment out or remove the following line of code
            // For more information, refer to: http://www.asp.net/web-api
            config.EnableSystemDiagnosticsTracing();
            config.Filters.Add(new UnhandledExceptionFilter());

            //uncomment to allow json to be returned directly to browsers
            //config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("text/html"));
        }
    }
}

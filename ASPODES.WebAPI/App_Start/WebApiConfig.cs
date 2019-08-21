using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ASPODES.Database;

using ASPODES.WebAPI.Security;
using System.Web.Http.Dispatcher;
using ASPODES.WebAPI.Filter;

namespace ASPODES.WebAPI
{
    /// <summary>
    /// Web API 配置
    /// </summary>
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务
            var json = config.Formatters.JsonFormatter;
            json.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.MessageHandlers.Add(new AuthorizationMessageHandler());
            config.Filters.Add(new ValidationAttribute());
        }
    }
}

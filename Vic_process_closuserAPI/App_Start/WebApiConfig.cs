using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Vic_process_closuserAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            string urlApi = getUrlAPi();
            // Web API configuration and services
            var cors = new EnableCorsAttribute(getUrlAPi(), "*", "*");
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling 
                = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            GlobalConfiguration.Configuration.Formatters
                .Remove(GlobalConfiguration.Configuration.Formatters.XmlFormatter);
            //config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(
            //config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml"));
            config.EnableCors(cors);
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
        private static string getUrlAPi()
        {
            bool? Isproduction = null;
            string api_url = "";
            try
            {
                if (ConfigurationManager.AppSettings.Get("is_production") == null)
                    Isproduction = null;
                else
                    Isproduction = bool.Parse(ConfigurationManager.AppSettings.Get("is_production"));

                if (Isproduction.HasValue)
                {
                    if (Isproduction.Value)
                        api_url = ConfigurationManager.AppSettings.Get("Api_Url_Prod");
                    else
                        api_url = ConfigurationManager.AppSettings.Get("Api_Url_Dev");
                }
            }
            catch (Exception)
            {

            }

            return api_url;
        }
    }
}

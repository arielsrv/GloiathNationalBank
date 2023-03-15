using GloiathNationalBank.WebApi.Common.ErrorHandling;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace GloiathNationalBank.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API routes
            config.MapHttpAttributeRoutes();
            config.Formatters.Clear();
            config.Formatters.Add(new JsonMediaTypeFormatter());
            config.Services.Replace(typeof(IExceptionHandler), new PassthroughExceptionHandler());
            config.Services.Add(typeof(IExceptionLogger), new SimpleExceptionLogger());
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
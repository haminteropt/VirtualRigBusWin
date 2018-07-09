using Microsoft.Owin.Cors;
using Owin;
using Swashbuckle.Application;
using System.Web.Http;

namespace VirtualKenwoodBusWin
{
    public  class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            HttpConfiguration config = new HttpConfiguration();
            config.EnableSwagger(c => c.SingleApiVersion("v1", "Virtual Kenwood Rig via RigBus")).
                EnableSwaggerUi();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            app.UseWebApi(config);
        }
    }
}

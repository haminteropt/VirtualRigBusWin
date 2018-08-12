using HamBusLib;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Logging;
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
                EnableSwaggerUi(c => c.DisableValidator());
            config.MapHttpAttributeRoutes();

            app.UseWebApi(config);
        }
    }
}

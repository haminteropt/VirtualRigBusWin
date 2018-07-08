using Microsoft.Owin.Cors;
using Owin;
using System.Web.Http;
using Swagger.Net.Application;

namespace OmniRigBus
{
    class Startup
    {

        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);

            // Configure Web API for self-host. 
            var config = new HttpConfiguration();
            config.EnableSwagger(c => c.SingleApiVersion("v1", "VirtualRigBusWin Control OmniRig via RigBus")).
            EnableSwaggerUi();

            config.MapHttpAttributeRoutes();
            app.UseWebApi(config);
        }
    }
}

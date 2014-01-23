using Joker.WebUI.Infrastructure.JsonConfigs;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Joker.WebUI.Startup))]
namespace Joker.WebUI
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
            JsonConfig.RegisterSerializationSettings();
        } 
    }
}
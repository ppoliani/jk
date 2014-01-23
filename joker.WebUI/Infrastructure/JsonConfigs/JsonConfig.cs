using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;

namespace Joker.WebUI.Infrastructure.JsonConfigs
{
    public class JsonConfig
    {
        public static void RegisterSerializationSettings()
        {
            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new SignalRContractResolver();
            var serializer = JsonSerializer.Create(settings);
            GlobalHost.DependencyResolver.Register(typeof(JsonSerializer), () => serializer);
            //JsonUtility.CreateDefaultSerializer().ContractResolver = new SignalRContractResolver();
        }
    }
}
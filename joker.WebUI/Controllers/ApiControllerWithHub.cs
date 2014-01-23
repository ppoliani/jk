using System;
using System.Web.Http;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Joker.WebUI.Controllers
{
    public abstract class ApiControllerWithHub<THub> : Controller
        where THub : IHub
    {
        readonly Lazy<IHubContext> hub = new Lazy<IHubContext>(
            () => GlobalHost.ConnectionManager.GetHubContext<THub>()
        );

        protected IHubContext Hub
        {
            get { return hub.Value; }
        }
    }
}

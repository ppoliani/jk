using System.Web.Mvc;
using AttributeRouting.Web.Mvc;
using Joker.WebUI.Infrastructure;
using Joker.WebUI.Infrastructure.Hubs;

namespace Joker.WebUI.Controllers
{
    public class HomeController : ApiControllerWithHub<GameHub>
    {
        private readonly GameController _gameController;

        public HomeController()
        {
            this._gameController = GameController.Instance;
        }

        [GET("/")]
        public ActionResult Index()
        {
            return View();
        }
    }
}

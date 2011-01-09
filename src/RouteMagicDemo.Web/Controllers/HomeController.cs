using System.Web.Mvc;

namespace RouteMagicDemo.Web.Controllers {
    public class HomeController : Controller {
        public ActionResult Index() {
            return View();
        }

        public string View(int id) {
            return "Viewing id: " + id;
        }

        public string List() {
            return "List";
        }
    }
}

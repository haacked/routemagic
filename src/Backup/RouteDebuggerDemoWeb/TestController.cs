using System.Web.Mvc;

namespace RouteDebug
{
    public class TestController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SomeJsonService()
        {
            return Json(new { Title = "Some title" }, JsonRequestBehavior.AllowGet);
        }
    }
}
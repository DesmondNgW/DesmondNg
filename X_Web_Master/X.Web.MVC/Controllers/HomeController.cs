using System.Web.Mvc;
using X.Web.MVC.Util;

namespace X.Web.MVC.Controllers
{
    public class HomeController : MyControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
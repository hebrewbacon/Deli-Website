using System.Web.Mvc;

namespace ItalianDeli.Controllers {
    public class HomeController : Controller {
        public ActionResult Index()
        {
            return View();
        }

        
        public ActionResult About() {
            ViewBag.Message = "Our story:";

            return View();
        }

        public ActionResult Contact() {
            ViewBag.Message = "Our contact details:";

            return View();
        }
    }
}

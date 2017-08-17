using System.Web.Mvc;

namespace Squidd.Commander.Web.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            var easySender = new EasySender("localhost", 13000);
            ViewBag.Response = easySender.Send("INFO");
            return View();
        }
    }
}
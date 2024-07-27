using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Sales.Controllers
{
    [Area("Sales")]
    public class OrderLinesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

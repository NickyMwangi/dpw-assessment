using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Sales.Controllers
{
    [Area("Sales")]
    public class OrderHeaderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

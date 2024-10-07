using Microsoft.AspNetCore.Mvc;

namespace WorkMateBE.Controllers
{
    public class SalaryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

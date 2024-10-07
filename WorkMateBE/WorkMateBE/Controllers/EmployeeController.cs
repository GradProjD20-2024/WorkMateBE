using Microsoft.AspNetCore.Mvc;

namespace WorkMateBE.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

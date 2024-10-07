using Microsoft.AspNetCore.Mvc;

namespace WorkMateBE.Controllers
{
    public class AttendanceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

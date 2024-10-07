using Microsoft.AspNetCore.Mvc;

namespace WorkMateBE.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

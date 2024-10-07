using Microsoft.AspNetCore.Mvc;

namespace WorkMateBE.Controllers
{
    public class AssetController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

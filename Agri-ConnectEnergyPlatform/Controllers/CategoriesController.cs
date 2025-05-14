using Microsoft.AspNetCore.Mvc;

namespace Agri_ConnectEnergyPlatform.Controllers
{

    public class CategoriesController : Controller
    {
        public IActionResult Index()
        {
            var categories = new List<string>
        {
            "Fruits", "Vegetables", "Green Solutions",
            "Automated Solutions", "Semi-Auto Solutions", "Fertilizer"
        };

            return View(categories);
        }
    }
}
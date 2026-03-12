using Microsoft.AspNetCore.Mvc;

namespace Inventory_List_System.Controllers
{
    public class InventoryControllers : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

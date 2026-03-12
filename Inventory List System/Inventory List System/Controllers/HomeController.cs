using Microsoft.AspNetCore.Mvc;
using Inventory_List_System.Models.Database;
using Inventory_List_System.Models.Repositories.Inventories;

namespace Inventory_List_System.Controllers
{
    public class HomeController : Controller
    {
        private readonly IInventoryRepository _inventoryRepository;

        public HomeController(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

       
        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(string itemName, int quantity, decimal price)
        {
            if (string.IsNullOrEmpty(itemName) || quantity <= 0 || price <= 0)
            {
                ModelState.AddModelError(string.Empty, "Please fill all fields with valid values.");
                return View();
            }

           
            int userId = 1;

            var item = new InventoryItem
            {
                ItemName = itemName,
                Quantity = quantity,
                Price = price,
                DateAdded = DateTime.Now,
                UserId = userId
            };

            _inventoryRepository.AddItem(item);

            
            return RedirectToAction("Index", "Inventory");
        }
    }
}
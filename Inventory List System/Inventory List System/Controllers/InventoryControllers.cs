using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Inventory_List_System.Models.Repositories.Inventories;
using Inventory_List_System.Models.Database;
using System;

[Authorize]
public class InventoryController : Controller
{
    private readonly IInventoryRepository _inventoryRepository;
    public InventoryController(IInventoryRepository inventoryRepository) => _inventoryRepository = inventoryRepository;

    private int? GetUserId() => int.TryParse(User.FindFirst("UserId")?.Value, out var id) ? id : null;

    public IActionResult Index(string search, int page = 1)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        int pageSize = 10;
        var items = _inventoryRepository.GetItemsByUser(userId.Value, search, page, pageSize);

        ViewBag.TotalItems = _inventoryRepository.GetTotalItems(userId.Value, search);
        ViewBag.Page = page;
        ViewBag.PageSize = pageSize;
        ViewBag.Search = search;

        return View(items);
    }

    [HttpPost]
    public IActionResult Create(string itemName, int quantity, decimal price)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        _inventoryRepository.AddItem(new InventoryItem { ItemName = itemName, Quantity = quantity, Price = price, DateAdded = DateTime.Now, UserId = userId.Value });
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Delete(int id)
    {
        var userId = GetUserId();
        var item = _inventoryRepository.GetItemById(id);
        if (item == null || item.UserId != userId) return Unauthorized();

        _inventoryRepository.DeleteItem(id);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Edit(int id, string itemName, int quantity, decimal price)
    {
        var userId = GetUserId();
        var item = _inventoryRepository.GetItemById(id);
        if (item == null || item.UserId != userId) return Unauthorized();

        item.ItemName = itemName; item.Quantity = quantity; item.Price = price;
        _inventoryRepository.UpdateItem(item);
        return RedirectToAction("Index");
    }
}
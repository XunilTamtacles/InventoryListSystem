using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;
using Inventory_List_System.Models.Repositories.Users;
using Inventory_List_System.Models.Database;
using Inventory_List_System.Controllers.helper;

public class AccountController : Controller
{
    private readonly IUserRepository _userRepository;
    public AccountController(IUserRepository userRepository) => _userRepository = userRepository;

    public IActionResult Register() => View();
    [HttpPost]
    public IActionResult Register(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            ModelState.AddModelError("", "Username and password required");
            return View();
        }

        if (_userRepository.UsernameExists(username))
        {
            ModelState.AddModelError("", "Username exists");
            return View();
        }

        _userRepository.AddUser(new User { Username = username, PasswordHash = password });
        return RedirectToAction("Login");
    }

    public IActionResult Login() => View();
    [HttpPost]
    public async Task<IActionResult> Login(string username, string password)
    {
        var user = _userRepository.ValidateUser(username, password);
        if (user == null) { ModelState.AddModelError("", "Invalid credentials"); return View(); }

        var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Username), new Claim("UserId", user.Id.ToString()) };
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)));
        return RedirectToAction("Index", "Inventory");
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }
}
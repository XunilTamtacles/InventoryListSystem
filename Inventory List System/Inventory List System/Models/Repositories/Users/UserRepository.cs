using Inventory_List_System.Controllers.helper;
using Inventory_List_System.Models.Database;
using Inventory_List_System.Models.Repositories.Users;

public class UserRepository : IUserRepository
{
    private readonly InventoryDbContext _context;

    public UserRepository(InventoryDbContext context) => _context = context;

    public User GetByUsername(string username)
    {
        return _context.Users.FirstOrDefault(u => u.Username == username);
    }

    public bool UsernameExists(string username) => _context.Users.Any(u => u.Username == username);

    public void AddUser(User user)
    {
        user.PasswordHash = SecurityHelper.HashPassword(user.PasswordHash);
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public User ? ValidateUser(string username, string password)
    {
        var user = _context.Users.FirstOrDefault(u => u.Username == username);
        return user != null && SecurityHelper.VerifyPassword(password, user.PasswordHash) ? user : null;
    }
}
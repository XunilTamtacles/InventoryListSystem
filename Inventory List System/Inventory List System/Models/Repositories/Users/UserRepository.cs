using Inventory_List_System.Models.Database;

namespace Inventory_List_System.Models.Repositories.Users
{

    public class UserRepository : IUserRepository
    {
        private readonly InventoryDbContext _context;

        public UserRepository(InventoryDbContext context)
        {
            _context = context;
        }

        public User GetByUsername(string username)
            => _context.Users.FirstOrDefault(u => u.Username == username);

        public bool UsernameExists(string username)
            => _context.Users.Any(u => u.Username == username);

        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public User ValidateUser(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user != null && PasswordHelper.VerifyPassword(password, user.PasswordHash))
                return user;
            return null;
        }
    }
}


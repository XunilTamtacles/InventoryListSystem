using Inventory_List_System.Models.Database;

namespace Inventory_List_System.Models.Repositories.Users
{
    public interface IUserRepository
    {
        User GetByUsername(string username);
        bool UsernameExists(string username);
        void AddUser(User user);
        User ValidateUser(string username, string passwordHash);
    }
}

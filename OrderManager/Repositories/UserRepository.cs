using OrderManager.Database;
using OrderManager.Database.Models;
using OrderManager.Models.ENUMs;
using System.Security.Cryptography;
using System.Text;

namespace OrderManager.Repositories
{
    public interface IUserRepository
    {
        User? Add (string name, string username, string password, string role);
        User? FindByUsername (string username);
        bool ValidatePassword (int userId, string password);
    }
        
    public class UserRepository : IUserRepository
    {
        private readonly OrderDbContext _context;

        public UserRepository (OrderDbContext context)
        {
            _context = context;
        }

        public User? Add (string name, string username, string password, string role)
        {
            var salt = GenerateSalt();

            var user = new User
            {
                Name = username,
                PasswordSalt = salt,
                PasswordHash = HashPassword(password, salt),
                Role = Enum.Parse<Role>(role, true)
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }

        public User? FindByUsername (string username) {
            return _context.Users.FirstOrDefault(u => u.Username == username);
        }

        public bool ValidatePassword (int userId, string password)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == userId);

            if (user is null) return false;

            return user.PasswordHash == HashPassword(password, user.PasswordSalt);
        }

        private static string GenerateSalt ()
        {
            var bytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }
            return Convert.ToBase64String(bytes);
        }

        private static string HashPassword (string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var combined = Encoding.UTF8.GetBytes(password + salt);
                var hash = sha256.ComputeHash(combined);
                return Convert.ToBase64String(hash);
            }
        }
    }
}

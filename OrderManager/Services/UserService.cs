using OrderManager.Database.Models;
using OrderManager.Models;
using OrderManager.Repositories;

namespace OrderManager.Services
{
    public interface IUserService
    {
        ServiceResponse<User> Signup (string name, string username, string password, string role);
        ServiceResponse<User> Validar (string username, string password);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public ServiceResponse<User> Signup (string name, string username, string password, string role)
        {
            var roles = new[] { "admin", "manager", "vendor" };

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || !roles.Contains(role.ToLower()))
            {
                return ServiceResponse<User>.Error("Validation error. Please provide a valid name, username, password, and role.");
            }

            if (_userRepository.FindByUsername(username) != null)
            {
                return ServiceResponse<User>.Error("Username already exists.");
            }

            var createdUser = _userRepository.Add(name, username, password, role);

            return createdUser != null
                ? ServiceResponse<User>.Ok(createdUser, "User created successfully.")
                : ServiceResponse<User>.Error("Failed to create user.");
        }

        public ServiceResponse<User> Validar (string username, string password)
        {
            var user = _userRepository.FindByUsername(username);

            if (user is null)
                return ServiceResponse<User>.Error("Usuário ou senha inválidos");

            var validUser = _userRepository.ValidatePassword(user.Id, password);

            if (!validUser)
                return ServiceResponse<User>.Error("Usuário ou senha inválidos");

            return ServiceResponse<User>.Ok(user);
        }
    }
}

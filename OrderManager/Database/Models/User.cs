using OrderManager.Models.ENUMs;
using System.ComponentModel.DataAnnotations;

namespace OrderManager.Database.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public Role Role { get; set; }

        [Required]
        public string PasswordSalt { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;
    }
}

using Microsoft.EntityFrameworkCore;
using OrderManager.Database.Models;

namespace OrderManager.Database
{
    public class OrderDbContext (DbContextOptions<OrderDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
    }
}

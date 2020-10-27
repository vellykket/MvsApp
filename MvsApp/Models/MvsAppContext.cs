using Microsoft.EntityFrameworkCore;

namespace MvsApplication.Models
{
    public class MvsAppContext : DbContext
    {
        public MvsAppContext(DbContextOptions<MvsAppContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }
}
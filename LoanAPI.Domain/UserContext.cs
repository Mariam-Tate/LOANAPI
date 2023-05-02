using LoanAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace LoanAPI.Domain
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Loan> Loans { get; set; }
    }
}

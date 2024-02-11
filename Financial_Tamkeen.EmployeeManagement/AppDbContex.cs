using Financial_Tamkeen.EmployeeManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace Financial_Tamkeen.EmployeeManagement
{
    public class AppDbContex: DbContext
    {
        public AppDbContex(DbContextOptions<AppDbContex> options)
        : base(options)
        {

        }

        public DbSet<Employee> Employee { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}

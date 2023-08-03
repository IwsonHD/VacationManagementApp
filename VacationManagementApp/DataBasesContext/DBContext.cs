using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VacationManagementApp.Models;

namespace VacationManagementApp.DataBases
{
    public class VacationManagerDbContext: IdentityDbContext<User>
    {
        public VacationManagerDbContext(DbContextOptions<VacationManagerDbContext> options) : base(options)
        {
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Employer> Employers { get; set; }
        public DbSet<Vacation> Vacations { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>().ToTable("Employees");
            modelBuilder.Entity<Employee>().HasBaseType<User>();

            modelBuilder.Entity<Employer>().ToTable("Employers");
            modelBuilder.Entity<Employer>().HasBaseType<User>();
        }



    }

    

}

using ASP.NETCoreD06.Models;
using Microsoft.EntityFrameworkCore;

namespace ASP.NETCoreD06.Data.Context
{
    public class AppDbContext : DbContext
    {
        /*------------------------------------------------------------------*/
        public AppDbContext() { }
        /*------------------------------------------------------------------*/
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        /*------------------------------------------------------------------*/
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = "Server=MOHAMED-HATEM\\SQLEXPRESS;DataBase=SDASPNETCoreD06;Trusted_Connection=true;TrustServerCertificate=true";
            optionsBuilder.UseSqlServer(connectionString);
        }
        /*------------------------------------------------------------------*/
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seeding
            var _departments = new List<Department>()
            {
                new Department {Id = 1, Name = "SD" },
                new Department {Id = 2, Name = "UI" },
                new Department {Id = 3, Name = "Mob" },
                new Department {Id = 4, Name = "UX" }
            };

            var _employees = new List<Employee>()
            {
                new Employee { Id = 1, Name = "Ahmed", Age = 26 , Salary = 1234, DepartmentId = 1 },
                new Employee { Id = 2, Name = "Mohamed", Age = 36 , Salary = 2234, DepartmentId = 2 },
                new Employee { Id = 3, Name = "Sara", Age = 46 , Salary = 4234, DepartmentId = 3 },
                new Employee { Id = 4, Name = "Omar", Age = 25 , Salary = 5234, DepartmentId = 4 },
                new Employee { Id = 5, Name = "Ali", Age = 23 , Salary = 6234, DepartmentId = 1 },
                new Employee { Id = 6, Name = "Mai", Age = 36 , Salary = 7234, DepartmentId = 2 },
                new Employee { Id = 7, Name = "Ramy", Age = 49 , Salary = 8234, DepartmentId = 3 },
                new Employee { Id = 8, Name = "Hamada", Age = 18 , Salary = 9234, DepartmentId = 4 },
                new Employee { Id = 9, Name = "Hatem", Age = 26 , Salary = 10234, DepartmentId = 1 },
                new Employee { Id = 10, Name = "Osama", Age = 25 , Salary = 17234, DepartmentId = 2 },
            };

            modelBuilder.Entity<Department>().HasData(_departments);
            modelBuilder.Entity<Employee>().HasData(_employees);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
        /*------------------------------------------------------------------*/
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        /*------------------------------------------------------------------*/
    }
}

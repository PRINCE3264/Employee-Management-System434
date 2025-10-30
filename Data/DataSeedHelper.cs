

using EmployeeManagement22.Entity;
using EmployeeManagement22.Service;

namespace EmployeeManagement22.Data
{
    public class DataSeedHelper
    {
        private readonly AppDbContext dbContext;

        public DataSeedHelper(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void InsertData()
        {
            var passwordHelper = new PasswordHelper();

            if (!dbContext.Employees.Any())
            {
                dbContext.Employees.AddRange(
                    new Employee { Name = "Prince Vidyarthi" },
                    new Employee { Name = "Prince Vidyarthi2" }
                );
            }

            if (!dbContext.Users.Any())
            {
                dbContext.Users.AddRange(
                    new User
                    {
                        Email = "admin@gmail.com",
                        Password = passwordHelper.HashPassword("324325"),
                        Role = "Admin"
                    },
                    new User
                    {
                        Email = "employee@gmail.com",
                        Password = passwordHelper.HashPassword("334325"),
                        Role = "Employee"
                    }
                );
            }

            dbContext.SaveChanges();
        }
    }
}

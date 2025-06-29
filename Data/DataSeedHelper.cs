using EmployeeManagement22.Entity;
using Microsoft.EntityFrameworkCore;
using System.Linq; 

namespace EmployeeManagement22.Data
{
    public class DataSeedHelper
    {
        private readonly AppDbContext _dbContext;

        public DataSeedHelper(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void InsertData()
        {
            if (!_dbContext.Employees.Any())
            {
                _dbContext.Employees.Add(new Employee
                {
                    Name = "Prince Vidyarthi"
                });
                _dbContext.Employees.Add(new Employee
                {
                    Name = "Prince Vidyarthi2"
                });

                _dbContext.SaveChanges();
            }
        }
    }
}

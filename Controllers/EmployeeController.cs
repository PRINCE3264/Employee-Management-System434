using EmployeeManagement22.Data;
using EmployeeManagement22.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement22.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public EmployeeController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // ✅ GET: api/Employee
        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = await _dbContext.Employees
                .Include(e => e.Department) // optional: if you want to return department data
                .ToListAsync();

            return Ok(employees);
        }

        // ✅ GET: api/Employee/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            var employee = await _dbContext.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null) return NotFound();

            return Ok(employee);
        }

        // ✅ POST: api/Employee
        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromBody] Employee model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _dbContext.Employees.AddAsync(model);
            await _dbContext.SaveChangesAsync();

            return Ok(model);
        }

        // ✅ PUT: api/Employee/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee([FromRoute]int id, [FromBody] Employee model)
        {
            var existing = await _dbContext.Employees.FindAsync(id);
            if (existing == null) return NotFound();

            // Update properties manually
            existing.Name = model.Name;
            existing.Email = model.Email;
            existing.Phone = model.Phone;
            existing.JobTitle = model.JobTitle;
            existing.Gender = model.Gender;
            existing.DepartmentId = model.DepartmentId;
            existing.JoiningDate = model.JoiningDate;
            existing.LastWorkingDate = model.LastWorkingDate;
            existing.DateOfBirth = model.DateOfBirth;

            await _dbContext.SaveChangesAsync();
            return Ok(existing);
        }

        // ✅ DELETE: api/Employee/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _dbContext.Employees.FindAsync(id);
            if (employee == null) return NotFound();
            _dbContext.Employees.Remove(employee);
            await _dbContext.SaveChangesAsync();
            return Ok(new { message = $"Employee with ID {id} deleted." });

            //return Ok($"Employee with ID {id} deleted.");
        }
    }
}

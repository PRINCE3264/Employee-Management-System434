
//using EmployeeManagement22.Data;
//using EmployeeManagement22.Entity;
//using EmployeeManagement22.Models;
//using EmployeeManagement22.Service;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace EmployeeManagement22.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class EmployeeController : ControllerBase
//    {
//        private readonly AppDbContext _dbContext;
//        private readonly IRepository<User> userRepo;

//        public EmployeeController(AppDbContext dbContext, IRepository<User> userRepo)
//        {
//            _dbContext = dbContext;
//            this.userRepo = userRepo;
//        }

//        // ✅ GET: api/Employee
//        [HttpGet]
//        [Authorize(Roles = "Admin")]
//        public async Task<IActionResult> GetAllEmployees([FromQuery] SearchOptions searchOption)
//        {
//            // var employees = await _dbContext.Employees
//            //.Include(e => e.Department)
//            //.ToListAsync();
//            var filterData = await dbContent.GetAll(x =>
//            x.Name.Contains(searchOption.Search) ||
//            x.Phone.Contains(searchOption.Search) ||
//            x.Email.Contains(searchOption.Search) 

//            );
//            return Ok();
//            //return Ok(employees);
//        }

//        // ✅ GET: api/Employee/{id}
//        [HttpGet("{id}")]
//        [Authorize]
//        public async Task<IActionResult> GetEmployee(int id)
//        {
//            var employee = await _dbContext.Employees
//                .Include(e => e.Department)
//                .FirstOrDefaultAsync(e => e.Id == id);

//            if (employee == null)
//                return NotFound(new { message = $"Employee with ID {id} not found." });

//            return Ok(employee);
//        }

//        // ✅ POST: api/Employee
//        [HttpPost]
//        [Authorize(Roles = "Admin")]
//        public async Task<IActionResult> AddEmployee([FromBody] Employee model)
//        {

//            var user = new User()
//            {
//                Email = model.Email,
//                Role = "Employee",
//                Password = (new PasswordHelper()).HashPassword("12345")
//            };
//            await userRepo.AddAsync(user);
//            model.User = user;
//            if (model == null)
//                return BadRequest(new { message = "Employee data is required." });

//            if (!ModelState.IsValid)
//                return BadRequest(ModelState);

//            // Optionally: Validate Department exists
//            var departmentExists = await _dbContext.Departments.AnyAsync(d => d.Id == model.DepartmentId);
//            if (!departmentExists)
//                return BadRequest(new { message = "Invalid DepartmentId." });

//            await _dbContext.Employees.AddAsync(model);
//            await _dbContext.SaveChangesAsync();

//            return CreatedAtAction(nameof(GetEmployee), new { id = model.Id }, model);
//        }

//        // ✅ PUT: api/Employee/{id}
//        [HttpPut("{id}")]
//        [Authorize(Roles = "Admin")]
//        public async Task<IActionResult> UpdateEmployee([FromRoute] int id, [FromBody] Employee model)
//        {
//            if (model == null)
//                return BadRequest(new { message = "Employee data is required." });

//            var existing = await _dbContext.Employees.FindAsync(id);
//            if (existing == null)
//                return NotFound(new { message = $"Employee with ID {id} not found." });

//            existing.Name = model.Name;
//            existing.Email = model.Email;
//            existing.Phone = model.Phone;
//            existing.JobTitle = model.JobTitle;
//            existing.Gender = model.Gender;
//            existing.DepartmentId = model.DepartmentId;
//            existing.JoiningDate = model.JoiningDate;
//            existing.LastWorkingDate = model.LastWorkingDate;
//            existing.DateOfBirth = model.DateOfBirth;

//            await _dbContext.SaveChangesAsync();
//            return Ok(existing);
//        }

//        // ✅ DELETE: api/Employee/{id}
//        [HttpDelete("{id}")]
//        [Authorize(Roles = "Admin")]
//        public async Task<IActionResult> DeleteEmployee(int id)
//        {
//            var employee = await _dbContext.Employees.FindAsync(id);
//            if (employee == null)
//                return NotFound(new { message = $"Employee with ID {id} not found." });

//            _dbContext.Employees.Remove(employee);
//            await _dbContext.SaveChangesAsync();

//            return Ok(new { message = $"Employee with ID {id} deleted." });
//        }
//    }
//}


using EmployeeManagement22.Data;
using EmployeeManagement22.Entity;
using EmployeeManagement22.Models;
using EmployeeManagement22.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement22.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly IRepository<User> userRepo;

        public EmployeeController(AppDbContext dbContext, IRepository<User> userRepo)
        {
            _dbContext = dbContext;
            this.userRepo = userRepo;
        }


        //[HttpGet]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> GetAllEmployees([FromQuery] SearchOptions searchOption)
        //{
        //    var query = _dbContext.Employees
        //        .Include(e => e.Department) // optional
        //        .AsQueryable();

        //    // ✅ Filter only if search provided
        //    if (!string.IsNullOrWhiteSpace(searchOption.Search))
        //    {
        //        var search = searchOption.Search.ToLower();
        //        query = query.Where(e =>
        //            e.Name.ToLower().Contains(search) ||
        //            e.Email.ToLower().Contains(search) ||
        //            e.Phone.Contains(search)
        //        );
        //    }

        //    // ✅ Apply sorting (optional, like by name)
        //    query = query.OrderBy(e => e.Name);

        //    // ✅ Pagination
        //    if (searchOption.PageIndex.HasValue && searchOption.PageSize.HasValue)
        //    {
        //        int skip = searchOption.PageIndex.Value * searchOption.PageSize.Value;
        //        query = query.Skip(skip).Take(searchOption.PageSize.Value);
        //    }

        //    var result = await query.ToListAsync();
        //    return Ok(result);
        //}

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllEmployees([FromQuery] SearchOptions searchOption)
        {
            var query = _dbContext.Employees.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchOption.Search))
            {
                var search = searchOption.Search.ToLower();
                query = query.Where(e =>
                    e.Name.ToLower().Contains(search) ||
                    e.Email.ToLower().Contains(search) ||
                    e.Phone.Contains(search));
            }

            int totalCount = await query.CountAsync();

            if (searchOption.PageIndex.HasValue && searchOption.PageSize.HasValue)
            {
                int skip = searchOption.PageIndex.Value * searchOption.PageSize.Value;
                query = query.Skip(skip).Take(searchOption.PageSize.Value);
            }

            var employees = await query.ToListAsync();

            return Ok(new
            {
                data = employees,
                totalCount = totalCount
            });
        }


        // ✅ GET: api/Employee/{id}
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetEmployee(int id)
        {
            var employee = await _dbContext.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
                return NotFound(new { message = $"Employee with ID {id} not found." });

            return Ok(employee);
        }

        // ✅ POST: api/Employee
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddEmployee([FromBody] Employee model)
        {
            if (model == null)
                return BadRequest(new { message = "Employee data is required." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // ✅ Validate department exists
            var departmentExists = await _dbContext.Departments.AnyAsync(d => d.Id == model.DepartmentId);
            if (!departmentExists)
                return BadRequest(new { message = "Invalid DepartmentId." });

            // ✅ Create User
            var user = new User
            {
                Email = model.Email,
                Role = "Employee",
                Password = new PasswordHelper().HashPassword("12345")
            };
            await userRepo.AddAsync(user);
            model.User = user;

            await _dbContext.Employees.AddAsync(model);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmployee), new { id = model.Id }, model);
        }

        // ✅ PUT: api/Employee/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateEmployee([FromRoute] int id, [FromBody] Employee model)
        {
            if (model == null)
                return BadRequest(new { message = "Employee data is required." });

            var existing = await _dbContext.Employees.FindAsync(id);
            if (existing == null)
                return NotFound(new { message = $"Employee with ID {id} not found." });

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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _dbContext.Employees.FindAsync(id);
            if (employee == null)
                return NotFound(new { message = $"Employee with ID {id} not found." });

            _dbContext.Employees.Remove(employee);
            await _dbContext.SaveChangesAsync();

            return Ok(new { message = $"Employee with ID {id} deleted." });
        }
    }
}

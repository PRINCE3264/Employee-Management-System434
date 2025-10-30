



using EmployeeManagement22.Data;
using EmployeeManagement22.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace EmployeeManagement22.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IRepository<Department> departmentRepository;
        public DepartmentController(IRepository<Department> departmentRepository)
        {
            this.departmentRepository = departmentRepository;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddDepartment([FromBody] Department model)
        {
            await departmentRepository.AddAsync(model);
            await departmentRepository.SaveChangesAsync();
            return Ok();
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateDepartment([FromRoute] int id, [FromBody] Department model)
        {
            var department = await departmentRepository.FindByIdAsync(id);

            if (department == null)
            {
                return NotFound($"Department with ID {id} not found.");
            }

            department.Name = model.Name;

            await departmentRepository.UpdateAsync(department); // Await this
            return Ok(department); // Return updated data
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllDepartment()
        {
            var list = await departmentRepository.GetAll();
            return Ok(list);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteDepartment([FromRoute] int id)
        {
            var department = await departmentRepository.FindByIdAsync(id);

            if (department == null)
            {
                return NotFound($"Department with ID {id} not found.");
            }

            await departmentRepository.DeleteAsync(id);
            await departmentRepository.SaveChangesAsync();
            return Ok();
        }
    }
}




//using EmployeeManagement22.Data;
//using EmployeeManagement22.Entity;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace EmployeeManagement22.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class DepartmentController : ControllerBase
//    {
//        private readonly IRepository<Department> departmentRepository;
//        private readonly AppDbContext _context;

//        public DepartmentController(IRepository<Department> departmentRepository, AppDbContext context)
//        {
//            this.departmentRepository = departmentRepository;
//            _context = context;
//        }

//        [HttpGet]
//        [Authorize]
//        public async Task<IActionResult> GetAllDepartment()
//        {
//            var list = await departmentRepository.GetAll();
//            return Ok(list);
//        }

//        [HttpPost]
//        [Authorize(Roles = "Admin")]
//        public async Task<IActionResult> AddDepartment([FromBody] Department model)
//        {
//            await departmentRepository.AddAsync(model);
//            await departmentRepository.SaveChangesAsync();
//            return Ok(model);
//        }

//        [HttpPut("{id}")]
//        [Authorize(Roles = "Admin")]
//        public async Task<IActionResult> UpdateDepartment([FromRoute] int id, [FromBody] Department model)
//        {
//            var department = await departmentRepository.FindByIdAsync(id);
//            if (department == null)
//                return NotFound($"Department with ID {id} not found.");

//            department.Name = model.Name;
//            await departmentRepository.UpdateAsync(department);
//            return Ok(department);
//        }

//        [HttpDelete("{id}")]
//        [Authorize(Roles = "Admin")]
//        public async Task<IActionResult> DeleteDepartment([FromRoute] int id)
//        {
//            var department = await departmentRepository.FindByIdAsync(id);
//            if (department == null)
//                return NotFound($"Department with ID {id} not found.");

//            // Prevent deletion if employees are assigned
//            var hasEmployees = await _context.Employees.AnyAsync(e => e.DepartmentId == id);
//            if (hasEmployees)
//            {
//                return BadRequest(new
//                {
//                    message = "❌ Cannot delete department. Employees are still assigned to it."
//                });
//            }

//            await departmentRepository.DeleteAsync(id);
//            await departmentRepository.SaveChangesAsync();

//            return Ok(new { message = "✅ Department deleted successfully." });
//        }
//    }
//}

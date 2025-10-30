//using EmployeeManagement22.Data;
//using EmployeeManagement22.Entity;
//using EmployeeManagement22.Models;
//using EmployeeManagement22.Service;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;

//namespace EmployeeManagement22.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class AttendanceController : ControllerBase
//    {
//        private readonly IRepository<Attendance> attendanceRepo;
//        private readonly UserHelper userHelper;

//        public AttendanceController(IRepository<Attendance> attendanceRepo, UserHelper userHelper)
//        {
//            this.attendanceRepo = attendanceRepo;
//            this.userHelper = userHelper;
//        }

//        // ✅ Get attendance history (Admin = all, Employee = own)
//        [HttpGet]
//        //[Authorize(Roles = "Employee,Admin")]
//        public async Task<IActionResult> GetAttendanceHistory([FromQuery] SearchOptions options)
//        {
//            if (!userHelper.IsAdmin(User))
//            {
//                options.EmployeeId = await userHelper.GetEmployeeIdAsync(User);
//            }

//            var list = await attendanceRepo.GetAll(x =>
//                x.EmployeeId == options.EmployeeId!.Value
//            );

//            var pagedData = new PagedData<Attendance>();
//            pagedData.TotalData = list.Count;

//            if (options.PageIndex.HasValue && options.PageSize.HasValue)
//            {
//                list = list
//                    .Skip(options.PageIndex.Value * options.PageSize.Value)
//                    .Take(options.PageSize.Value)
//                    .ToList();
//            }

//            pagedData.Data = list;

//            return Ok(pagedData);
//        }

//        // ✅ Mark Attendance (for Present)
//        [HttpPost("mark-present")]
//        [Authorize(Roles = "Employee")]
//        public async Task<IActionResult> MarkAttendance()
//        {
//            var employeeId = await userHelper.GetEmployeeIdAsync(User);
//            var attendanceList = await attendanceRepo.GetAll(x =>
//                x.EmployeeId == employeeId &&
//                DateTime.Compare(x.Date.Date, DateTime.UtcNow.Date) == 0
//            );

//            if (attendanceList.Any())
//                return BadRequest("Already marked for today.");

//            var attendance = new Attendance
//            {
//                Date = DateTime.UtcNow,
//                EmployeeId = employeeId.Value,
//                Type = (int)AttendanceType.Present
//            };

//            await attendanceRepo.AddAsync(attendance);
//            await attendanceRepo.SaveChangesAsync();

//            return Ok("Attendance marked successfully.");
//        }
//    }
//}




//// AttendanceController.cs
//using EmployeeManagement22.Data;
//using EmployeeManagement22.Entity;
//using EmployeeManagement22.Models;
//using EmployeeManagement22.Service;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using System.Security.Claims;

//namespace EmployeeManagement22.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class AttendanceController : ControllerBase
//    {
//        private readonly IRepository<Attendance> attendanceRepo;
//        private readonly UserHelper userHelper;

//        public AttendanceController(IRepository<Attendance> attendanceRepo, UserHelper userHelper)
//        {
//            this.attendanceRepo = attendanceRepo;
//            this.userHelper = userHelper;
//        }

//        // ✅ Admin or Employee can view attendance, but employees can only view their own
//        [HttpGet("Employee/{employeeId}")]
//        [Authorize(Roles = "Admin,Employee")]
//        public async Task<IActionResult> GetAttendanceByEmployee(int employeeId, [FromQuery] SearchOptions options)
//        {
//            var currentUserId = await userHelper.GetEmployeeIdAsync(User);

//            if (User.IsInRole("Employee") && currentUserId != employeeId)
//                return Forbid(); // Prevent employee from accessing others' data

//            var list = await attendanceRepo.GetAll(x => x.EmployeeId == employeeId);

//            var pagedData = new PagedData<Attendance>
//            {
//                TotalData = list.Count
//            };

//            if (options.PageIndex.HasValue && options.PageSize.HasValue)
//            {
//                list = list
//                    .Skip(options.PageIndex.Value * options.PageSize.Value)
//                    .Take(options.PageSize.Value)
//                    .ToList();
//            }

//            pagedData.Data = list;
//            return Ok(pagedData);
//        }

//        // ✅ Current logged-in employee's attendance
//        [HttpGet("me")]
//        [Authorize(Roles = "Employee")]
//        public async Task<IActionResult> GetMyAttendance([FromQuery] SearchOptions options)
//        {
//            var employeeId = await userHelper.GetEmployeeIdAsync(User);
//            +return await GetAttendanceByEmployee(employeeId!.Value, options);
//        }

//        // ✅ Mark attendance (present)
//        [HttpPost("mark-present")]
//        [Authorize(Roles = "Employee")]
//        public async Task<IActionResult> MarkAttendance()
//        {
//            var employeeId = await userHelper.GetEmployeeIdAsync(User);
//            var attendanceList = await attendanceRepo.GetAll(x =>
//                x.EmployeeId == employeeId &&
//                DateTime.Compare(x.Date.Date, DateTime.UtcNow.Date) == 0
//            );

//            if (attendanceList.Any())
//                return BadRequest("Already marked for today.");

//            var attendance = new Attendance
//            {
//                Date = DateTime.UtcNow,
//                EmployeeId = employeeId.Value,
//                Type = (int)AttendanceType.Present
//            };

//            await attendanceRepo.AddAsync(attendance);
//            await attendanceRepo.SaveChangesAsync();

//            return Ok("Attendance marked successfully.");
//        }
//    }
//}




using EmployeeManagement22.Data;
using EmployeeManagement22.Entity;
using EmployeeManagement22.Models;
using EmployeeManagement22.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EmployeeManagement22.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly IRepository<Attendance> attendanceRepo;
        private readonly UserHelper userHelper;

        public AttendanceController(IRepository<Attendance> attendanceRepo, UserHelper userHelper)
        {
            this.attendanceRepo = attendanceRepo;
            this.userHelper = userHelper;
        }

        // ✅ Admin or Employee
        [HttpGet("Employee/{employeeId}")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> GetAttendanceByEmployee(int employeeId, [FromQuery] SearchOptions options)
        {
            var currentUserId = await userHelper.GetEmployeeIdAsync(User);
            if (User.IsInRole("Employee") && currentUserId != employeeId)
                return Forbid();

            var list = await attendanceRepo.GetAll(x => x.EmployeeId == employeeId);

            var pagedData = new PagedData<Attendance>
            {
                TotalData = list.Count
            };

            if (options.PageIndex.HasValue && options.PageSize.HasValue)
            {
                list = list
                    .Skip(options.PageIndex.Value * options.PageSize.Value)
                    .Take(options.PageSize.Value)
                    .ToList();
            }

            pagedData.Data = list;
            return Ok(pagedData);
        }

        // ✅ Current Employee's own attendance
        [HttpGet("me")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> GetMyAttendance([FromQuery] SearchOptions options)
        {
            var employeeId = await userHelper.GetEmployeeIdAsync(User);

            if (employeeId == null)
                return Unauthorized("User is not associated with an employee.");

            return await GetAttendanceByEmployee(employeeId.Value, options);
        }

        // ✅ Mark Present
        [HttpPost("mark-present")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> MarkAttendance()
        {
            var employeeId = await userHelper.GetEmployeeIdAsync(User);

            if (employeeId == null)
                return Unauthorized("User is not associated with an employee.");

            var today = DateTime.UtcNow.Date;

            var attendanceList = await attendanceRepo.GetAll(x =>
                x.EmployeeId == employeeId &&
                x.Date.Date == today
            );

            if (attendanceList.Any())
                return BadRequest("Already marked for today.");

            var attendance = new Attendance
            {
                Date = DateTime.UtcNow,
                EmployeeId = employeeId.Value,
                Type = (int)AttendanceType.Present
            };
            await attendanceRepo.AddAsync(attendance);
            await attendanceRepo.SaveChangesAsync();
            return Ok("Attendance marked successfully.");
        }
    }

}



////using EmployeeManagement22.Data;
////using EmployeeManagement22.Entity;
////using EmployeeManagement22.Models;
////using EmployeeManagement22.Service;
////using Microsoft.AspNetCore.Authorization;
////using Microsoft.AspNetCore.Mvc;
////using Microsoft.EntityFrameworkCore;
////using System.Security.Claims;

////namespace EmployeeManagement22.Controllers
////{
////    [Route("api/[controller]")]
////    [ApiController]
////    public class LeaveController : ControllerBase
////    {
////        private readonly IRepository<Leave> leaveRepo;
////        private readonly UserHelper userHelper;
////        private readonly AppDbContext _dbContext;

////        public LeaveController(IRepository<Leave> leaveRepo, UserHelper userHelper, AppDbContext dbContext)
////        {
////            this.leaveRepo = leaveRepo;
////            this.userHelper = userHelper;
////            _dbContext = dbContext;
////        }

////        [HttpPost("apply")]
////        [Authorize(Roles = "Employee")]
////        public async Task<IActionResult> ApplyLeave([FromBody] LeaveDto model)
////        {
////            var employeeId = await userHelper.GetEmployeeIdAsync(User);
////            if (employeeId == null)
////                return Unauthorized("Employee not found.");

////            if (model.Type == null)
////                return BadRequest("Leave type is required.");
////            if (string.IsNullOrWhiteSpace(model.Reason))
////                return BadRequest("Reason is required.");
////            if (model.LeaveDate == null)
////                return BadRequest("Leave date is required.");

////            var leave = new Leave
////            {
////                EmployeeId = employeeId.Value,
////                Reason = model.Reason,
////                Type = model.Type.Value,
////                LeaveDate = model.LeaveDate.Value,
////                Status = (int)LeaveStatus.Pending
////            };

////            await leaveRepo.AddAsync(leave);
////            await leaveRepo.SaveChangesAsync();
////            return Ok();
////        }

////        [HttpPost("updatestatus")]
////        [Authorize(Roles = "Employee,Admin")]
////        public async Task<IActionResult> UpdateLeaveStatus([FromBody] LeaveDto model)
////        {
////            if (model.Id == null)
////                return BadRequest("Leave ID is required.");

////            var leave = await leaveRepo.FindByIdAsync(model.Id.Value);
////            if (leave == null)
////                return NotFound("Leave not found.");

////            if (model.Status == null)
////                return BadRequest("Status is required.");

////            var isAdmin = userHelper.IsAdmin(User);

////            if (isAdmin)
////            {
////                leave.Status = model.Status.Value;
////            }
////            else
////            {
////                if (model.Status == (int)LeaveStatus.Cancelled) // ✅ fixed typo
////                {
////                    leave.Status = model.Status.Value;
////                }
////                else
////                {
////                    return BadRequest("Only cancel is allowed by employee.");
////                }
////            }

////            await leaveRepo.SaveChangesAsync();
////            return Ok();
////        }

////        [HttpGet]
////        [Authorize(Roles = "Employee,Admin")]
////        public async Task<IActionResult> GetAllLeaves([FromQuery] SearchOptions searchOption)
////        {
////            var query = _dbContext.Leaves
////                .Include(l => l.Employee)
////                .AsQueryable();

////            // ✅ Optional search filter
////            if (!string.IsNullOrWhiteSpace(searchOption.Search))
////            {
////                var search = searchOption.Search.ToLower();
////                query = query.Where(l =>
////                    l.Reason.ToLower().Contains(search) ||
////                    l.Employee.Name.ToLower().Contains(search));
////            }

////            // ✅ Total count before pagination
////            var totalCount = await query.CountAsync();

////            // ✅ Apply pagination
////            if (searchOption.PageIndex.HasValue && searchOption.PageSize.HasValue)
////            {
////                int skip = searchOption.PageIndex.Value * searchOption.PageSize.Value;
////                query = query.Skip(skip).Take(searchOption.PageSize.Value);
////            }

////            // ✅ Get final list
////            var leaves = await query.ToListAsync();

////            // ✅ Return paged result
////            return Ok(new
////            {
////                data = leaves,
////                totalCount
////            });
////        }

////    }
////}




//using EmployeeManagement22.Data;
//using EmployeeManagement22.Entity;
//using EmployeeManagement22.Models;
//using EmployeeManagement22.Service;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System.Security.Claims;

//namespace EmployeeManagement22.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class LeaveController : ControllerBase
//    {
//        private readonly IRepository<Leave> leaveRepo;
//        private readonly UserHelper userHelper;
//        private readonly AppDbContext _dbContext;

//        public LeaveController(IRepository<Leave> leaveRepo, UserHelper userHelper, AppDbContext dbContext)
//        {
//            this.leaveRepo = leaveRepo;
//            this.userHelper = userHelper;
//            _dbContext = dbContext;
//        }

//        // ✅ Apply for a new leave
//        [HttpPost("apply")]
//        [Authorize(Roles = "Employee")]
//        public async Task<IActionResult> ApplyLeave([FromBody] LeaveDto model)
//        {
//            var employeeId = await userHelper.GetEmployeeIdAsync(User);
//            if (employeeId == null) return Unauthorized("Employee not found.");

//            if (model.Type == null) return BadRequest("Leave type is required.");
//            if (string.IsNullOrWhiteSpace(model.Reason)) return BadRequest("Reason is required.");
//            if (model.LeaveDate == null) return BadRequest("Leave date is required.");

//            var leave = new Leave
//            {
//                EmployeeId = employeeId.Value,
//                Reason = model.Reason,
//                Type = model.Type.Value,
//                LeaveDate = model.LeaveDate.Value,
//                Status = (int)LeaveStatus.Pending
//            };

//            await leaveRepo.AddAsync(leave);
//            await leaveRepo.SaveChangesAsync();
//            return Ok();
//        }

//        // ✅ Cancel a leave (Employee only)
//        [HttpPut("cancel/{id}")]
//        [Authorize(Roles = "Employee")]
//        public async Task<IActionResult> Cancel(int id)
//        {
//            var employeeId = await userHelper.GetEmployeeIdAsync(User);
//            var leave = await leaveRepo.FindByIdAsync(id);

//            if (leave == null || leave.EmployeeId != employeeId)
//                return NotFound("Leave not found or not your leave.");

//            if (leave.Status != (int)LeaveStatus.Pending)
//                return BadRequest("Only pending leave can be cancelled.");

//            leave.Status = (int)LeaveStatus.Cancelled;
//            await leaveRepo.SaveChangesAsync();
//            return NoContent();
//        }

//        // ✅ Accept a leave (Admin only)
//        [HttpPut("accept/{id}")]
//        [Authorize(Roles = "Admin")]
//        public async Task<IActionResult> Accept(int id)
//        {
//            var leave = await leaveRepo.FindByIdAsync(id);
//            if (leave == null) return NotFound("Leave not found.");

//            if (leave.Status != (int)LeaveStatus.Pending)
//                return BadRequest("Only pending leave can be accepted.");

//            leave.Status = (int)LeaveStatus.Accepted;
//            await leaveRepo.SaveChangesAsync();
//            return NoContent();
//        }

//        // ✅ Reject a leave (Admin only)
//        [HttpPut("reject/{id}")]
//        [Authorize(Roles = "Admin")]
//        public async Task<IActionResult> Reject(int id)
//        {
//            var leave = await leaveRepo.FindByIdAsync(id);
//            if (leave == null) return NotFound("Leave not found.");

//            if (leave.Status != (int)LeaveStatus.Pending)
//                return BadRequest("Only pending leave can be rejected.");

//            leave.Status = (int)LeaveStatus.Rejected;
//            await leaveRepo.SaveChangesAsync();
//            return NoContent();
//        }

//        // ✅ Update leave status (Shared logic — optional)
//        [HttpPost("updatestatus")]
//        [Authorize(Roles = "Employee,Admin")]
//        public async Task<IActionResult> UpdateLeaveStatus([FromBody] LeaveDto model)
//        {
//            if (model.Id == null) return BadRequest("Leave ID is required.");
//            if (model.Status == null) return BadRequest("Status is required.");

//            var leave = await leaveRepo.FindByIdAsync(model.Id.Value);
//            if (leave == null) return NotFound("Leave not found.");

//            var isAdmin = userHelper.IsAdmin(User);

//            if (isAdmin)
//            {
//                leave.Status = model.Status.Value;
//            }
//            else
//            {
//                if (model.Status == (int)LeaveStatus.Cancelled)
//                {
//                    leave.Status = model.Status.Value;
//                }
//                else
//                {
//                    return BadRequest("Only cancellation is allowed by employee.");
//                }
//            }

//            await leaveRepo.SaveChangesAsync();
//            return Ok();
//        }

//        // ✅ Get all leaves (Admin & Employee)
//        [HttpGet]
//        [Authorize(Roles = "Employee,Admin")]
//        public async Task<IActionResult> GetAllLeaves([FromQuery] SearchOptions searchOption)
//        {
//            var query = _dbContext.Leaves
//                .Include(l => l.Employee)
//                .AsQueryable();

//            if (!string.IsNullOrWhiteSpace(searchOption.Search))
//            {
//                var search = searchOption.Search.ToLower();
//                query = query.Where(l =>
//                    l.Reason.ToLower().Contains(search) ||
//                    l.Employee.Name.ToLower().Contains(search));
//            }

//            var totalCount = await query.CountAsync();

//            if (searchOption.PageIndex.HasValue && searchOption.PageSize.HasValue)
//            {
//                int skip = searchOption.PageIndex.Value * searchOption.PageSize.Value;
//                query = query.Skip(skip).Take(searchOption.PageSize.Value);
//            }

//            var leaves = await query.ToListAsync();

//            return Ok(new
//            {
//                data = leaves,
//                totalCount
//            });
//        }
//    }
//}






//using EmployeeManagement22.Data;
//using EmployeeManagement22.Entity;
//using EmployeeManagement22.Models;
//using EmployeeManagement22.Service;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System.Security.Claims;

//namespace EmployeeManagement22.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class LeaveController : ControllerBase
//    {
//        private readonly IRepository<Leave> leaveRepo;
//        private readonly UserHelper userHelper;
//        private readonly AppDbContext _dbContext;

//        public LeaveController(IRepository<Leave> leaveRepo, UserHelper userHelper, AppDbContext dbContext)
//        {
//            this.leaveRepo = leaveRepo;
//            this.userHelper = userHelper;
//            _dbContext = dbContext;
//        }

//        [HttpPost("apply")]
//        [Authorize(Roles = "Employee")]
//        public async Task<IActionResult> ApplyLeave([FromBody] LeaveDto model)
//        {
//            var employeeId = await userHelper.GetEmployeeIdAsync(User);
//            if (employeeId == null)
//                return Unauthorized("Employee not found.");

//            if (model.Type == null)
//                return BadRequest("Leave type is required.");
//            if (string.IsNullOrWhiteSpace(model.Reason))
//                return BadRequest("Reason is required.");
//            if (model.LeaveDate == null)
//                return BadRequest("Leave date is required.");

//            var leave = new Leave
//            {
//                EmployeeId = employeeId.Value,
//                Reason = model.Reason,
//                Type = model.Type.Value,
//                LeaveDate = model.LeaveDate.Value,
//                Status = (int)LeaveStatus.Pending
//            };

//            await leaveRepo.AddAsync(leave);
//            await leaveRepo.SaveChangesAsync();
//            return Ok();
//        }

//        [HttpPost("updatestatus")]
//        [Authorize(Roles = "Employee,Admin")]
//        public async Task<IActionResult> UpdateLeaveStatus([FromBody] LeaveDto model)
//        {
//            if (model.Id == null)
//                return BadRequest("Leave ID is required.");

//            var leave = await leaveRepo.FindByIdAsync(model.Id.Value);
//            if (leave == null)
//                return NotFound("Leave not found.");

//            if (model.Status == null)
//                return BadRequest("Status is required.");

//            var isAdmin = userHelper.IsAdmin(User);

//            if (isAdmin)
//            {
//                leave.Status = model.Status.Value;
//            }
//            else
//            {
//                if (model.Status == (int)LeaveStatus.Cancelled) // ✅ fixed typo
//                {
//                    leave.Status = model.Status.Value;
//                }
//                else
//                {
//                    return BadRequest("Only cancel is allowed by employee.");
//                }
//            }

//            await leaveRepo.SaveChangesAsync();
//            return Ok();
//        }

//        [HttpGet]
//        [Authorize(Roles = "Employee,Admin")]
//        public async Task<IActionResult> GetAllLeaves([FromQuery] SearchOptions searchOption)
//        {
//            var query = _dbContext.Leaves
//                .Include(l => l.Employee)
//                .AsQueryable();

//            // ✅ Optional search filter
//            if (!string.IsNullOrWhiteSpace(searchOption.Search))
//            {
//                var search = searchOption.Search.ToLower();
//                query = query.Where(l =>
//                    l.Reason.ToLower().Contains(search) ||
//                    l.Employee.Name.ToLower().Contains(search));
//            }

//            // ✅ Total count before pagination
//            var totalCount = await query.CountAsync();

//            // ✅ Apply pagination
//            if (searchOption.PageIndex.HasValue && searchOption.PageSize.HasValue)
//            {
//                int skip = searchOption.PageIndex.Value * searchOption.PageSize.Value;
//                query = query.Skip(skip).Take(searchOption.PageSize.Value);
//            }

//            // ✅ Get final list
//            var leaves = await query.ToListAsync();

//            // ✅ Return paged result
//            return Ok(new
//            {
//                data = leaves,
//                totalCount
//            });
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
using System.Security.Claims;

namespace EmployeeManagement22.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveController : ControllerBase
    {
        private readonly IRepository<Leave> leaveRepo;
        private readonly UserHelper userHelper;
        private readonly AppDbContext _dbContext;

        public LeaveController(IRepository<Leave> leaveRepo, UserHelper userHelper, AppDbContext dbContext)
        {
            this.leaveRepo = leaveRepo;
            this.userHelper = userHelper;
            _dbContext = dbContext;
        }

        // ✅ Apply for a new leave
        [HttpPost("apply")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> ApplyLeave([FromBody] LeaveDto model)
        {
            var employeeId = await userHelper.GetEmployeeIdAsync(User);
            if (employeeId == null) return Unauthorized("Employee not found.");

            if (model.Type == null) return BadRequest("Leave type is required.");
            if (string.IsNullOrWhiteSpace(model.Reason)) return BadRequest("Reason is required.");
            if (model.LeaveDate == null) return BadRequest("Leave date is required.");

            var leave = new Leave
            {
                EmployeeId = employeeId.Value,
                Reason = model.Reason,
                Type = model.Type.Value,
                LeaveDate = model.LeaveDate.Value,
                Status = (int)LeaveStatus.Pending
            };

            await leaveRepo.AddAsync(leave);
            await leaveRepo.SaveChangesAsync();
            return Ok();
        }

        // ✅ Cancel a leave (Employee only)
        [HttpPut("cancel/{id}")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Cancel(int id)
        {
            var employeeId = await userHelper.GetEmployeeIdAsync(User);
            var leave = await leaveRepo.FindByIdAsync(id);

            if (leave == null || leave.EmployeeId != employeeId)
                return NotFound("Leave not found or not your leave.");

            if (leave.Status != (int)LeaveStatus.Pending)
                return BadRequest("Only pending leave can be cancelled.");

            leave.Status = (int)LeaveStatus.Cancelled;
            await leaveRepo.SaveChangesAsync();
            return NoContent();
        }

        // ✅ Accept a leave (Admin only)
        [HttpPut("accept/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Accept(int id)
        {
            var leave = await leaveRepo.FindByIdAsync(id);
            if (leave == null) return NotFound("Leave not found.");

            if (leave.Status != (int)LeaveStatus.Pending)
                return BadRequest("Only pending leave can be accepted.");

            leave.Status = (int)LeaveStatus.Accepted;

            // Add attendance entry for accepted leave
            _dbContext.Attendances.Add(new Attendance
            {
                Date = leave.LeaveDate,
                EmployeeId = leave.EmployeeId,
                Type = (int)AttendanceType.Leave
            });

            await leaveRepo.SaveChangesAsync();
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }

        // ✅ Reject a leave (Admin only)
        [HttpPut("reject/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Reject(int id)
        {
            var leave = await leaveRepo.FindByIdAsync(id);
            if (leave == null) return NotFound("Leave not found.");

            if (leave.Status != (int)LeaveStatus.Pending)
                return BadRequest("Only pending leave can be rejected.");

            leave.Status = (int)LeaveStatus.Rejected;

            // Add attendance entry as absent for rejected leave
            _dbContext.Attendances.Add(new Attendance
            {
                Date = leave.LeaveDate,
                EmployeeId = leave.EmployeeId,
                Type = (int)AttendanceType.Absent
            });

            await leaveRepo.SaveChangesAsync();
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }

        // ✅ Shared update logic (optional)
        [HttpPost("updatestatus")]
        [Authorize(Roles = "Employee,Admin")]
        public async Task<IActionResult> UpdateLeaveStatus([FromBody] LeaveDto model)
        {
            if (model.Id == null) return BadRequest("Leave ID is required.");
            if (model.Status == null) return BadRequest("Status is required.");

            var leave = await leaveRepo.FindByIdAsync(model.Id.Value);
            if (leave == null) return NotFound("Leave not found.");

            var isAdmin = userHelper.IsAdmin(User);

            if (isAdmin)
            {
                leave.Status = model.Status.Value;

                if (leave.Status == (int)LeaveStatus.Accepted)
                {
                    _dbContext.Attendances.Add(new Attendance
                    {
                        Date = leave.LeaveDate,
                        EmployeeId = leave.EmployeeId,
                        Type = (int)AttendanceType.Leave
                    });
                }
                else if (leave.Status == (int)LeaveStatus.Rejected)
                {
                    _dbContext.Attendances.Add(new Attendance
                    {
                        Date = leave.LeaveDate,
                        EmployeeId = leave.EmployeeId,
                        Type = (int)AttendanceType.Absent
                    });
                }
            }
            else
            {
                if (model.Status == (int)LeaveStatus.Cancelled)
                {
                    leave.Status = model.Status.Value;
                }
                else
                {
                    return BadRequest("Only cancellation is allowed by employee.");
                }
            }

            await leaveRepo.SaveChangesAsync();
            await _dbContext.SaveChangesAsync();
            return Ok("Leave status updated successfully.");
        }

        // ✅ Get all leaves (Admin & Employee)
        [HttpGet]
        [Authorize(Roles = "Employee,Admin")]
        public async Task<IActionResult> GetAllLeaves([FromQuery] SearchOptions searchOption)
        {
            var query = _dbContext.Leaves
                .Include(l => l.Employee)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchOption.Search))
            {
                var search = searchOption.Search.ToLower();
                query = query.Where(l =>
                    l.Reason.ToLower().Contains(search) ||
                    l.Employee.Name.ToLower().Contains(search));
            }

            var totalCount = await query.CountAsync();

            if (searchOption.PageIndex.HasValue && searchOption.PageSize.HasValue)
            {
                int skip = searchOption.PageIndex.Value * searchOption.PageSize.Value;
                query = query.Skip(skip).Take(searchOption.PageSize.Value);
            }

            var leaves = await query.ToListAsync();

            return Ok(new
            {
                data = leaves,
                totalCount
            });
        }
    }
}

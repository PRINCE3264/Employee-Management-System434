
//using EmployeeManagement22.Entity;
//using System.Security.Claims;
//using EmployeeManagement22.Data;

//namespace EmployeeManagement22.Service
//{
//    public class UserHelper
//    {
//        private readonly IRepository<User> _userRepo;
//        private readonly IRepository<Employee> _empRepo;

//        public UserHelper(IRepository<User> userRepo, IRepository<Employee> empRepo)
//        {
//            _userRepo = userRepo;
//            _empRepo = empRepo;
//        }

//        // ✅ Get User ID from JWT claims
//        public async Task<int?> GetUserIdAsync(ClaimsPrincipal user)
//        {
//            if (user == null || !user.Identity.IsAuthenticated)
//                return null;

//            var email = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
//            if (string.IsNullOrEmpty(email))
//                return null;

//            var dbUser = (await _userRepo.GetAll(u => u.Email == email)).FirstOrDefault();
//            return dbUser?.Id;
//        }

//        // ✅ Get Employee ID linked to User
//        public async Task<int?> GetEmployeeIdAsync(ClaimsPrincipal user)
//        {
//            var userId = await GetUserIdAsync(user);
//            if (userId == null)
//                return null;

//            var employee = (await _empRepo.GetAll(e => e.UserId == userId.Value)).FirstOrDefault();
//            return employee?.Id;
//        }

//        public bool IsAdmin(ClaimsPrincipal userClaim)
//        {
//            var role = userClaim.FindFirstValue(ClaimTypes.Role);
//            return role == "Admin";
//        }
//    }
//}



using EmployeeManagement22.Entity;
using System.Security.Claims;
using EmployeeManagement22.Data;

namespace EmployeeManagement22.Service
{
    public class UserHelper
    {
        private readonly IRepository<User> _userRepo;
        private readonly IRepository<Employee> _empRepo;

        public UserHelper(IRepository<User> userRepo, IRepository<Employee> empRepo)
        {
            _userRepo = userRepo;
            _empRepo = empRepo;
        }

        // ✅ Get User ID from JWT claims
        public async Task<int?> GetUserIdAsync(ClaimsPrincipal user)
        {
            if (user == null || !user.Identity.IsAuthenticated)
                return null;

            var email = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
                return null;

            var dbUser = (await _userRepo.GetAll(u => u.Email == email)).FirstOrDefault();
            return dbUser?.Id;
        }

        // ✅ Get Employee ID linked to User
        public async Task<int?> GetEmployeeIdAsync(ClaimsPrincipal user)
        {
            var userId = await GetUserIdAsync(user);
            if (userId == null)
                return null;

            var employee = (await _empRepo.GetAll(e => e.UserId == userId.Value)).FirstOrDefault();
            return employee?.Id;
        }
        public bool IsAdmin(ClaimsPrincipal userClaim)
        {
            var role = userClaim.FindFirstValue(ClaimTypes.Role);
            return role == "Admin";
        }
    }
}








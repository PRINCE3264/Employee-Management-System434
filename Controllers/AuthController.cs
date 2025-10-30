//using EmployeeManagement22.Data;
//using EmployeeManagement22.Entity;
//using EmployeeManagement22.Models;
//using EmployeeManagement22.Service;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;

//namespace EmployeeManagement22.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class AuthController : ControllerBase
//    {
//        private readonly IRepository<User> _userRepo;
//        private readonly IConfiguration _config;

//        public AuthController(IRepository<User> userRepo, IConfiguration config)
//        {
//            _userRepo = userRepo;
//            _config = config;
//        }

//        [HttpPost("login")]
//        public async Task<IActionResult> Login([FromBody] AuthDto model)
//        {
//            if (!ModelState.IsValid)
//                return BadRequest("Invalid data");

//            var user = (await _userRepo.GetAll(x => x.Email == model.Email)).FirstOrDefault();

//            if (user == null)
//                return Unauthorized("User not found");

//            // ✅ Use PasswordHelper to verify hashed password
//            var passwordHelper = new PasswordHelper();
//            if (!passwordHelper.VerifyPassword(model.Password, user.Password))
//                return Unauthorized("Invalid password");

//            // ✅ Generate JWT Token
//            var token = GenerateToken(user.Email, user.Role);

//            return Ok(new AuthTokenDto
//            {
//                Id = user.Id,
//                Email = user.Email,
//                Token = token,
//                Role = user.Role
//            });
//        }

//        // ✅ Generate secure JWT token with claims
//        private string GenerateToken(string email, string role)
//        {
//            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);

//            if (key.Length < 32)
//                throw new Exception("JWT Key must be at least 32 characters.");

//            var securityKey = new SymmetricSecurityKey(key);
//            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

//            var claims = new[]
//            {
//                new Claim(ClaimTypes.Name, email),
//                new Claim(ClaimTypes.Role, role)
//            };

//            var token = new JwtSecurityToken(
//                issuer: _config["Jwt:Issuer"],
//                audience: _config["Jwt:Audience"],
//                claims: claims,
//                expires: DateTime.UtcNow.AddHours(1),
//                signingCredentials: credentials
//            );

//            return new JwtSecurityTokenHandler().WriteToken(token);
//        }

//    }

//}


using EmployeeManagement22.Data;
using EmployeeManagement22.Entity;
using EmployeeManagement22.Models;
using EmployeeManagement22.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EmployeeManagement22.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IRepository<User> _userRepo;
        private readonly IRepository<Employee> _empRepo;
        private readonly IConfiguration _config;

        public AuthController(IRepository<User> userRepo, IConfiguration config, IRepository<Employee> empRepo)
        {
            _userRepo = userRepo;
            _config = config;
            _empRepo = empRepo;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data");

            var user = (await _userRepo.GetAll(x => x.Email == model.Email)).FirstOrDefault();
            if (user == null)
                return Unauthorized("User not found");

            var passwordHelper = new PasswordHelper();
            if (!passwordHelper.VerifyPassword(model.Password, user.Password))
                return Unauthorized("Invalid password");

            var token = GenerateToken(user.Email, user.Role);

            return Ok(new AuthTokenDto
            {
                Id = user.Id,
                Email = user.Email,
                Token = token,
                Role = user.Role
            });
        }

        private string GenerateToken(string email, string role)
        {
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);

            if (key.Length < 32)
                throw new Exception("JWT Key must be at least 32 characters.");

            var securityKey = new SymmetricSecurityKey(key);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, email), // ✅ Use ClaimTypes.Email
                new Claim(ClaimTypes.Role, role)
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        [Authorize]
        [HttpPost("Profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] ProfileDto model)
        {
            // ✅ Extract email from JWT claim
            var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(emailClaim))
                return Unauthorized("Unauthorized: Email claim missing from token.");

            // ✅ Find user by email
            var user = (await _userRepo.GetAll(x => x.Email == emailClaim)).FirstOrDefault();
            if (user == null)
                return NotFound("User not found");

            // ✅ Update associated employee info
            var employee = (await _empRepo.GetAll(x => x.UserId == user.Id)).FirstOrDefault();
            if (employee != null)
            {
                employee.Name = model.Name;
                employee.Phone = model.Phone;
                employee.Email = model.Email;
                await _empRepo.UpdateAsync(employee); // Use await if UpdateAsync is async
            }

            // ✅ Update user fields
            user.Email = model.Email;
            user.ProfileImage = model.ProfileImage;

            var passwordHelper = new PasswordHelper();
            user.Password = passwordHelper.HashPassword(model.Password);

            await _userRepo.UpdateAsync(user);
            await _userRepo.SaveChangesAsync();

            return Ok(new
            {
                Message = "Profile updated successfully",
                User = new
                {
                    user.Id,
                    user.Email,
                    user.Role,
                    user.ProfileImage
                }
            });
        }
        [Authorize]
        [HttpGet("Profile")]
        public async Task<IActionResult> GetProfile()
        {
            // ✅ Extract email from JWT token
            var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(emailClaim))
                return Unauthorized("Unauthorized: Email claim missing from token.");

            // ✅ Fetch user
            var user = (await _userRepo.GetAll(x => x.Email == emailClaim)).FirstOrDefault();
            if (user == null)
                return NotFound("User not found");

            // ✅ Fetch employee
            var employee = (await _empRepo.GetAll(x => x.UserId == user.Id)).FirstOrDefault();

            // ✅ Return combined result
            return Ok(new ProfileDto
            {
                Name = employee?.Name ?? string.Empty,
                Email = user.Email,
                Phone = employee?.Phone ?? string.Empty,
                ProfileImage = user.ProfileImage ?? string.Empty,
                Password = "" // 🔐 Optional: Avoid returning password
            });
        }

    }
};

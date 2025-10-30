
using Microsoft.AspNetCore.Identity;

namespace EmployeeManagement22.Service
{
    public class PasswordHelper
    {
        private readonly PasswordHasher<string> hasher = new();

        public string HashPassword(string password)
        {
            return hasher.HashPassword(null!, password);
        }

        public bool VerifyPassword(string enteredPassword, string hashedPassword)
        {
            var result = hasher.VerifyHashedPassword(null!, hashedPassword, enteredPassword);
            return result == PasswordVerificationResult.Success;
        }
    }
}

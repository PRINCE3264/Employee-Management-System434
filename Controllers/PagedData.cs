//using EmployeeManagement22.Data;
//using EmployeeManagement22.Entity;
//using EmployeeManagement22.Service;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

using EmployeeManagement22.Entity;

namespace EmployeeManagement22.Controllers
{
    internal class PagedData<T>
    {
        public PagedData()
        {
        }

        public List<Attendance> Data { get; internal set; }
        public int TotalData { get; internal set; }
    }
}
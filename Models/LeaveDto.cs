namespace EmployeeManagement22.Models
{
    public class LeaveDto
    {
        public int? Id { get; set; }
        public int? Type { get; set; }

        public string Reason { get; set; }

        public int? Status { get; set; }

        public DateTime? LeaveDate { get; set; }

        public int? EmployeeId { get; set; }
    }
}
//public class LeaveDto
//{
//    public int? Id { get; set; }
//    public int? Type { get; set; }
//    public string Reason { get; set; }
//    public int? Status { get; set; }
//    public DateTime? LeaveDate { get; set; }
//    public int? EmployeeId { get; set; }
//}

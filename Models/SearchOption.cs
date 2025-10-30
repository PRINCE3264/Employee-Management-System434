namespace EmployeeManagement22.Models
{
    public class SearchOptions
    {
        public string? Search { get; set; }

        public int? PageIndex { get; set; }

        public int? PageSize { get; set; } = 10;

        public int? EmployeeId { get; set; }
    }
}

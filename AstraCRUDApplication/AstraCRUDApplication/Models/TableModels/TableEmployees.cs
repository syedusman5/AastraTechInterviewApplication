namespace AstraCRUDApplication.Models.TableModels
{
    public class Employees
    {
        public int? Id { get; set; }
        public string? EmployeeUniqueId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? DepartmentId { get; set; }
        public string? Location { get; set; }
        public byte? IsActive { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; } 
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; } 
        public DateTime? DeletedAt { get; set; } 
        public string? DeletedBy { get; set; } 
        public byte? IsDeleted { get; set; } = 0;

    }
}

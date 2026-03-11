namespace AstraCRUDApplication.Models.TableModels
{
    public class Department
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Manager { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }
        public byte? IsDeleted { get; set; } = 0;
    }
}

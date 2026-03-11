namespace AstraCRUDApplication.Models.TableModels
{
    public class Users
    {
        public int? Id { get; set; }

        public string? UniqueId { get; set; }

        public string? Name { get; set; }

        public string? EmailId { get; set; }

        public string? Gender { get; set; }
        public int RoleId { get; set; }

        public string? Password { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public byte? IsDeleted { get; set; }
    }
}


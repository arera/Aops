using AOps.Domain.Entities.Common;

namespace AOps.Domain.Entities
{
    public class Orglevels : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Role { get; set; }
        public Guid UserID { get; set; } = new Guid();
        public string Mobile { get; set; } = string.Empty;
        public string Password_hash { get; set; } = string.Empty;
    }
}

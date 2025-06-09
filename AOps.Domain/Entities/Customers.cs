using AOps.Domain.Entities.Common;

namespace AOps.Domain.Entities
{
    public class Customer : BaseEntity
    {
        public Guid UserId { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public Address Address { get; set; } = new Address();
        public string PrimaryMobile { get; set; } = string.Empty;
        public string? SecondaryMobile { get; set; }
        public string GST { get; set; } = string.Empty;
    }
}

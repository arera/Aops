using AOps.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations;

namespace AOps.Domain.Entities
{
    public class Customer : BaseEntity
    {
        [Key]
        public Guid UserId { get; set; } 
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public Address Address { get; set; } = new Address();
        public string PrimaryMobile { get; set; } = string.Empty;
        public string? SecondaryMobile { get; set; }
        public string GST { get; set; } = string.Empty;

        public ICollection<CustomerContract> Contracts { get; set; }
        public virtual CustomerLogin CustomerLogin { get; set; }
    }
}

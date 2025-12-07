using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOps.Domain.Entities
{
    public class CustomerLogin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }  

        [ForeignKey(nameof(Customer))]
        public Guid CustomerId { get; set; } 

        [Required]
        [MaxLength(256)]
        public string PasswordHash { get; set; }

        public DateTime? LastLogin { get; set; }   

        [MaxLength(45)] // IPv6 max length
        public string IpAddress { get; set; }
    }
}

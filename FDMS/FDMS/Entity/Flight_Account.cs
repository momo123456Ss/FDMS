using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FDMS.Entity
{
    [Table("Flight_Account")]
    public class Flight_Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Flight_AccountId { get; set; }
        public int FlightId { get; set; }
        [Required]
        [MaxLength(50)]
        public string AccountEmail { get; set; }
        [ForeignKey("AccountEmail")]
        public Account AccountNavigation { get; set; }
        [ForeignKey("FlightId")]
        public Flight FlightNavigation { get; set; }

    }
}

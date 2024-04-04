using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FDMS.Entity
{
    [Table("Flight")]
    public class Flight
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string FlightId { get; set; }
        [Required]
        [MaxLength(50)]
        public string FlightCode { get; set; }
        [Required]
        [MaxLength(50)]
        public string Route { get; set; }
        [Required]
        [MaxLength(50)]
        public string PointOfLoading { get; set; }
        [Required]
        [MaxLength(50)]
        public string PointOfUnLoading { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        [MaxLength(255)]
        public string? Signature { get; set; }
        public bool? Confirm { get; set; }
        [MaxLength(50)]
        public string? AccountConfirm { get; set; }
        [ForeignKey("AccountConfirm")]
        public Account? AccountNavigation { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FDMS.Entity
{
    [Table("Flight")]
    public class Flight
    {
        public Flight()
        {
            Flight_Accounts = new HashSet<Flight_Account>();
            FlightDocuments = new HashSet<FlightDocument>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FlightId { get; set; }
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
        public int TotalDocuments { get; set; }

        [MaxLength(255)]
        public string? Signature { get; set; }
        public bool? IsConfirm { get; set; }
        [MaxLength(50)]
        public string? AccountConfirm { get; set; }
        [ForeignKey("AccountConfirm")]
        public Account? AccountNavigation { get; set; }
        [JsonIgnore]
        public virtual ICollection<Flight_Account> Flight_Accounts { get; set; }
        [JsonIgnore]
        public virtual ICollection<FlightDocument> FlightDocuments { get; set; }
    }
}

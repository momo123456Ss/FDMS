using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FDMS.Model
{
    public class FlightCreateModel
    {
        [Required]
        [MaxLength(50)]
        public string PointOfLoading { get; set; }
        [Required]
        [MaxLength(50)]
        public string PointOfUnLoading { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }
    }
}

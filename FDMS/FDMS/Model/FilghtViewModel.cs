using System.ComponentModel.DataAnnotations;

namespace FDMS.Model
{
    public class FilghtViewModel
    {
        public int FlightId { get; set; }
        public string FlightNo { get; set; }
        public string Route { get; set; }
        public string PointOfLoading { get; set; }
        public string PointOfUnLoading { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public string TotalDocuments { get; set; }

        public string? Signature { get; set; }
        public bool? IsConfirm { get; set; }
        public string? AccountConfirm { get; set; }
    }
}

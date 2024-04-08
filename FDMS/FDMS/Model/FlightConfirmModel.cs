using System.ComponentModel.DataAnnotations;

namespace FDMS.Model
{
    public class FlightConfirmModel
    {
        public string? Signature { get; set; }
        public bool? IsConfirm { get; set; }
    }
}

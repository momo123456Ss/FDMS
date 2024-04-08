using System.ComponentModel.DataAnnotations;

namespace FDMS.Model
{
    public class FlightDocumentCreateModel
    {
        [Required]
        [MaxLength(100)]
        public string FileName { get; set; }
        [Required]
        public int Version { get; set; }
        [Required]
        [MaxLength(255)]
        public string Note { get; set; }
        public int DocumentTypeId { get; set; }
        public List<int> GroupPermissionIds { get; set; }
        public IFormFile newFile { get; set; }
    }
}

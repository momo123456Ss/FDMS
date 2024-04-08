using System.ComponentModel.DataAnnotations;

namespace FDMS.Model
{
    public class DocumentTypeCreateOrUpdateModel
    {
        [MaxLength(50)]
        public string? Type { get; set; }
        [MaxLength(255)]
        public string? Note { get; set; }
    }
}

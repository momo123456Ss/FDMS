using FDMS.Entity;
using System.ComponentModel.DataAnnotations;

namespace FDMS.Model
{
    public class FlightDocumentViewModel
    {
        public int FlightDocumentId { get; set; }

        [MaxLength(25)]
        public string? FileType { get; set; }
        [MaxLength(100)]
        public string? FileName { get; set; }
        [MaxLength(25)]
        public string? FileSize { get; set; }
        [MaxLength(255)]
        public string? FileUrl { get; set; }
        [MaxLength(255)]
        public string? FileViewUrl { get; set; }

        //-----
        public int DocumentTypeId { get; set; }
        public DocumentTypeViewModel DocumentTypeNavigation { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string Creator { get; set; }
        public AccountViewModel AccountNavigation { get; set; }
        public int Version { get; set; }
        public int VersionPatch { get; set; }
        public string VersionToString { get; set; }
        public string FlightNo { get; set; }
        public string? Note { get; set; }
    }
}

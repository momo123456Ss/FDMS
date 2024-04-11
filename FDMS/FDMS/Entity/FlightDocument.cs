using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FDMS.Entity
{
    [Table("FlightDocument")]
    public class FlightDocument
    {
        public FlightDocument() {
            FlightDocuments = new HashSet<FlightDocument>();
            FlightDocument_GroupPermissions = new HashSet<FlightDocument_GroupPermission>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? FlightDocumentId { get; set; }
        public int Version { get; set; }
        public int VersionPatch {  get; set; }
        [MaxLength(100)]
        public string? Note { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int DocumentTypeId { get; set; }
        public int FlightId { get; set; }
        public string Creator {  get; set; }
        public int? FlightDocumentIdFK { get; set; }


        [ForeignKey("DocumentTypeId")]
        public DocumentType DocumentTypeNavigation { get; set; }
        [ForeignKey("FlightId")]
        public Flight FlightNavigation { get; set; }
        [ForeignKey("Creator")]
        public Account AccountNavigation { get; set; }
        [ForeignKey("FlightDocumentIdFK")]
        public FlightDocument? FlightDocumentNavigation { get; set; }

        //Thông tin File
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

        [JsonIgnore]
        public virtual ICollection<FlightDocument> FlightDocuments { get; set; }
        [JsonIgnore]
        public virtual ICollection<FlightDocument_GroupPermission> FlightDocument_GroupPermissions { get; set; }
    }
}

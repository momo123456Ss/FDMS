using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace FDMS.Entity
{
    [Table("DocumentType")]
    public class DocumentType
    {
        public DocumentType()
        {
            FlightDocuments = new HashSet<FlightDocument>();
            DocumentType_Permissions = new HashSet<DocumentType_Permission>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DocumentTypeId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Type { get; set; }
        [Required]
        [MaxLength(255)]
        public string Note { get; set; }
        public DateTime CreateDate { get; set; }       
        public int TotalGroups { get; set; }
        [Required]
        [MaxLength(50)]
        public string Creator { get; set; }
        [ForeignKey("Creator")]
        public Account AccountNavigation { get; set; }
        [JsonIgnore]
        public virtual ICollection<DocumentType_Permission> DocumentType_Permissions { get; set; }
        [JsonIgnore]
        public virtual ICollection<FlightDocument> FlightDocuments { get; set; }
    }
}

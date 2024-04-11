using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FDMS.Entity
{
    [Table("FlightDocument_GroupPermission")]
    public class FlightDocument_GroupPermission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int FlightDocumentId { get; set; }
        public int GroupPermissionId { get; set; }

        [ForeignKey("FlightDocumentId")]
        public FlightDocument FlightDocumentNavigation { get; set; }
        [ForeignKey("GroupPermissionId")]
        public GroupPermission GroupPermissionNavigation { get; set; }
    }
}

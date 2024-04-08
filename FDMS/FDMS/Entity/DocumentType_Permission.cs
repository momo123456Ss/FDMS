using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FDMS.Entity
{
    [Table("DocumentType_Permission")]
    public class DocumentType_Permission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [DefaultValue(false)]
        public bool ReadAndModify { get; set; }
        [DefaultValue(false)]
        public bool ReadOnly { get; set; }
        [DefaultValue(false)]
        public bool NoPermission { get; set; }
        public int DocumentTypeId { get; set; }
        public int GroupPermissionId { get; set; }
        [ForeignKey("DocumentTypeId")]
        public DocumentType DocumentTypeNavigation { get; set; }
        [ForeignKey("GroupPermissionId")]
        public GroupPermission GroupPermissionNavigation { get; set; }
    }
}

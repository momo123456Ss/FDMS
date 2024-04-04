using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FDMS.Entity
{
    [Table("Account_GroupPermission")]
    public class Account_GroupPermission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int GroupPermissionId { get; set; }
        [Required]
        [MaxLength(50)]
        public string AccountEmail { get; set; }
        [ForeignKey("GroupPermissionId")]
        public GroupPermission GroupPermissionNavigation { get; set; }
        [ForeignKey("AccountEmail")]
        public Account AccountNavigation { get; set; }
    }
}

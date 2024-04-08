using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FDMS.Entity
{
    [Table("GroupPermission")]
    public class GroupPermission
    {
        public GroupPermission()
        {
            Account_GroupPermissions = new HashSet<Account_GroupPermission>();
            DocumentType_Permissions = new HashSet<DocumentType_Permission>(); 
            FlightDocument_GroupPermissions = new HashSet<FlightDocument_GroupPermission>();
        }
        [Key]
        public int GroupPermissionId { get; set; }
        [Required]
        [MaxLength(50)]
        public string GroupName { get; set; }
        [DefaultValue(0)]
        public int TotalMembers { get; set; }
        public DateTime CreatedDate { get; set; }
        [Required]
        [MaxLength(255)]
        public string Note { get; set; }
        [Required]
        [MaxLength(50)]
        public string Creator { get; set; }
        [ForeignKey("Creator")]
        public Account AccountNavigation { get; set; }
        [JsonIgnore]
        public virtual ICollection<Account_GroupPermission> Account_GroupPermissions { get; set; }
        [JsonIgnore]
        public virtual ICollection<DocumentType_Permission> DocumentType_Permissions { get; set; }
        [JsonIgnore]
        public virtual ICollection<FlightDocument_GroupPermission> FlightDocument_GroupPermissions { get; set; }
    }
}

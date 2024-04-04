using CloudinaryDotNet;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FDMS.Entity
{
    [Table("Account")]
    public class Account
    {
        public Account()
        {
            Account_GroupPermissions = new HashSet<Account_GroupPermission>();
            AccountSessions = new HashSet<AccountSession>();
        }
        [Key]
        [Required]
        [MaxLength(50)]
        public string Email { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [MaxLength(15)]
        public string Phone { get; set; }
        [Required]
        [MaxLength(255)]
        public string Password { get; set; }
        [Required]
        [MaxLength(255)]
        public string Avatar { get; set; }
        public bool IsActived { get; set; }

        public string RoleId { get; set; }
        [ForeignKey("RoleId")]
        public Role RoleNavigation { get; set; }

        [JsonIgnore]
        public virtual ICollection<Account_GroupPermission> Account_GroupPermissions { get; set; }
        [JsonIgnore]
        public virtual ICollection<AccountSession> AccountSessions { get; set; }
    }
}

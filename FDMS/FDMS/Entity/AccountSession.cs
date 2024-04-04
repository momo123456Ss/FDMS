using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FDMS.Entity
{
    [Table("AccountSession")]
    public class AccountSession
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountSessionId { get; set; }
        [Required]
        [MaxLength]
        public string AccessToken { get; set; }
        public DateTime NotBefore { get; set; }
        public DateTime ExpirationTime { get; set; }
        public DateTime IssuedAt { get; set; }

        [Required]
        [MaxLength(50)]
        public string AccountEmail { get; set; }
        [ForeignKey("AccountEmail")]
        public Account AccountNavigation { get; set; }
    }
}

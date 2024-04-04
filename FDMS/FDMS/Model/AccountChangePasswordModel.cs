using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FDMS.Model
{
    public class AccountChangePasswordModel
    {
        [Required]
        [MaxLength(255)]
        [DefaultValue("string")]
        public string CurrentPassword { get; set; }
        [Required]
        [MaxLength(255)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$", ErrorMessage = "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
        [DefaultValue("string")]
        public string NewPassword { get; set; }
    }
}

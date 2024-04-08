using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FDMS.Model
{
    public class AccountCreateModel
    {
        [Required]
        [MaxLength(50)]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@vietjetair.com$", ErrorMessage = "Email must be from @vietjetair.com domain.")]
        [DefaultValue("@vietjetair.com")]
        public string Email { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [MaxLength(15)]
        public string Phone { get; set; }
        [Required]
        [MaxLength(255)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$", ErrorMessage = "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
        [DefaultValue("string")]
        public string Password { get; set; }
        public bool IsActived { get; set; }
        public string RoleId { get; set; }

    }
}

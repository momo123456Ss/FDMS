using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace FDMS.Model
{
    public class SignInModel
    {
        [Required]
        [MaxLength(50)]
        [DataType(DataType.EmailAddress)]
        [DefaultValue("test0@vietjetair.com")]
        public string Email { get; set; }
        [Required]
        [MaxLength(250)]
        [DataType(DataType.Password)]
        [DefaultValue("123456")]
        public string Password { get; set; }
    }
}

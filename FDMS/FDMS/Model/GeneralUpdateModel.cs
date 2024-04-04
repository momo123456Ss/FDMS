using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace FDMS.Model
{
    public class GeneralUpdateModel
    {
        [MaxLength(20)]
        public string? Theme { get; set; }
        [DefaultValue(false)]
        public bool? CAPTCHA { get; set; }
        public IFormFile? LogoFile { get; set; }
    }
}

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FDMS.Entity
{
    [Table("General")]
    public class General
    {
        [Key]
        [MaxLength(10)]
        public string GeneralId { get; set; }
        [Required]
        [MaxLength(20)]
        public string Theme { get; set; }
        [Required]
        [MaxLength(255)]
        public string Logo { get; set; }
        [DefaultValue(false)]
        public bool CAPTCHA { get; set; }

    }
}

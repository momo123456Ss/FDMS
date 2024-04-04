using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FDMS.Entity
{
    [Table("SystemNofication")]
    public class SystemNofication
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string Content { get; set; }
        public DateTime Date { get; set; }

    }
}

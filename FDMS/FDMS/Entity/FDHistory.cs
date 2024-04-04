using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FDMS.Entity
{
    [Table("FDHistory")]
    public class FDHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FDHistoryId { get; set; }
        [Required]
        [MaxLength(255)]
        public string Content { get; set; }
        [Required]
        public DateTime Date { get; set; }
    }
}

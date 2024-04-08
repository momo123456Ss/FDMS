using System.ComponentModel.DataAnnotations;

namespace FDMS.Model
{
    public class DocumentTypeViewModel
    {
        public int DocumentTypeId { get; set; }
        public string Type { get; set; }
        public DateTime CreateDate { get; set; }
        public string Creator { get; set; }
        public AccountViewModel AccountNavigation { get; set; }
        public int TotalGroups { get; set; }
        public string Note { get; set; }
    }
}

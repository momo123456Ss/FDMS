using System.ComponentModel.DataAnnotations;

namespace FDMS.Model
{
    public class AccountUpdateModel
    {
        [MaxLength(50)]
        public string? Name { get; set; }
        [MaxLength(15)]
        public string? Phone { get; set; }
        public bool? IsActived { get; set; }
        public string? RoleId { get; set; }
    }
}

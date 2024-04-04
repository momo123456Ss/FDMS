using System.ComponentModel.DataAnnotations;

namespace FDMS.Model
{
    public class RoleCreateOrUpdateModel
    {
        [MaxLength(50)]
        public string? RoleName { get; set; }
        [MaxLength(255)]
        public string? Description { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace FDMS.Model
{
    public class GroupPermissionCreateOrUpdateModel
    {
        [MaxLength(50)]
        public string? GroupName { get; set; }
        [MaxLength(255)]
        public string? Note { get; set; }
    }
}

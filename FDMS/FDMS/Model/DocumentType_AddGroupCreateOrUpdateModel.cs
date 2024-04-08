using System.ComponentModel;

namespace FDMS.Model
{
    public class DocumentType_AddGroupCreateOrUpdateModel
    {
        public int DocumentTypeId { get; set; }
        public ICollection<GroupPermissionConfiguration> GroupPermissionConfigurations { get; set; }
    }
    public class GroupPermissionConfiguration
    {
        public int GroupPermissionId { get; set; }
        [DefaultValue(false)]
        public bool ReadAndModify { get; set; }
        [DefaultValue(false)]
        public bool ReadOnly { get; set; }
        [DefaultValue(false)]
        public bool NoPermission { get; set; }
    }
}

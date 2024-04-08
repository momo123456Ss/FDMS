using System.ComponentModel;

namespace FDMS.Model
{
    public class DocumentType_ConfigGroupPermissionUpdateModel
    {
        [DefaultValue(false)]
        public bool ReadAndModify { get; set; }
        [DefaultValue(false)]
        public bool ReadOnly { get; set; }
        [DefaultValue(false)]
        public bool NoPermission { get; set; }
    }
}

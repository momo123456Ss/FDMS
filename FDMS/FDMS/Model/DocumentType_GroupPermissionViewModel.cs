using FDMS.Entity;
using System.ComponentModel;

namespace FDMS.Model
{
    public class DocumentType_GroupPermissionViewModel
    {
        public int GroupPermissionId { get; set; }
        public string GroupName { get; set; }
        public bool ReadAndModify { get; set; }
        public bool ReadOnly { get; set; }
        public bool NoPermission { get; set; }   
    }
}

namespace FDMS.Model
{
    public class DocumentType_RemoveGroupDeleteModel
    {
        public int DocumentTypeId { get; set; }
        public List<int> GroupPermissionIds { get; set; }
    }
}

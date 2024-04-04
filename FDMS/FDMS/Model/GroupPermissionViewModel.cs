namespace FDMS.Model
{
    public class GroupPermissionViewModel
    {
        public int GroupPermissionId { get; set; }
        public string GroupName { get; set; }
        public string Members { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Note { get; set; }
        public string Creator { get; set; }
        public AccountViewModel AccountNavigation { get; set; }
    }
}

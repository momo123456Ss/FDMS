using System.ComponentModel.DataAnnotations;

namespace FDMS.Model
{
    public class AccountViewModel
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public RoleViewModel RoleNavigation { get; set; }

    }
}

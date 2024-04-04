using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace FDMS.Model
{
    public class GeneralViewModel
    {
        public string Theme { get; set; }
        public string Logo { get; set; }
        public bool CAPTCHA { get; set; }
    }
}

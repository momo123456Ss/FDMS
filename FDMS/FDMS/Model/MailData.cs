using System.ComponentModel;
using System.Text.Json.Serialization;

namespace FDMS.Model
{
    public class MailData
    {
        [DisplayName("Địa chỉ email người nhận")]
        public string ReceiverEmail { get; set; }
        [DisplayName("Tên người nhận")]
        [JsonIgnore]
        public string? ReceiverName { get; set; }
        [DisplayName("Tiêu đề")]
        [JsonIgnore]
        public string? Title { get; set; }
        [JsonIgnore]
        public string? Body { get; set; }
    }
}

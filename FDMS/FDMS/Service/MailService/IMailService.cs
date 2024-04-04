using FDMS.Model;
using Org.BouncyCastle.Asn1.Pkcs;

namespace FDMS.Service.MailService
{
    public interface IMailService
    {
        Task<APIResponse> SendMail(MailData mailData);
    }
}

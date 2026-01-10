using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.ServiceAbstraction
{
    public interface ISMSService
    {
        Task<bool> SendSMSAsync(string phoneNumber, string message);
        List<string> GetSMSTemplates();
        string FormatPhoneNumber(string phoneNumber);
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class EmailRequiredInfo
    {
        public string ReceiverEmail { get; set; }
        public string ReceiverName { get; set; }
        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
        public string Host { get; set; }
        public string EmailLogin { get; set; }
        public string PasswordLogin { get; set; }
    }
    public interface IEmailService
    {
        void SendMessage(EmailRequiredInfo eri, String messageContent);
    }
}

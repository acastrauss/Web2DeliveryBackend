using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Smtp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class EmailService : IEmailService
    {
        public void SendMessage(EmailRequiredInfo eri, string messageContent)
        {
            MailMessage message = new MailMessage();

            // Set subject of the message, body and sender information
            message.Subject = "Approval";
            message.Body = messageContent;
            message.From = new MailAddress(eri.SenderEmail, eri.SenderName, false);

            // Add To recipients and CC recipients
            message.To.Add(new MailAddress(eri.ReceiverEmail, eri.ReceiverName, false));

            // Save message in EML, EMLX, MSG and MHTML formats
            message.Save("EmailMessage.eml", SaveOptions.DefaultEml);
            message.Save("EmailMessage.emlx", SaveOptions.CreateSaveOptions(MailMessageSaveType.EmlxFormat));
            message.Save("EmailMessage.msg", SaveOptions.DefaultMsgUnicode);
            message.Save("EmailMessage.mhtml", SaveOptions.DefaultMhtml);
            SmtpClient client = new SmtpClient();

            // Specify your mailing Host, Username, Password, Port # and Security option
            client.Host = eri.Host;
            client.Username = eri.EmailLogin;
            client.Password = eri.PasswordLogin;
            client.Port = 587;
            client.SecurityOptions = SecurityOptions.SSLExplicit;
            try
            {
                // Send this email
                client.Send(message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while sending outlook mail.");
            }
        }
    }
}

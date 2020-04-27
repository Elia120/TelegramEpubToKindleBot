using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;

namespace TelegramBot.Email
{
    public static class SendEMail
    {
        public async static void Send(string Email, string FilePath)
        {
            using (SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com"))
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("epubtokindletelegrambot@gmail.com");
                    mail.To.Add(Email);
                    mail.Subject = "Your Book Got Coverted";

                    Attachment attachment;
                    attachment = new Attachment(FilePath);
                    mail.Attachments.Add(attachment);

                    SmtpServer.Port = 587;
                    SmtpServer.Credentials = new System.Net.NetworkCredential("epubtokindletelegrambot@gmail.com", "Es1234567890");
                    SmtpServer.EnableSsl = true;

                    await SmtpServer.SendMailAsync(mail);
                }
            }
        }
    }
}

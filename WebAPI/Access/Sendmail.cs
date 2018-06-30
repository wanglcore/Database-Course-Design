using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Web.Access
{
    public class Sendmail
    {
        private readonly string passwd = "0624@wsl.";
        private readonly string subject = "modify password";
        public string EmailAddress { get; set; }
        public string Verificode { get; set; }
        private readonly MailAddress fromaddress = new MailAddress("bhudomin@gmail.com");
        public bool Sendemail(string ToAddress)
        {
            Random random = new Random();
            int code = random.Next(100000000, 999999999);
            Verificode = "123456";
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(fromaddress.Address, passwd),
            };

            MailAddress toaddress = new MailAddress(ToAddress);
            MailMessage mailMessage = new MailMessage(fromaddress, toaddress)
            {
                Subject = subject,
                Body = Verificode
            };
            object userstat = mailMessage;
            try
            {
                smtp.SendAsync(mailMessage, userstat);
                return true;
            }
            catch (SmtpException e)
            {
                return false;
            }
        }
        public bool IsValidcode(string code)
        {
            Verificode = "123456";
            if (code == Verificode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

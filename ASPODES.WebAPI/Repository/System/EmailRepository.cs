using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net.Http;
using ASPODES.Model;
using ASPODES.Database;
using System.Net.Mail;
using System.Net;
using ASPODES.WebAPI.Common;

namespace ASPODES.WebAPI.Repository.System
{
    public class EmailRepository
    {
        public void AddEmail(Email email)
        {
            email.SendTime = DateTime.Now;
            var client = CreateSmtpClient();
            var mail = CreateMailMessage(email);
            
            try
            {
                client.Send(mail);
                email.Status = EmailStatus.SUCCESS;
            }
            catch( Exception e)
            {
                email.Status = EmailStatus.FAIL;
            }

            try
            {
                using (var ctx = new AspodesDB())
                {
                    ctx.Emails.Add(email);
                    ctx.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }

        public void ResendAllEmail() 
        {
            using (var ctx = new AspodesDB())
            {
                var emails = ctx.Emails.Where(e => e.Status < EmailStatus.FAIL);
                var client = CreateSmtpClient();
                foreach (var email in emails)
                {
                    var mailMsg = CreateMailMessage(email);
                    try
                    {
                        client.Send(mailMsg);
                        email.Status = EmailStatus.SUCCESS;
                    }
                    catch (Exception e)
                    {
                        email.Status++;
                    }
                    ctx.SaveChanges();
                }
            }
        }

        private SmtpClient CreateSmtpClient()
        {
            SmtpClient client = new SmtpClient();
            client.DeliveryMethod = SmtpDeliveryMethod.Network;//指定电子邮件发送方式    
            client.Host = SystemConfig.SMTPHost;//邮件服务器
            client.Port = 25;
            client.EnableSsl = false;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(SystemConfig.SMTPUserName, SystemConfig.SMTPPassword);//用户名、密码
            return client;
        }

        private MailMessage CreateMailMessage(Email email) 
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(email.SenderId);
            mail.To.Add(email.ReciveAddress);
            mail.Subject = email.Subject;
            mail.Body = email.Content;
            mail.BodyEncoding = Encoding.UTF8;
            mail.IsBodyHtml = false;
            mail.Priority = MailPriority.High;
            return mail;
        }
    }
}
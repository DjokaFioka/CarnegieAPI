using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Web;

namespace CarnegieAPI.Helpers
{
    public class EmailHelper
    {
        public static void sendNewPasswordMail(string emailTo, string password)
        {
            string text = "Please change the password after your next login! Your new password is: " + password;
            string html = "Your new password is <br><b>" + password + "</b> <br>Change it the next time you login!";

            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("carnegie.task@gmail.com");
            msg.To.Add(new MailAddress(emailTo));
            msg.Subject = "Carnegie - Changed Password";
            msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, Encoding.UTF8, MediaTypeNames.Text.Plain));
            msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, Encoding.UTF8, MediaTypeNames.Text.Html));
            msg.Body = html;
            msg.IsBodyHtml = true;

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", Convert.ToInt32(587));
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("carnegie.task@gmail.com", "carneg1e123.");
            smtpClient.Credentials = credentials;
            smtpClient.EnableSsl = true;
            smtpClient.Send(msg);
        }
    }
}
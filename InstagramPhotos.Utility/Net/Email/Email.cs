using System.Net;
using System.Net.Mail;
using System.Text;
using InstagramPhotos.Utility.Configuration;

namespace InstagramPhotos.Utility.Net.Email
{
    public static class Email
    {
        private static readonly string EMAIL_HOST = AppSettings.GetValue("EMAIL_HOST", "smtp.exmail.qq.com");
        private static readonly int EMAIL_PORT = AppSettings.GetValue("EMAIL_PORT", 25);

        private static readonly string FROM_EMAIL_ACCOUNT = AppSettings.GetValue("FROM_EMAIL_ACCOUNT",
            "monitor@ccjoy-inc.com");

        private static readonly string FROM_EMAIL_PWD = AppSettings.GetValue("FROM_EMAIL_PWD", "ccjoy-inc.com");

        private static readonly string TO_EMAIL_ACCOUNTS = AppSettings.GetValue("TO_EMAIL_ACCOUNTS",
            "cwrola@126.com");

        /// <summary>
        ///     发邮件
        /// </summary>
        /// <param name="show_name">邮件发送者（显示名）</param>
        /// <param name="title">主题</param>
        /// <param name="body">邮件主体</param>
        public static void Send(string show_name, string title, string body)
        {
            var smtp = new SmtpClient();
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.EnableSsl = false;
            smtp.Host = EMAIL_HOST;
            smtp.Port = EMAIL_PORT;
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = new NetworkCredential(FROM_EMAIL_ACCOUNT, FROM_EMAIL_PWD);
            var mm = new MailMessage();
            mm.Subject = title;
            mm.Priority = MailPriority.High;
            mm.From = new MailAddress(FROM_EMAIL_ACCOUNT, show_name, Encoding.GetEncoding(936));
            mm.ReplyToList.Add(new MailAddress(FROM_EMAIL_ACCOUNT, FROM_EMAIL_ACCOUNT, Encoding.GetEncoding(936)));
            string[] to_names = TO_EMAIL_ACCOUNTS.Split(',');
            mm.Sender = new MailAddress(FROM_EMAIL_ACCOUNT);
            foreach (string name in to_names)
            {
                mm.To.Add(new MailAddress(name, string.Empty, Encoding.GetEncoding(936)));
            }
            mm.Body = body;
            mm.IsBodyHtml = true;
            smtp.Send(mm);
        }
    }
}
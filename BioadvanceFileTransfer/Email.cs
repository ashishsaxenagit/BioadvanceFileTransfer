using System;
using System.Net.Mail;
using System.Text.RegularExpressions;
namespace BioadvanceFileTransfer
{
    class Email
    {
        public ReadConfigFile readConfig = new ReadConfigFile();
        public bool SendMail(string strEmail_Body, string strFileDateFormat)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient();
                MailMessage message = new MailMessage();

                MailAddress fromAddress = new MailAddress(readConfig.strEmail_FromEmail, readConfig.strEmail_FromEmail);

                string[] sEmailTo = Regex.Split(readConfig.strEmail_ToEmail, ";");

                smtpClient.Host = readConfig.strEmail_SmtpServer;
                smtpClient.Port = int.Parse(readConfig.strEmail_Port);
                smtpClient.UseDefaultCredentials = true;
                smtpClient.EnableSsl = Convert.ToBoolean(readConfig.strEmail_EnableSsl);

                message.From = fromAddress;

                if (sEmailTo != null)
                {
                    for (int i = 0; i < sEmailTo.Length; ++i)
                    {
                        if (sEmailTo[i] != null && sEmailTo[i] != "")
                        {
                            message.To.Add(sEmailTo[i]);
                        }
                    }
                }

                switch (int.Parse(readConfig.strEmail_Priority))
                {
                    case 1:
                        message.Priority = MailPriority.High;
                        break;
                    case 3:
                        message.Priority = MailPriority.Low;
                        break;
                    default:
                        message.Priority = MailPriority.Normal;
                        break;
                }
                message.Subject = readConfig.strEmail_EmailSubject;
                message.IsBodyHtml = true;
                message.Body = strEmail_Body;

                smtpClient.Send(message);

                return true;
            }
            catch (Exception ex)
            {
                Logger.Write("**********File download Process Started: " + strFileDateFormat + " **********");
                Logger.Write("FileManagement:SendMail():Exception:" + ex.Message);
                Logger.Write("**********File download Process completed with above error:" + strFileDateFormat + "**********");
                Logger.Write("");
                return false;

            }
        }

    }
}

using System;
using System.Text;
namespace BioadvanceFileTransfer
{
    class Security
    {
        public Email email = new Email();
        public string EncryptString(string inputString)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(inputString);
            return Convert.ToBase64String(bytes, Base64FormattingOptions.None);
        }
        public string DecryptString(string inputString, string strFileDateFormat)
        {
            string strEmailBody = String.Empty;
            byte[] bytes =null;
            try
            {
                bytes = Convert.FromBase64String(inputString);
            }
            catch(Exception ex)
            {
                Logger.Write("**********File download Process Started: " + strFileDateFormat + " **********");
                Logger.Write("DecryptPassword():Exception:" + ex.Message);
                //Logger.Write("**********File download Process completed with above error:" + strFileDateFormat + "**********");
                //Logger.Write("");
                strEmailBody = strEmailBody + "</br>**********File download Process Started: " + strFileDateFormat + " **********";
                strEmailBody = strEmailBody + "</br>DecryptPassword():Exception:" + ex.Message;
                strEmailBody = strEmailBody + "</br>**********File download Process completed with above error:" + strFileDateFormat + "**********";
                email.SendMail(strEmailBody, strFileDateFormat);
            }
            return Encoding.ASCII.GetString(bytes);
        }

    }
}

using System;
using System.Configuration;
using System.Text;
namespace BioadvanceFileTransfer
{
    class ReadConfigFile
    {
        public String strFile_DateFormat = String.Empty;
        public String strImportsFTPUrl = String.Empty;
        public String strImportsFTPPort = String.Empty;
        public String strImportsFTPSourceFolder = String.Empty;
        public String strImportsFTPSourceArchiveFolder = String.Empty;
        public String strImportsFTPUserName = String.Empty;
        public String strImportsFTPUserPwd = String.Empty;
        public String strExportsFTPTargetFolder = String.Empty;
        public String strBioadvanceFileTransferLogFolder = String.Empty;

        public String strEmail_EmailSubject = String.Empty;
        public String strEmail_FromEmail = String.Empty;
        public String strEmail_ToEmail = String.Empty;
        public String strEmail_SmtpServer = String.Empty;
        public String strEmail_Port = String.Empty;
        public String strEmail_EnableSsl = String.Empty;
        public String strEmail_Priority = String.Empty;
        string strEmail_Body = String.Empty;
        public ReadConfigFile()
        {
            try
            {
                //Logger.Write("Starting read configuration file");
                strFile_DateFormat = DateTime.Now.ToString("yyyyMMdd");
                strImportsFTPUrl = ConfigurationManager.AppSettings["ImportsFTPUrl"];
                strImportsFTPPort = ConfigurationManager.AppSettings["ImportsFTPPort"];
                strImportsFTPSourceFolder = ConfigurationManager.AppSettings["ImportsFTPSourceFolder"];
                strImportsFTPSourceArchiveFolder = ConfigurationManager.AppSettings["ImportsFTPSourceArchiveFolder"];
                strImportsFTPUserName = ConfigurationManager.AppSettings["ImportsFTPUserName"];
                strImportsFTPUserPwd = ConfigurationManager.AppSettings["ImportsFTPUserPwd"];
                strExportsFTPTargetFolder = ConfigurationManager.AppSettings["ExportsFTPTargetFolder"];
                strBioadvanceFileTransferLogFolder = ConfigurationManager.AppSettings["BioadvanceFileTransferLogFolder"];


                strEmail_Port = ConfigurationManager.AppSettings["Email_Port"];
                strEmail_FromEmail = ConfigurationManager.AppSettings["Email_FromEmail"];
                strEmail_ToEmail = ConfigurationManager.AppSettings["Email_ToEmail"];
                strEmail_EmailSubject = ConfigurationManager.AppSettings["Email_Subject"];
                strEmail_SmtpServer = ConfigurationManager.AppSettings["Email_SmtpServer"];
                strEmail_EnableSsl = ConfigurationManager.AppSettings["Email_EnableSsl"];
                strEmail_Priority = ConfigurationManager.AppSettings["Email_Priority"];
                //Logger.Write("Finishing read configuration file");
            }
            catch (Exception e)
            {
                //Logger.Write("FileManagement:ReadConfigFile():Exception:" + e.Message);
                Logger.Write("**********File download Process Started: " + strFile_DateFormat + " **********");
                Logger.Write("ReadConfigFile():Exception:" + e.Message);
                //Logger.Write("**********File download Process completed with above error:" + strFileDateFormat + "**********");
                //Logger.Write("");
                //strEmail_Body = strEmail_Body + "</br>**********File download Process Started: " + strFile_DateFormat + " **********";
                //strEmail_Body = strEmail_Body + "</br>ReadConfigFile():Exception:" + e.Message;
                //strEmail_Body = strEmail_Body + "</br>**********File download Process completed with above error:" + strFile_DateFormat + "**********";
                //email.SendMail(strEmail_Body, strFile_DateFormat);
            }
        }

    }
}

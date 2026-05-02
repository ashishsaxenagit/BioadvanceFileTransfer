using System;
using System.Configuration;
namespace BioadvanceFileTransfer
{
    class Program
    {
        public Email email = new Email();
        static void Main(string[] args)
        {
            try
            {
                string strBioadvanceFileTransferLogFolder = ConfigurationManager.AppSettings["BioadvanceFileTransferLogFolder"];//Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                Logger LoggerInterface = new Logger(strBioadvanceFileTransferLogFolder, "BioadvanceFileTransfer_Log", "LogName" + "BioadvanceFileTransfer_Log.log", "Natsupport" + "BioadvanceFileTransfer_Problems.log");
                String[] arrParameters;
                arrParameters = null;
                ReadConfigFile readConfig = new ReadConfigFile();
                FileManagement nexGenProcess = new FileManagement(readConfig, arrParameters);
            }
            catch (Exception ex)
            {
                Logger.Write("BioadvanceFileTransfer.Main():Exception:" + ex.ToString());
                //email.SendMail(readConfig.strEmail_EmailSubject, readConfig.strEmail_FromEmail, readConfig.strEmail_ToEmail, strEmailBody, readConfig.strEmail_SmtpServer, readConfig.strEmail_Port, readConfig.strEmail_EnableSsl, readConfig.strEmail_Priority);
            }
        }
    }
}

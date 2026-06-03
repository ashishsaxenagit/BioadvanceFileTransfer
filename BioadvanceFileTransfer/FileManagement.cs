using Renci.SshNet;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
namespace BioadvanceFileTransfer
{
    class FileManagement
    {
        private ReadConfigFile readConfig;
        private DataTable dtEXL = new DataTable();
        private bool cRes = true;
        private string strFileName = string.Empty;
        public Email email = new Email();
        public Security security = new Security();
        public FileManagement(ReadConfigFile readConfig, string[] arrParameters)
        {
            try
            {
                //Init DB config data
                this.readConfig = readConfig;
                cRes = GetFilefromsFTP();
            }
            catch (Exception ex)
            {
                Logger.Write("FileManagement():Exception:" + ":" + ex.Message.ToString());
                Logger.Write("**********FileManagement:GetFilefromsFTP():Process completed with above error**********");
                Logger.Write("");

            }
        }

        public bool GetFilefromsFTP()
        {
            //string samplePwd = security.EncryptString("BioAMS@");

            int fileCount = 0;
            bool isDownloaded = false;
            string strEmailBody = String.Empty;
            string strFileDateFormat = DateTime.Now.ToString("yyyyMMdd");
            //string samplepwd1 = security.DecryptString("VGFzaHUwMDE3QA==", strFileDateFormat);
            //strFileDateFormat = "20260506";
            string strEmailPwd = String.Empty;
            strEmailPwd = security.DecryptString(readConfig.strImportsFTPUserPwd, strFileDateFormat);

            try
            {
                strEmailBody = "**********File download Process Started:" + strFileDateFormat + "**********";
                Logger.Write("**********FileManagement:GetFilefromsFTP():Process Started:" + strFileDateFormat+ "**********");
                ConnectionInfo connectionInfo = new PasswordConnectionInfo(readConfig.strImportsFTPUrl, Convert.ToInt32(readConfig.strImportsFTPPort), readConfig.strImportsFTPUserName, strEmailPwd);
                using (var sftp = new SftpClient(connectionInfo))
                {
                    sftp.Connect();
                    var files = sftp.ListDirectory(readConfig.strImportsFTPSourceFolder).Where(f => !f.IsDirectory && f.Name.Contains("_"+strFileDateFormat));
                    foreach (var file in files)
                    {
                        using (Stream fileStream = File.Create(Path.Combine(readConfig.strExportsFTPTargetFolder, file.Name)))
                        {
                            sftp.DownloadFile(file.FullName, fileStream);
                        }
                        Console.WriteLine($"FileManagement:Downloaded: {file.Name}");
                        Logger.Write("FileManagement:Downloaded:" + file.Name);
                        strEmailBody = strEmailBody + "</br>Downloaded: " + file.Name;
                        fileCount = fileCount + 1;
                    }
                    sftp.Disconnect();
                }
                if (fileCount==0)
                {
                    isDownloaded = false;
                    Console.WriteLine($"FileManagement:File not found on server :" + readConfig.strImportsFTPUrl);
                    Logger.Write($"FileManagement:File not found on server :" + readConfig.strImportsFTPUrl);
                    Logger.Write("**********FileManagement:GetFilefromsFTP():Process completed:" + strFileDateFormat+ "**********");
                    strEmailBody = strEmailBody + "</br>FileManagement: File not found on server: " + readConfig.strImportsFTPUrl; 
                    strEmailBody = strEmailBody + "</br>**********File download Process completed:" + strFileDateFormat + "**********";
                }
                else
                {
                    isDownloaded = true;
                    Logger.Write("**********FileManagement:GetFilefromsFTP():Process completed:" + strFileDateFormat+ "**********");
                    strEmailBody = strEmailBody + "</br>**********File download Process completed:" + strFileDateFormat + "**********";
                }

            }
            catch (Exception ex)
            {
                Logger.Write("FileManagement:GetFilefromsFTP():Exception:" + ex.Message);
                Logger.Write("**********FileManagement:GetFilefromsFTP():Process completed with above error:" + strFileDateFormat + "**********");
                
                strEmailBody = strEmailBody + "</br>FileManagement:GetFilefromsFTP():Exception:" + ex.Message;
                strEmailBody = strEmailBody + "</br>**********File download Process completed with above error:" + strFileDateFormat + "**********";
            }
            finally
            {
                Logger.Write("");
                email.SendMail(strEmailBody, strFileDateFormat);
            }

            return isDownloaded;
        }
       

    }

}

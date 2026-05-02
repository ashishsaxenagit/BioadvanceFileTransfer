using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace BioadvanceFileTransfer
{
    class Logger
    {
        /*************Class Variables*************************************/
        public static string currentDirectory = string.Empty;
        public static string logFilePath = string.Empty;
        public static string problemsLogFilePath = string.Empty;
        public static string logFileName = "BioadvanceFileTransfer_CheckStatus.log";
        public static string logNameProblems = "BioadvanceFileTransfer_CheckStatus_Problems.log";
        public static string eventSource = string.Empty;

        public static StreamWriter logWriter = null;
        public static StreamWriter problemLogWriter = null;

        public static FileInfo fileInfo = null;
        public static FileInfo problemFileInfo = null;

        public static DateTime currentDate = DateTime.Now;

        public static object logWriteMutex = new object();
        public static object problemWriteMutex = new object();

        public static System.Text.UnicodeEncoding unicode;

        public static bool debug = true;

        /**************************************************************/
        //Write all information to log file
        public Logger()
        {
            try
            {
            }
            catch (Exception e)
            {
                Logger.ProblemsWrite("Logger:LoggerDefaultConstructor():Exception:" + e.ToString());
            }
            finally
            {
            }
        }

        /**************************************************************/
        public Logger(string logDirectory,
                      string EventSoutce,
                      string logName,
                      string logNameProb)
        {
            try
            {
                eventSource = EventSoutce;
                currentDirectory = logDirectory;

                logFileName = logName;
                logNameProblems = logNameProb;

                logFilePath = BuildLogFilePath();
                unicode = new System.Text.UnicodeEncoding();

                fileInfo = new FileInfo(logFilePath);
                problemFileInfo = new FileInfo(problemsLogFilePath);

                /*---------open files for logging----------------------*/
                OpenLogFIle();
            }
            catch (Exception e)
            {
                Logger.ProblemsWrite("Logger:LoggerParameterizedConstructor():Exception:" + e.ToString());
            }
            finally
            {
            }
        }
        //Build log file path related to the specific date
        /**************************************************************/
        public static string BuildLogFilePath()
        {

            try
            {
                string newDateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss tt");

                string m_year = newDateTime.Substring(0, 4);
                string m_month = newDateTime.Substring(5, 2);
                string m_day = newDateTime.Substring(8, 2);
                string m_hour = newDateTime.Substring(11, 2);
                string m_min = newDateTime.Substring(14, 2);
                logFileName = eventSource + ".log";
                logNameProblems = eventSource + "Problems" + ".log";

                logFilePath = currentDirectory + "\\" + logFileName;
                problemsLogFilePath = currentDirectory + "\\" + logNameProblems;

                return logFilePath;
            }
            catch (Exception e)
            {
                Logger.ProblemsWrite("Logger:BuildLogFilePath():Exception:" + e.ToString());
                return currentDirectory + logFileName + "911";
            }
            finally
            {
            }
        }
        /**************************************************************/
        public static bool WriteToEventLog(string eventString)
        {
            try
            {
                if (!EventLog.SourceExists(eventSource))
                {
                    EventLog.CreateEventSource(eventSource, "Application");
                }
                EventLog.WriteEntry(eventSource, eventString);

                return true;
            }
            catch (Exception e)
            {
                Logger.ProblemsWrite("Logger:WriteToEventLog():Exception:" + e.ToString());
                return false;
            }
        }
        /*************************************************************/
        public static bool OpenLogFIle()
        {
            try
            {
                logWriter = new StreamWriter(logFilePath, true, unicode);

                problemLogWriter = new StreamWriter(problemsLogFilePath, true, unicode);

                if (logWriter == null || problemLogWriter == null)
                {
                    return false;
                }
                return true;
            }
            catch (System.IO.IOException e)
            {
                Logger.ProblemsWrite("Logger:OpenLogFIle():Exception:" + e.ToString());
                return false;
            }
        }
        /*************************************************************/
        public static bool CloseLogFile()
        {
            try
            {
                if (logWriter != null)
                {
                    logWriter.Close();
                }
                if (problemLogWriter != null)
                {
                    problemLogWriter.Close();
                }

                return true;
            }
            catch (Exception e)
            {
                Logger.ProblemsWrite("Logger:CloseLogFile():Exception:" + e.ToString()); 
                return false;
            }
        }
        /*************************************************************/
        public static bool ReopenLogFile()
        {
            try
            {
                CloseLogFile();

                logWriter = null;
                problemLogWriter = null;

                OpenLogFIle();

                return true;
            }
            catch (Exception e)
            {
                WriteToEventLog("Logger:ReopenLogFile():Exception:" + e.ToString());
                return false;
            }
        }
        //Compare two date's
        /*************************************************************/
        public static bool IsNewDay()
        {
            try
            {
                DateTime newDate = DateTime.Now;
                if (newDate.DayOfYear != currentDate.DayOfYear)
                {
                    currentDate = newDate;
                    return true;
                }
                else
                    return false;
            }
            catch (Exception e)
            {
                WriteToEventLog("Logger:IsNewDay(): " + e.ToString());
                return false;
            }
        }
        /*************************************************************/
        public static bool ProblemsWrite(string message)
        {
            try
            {
                lock (problemWriteMutex)
                {
                    //Check if we have a new day
                    if (IsNewDay())
                    {
                        BuildLogFilePath();
                        ReopenLogFile();
                    }

                    problemLogWriter.Write("[ " + DateTime.Now.ToString() + " ]");
                    problemLogWriter.WriteLine(message);
                    problemLogWriter.Flush();

                }
                return true;
            }
            catch (Exception e)
            {
                WriteToEventLog("Logger:ProblemsWrite(): " + e.ToString());
                return false;
            }

        }
        /*************************************************************/
        public static bool Write(string message)
        {
            try
            {
                lock (logWriteMutex)
                {
                    //Check if we have a new day


                    if (IsNewDay())
                    {
                        BuildLogFilePath();
                        ReopenLogFile();
                    }

                    if (Logger.debug)
                    {
                        if (message == "")
                        {
                            logWriter.WriteLine(message);
                            logWriter.Flush();
                        }
                        else
                        {
                            logWriter.Write("[ " + DateTime.Now.ToString() + " ]");
                            logWriter.WriteLine(message);
                            logWriter.Flush();
                        }
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                WriteToEventLog("Logger:Write(): " + e.ToString());
                return false;
            }

        }
        /*************************************************************/
        public static bool WriteQuickly(string message)
        {
            try
            {
                lock (logWriteMutex)
                {
                    //Check if we have a new day

                    if (IsNewDay())
                    {
                        BuildLogFilePath();
                        ReopenLogFile();
                    }

                    logWriter.Write("[ " + DateTime.Now.ToString() + " ]");
                    logWriter.WriteLine(message);
                    logWriter.Flush();

                }

                return true;
            }
            catch (Exception e)
            {
                WriteToEventLog("Logger:WriteQuickly:Write(): " + e.ToString());
                return false;
            }

        }
        /*************************************************************/
    }
}

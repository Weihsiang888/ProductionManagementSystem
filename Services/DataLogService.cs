using DevExpress.Utils.Svg;
using DxBlazorApplication7.Data;
using NuGet.ProjectModel;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Text;

namespace DxBlazorApplication7.Data
{
    public class DataLogService
    {
        public string _Directory = System.AppDomain.CurrentDomain.BaseDirectory;

        public DataLogService()
        {
            //string _Directory = System.IO.Directory.GetCurrentDirectory();

            if (!Directory.Exists(_Directory + @"\Logs"))
            {
                Directory.CreateDirectory(_Directory + @"\Logs");
            }
            CheckLogFile();
        }

        public void WriteEventLog(string LogType, string Msg)
        {
            try
            {
                //string _Directory = System.IO.Directory.GetCurrentDirectory();

                CheckLogFile();

                string NowYear = DateTime.Today.Year.ToString();
                string NowMonth = DateTime.Today.Month.ToString("D2");
                string NowDay = DateTime.Today.Day.ToString("D2");

                string LogTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff");

                var LogMsg = string.Format(LogTime + "|" + Msg + "\r\n");
                var LogMsgBytes = Encoding.Default.GetBytes(LogMsg);

                switch (LogType)
                {
                    case "DB":
                        using (FileStream _LogFile = new FileStream(_Directory + @"\Logs\DB_" + NowYear + NowMonth + NowDay + ".log", FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write))
                        {
                            _LogFile.Seek(0, SeekOrigin.End);
                            _LogFile.Write(LogMsgBytes, 0, LogMsgBytes.Length);
                        }
                        //_StreamWriter = new StreamWriter(_Directory + @"\Logs\DB_" + NowYear + NowMonth + NowDay + ".log");
                        //_StreamWriter.WriteLine(LogTime + "|" + Msg);
                        //_StreamWriter.Close();
                        break;
                    case "Motion":
                        using (FileStream _LogFile = new FileStream(_Directory + @"\Logs\Motion_" + NowYear + NowMonth + NowDay + ".log", FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write))
                        {
                            _LogFile.Seek(0, SeekOrigin.End);
                            _LogFile.Write(LogMsgBytes, 0, LogMsgBytes.Length);
                        }
                        //_StreamWriter = new StreamWriter(_Directory + @"\Logs\Motion_" + NowYear + NowMonth + NowDay + ".log");
                        //_StreamWriter.WriteLine(LogTime + "|" + Msg);
                        //_StreamWriter.Close();
                        break;
                    case "Code":
                        using (FileStream _LogFile = new FileStream(_Directory + @"\Logs\Code_" + NowYear + NowMonth + NowDay + ".log", FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write))
                        {
                            _LogFile.Seek(0, SeekOrigin.End);
                            _LogFile.Write(LogMsgBytes, 0, LogMsgBytes.Length);
                        }
                        //_StreamWriter = new StreamWriter(_Directory + @"\Logs\Code_" + NowYear + NowMonth + NowDay + ".log");
                        //_StreamWriter.WriteLine(LogTime + "|" + Msg);
                        //_StreamWriter.Close();
                        break;
                }
            }
            catch (Exception ex)
            {
                //string _Directory = System.IO.Directory.GetCurrentDirectory();

                CheckLogFile();

                string NowYear = DateTime.Today.Year.ToString();
                string NowMonth = DateTime.Today.Month.ToString();
                string NowDay = DateTime.Today.Day.ToString();

                string LogTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff");

                var LogMsg = string.Format(LogTime + "|" + ex);
                var LogMsgBytes = Encoding.Default.GetBytes(LogMsg);

                using (FileStream _LogFile = new FileStream(_Directory + @"\Logs\Log_" + NowYear + NowMonth + NowDay + ".log", FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write))
                {
                    _LogFile.Seek(0, SeekOrigin.End);
                    _LogFile.Write(LogMsgBytes, 0, LogMsgBytes.Length);
                }

                //StreamWriter _StreamWriter = new StreamWriter(_Directory + @"\Logs\Log_" + NowYear + NowMonth + NowDay + ".log");
                //_StreamWriter.WriteLine(LogTime + "|" + ex);
                //_StreamWriter.Close();
            }
        }

        private void CheckLogFile()
        {
            //string _Directory = System.IO.Directory.GetCurrentDirectory();

            if (Directory.Exists(_Directory + @"\Logs"))
            {
                string NowYear = DateTime.Today.Year.ToString();
                string NowMonth = DateTime.Today.Month.ToString("D2");
                string NowDay = DateTime.Today.Day.ToString("D2");

                if (!File.Exists(_Directory + @"\Logs\DB_" + NowYear + NowMonth + NowDay + ".log"))
                {
                    File.Create(_Directory + @"\Logs\DB_" + NowYear + NowMonth + NowDay + ".log").Close();
                }

                if (!File.Exists(_Directory + @"\Logs\Motion_" + NowYear + NowMonth + NowDay + ".log"))
                {
                    File.Create(_Directory + @"\Logs\Motion_" + NowYear + NowMonth + NowDay + ".log").Close();
                }

                if (!File.Exists(_Directory + @"\Logs\Code_" + NowYear + NowMonth + NowDay + ".log"))
                {
                    File.Create(_Directory + @"\Logs\Code_" + NowYear + NowMonth + NowDay + ".log").Close();
                }

                if (!File.Exists(_Directory + @"\Logs\Log_" + NowYear + NowMonth + NowDay + ".log"))
                {
                    File.Create(_Directory + @"\Logs\Log_" + NowYear + NowMonth + NowDay + ".log").Close();
                }
            }
        }
    }
}

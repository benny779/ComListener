using System;
using System.Diagnostics;
using System.IO;

namespace ComListener
{
    class Logger
    {
        public string category { get; set; } = "ComListener";
        public bool toFile { get; set; } = false;
        public string logFileName { get; set; } = "ComListener_log.log";
        public string logFilePath { get; set; } = Environment.CurrentDirectory;


        private string logFileFullPath => Path.Combine(logFilePath, logFileName);
        private string now => DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");


        public void Log(string str, string category = null)
        {
            _Log(str, category);
        }
        public void Log(Exception ex, string category = null)
        {
            _Log("Exception: " + ex.Message, category);
        }


        private void _Log(string str, string category = null)
        {
            Trace.WriteLine(str, GetCategory(category));
            if (toFile)
            {
                string prefix = now + " - ";
                try
                {
                    File.AppendAllText(logFileFullPath, prefix + str + Environment.NewLine);
                }
                catch { }
            }
        }

        private string GetCategory(string category)
        {
            return category == null ? this.category : category;
        }
    }
}

namespace OakbrookFinanceTechTest.Repository
{
    using System;
    using System.IO;
    using OakbrookFinanceTechTest.Repository.Interfaces;

    public class Logger : ILogger
    {
        private string logFileDest;
        public Logger(string logFileDest)
        {
            this.logFileDest = AppDomain.CurrentDomain.BaseDirectory + logFileDest;
        }

        public void Log(string message)
        { 
            
            using (StreamWriter sw = new StreamWriter(File.Open(this.logFileDest, System.IO.FileMode.Append)))
            {
                sw.WriteLine(message);
            }  
        }
    }
}

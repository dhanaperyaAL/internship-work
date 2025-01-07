// Logger.cs
using System;
using System.IO;

namespace databaseexpress
{
    public class Logger
    {
        private string logFilePath = @"C:\Users\User\Downloads\project\logfiles\app_log.txt"; 

        public Logger(string logFilePath = null)
        {
            if (!string.IsNullOrEmpty(logFilePath))
            {
                this.logFilePath = logFilePath;
            }
        }

        public void Log(string message)
        {
            try
            {
                string directory = Path.GetDirectoryName(logFilePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                if (!File.Exists(logFilePath))
                {
                    using (FileStream fs = File.Create(logFilePath)) { }
                }

                using (StreamWriter writer = new StreamWriter(logFilePath, append: true))
                {
                    writer.WriteLine($"{DateTime.Now}: {message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error logging to file: {ex.Message}");
            }
        }
    }
}

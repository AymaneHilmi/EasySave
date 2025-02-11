using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace EasySave
{
    public static class GeneralTools
    {
        /// <summary>
        /// Calculate the size of a directory
        /// </summary>+
        /// <returns>The size of the directory</returns>
        public static long DirectorySize(string sourcePath, string targerPath) 
        {
            long size = 0;

            if (sourcePath != null && targerPath != null)
            {
                DirectoryInfo sourceDirectory = new DirectoryInfo(sourcePath);
                
                foreach (FileInfo file in sourceDirectory.EnumerateFiles("*", SearchOption.AllDirectories))
                    size += file.Length;
            }

            return size;
        }
        
        /// <summary>
        /// Var who stock the path to the logs directory
        /// </summary>
        public static IConfiguration conf = new ConfigurationBuilder().AddJsonFile(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\ViewModel\appsettings.json"), optional: false, reloadOnChange: true).Build();
        
        /// <summary>
        /// A simple function who write a warning message
        /// </summary>
        /// <param name="word">Message you want to write on Console</param>
        internal static void WriteWarningMessage(string word) {
            ConsoleColor temp = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(word);
            Console.ForegroundColor = temp;
        }
        
        /// <summary>
        /// Create log folder and logs files if they don't exist
        /// </summary>
        public static void CreateLogsFiles()
        {
            string logPath = conf["Log_Path"];
            if (!Directory.Exists(logPath))
                Directory.CreateDirectory(logPath);

            if(!File.Exists(logPath + "/logs.json"))
                File.Create(logPath + "/logs.json");
                
            
            if(!File.Exists(logPath + "/state.json"))
                File.Create(logPath + "/state.json");
            
            if(!File.Exists(logPath + "/logs.xml"))
                File.Create(logPath + "/logs.xml");
            
            if(!File.Exists(logPath + "/state.xml"))
                File.Create(logPath + "/state.xml");
        }
        
        /// <summary>
        /// Get the valid directory of a path :
        ///  ex : C:/Bureau/rep --> get C:/Bureau cause rep does not exist
        /// </summary>
        /// <param name="path">Invalid path</param>
        /// <returns>The valid path</returns>
        public static string GetValidDirectoryPath(string? path)
        {
            if (path != null && !path.Contains(@"\") && !path.Contains("/")) return "";
            while (!Directory.Exists(path)) path = Path.GetDirectoryName(path);
            return path;
        }


        public static bool VerifyBusinessSoftwareRunning(List<string> processes)
        {
            if (processes == null) return false;
            foreach (string process in processes)
            {
                if (Process.GetProcessesByName(process).Length > 0)
                    return true;
            }

            // No process is running, return false
            return false;
        }
    }
}
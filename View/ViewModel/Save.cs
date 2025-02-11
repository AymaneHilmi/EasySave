using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;
using System.Xml.Serialization;
using EasySaveApp.ViewModel;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using View.MVVM.View;

namespace EasySave
{
    public interface ISave {}
    
    public abstract class Save
    {
        /// <summary>
        /// Name of the save
        /// </summary>
        internal readonly string? Appellation;

        /// <summary>
        /// What you want to save
        /// </summary>
        public readonly string? SourcePath;

        /// <summary>
        /// Where you want to save
        /// </summary>
        internal readonly string? TargetPath;

        /// <summary>
        /// Files need to be copied (decrement when a file is copied)
        /// </summary>
        internal long FilesToCopy;

        /// <summary>
        /// Size of path directory
        /// </summary>
        internal long FilesSize;

        /// <summary>
        /// Files total to be copied
        /// </summary>
        internal long TotalFiles;
        
        protected bool _prioFilesExt = true; 
        
        private static object logsLockJson = new object();
        private static object logsLockXml = new object();
        private static object fileSizeLock = new object();
        private static object stateLockJson = new object();
        private static object stateLockXml = new object();
        
        private static long sizeParallelFiles = 0;

        public static long globalFilesLeftToDo = HomeView.GlobalSize;
        public static double globalProgression = 0;
        
        /// <summary>
        /// Handle the stop button in our app
        /// </summary>
        public static bool _stopped = false;

        public static bool _paused = false;

        protected Save(string? appellation, string? sourcePath, string? targetPath)
        {
            Appellation = appellation;
            SourcePath = sourcePath;
            TargetPath = targetPath;
            this.TotalFiles = 0;
        }

         /// <summary>
        /// Function who update dynamically the log file
        /// </summary>
        /// <param name="sourceFile">File source to be copied</param>
        /// <param name="fileTarget">File target, location to where we want to copy sourceFile</param>
        /// <param name="fileSize">Size of the file</param>
        /// <param name="stopwatch">Time of the copy</param>
        private void UpdateLogs(string sourceFile, string? fileTarget, long fileSize, string stopwatch,
            string cryptTime)
        {
            string xmlorjson = GeneralTools.conf["JsonOrXml"];

                if (xmlorjson == "json")
                {
                    lock (logsLockJson)
                    {
                        string filepath = GeneralTools.conf["Log_Path"] + "\\logs.json";

                        // Read the existing JSON file content into a string
                        string jsonContent = File.ReadAllText(filepath);

                        // Parse the string into a JObject
                        List<dynamic> jsonObjects = JsonConvert.DeserializeObject<List<dynamic>>(jsonContent);

                        // Create a new object
                        LogFile jsonInfoSaveFile = new LogFile(this.Appellation,
                            sourceFile,
                            fileTarget,
                            this.TargetPath,
                            fileSize.ToString(),
                            stopwatch,
                            DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                            cryptTime);

                        if (jsonObjects == null) jsonObjects = new List<dynamic>();
                        jsonObjects.Add(jsonInfoSaveFile);

                        // Serialize created object
                        string updatedJson = JsonConvert.SerializeObject(jsonObjects, Formatting.Indented);

                        File.WriteAllText(filepath, updatedJson);
                    }
                }
                else
                {
                    lock (logsLockXml)
                    {
                        string filepath = GeneralTools.conf["Log_Path"] + "\\logs.xml";

                        List<LogFile> xmlObjects;
                        XmlSerializer serializer = new XmlSerializer(typeof(List<LogFile>));

                        // Try to open the file in a shared mode
                        using (var fileStream = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.ReadWrite,
                                   FileShare.ReadWrite))
                        {
                            if (fileStream.Length > 0)
                            {
                                // Read the existing XML file content into a List of LogFile objects
                                xmlObjects = (List<LogFile>)serializer.Deserialize(fileStream);
                            }
                            else
                            {
                                xmlObjects = new List<LogFile>();
                            }

                            // Create a new LogFile object
                            LogFile xmlInfoSaveFile = new LogFile(this.Appellation,
                                sourceFile,
                                fileTarget,
                                this.TargetPath,
                                fileSize.ToString(),
                                stopwatch,
                                DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                                cryptTime);

                            xmlObjects.Add(xmlInfoSaveFile);

                            // Serialize and write the updated List of LogFile objects to the XML file
                            fileStream.Position = 0;
                            serializer.Serialize(fileStream, xmlObjects);
                        }
                    }
                }
        }

         /// <summary>
        /// Function who update dynamically the state file
        /// </summary>
        /// <param name="status"></param>
        public void UpdateState()
        {
   
            string xmlorjson = GeneralTools.conf["JsonOrXml"];

            if (xmlorjson == "json")
            {
                lock (stateLockJson)
                {
                    string filepath = GeneralTools.conf["Log_Path"] + "\\state.json";

                    // Parse the string into a JObject
                    List<StateFile>? jsonObjects =
                        JsonConvert.DeserializeObject<List<StateFile>>(File.ReadAllText(filepath));

                    if (this.FilesToCopy == 0) jsonObjects[^1].State = "END";

                    for (int i = 0; i < jsonObjects.Count; i++)
                    {
                        if (jsonObjects[i].Name == this.Appellation)
                        {
                            jsonObjects[i].TotalFilesToCopy = this.TotalFiles.ToString();
                            jsonObjects[i].TotalFilesSize = this.FilesSize.ToString();
                            jsonObjects[i].NbFilesLeftToDo = this.FilesToCopy.ToString();
                            jsonObjects[i].Progression =
                                this.TotalFiles == 0 || this.FilesToCopy == 0
                                    ? 100 + "%"
                                    : (100 - ((double)this.FilesToCopy / this.TotalFiles) * 100).ToString("0.00") +
                                      "%";

                            // For exit the loop
                            i = jsonObjects.Count;
                        }
                    }

                    string updatedJson = JsonConvert.SerializeObject(jsonObjects, Formatting.Indented);
                    File.WriteAllText(filepath, updatedJson);
                }
            }
            else
            {
                lock (stateLockXml)
                {
                    string filepath = GeneralTools.conf["Log_Path"] + "\\state.xml";
                    XmlSerializer serializer = new XmlSerializer(typeof(List<StateFile>));
                    List<StateFile> xmlObjects;

                    using (var fileStream = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.ReadWrite,
                               FileShare.ReadWrite))
                    {
                        xmlObjects = (List<StateFile>)serializer.Deserialize(fileStream);


                        for (int i = 0; i < xmlObjects.Count; i++)
                        {
                            if (xmlObjects[i].Name == this.Appellation)
                            {
                                // No files left to copy sot it set to end
                                if (this.FilesToCopy == 0) xmlObjects[i].State = "END";

                                xmlObjects[i].TotalFilesToCopy = this.TotalFiles.ToString();
                                xmlObjects[i].TotalFilesSize = this.FilesSize.ToString();
                                xmlObjects[i].NbFilesLeftToDo = this.FilesToCopy.ToString();
                                xmlObjects[i].Progression = this.TotalFiles == 0 || this.FilesToCopy == 0
                                    ? "100%"
                                    : ((100 - ((double)this.FilesToCopy / this.TotalFiles) * 100))
                                    .ToString("0.00") + "%";

                                // For exit the loop
                                i = xmlObjects.Count;
                            }
                        }

                        fileStream.SetLength(0); // Truncate the file before writing
                    }

                    using (var fileStream = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.Write,
                               FileShare.ReadWrite))
                    {
                        serializer.Serialize(fileStream, xmlObjects);
                    }
                }
            }
        }

        /// <summary>
        /// Function who update dynamically the state file
        /// </summary>
        /// <param name="status"></param>
        public void CreateState(IConfiguration conf)
        {
            string xmlorjson = conf["JsonOrXml"];
            if (xmlorjson == "json")
            {
                lock (stateLockJson)
                {
                    string filepath = GeneralTools.conf["Log_Path"] + "\\state.json";

                    // Parse the string into a JObject
                    List<dynamic>? jsonObjects =
                        JsonConvert.DeserializeObject<List<dynamic>>(File.ReadAllText(filepath));

                    // Create JObject with our values
                    StateFile json = new StateFile(this.Appellation,
                        this.SourcePath,
                        this.TargetPath,
                        "ACTIVE",
                        this.TotalFiles.ToString(),
                        this.FilesSize.ToString(),
                        0 + "%",
                        this.FilesToCopy.ToString()
                    );

                    // Transform our object into a JObject
                    JObject jobject = JObject.FromObject(json);

                    if (jsonObjects != null)
                    {
                        jsonObjects.Add(jobject);
                    }
                    else
                    {
                        // Initialize list and add objects to list
                        jsonObjects = new List<dynamic> { jobject };
                    }

                    string updatedJson = JsonConvert.SerializeObject(jsonObjects, Formatting.Indented);
                    File.WriteAllText(filepath, updatedJson);
                }
            }
            else
            {
                lock(stateLockXml) {
                    string filepath = GeneralTools.conf["Log_Path"] + "\\state.xml";

                    // Create a new serializer for our object
                    XmlSerializer serializer = new XmlSerializer(typeof(List<StateFile>));

                    // Try to open the file in a shared mode
                    using (var fileStream = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.ReadWrite,
                               FileShare.ReadWrite))
                    {
                        List<StateFile>? xmlObjects;
                        if (fileStream.Length > 0)
                        {
                            // Read the existing XML file content into a List of StateFile objects
                            xmlObjects = (List<StateFile>)serializer.Deserialize(fileStream);
                        }
                        else
                        {
                            xmlObjects = new List<StateFile>();
                        }

                        // Create a new StateFile object
                        StateFile xmlStateFile = new StateFile(this.Appellation,
                            this.SourcePath,
                            this.TargetPath,
                            "ACTIVE",
                            this.TotalFiles.ToString(),
                            this.FilesSize.ToString(),
                            0 + "%",
                            this.FilesToCopy.ToString()
                        );

                        xmlObjects.Add(xmlStateFile);

                        // Serialize and write the updated List of StateFile objects to the XML file
                        fileStream.Position = 0;
                        serializer.Serialize(fileStream, xmlObjects);
                    }
                }
            }
        }

        /// <summary>
        /// Function who save a repository (completeSave and diffSave)
        /// </summary>
        /// <param name="sourceDirectory">The source directory we actually browse</param>
        /// <param name="targetDirectory">The target directory we want to add modifications</param>
        protected void RepositorySave(string? sourceDirectory, string? targetDirectory)
        {
            // Global param
            string? path = null;
            Stopwatch stopwatch = new Stopwatch();
            
            List<Process> pro = new List<Process>();
            
            List<string> cryptoList = GeneralTools.conf.GetSection("Crypto_ext").Get<List<string>>();

            // Params only used for DiffSave

            // This code is protected by the lock
            // and can only be accessed by one thread at a time
            if (sourceDirectory != null && targetDirectory != null ){
                foreach (var file in Directory.GetFiles(sourceDirectory))
                {
                    FileInfo fi = new FileInfo(file);
                    
                    // Used for calculate perf
                    var memory = new Microsoft.VisualBasic.Devices.ComputerInfo();
                    if (((_prioFilesExt && GeneralTools.conf.GetSection("Priority_Ext").Get<List<string>>()!=null 
                                        && GeneralTools.conf.GetSection("Priority_Ext").Get<List<string>>().Contains(fi.Extension)) 
                         || (!_prioFilesExt && GeneralTools.conf.GetSection("Priority_Ext").Get<List<string>>()!=null 
                                            && !GeneralTools.conf.GetSection("Priority_Ext").Get<List<string>>().Contains(fi.Extension))
                         || GeneralTools.conf.GetSection("Priority_Ext").Get<List<string>>() == null) && !_stopped)
                     {
                        path = Path.Combine(targetDirectory, Path.GetFileName(file));
                        var path2 = Path.Combine(sourceDirectory, Path.GetFileName(file));
                        var targetInfo = new FileInfo(path);
                        var sourceInfo = new FileInfo(path2);
                        long fileSize = 0;
                        stopwatch.Start();

                        sizeParallelFiles += fi.Length; // For recuperate the size in Ko

                        List<string> businessList =
                            GeneralTools.conf.GetSection("Business_Software").Get<List<string>>();
                        
                        // Return the perf (network load)
                        var usedMem = memory.TotalPhysicalMemory - memory.AvailablePhysicalMemory;
                        
                        while (_paused)
                        {
                            Task.Delay(100);
                        }
                        if ((sizeParallelFiles >
                             long.Parse(GeneralTools.conf["Limit_Parallel_Size"] ?? string.Empty) * 1024)) //|| (usedMem < float.Parse(GeneralTools.conf["Network_Load"] ?? string.Empty)))
                        {
                            lock (fileSizeLock)
                            {
                                if ((sourceInfo.LastWriteTime > targetInfo.LastWriteTime && this is DiffSave) ||
                                    this is CompleteSave)
                                {
                                    // pause if business software is running
                                    while (VerifyBusinessSoftwareRunning(businessList))
                                        MessageBox.Show("Un logiciel Metier est en cours d'execution, Veuillez le fermer pour finaliser la Sauvegarde");

                                    File.Copy(file, path, true);
                                }
                                else
                                {
                                    this.TotalFiles--;
                                }
                            }
                        }
                        else
                        {
                            if ((sourceInfo.LastWriteTime > targetInfo.LastWriteTime && this is DiffSave) ||
                                this is CompleteSave)
                            {
                                while (businessList != null && VerifyBusinessSoftwareRunning(businessList))
                                    MessageBox.Show("Un logiciel Metier est en cours d'execution, Veuillez le fermer pour finaliser la Sauvegarde");
                                File.Copy(file, path, true);
                            }
                            else
                            {
                                this.TotalFiles--;
                            }
                        }

                        stopwatch.Stop(); // Copy done so stop stopwatch
                        sizeParallelFiles -= fi.Length; // Save done so decrement

                        // Recup list of ext in the config file
                        fileSize = fi.Length;

                        if (cryptoList != null && cryptoList.Contains(fi.Extension))
                        {
                            pro.Add(new Process());

                            pro[^1].StartInfo = new ProcessStartInfo
                            {
                                FileName = GeneralTools.conf["Crypto_Path"],
                                Arguments = path + " " + GeneralTools.conf["Crypto_Path"] + " encrypt",
                                CreateNoWindow = true
                            };
                            pro[pro.Count - 1].Start();

                            // Create a Task for stop the stopwatch cause it need to wait the end of the process
                            pro[pro.Count - 1].WaitForExit();

                            // Different values we need for json
                            UpdateLogs(file, path, fileSize, stopwatch.Elapsed.ToString(),
                                pro[pro.Count - 1].ExitCode == 0
                                    ? (pro[pro.Count - 1].ExitTime - pro[pro.Count - 1].StartTime).ToString(
                                        @"hh\:mm\:ss\.fffffff")
                                    : "Error in encryption");
                        }
                        else
                        {
                            UpdateLogs(file, path, fileSize, stopwatch.Elapsed.ToString(), "Not encrypted");
                        }

                        this.FilesSize -= fileSize;
                        this.FilesToCopy--;
                        globalFilesLeftToDo -= fi.Length;
                        globalProgression = (1 - ((double)globalFilesLeftToDo / HomeView.GlobalSize)) * 100;

                        // Update our logs
                        UpdateState();
                        stopwatch.Reset();
                     }
                }
            }

            foreach (var directory in Directory.GetDirectories(sourceDirectory))
            {
                if (targetDirectory != null) path = Path.Combine(targetDirectory, Path.GetFileName(directory));
                RepositorySave(directory, path);
            }
        }

        protected void CreateDirectoriesOfADirectory()
        {
            if (SourcePath != null && TargetPath != null)
            {
                foreach (string subDirectory in Directory.GetDirectories(SourcePath, "*", SearchOption.AllDirectories))
                {
                    string relativePath = subDirectory.Replace(SourcePath, "");
                    string targetSubDirectory = Path.Combine(TargetPath, relativePath);
                    Directory.CreateDirectory(TargetPath + targetSubDirectory);
                }
            }
        }

        private bool VerifyBusinessSoftwareRunning(List<string> processes)
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
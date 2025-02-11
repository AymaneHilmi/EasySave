using System.Xml.Serialization;
using Newtonsoft.Json;

namespace EasySave
{

    public class LogFile
    {
        [JsonProperty("Name"), XmlElement("Name")]
        public string? Name { get; set; }

        [JsonProperty("FileSource"), XmlElement("FileSource")]
        public string FileSource { get; set; }

        [JsonProperty("FileTarget"), XmlElement("FileTarget")]
        public string? FileTarget { get; set; }

        [JsonProperty("DestPath"), XmlElement("DestPath")]
        public string? DestPath { get; set; }

        [JsonProperty("FileSize"), XmlElement("FileSize")]
        public string FileSize { get; set; }

        [JsonProperty("FileTransferTime"), XmlElement("FileTransfertTime")]
        public string FileTransferTime { get; set; }

        [JsonProperty("Time"), XmlElement("Time")]
        public string Time { get; set; }

        [JsonProperty("EncryptTime"), XmlElement("EncryptTime")]
        public string EncryptTime { get; set; }
        
        //Constructeur pas défaut
        public LogFile() { }
        
        //Constructeur personnalisé
        public LogFile(string? name, string fileSource, string? fileTarget, string? destPath, string fileSize, string fileTransferTime, string time, string encryptTime)
        {
            Name = name;
            FileSource = fileSource;
            FileTarget = fileTarget;
            DestPath = destPath;
            FileSize = fileSize;
            FileTransferTime = fileTransferTime;
            Time = time;
            EncryptTime = encryptTime;
        }
    }
}
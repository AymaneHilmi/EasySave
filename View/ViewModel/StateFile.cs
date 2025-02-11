using System.Xml.Serialization;
using Newtonsoft.Json;

namespace EasySave
{
    public class StateFile
    {
        [JsonProperty("Name"), XmlElement("Name")]
        public string? Name  {get; set; }

        [JsonProperty("FileSource"), XmlElement("FileSource")]
        public string? FileSource  {get; set; }

        [JsonProperty("FileTarget"), XmlElement("FileTarget")]
        public string? FileTarget  {get; set; }

        [JsonProperty("State"), XmlElement("State")]
        public string State  {get; set; }

        [JsonProperty("TotalFilesToCopy"), XmlElement("TotalFilesToCopy")]
        public string TotalFilesToCopy  {get; set; }

        [JsonProperty("TotalFilesSize"), XmlElement("TotalFilesSize")]
        public string TotalFilesSize  {get; set; }

        [JsonProperty("Progression"), XmlElement("Progression")]
        public string Progression  {get; set; }
        
        [JsonProperty("NbFilesLeftToDo"), XmlElement("NbFilesLeftToDo")]
        public string NbFilesLeftToDo  {get; set; }

        //Constructeur pas défaut
        public StateFile() { }
        
        //Constructeur personnalisé
        public StateFile(string? name, string? fileSource, string? fileTarget, string state, string totalFilesToCopy, string totalFilesSize, string progression, string nbFilesLeftToDo)
        {
            Name = name;
            FileSource = fileSource;
            FileTarget = fileTarget;
            State = state;
            TotalFilesToCopy = totalFilesToCopy;
            TotalFilesSize = totalFilesSize;
            Progression = progression;
            NbFilesLeftToDo = nbFilesLeftToDo;
        }
    }
}
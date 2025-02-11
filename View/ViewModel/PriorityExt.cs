using System.Collections.Generic;
using System.IO;
using EasySave;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace EasySaveApp.ViewModel
{
    public class PriorityExt
    {
        public List<string> Name { get; set; }
    
        public PriorityExt()
        {
            var businessList = GeneralTools.conf.GetSection("Priority_Ext").Get<List<string>>();
            this.Name = (businessList == null) ? new List<string>() : businessList;
        }

        public bool Add(string extensionName)
        {
            if (extensionName.Substring(0,1) != ".") extensionName = "." + extensionName;
            if (!this.Name.Contains(extensionName))
            {
                string app_path = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\ViewModel\appsettings.json");
                this.Name.Add(extensionName);
                var crypto = JsonConvert.DeserializeObject<Dictionary<string, object>>(File.ReadAllText(app_path));
                crypto["Priority_Ext"] = this.Name;
                var json = JsonConvert.SerializeObject(crypto, Formatting.Indented);
                File.WriteAllText(app_path, json);
                return true;
            }
            return false;
        }
    
        public bool Delete(string extensionName)
        {
            if (extensionName.Substring(0,1) != ".") extensionName = "." + extensionName;
            if (this.Name.Contains(extensionName))
            {
                string app_path = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\ViewModel\appsettings.json");
                this.Name.Remove(extensionName);
                var crypto = JsonConvert.DeserializeObject<Dictionary<string, object>>(File.ReadAllText(app_path));
                crypto["Priority_Ext"] = this.Name;
                var json = JsonConvert.SerializeObject(crypto, Formatting.Indented);

                File.WriteAllText(app_path, json);
                return true;
            }
            return false;
        }
    }   
}
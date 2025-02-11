using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace EasySave;

public class BusinessSoftware
{
    public List<string>? Name { get; set; }
    
     public BusinessSoftware()
     {
         var businessList = GeneralTools.conf.GetSection("Business_Software").Get<List<string>>();
         this.Name = (businessList == null) ? new List<string>() : businessList;
     }

    public bool Add(string processName)
    {
        if (verifyProcessExistence(processName) && !this.Name.Contains(processName))
        {
            string app_path = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\ViewModel\appsettings.json");
            this.Name.Add(processName);
            var business = JsonConvert.DeserializeObject<Dictionary<string, object>>(File.ReadAllText(app_path));
            business["Business_Software"] = this.Name;
            var json = JsonConvert.SerializeObject(business, Formatting.Indented);
            File.WriteAllText(app_path, json);
            return true;
        }
        return false;
    }
    
    public bool Delete(string processName)
    {
        if (this.Name.Contains(processName))
        {
            string app_path = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\ViewModel\appsettings.json");
            this.Name.Remove(processName);
            var business = JsonConvert.DeserializeObject<Dictionary<string, object>>(File.ReadAllText(app_path));
            business["Business_Software"] = this.Name;
            var json = JsonConvert.SerializeObject(business, Formatting.Indented);

            File.WriteAllText(app_path, json);
            return true;
        }
        return false;
    }

    public bool verifyProcessExistence(string processName)
    {
        Process[] processes = Process.GetProcesses();
        foreach (Process process in processes)
        {
            if (string.Equals(process.ProcessName, processName, StringComparison.OrdinalIgnoreCase))
                return true;
        }
        return false;
    }
}
using System.Collections.Generic;
using System.IO;
using EasySave;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace EasySaveApp.ViewModel;

public class NetworkLoad
{
    public NetworkLoad()
    {
    }

    public void Modify(long netLoadNumber)
    {
        string app_path = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\ViewModel\appsettings.json");
        var netLoad = JsonConvert.DeserializeObject<Dictionary<string, object>>(File.ReadAllText(app_path));
        netLoad["Network_Load"] = netLoadNumber;
        var json = JsonConvert.SerializeObject(netLoad, Formatting.Indented);
        File.WriteAllText(app_path, json);
    }
    
}
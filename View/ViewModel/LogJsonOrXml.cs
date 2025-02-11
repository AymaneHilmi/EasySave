using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace EasySaveApp.ViewModel;

public class LogJsonOrXml
{
    public LogJsonOrXml()
    {
    }

    public void ToJson()
    {
        string app_path = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\ViewModel\appsettings.json");
        var netLoad = JsonConvert.DeserializeObject<Dictionary<string, object>>(File.ReadAllText(app_path));
        netLoad["JsonOrXml"] = "json";
        var json = JsonConvert.SerializeObject(netLoad, Formatting.Indented);
        File.WriteAllText(app_path, json);
    }


    public void ToXml()
    {
        string app_path = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\ViewModel\appsettings.json");
        var netLoad = JsonConvert.DeserializeObject<Dictionary<string, object>>(File.ReadAllText(app_path));
        netLoad["JsonOrXml"] = "xml";
        var json = JsonConvert.SerializeObject(netLoad, Formatting.Indented);
        File.WriteAllText(app_path, json);
    }

}
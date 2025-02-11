using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.IO;
using EasySave;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace EasySaveApp.ViewModel;

public class MaxParallelSize
{
    public MaxParallelSize()
    {
    }

    public void Modify(long netLoadNumber)
    {
        string app_path = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\ViewModel\appsettings.json");
        var netLoad = JsonConvert.DeserializeObject<Dictionary<string, object>>(File.ReadAllText(app_path));
        netLoad["Limit_Parallel_Size"] = netLoadNumber;
        var json = JsonConvert.SerializeObject(netLoad, Formatting.Indented);
        File.WriteAllText(app_path, json);
    }

}

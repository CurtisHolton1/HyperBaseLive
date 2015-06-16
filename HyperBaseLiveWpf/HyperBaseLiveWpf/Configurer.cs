using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperBaseLiveWpf
{
  public static class Configurer
    {
     public static void UpdateConfig(KeyValuePair<string, string> kvp)
      {
          string appPath = System.IO.Path.GetDirectoryName(ConfigInfo.FinalLoc);

          string configFile = System.IO.Path.Combine(appPath.Substring(0, appPath.LastIndexOf("HyperBaseLiveWpf") + "HyperBaseLiveWpf".Length), "App.config");
          ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap();
          configFileMap.ExeConfigFilename = configFile;
          System.Configuration.Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
          config.AppSettings.Settings[kvp.Key].Value = kvp.Value;
          config.Save();
      }

      public static void UpdateConfig(List<KeyValuePair<string, string>> kvpList)
      {
          

          string configFile = System.IO.Path.Combine(ConfigInfo.FinalLoc,"Hyperbase.Live.Client.exe.config");
          ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap();
          configFileMap.ExeConfigFilename = configFile;
          System.Configuration.Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);       
          foreach(var kvp in kvpList){
              config.AppSettings.Settings[kvp.Key].Value = kvp.Value; 
          }
          config.Save();
      }
    }
}

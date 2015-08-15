
using System.Collections.Generic;
using System.Configuration;

namespace HyperBaseLiveWpf
{
    public class Client
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public string HBLStatus { get; set; }
        public string Location { get; set; }
        public int Id { get; set; }
        public int Version { get; set; }
        public string InstanceID { get; set; }
        public string HBLAssetDir { get; set; }
        public Client()
        {

        }
        public void UpdateConfig(List<KeyValuePair<string, string>> kvpList)
        {
            string configFile = System.IO.Path.Combine(Location, "Hyperbase.Live.Client.exe.config");
            ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap();
            configFileMap.ExeConfigFilename = configFile;
            System.Configuration.Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
            foreach (var kvp in kvpList)
            {
                config.AppSettings.Settings[kvp.Key].Value = kvp.Value;
            }
            config.Save();
        }

        public void UpdateAtlasSettings()
        {
            string configFile = System.IO.Path.Combine(Location, "Hyperbase.Live.Client.exe.config");
            ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap();
            configFileMap.ExeConfigFilename = configFile;
            System.Configuration.Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
            var sections = config.Sections;
            var sectionKeys = sections.Keys;
            string key = "";
            int i = 0;
            while (!key.Equals("atlas"))
            {
                key = sectionKeys.Get(i);
                i++;
            }

            string s = " ";
        }
        /// /////////////////////////////////////////////
        public bool Start()
        {
            return true;
        }
        public bool Stop()
        {
            return true;
        }
        public bool Update()
        {
            return true;
        }
        ///////////////////////////////////////////////
    }
}


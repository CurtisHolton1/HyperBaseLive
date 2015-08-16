
using HyperBaseLiveWpf.Helpers;
using HyperBaseLiveWpf.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace HyperBaseLiveWpf.Models
{
    public class Client
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public string HBLStatus { get; set; }
        public string Location { get; set; }
        public int ID { get; set; }
        public string Version { get; set; }
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
        private void WriteInstall()
        {
            using (StreamWriter sw = new StreamWriter(Location +"\\Install.bat"))
            { 
                sw.WriteLine("hyperbase.live.client /i");
            }
        }
        private void WriteStart()
        {
            using (StreamWriter sw = new StreamWriter(Location + "\\Start.bat"))
            {            
                sw.WriteLine("sc start \"" + Name + "\"");               
            }
        }
        private void WriteStop()
        {
            using (StreamWriter sw = new StreamWriter(Location +"\\Stop.bat"))
            {   
                sw.WriteLine("sc stop \"" + Name + "\"");
            }
        }
        private void WriteDelete()
        {
            using (StreamWriter sw = new StreamWriter(Location + "\\Delete.bat"))
            {
                sw.WriteLine("sc delete \"" + Name + "\"");
            }
        }
        public void Install()
        {
            if (!File.Exists(Location + "\\Install.bat"))
                WriteInstall();
            var proc1 = new ProcessStartInfo();
            proc1.UseShellExecute = true;
            proc1.WorkingDirectory = Location;
            proc1.FileName = Location + "\\Install.bat";
            proc1.Verb = "runas";
            proc1.CreateNoWindow = true;
            proc1.WindowStyle = ProcessWindowStyle.Hidden;
            var p = Process.Start(proc1);
            p.WaitForExit();
            ClientsView clientsViewWindow = (ClientsView)WindowWatcher.GetWindowOfType<ClientsView>();
            clientsViewWindow.AddClientButton.IsEnabled = true;
            clientsViewWindow.UpdateClientList();

        }
        public void Start()
        {
            if (!File.Exists(Location + "\\Start.bat"))
                WriteStart();
            var proc1 = new ProcessStartInfo();
            proc1.UseShellExecute = true;
            proc1.FileName = Location + "\\Start.bat";
            proc1.Verb = "runas";
            proc1.WorkingDirectory = Location;
            proc1.CreateNoWindow = true;
            proc1.WindowStyle = ProcessWindowStyle.Hidden;
            var p = Process.Start(proc1);
            p.WaitForExit();
            ClientsView clientsViewWindow = (ClientsView)WindowWatcher.GetWindowOfType<ClientsView>();
            if (clientsViewWindow != null)
            {
                clientsViewWindow.AddClientButton.IsEnabled = true;
                clientsViewWindow.UpdateClientList();
            }
        }
        public void Stop()
        {
            if (!File.Exists(Location + "\\Stop.bat"))
                WriteStop();
            var proc1 = new ProcessStartInfo();
            proc1.UseShellExecute = true;
            proc1.FileName = Location + "\\Stop.bat";
            proc1.Verb = "runas";
            proc1.WorkingDirectory = Location;
            proc1.CreateNoWindow = true;
            proc1.WindowStyle = ProcessWindowStyle.Hidden;
            var p = Process.Start(proc1);
            p.WaitForExit();
            ClientsView clientsViewWindow = (ClientsView)WindowWatcher.GetWindowOfType<ClientsView>();
            if (clientsViewWindow != null)
            {
                clientsViewWindow.AddClientButton.IsEnabled = true;
                clientsViewWindow.UpdateClientList();
            }
        }
        public void Delete()
        {
            if (!File.Exists(Location + "\\Delete.bat"))
                WriteStop();
            var proc1 = new ProcessStartInfo();
            proc1.UseShellExecute = true;
            proc1.FileName = Location + "\\Delete.bat";
            proc1.Verb = "runas";
            proc1.WorkingDirectory = Location;
            proc1.CreateNoWindow = true;
            proc1.WindowStyle = ProcessWindowStyle.Hidden;
            var p = Process.Start(proc1);
            p.WaitForExit();
            ClientsView clientsViewWindow = (ClientsView)WindowWatcher.GetWindowOfType<ClientsView>();
            if (clientsViewWindow != null)
            {
                clientsViewWindow.AddClientButton.IsEnabled = true;
                clientsViewWindow.UpdateClientList();
            }
        }
        public async void Update(GetServiceVersionResponse response)
        {
            this.Stop();
            this.Delete();
            Directory.Delete(Location);
            Directory.CreateDirectory(Location);           
            File.WriteAllBytes(Location, response.Bytes);
            this.Version = response.Version;
            DbManager dbM = new DbManager();
            await Task.Run(()=>dbM.AddOrUpdateClient(this));
        }       
    }
    }



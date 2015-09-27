
using HyperBaseLiveWpf.Helpers;
using HyperBaseLiveWpf.Models;
using HyperBaseLiveWpf.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace HyperBaseLiveWpf.Models
{
    public class Client : INotifyPropertyChanged
    {
        private string name;
        public string Name { get { return name; } set { name = value; this.OnPropertyChanged("Name"); } }
        private string status;
        public string Status { get { return status; } set { status = value; this.OnPropertyChanged("Status"); } }
        private string hblStatus;
        public string HBLStatus { get { return hblStatus; } set { hblStatus = value; this.OnPropertyChanged("HBLStatus"); } }
        private string location;
        public string Location { get { return location; } set { location = value; this.OnPropertyChanged("Location"); } }
        public int ID { get; set; }
        public string Version { get; set; }
        private string instanceID;
        public string InstanceID { get { return instanceID; } set { instanceID = value; this.OnPropertyChanged("InstanceID"); } }
        public string HBLAssetDir { get; set; }
        public string WinPass { get; set; }
        public Client()
        {

        }
        private void WriteInstall()
        {
            using (StreamWriter sw = new StreamWriter(Location + "\\Install.bat"))
            {
                sw.WriteLine("hyperbase.live.client /install -start /username=" + Environment.UserName +  " -password=" + WinPass +   " /accounttype=localsystem /startup=automatic");
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
            using (StreamWriter sw = new StreamWriter(Location + "\\Stop.bat"))
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
                WriteDelete();
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
        public async void Update(GetServiceVersionResponse response)
        {
            if (this.Version.CompareTo(response.Version) < 0 && response.Bytes != null)
            {
                try
                {
                    this.Status = "Updating";
                    this.Stop();
                    this.Delete();
                    Directory.Delete(Location, true);
                    Directory.CreateDirectory(Location);
                    File.WriteAllBytes(Location + "\\hblservice.zip", response.Bytes);
                    System.IO.Compression.ZipFile.ExtractToDirectory("hblsvc.zip", this.Location);
                    File.Delete(Location + "\\hblservice.zip");
                    this.Version = response.Version;
                    List<KeyValuePair<string, string>> configList = new List<KeyValuePair<string, string>>();
                    configList.Add(new KeyValuePair<string, string>("finalLoc", this.Location));
                    configList.Add(new KeyValuePair<string, string>("instanceId", this.InstanceID));
                    configList.Add(new KeyValuePair<string, string>("HBLAssetDir", this.HBLAssetDir));
                    UpdateConfig(configList);
                    this.Install();
                    this.Start();
                }
                catch (Exception e)
                {
                    System.Windows.MessageBox.Show("Exception in client.update: " + e.Message);
                }
                DbManager dbM = new DbManager();
                await Task.Run(() => dbM.AddOrUpdateClient(this));
            }
        }
        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}



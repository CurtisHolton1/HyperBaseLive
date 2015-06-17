﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows.Forms;
using System.Net;
using System.Diagnostics;
using System.IO;
namespace HyperBaseLiveWpf.Views
{
    /// <summary>
    /// Interaction logic for InstallServiceView.xaml
    /// </summary>
    public partial class InstallServiceView : Window, INotifyPropertyChanged
    {
        CancellationTokenSource cts;
        private string buttonContent;
        public string ButtonContent { get { return buttonContent; } set { buttonContent = value; this.OnPropertyChanged("ButtonContent"); } }
        private string statusLabelContent;
        public string StatusLabelContent { get { return statusLabelContent; } set { statusLabelContent = value; this.OnPropertyChanged("StatusLabelContent"); } }
        private WebClient client;
        public InstallServiceView()
        {
            this.DataContext = this;
            InitializeComponent();
            InstallBar.Maximum = 100;
            InstallBar.Value = 0;
            ButtonContent = "Cancel";
            StatusLabelContent = "Downloading...";
            client = new WebClient();
            
            Download(); // and install in turn
        }



        private void Download()
        {
            try
            {
                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                client.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                client.DownloadFileAsync(new Uri("http://q.hyperbase-live.com/hblsvc.zip"), "hblsvc.zip");
                
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show("Error downloading file: " + e.Message);
                HandleException();
            }
        }
        private void Install()
        {
            StatusLabelContent = "Unzipping...";
            try
            {

                System.IO.Compression.ZipFile.ExtractToDirectory("hblsvc.zip", ConfigInfo.FinalLoc);

            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show("Error Unzipping File:" + e.Message);
                
                HandleException();
                return;
            }
            StatusLabelContent = "Configuring...";
            try
            {
                InstallBar.Value += 10;
                List<KeyValuePair<string, string>> configList = new List<KeyValuePair<string, string>>();
                configList.Add(new KeyValuePair<string, string>("finalLoc", ConfigInfo.FinalLoc));
                configList.Add(new KeyValuePair<string, string>("instanceId", ConfigInfo.InstanceId));
                configList.Add(new KeyValuePair<string, string>("HBLAssetDir", ConfigInfo.HBLAssetDir));
                configList.Add(new KeyValuePair<string, string>("user", ConfigInfo.User));
                configList.Add(new KeyValuePair<string, string>("pass", ConfigInfo.Password));
                Configurer.UpdateConfig(configList);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error Writing to File:" + ex.Message);
                HandleException();
                return;
            }
            try
            {
                InstallBar.Value = 100;
                var proc1 = new ProcessStartInfo();
                proc1.UseShellExecute = false;
                proc1.WorkingDirectory = ConfigInfo.FinalLoc;
                proc1.FileName = @"C:\Windows\System32\cmd.exe";
                proc1.Verb = "runas";
                proc1.Arguments = "/k " + "Hyperbase.Live.Client /i";
                proc1.WindowStyle = ProcessWindowStyle.Normal;
                Process.Start(proc1);

                InstallComplete();
            }
            catch (Exception exc)
            {
                HandleException();
            }
        }
        private void HandleException()
        {
            System.IO.File.Delete(System.IO.Path.Combine(ConfigInfo.FinalLoc, "hblsvc.zip"));
            InstallBar.Visibility = Visibility.Hidden;
            StatusLabelContent = "Installation Cancelled";
            ButtonContent = "Close";
            WrapUp();
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                HandleException();
            }
            else
            Install();
        }

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {           
            InstallBar.Value = (double)e.ProgressPercentage / 1.5; 
        }
        private void BottomButton_Click(object sender, RoutedEventArgs e)
        {
            if (ButtonContent.Equals("Cancel"))
            {
                client.CancelAsync();
            }
            else
            {
                this.Close();
            }
        }

        private void InstallComplete()
        {
            InstallBar.Visibility = Visibility.Hidden;
            InstallCompleteLabel.Visibility = Visibility.Visible;
            StatusLabel.Visibility = Visibility.Hidden;
            Launch.Visibility = Visibility.Visible;
            AddClientToFile();
            ButtonContent = "Finish";
            WrapUp();
        }

        private async void AddClientToFile()
        {
            string filePath = "../../Clients.txt";
            string text = ConfigInfo.ClientName + "\t" + ConfigInfo.FinalLoc + "\n";
            await WriteTextAsync(filePath, text);
        }

        private async Task WriteTextAsync(string filePath, string text)
        {
            
                byte[] encodedText = Encoding.Default.GetBytes(text);
                using (FileStream sourceStream = new FileStream(filePath,
                    FileMode.Append, FileAccess.Write, FileShare.None,
                    bufferSize: 4096, useAsync: true))
                {
                    await sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
                };
            
        }

        private void WrapUp()
        {
            foreach (Window w in App.Current.Windows)
            {
                if (w is ClientsView)
                {
                    (w as ClientsView).AddClientButton.IsEnabled = true;
                    break;
                }
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

        private void Launch_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //Launch Application
        }
    }
}
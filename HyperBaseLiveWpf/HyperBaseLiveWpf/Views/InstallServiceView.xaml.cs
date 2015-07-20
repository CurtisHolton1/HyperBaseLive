using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Net;
using System.IO;
using System.Timers;
using HyperBaseLiveWpf.Helpers;
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
        private int timeoutCount;
        System.Timers.Timer timeout;
        public InstallServiceView()
        {
            this.DataContext = this;
            InitializeComponent();
            InstallBar.Maximum = 100;
            InstallBar.Value = 0;
            ButtonContent = "Cancel";
            StatusLabelContent = "Downloading...";
            client = new WebClient();
            timeout = new System.Timers.Timer(15000);
            timeout.Elapsed += timeout_Elapsed;
            timeoutCount = 0;
            timeout.AutoReset = false;
            timeout.Enabled = true;
            WindowWatcher.AddWindow(this);
            Download(); // and install in turn
        }


     private void timeout_Elapsed(object source, ElapsedEventArgs e){
         try
         {
             timeout.Enabled = false;
             timeout.Stop();
             timeout.Dispose();
             timeout = null;
             if (WindowWatcher.Contains(this))
             {
                 System.Windows.MessageBox.Show("The connection has timed out. Please check your internet connection and try again.");
                 client.CancelAsync();
             }
         }
         catch (Exception ex)
         {
             Console.WriteLine("Error in Timeout method");
         }
         
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
                BatchManager bm = new BatchManager();
                bm.WriteInstall(ConfigInfo.FinalLoc);
                bm.WriteStart("HyperBase Client");
                bm.WriteStop("HyperBase Client");
                bm.LaunchInstall();            
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
     
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled)
            {

                HandleException();
            }
            else
            {
                timeout.Enabled = false;
                timeout.Stop();
                timeout.Dispose();
                timeout = null;
                Install();
            }
        }

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {           
            InstallBar.Value = (double)e.ProgressPercentage / 1.5;
            if (timeout != null)
            {
                timeout.Stop();
                timeout.Start();
                timeout.Enabled = true;
            }
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

        private void Window_Closed(object sender, EventArgs e)
        {    
            
            client.CancelAsync();
            
            WindowWatcher.RemoveWindow(this);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Net;
using System.Timers;
using HyperBaseLiveWpf.Helpers;
using HyperBaseLiveWpf.Menu;

namespace HyperBaseLiveWpf.Views
{
    /// <summary>
    /// Interaction logic for InstallClientView.xaml
    /// </summary>
    public partial class InstallClientView : Window, INotifyPropertyChanged
    {
        CancellationTokenSource cts;
        private string buttonContent;
        public string ButtonContent { get { return buttonContent; } set { buttonContent = value; this.OnPropertyChanged("ButtonContent"); } }
        private string statusLabelContent;
        public string StatusLabelContent { get { return statusLabelContent; } set { statusLabelContent = value; this.OnPropertyChanged("StatusLabelContent"); } }
        private WebClient webClient1;
        private Client clientToInstall;
        private int timeoutCount;
        System.Timers.Timer timeout;
        public InstallClientView(Client clientToInstall)
        {
            this.DataContext = this;
            this.clientToInstall = clientToInstall;
            InitializeComponent();
            InstallBar.Maximum = 100;
            InstallBar.Value = 0;
            ButtonContent = "Cancel";
            StatusLabelContent = "Downloading...";
            webClient1 = new WebClient();
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
                 webClient1.CancelAsync();
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
                webClient1.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                webClient1.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadCompleted);
                webClient1.DownloadFileAsync(new Uri("http://q.hyperbase-live.com/hblsvc.zip"), "hblsvc.zip");            
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

                System.IO.Compression.ZipFile.ExtractToDirectory("hblsvc.zip", clientToInstall.Location);

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
                configList.Add(new KeyValuePair<string, string>("finalLoc", clientToInstall.Location));
                configList.Add(new KeyValuePair<string, string>("instanceId", clientToInstall.InstanceID));
                configList.Add(new KeyValuePair<string, string>("HBLAssetDir", clientToInstall.HBLAssetDir));
                //configList.Add(new KeyValuePair<string, string>("id", ConfigInfo.Id));
                clientToInstall.UpdateConfig(configList);
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
                bm.WriteInstall(clientToInstall.Location);
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
            System.IO.File.Delete(System.IO.Path.Combine(clientToInstall.Location, "hblsvc.zip"));
            InstallBar.Visibility = Visibility.Hidden;
            StatusLabelContent = "Installation Cancelled";
            ButtonContent = "Close";
     
        }

        private void DownloadCompleted(object sender, AsyncCompletedEventArgs e)
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
                webClient1.CancelAsync();
            }
            else
            {
                this.Close();
            }
        }

        private async Task<string> InstallComplete()
        {
            InstallBar.Visibility = Visibility.Hidden;
            InstallCompleteLabel.Visibility = Visibility.Visible;
            StatusLabel.Visibility = Visibility.Hidden;
            Launch.Visibility = Visibility.Visible;
            await Task.Run(()=>ClientFileManager.AddClientToFile(clientToInstall));
            BatchManager bm = new BatchManager();
            bm.WriteStart("HyperBase Client");
            bm.LaunchStart();          
            ButtonContent = "Finish";
            return "";
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
            
            webClient1.CancelAsync();
            
            WindowWatcher.RemoveWindow(this);
        }
    }
}

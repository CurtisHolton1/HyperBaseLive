using System;
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
        private string hyperBaseFolder;
        private string serviceFolder;
        private string clientID;
        public InstallServiceView()
        {
            this.DataContext = this;
            InitializeComponent();
            InstallBar.Maximum = 100;
            InstallBar.Value = 0;

            ButtonContent = "Cancel";
            StatusLabelContent = "Downloading...";
            Download();
           
        }



        private void Download()
        {
            try
            {
                var client = new WebClient();
                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                client.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                client.DownloadFileAsync(new Uri("http://q.hyperbase-live.com/hblsvc.zip"), "hblsvc.zip");
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show("Error downloading file: " + e.Message);
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
            }
            statusLabelContent = "Configuring...";
                try
                {
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
                }
                try
                {
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

                }
        }

      private void Completed(object sender, AsyncCompletedEventArgs e){
          Install();
          
          
      }


        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            InstallBar.Value = (double)e.ProgressPercentage/1.5;
        }


     

        private void BottomButton_Click(object sender, RoutedEventArgs e)
        {
            if (ButtonContent.Equals("Cancel"))
            {

            }
              
            else{
                this.Close();
            }
        }

        private void OnCancel()
        {
            StatusLabelContent = "Installation Cancelled";
            BottomButton.IsEnabled = false;
        }

        private void InstallComplete()
        {
            InstallBar.Visibility = Visibility.Hidden;
            InstallCompleteLabel.Visibility = Visibility.Visible;
            StatusLabel.Visibility = Visibility.Hidden;
            Launch.Visibility = Visibility.Visible;
            ButtonContent = "Finish";
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

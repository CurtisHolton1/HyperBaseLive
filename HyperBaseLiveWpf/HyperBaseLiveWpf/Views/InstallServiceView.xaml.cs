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
        public InstallServiceView(string hyperBaseFolder, string serviceFolder)
        {
            this.DataContext = this;
            InitializeComponent();
            InstallBar.Maximum = 100;
            InstallBar.Value = 0;
            this.hyperBaseFolder = hyperBaseFolder;
            this.serviceFolder = serviceFolder;
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
                System.IO.Compression.ZipFile.ExtractToDirectory("hblsvc.zip", serviceFolder);
                InstallBar.Value = 100;
                InstallComplete();
            }
            catch (Exception e) {
                System.Windows.MessageBox.Show("Error Unzipping File:" + e.Message);
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
                cts.Cancel();
            else
                this.Close();
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

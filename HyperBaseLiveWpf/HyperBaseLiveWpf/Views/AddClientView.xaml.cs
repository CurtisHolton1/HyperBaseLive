using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.ComponentModel;
using HyperBaseLiveWpf.Helpers;
using System.Threading.Tasks;
using HyperBaseLiveWpf.Models;
using System.Linq;

namespace HyperBaseLiveWpf.Views
{
    /// <summary>
    /// Interaction logic for AddClient.xaml
    /// </summary>
    public partial class AddClientView : Window, INotifyPropertyChanged
    {
        private string serviceFolderBrowserText;
        public string ServiceFolderBrowserText { get { return serviceFolderBrowserText; } set { serviceFolderBrowserText = value; this.OnPropertyChanged("ServiceFolderBrowserText"); } }
        private string hyperSpinFolderBrowserText;
        public string HyperSpinFolderBrowserText { get { return hyperSpinFolderBrowserText; } set { hyperSpinFolderBrowserText = value; this.OnPropertyChanged("HyperSpinFolderBrowserText"); } }
        private string clientNameText;
        public string ClientNameText { get { return clientNameText; } set { clientNameText = value; this.OnPropertyChanged("ClientNameText"); } }
        private Client clientToInstall;
        //private string error1;
        //public string Error1 { get { return error1; } set { error1 = value; this.OnPropertyChanged("Error1"); } }
        //private string error2;
        //public string Error2 { get { return error2; } set { error2 = value; this.OnPropertyChanged("Error2"); } }

        public AddClientView(Client clientToInstall)
        {
            this.clientToInstall = clientToInstall;
            InitializeComponent();
            this.DataContext = this;
            ClientNameText = clientToInstall.Name;
            HyperSpinFolderBrowserText = @"C:\HyperSpin";
            if(Directory.Exists(@"C:\Program Files(x86)"))
                ServiceFolderBrowserText = @"C:\Program Files (x86)\Hyperbase\Hyperbase Live";
            else
                ServiceFolderBrowserText = @"C:\Program Files\Hyperbase\Hyperbase Live";
            WindowWatcher.AddWindow(this);
        }

        private void ServiceButton_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            try
            {
                DialogResult result = fbd.ShowDialog();
                if (result.ToString().Equals("OK"))
                    ServiceFolderBrowserText = fbd.SelectedPath;

            }
            catch (Exception) { }
        }

        private async void NextButton_Click(object sender, RoutedEventArgs e)
        {
            this.Error1.Visibility = Visibility.Hidden;
            this.Error2.Visibility = Visibility.Hidden;
            try
            {
                if (!Directory.Exists(ServiceFolderBrowserText))
                {
                    Directory.CreateDirectory(ServiceFolderBrowserText);
                }
                if (!Directory.Exists(HyperSpinFolderBrowserText))
                {
                    Directory.CreateDirectory(HyperSpinFolderBrowserText);
                }

                if (Directory.Exists(HyperSpinFolderBrowserText) && Directory.Exists(ServiceFolderBrowserText))
                {
                    if (IsDirectoryEmpty(ServiceFolderBrowserText))
                    {
                        this.NextButton.IsEnabled = false;
                        clientToInstall.Location = serviceFolderBrowserText;
                        clientToInstall.HBLAssetDir = hyperSpinFolderBrowserText;
                        /////////////////////////////////////////////////////
                        //  TODO REPLACE
                        clientToInstall.Name = "HyperBase Client";
                        var versionResponse = await Task.Run(() => HblApiCaller.GetServiceVersion());
                        if (versionResponse != null)
                        {
                            clientToInstall.Version = versionResponse.Version;
                            ///////////////////////////
                            var wnd = new InstallClientView(clientToInstall);
                            wnd.Show();
                            this.Close();
                        }
                    }
                    else
                    {
                        this.Error2.Text = "Folder Must Be empty";
                        this.Error2.Visibility = Visibility.Visible;
                    }
                }
            }
            catch (Exception exce)
            {
                System.Windows.MessageBox.Show(exce.Message);
                throw exce;
            }
        }

        private bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }
        private void HyperSpinButton_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            try
            {
                DialogResult result = fbd.ShowDialog();
                HyperSpinFolderBrowserText = fbd.SelectedPath;
            }
            catch (Exception) { }
        }
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        private void Window_Closed(object sender, EventArgs e)
        {
            WindowWatcher.RemoveWindow(this);
        }


    }
}

using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.ComponentModel;


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
        //private string error1;
        //public string Error1 { get { return error1; } set { error1 = value; this.OnPropertyChanged("Error1"); } }
        //private string error2;
        //public string Error2 { get { return error2; } set { error2 = value; this.OnPropertyChanged("Error2"); } }

        public AddClientView(string clientName)
        {
            InitializeComponent();
            this.DataContext = this;
            ClientNameText = clientName.Substring(1,clientName.Length-2);
            HyperSpinFolderBrowserText = @"C:\HyperSpin";
            ServiceFolderBrowserText = @"C:\Program Files\HyperBaseLive\Services";
            WindowWatcher.AddWindow(this);
        }

        private void ServiceButton_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            try
            {
                DialogResult result = fbd.ShowDialog();             
                ServiceFolderBrowserText = fbd.SelectedPath;
                
            }
            catch (Exception ) { }
        }
             
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
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
                    ConfigInfo.HBLAssetDir = HyperSpinFolderBrowserText;
                    ConfigInfo.FinalLoc = ServiceFolderBrowserText;
                    ConfigInfo.ClientName = ClientNameText;
                    Client clientToInstall = new Client { Name = ClientNameText, Location = ServiceFolderBrowserText };
                    var wnd = new InstallClientView(clientToInstall);
                    wnd.Show();
                    this.Close();
                }
            }
            catch (Exception exce)
            {
                System.Windows.MessageBox.Show(exce.Message);
                throw exce;
            }
            //else
            //{
            //    if (!Directory.Exists(ServiceFolderBrowserText))
            //    {
            //        //Error2.Text = "Directory does not exist";
            //        //Error2.Visibility = Visibility.Visible;
            //        Directory.CreateDirectory(ServiceFolderBrowserText);
            //    }
            //    else
            //    {
            //       // Error2.Visibility = Visibility.Hidden;
            //    }

            //    if (!Directory.Exists(HyperSpinFolderBrowserText))
            //    {
            //        Directory.CreateDirectory(HyperSpinFolderBrowserText);
            //        //Error1.Text = "Directory does not exist";
            //        //Error1.Visibility = Visibility.Visible;
            //    }
            //    else
            //    {
            //        //Error1.Visibility = Visibility.Hidden;
            //    }
               

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

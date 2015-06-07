using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.ComponentModel;


namespace HyperBaseLiveWpf.Views
{
    /// <summary>
    /// Interaction logic for AddClient.xaml
    /// </summary>
    public partial class AddClientView : Window, INotifyPropertyChanged
    {
        private string folderBrowserText;
        public string FolderBrowserText { get { return folderBrowserText; } set { folderBrowserText = value; this.OnPropertyChanged("FolderBrowserText"); } }
        private string clientNameText;
        public string ClientNameText { get { return clientNameText; } set { clientNameText = value; this.OnPropertyChanged("ClientNameText"); } }
        private string clientIDText;
        public string ClientIDText { get { return clientIDText; } set { clientIDText = value; this.OnPropertyChanged("ClientIDText"); } }

        public AddClientView()
        {
            InitializeComponent();
            this.DataContext = this;
            FolderBrowserText = "Install Location";
            ClientNameText = "Client Name";
            ClientIDText = "Client ID";
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            try
            {
                DialogResult result = fbd.ShowDialog();
                FolderBrowserText = fbd.SelectedPath;
                
            }
            catch (Exception ) { }
        }



        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        private void ClientNameBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (ClientNameText.Equals("Client Name"))           
                ClientNameText = "";                     
        }

        private void FolderBrowserBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (FolderBrowserText.Equals("Install Location"))
                FolderBrowserText = "";
        }

        private void ClientID_GotFocus(object sender, RoutedEventArgs e)
        {
            if (ClientIDText.Equals("Client ID"))
                ClientIDText = "";
        }

        private void ClientNameBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ClientNameText.Equals(""))
                ClientNameText = "Client Name";
        }

        private void FolderBrowserBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (FolderBrowserText.Equals(""))
                FolderBrowserText = "Install Location";
        }

        private void ClientID_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ClientIDText.Equals(""))
                ClientIDText = "Client ID";
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {           
            var wnd = new InstallServiceView();
            wnd.Show();
            this.Close();
        }

        private void Generate_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ClientIDText = "GENERATED ID HERE";
        }


    }
}

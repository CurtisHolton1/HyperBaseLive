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
        private string serviceFolderBrowserText;
        public string ServiceFolderBrowserText { get { return serviceFolderBrowserText; } set { serviceFolderBrowserText = value; this.OnPropertyChanged("ServiceFolderBrowserText"); } }
        private string hyperSpinFolderBrowserText;
        public string HyperSpinFolderBrowserText { get { return hyperSpinFolderBrowserText; } set { hyperSpinFolderBrowserText = value; this.OnPropertyChanged("HyperSpinFolderBrowserText"); } }
        private string clientNameText;
        public string ClientNameText { get { return clientNameText; } set { clientNameText = value; this.OnPropertyChanged("ClientNameText"); } }
       

        public AddClientView(string ClientName)
        {
            InitializeComponent();
            this.DataContext = this;
            ClientNameText = ClientName.Substring(1,ClientName.Length-2);
            HyperSpinFolderBrowserText = @"C:\HyperSpin";
            ServiceFolderBrowserText = @"C:\Programs\HyperBaseLive\Services";
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
            var wnd = new InstallServiceView(HyperSpinFolderBrowserText, ServiceFolderBrowserText);
            wnd.Show();
            this.Close();
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
       

    }
}

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
        public string FolderBrowserText { get { return folderBrowserText; } set { folderBrowserText = value;  this.OnPropertyChanged("FolderBrowserText"); } }

        public AddClientView()
        {
            InitializeComponent();
            this.DataContext = this;
            FolderBrowserText = "Install Location";
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

    }
}

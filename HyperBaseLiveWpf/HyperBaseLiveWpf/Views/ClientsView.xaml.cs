using HyperBaseLiveWpf.Helpers;
using HyperBaseLiveWpf.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.ServiceProcess;
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
using System.Timers;

namespace HyperBaseLiveWpf
{
    /// <summary>
    /// Interaction logic for Clients.xaml
    /// </summary>
    public partial class ClientsView : Window, INotifyPropertyChanged
    {
        private List<Client> dataList = new List<Client>();
        public List<Client> DataList { get { return dataList; } set { dataList = value; this.OnPropertyChanged("DataList"); } }
        System.Timers.Timer timer1;

        public ClientsView()
        {
            InitializeComponent();
            this.DataContext = this;
            WindowWatcher.AddWindow(this);
            DataList = ClientFileManager.DetermineClients();
            timer1 = new System.Timers.Timer(2000);
            timer1.Elapsed += timer1_Elapsed;           
            timer1.AutoReset = true;
            timer1.Enabled = true;
        }

        private void timer1_Elapsed(object sender, ElapsedEventArgs e)
        {
            UpdateClientList();
        }

        public void UpdateClientList()
        {
            DataList = ClientFileManager.DetermineClients();
        }

        private void AddClientButton_Click(object sender, RoutedEventArgs e)
        {
            AddClientButton.IsEnabled = false;
            var w = new ValidateIDView();
            w.Show();
            w.Activate();          
        }

        //private void RefreshButton_Click(object sender, RoutedEventArgs e)
        //{
        //    RefreshButton.IsEnabled = false;   
        //    DataList =  ClientFileManager.DetermineClients();
        //    RefreshButton.IsEnabled = true;        
        //}

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            WindowWatcher.RemoveWindow(this);
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

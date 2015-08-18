using HyperBaseLiveWpf.Helpers;
using HyperBaseLiveWpf.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Timers;
using HyperBaseLiveWpf.Models;

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
        private static DateTime lastHBLStatusCheck;
        public ClientsView()
        {
            InitializeComponent();
            this.DataContext = this;         
            WindowWatcher.AddWindow(this);
            AsyncWrapper();
            timer1 = new System.Timers.Timer(2000);
            timer1.Elapsed += timer1_Elapsed;
            timer1.AutoReset = true;
            timer1.Enabled = true;
        }
        private async void AsyncWrapper()
        {
            DbManager dbM = new DbManager();        
            DataList = await Task.Run(() => dbM.GetAllClients());
            foreach (var c in DataList)
            {
                c.Status = ClientManager.GetServiceStatus(c.Name);
            }
        }

        private async void timer1_Elapsed(object sender, ElapsedEventArgs e)
        {                           
                UpdateClientList();                     
        }

        public async void UpdateClientList()
        {                    
            foreach(var c in DataList)
            {
                c.Status = ClientManager.GetServiceStatus(c.Name);
            }
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

        private async void MainWindow_Activated(object sender, EventArgs e)
        {          
            if (lastHBLStatusCheck == null || (DateTime.Now - lastHBLStatusCheck).Minutes > 5 )
            {
                lastHBLStatusCheck = DateTime.Now;
                var HBLStatus = await Task.Run(() => HblApiCaller.CheckStatus());
                foreach (var c in DataList)
                {
                    c.HBLStatus = HBLStatus;
                }
            }
        }

        private void ContextMenu_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        private void StartItem_Click(object sender, RoutedEventArgs e)
        {
          var c =  (Client)this.DataGrid1.SelectedItem;
            c.Start();
        }

        private void StopItem_Click(object sender, RoutedEventArgs e)
        {
            var c = (Client)this.DataGrid1.SelectedItem;
            c.Stop();
        }

        private async void UpdateItem_Click(object sender, RoutedEventArgs e)
        {
            var c = (Client)this.DataGrid1.SelectedItem;
            var response = await Task.Run(()=> HblApiCaller.GetServiceVersion());
            if(response != null)
             c.Update(response);
        }
    }
}

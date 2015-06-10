using HyperBaseLiveWpf.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace HyperBaseLiveWpf
{
    /// <summary>
    /// Interaction logic for Clients.xaml
    /// </summary>
    public partial class ClientsView : Window, INotifyPropertyChanged
    {
        private List<DataObject> dataList = new List<DataObject>();
        public List<DataObject> DataList { get { return dataList; } set { dataList = value; } }
          

        public ClientsView()
        {
            InitializeComponent();
            this.DataContext = this;
            DataList.Add(new DataObject { Name = "Client1", Status = "Client1Status", HBLStatus = "Live" });
            DataList.Add(new DataObject { Name = "Client2", Status = "Client2Status", HBLStatus = "Live" });
            DataList.Add(new DataObject { Name = "Client3", Status = "Client3Status", HBLStatus = "Live" });
            DataList.Add(new DataObject { Name = "Client4", Status = "Client4Status", HBLStatus = "Live" });
            DataList.Add(new DataObject { Name = "Client5", Status = "Client5Status", HBLStatus = "Live" });
            DataList.Add(new DataObject { Name = "Client6", Status = "Client6Status", HBLStatus = "Live" });
            DataList.Add(new DataObject { Name = "Client7", Status = "Client7Status", HBLStatus = "Live" });
            DataList.Add(new DataObject { Name = "Client8", Status = "Client8Status", HBLStatus = "Live" });

            DataList.Add(new DataObject { Name = "Client1", Status = "Client1Status", HBLStatus = "Live" });
            DataList.Add(new DataObject { Name = "Client2", Status = "Client2Status", HBLStatus = "Live" });
            DataList.Add(new DataObject { Name = "Client3", Status = "Client3Status", HBLStatus = "Live" });
            DataList.Add(new DataObject { Name = "Client4", Status = "Client4Status", HBLStatus = "Live" });
            DataList.Add(new DataObject { Name = "Client5", Status = "Client5Status", HBLStatus = "Live" });
            DataList.Add(new DataObject { Name = "Client6", Status = "Client6Status", HBLStatus = "Live" });
            DataList.Add(new DataObject { Name = "Client7", Status = "Client7Status", HBLStatus = "Live" });
            DataList.Add(new DataObject { Name = "Client8", Status = "Client8Status", HBLStatus = "Live" });


        }

        private void AddClientButton_Click(object sender, RoutedEventArgs e)
        {
            Window wnd = new ValidateIDView();           
            wnd.Show();
            this.Close();
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

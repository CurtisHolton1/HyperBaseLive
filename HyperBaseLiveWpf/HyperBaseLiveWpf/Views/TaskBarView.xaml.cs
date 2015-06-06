using HyperBaseLiveWpf.Menu;
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
    /// Interaction logic for TaskBar.xaml
    /// </summary>
    public partial class TaskBarView : Window, INotifyPropertyChanged
    {
        public string UserImg { get; set; }
        public List<string> SyncHistory { get; set; }
        public List<IMenuItem> MenuOptions { get; set; }
        public int OutSideWidth { get; set; }
        public int OutSideHeight { get; set; }
        public TaskBarView()
        {
           
            this.DataContext = this;
            UserImg = "../Content/profile.jpg";
            SyncHistory = new List<string>();
            MenuOptions = new List<IMenuItem>();
            InitializeComponent();            
            SyncHistory.Add("Item1");
            SyncHistory.Add("Item2");
            SyncHistory.Add("Item3");
            SyncHistory.Add("Item3");
            SyncHistory.Add("Item4");
            SyncHistory.Add("Item5");
            SyncHistory.Add("Item6");
            SyncHistory.Add("Item7");
            SyncHistory.Add("Item8");
            SyncHistory.Add("Item9");
            SyncHistory.Add("Item10");
            SyncHistory.Add("Item11");
            MenuOptions.Add(MenuStaticClassFactory.GetStartClass());
            MenuOptions.Add(MenuStaticClassFactory.GetStopClass());
            MenuOptions.Add(MenuStaticClassFactory.GetUpdateClass());
            MenuOptions.Add(MenuStaticClassFactory.GetClientsClass());
            MenuOptions.Add(MenuStaticClassFactory.GetExitClass());
         
        }
        

        private void CustomExpanderButtom_Click(object sender, RoutedEventArgs e)
        {
            if (CustomExpanderMenu.Visibility == Visibility.Collapsed)
                CustomExpanderMenu.Visibility = Visibility.Visible;
            else
                CustomExpanderMenu.Visibility = Visibility.Collapsed;
        }
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        private void Menu_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = (IMenuItem)Menu.SelectedItem;
            item.PerformAction();
        }



    }
}

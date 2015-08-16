using HyperBaseLiveWpf.Helpers;
using HyperBaseLiveWpf.Menu;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace HyperBaseLiveWpf
{
    /// <summary>
    /// Interaction logic for TaskBar.xaml
    /// </summary>
    public partial class TaskBarView : Window, INotifyPropertyChanged
    {
        // public string UserImg { get; set; }
        // public List<string> SyncHistory { get; set; }
        //public List<IMenuItem> MenuOptions { get; set; }
        // public int OutSideWidth { get; set; }
        // public int OutSideHeight { get; set; }
        private List<MenuItem> menu;
        public List<MenuItem> Menu { get { return menu; } set { menu = value; this.OnPropertyChanged("Menu"); } }
     
       /*
        *uncomment all commented code for custom taskbar menu
        */
        public TaskBarView()
        {                      
            this.DataContext = this;
           
           // UserImg = "../Content/profile.jpg";
            //SyncHistory = new List<string>();
           // MenuOptions = new List<IMenuItem>();
            InitializeComponent();
            //SyncHistory.Add("Item1");           
            //MenuOptions.Add(MenuStaticClassFactory.GetStartClass());
            //MenuOptions.Add(MenuStaticClassFactory.GetStopClass());
            //MenuOptions.Add(MenuStaticClassFactory.GetUpdateClass());
            //MenuOptions.Add(MenuStaticClassFactory.GetClientsClass());
            //MenuOptions.Add(MenuStaticClassFactory.GetExitClass());
            //Menu = new List<MenuItem>();
            //Menu.Add(new MenuItem { Header = "Start" });
            //Menu.Add(new MenuItem { Header = "Stop" });           
            //Menu.Add(new MenuItem { Header = "Clients" });
            //Menu.Add(new MenuItem { Header = "Exit" });
            //Menu.Add(new MenuItem { Header = "Update" });
            WindowWatcher.AddWindow(this);
            
        }

        public void UpdateView(Client c)
        {
            if (c.Status.Equals("Stopped"))
            {
                if (!Dispatcher.CheckAccess())
                {
                    Dispatcher.Invoke(() => StartMenuItem.IsEnabled = true);
                    Dispatcher.Invoke(() => StopMenuItem.IsEnabled = false);                   
                }              
            }
            else if(c.Status.Equals("Running") || c.Status.Equals("StartPending"))
            {
                if (!Dispatcher.CheckAccess())
                {
                    Dispatcher.Invoke(() => StartMenuItem.IsEnabled = false);
                    Dispatcher.Invoke(() => StopMenuItem.IsEnabled = true);
                }
            }
        }


        //private void CustomExpanderButtom_Click(object sender, RoutedEventArgs e)
        //{
        //    if (CustomExpanderMenu.Visibility == Visibility.Hidden)
        //        CustomExpanderMenu.Visibility = Visibility.Visible;
        //    else
        //        CustomExpanderMenu.Visibility = Visibility.Hidden;
        //}
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
           // var item = (IMenuItem)Menu.SelectedItem;
           // item.PerformAction();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            WindowWatcher.RemoveWindow(this);
        }

        private void StartMenuItem_Click(object sender, RoutedEventArgs e)
        {  
            MenuStaticClassFactory.GetStartClass().PerformAction();        
        }

        private void StopMenuItem_Click(object sender, RoutedEventArgs e)
        {                    
            MenuStaticClassFactory.GetStopClass().PerformAction();
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuStaticClassFactory.GetExitClass().PerformAction();
        }

        private void ClientsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuStaticClassFactory.GetClientsClass().PerformAction();
        }

        private void UpdateMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuStaticClassFactory.GetUpdateClass().PerformAction();
        }

        private void TaskBarContextMenu_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
        }

     
    }
}

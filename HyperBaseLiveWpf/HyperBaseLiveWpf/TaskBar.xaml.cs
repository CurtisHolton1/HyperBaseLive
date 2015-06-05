using System;
using System.Collections.Generic;
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
    public partial class TaskBar : Window
    {
        public string UserImg { get; set; }
        public List<string> SyncHistory { get; set; }
        public TaskBar()
        {
            this.DataContext = this;
            UserImg = "Content/profile.jpg";
            SyncHistory = new List<string>();
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
            
            
        }

        private void CustomExpanderButtom_Click(object sender, RoutedEventArgs e)
        {
            if (CustomExpanderMenu.Visibility == Visibility.Collapsed)
                CustomExpanderMenu.Visibility = Visibility.Visible;
            else
                CustomExpanderMenu.Visibility = Visibility.Collapsed;
        }
    }
}

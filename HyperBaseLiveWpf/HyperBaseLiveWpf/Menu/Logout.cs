using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HyperBaseLiveWpf.Menu
{
    public class Logout: IMenuItem
    {
        private string name = "Logout";
        public string Name { get { return name; } set { name = value; } }
        public void PerformAction()
        {
            Properties.Settings.Default.UserName = "";
            Properties.Settings.Default.Password = "";
            Properties.Settings.Default.Save();
            Application.Current.Shutdown();
        }
    }
}

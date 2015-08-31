using HyperBaseLiveWpf.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HyperBaseLiveWpf.Menu
{
    public class Clients : IMenuItem
    {
        private string name = "Clients";
        public string Name { get { return name; } set { name = value; } }
        public void PerformAction()
        {
            var w =  (ClientsView)WindowWatcher.GetWindowOfType<ClientsView>();          
            if (w == null)
                w = new ClientsView();
            if (!w.IsVisible)
                w.Show();
            if (w.WindowState == WindowState.Minimized)
                w.WindowState = WindowState.Normal;
            w.Activate();
            w.Topmost = true;  
            w.Topmost = false;
            w.Focus();
        }
    }
}

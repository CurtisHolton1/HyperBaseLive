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
            bool flag = false;
            foreach (Window w in App.Current.Windows)
            {
                if (w is ClientsView)
                {
                    flag = true;
                    w.Show();
                    w.Activate();
                    break;
                }
            }
            if (flag == false)
            {
                var wnd = new ClientsView();
                wnd.Show();
            }
        }


    }
}

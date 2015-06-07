using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HyperBaseLiveWpf.Menu
{
    public class Exit : IMenuItem
    {
        private string name = "Exit";
        public string Name { get { return name; } set { name = value; } }
        public void PerformAction(){
            Application.Current.Shutdown();
        }
    }
}

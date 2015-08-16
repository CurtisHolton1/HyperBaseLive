using HyperBaseLiveWpf.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperBaseLiveWpf.Menu
{
  public class Stop: IMenuItem
    {
        private string name = "Stop";
        public string Name { get { return name; } set { name = value; } }

        public async void PerformAction()
        {
            DbManager dbM = new DbManager();
            foreach (var c in await Task.Run(() => dbM.GetAllClients()))
            {
                c.Stop();
            }
        }
    }
}

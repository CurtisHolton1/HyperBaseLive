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

        public void PerformAction()
        {
            BatchManager bm = new BatchManager();
            bm.WriteStop("HyperBase Client");
            bm.LaunchStop();
        }
    }
}

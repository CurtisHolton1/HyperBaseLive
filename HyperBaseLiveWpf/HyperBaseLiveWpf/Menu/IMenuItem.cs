using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperBaseLiveWpf.Menu
{
    public interface IMenuItem
    {
        string Name { get; set; }
        void PerformAction();
    }
}

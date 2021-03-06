﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HyperBaseLiveWpf.Helpers;

namespace HyperBaseLiveWpf.Menu
{
   public class Start :IMenuItem
    {
       private string name = "Start";
       public string Name { get { return name; } set { name = value;  } }

        public async void PerformAction(){
            DbManager dbM = new DbManager();
           foreach(var c in await Task.Run(() => dbM.GetAllClients()))
            {
                c.Start();
            }
        
        }
    }
}

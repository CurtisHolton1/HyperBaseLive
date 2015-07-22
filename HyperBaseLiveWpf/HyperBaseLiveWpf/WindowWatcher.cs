using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HyperBaseLiveWpf
{
     public static class WindowWatcher
    {
         static List<Window> allWindows = new List<Window>();

   
         public static void Startup(){
            allWindows =  System.Windows.Application.Current.Windows.Cast<Window>().ToList<Window>();
         }

         public static void AddWindow(Window w){
             allWindows.Add(w);
         }
         public static void RemoveWindow(Window w){
             allWindows.Remove(w);
             if (allWindows.Count==2 )
             {
                 var wnd = allWindows.Single<Window>(m => m is ClientsView);
                 (wnd as ClientsView).AddClientButton.IsEnabled = true;
                (wnd as ClientsView).UpdateClientList();
             }
         }
         public static bool Contains(Window w)
         {
             return allWindows.Contains(w);             
         }
    }
}

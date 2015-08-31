using HyperBaseLiveWpf.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;

namespace HyperBaseLiveWpf
{
    public static class WindowWatcher
    {
         static List<Window> allWindows = new List<Window>();

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);
        /// <summary>Returns true if the current application has focus, false otherwise</summary>
        public static bool ApplicationIsActivated()
        {
            var activatedHandle = GetForegroundWindow();
            if (activatedHandle == IntPtr.Zero)
            {
                return false;       // No window is currently activated
            }

            var procId = Process.GetCurrentProcess().Id;
            int activeProcId;
            GetWindowThreadProcessId(activatedHandle, out activeProcId);
            return activeProcId == procId;
        }

        public static void Startup(){
            allWindows =  System.Windows.Application.Current.Windows.Cast<Window>().ToList<Window>();
         }

         public static void AddWindow(Window w){
             allWindows.Add(w);
         }
         public static void RemoveWindow(Window w){
             allWindows.Remove(w);
            if (allWindows.Count == 2)
            {
                ClientsView clientsViewWindow = (ClientsView)GetWindowOfType<ClientsView>();
                clientsViewWindow.AddClientButton.IsEnabled = true;
            }
        }
         public static bool Contains(Window w)
         {
             return allWindows.Contains(w);             
         }
        public static Window GetWindowOfType<T>() where T : Window
        {
            var result = allWindows.OfType<T>();
            return result.FirstOrDefault();
        }
    }
}

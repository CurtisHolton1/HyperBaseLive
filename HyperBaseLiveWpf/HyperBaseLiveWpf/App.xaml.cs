using Microsoft.Shell;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace HyperBaseLiveWpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, ISingleInstanceApp
    {
        private const string Unique = "HyperBase_ClientManager";
        [STAThread]
        public static void Main()
        {
            if (SingleInstance<App>.InitializeAsFirstInstance(Unique))
            {              
                var application = new App();

                application.InitializeComponent();
                application.Run();

                // Allow single instance code to perform cleanup operations
                SingleInstance<App>.Cleanup();
            }
        }

        
        #region ISingleInstanceApp Members

        public bool SignalExternalCommandLineArgs(IList<string> args)
        {
            for (int i = 0; i<this.Windows.Count; i++){
                if (this.Windows[i].WindowState == WindowState.Minimized)
                {
                    this.Windows[i].WindowState = WindowState.Normal;
                }               
                this.Windows[i].Activate();
            }
            return true;
        }

        #endregion
    }
}

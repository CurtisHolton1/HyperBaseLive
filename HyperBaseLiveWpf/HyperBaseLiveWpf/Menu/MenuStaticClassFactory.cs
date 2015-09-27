using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperBaseLiveWpf.Menu
{
    public static class MenuStaticClassFactory
    {
        public static IMenuItem GetStartClass()
        {
            return new Start();
        }
        public static IMenuItem GetStopClass()
        {
            return new Stop();
        }
        public static IMenuItem GetUpdateClass()
        {
            return new Update();
        }
        public static IMenuItem GetExitClass()
        {
            return new Exit();
        }
        public static IMenuItem GetClientsClass()
        {
            return new Clients();
        }
        public static IMenuItem GetLogoutClass()
        {
            return new Logout();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace HyperBaseLiveWpf.Helpers
{
   public class ClientManager
    {
        public static bool IsClientInstalled(string clientName)
        {
            // get list of Windows services
            ServiceController[] services = ServiceController.GetServices();
            // try to find service name
            foreach (ServiceController service in services)
            {
                if (service.ServiceName.Equals(clientName))
                    return true;
            }
            return false;
        }
        public static string GetServiceStatus(string clientName)
        {
            if (IsClientInstalled(clientName))
            {
                ServiceController[] services = ServiceController.GetServices();
                foreach (ServiceController service in services)
                {
                    if (service.ServiceName.Equals(clientName))
                        return service.Status.ToString();                      
                }
            }
            return "";
        }
    }
}

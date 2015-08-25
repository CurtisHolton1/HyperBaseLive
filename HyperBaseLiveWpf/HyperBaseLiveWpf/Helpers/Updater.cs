using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HyperBaseLiveWpf.Helpers
{
    public static class Updater
    {
        static Updater()
        {
            var hubConnection = new HubConnection("http://q.hyperbase-live.com/signalr");
            var HblHubProxy = hubConnection.CreateHubProxy("HyperbaseServer");
            HblHubProxy.On("DoUpdate", UpdateAllClients);
            ServicePointManager.DefaultConnectionLimit = 10;
            hubConnection.Start();
        }

        public static async void UpdateAllClients()
        {
            DbManager dbM = new DbManager();
            var clients = await Task.Run(() => dbM.GetAllClients());
            if (clients.Count > 0)
            {
                var response = await Task.Run(() => HblApiCaller.GetServiceVersion());
                if (response != null)
                {
                    var latestVersion = response.Version;
                    foreach (var c in clients)
                    {
                        c.Update(response);
                    }
                }
            }
        }
    }
}

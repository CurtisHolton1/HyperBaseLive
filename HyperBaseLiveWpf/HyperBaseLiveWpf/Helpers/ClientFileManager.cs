using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace HyperBaseLiveWpf.Helpers
{
    public static class ClientFileManager
    {
        private static List<Client> installedClients = BuildClientListFromFile();
        private static List<Client> BuildClientListFromFile()
        {
            List<Client> toReturn = new List<Client>();
            string filePath = "Clients.txt";
            if (!File.Exists(filePath))
            {
                var f = File.Create(filePath);
                f.Close();
                f.Dispose(); 
            }
            else
            {
                using (StreamReader sw = new StreamReader(filePath))
                {
                    string line;
                    while ((line = sw.ReadLine()) != null && !string.IsNullOrEmpty(line))
                    {
                        var split = line.Split('\t');
                        string status = "ERROR";
                        /////////////////////////////////////////////////////////////////////
                        //what needs to be used when client name is used
                        ////////////////////////////////////////////////////////////////////
                        //if (ClientManager.IsClientInstalled(split[0]))
                        //{
                        //    status = ClientManager.GetServiceStatus(split[0]);
                        //}
                        if (ClientManager.IsClientInstalled("HyperBase Client"))
                        {
                            status = ClientManager.GetServiceStatus("HyperBase Client");
                        }
                        toReturn.Add(new Client { Name = split[0], Location = split[1], Status = status, HBLStatus = "Pending Check", InstanceID = split[3], Version = Convert.ToInt32(split[4]) });
                    }
                }
            }
            return toReturn;
        }

        public static async Task<List<Client>> DetermineClients()
        {
            List<Client> toReturn = new List<Client>();
            foreach (var c in installedClients)
            {
                string status = "ERROR";
                /////////////////////////////////////////////////////////////////////
                //what needs to be used when client name is used
                ////////////////////////////////////////////////////////////////////
                //if (ClientManager.IsClientInstalled(c.Name))
                //{
                //    status = ClientManager.GetServiceStatus(c.Name);
                //}
                if (ClientManager.IsClientInstalled("HyperBase Client"))
                {
                    status = ClientManager.GetServiceStatus("HyperBase Client");
                }
                toReturn.Add(new Client { Name = c.Name, Location = c.Location, Status = status, HBLStatus = await Task.Run(() => HblApiCaller.CheckStatus()), Version = c.Version, InstanceID = c.InstanceID, Id = c.Id });
            }
            var taskWnd = (TaskBarView)WindowWatcher.GetWindowOfType<TaskBarView>();
            ///////////////////////////
            if (taskWnd != null && toReturn.Count > 0)
            {
                taskWnd.UpdateView(toReturn[0]);
            }
            /////////////////////////////
            return toReturn;
        }
        public static async void AddClientToFile(Client c)
        {
         
            string filePath = "Clients.txt";
            if (File.Exists(filePath))
            {
                var text = File.ReadAllText(filePath) + c.Name + "\t" + c.Location + "\t" + c.InstanceID + "\t" + c.Version + "\t" + c.Id + Environment.NewLine;
                File.WriteAllText(filePath, text);
                installedClients = BuildClientListFromFile();
            }
        }
    }
}

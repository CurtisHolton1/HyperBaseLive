using System;
using System.Collections.Generic;
using System.IO;

namespace HyperBaseLiveWpf.Helpers
{
    public static class ClientFileManager
    {
        public static List<Client> DetermineClients()
        {
            List<Client> clientList = new List<Client>();
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
                        clientList.Add(new Client { Name = split[0], Location = split[1] ,Status = status});
                    }
                }
            }
            return clientList;
        }


        public static async void AddClientToFile(Client c)
        {
            string filePath = "Clients.txt";
            var text = File.ReadAllText(filePath) + c.Name + "\t" + c.Location + Environment.NewLine;
            File.WriteAllText(filePath, text);
        }
    }
}

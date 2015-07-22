using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                        clientList.Add(new Client { Name = split[0], Location = split[1] });
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

        //private async Task WriteTextAsync(string filePath, string text)
        //{

        //    byte[] encodedText = Encoding.Default.GetBytes(text);
        //    using (FileStream sourceStream = new FileStream(filePath,
        //        FileMode.Append, FileAccess.Write, FileShare.None,
        //        bufferSize: 4096, useAsync: true))
        //    {
        //        await sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
        //    };
        //}

        //public string ParseClientFile()
        //{

        //    string line;
        //    string filePath = "Clients.txt";
        //    if (!File.Exists(filePath))
        //    {
        //        var f = File.Create(filePath);
        //        f.Close();
        //        f.Dispose();
        //        return "";
        //    }
        //    else
        //    {
        //        TextReader readerAll = File.OpenText(filePath);
        //        string allText = readerAll.ReadToEnd();
        //        TextReader reader = File.OpenText(filePath);
        //        while ((line = reader.ReadLine()) != null)
        //        {
        //            string[] a = line.Split('\t');
        //            tmpList.Add(new Client { Name = a[0], Location = a[1] });
        //        }
        //        readerAll.Close();
        //        reader.Close();


        //        return allText;
        //    }
        //}

        //private string ValidateList(string allText)
        //{
        //    List<Client> tmpList = DataList;
        //    for (int i = 0; i < tmpList.Count; i++)
        //    {
        //        if (!IsServiceInstalled(tmpList[i].Name))
        //        {
        //            string lineToRemove = tmpList[i].Name + "\t" + tmpList[i].Location + "\n";
        //            allText = allText.Replace(lineToRemove, "");
        //            tmpList.Remove(tmpList[i]);
        //        }
        //    }
        //    DataList = tmpList;

        //    return allText;
        //}

        //private void UpdateClientFile(string allText)
        //{
        //    // await WriteTextAsync("Clients.txt", allText);
        //    try
        //    {
        //        StreamWriter sw = new StreamWriter("Clients.txt");
        //        sw.Write(allText);
        //        sw.Close();
        //        sw.Dispose();
        //    }
        //    catch { }
        //}

        //private async Task WriteTextAsync(string filePath, string text)
        //{

        //    //byte[] encodedText = Encoding.Default.GetBytes(text);
        //    //using (FileStream sourceStream = new FileStream(filePath,
        //    //    FileMode.Create, FileAccess.Write, FileShare.None,
        //    //    bufferSize: 4096, useAsync: true))
        //    //{
        //    //    await sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
        //    //};

        //}


        //public static bool IsServiceInstalled(string serviceName)
        //{
        //    // get list of Windows services
        //    ServiceController[] services = ServiceController.GetServices();

        //    // try to find service name
        //    foreach (ServiceController service in services)
        //    {
        //        if (service.ServiceName.Equals(serviceName))
        //            return true;
        //    }
        //    return false;
        //}



    }
}

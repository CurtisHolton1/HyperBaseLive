using HyperBaseLiveWpf.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HyperBaseLiveWpf
{
    /// <summary>
    /// Interaction logic for Clients.xaml
    /// </summary>
    public partial class ClientsView : Window, INotifyPropertyChanged
    {
        private List<Client> dataList = new List<Client>();
        public List<Client> DataList { get { return dataList; } set { dataList = value; this.OnPropertyChanged("DataList"); } }


        public ClientsView()
        {
            InitializeComponent();
            this.DataContext = this;
            WindowWatcher.AddWindow(this);
            DetermineClients();
        }

        private string ParseClientFile()
        {
            List<Client> tmpList = new List<Client>();
            string line;
            string filePath = "../../Clients.txt";
            TextReader readerAll = File.OpenText(filePath);
            string allText = readerAll.ReadToEnd();
            TextReader reader = File.OpenText(filePath);
            while ((line = reader.ReadLine()) !=null)
            {
                string [] a = line.Split('\t');
                tmpList.Add(new Client { Name = a[0], Location = a[1] });
            }
            readerAll.Close();
            reader.Close();
            DataList = tmpList;
            return allText;
        }

        private string ValidateList(string allText){
            List<Client> tmpList = DataList;
            for (int i = 0; i < tmpList.Count; i++)
            {
                if (!IsServiceInstalled(tmpList[i].Name))
                {
                    string lineToRemove = tmpList[i].Name + "\t" + tmpList[i].Location + "\n";
                  allText = allText.Replace(lineToRemove, "");
                  tmpList.Remove(tmpList[i]);              
                }
            }
            DataList = tmpList;

            return allText;
        }

        private async void UpdateClientFile(string allText)
        {
            await WriteTextAsync("../../Clients.txt", allText);
        }

        private async Task WriteTextAsync(string filePath, string text)
        {

            byte[] encodedText = Encoding.Default.GetBytes(text);
            using (FileStream sourceStream = new FileStream(filePath,
                FileMode.Create, FileAccess.Write, FileShare.None,
                bufferSize: 4096, useAsync: true))
            {
                await sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
            };

        }
       

        public static bool IsServiceInstalled(string serviceName)
        {
            // get list of Windows services
            ServiceController[] services = ServiceController.GetServices();

            // try to find service name
            foreach (ServiceController service in services)
            {
                if (service.ServiceName.Equals(serviceName))
                    return true;
            }
            return false;
        }


        private void AddClientButton_Click(object sender, RoutedEventArgs e)
        {
            //bool flag = false;
            //foreach (Window w in App.Current.Windows)
            //{
            //    if (w is ValidateIDView)
            //    {
            //        flag = true;
            //        w.Show();
            //        w.Activate();
            //        break;
            //    }
            //}
            //if (flag == false)
            //{
            //    var wnd = new ValidateIDView();
            //    wnd.Show();
            //}
            var w = new ValidateIDView();
            w.Show();
            w.Activate();
            AddClientButton.IsEnabled = false;
        }

        public void DetermineClients()
        {
            RefreshButton.IsEnabled = false;
            var allText = ParseClientFile();
           // allText = ValidateList(allText);
            UpdateClientFile(allText);
            RefreshButton.IsEnabled = true;
        }



        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {          
            DetermineClients();          
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            WindowWatcher.RemoveWindow(this);
        }

      



    }
}

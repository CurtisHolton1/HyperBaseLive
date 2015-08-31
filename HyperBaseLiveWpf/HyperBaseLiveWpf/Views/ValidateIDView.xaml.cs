using HyperBaseLiveWpf.Helpers;
using HyperBaseLiveWpf.Models;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HyperBaseLiveWpf.Views
{
    /// <summary>
    /// Interaction logic for ValidateIDView.xaml
    /// </summary>
    public partial class ValidateIDView : Window, INotifyPropertyChanged
    {
        private string clientIDText;
        public string ClientIDText { get { return clientIDText; } set { clientIDText = value; this.OnPropertyChanged("ClientIDText"); } }
        private string errorMessage;
        public string ErrorMessage { get { return errorMessage; } set { errorMessage = value; this.OnPropertyChanged("ErrorMessage"); } }
        public ValidateIDView()
        {
            this.DataContext = this;
            InitializeComponent();
            WindowWatcher.AddWindow(this);
        }

        private void Generate_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start("http://beta.hyperbase-live.com/ClientInstances");
        }


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        private async void ValidateButton_Click(object sender, RoutedEventArgs e)
        {
            ErrorMessage = "";
            ValidateButton.IsEnabled = false;
            if (string.IsNullOrEmpty(ClientIDText))
            {
                ErrorMessage = "The ID field cannot be empty";
                ValidateButton.IsEnabled = true;
            }
            else
            {
                MainForm.Visibility = Visibility.Hidden;
                LoadingImg.Visibility = Visibility.Visible;
                Client clientToInstall = new Client();
                var status = await Task.Run(() => HblApiCaller.ValidateID(ClientIDText));
                MainForm.Visibility = Visibility.Visible;
                LoadingImg.Visibility = Visibility.Hidden;
                if(!string.IsNullOrEmpty(status))
                {
                    switch (status)
                    {
                        case "The operation has timed out":
                            {
                                ErrorMessage = status;
                                break;
                            }
                        case "NotFound":
                            {
                                ErrorMessage = "Validation Failed";
                                break;
                            }
                        default:
                            {
                                clientToInstall.Name = status.Substring(1, status.Length - 2);
                                clientToInstall.InstanceID = clientIDText;
                                var wnd = new AddClientView(clientToInstall);
                                wnd.Show();
                                this.Close();
                                break;
                            }
                    }
                    ValidateButton.IsEnabled = true;
                }
                else
                {
                    ValidateButton.IsEnabled = true;
                    ErrorMessage = "An unexpected problem has occurred";
                }
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            WindowWatcher.RemoveWindow(this);
        }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

namespace HyperBaseLiveWpf.Views
{
    /// <summary>
    /// Interaction logic for ValidateIDView.xaml
    /// </summary>
    public partial class ValidateIDView : Window, INotifyPropertyChanged
    {
        private string clientIDText;
        public string ClientIDText { get { return clientIDText; } set { clientIDText = value; this.OnPropertyChanged("ClientIDText"); } }
        public ValidateIDView()
        {
            this.DataContext = this;
            InitializeComponent();
            ClientIDText = "Client ID";
        }

        private void ClientID_GotFocus(object sender, RoutedEventArgs e)
        {
            if (ClientIDText.Equals("Client ID"))
                ClientIDText = "";
        }

        private void ClientID_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ClientIDText.Equals(""))
                ClientIDText = "Client ID";
        }

        private void Generate_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ClientIDText = "GENERATED ID HERE";
        }


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        private void ValidateButtom_Click(object sender, RoutedEventArgs e)
        {
            var wnd = new AddClientView();
            wnd.Show();
            this.Hide();

           
        }
    }
}

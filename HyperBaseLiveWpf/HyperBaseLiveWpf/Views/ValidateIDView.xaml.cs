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
        private string errorMessage;
        public string ErrorMessage { get { return errorMessage; } set { errorMessage = value; this.OnPropertyChanged("ErrorMessage"); } }
        public ValidateIDView()
        {
            this.DataContext = this;
            InitializeComponent();
            
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

    
        private bool ValidationCheck()
        {
            if (string.IsNullOrEmpty(ClientIDText))
            {
                ErrorMessage = "The ID field cannot be empty";
                return false;
            }
            //api call for further processing
            return true;
        }

        private void ValidateButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidationCheck())
            {
                var wnd = new AddClientView();
                wnd.Show();
                this.Hide();
            }
        }
    }
}

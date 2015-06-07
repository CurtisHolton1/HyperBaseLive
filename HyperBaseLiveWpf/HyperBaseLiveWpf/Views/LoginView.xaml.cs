using HyperBaseLiveWpf.Views;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HyperBaseLiveWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginView : Window, INotifyPropertyChanged
    {
        private string userName;
        public string UserName { get {return userName; } set {userName = value; this.OnPropertyChanged("UserName");} }

        private string password;
        public string PassWord { get { return this.PasswordBox1.Password; } set { password = value; this.OnPropertyChanged("PassWord"); } }
        public LoginView()
        {
            UserName = "Username";           
            InitializeComponent();
            this.DataContext = this;

            //APICaller apiCaller = new APICaller();
            //apiCaller.APIStart();
          
        }
        private void UserNameBox_GotFocus(object sender, RoutedEventArgs e)       
        {
            if (UserName.Equals("Username"))
           UserName ="";
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (UserName == "admin" && PassWord == "password")
            {
                
                LoginComplete();
               
            }
            else
            {
                ErrorMessage.Visibility = Visibility.Visible;
            }
        }

        private void PasswordBox1_GotFocus(object sender, RoutedEventArgs e)
        {
            PasswordCover.Visibility = Visibility.Hidden;
            PasswordBox1.Focus();
        }



        private void PasswordCover_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            PasswordBox1.Focus();
        }

        private void PasswordBox1_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
                SubmitButton_Click(sender, e);
        }

        private void LoginComplete()
        {
            Window win = new ClientsView();
            win.Show();
            win = new TaskBarView();
            win.Show();
            this.Close();
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        private void UserNameBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (UserName.Equals(""))
                UserName = "Username";
        }

        private void PasswordBox1_LostFocus(object sender, RoutedEventArgs e)
        {
            if (PassWord.Equals(""))
                PasswordCover.Visibility = Visibility.Visible;
        }
    }
}

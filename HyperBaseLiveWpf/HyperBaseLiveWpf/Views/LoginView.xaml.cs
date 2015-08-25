using HyperBaseLiveWpf.Helpers;
using HyperBaseLiveWpf.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
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
        public string Password { get { return this.PasswordBox1.Password; } set { password = value; this.OnPropertyChanged("PassWord"); } }
        public LoginView()
        {
            UserName = "Username";           
            InitializeComponent();
            this.DataContext = this;
            WindowWatcher.AddWindow(this);
            DbManager dbM = new DbManager();
            dbM.CreateDB();
        }
        private void UserNameBox_GotFocus(object sender, RoutedEventArgs e)       
        {
            ErrorMessage.Visibility = Visibility.Hidden;
            if (UserName.Equals("Username"))
           UserName ="";
        }

        private async void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            ErrorMessage.Visibility = Visibility.Hidden;
            SubmitButton.IsEnabled = false;
            if (CheckBoxes() && await Task.Run(() => HblApiCaller.Authenticate(UserName, Password)))
                LoginComplete();
            else
            {
                ErrorMessage.Visibility = Visibility.Visible;
                SubmitButton.IsEnabled = true;
            }
            
        }

        private void PasswordBox1_GotFocus(object sender, RoutedEventArgs e)
        {
            ErrorMessage.Visibility = Visibility.Hidden;
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

        private async void LoginComplete()
        {
            DbManager dbM = new DbManager();
            foreach (var c in await Task.Run(() => dbM.GetAllClients())){
                c.Start();
            }
            //Updater.UpdateAllClients();
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
            if (Password.Equals(""))
                PasswordCover.Visibility = Visibility.Visible;
        }

        private bool CheckBoxes()
        {
            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password) || UserName.Equals("Username"))
                return false;
            return true;
        }

        private void HyperSpinLink_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start("http://www.hyperspin-fe.com/");
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            WindowWatcher.RemoveWindow(this);
        }


       
    }
}

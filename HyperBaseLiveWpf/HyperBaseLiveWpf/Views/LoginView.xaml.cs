using HyperBaseLiveWpf.Helpers;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HyperBaseLiveWpf.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginView : Window, INotifyPropertyChanged
    {
        private string userName;
        public string UserName { get {return userName; } set {userName = value; this.OnPropertyChanged("UserName");} }

        private string password;
        public string Password { get { return PasswordBox1.Password; } set { password = value; this.OnPropertyChanged("Password"); } }
        public LoginView()
        {
            //Properties.Settings.Default.UserName = "yhhh";
            //Properties.Settings.Default.Password = "fdsafdsa";
            //Properties.Settings.Default.Save();

            if (!string.IsNullOrEmpty(Properties.Settings.Default.UserName))
            {
                this.Hide();
                if (HblApiCaller.Authenticate(Properties.Settings.Default.UserName, Properties.Settings.Default.Password))
                {
                    LoginComplete();
                }
                else {
                    this.Show();
                }
            }
          
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
            UserNamePassForm.Visibility = Visibility.Hidden;
            LoadingImg.Visibility = Visibility.Visible;
            if (CheckFields() && await Task.Run(() => HblApiCaller.AuthenticateAsync(UserName, Password)))
                LoginComplete();
            else
            {
                LoadingImg.Visibility = Visibility.Hidden;
                UserNamePassForm.Visibility = Visibility.Visible;
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
            Properties.Settings.Default.UserName = UserName;
            Properties.Settings.Default.Password = Password;
            if ((bool)RememberCheckbox.IsChecked)
            {
                Properties.Settings.Default.Save();
            }
            Updater.UpdateAllClients();
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

        private bool CheckFields()
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

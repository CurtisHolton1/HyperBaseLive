using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows.Forms;
namespace HyperBaseLiveWpf.Views

{
    /// <summary>
    /// Interaction logic for InstallServiceView.xaml
    /// </summary>
    public partial class InstallServiceView : Window, INotifyPropertyChanged
    {
        CancellationTokenSource cts;
        private string buttonContent;
        public string ButtonContent { get { return buttonContent; } set { buttonContent = value; this.OnPropertyChanged("ButtonContent"); } }
        private string statusLabelContent;
        public string StatusLabelContent { get { return statusLabelContent; } set { statusLabelContent = value; this.OnPropertyChanged("StatusLabelContent"); } }
        public InstallServiceView()
        {
            this.DataContext = this;
            InitializeComponent();
            InstallBar.Maximum = 100;
            InstallBar.Value = 0;
            ButtonContent = "Cancel";
            StatusLabelContent = "Installing...";
            Wrapper();
        }



        async Task<int> Worker(IProgress<int> progress, CancellationToken ct)
        {
            int tempCount = 0;
            int processCount = await Task.Run<int>(() =>
            {
                for (int i = 0; i <= 100; i++)
                {
                    Thread.Sleep(100);
                    progress.Report(i);
                    ct.ThrowIfCancellationRequested();
                    tempCount++;
                }

                return tempCount;
            }, ct);
            return processCount;
        }

        private async void  Wrapper()
        {
            var progressIndicator = new Progress<int>(ReportProgress);
            cts = new CancellationTokenSource();
            try
            {
                var thing = await Worker(progressIndicator, cts.Token);
                InstallComplete();
            }
            catch (OperationCanceledException ex)
            {
                OnCancel();
            }
        }

        private  void InstallBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        void ReportProgress(int value)
        {
            InstallBar.Value = value;
        }

        private void BottomButton_Click(object sender, RoutedEventArgs e)
        {
            if (ButtonContent.Equals("Cancel"))
                cts.Cancel();
            else
                this.Close();
        }

        private void OnCancel()
        {
            StatusLabelContent = "Installation Cancelled";
            BottomButton.IsEnabled = false;
        }

        private void InstallComplete()
        {
            InstallBar.Visibility = Visibility.Hidden;
            InstallCompleteLabel.Visibility = Visibility.Visible;
            StatusLabel.Visibility = Visibility.Hidden;
            Launch.Visibility = Visibility.Visible;
            ButtonContent = "Finish";
        }


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        private void Launch_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //Launch Application
        }
    }
}

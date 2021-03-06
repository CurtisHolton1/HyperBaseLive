﻿using HyperBaseLiveWpf.Models;
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
    /// Interaction logic for EditClientView.xaml
    /// </summary>
    public partial class EditClientView : Window
    {
        private string clientName;
        public string ClientName { get { return clientName;  } set { clientName = value; this.OnPropertyChanged("ClientName"); } }  
        public EditClientView(Client c)
        {
            this.ClientName = c.Name;
            InitializeComponent();
            this.DataContext = this;
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        private void HyperSpinButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

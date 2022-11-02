using Avalonia.Controls;
using Avalonia.Interactivity;
using CreativeWritersToolkitApp.Models;
using CreativeWritersToolkitApp.Events;
using System.ComponentModel;
using License = CreativeWritersToolkitApp.Models.License;
using System;

namespace CreativeWritersToolkitApp.Views
{
    public partial class LicenseWindow : Window
    {
        public event EventHandler<LicenseWindowCancelArgs> Canceled;
        public event EventHandler Register;
        public License? License { get; set; }

        public LicenseWindow()
        {
            InitializeComponent();
        }

        private void RegisterBtn_Click(object sender, RoutedEventArgs args)
        {
            License.Email = EmailTxBx.Text;
            License.Key = KeyTxBx.Text;
            Register?.Invoke(this, args);
        }
        private void CancelBtn_Click(object sender, RoutedEventArgs args)
        {
            var e = new LicenseWindowCancelArgs();
            e.Cancel = true;
            Canceled?.Invoke(this, e);
            
        }
    }
}

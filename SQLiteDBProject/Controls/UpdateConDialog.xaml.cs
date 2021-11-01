using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SQLiteDBProject.Controls
{
    public sealed partial class UpdateConDialog : ContentDialog
    {

        public string Password { get; set; }

        public string Name { get; set; }

        public UpdateConDialog()
        {
            this.InitializeComponent();
        }

        private void TxtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            EnableDisablePrimaryButton();
        }

        private void TxtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableDisablePrimaryButton();
        }

        private void UpdateConDialog_Loaded(object sender, RoutedEventArgs e)
        {
            EnableDisablePrimaryButton();
        }

        private void EnableDisablePrimaryButton()
        {
            if (!string.IsNullOrEmpty(txtPassword.Password) && !string.IsNullOrEmpty(txtName.Text))
            {
                this.IsPrimaryButtonEnabled = true;
            }
            else
            {
                this.IsPrimaryButtonEnabled = false;
            }
        }
    }
}

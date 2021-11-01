using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WinUX.Data.Validation;
using WinUX.Data.Validation.Rules;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SQLiteDBProject
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        #region Properties

        public ObservableCollection<Model.Employee> Employees { set; get; } = null;

        public ValidationRules Validation { get; set; } = null;

        #endregion


        #region Main operation: Add, Update, Delete

        //Initialize the page and Employee property
        public MainPage()
        {
            this.InitializeComponent();

            //Initialize the Employee property
            Employees = Helper.DataAccess.GetEmployees();

            //Initialize the validation and add rule to it
            Validation = new ValidationRules();

            Validation.Rules.Add(new EmailValidationRule());
        }


        //Add new Employee to list and database
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddEmployee();
        }
        private void TxtEmail_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                AddEmployee();
            }
        }
        private void TxtPassword_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                AddEmployee();
            }
        }
        private void TxtName_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                AddEmployee();
            }
        }
        private async Task<bool> DataIsValidAsync()
        {
            if (string.IsNullOrEmpty(txtEmail.Text)
                || string.IsNullOrEmpty(txtPassword.Password)
                || string.IsNullOrEmpty(txtName.Text))
            {
                await new Windows.UI.Xaml.Controls.ContentDialog()
                {
                    Title = "Attention!!",
                    Content = "you must fill textboxes firstly",
                    CloseButtonText = "OK",
                    DefaultButton = ContentDialogButton.Close
                }.ShowAsync();

                return false;
            }
            else if (!this.Validation.Rules[0].IsValid(txtEmail.Text))
            {
                await new Windows.UI.Xaml.Controls.ContentDialog()
                {
                    Title = "Attention!!",
                    Content = "Please enter a correct email",
                    CloseButtonText = "OK",
                    DefaultButton = ContentDialogButton.Close
                }.ShowAsync();

                return false;
            }
            else if (Helper.DataAccess.EmailIsFound(txtEmail.Text))
            {
                await new Windows.UI.Xaml.Controls.ContentDialog()
                {
                    Title = "Attention!!",
                    Content = "There is another one has the same email please enter another email",
                    CloseButtonText = "OK",
                    DefaultButton = ContentDialogButton.Close
                }.ShowAsync();

                return false;
            }

            return true;
        }
        private async void AddEmployee()
        {
            if (await DataIsValidAsync())
            {

                if (Employees.Count != 0)
                {
                    Model.Employee employee = new Model.Employee()
                    {
                        Id = Employees[Employees.Count - 1].Id + 1,
                        Email = txtEmail.Text,
                        Password = txtPassword.Password,
                        Name = txtName.Text
                    };

                    Employees.Add(employee);

                    Helper.DataAccess.AddEmployee(employee);

                    txtEmail.Text = string.Empty;
                    txtPassword.Password = string.Empty;
                    txtName.Text = string.Empty;
                }
                else
                {
                    Model.Employee employee = new Model.Employee()
                    {
                        Id = 1,
                        Email = txtEmail.Text,
                        Password = txtPassword.Password,
                        Name = txtName.Text
                    };

                    Employees.Add(employee);

                    Helper.DataAccess.AddEmployee(employee);

                    txtEmail.Text = string.Empty;
                    txtPassword.Password = string.Empty;
                    txtName.Text = string.Empty;
                }
            }
        }


        //Update Employee on list and database
        private async void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (lstEmployees.SelectedItem != null)
            {
                Controls.UpdateConDialog updateConDialog
                    = new Controls.UpdateConDialog();

                updateConDialog.Password = ((Model.Employee)lstEmployees.SelectedItem).Password;
                updateConDialog.Name = ((Model.Employee)lstEmployees.SelectedItem).Name;

                ContentDialogResult result = await updateConDialog.ShowAsync();

                if (result == ContentDialogResult.Primary)
                {
                    int IndexOfDeleteInsert = lstEmployees.SelectedIndex;

                    Model.Employee employee = new Model.Employee()
                    {
                        Id = ((Model.Employee)lstEmployees.SelectedItem).Id,
                        Email = ((Model.Employee)lstEmployees.SelectedItem).Email,
                        Password = updateConDialog.Password,
                        Name = updateConDialog.Name
                    };


                    Employees.RemoveAt(IndexOfDeleteInsert);

                    Employees.Insert(IndexOfDeleteInsert, employee);

                    Helper.DataAccess.UpdateEmployee(employee.Id, employee);
                }
            }
            else
            {
                ContentDialogResult result = await new Windows.UI.Xaml.Controls.ContentDialog()
                {
                    Title = "Attention!!",
                    Content = "you must select on an item in listview",
                    CloseButtonText = "OK",
                    DefaultButton = ContentDialogButton.Close
                }.ShowAsync();
            }
        }


        //Delete Employee from list and database
        private async void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lstEmployees.SelectedItem != null)
            {
                ContentDialogResult result = await new Windows.UI.Xaml.Controls.ContentDialog()
                {
                    Title = "Attention",
                    Content = "Do you want to delete this item?",
                    PrimaryButtonText = "YES",
                    CloseButtonText = "NO",
                    DefaultButton = ContentDialogButton.Close
                }.ShowAsync();

                if (result == ContentDialogResult.Primary)
                {
                    int id = ((Model.Employee)lstEmployees.SelectedItem).Id;

                    Employees.RemoveAt(lstEmployees.SelectedIndex);

                    Helper.DataAccess.DeleteEmployee(id);
                }

            }
            else
            {
                await new Windows.UI.Xaml.Controls.ContentDialog()
                {
                    Title = "Attention!!",
                    Content = "you must select on an item from listview",
                    CloseButtonText = "Ok",
                    DefaultButton = ContentDialogButton.Close
                }.ShowAsync();
            }


        }

        #endregion 
    }
}

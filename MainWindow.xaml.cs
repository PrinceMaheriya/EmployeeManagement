using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
using WpfApp1.Helper;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
    

        public MainWindow()
        {
            InitializeComponent();
            EnableDisableButton();
            BindEmployeeGrid();
            
        }

        #region functions

        /// <summary>
        /// This function is used to bind all the employee detail with grid
        /// </summary>
        private void BindEmployeeGrid()
        {
            try
            {
                ClearFields();
                grdEmployee.ItemsSource = EmployeeService.GetEmployeeData();
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Unable to fetch the employee details atm please try after sometime. message: {ex.Message}");
            }
        }


        /// <summary>
        /// this function is used to validate the email 
        /// </summary>
        /// <returns></returns>
        private bool ValidateEmail()
        {
            bool isValidEmail = true;

            if(string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Email is required.");
                isValidEmail = false;
            }
            else
            {
                isValidEmail = Common.IsValidEmail(txtEmail.Text);

                if(!isValidEmail)
                {
                    MessageBox.Show("Invalid Email");
                }
            }

            return isValidEmail;
        }


        /// <summary>
        /// this function is used to clear all the fields like (texbox, radiobutton, checkbox etc)
        /// </summary>
        private void ClearFields()
        {
            txtId.Text = string.Empty;
            txtName.Text = string.Empty;
            txtEmail.Text = string.Empty;
            rdbtFemale.IsChecked = false;
            rdbtMale.IsChecked = false;
            cbStatus.IsChecked = false;

            EnableDisableButton();
        }

        /// <summary>
        /// This function is used to enable/disable the buttons 
        /// </summary>
        private void EnableDisableButton()
        {
            btnDelete.IsEnabled = false;
            btnUpdate.IsEnabled = false;
            btnInsert.IsEnabled = true;
        }

        /// <summary>
        /// This function is used to load all the fields data 
        /// </summary>
        /// <param name="employeeDetail"></param>
        private void loadFieldsData(Datum employeeDetail)
        {
            txtEmail.Text = employeeDetail.Email;
            txtName.Text = employeeDetail.Name;
            rdbtMale.IsChecked = employeeDetail.Gender == Common.male;
            rdbtFemale.IsChecked = employeeDetail.Gender == Common.female;
            cbStatus.IsChecked = employeeDetail.Status == Common.active;

            btnDelete.IsEnabled = true;
            btnUpdate.IsEnabled = true;
            btnInsert.IsEnabled = false;
        }

        #endregion

        #region Event

        private void ClearAll_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();   
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                EmployeeService.DeleteEmployeeData(Convert.ToInt32(txtId.Text));
                BindEmployeeGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to delete the record atm please try again later." 
                    +Environment.NewLine +"Error Detail: "+ex.Message);
            }
        }

        private void BtnInsert_Click(object sender, RoutedEventArgs e)
        {
            try
            {

              var isValidEmail =  ValidateEmail();

                if(isValidEmail)
                {

                    Datum empDetail = new Datum();

                    empDetail.Name = txtName.Text;
                    empDetail.Email = txtEmail.Text;
                    empDetail.Gender = rdbtMale.IsChecked == true ? Common.male : Common.female;
                    empDetail.Status = cbStatus.IsChecked == true ? Common.active : Common.inactive;

                    var response = EmployeeService.AddEmployee(empDetail);

                    if (response.IsSuccessStatusCode)
                    {
                        BindEmployeeGrid();

                        response.Dispose();
                    }
                }
                 
            }
            catch(Exception ex)
            {
                MessageBox.Show("Unable to add employee atm please try again later." + Environment.NewLine + $"Error Detail: {ex.Message}");
            }
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(!string.IsNullOrWhiteSpace(txtId.Text))
                {

                    var employeeDetail = EmployeeService.GetEmployeeDetails(Convert.ToInt32(txtId.Text));

                    if (!string.IsNullOrWhiteSpace(employeeDetail.Email))
                    {
                        loadFieldsData(employeeDetail);
                    }
                    
                }
                else
                {
                    MessageBox.Show("Id is required.");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Unable to fetch employee detail atm please try again after sometime." + Environment.NewLine + "Error Details: " + ex.Message
                    );
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                var isValidEmail = ValidateEmail();

                if (!isValidEmail) return;
                
                Datum empDetail = new Datum();

                empDetail.Id = Convert.ToInt64(txtId.Text);
                empDetail.Name = txtName.Text;
                empDetail.Email = txtEmail.Text;
                empDetail.Gender = rdbtMale.IsChecked == true ? Common.male : Common.female;
                empDetail.Status = cbStatus.IsChecked == true ? Common.active : Common.inactive;

                EmployeeService.UpdateEmployeeDetailsAsync(empDetail);

                BindEmployeeGrid();

            }
            catch(Exception ex)
            {
                MessageBox.Show("Unable to update employee details due to the below error," +
                    " please try again after some time or connect to the IT support team." + Environment.NewLine + ex.Message);
            }
        }

        private void txtId_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
           e.Handled =  Common.IsTextAllowed(e.Text);
        }

        #endregion
    }
}

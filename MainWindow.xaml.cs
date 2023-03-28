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
        private readonly string baseUrl = ConfigurationManager.AppSettings.Get("ApiBaseUrl");
        private readonly string token = ConfigurationManager.AppSettings.Get("Token");
    

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

                HttpClient client = new HttpClient();

                client.BaseAddress = new Uri(baseUrl);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage responseMessage = client.GetAsync("users").Result;

                if(responseMessage.IsSuccessStatusCode)
                {
                    var emplyeeDetails = responseMessage.Content.ReadAsAsync<EmployeeModel>().Result;
                    grdEmployee.ItemsSource = emplyeeDetails.Data;

                }
                else
                {
                    MessageBox.Show($"Error Code: {responseMessage.StatusCode} message: {responseMessage.ReasonPhrase}");
                }

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
                isValidEmail = EmailService.IsValidEmail(txtEmail.Text);

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
                HttpClient client = new HttpClient();

                client.BaseAddress = new Uri(baseUrl);

                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.DeleteAsync($"users/{txtId.Text}").Result;

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync();

                    BindEmployeeGrid();

                    MessageBox.Show("Record deleted successfully.");
                }

            }
            catch(Exception ex)
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
                    string json = JsonConvert.SerializeObject(empDetail);

                    HttpClient client = new HttpClient();

                    client.BaseAddress = new Uri(baseUrl);

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = client.PostAsJsonAsync("users", empDetail).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string result = response.Content.ReadAsStringAsync().Result;

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
                    HttpClient client = new HttpClient();

                    client.BaseAddress = new Uri(baseUrl);

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    var response = client.GetAsync($"users/{txtId.Text}").Result;

                    if(response.IsSuccessStatusCode)
                    {
                      var result =  response.Content.ReadAsAsync<SingleEmplyee>().Result;

                        var employeeDetail = result.Data;

                        if(!string.IsNullOrWhiteSpace(employeeDetail.Email))
                        {
                            loadFieldsData(result.Data);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Id is required.");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Unable to fetch employee detail atm please try again afater sometime." + Environment.NewLine + "Error Details: " + ex.Message
                    );
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                var isValidEmail = ValidateEmail();

                if (!isValidEmail) return;

                using (HttpClient client = new HttpClient())
                {
                    Datum empDetail = new Datum();

                    empDetail.Id = Convert.ToInt64(txtId.Text);
                    empDetail.Name = txtName.Text;
                    empDetail.Email = txtEmail.Text;
                    empDetail.Gender = rdbtMale.IsChecked == true ? Common.male : Common.female;
                    empDetail.Status = cbStatus.IsChecked == true ? Common.active : Common.inactive;

                    client.BaseAddress = new Uri(baseUrl);
                    client.Timeout = TimeSpan.FromSeconds(900);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = client.PutAsJsonAsync("users", empDetail).Result;

                    if(response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Employee Details updated successfully.");
                    }
                    else
                    {
                        MessageBox.Show("Unable to update the employee details atm please connect to the IT support team.");
                    }
                }
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

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows;

namespace WpfApp1.Helper
{
    public static class EmployeeService
    {
        private static readonly string baseUrl = "https://gorest.co.in/public-api/"; // ConfigurationManager.AppSettings.Get("ApiBaseUrl");
        private static readonly string token = "fa114107311259f5f33e70a5d85de34a2499b4401da069af0b1d835cd5ec0d56";   //"ConfigurationManager.AppSettings.Get("Token");

        /// <summary>
        /// This method is used to fetch all the employee data
        /// </summary>
        /// <returns></returns>
        public static List<Datum> GetEmployeeData()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage responseMessage = client.GetAsync("users").Result;

                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var emplyeeDetails = responseMessage.Content.ReadAsAsync<EmployeeModel>().Result;
                        return emplyeeDetails.Data; ;
                    }
                    else
                    {
                        MessageBox.Show($"Unable to fetch the employee details atm, please try after sometime. {Environment.NewLine}" +
                            $"Error Code: {responseMessage.StatusCode} message: {responseMessage.ReasonPhrase}");

                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unable to fetch the employee details atm, please try after sometime. {Environment.NewLine}" +
                            $"Error details: {ex.Message}");
            }
            return null;
         }


        /// <summary>
        /// This method is used to add new employee
        /// </summary>
        /// <param name="empDetail"></param>
        /// <returns></returns>
        public static HttpResponseMessage AddEmployee(Datum empDetail)
        {
            try
            {
                string json = JsonConvert.SerializeObject(empDetail);

                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = client.PostAsJsonAsync("users", empDetail).Result;

                    return response;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// This method is used to get employee details
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public static Datum GetEmployeeDetails(int employeeId)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = client.GetAsync($"users/{employeeId}").Result;

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsAsync<SingleEmplyee>().Result;

                    return result.Data;
                }
            }

            return null;
        }


        /// <summary>
        /// This method is used to update the employee details
        /// </summary>
        /// <param name="empDetail"></param>
        public static void UpdateEmployeeDetailsAsync(Datum empDetail)
        {

            try
            {
                string json = JsonConvert.SerializeObject(empDetail);

                var client = new HttpClient();

                var request = new HttpRequestMessage(HttpMethod.Put, $"{baseUrl}/users/{empDetail.Id}");

                request.Headers.Add("Authorization", $"Bearer {token}");

                var content = new StringContent(json, null, "application/json");

                request.Content = content;

                var response =  client.SendAsync(request).Result;

                response.EnsureSuccessStatusCode();

                Console.WriteLine(response.Content.ReadAsStringAsync());

                MessageBox.Show("Employee Details updated successfully.");
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Unable to update the employee details atm, please connect to the IT support team.{Environment.NewLine} Error Detail : {ex.Message}");
            }
          
        }

        /// <summary>
        /// This method is used to delete the employee details
        /// </summary>
        /// <param name="employeeId"></param>
        public static void DeleteEmployeeData(int employeeId)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);

                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.DeleteAsync($"users/{employeeId}").Result;

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Record deleted successfully.");
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private readonly HttpClient _httpClient;
        private string _token;
        public Form1()
        {
            InitializeComponent();
            _httpClient = new HttpClient();
        }
        private async void button1_Click(object sender, EventArgs e)
        {
            var url = "http://localhost:8080/api/users/authenticate";
            var username = textBox1.Text;
            var password = textBox2.Text;
            var content = new StringContent($"{{\"username\":\"{username}\",\"password\":\"{password}\"}}", Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);
            if (response.IsSuccessStatusCode)
            {
                var tokenResponse = await response.Content.ReadAsStringAsync();
                var tokenObject = Newtonsoft.Json.Linq.JObject.Parse(tokenResponse);
                _token = (string)tokenObject.GetValue("token");

                textBox5.Text = _token;

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
            }
            else
            {
                MessageBox.Show("Failed to authenticate. Please check your credentials.");
            }
        }


        private async void button2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_token))
            {
                var url = "http://localhost:8080/api/users/registeredUsers";

                
                Console.WriteLine("Authorization header: " + _httpClient.DefaultRequestHeaders.Authorization);

                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var usersCount = await response.Content.ReadAsStringAsync();
                   
                    textBox3.Text = usersCount;
                }
                else
                {
                    MessageBox.Show("Failed to retrieve users count. Please try again.");

                    
                    Console.WriteLine("Failed to retrieve users count. Status code: " + response.StatusCode);
                }
            }
            else
            {
                MessageBox.Show("Please authenticate first to retrieve user count.");
            }
        }


        private async void button3_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_token))
            {
                var url = "http://localhost:8080/api/users/randomPrime";
               // _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token); // Include a space after "Bearer"

                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var randomPrime = await response.Content.ReadAsStringAsync();
                   
                    textBox6.Text = randomPrime;
                }
                else
                {
                    MessageBox.Show("Failed to retrieve random prime number. Please try again.");
                    
                    Console.WriteLine("Failed to retrieve random prime number. Status code: " + response.StatusCode);
                
            }
            }
            else
            {
                MessageBox.Show("Please authenticate first to retrieve random prime number.");
            }
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_token))
            {
                var url = "http://localhost:8080/api/users/users";
               // _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);

                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var users = await response.Content.ReadAsStringAsync();
                    
                    textBox4.Text = users;
                }
                else
                {
                    MessageBox.Show("Failed to retrieve the list of users. Please try again.");
                }
            }
            else
            {
                MessageBox.Show("Please authenticate first to retrieve the list of users.");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        
    }


}


using System;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using System.Security.Cryptography;
using Windows.UI.Xaml.Controls;
using System.Text;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace APP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public struct MyStruct
    {
        public string Emails;
        public int id;
    }
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            if (passwdbox.Password == "" || Account.Text == "")
            {

                if (Account.Text == "")
                {
                    Acconterror.Visibility = Visibility.Visible;
                    Acconterror.Text = "账号不能为空";
                }
                if (passwdbox.Password == "")
                {
                    passwderror.Text = "密码不能为空";
                    passwderror.Visibility = Visibility.Visible;
                }
            }
            else
            {
                MD5 mD5 = MD5.Create();
                byte[] inputbyte = System.Text.Encoding.UTF8.GetBytes(passwdbox.Password);
                byte[] hashpasswd = mD5.ComputeHash(inputbyte);
                StringBuilder stringBuilder = new StringBuilder();
                for(int i = 0; i < hashpasswd.Length; i++)
                {
                    stringBuilder.Append(hashpasswd[i].ToString("X2"));
                }
                Model.People people = new Model.People
                {
                    Passwd = stringBuilder.ToString(),
                    Email = Account.Text,
                };
                await IsValidUser(people.Email, people.Passwd);
            }

        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(View.RegisterPage));
        }

        private void ForgetPasswd_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(View.FindPasswd));
        }
        public async Task IsValidUser(string Email, string Password)
        {

            HttpClient httpClient = new HttpClient
            {
                BaseAddress = new System.Uri("http://localhost:60671/")
            };
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            string str = $"api/values/{Email}/{Password}";
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(str);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var isvalid = await httpResponseMessage.Content.ReadAsAsync<int>();
                if (isvalid != -1)
                {
                    MyStruct myStruct = new MyStruct
                    {
                        Emails = Email,
                        id = isvalid
                    };
                    this.Frame.Navigate(typeof(View.HomePage),myStruct);
                }
                else
                {
                    error.Visibility = Visibility.Visible;
                    error.Text = "账号或密码错误";
                }
            }
        }
    }
}

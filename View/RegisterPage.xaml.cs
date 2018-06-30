using APP.Model;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace APP.View
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            this.InitializeComponent();
        }
        private async void Register_Click(object sender, RoutedEventArgs e)
        {
            if (Namebox.Text == "" || passwdbox.Password == "" || Emailbox.Text == "")
            {
                if (Namebox.Text == "")
                {
                   
                    Nameerror.Text = "名字不能为空";
                    Nameerror.Visibility = Visibility.Visible;
                }
                if (Emailbox.Text == "")
                {
                   
                    Emailerror.Text = "电子邮件不能为空";
                    Emailerror.Visibility = Visibility.Visible;
                }
                if (passwdbox.Password == "")
                {
                  
                    Passwderror.Text = "密码不能为空";
                    Passwderror.Visibility = Visibility.Visible;
                }
            }
            else
            {
                MD5 mD5 = MD5.Create();
                byte[] inputbyte = System.Text.Encoding.UTF8.GetBytes(passwdbox.Password);
                byte[] hashpasswd = mD5.ComputeHash(inputbyte);
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < hashpasswd.Length; i++)
                {
                    stringBuilder.Append(hashpasswd[i].ToString("X2"));
                }
                string Email = Emailbox.Text;
                string Passwd = stringBuilder.ToString();
                string Name  = Namebox.Text;
                await RegisUser(Email, Name, Passwd);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
           
            Frame.Navigate(typeof(MainPage));
        }
        public async Task RegisUser(string Email, string Name, string Passwd)
        {

            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            People people = new People
            {
                Email = Email,
                Passwd = Passwd,
                Name = Name,
            };

            var content = new StringContent(JsonConvert.SerializeObject(people), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("http://localhost:60671/api/values/newpeople", content);
            var responseString = await response.Content.ReadAsAsync<int>();
            if (responseString == 0)
            {
                Emailerror.Visibility = Visibility.Visible;
                Emailerror.Text = "账号已经存在";
            }
            else if (responseString == 1)
            {
                this.Frame.Navigate(typeof(MainPage));
            }
            else if (responseString == 2 || responseString == 3)
            {
                Emailerror.Visibility = Visibility.Visible;
                Emailerror.Text = "出现错误";
            }
        }
    }
}

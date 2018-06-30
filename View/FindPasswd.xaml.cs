using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace APP.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FindPasswd : Page
    {
        string UserEmail;
        public FindPasswd()
        {
            this.InitializeComponent();
        }
        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Findbutton_Click(object sender, RoutedEventArgs e)
        {
            //查找是否含有该用户
            UserEmail = Email.Text;
            if (UserEmail == "")
            {
                existEmial.Text = "输入邮件地址";
                existEmial.Visibility = Visibility.Visible;
            }
            else
            {
                await SendMail(UserEmail);
            }
        }
        /// <summary>
        /// 向服务器发送验证码是否正确
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Sen_button_Click(object sender, RoutedEventArgs e)
        {
            await SendCode(verify.Text);
        }

        private async void Modify_Click(object sender, RoutedEventArgs e)
        {
            string ps1 = firstpasswd.Password;
            string ps2 = secondpasswd.Password;
            if (ps1 != "" && ps2 != "")
            {
                if (ps1 != ps2)
                {
                    tblock.Text = "两次的密码不匹配,请重新输入";
                    tblock.Visibility = Visibility.Visible;
                }
                else
                {
                    MD5 mD5 = MD5.Create();
                    byte[] inputbyte = System.Text.Encoding.UTF8.GetBytes(secondpasswd.Password);
                    byte[] hashpasswd = mD5.ComputeHash(inputbyte);
                    StringBuilder stringBuilder = new StringBuilder();
                    for (int i = 0; i < hashpasswd.Length; i++)
                    {
                        stringBuilder.Append(hashpasswd[i].ToString("X2"));
                    }
                    await FixPasswd(stringBuilder.ToString());

                }
            }
            else
            {
                tblock.Text = "密码不能为空";
                tblock.Visibility = Visibility.Visible;
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
        /// <summary>
        /// 验证邮箱
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        private async Task SendMail(string Email)
        {
            HttpClient httpClient = new HttpClient()
            {
                BaseAddress = new Uri("http://localhost:60671/")
            };
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            string str = $"api/values/Yan/{Email}";
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(str);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var isvalid = await httpResponseMessage.Content.ReadAsAsync<bool>();
                if (isvalid == true)
                {
                    verify.Visibility = Visibility.Visible;
                }
                else
                {
                    existEmial.Text = "发送失败,请稍后重试";
                }
            }
        }
        /// <summary>
        /// 判断验证码是否正确
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private async Task SendCode(string code)
        {
            if (verify.Visibility == Visibility.Collapsed)
            {
                existEmial.Text = "你没有输入验证码";
                existEmial.Visibility = Visibility.Visible;
            }
            else if (verify.Visibility == Visibility.Visible && verify.Text == "")
            {
                existEmial.Text = "你没有输入验证码";
                existEmial.Visibility = Visibility.Visible;
            }
            else
            {
                HttpClient httpClient = new HttpClient()
                {
                    BaseAddress = new Uri("http://localhost:60671/")
                };
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                string str = $"api/values/validyan/{code}";
                HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(str);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var isvalid = await httpResponseMessage.Content.ReadAsAsync<bool>();
                    if (isvalid == true)
                    {
                        findmima.Visibility = Visibility.Collapsed;
                        modifymima.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        verifyblock.Visibility = Visibility.Visible;
                        verifyblock.Text = "验证码错误";
                    }
                }
            }
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="Passwd"></param>
        /// <returns></returns>
        private async Task FixPasswd(string Passwd)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            string str = $"api/values/{UserEmail}";
            var content = new StringContent(Passwd, Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await httpClient.PutAsync($"http://localhost:60671/api/values/{UserEmail}", content);
            var res = await httpResponseMessage.Content.ReadAsAsync<bool>();
            if (res == true)
            {
                tblock.Text = "密码已经修改,请重新登陆";
                tblock.Visibility = Visibility.Visible;
            }
            else
            {
                tblock.Text = "发生错误";
                tblock.Visibility = Visibility.Visible;
            }
        }
    }
}

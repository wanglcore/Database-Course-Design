using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
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

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace APP.View
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class AnswerQusition : Page
    {
        public static AnswerQusition Current;
        Dictionary<string, string> keyValues;
        int Userid;
        int Qusitionid;

        public AnswerQusition()
        {
            this.InitializeComponent();
            Current = this;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            keyValues = (Dictionary<string, string>)e.Parameter;

            SetBlockValue();
        }
        private async void SetBlockValue()
        {
            TitleBlock.Text = keyValues["QusitionTitle"].ToString();
            ContentBlock.Text = keyValues["QusitionContent"].ToString();
            Follownum.Text = keyValues["Follownum"].ToString();
            Answernum.Text = keyValues["Answernum"].ToString();
            Qusitionid = Convert.ToInt32(keyValues["Qusitionid"]);
            Userid = Convert.ToInt32(keyValues["Userid"]);
            await Getisfollow(Userid, Qusitionid);
        }
        private void Answer_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, int> keys = new Dictionary<string, int>
            {
                { "Qusitionid", Qusitionid },
                { "Userid", Userid }
            };
            CurrentAnswer.Navigate(typeof(MakeAnswer), keys);
        }

        private async void Follow_Click(object sender, RoutedEventArgs e)
        {
            if (Follow.Content.ToString() == "follow")
            {
                await AddFollow(Userid, Qusitionid);
            }
            else
            {
                await DeleteFollow(Userid, Qusitionid);
            }
        }
        private async Task DeleteFollow(int Userid, int Qusitionid)
        {
            HttpClient httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:60671/")
            };
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            string str = $"api/qusition/deletefollow/{Userid}/{Qusitionid}";
            HttpResponseMessage httpResponseMessage = await httpClient.DeleteAsync(str);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var res = await httpResponseMessage.Content.ReadAsAsync<bool>();
                if (res == true)
                {
                    Follow.Content = "follow";
                    Follownum.Text = (Convert.ToInt32(Follownum.Text.ToString()) - 1).ToString();
                }
            }
        }
        private async Task AddFollow(int Userid, int Qusitionid)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            string str = $"api/qusition/{Userid}/{Qusitionid}";
            var content = new StringContent("", Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await httpClient.PutAsync($"http://localhost:60671/api/qusition/addfollow/{Userid}/{Qusitionid}", content);
            var res = await httpResponseMessage.Content.ReadAsAsync<bool>();
            if (res == true)
            {
                Follow.Content = "unfollow";
                Follownum.Text = (Convert.ToInt32(Follownum.Text.ToString()) + 1).ToString();
            }
        }
        private async Task Getisfollow(int Userid, int Qusitionid)
        {
            HttpClient httpClient = new HttpClient
            {
                BaseAddress = new System.Uri("http://localhost:60671/")
            };
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            string str = $"api/values/getisfollow/{Userid}/{Qusitionid}";
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(str);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var res = await httpResponseMessage.Content.ReadAsAsync<bool>();
                if (res == true)
                {
                    Follow.Content = "followed";
                }
            }
        }
        private async Task GetisAnswer(int Userid, int Qusitionid)
        {
            HttpClient httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:60671/")
            };
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            string str = $"api/values/getisanswer/{Userid}/{Qusitionid}";
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(str);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var res = await httpResponseMessage.Content.ReadAsAsync<bool>();
                if (res == true)
                {
                    Answer.Content = "answered";
                }
            }
        }

        private void UpdateAns_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

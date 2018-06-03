using Newtonsoft.Json;
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
    public sealed partial class MakeAnswer : Page
    {

        Dictionary<string, int> keys;
        int Qusitionid;
        int Userid;
        public MakeAnswer()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            keys = (Dictionary<string, int>)e.Parameter;
            Qusitionid = keys["Qusitionid"];
            Userid = keys["Userid"];
        }

        private async void SubAnswer_Click(object sender, RoutedEventArgs e)
        {
            //TODO 回答问题
            await ContentDialogshow();
        }
        private async Task SubAnswers(Model.Answer answer)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var content = new StringContent(JsonConvert.SerializeObject(answer), Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await httpClient.PostAsync("http://localhost:60671/api/answer/addanswer", content);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var res = await httpResponseMessage.Content.ReadAsAsync<bool>();
                if (res == true)
                {
                    issucc.Text = "Success";
                    AnswerQusition.Current.Answernum.Text = (Convert.ToInt32(AnswerQusition.Current.Answernum.Text) + 1).ToString();
                    AnswerQusition.Current.UpdateAns.Visibility = Visibility.Visible;
                }
                else
                {
                    issucc.Text = "Failure";
                }
            }
        }
        private async Task ContentDialogshow()
        {
            ContentDialog contentDialog = new ContentDialog
            {
                Title = "确认提交回答?",
                PrimaryButtonText = "确认提交",
                SecondaryButtonText = "不提交"
            };
            contentDialog.PrimaryButtonClick += async (_s, _e) =>
            {
                string answertext = MyAnswer.Text;
                Model.Answer answer = new Model.Answer
                {
                    Userid = Userid,
                    Qusitionid = Qusitionid,
                    AnswerContent = MyAnswer.Text
                };
                answer.UpAnsTime = answer.AnswerTime = DateTime.Now;
                await SubAnswers(answer);
            };
            contentDialog.SecondaryButtonClick += (_s, _e) =>
            {
                issucc.Text = "已取消";
            };
            await contentDialog.ShowAsync();
        }
    }
}

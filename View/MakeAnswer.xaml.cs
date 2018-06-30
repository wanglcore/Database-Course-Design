using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

using APP.Model;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.UI;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace APP.View
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MakeAnswer : Page
    {

        Dictionary<string, int> keys;
        People people = HomePage.Current.people;
        Qusition qusition;
        Answershow answershow;
        AnswerDraft answerDraft=null;
        int Userid=HomePage.Current.myStruct.id;
        
        public MakeAnswer()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if(e.Parameter is Qusition)
            {
                qusition = (Qusition)e.Parameter;
            }
            else if(e.Parameter is AnswerDraft)
            {
                answerDraft = (AnswerDraft)e.Parameter;
                MyAnswer.Text = answerDraft.Content;
            }
        }
        /// <summary>
        /// 答案的提交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SubAnswer_Click(object sender, RoutedEventArgs e)
        {
            if (MyAnswer.Text == "")
            {
                MyAnswer.PlaceholderForeground =new Windows.UI.Xaml.Media.SolidColorBrush( Colors.Red);
                MyAnswer.PlaceholderText = "没有输入内容";
            }
            else
            {
                await ContentDialogshow();
            }
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
                    SubAnswer.Content = "已回答";
                    SubAnswer.IsEnabled = false ;
                    CancelAnswer.IsEnabled = false;
                    AnswerQusition.Current.qusition.Answerednum++;
                    answershow = new Answershow
                    {
                        Qusitionid = answer.Qusitionid,
                        Userid = answer.Userid,
                        AnswerContent = answer.AnswerContent,
                        AnswerTime = answer.AnswerTime,
                        UpAnsTime = answer.UpAnsTime,
                        UpAnsnum = answer.UpAnsnum,
                        DownAnsnum = answer.DownAnsnum,
                        AnsCmtnum = answer.AnsCmtnum,
                        Name = people.Name,
                        QName = qusition.QusitionTitle
                    };
                    AnswerQusition.Current.answers.Add(answershow);
                    Frame.GoBack();
                }
                else
                {
                    
                }
            }
        }
        private async Task SubUPAnswer(Model.Answer answer)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("applocation/json"));
            var content = new StringContent(JsonConvert.SerializeObject(answer), Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await httpClient.PostAsync("http://localhost:60671/api/answer/Updateanswer", content);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var res = await httpResponseMessage.Content.ReadAsAsync<bool>();
                if (res == true)
                {
                    Frame.Navigate(typeof(AnswerQusition),answer.Qusitionid);
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
                Answer answer;
                string answertext = MyAnswer.Text;
                int Qusitionid = 0;
                if (answerDraft == null)
                {
                    answer = new Model.Answer
                    {
                        Userid = Userid,
                        Qusitionid = qusition.Qusitionid,
                        AnswerContent =answertext,
                    };
                    Qusitionid = qusition.Qusitionid;
                }
                else
                {
                    answer = new Answer
                    {
                        Userid = people.UserId,
                        Qusitionid = answerDraft.Qid,
                        AnswerContent = answertext,
                    };
                    Qusitionid = answerDraft.Qid;
                }
                answer.UpAnsTime = answer.AnswerTime = DateTime.Now;
                var temp = await GetisAnswer(Userid, Qusitionid);
                if (temp == true)
                {
                    await SubUPAnswer(answer);
                }
                else
                {
                    await SubAnswers(answer);
                }
            };
            contentDialog.SecondaryButtonClick += (_s, _e) =>
            {
                
            };
            await contentDialog.ShowAsync();
        }

        private async void CancelAnswer_Click(object sender, RoutedEventArgs e)
        {
            await ContentDialog();
        }
        private async Task ContentDialog()
        {
            ContentDialog contentDialog = new ContentDialog
            {
                Title = "回答未提交,是否保存?",
                PrimaryButtonText = "保存并退出",
                SecondaryButtonText = "直接退出"
            };
            contentDialog.PrimaryButtonClick +=  (_s, _e) =>
            {
                string str = $"Q---{DateTime.Now}---{qusition.QusitionTitle}---{qusition.Qusitionid}---{MyAnswer.Text}---{people.UserId}";
                Frame.Navigate(typeof(MyDraft), str);
            };
            contentDialog.SecondaryButtonClick += (_s, _e) =>
            {
                if (Frame.CanGoBack)
                {
                    Frame.GoBack();
                }
            };
            await contentDialog.ShowAsync();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        private void SaveAnswer_Click(object sender, RoutedEventArgs e)
        {
            string str = $"Q---{DateTime.Now}---{qusition.QusitionTitle}---{qusition.Qusitionid}---{MyAnswer.Text}---{people.UserId}";
            Frame.Navigate(typeof(MyDraft), str);
        }



        private async Task<bool> GetisAnswer(int Userid, int Qusitionid)
        {
            HttpClient httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:60671/")
            };
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            string str = $"api/answer/getisanswer/{Userid}/{Qusitionid}";
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(str);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var res = await httpResponseMessage.Content.ReadAsAsync<bool>();
                return res;
            }
            else
            {
                return false;
            }
        }
    }
}

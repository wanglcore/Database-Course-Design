using APP.Model;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Net.Http;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI;
using System.Text;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace APP.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>


    public sealed partial class LabelDetail : Page
    {
        
        private Label label= new Label();
        Dictionary<string, string> NameMap = new Dictionary<string, string>
        {
            {"C#","C%23" },
            {"C++","C%2B%2B" }
        };
        MyStruct myStruct= HomePage.Current.myStruct;
        public LabelDetail()
        {
            this.InitializeComponent();
           
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is Label)
            {
                label = (Label)e.Parameter;
            }
            else if(e.Parameter is string labelname)
            {
                string mapname=labelname;
                if (labelname == "C#" || labelname == "C++")
                {
                    mapname = NameMap[labelname];
                }
                await GetLabelDetail(mapname);
            }
            ContentFrame.Navigate(typeof(LabelDetailHQ),label);
            base.OnNavigatedTo(e);
        }
        
        private void HaveAnswerButton_Click(object sender, RoutedEventArgs e)
        {
           
            this.ContentFrame.Navigate(typeof(LabelDetailHQ), label);
        }

        private void UnanswerButton_Click(object sender, RoutedEventArgs e)
        {
           
            ContentFrame.Navigate(typeof(LabelDetailQ), label);
        }

        private void FollowedpeopleButton_Click(object sender, RoutedEventArgs e)
        {
         
            Frame.Navigate(typeof(LabelDetailP), label);
        }

        private async Task GetLabelDetail(String labelname)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new System.Uri("http://localhost:60671/");
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            string str = $"api/label/getlabeldetail/{labelname}";
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(str);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                label = await httpResponseMessage.Content.ReadAsAsync<Label>();
                Labelname.Text = label.Labelname;
                Labelbrief.Text = label.LabelBrief;
                ClockIcon.Label = label.LabelCTime.ToString();
                MessageIcon.Label = label.LabelQnum.ToString();
                AddnumIcon.Label = label.LabelUnum.ToString();
                await IsFollow(myStruct.id, label.Labelid);
            }
        }

        private async void AddFollow_Click(object sender, RoutedEventArgs e)
        {
            var fore = AddFollow.Foreground as SolidColorBrush;
            if (fore.Color == Colors.Black)
            {
                await PutFollow(myStruct.id, label.Labelid);
            }
            else if (fore.Color == Colors.Blue)
            {
                await DelPutFollow(myStruct.id, label.Labelid);
            }
        }

        private async Task PutFollow(int Userid,int labelid)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var content = new StringContent("", Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await httpClient.PutAsync($"http://localhost:60671/api/label/flabel/{Userid}/{labelid}", content);
            var res = await httpResponseMessage.Content.ReadAsAsync<bool>();
            
                if (res == true)
                {
                    AddFollow.Foreground = new SolidColorBrush(Colors.Blue);
                    label.LabelUnum++;
                    AddnumIcon.Label = label.LabelUnum.ToString();
                }
            

        }

        private async Task DelPutFollow(int Userid, int labelid)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var content = new StringContent("", Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await httpClient.PutAsync($"http://localhost:60671/api/label/uflabel/{Userid}/{labelid}", content);
            var res = await httpResponseMessage.Content.ReadAsAsync<bool>();
            if (res == true)
                {
                    AddFollow.Foreground = new SolidColorBrush(Colors.Black);
                    label.LabelUnum--;
                    AddnumIcon.Label = label.LabelUnum.ToString();

                }
            

        }

        private async Task IsFollow(int Userid, int labelid)
        {
            HttpClient httpClient = new HttpClient
            {
                BaseAddress = new System.Uri("http://localhost:60671/")
            };
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            string str = $"api/label/isfollow/{Userid}/{labelid}";
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(str);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var res = await httpResponseMessage.Content.ReadAsAsync<bool>();
                if (res == true)
                {
                    AddFollow.Foreground = new SolidColorBrush(Colors.Blue);
                }
            }

        }
    }

}

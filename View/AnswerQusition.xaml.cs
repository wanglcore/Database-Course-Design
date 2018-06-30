using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json;
using APP.Model;
using Windows.UI.Xaml.Media;
using Windows.UI;
using System.ServiceModel.Channels;
using Windows.Storage;

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
        private Answershow someoneshow;
        public Qusition qusition;
        public ObservableCollection<Answershow> answers;
        int Askerid;
        private int Myid = HomePage.Current.myStruct.id;
        int Answerid;
        int Qusitionid;
        int Pageid;
        public AnswerQusition()
        {
            this.InitializeComponent();
            Current = this;
        }
        protected override async  void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            await NameMap.TextWrapping();

        }


        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await NameMap.Read();
            if (e.Parameter is Qusition)
            {

                qusition = (Qusition)e.Parameter;
                if (qusition.Qlabel1 == "")
                {
                    Label1.Visibility = Visibility.Collapsed;
                }
                if (qusition.QLabel2 == "")
                {
                    Label2.Visibility = Visibility.Collapsed;
                }
                SetBlockValue();
                Qusitionid = qusition.Qusitionid;
                Askerid = qusition.Askerid;
                await GetAllAnswer(qusition.Qusitionid);
            }
            else if (e.Parameter is Answershow answershow)
            {
                someoneshow = answershow;
                await GetById(someoneshow.Qusitionid);
            }
            else if(e.Parameter is int ID)
            {
                await GetById(ID);
            }
            base.OnNavigatedTo(e);
        }

        private async Task GetById(int Qusitonid)
        {
            HttpClient httpClient = new HttpClient
            {
                BaseAddress = new System.Uri("http://localhost:60671/")
            };
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            string str = $"api/qusition/getqusition/{Qusitonid}";
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(str);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                qusition = await httpResponseMessage.Content.ReadAsAsync<Qusition>();
            if (qusition.Qlabel1 == "")
            {
                Label1.Visibility = Visibility.Collapsed;
            }
            if (qusition.QLabel2 == "")
            {
                Label2.Visibility = Visibility.Collapsed;
            }
                SetBlockValue();
                Qusitionid = qusition.Qusitionid;
                Askerid = qusition.Askerid;
                await GetAllAnswer(qusition.Qusitionid);
            }
        }
        private async void SetBlockValue()
        {
            await Getisfollow(Myid, Qusitionid);
            await GetisAnswer(Myid, Qusitionid);
            this.Bindings.Update();
        }
        /// <summary>
        /// 点击回答按钮,激发该事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Answer_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MakeAnswer), qusition);
        }

        /// <summary>
        /// 点击关注按钮,激发该事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Follow_Click(object sender, RoutedEventArgs e)
        {
            if (Follow.Label.ToString() == "follow")
            {
                await AddFollow(Myid, Qusitionid);
            }
            else
            {
                await DeleteFollow(Myid, Qusitionid);
            }
        }
        /// <summary>
        /// 取消对问题的关注
        /// </summary>
        /// <param name="Userid"></param>
        /// <param name="Qusitionid"></param>
        /// <returns></returns>
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
                    Follow.Label = "follow";
                    qusition.Followednum = qusition.Followednum - 1;
                    AddButton.Label = qusition.Followednum.ToString();
                }
            }
        }
        /// <summary>
        /// 添加关注
        /// </summary>
        /// <param name="Userid"></param>
        /// <param name="Qusitionid"></param>
        /// <returns></returns>
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
                Follow.Label = "unfollow";
                qusition.Followednum = qusition.Followednum + 1;
                AddButton.Label = qusition.Followednum.ToString();
            }
            
        }
        

        /// <summary>
        /// 判断一个用户是否关注一个问题
        /// </summary>
        /// <param name="Userid"></param>
        /// <param name="Qusitionid"></param>
        /// <returns></returns>
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
                    Follow.Label = "unfollow";
                }
                else
                {
                    Follow.Label = "follow";
                }
            }
        }

        /// <summary>
        /// 判断一个用户是否回答一个问题
        /// </summary>
        /// <param name="Userid"></param>
        /// <param name="Qusitionid"></param>
        /// <returns></returns>
        private async Task GetisAnswer(int Userid, int Qusitionid)
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
                if (res == true)
                {
                    Answer.Label = "answered";
                    Answer.IsEnabled = false;
                    UPAnswerButton.Visibility = Visibility.Visible;
                }
                else
                {
                    Answer.Label = "answer";
                    Answer.IsEnabled = true;
                    UPAnswerButton.Visibility = Visibility.Collapsed;
                }
            }
        }

        /// <summary>
        /// 获得一个问题的全部回答
        /// </summary>
        /// <param name="Qusitionid"></param>
        private async Task GetAllAnswer(int Qusitionid)
        {
            HttpClient httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:60671/")
            };
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            string str = $"api/answer/getallqa/{qusition.Askerid}/{qusition.Qusitionid}";
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(str);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var res = await httpResponseMessage.Content.ReadAsAsync<string>();
                if (res != string.Empty)
                {
                    answers = new ObservableCollection<Answershow>(JsonConvert.DeserializeObject<List<Answershow>>(res));
                    CurrentAnswerList.ItemsSource = answers;
                }
            }
        }

        private void CurrentAnswerList_ItemClick(object sender, ItemClickEventArgs e)
        {
            foreach (var item in CurrentAnswerList.Items)
            {
                var container = (ListViewItem)CurrentAnswerList.ContainerFromItem(item);
                if (container != null)
                {
                    var ContentGrid = (container.ContentTemplateRoot as FrameworkElement)?.FindName("ContentGrid") as Grid;
                    var ContentBlock = (container.ContentTemplateRoot as FrameworkElement)?.FindName("ContentBlock") as TextBlock;
                    ContentGrid.Visibility = e.ClickedItem == item ? Visibility.Visible : Visibility.Collapsed;
                    ContentBlock.Visibility = e.ClickedItem == item ? Visibility.Collapsed : Visibility.Visible;
                    var Buttons = (container.ContentTemplateRoot as FrameworkElement)?.FindName("NameBlock") as Button;
                    int Userid = Convert.ToInt32(Buttons.Tag);
                    if (NameMap.UPAnswer.Contains($"{Qusitionid} {Userid} {Myid}"))
                    {
                        var LikeButton = (container.ContentTemplateRoot as FrameworkElement)?.FindName("LikeButton") as AppBarButton;
                        LikeButton.Foreground = new SolidColorBrush(Colors.Blue);
                    }
                    if (NameMap.DNAnswer.Contains($"{Qusitionid} {Userid} {Myid}"))
                    {
                        var Dislikebutton = (container.ContentTemplateRoot as FrameworkElement)?.FindName("DislikeButton") as AppBarButton;
                        Dislikebutton.Foreground = new SolidColorBrush(Colors.Blue);
                    }
        
                }
            }
        }
        private  void CommentButton_Click(object sender, RoutedEventArgs e)
        {
            AppBarButton appBarButton = (AppBarButton)sender;
            int Answerid = Convert.ToInt32(appBarButton.Tag);
            ListViewItem listViewItem = NameMap.FindParent<ListViewItem>(appBarButton);
            if (listViewItem != null)
            {
                string Name;
                if((listViewItem.ContentTemplateRoot as FrameworkElement)?.FindName("NameBlock") is Button NameBlock){
                    Name = NameBlock.Content.ToString();
                    Dictionary<string, string> keyValuePairs = new Dictionary<string, string>{
                { "Qusitionid",Qusitionid.ToString()},
                {"Userid",Answerid.ToString() },
                {"Name",Name }
            };

                    Frame.Navigate(typeof(AnswerCmt), keyValuePairs);
                }
            }
        }

        private  void UPAnswerButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MakeAnswer), qusition);
        }

        private async void DislikeButton_Click(object sender, RoutedEventArgs e)
        {
            AppBarButton appBarButton = (AppBarButton)sender;
            ListViewItem listViewItem = NameMap.FindParent<ListViewItem>(appBarButton);
            if (listViewItem != null)
            {

            }
            int Answerid = Convert.ToInt32(appBarButton.Tag);
            int Myid = (HomePage.Current.myStruct.id);
            var fore = appBarButton.Foreground as SolidColorBrush;
            if (fore.Color == Colors.Blue)
            {
                await DelPutDisLike(Answerid, Myid, Qusitionid);
            }
            else
            {
                await PutDisLike(Answerid, Myid, Qusitionid);
            }
        }
        private async Task DelPutDisLike(int Userid,int Myid,int Qusitionid)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            string str = $"api/answer/delputdislike/{Userid}/{Qusitionid}/{Myid}";
            var content = new StringContent("", Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await httpClient.PutAsync($"http://localhost:60671/api/answer/delputdislike/{Userid}/{Qusitionid}/{Myid}", content);
            var res = await httpResponseMessage.Content.ReadAsAsync<bool>();
            if (res == true)
            {
                Modifyinf(Userid, 1);
            }
        }
        private async Task DelPutLike(int Userid,int Myid,int Qusitionid)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            string str = $"api/answer/delputlike/{Userid}/{Qusitionid}/{Myid}";
            var content = new StringContent("", Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await httpClient.PutAsync($"http://localhost:60671/api/answer/delputlike/{Userid}/{Qusitionid}/{Myid}", content);
            var res = await httpResponseMessage.Content.ReadAsAsync<bool>();
            if (res == true)
            {
                Modifyinf(Userid, 0);
            }
        }

        private async void LikeButton_Click(object sender, RoutedEventArgs e)
        {
            AppBarButton appBarButton = (AppBarButton)sender;
            int Answerid = Convert.ToInt32(appBarButton.Tag);
            int Myid = (HomePage.Current.myStruct.id);
            var fore = appBarButton.Foreground as SolidColorBrush;
            if (fore.Color == Colors.Black)
            {
                await PutLike(Answerid, Myid, Qusitionid);
            }
            else
            {
                await DelPutLike(Answerid, Myid, Qusitionid);
            }
        }
        private async Task PutDisLike(int Userid, int Myid, int Qusitionid)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            string str = $"api/answer/putdislike/{Userid}/{Qusitionid}/{Myid}";
            var content = new StringContent("", Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await httpClient.PutAsync($"http://localhost:60671/api/answer/putdislike/{Userid}/{Qusitionid}/{Myid}", content);
            var res = await httpResponseMessage.Content.ReadAsAsync<bool>();
            if (res == true)
            {
                Modifyinf(Userid, 1);
            }
        }
        private async Task PutLike(int Userid, int Myid, int Qusitionid)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            string str = $"api/answer/putlike/{Userid}/{Qusitionid}/{Myid}";
            var content = new StringContent("", Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await httpClient.PutAsync($"http://localhost:60671/api/answer/putlike/{Userid}/{Qusitionid}/{Myid}", content);
            var res = await httpResponseMessage.Content.ReadAsAsync<bool>();
            if (res == true)
            {
                Modifyinf(Userid, 0);
            }
        }

        private void Modifyinf(int Userid, int id)
        {
            var ans = answers.Where(p => p.Userid == Userid).FirstOrDefault();
            List<AppBarButton> appBarButtons = NameMap.GetChildObjects<AppBarButton>(CurrentAnswerList);
            if (id == 0)
            {
                var apps = appBarButtons.Where(p => p.Name == "LikeButton" && Convert.ToInt32(p.Tag) == Userid).FirstOrDefault();
                if (apps != null)
                {
                    var fore = apps.Foreground as SolidColorBrush;
                    if (fore.Color == Colors.Black)
                    {
                        ans.UpAnsnum++;
                        apps.Foreground = new SolidColorBrush(Colors.Blue);
                        NameMap.UPAnswer.Add($"{qusition.Qusitionid} {apps.Tag} {Myid}");
                    }
                    else
                    {
                        ans.UpAnsnum--;
                        apps.Foreground = new SolidColorBrush(Colors.Black);
                        NameMap.UPAnswer.Remove($"{qusition.Qusitionid} {apps.Tag} {Myid}");
                    }
                    apps.Label = ans.UpAnsnum.ToString();

                }
            }
            else if (id == 1)
            {
                var apps = appBarButtons.Where(p => p.Name == "DislikeButton" && Convert.ToInt32(p.Tag) == Userid).FirstOrDefault();
                if (apps != null)
                {
                    var fore = apps.Foreground as SolidColorBrush;
                    if (fore.Color == Colors.Black)
                    {
                        ans.DownAnsnum++;
                        apps.Foreground = new SolidColorBrush(Colors.Blue);
                        NameMap.DNAnswer.Add($"{qusition.Qusitionid} {apps.Tag} {Myid}");
                    }
                    else
                    {
                        ans.DownAnsnum--;
                        apps.Foreground = new SolidColorBrush(Colors.Black);
                        NameMap.DNAnswer.Remove($"{qusition.Qusitionid} {apps.Tag} {Myid}");
                    }
                    apps.Label = ans.DownAnsnum.ToString();
                }
            }

        }
      
        private  void Label0_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string labelname = button.Content.ToString();
            this.Frame.Navigate(typeof(LabelDetail),labelname);
        }

        private  void NameBlock_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            this.Frame.Navigate(typeof(UUserPage),Convert.ToInt32(button.Tag));
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }
    }
}

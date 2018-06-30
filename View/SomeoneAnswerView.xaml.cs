using APP.Model;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.IO;
using Windows.UI.Xaml.Media;
using Windows.UI;
using System.Text;
using Windows.Storage;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace APP.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SomeoneAnswerView : Page
    {
        ObservableCollection<Answershow> answershows;
        People people;
        MyStruct myStruct = HomePage.Current.myStruct;
        public SomeoneAnswerView()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            NameMap.TextWrapping();

        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if(e.Parameter is People)
            {
                people = (People)e.Parameter;
                await GetHisAnswer(people);
            
            }
        }

        private async Task GetHisAnswer(People people)
        {
            HttpClient httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:60671/")
            };
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            string str = $"api/answer/gethisanswer/{people.UserId}";
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(str);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var res = await httpResponseMessage.Content.ReadAsAsync<string>();
                answershows = new ObservableCollection<Answershow>(JsonConvert.DeserializeObject<List<Answershow>>(res));
                CurrentAnswerList.ItemsSource = answershows;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
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
                    int Qusitionid = Convert.ToInt32(Buttons.Tag);
                    if (NameMap.UPAnswer.Contains($"{Qusitionid} {people.UserId} {myStruct.id}"))
                    {
                        var LikeButton= (container.ContentTemplateRoot as FrameworkElement)?.FindName("LikeButton") as AppBarButton;
                        LikeButton.Foreground = new SolidColorBrush(Colors.Blue);
                    }
                    if(NameMap.DNAnswer.Contains($"{Qusitionid} {people.UserId} {myStruct.id}"))
                    {
                        var Dislikebutton = (container.ContentTemplateRoot as FrameworkElement)?.FindName("DislikeButton") as AppBarButton;
                        Dislikebutton.Foreground = new SolidColorBrush(Colors.Blue);
                    }
                }
            }
        }

        private  void NameBlock_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            var ans = answershows.Where(p => p.Qusitionid == Convert.ToInt32(button.Tag)).FirstOrDefault();
            this.Frame.Navigate(typeof(AnswerQusition), ans);
        }

        private async void DislikeButton_Click(object sender, RoutedEventArgs e)
        {
            AppBarButton appBarButton = (AppBarButton)sender;
            ListViewItem listViewItem = NameMap.FindParent<ListViewItem>(appBarButton);
            var fore = appBarButton.Foreground as SolidColorBrush;
            if (fore.Color == Colors.Blue)
            {
                await DelPutDisLike(people.UserId, myStruct.id, Convert.ToInt32(appBarButton.Tag));
            }
            else
            {
                await PutDisLike(people.UserId, myStruct.id, Convert.ToInt32(appBarButton.Tag));
            }
        }
        private async void LikeButton_Click(object sender, RoutedEventArgs e)
        {
            AppBarButton appBarButton = (AppBarButton)sender;
            var fore = appBarButton.Foreground as SolidColorBrush;
            if (fore.Color == Colors.Black)
            {
                await PutLike(people.UserId, myStruct.id, Convert.ToInt32(appBarButton.Tag));
            }
            else
            {
                await DelPutLike(people.UserId, myStruct.id, Convert.ToInt32(appBarButton.Tag));
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
                Modifyinf(Qusitionid, 1);
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
                Modifyinf(Qusitionid, 0);
            }
        }

        private async Task DelPutDisLike(int Userid, int Myid, int Qusitionid)
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
                Modifyinf(Qusitionid, 1);
            }
        }
        private async Task DelPutLike(int Userid, int Myid, int Qusitionid)
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
                Modifyinf(Qusitionid, 0);
            }
        }

        private void Modifyinf(int Userid, int id)
        {
            var ans = answershows.Where(p => p.Qusitionid == Userid).FirstOrDefault();
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
                        NameMap.UPAnswer.Add($"{apps.Tag} {people.UserId} {myStruct.id}");
                    }
                    else
                    {
                        ans.UpAnsnum--;
                        apps.Foreground = new SolidColorBrush(Colors.Black);
                        NameMap.UPAnswer.Remove($"{apps.Tag} {people.UserId} {myStruct.id}");
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
                        NameMap.DNAnswer.Add($"{apps.Tag} {people.UserId} {myStruct.id}");
                    }
                    else
                    {
                        ans.DownAnsnum--;
                        apps.Foreground = new SolidColorBrush(Colors.Black);
                        NameMap.DNAnswer.Remove($"{apps.Tag} {people.UserId} {myStruct.id}");
                    }
                    apps.Label = ans.DownAnsnum.ToString();
                }
            }

        }

        private void ReadMore_Click(object sender, RoutedEventArgs e)
        {
            HyperlinkButton hyperlinkButton = (HyperlinkButton)sender;
            var answershow = answershows.Where(p => p.Qusitionid == Convert.ToInt32(hyperlinkButton.Tag)).FirstOrDefault();
            Frame.Navigate(typeof(AnswerQusition), answershow);
        }
    }
}

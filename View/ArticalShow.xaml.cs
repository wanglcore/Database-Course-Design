using APP.Model;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace APP.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ArticalShow : Page
    {
        Artical artical;
        People people = HomePage.Current.people;
        DispatcherTimer timer;
        public ArticalShow()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            await NameMap.TextWrapping();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            await NameMap.Read();
            base.OnNavigatedTo(e);
            if(e.Parameter is Artical)
            {
                artical = (Artical)e.Parameter;
                if (artical.Userid == people.UserId)
                {
                    AddFriendIcon.IsEnabled = false;
                }
                else
                {
                    await GetisFollow(artical.Userid);
                }
            }
            this.Bindings.Update();
        }

        private void CommentButton_Click(object sender, RoutedEventArgs e)
        {
           
            Frame.Navigate(typeof(ArticalCmt),artical);
        }

        private async  void AddFriendIcon_Click(object sender, RoutedEventArgs e)
        {

            UserFollow userFollow = new UserFollow
            {
                Userid = people.UserId,
                FollowUserid = artical.Userid,
                FollowTime = DateTime.Now
            };
            await AddFollow(userFollow);
        }


        private async Task GetisFollow(int Userid)
        {
            int Myid = HomePage.Current.myStruct.id;
            HttpClient httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:60671/")
            };
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            string str = $"api/values/isfollow/{Userid}/{Myid}";
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(str);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var res =await httpResponseMessage.Content.ReadAsAsync<int>();
                if (res == 1)
                {
                    AddFriendIcon.Visibility = Visibility.Collapsed;
                    NoAddFriendIcon.Visibility = Visibility.Visible;
                }
                else
                {
                    AddFriendIcon.Visibility = Visibility.Visible;
                    NoAddFriendIcon.Visibility = Visibility.Collapsed;
                }
            }
            if(NameMap.UPArtical.Contains($"{artical.Articalid} {artical.Userid} {Myid}"))
            {
                Likebutton.Foreground = new SolidColorBrush(Colors.Blue);
            }

            if (NameMap.DNArtical.Contains($"{artical.Articalid} {artical.Userid} {Myid}"))
            {
                Dislikebutton.Foreground = new SolidColorBrush(Colors.Blue);
            }

        }

        private async Task AddFollow(UserFollow userFollow)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var content = new StringContent(JsonConvert.SerializeObject(userFollow), Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await httpClient.PostAsync("http://localhost:60671/api/values/addfollow", content);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var res = await httpResponseMessage.Content.ReadAsAsync<bool>();
                if (res == true)
                {
                    AddFriendIcon.Visibility = Visibility.Collapsed;
                    NoAddFriendIcon.Visibility = Visibility.Visible;
                }
            }
        }

        private async Task DeleteFollow(UserFollow userFollow)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var content = new StringContent(JsonConvert.SerializeObject(userFollow), Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await httpClient.PostAsync("http://localhost:60671/api/values/deletefollow", content);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var res = await httpResponseMessage.Content.ReadAsAsync<bool>();
                if (res == true)
                {
                    AddFriendIcon.Visibility = Visibility.Visible;
                    NoAddFriendIcon.Visibility = Visibility.Collapsed;
                }
            }
        }

        private async void NoAddFriendIcon_Click(object sender, RoutedEventArgs e)
        {
            UserFollow userFollow = new UserFollow
            {
                Userid = people.UserId,
                FollowUserid = artical.Userid,
            };
            await DeleteFollow(userFollow);
        }

        private void Label0_Click(object sender, RoutedEventArgs e)
        {
           
            Button button = (Button)sender;
            string name = button.Content.ToString();
            this.Frame.Navigate(typeof(LabelDetail), name);
        }

        private async void Dislikebutton_Click(object sender, RoutedEventArgs e)
        {
            AppBarButton appBarButton = (AppBarButton)sender;
            var fore = appBarButton.Foreground as SolidColorBrush;
            if (fore.Color == Colors.Black)
            {
                await Putdislike(artical.Articalid, people.UserId, appBarButton);
            }
            else
            {
                await DelPutdislike(artical.Articalid, people.UserId, appBarButton);
            }
        }

        private async Task Putlike(int Articalid,int Myid,AppBarButton appBarButton)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var content = new StringContent("", Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await httpClient.PutAsync($"http://localhost:60671/api/artical/putlike/{Articalid}/{Myid}", content);
            var res = await httpResponseMessage.Content.ReadAsAsync<bool>();
            if (res == true)
            {
                Modifyinfor(0, appBarButton);
            }
        }
        private async Task Putdislike(int Articalid,int Myid,AppBarButton appBarButton)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var content = new StringContent("", Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await httpClient.PutAsync($"http://localhost:60671/api/artical/putdislike/{Articalid}/{Myid}", content);
            var res = await httpResponseMessage.Content.ReadAsAsync<bool>();
            if (res == true)
            {
                Modifyinfor(1, appBarButton);
            }
        }

        private async Task DelPutdislike(int Articalid,int Myid,AppBarButton appBarButton)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var content = new StringContent("", Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await httpClient.PutAsync($"http://localhost:60671/api/artical/delputdislike/{Articalid}/{Myid}", content);
            var res = await httpResponseMessage.Content.ReadAsAsync<bool>();
            if (res == true)
            {
                Modifyinfor(1, appBarButton);
            }
        }

        private async Task DelPutlike(int Articalid,int Myid,AppBarButton appBarButton)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var content = new StringContent("", Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await httpClient.PutAsync($"http://localhost:60671/api/artical/delputlike/{Articalid}/{Myid}", content);
            var res = await httpResponseMessage.Content.ReadAsAsync<bool>();
            if (res == true)
            {
                Modifyinfor(0, appBarButton);
            }
        }

        private void Modifyinfor(int option,AppBarButton appBarButton)
        {
            var fore = appBarButton.Foreground as SolidColorBrush;
            if (option == 0)
            {
                if (fore.Color == Colors.Black)
                {
                    artical.UpArtnum++;
                    appBarButton.Foreground = new SolidColorBrush(Colors.Blue);
                    NameMap.UPArtical.Add($"{artical.Articalid} {artical.Userid} {HomePage.Current.myStruct.id}");
                }
                else if (fore.Color == Colors.Blue)
                {
                    artical.UpArtnum--;
                    appBarButton.Foreground = new SolidColorBrush(Colors.Black);
                    NameMap.UPArtical.Remove($"{artical.Articalid} {artical.Userid} {HomePage.Current.myStruct.id}");

                }
                appBarButton.Label = artical.UpArtnum.ToString();
            }
            else if (option == 1)
            {
                if (fore.Color == Colors.Black)
                {
                    artical.DownArtnum++;
                    appBarButton.Foreground = new SolidColorBrush(Colors.Blue);
                    NameMap.DNArtical.Add($"{artical.Articalid} {artical.Userid} {HomePage.Current.myStruct.id}");

                }
                else if (fore.Color == Colors.Blue)
                {
                    artical.DownArtnum--;
                    appBarButton.Foreground = new SolidColorBrush(Colors.Black);
                    NameMap.DNArtical.Remove($"{artical.Articalid} {artical.Userid} {HomePage.Current.myStruct.id}");

                }
                appBarButton.Label = artical.DownArtnum.ToString();
            }
        }

        private async void Likebutton_Click(object sender, RoutedEventArgs e)
        {
            AppBarButton appBarButton = (AppBarButton)sender;
            var fore = appBarButton.Foreground as SolidColorBrush;
            if (fore.Color == Colors.Black)
            {
                await Putlike(artical.Articalid, people.UserId, appBarButton);
            }
            else
            {
                await DelPutlike(artical.Articalid, people.UserId, appBarButton);
            }
        }

        private void Username_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Frame.Navigate(typeof(UUserPage), Convert.ToInt32(button.Tag));
        }

       
    }
}

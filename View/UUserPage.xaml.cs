using APP.Model;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace APP.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UUserPage : Page
    {
        People people;
        public UUserPage()
        {
            this.InitializeComponent();
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if(e.Parameter is People)
            {
                people = (People)e.Parameter;
                await GetInfor(people.UserId);
            }
            else if(e.Parameter is int Uid)
            {
                await GetInfor(Uid);
            }
        }

        private async Task GetInfor(int Uid)
        {
            HttpClient httpClient = new HttpClient
            {
                BaseAddress = new System.Uri("http://localhost:60671/")
            };
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            string str = $"api/values/userinfor/{Uid}";
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(str);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var res =await httpResponseMessage.Content.ReadAsAsync<People>();
                if (res != null)
                {
                    people = res;
                    Bindings.Update();
                }
            }
        }

        private void Hisanswer_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SomeoneAnswerView), people);
        }

        private void Hisqusition_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(UseridQusition), people);
        }

        private void Hisconpeople_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Somepeople), people);
        }

        private void Hisartical_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SomeoneArtical), people);

        }

        private void Conhispeople_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Fsomeone), people);

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        private async Task GetisFollow(int Userid,int Myid)
        {
            HttpClient httpClient = new HttpClient
            {
                BaseAddress = new System.Uri("http://localhost:60671/")
            };
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            string str = $"api/values/isfollow/{Userid}/{Myid}";
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(str);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var content = await httpResponseMessage.Content.ReadAsAsync<bool>();
                if (content == true)
                {
                    AddFriend.Visibility = Visibility.Collapsed;
                    Contact.Visibility = Visibility.Visible;
                }
                else
                {
                    AddFriend.Visibility = Visibility.Visible;
                    Contact.Visibility = Visibility.Collapsed;
                }
            }
        }

        private async void AddFriend_Click(object sender, RoutedEventArgs e)
        {
            UserFollow userFollow = new UserFollow
            {
                Userid = HomePage.Current.myStruct.id,
                FollowUserid = people.UserId,
                FollowTime = DateTime.Now,
            };
            await AddFollow(userFollow);
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
                    AddFriend.Visibility = Visibility.Collapsed;
                    Contact.Visibility = Visibility.Visible;
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
                    AddFriend.Visibility = Visibility.Visible;
                    Contact.Visibility = Visibility.Collapsed;
                }
            }
        }

        private async void Contact_Click(object sender, RoutedEventArgs e)
        {
            UserFollow userFollow = new UserFollow
            {
                Userid = HomePage.Current.myStruct.id,
                FollowTime = DateTime.Now,
                FollowUserid = people.UserId,
            };
            await DeleteFollow(userFollow);
        }
    }
}




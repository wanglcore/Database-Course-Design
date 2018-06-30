using APP.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public sealed partial class Somepeople : Page
    {
        People people;
        ObservableCollection<MyFollowUser> peoples;
        int sourcePage = 0;
        public Somepeople()
        {
            this.InitializeComponent();
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if(e.Parameter is People)
            {
                people = (People)e.Parameter;
                await GetMyFollow(people);
                if (people.UserId == HomePage.Current.people.UserId)
                {
                    sourcePage = 1;
                }
            }
        }

        private void Cancelbutton_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private async Task GetMyFollow(People people)
        {
            HttpClient httpClient = new HttpClient
            {
                BaseAddress = new System.Uri("http://localhost:60671/")
            };
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            string str = $"api/values/getfollow/{people.UserId}";
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(str);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var listlabel = await httpResponseMessage.Content.ReadAsAsync<string>();
                if (listlabel != string.Empty)
                {
                    peoples = new ObservableCollection<MyFollowUser>(JsonConvert.DeserializeObject<List<MyFollowUser>>(listlabel));
                    Myfollow.ItemsSource = peoples;
                }
            }
        }

        private void Myfollow_ItemClick(object sender, ItemClickEventArgs e)
        {
            MyFollowUser fpeople = (MyFollowUser)e.ClickedItem;
            Frame.Navigate(typeof(UUserPage), fpeople.FollowUserid);
        }

        private async void Contact_Click(object sender, RoutedEventArgs e)
        {
            AppBarButton appBarButton = (AppBarButton)sender;
            int tag = Convert.ToInt32(appBarButton.Tag);
            if (sourcePage==1)
            {
                UserFollow userFollow = new UserFollow
                {
                    Userid = HomePage.Current.myStruct.id,
                    FollowUserid = tag,
                };
                await DeleteFollow(userFollow, appBarButton);
            }
        }

        private async Task DeleteFollow(UserFollow userFollow,AppBarButton appBarButton)
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
                    var listviewitem = NameMap.FindParent<ListViewItem>(appBarButton);
                    if (listviewitem != null)
                    {
                        var contact = (listviewitem.ContentTemplateRoot as FrameworkElement)?.FindName("Contact") as AppBarButton;
                        var addfriend = (listviewitem.ContentTemplateRoot as FrameworkElement)?.FindName("AddFriend") as AppBarButton;
                        contact.Visibility = Visibility.Collapsed;
                        addfriend.Visibility = Visibility.Visible;
                    }
                }
            }
        }

        private async void AddFriend_Click(object sender, RoutedEventArgs e)
        {
            AppBarButton appBarButton = (AppBarButton)sender;
            int tag = Convert.ToInt32(appBarButton.Tag);
            if (sourcePage==1)
            {
                UserFollow userFollow = new UserFollow
                {
                    Userid = HomePage.Current.myStruct.id,
                    FollowTime = DateTime.Now,
                    FollowUserid = tag,
                };
                await AddFollow(userFollow, appBarButton);
            }
        }

        private async Task AddFollow(UserFollow userFollow, AppBarButton appBarButton)
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
                    var listviewitem = NameMap.FindParent<ListViewItem>(appBarButton);
                    if (listviewitem != null)
                    {
                        var contact = (listviewitem.ContentTemplateRoot as FrameworkElement)?.FindName("Contact") as AppBarButton;
                        var addfriend = (listviewitem.ContentTemplateRoot as FrameworkElement)?.FindName("AddFriend") as AppBarButton;
                        contact.Visibility = Visibility.Visible;
                        addfriend.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }
    }
}

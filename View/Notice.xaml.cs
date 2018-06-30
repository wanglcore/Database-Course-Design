using APP.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
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
    public sealed partial class Notice : Page
    {
        ObservableCollection<ArtComment> artComments;
        ObservableCollection<Comment> comments;
        People people = HomePage.Current.people;
        public Notice()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if(e.Parameter is int)
            {
                RootPivot.SelectedIndex = (int)e.Parameter;
            }
            await GetComment(people.UserId);
            await GetComment2(people.UserId);
        }
        private async Task GetComment(int Userid)
        {
            HttpClient httpClient = new HttpClient
            {
                BaseAddress = new System.Uri("http://localhost:60671/")
            };
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            string str = $"api/artcmt/getcomment2/{Userid}";
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(str);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var res = await httpResponseMessage.Content.ReadAsAsync<string>();
                artComments = new ObservableCollection<ArtComment>(JsonConvert.DeserializeObject<List<ArtComment>>(res));
                Articallist.ItemsSource = artComments;
                this.Bindings.Update();
            }

        }
        private async Task GetComment2(int Userid)
        {
            HttpClient httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:60671/")
            };
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            string str = $"api/comment/getcomment2/{Userid}";
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(str);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var res = await httpResponseMessage.Content.ReadAsAsync<string>();
                if (res != string.Empty)
                {
                    comments = new ObservableCollection<Comment>(JsonConvert.DeserializeObject<List<Comment>>(res));
                    Answerlist.ItemsSource = comments;
                    this.Bindings.Update();
                   
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
            else
            {
                Frame.Navigate(typeof(Blank));
            }
        }

        private void CmdUseridName_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            int tag = Convert.ToInt32(button.Tag);
            Frame.Navigate(typeof(UUserPage), tag);
        }

        private void Articallist_ItemClick(object sender, ItemClickEventArgs e)
        {
            ArtComment artComment = (ArtComment)e.ClickedItem;
            Frame.Navigate(typeof(ArticalCmt), artComment);
        }

        private void Answerlist_ItemClick(object sender, ItemClickEventArgs e)
        {
            Comment answerCmt = (Comment)e.ClickedItem;
            Frame.Navigate(typeof(AnswerCmt), answerCmt);
        }
    }
}

using APP.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace APP.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SearchlistArtical : Page
    {
        List<Artical> articals;
        public static SearchlistArtical Current;
        public SearchlistArtical()
        {
            this.InitializeComponent();
            Current = this;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is List<Artical>)
            {
                articals = (List<Artical>)e.Parameter;
            }
            else if (e.Parameter is Tuple<string, List<Artical>> tuple)
            {

                AutoSuggest.Text = tuple.Item1;
                articals = tuple.Item2;

            }
            else
            {
                await Getallartical();

            }
        }
        private async Task Getallartical()
        {
            HttpClient httpClient = new HttpClient
            {
                BaseAddress = new System.Uri("http://localhost:60671/")
            };
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            string str = "api/artical/getallartical";
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(str);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var listlabel = await httpResponseMessage.Content.ReadAsAsync<string>();
                if (listlabel != string.Empty)
                {
                    articals = (JsonConvert.DeserializeObject<List<Model.Artical>>(listlabel));
                    ArticalList.ItemsSource = articals;
                }
            }
        }

        private void ArticalList_ItemClick(object sender, ItemClickEventArgs e)
        {
            Artical artical = (Artical)e.ClickedItem;
            HomePage.Current.ContentViewFrame.Navigate(typeof(ArticalShow), artical);
        }

        private void Label0_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string label = button.Content.ToString();
            HomePage.Current.ContentViewFrame.Navigate(typeof(LabelDetail), label);
        }

        private async void AutoSuggest_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (sender.Text.Length >= 1)
            {
                await GetArtical(sender.Text);
            }
            else
            {
                sender.PlaceholderText = "查询文章";
                ArticalList.ItemsSource = null;
            }
        }

        private async Task GetArtical(string text)
        {
            HttpClient httpClient = new HttpClient
            {
                BaseAddress = new System.Uri("http://localhost:60671/")
            };
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            string sql = $"api/artical/getbyname/{text}";
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(sql);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                articals = await httpResponseMessage.Content.ReadAsAsync<List<Artical>>();
                if (articals != null)
                {
                    ArticalList.ItemsSource = articals;
                }
            }
        }

        private async void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            await Getallartical();
        }

        private void EditArtical_Click(object sender, RoutedEventArgs e)
        {
            HomePage.Current.ContentViewFrame.Navigate(typeof(EditArtical), HomePage.Current.myStruct);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            HomePage.Current.ContentViewFrame.Navigate(typeof(UUserPage), Convert.ToInt32(button.Tag));
        }
    }
}

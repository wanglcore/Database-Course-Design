using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Net.Http;
using Newtonsoft.Json;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using APP.Model;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace APP.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SearchlistUser : Page
    {
        List<People> peoples;
        string SearchText;
        public SearchlistUser()
        {
            InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if(e.Parameter is List<People>)
            {
                peoples = (List<People>)e.Parameter;
            }
            else if(e.Parameter is Tuple<string, List<People>> tuple)
            {
                AutoSuggest.Text = tuple.Item1;
                peoples = tuple.Item2;
            }
        }
        private async Task GetAllUser(string Text)
        {
            HttpClient httpClient = new HttpClient
            {
                BaseAddress = new System.Uri("http://localhost:60671/")
            };

            
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            string str = $"api/values/getalluser/{NameMap.ChangeMap(Text)}";
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(str);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var res =await httpResponseMessage.Content.ReadAsAsync<string>();
                if (res != string.Empty)
                {
                    peoples = (JsonConvert.DeserializeObject<List<People>>(res));
                }
            }
        }

        private async void AutoSuggest_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (sender.Text.Length >= 1)
            {
                await GetPeople(sender.Text);
            }
            else
            {
                sender.PlaceholderText = "查询用户";
                Peoplelist.ItemsSource = null;
            }
        }

        private async Task GetPeople(string text)
        {
            HttpClient httpClient = new HttpClient
            {
                BaseAddress = new System.Uri("http://localhost:60671/")
            };
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            string sql = $"api/values/getbyname/{NameMap.ChangeMap(text)}";
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(sql);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                peoples = await httpResponseMessage.Content.ReadAsAsync<List<People>>();
                if (peoples != null)
                {
                    Peoplelist.ItemsSource = peoples;
                }
            }
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            People people = (People)e.ClickedItem;
            this.Frame.Navigate(typeof(UUserPage), people);
        }
    }
}

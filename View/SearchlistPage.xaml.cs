using APP.Model;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace APP.View
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SearchlistPage : Page
    {
        List<Qusition> qusitions;
        MyStruct myStruct=HomePage.Current.myStruct;
        List<Artical> articals;
        List<People> peoples;
        List<Label> labels;
        List<Label> folabels = new List<Label>();
        public SearchlistPage()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }
        private void Searchlist_ItemClick(object sender, ItemClickEventArgs e)
        {
            var clickeditem = (Qusition)e.ClickedItem;
            HomePage.Current.ContentViewFrame.Navigate(typeof(AnswerQusition), clickeditem);
        }

        private void Label0_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string labelname = button.Content.ToString();
            HomePage.Current.ContentViewFrame.Navigate(typeof(LabelDetail), labelname);
        }

        private async void AutoSearch_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (sender.Text.Length >= 1)
            {
                string text = NameMap.ChangeMap(sender.Text);
                await GetLabel(text);
                await GetArtical(text);
                await GetQusiton(text);
                await GetPeople(text);
            }
            else
            {
                sender.PlaceholderText = "输入内容";
            }
        }

        private async Task GetLabel(string text)
        {
            HttpClient httpClient = new HttpClient
            {
                BaseAddress = new System.Uri("http://localhost:60671/")
            };
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            string sql = $"api/label/getbyname/{text}";
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(sql);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                labels =await httpResponseMessage.Content.ReadAsAsync<List<Label>>();
                if (labels != null)
                {
                    Labellistframe.Visibility = Visibility.Visible;
                }
                else
                {
                    Labellistframe.Visibility = Visibility.Collapsed;

                }
                Labellist.ItemsSource = labels;
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
                    Articallistframe.Visibility = Visibility.Visible;
                }
                else
                {
                    Articallistframe.Visibility = Visibility.Collapsed;

                }
                ArticalList.ItemsSource = articals;
            }
        }

        private async Task GetQusiton(string text)
        {
            HttpClient httpClient = new HttpClient
            {
                BaseAddress = new System.Uri("http://localhost:60671/")
            };
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            string sql = $"api/qusition/getbyname/{text}";
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(sql);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                qusitions = await httpResponseMessage.Content.ReadAsAsync<List<Qusition>>();
                if (qusitions != null)
                {
                    Searchlistframe.Visibility = Visibility.Visible;
                }
                else
                {
                    Searchlistframe.Visibility = Visibility.Collapsed;

                }
                Searchlist.ItemsSource = qusitions;
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
            string sql = $"api/values/getbyname/{text}";
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(sql);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                peoples = await httpResponseMessage.Content.ReadAsAsync<List<People>>();
                if (peoples != null)
                {
                    Peoplelistframe.Visibility = Visibility.Visible;
                }
                else
                {
                    Peoplelistframe.Visibility = Visibility.Collapsed;

                }
                Peoplelist.ItemsSource = peoples;
            }
        }
        private void Labellist_ItemClick(object sender, ItemClickEventArgs e)
        {
            Label label = (Label)e.ClickedItem;
            HomePage.Current.ContentViewFrame.Navigate(typeof(LabelDetail), label);
        }

        private void ArticalList_ItemClick(object sender, ItemClickEventArgs e)
        {
            Artical clickItem = (Artical)e.ClickedItem;
            HomePage.Current.ContentViewFrame.Navigate(typeof(ArticalShow), clickItem);
        }

        private void Selectitem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Searchlistframe.MaxHeight = ActualHeight / 4;
            Articallistframe.MaxHeight = ActualHeight / 4;
            Peoplelistframe.MaxHeight = ActualHeight / 4;
            Labellistframe.MaxHeight = ActualHeight / 4;
        }

        private void Readqusition_Click(object sender, RoutedEventArgs e)
        {
            Tuple<string,List<Qusition> >tuple= new Tuple<string, List<Qusition>>(AutoSearch.Text,qusitions);
            HomePage.Current.ContentViewFrame.Navigate(typeof(Qusitionviewpage), tuple);
        }

        private void ReadArtical_Click(object sender, RoutedEventArgs e)
        {
            Tuple<string, List<Artical>> tuple = new Tuple<string, List<Artical>>(AutoSearch.Text, articals);
            HomePage.Current.ContentViewFrame.Navigate(typeof(SearchlistArtical), tuple);
        }

        private void ReadmorePeople_Click(object sender, RoutedEventArgs e)
        {
            Tuple<string, List<People>> tuple = new Tuple<string, List<People>>(AutoSearch.Text, peoples);

            HomePage.Current.ContentViewFrame.Navigate(typeof(SearchlistUser), tuple);
        }

        private void ReadLabel_Click(object sender, RoutedEventArgs e)
        {
            Tuple<string, List<Label>> tuple = new Tuple<string, List<Label>>(AutoSearch.Text, labels);

            HomePage.Current.ContentViewFrame.Navigate(typeof(SearchlistPagelabel), tuple);
        }

        private void Peoplelist_ItemClick(object sender, ItemClickEventArgs e)
        {
            People people = (People)e.ClickedItem;
            HomePage.Current.ContentViewFrame.Navigate(typeof(UUserPage), people);
        }
    }
}

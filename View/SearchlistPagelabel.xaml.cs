using APP.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
    public sealed partial class SearchlistPagelabel : Page
    {
        private List<Label> labels;
        public static SearchlistPagelabel Current;
        ObservableCollection<Label> folabels = new ObservableCollection<Label>();
        MyStruct myStruct = HomePage.Current.myStruct;
        string Searchtext;
        public SearchlistPagelabel()
        {
            this.InitializeComponent();
            Current = this;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
          if(e.Parameter is List<Label>)
            {
                labels = (List<Label>)e.Parameter;
                Labellist.ItemsSource = labels;
            }
          else if(e.Parameter is Tuple<string, List<Label>> tuple)
            {
                AutoSuggest.Text = tuple.Item1;
                labels = tuple.Item2;
            }
        }
        /// <summary>
        /// 这个函数没啥用,
        /// </summary>
        /// <returns></returns>
        private async Task GetFlabel()
        {
            HttpClient httpClient = new HttpClient
            {
                BaseAddress = new System.Uri("http://localhost:60671/")
            };
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            string str = "api/label/getlabel";
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(str);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var listlabel = await httpResponseMessage.Content.ReadAsAsync<string>();
                if (listlabel != string.Empty)
                {
                    labels = (JsonConvert.DeserializeObject<List<Model.Label>>(listlabel));
                    Labellist.ItemsSource = labels;
                }
            }
        }
  
        private async void Selectitem_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            int lid = Convert.ToInt32(button.Tag);
            await PutFLabel(lid);
        }
        private async void DFButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            int lid = Convert.ToInt32(button.Tag);
            await PutUFLabel(lid);
        }

        private void Labellist_ItemClick(object sender, ItemClickEventArgs e)
        {
            Label label = (Label)e.ClickedItem;
            HomePage.Current.ContentViewFrame.Navigate(typeof(LabelDetail), label);
        }

        private async Task PutFLabel(int labelid)
        {
            int Userid = myStruct.id;
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            
            var content = new StringContent("", Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await httpClient.PutAsync($"http://localhost:60671/api/label/flabel/{Userid}/{labelid}", content);
            var res = await httpResponseMessage.Content.ReadAsAsync<bool>();
            Model.Label label;
            if (res == true)
            {
                label = labels.Where(p => p.Labelid == labelid).FirstOrDefault();
                folabels.Add(label);
                labels.Remove(label);
                Haveselect.ItemsSource = folabels;
            }

        }

        private async Task PutUFLabel(int labelid)
        {
            int Userid = myStruct.id;
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            string str = $"api/label/flabel/{Userid}/{labelid}";
            var content = new StringContent("", Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await httpClient.PutAsync($"http://localhost:60671/api/label/uflabel/{Userid}/{labelid}", content);
            var res = await httpResponseMessage.Content.ReadAsAsync<bool>();
            Model.Label label;
            if (res == true)
            {
                label = folabels.Where(p => p.Labelid == labelid).FirstOrDefault();
                folabels.Remove(label);
                labels.Add(label);
                Haveselect.ItemsSource = folabels;
            }

        }

        private async void AutoSuggest_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (sender.Text.Length >= 1)
            {
                await GetLabel(sender.Text);
            }
            else
            {
                sender.PlaceholderText = "查询标签";
                Labellist.ItemsSource = null;
            }
        }


        private async Task GetLabel(string text)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new System.Uri("http://localhost:60671/");
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            string sql = $"api/label/getbyname/{text}";
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(sql);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                labels = await httpResponseMessage.Content.ReadAsAsync<List<Label>>();
                if (labels != null)
                {
                  
                    Labellist.ItemsSource = labels;
                }
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }
    }
}

using APP.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace APP.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LabelDetailP : Page
    {
        private ObservableCollection<People> peoples;
        Label label;
        int userid;
        public LabelDetailP()
        {
            InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is Label)
            {
                label = (Label)e.Parameter; 
                await GetLabelP(label);
            }
        }

        private void Followedpeople_ItemClick(object sender, ItemClickEventArgs e)
        {
            var clickeditem = (People)e.ClickedItem;
            Frame.Navigate(typeof(UUserPage),clickeditem);
        }

        private async Task GetLabelP(Label label)
        {
            HttpClient httpClient = new HttpClient
            {
                BaseAddress = new System.Uri("http://localhost:60671/")
            };
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            string str = $"api/values/labelp/{label.Labelid}/{label.Labelname}";
            HttpResponseMessage httpResponseMessage =await httpClient.GetAsync(str);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var content =await httpResponseMessage.Content.ReadAsAsync<string>();
                if (content != string.Empty)
                {
                    peoples = new ObservableCollection<People>(JsonConvert.DeserializeObject<List<People>>(content));
                    Followedpeople.ItemsSource = peoples;
                }
            }

        }

        private void CancelButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}

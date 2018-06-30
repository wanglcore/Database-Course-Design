using APP.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace APP.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SomeoneArtical : Page
    {
        People people;
        ObservableCollection<Artical> articals;
        public SomeoneArtical()
        {
            this.InitializeComponent();
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(e.Parameter is People)
            {
                people = (People)e.Parameter;
                await GetArtical(people);
                if (people.UserId == HomePage.Current.myStruct.id)
                {
                    IEnumerable<AppBarButton> appBars =( NameMap.GetChildObjects<AppBarButton>(Articallist));
                    List<AppBarButton> apps = appBars.Where(p => p.Name.ToLower() == "Edit").ToList();
                    apps.ForEach(p => p.Visibility = Visibility.Visible);

                }
            }
            base.OnNavigatedTo(e);
        }

        private async Task GetArtical(People people)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new System.Uri("http://localhost:60671/");
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            string str = $"api/artical/getartical/{people.UserId}";
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(str);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var res =await httpResponseMessage.Content.ReadAsAsync<string>();
                if (res != string.Empty)
                {
                    articals = new ObservableCollection<Artical>(JsonConvert.DeserializeObject<List<Artical>>(res));
                    Articallist.ItemsSource = articals;

                }
            }
        }

        private void Label0_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string labelname = button.Content.ToString();
            Frame.Navigate(typeof(LabelDetail), labelname);
        }

        private void Articallist_ItemClick(object sender, ItemClickEventArgs e)
        {
            Artical artical = (Artical)e.ClickedItem;
            Frame.Navigate(typeof(ArticalShow), artical);
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

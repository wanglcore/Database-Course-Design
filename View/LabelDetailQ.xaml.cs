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
    public sealed partial class LabelDetailQ : Page
    {
        public ObservableCollection<Qusition> unqusitions;
        MyStruct myStruct=HomePage.Current.myStruct;
        Label label;
        public LabelDetailQ()
        {
            this.InitializeComponent();
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is Label)
            {
                label = (Label)e.Parameter;
            }
            await GetlabelQ(label);
        }
        private void Unasnwerid_ItemClick(object sender, ItemClickEventArgs e)
        {
            var clickeditem = (Qusition)e.ClickedItem;
            HomePage.Current.ContentViewFrame.Navigate(typeof(AnswerQusition), clickeditem);
        }
        /// <summary>
        /// 得到一个标签下的未回答的问题
        /// </summary>
        /// <param name="labelid"></param>
        /// <returns></returns>
        private async Task GetlabelQ(Label label)
        {
            HttpClient httpClient = new HttpClient
            {
                BaseAddress = new System.Uri("http://localhost:60671/")
            };
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            string str = $"api/qusition/labelUQ/{label.Labelid}/{label.Labelname}";
            HttpResponseMessage httpResponseMessage =await httpClient.GetAsync(str);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var content = await httpResponseMessage.Content.ReadAsAsync<string>();
                if (content != string.Empty)
                {
                    unqusitions = new ObservableCollection<Qusition>(JsonConvert.DeserializeObject<List<Qusition>>(content));
                    Unasnwerid.ItemsSource = unqusitions;
                }
            }
        }

        
    }
}

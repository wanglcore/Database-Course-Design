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
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using APP.Model;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Text;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace APP.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class someonelabel : Page
    {
        People people;
        ObservableCollection<Label> labels;
        public someonelabel()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(e.Parameter is People)
            {
                people = (People)e.Parameter;
                await Getlabel(people);
            }
            base.OnNavigatedTo(e);
        }

        private void Labellist_ItemClick(object sender, ItemClickEventArgs e)
        {
            Label label = (Label)e.ClickedItem;
            Frame.Navigate(typeof(LabelDetail), label);
        }

        private async Task Getlabel(People people)
        {
            HttpClient httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:60671/")
            };
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            string sql = $"api/label/getflabel/{people.UserId}";
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(sql);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var res =await httpResponseMessage.Content.ReadAsAsync<string>();
                if (res != string.Empty)
                {
                    labels = new ObservableCollection<Label>(JsonConvert.DeserializeObject<List<Label>>(res));
                    Haveselect.ItemsSource = labels;
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        private async void DFButton_Click(object sender, RoutedEventArgs e)
        {

            Button button = (Button)sender;
            await DelPutFollow(people.UserId, Convert.ToInt32(button.Tag));
        }

        private async Task DelPutFollow(int Userid, int labelid)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var content = new StringContent("", Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await httpClient.PutAsync($"http://localhost:60671/api/label/uflabel/{Userid}/{labelid}", content);
            var res = await httpResponseMessage.Content.ReadAsAsync<bool>();
            if (res == true)
            {

                var temp = labels.Where(p => p.Labelid == labelid).FirstOrDefault();
                if (temp != null)
                {
                    labels.Remove(temp);
                }
            }


        }

    }
}

using APP.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace APP.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UserFollowQ : Page
    {
        List<GetUserFollow> getUserFollows;
        MyStruct myStruct;
        public UserFollowQ()
        {
            this.InitializeComponent();
        }
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            myStruct = (MyStruct)e.Parameter;
            await GetFollowQ(myStruct.id);
        }
        private async Task GetFollowQ(int Userid)
        {
            HttpClient httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:60671/")
            };
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            string str = $"api/qusition/getfollow/{Userid}";
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(str);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var success = await httpResponseMessage.Content.ReadAsAsync<string>();
                if (success != string.Empty)
                {
                    getUserFollows = JsonConvert.DeserializeObject<List<GetUserFollow>>(success);
                }
                else
                {

                }
            }
        }

        private void UserFollowQusition_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void Follow_Click(object sender, RoutedEventArgs e)
        {
            Button button = e.OriginalSource as Button;
            string s = button.DataContext.ToString();
        }

        private void Answer_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ItemListview_ItemClick(object sender, ItemClickEventArgs e)
        {

        }
    }
}

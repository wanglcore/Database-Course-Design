using APP.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public sealed partial class Fsomeone : Page
    {
        ObservableCollection<MyFollowUser> followpeople;
        People people;
        public Fsomeone()
        {
            this.InitializeComponent();
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(e.Parameter is People)
            {
                people = (People)e.Parameter;
                await Getfollowmw(people);
            }
            base.OnNavigatedTo(e);
        }

        private async Task Getfollowmw(People people)
        {
            HttpClient httpClient = new HttpClient
            {
                BaseAddress = new System.Uri("http://localhost:60671/")
            };
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            string str = $"api/values/getfollowme/{people.UserId}";
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(str);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var listlabel = await httpResponseMessage.Content.ReadAsAsync<string>();
                if (listlabel != string.Empty)
                {
                    followpeople = new ObservableCollection<MyFollowUser>(JsonConvert.DeserializeObject<List<Model.MyFollowUser>>(listlabel));
                    Followme.ItemsSource = followpeople;
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private void Followme_ItemClick(object sender, ItemClickEventArgs e)
        {
            MyFollowUser people = (MyFollowUser)e.ClickedItem;
            Frame.Navigate(typeof(UUserPage), people.FollowUserid);
        }
    }
}

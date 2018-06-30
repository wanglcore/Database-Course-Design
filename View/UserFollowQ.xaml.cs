using APP.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace APP.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UserFollowQ : Page
    {
        ObservableCollection<Qusition> qusitions;
        MyStruct myStruct=(HomePage.Current.myStruct);
        People people;
        public UserFollowQ()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if(e.Parameter is People)
            {
                people = (People)e.Parameter;
                await GetFollowQ(people.UserId);
            }
        }
       
        /// <summary>
        /// 点击问题视图中的某一项,进入该问题的详细信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserFollowQusition_ItemClick(object sender, ItemClickEventArgs e)
        {
            var clickeditem = (Qusition)e.ClickedItem;
            Frame.Navigate(typeof(AnswerQusition), clickeditem);
        }
        /// <summary>
        /// 得到一个人关注的问题
        /// </summary>
        /// <param name="Userid"></param>
        /// <returns></returns>
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
                    qusitions = new ObservableCollection<Qusition>(JsonConvert.DeserializeObject<List<Qusition>>(success));
                    Searchlist.ItemsSource = qusitions;
                }
            }
        }
  
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if(Frame.CanGoBack)
            Frame.GoBack();
        }

        private void Label0_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string labelname = button.Content.ToString();
            Frame.Navigate(typeof(LabelDetail), labelname);
        }

        private void Searchlist_ItemClick(object sender, ItemClickEventArgs e)
        {
            Qusition qusition = (Qusition)e.ClickedItem;
            Frame.Navigate(typeof(AnswerQusition), qusition);
        }
    }
}

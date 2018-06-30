using APP.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace APP.View
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class HomePage : Page
    {
        public static HomePage Current;
        private List<Qusition> qusitions;
        public MyStruct myStruct;
        public People people;
        public HomePage()
        {
            this.InitializeComponent();
            Current = this;
        }
        /// <summary>
        /// 接受从登陆界面的传值
        /// </summary>
        /// <param name="e"></param>
        /// 
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is MyStruct)
            {
                myStruct = (MyStruct)e.Parameter;
                await GetUserQusition(myStruct);
                await Getinfo(myStruct.id);
            }
            base.OnNavigatedTo(e);
        }

        /// <summary>
        /// 导航栏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void Nvall_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                //TODO设置页
                Homesuggestion.Navigate(typeof(SettingPage), myStruct);
            }
            else
            {
                switch (args.InvokedItem)
                {
                    case "Account":
                        ContentViewFrame.Navigate(typeof(MyInfor), myStruct);
                        break;
                    case "Home":                        
                        this.Frame.Navigate(typeof(HomePage), myStruct);
                        break;
                    case "Qusition":
                        ContentViewFrame.Navigate(typeof(MakeQusition), myStruct);
                        break;
                    case "Search":
                        Homesuggestion.Navigate(typeof(SearchlistPage), myStruct);
                        break;
                    case "Answer":
                        Homesuggestion.Navigate(typeof(Qusitionviewpage), myStruct);
                        break;
                    case "Artical":
                        Homesuggestion.Navigate(typeof(SearchlistArtical));
                        break;
                    case "Draft":
                        ContentViewFrame.Navigate(typeof(MyDraft));
                        break;
                    case "Notice":
                        ContentViewFrame.Navigate(typeof(Notice));
                        break;
                    default:
                        break;
                }
            }
        }
        private void ItemListview_ItemClick(object sender, ItemClickEventArgs e)
        {
            var clickeditem = (Qusition)e.ClickedItem;
            ContentViewFrame.Navigate(typeof(AnswerQusition), clickeditem);
        }
        /// <summary>
        /// 搜索按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Homesuggestion.Navigate(typeof(SearchlistPage));
        }
        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Refrash_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await GetUserQusition(myStruct);
        }

        private async Task GetUserQusition(MyStruct myStruct)
        {
            HttpClient httpClient = new HttpClient
            {
                BaseAddress = new System.Uri("http://localhost:60671/")
            };
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            string str = $"api/Qusition/allqusition/{myStruct.id}";
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(str);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var succstring = await httpResponseMessage.Content.ReadAsAsync<string>();
                if (succstring != string.Empty)
                {
                    qusitions = JsonConvert.DeserializeObject<List<Qusition>>(succstring);
                    itemListview.ItemsSource = qusitions;
                }
            }
        }

        private async Task Getinfo(int Userid)
        {
            HttpClient httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:60671/")
            };
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            string str = $"api/values/userinfor/{Userid}";
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(str);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var success = await httpResponseMessage.Content.ReadAsAsync<People>();
                if (success != null)
                {
                    people = success;
                }
            }
        }

        private void Label0_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string labelname = button.Content.ToString();
            ContentViewFrame.Navigate(typeof(LabelDetail), labelname);
        }

        private void itemListview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}

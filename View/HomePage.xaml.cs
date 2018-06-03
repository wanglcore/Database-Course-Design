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
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace APP.View
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class HomePage : Page
    {
        public static HomePage Current;
        List<Qusition> qusitions;
        MyStruct myStruct;
        public string UserEmail;
        public string Userid;
        public HomePage()
        {
            this.InitializeComponent();
            Current = this;
        }
        /// <summary>
        /// 接受从登陆界面的传值
        /// </summary>
        /// <param name="e"></param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            myStruct = (MyStruct)e.Parameter;
            await GetUserQusition(myStruct);
        }

        private void Autobox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            string text = args.QueryText;
            if (args.ChosenSuggestion != null)
            {

            }
            else
            {
                //TODO根据用户的输入进行查询,此处更新数据库的查询
                Suggestionlist.Navigate(typeof(View.SearchlistPage));
            }
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

                //TODO实现
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
                    default:
                        break;
                }
            }
        }
        private void ItemListview_ItemClick(object sender, ItemClickEventArgs e)
        {
            var clickeditem = (Qusition)e.ClickedItem;

            Dictionary<string, string> keyValues = new Dictionary<string, string>
            {
                { "QusitionTitle", clickeditem.QusitionTitle },
                { "QusitionContent", clickeditem.QusitionContent },
                { "Follownum", clickeditem.Followednum.ToString() },
                { "Answernum", clickeditem.Answerednum.ToString() },
                { "Qusitionid", clickeditem.Qusitionid.ToString() },
                { "Userid", myStruct.id.ToString() }
            };
            ContentViewFrame.Navigate(typeof(AnswerQusition), keyValues);
        }
        private void HomeFrameView_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            double width = Windows.UI.Xaml.Window.Current.Bounds.Width;
            double height = Windows.UI.Xaml.Window.Current.Bounds.Height;
            if (height > width)
            {
                Homesuggestion.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
            else
            {
                Homesuggestion.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
        }

        private void VisualStateGroup_CurrentStateChanged(object sender, Windows.UI.Xaml.VisualStateChangedEventArgs e)
        {

        }

        /// <summary>
        /// 搜索按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Homesuggestion.Navigate(typeof(SearchlistPage), myStruct);
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

        public async Task GetUserQusition(MyStruct myStruct)
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

        private void Follow_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

        }

        private void More_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

        }
    }
}

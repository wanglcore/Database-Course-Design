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

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace APP.View
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SearchlistPage : Page
    {
        List<Qusition> qusitions;
        MyStruct myStruct;
        public SearchlistPage()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            myStruct = (MyStruct)e.Parameter;
        }
        private void Searchlist_ItemClick(object sender, ItemClickEventArgs e)
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
            HomePage.Current.ContentViewFrame.Navigate(typeof(AnswerQusition), keyValues);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void Qusition_Click(object sender, RoutedEventArgs e)
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
                    Searchlist.ItemsSource = qusitions;
                }
            }
        }
    }
}

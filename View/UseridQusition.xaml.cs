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
    public sealed partial class UseridQusition : Page
    {
        MyStruct myStruct;
        People people;
        List<Qusition> qusitions=new List<Qusition>();
        public UseridQusition()
        {
            this.InitializeComponent();
        }
        private void UserQusition_ItemClick(object sender, ItemClickEventArgs e)
        {
            Qusition qusition = (Qusition)e.ClickedItem;
            Frame.Navigate(typeof(AnswerQusition), qusition);
        }



        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
           if(e.Parameter is People)
            {
                people = (People)e.Parameter;
                await GetUserQusition(people);
            }
            base.OnNavigatedTo(e);
        }
        public async Task GetUserQusition(People people)
        {
            HttpClient httpClient = new HttpClient
            {
                BaseAddress = new System.Uri("http://localhost:60671/")
            };
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            string str = $"api/qusition/userqusition/{people.UserId}";
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(str);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var succstring = await httpResponseMessage.Content.ReadAsAsync<string>();
                if (succstring != string.Empty)
                {
                    qusitions = JsonConvert.DeserializeObject<List<Qusition>>(succstring);
                    UserQusition.ItemsSource = qusitions;
                }
            }
        }
        private void Label0_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string labelname = button.Content.ToString();
            this.Frame.Navigate(typeof(LabelDetail), labelname);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}

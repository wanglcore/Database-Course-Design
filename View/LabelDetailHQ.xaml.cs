using APP.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace APP.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LabelDetailHQ : Page
    {
        ObservableCollection<Qusition> qusitions;
        MyStruct myStruct = HomePage.Current.myStruct;
        Label label;
        public LabelDetailHQ()
        {
            this.InitializeComponent();
            
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            
            if(e.Parameter is Label)
            {
                label = (Label)e.Parameter;
                await HaveAnswered(label);
            }
        }
        /// <summary>
        /// 点击列表中的某一项触发该事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Haveanswerid_ItemClick(object sender, ItemClickEventArgs e)
        {
            var clickeditem = (Qusition)e.ClickedItem;
            HomePage.Current.ContentViewFrame.Navigate(typeof(AnswerQusition), clickeditem);
        }
        /// <summary>
        /// 得到一个标签下已经回答的问题
        /// </summary>
        /// <param name="labelid"></param>
        /// <returns></returns>
        private async Task HaveAnswered(Label label)
        {
            HttpClient httpClient = new HttpClient
            {
                BaseAddress = new System.Uri("http://localhost:60671/")
            };
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            string str = $"api/qusition/labelHQ/{label.Labelid}/{label.Labelname}";
            HttpResponseMessage httpResponseMessage =await httpClient.GetAsync(str);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var content = await httpResponseMessage.Content.ReadAsAsync<string>();
                qusitions =new ObservableCollection<Qusition>(JsonConvert.DeserializeObject<List<Qusition>>(content));
                Haveanswerid.ItemsSource = qusitions;
            }
        }
        /// <summary>
        /// 查看答案的细节
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Articaldetail_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

        }

        private void Label0_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string labelname = button.Content.ToString();
            this.Frame.Navigate(typeof(LabelDetail), labelname);
        }
    }
}

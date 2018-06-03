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
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace APP.View
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MakeQusition : Page
    {
        MyStruct myStruct;
        public MakeQusition()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 选择标签按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectLabels_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SelectLabels));

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            myStruct = (MyStruct)e.Parameter;

        }
        /// <summary>
        /// 提交提问
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Submit_Click(object sender, RoutedEventArgs e)
        {
            Model.Qusition qusition = new Model.Qusition
            {
                Askerid = myStruct.id,
                AskTime = DateTime.Now,
                UpAskTime = DateTime.Now,
                QusitionTitle = QusitionDescribe.Text,
                QusitionContent = Qusitiondetail.Text,
                Qlabel0 = "Math",
                Qlabel1 = "d",
                QLabel2 = "c"
            };
            await SubQusition(qusition);
        }
        public async Task SubQusition(Model.Qusition qusition)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var content = new StringContent(JsonConvert.SerializeObject(qusition), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("http://localhost:60671/api/qusition/AddQusition", content);
            var res = await response.Content.ReadAsAsync<bool>();
            if (res == true)
            {
                issucc.Text = "Success";
            }
            else
            {
                issucc.Text = "Failure";
            }
        }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
        string QusitionTitle=string.Empty;
        string QusitiondETAILE=string.Empty;
        ObservableCollection<Model.Label> labels=new ObservableCollection<Model.Label>();
        public MakeQusition()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 选择标签按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectLabels_Click(object sender, RoutedEventArgs e)
        {
            QusitionTitle = QusitionDescribe.Text;
            QusitiondETAILE = Qusitiondetail.Text;
            Frame.Navigate(typeof(SelectLabels),0);
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is MyStruct)
            {
                myStruct = (MyStruct)e.Parameter;
            }
            else if(e.Parameter is ObservableCollection<Model.Label>)
            {
                labels = (ObservableCollection<Model.Label>)e.Parameter;
                labelview.ItemsSource = labels;
            }
            QusitionDescribe.Text = QusitionTitle;
            Qusitiondetail.Text = QusitiondETAILE;
        }
        /// <summary>
        /// 提交提问
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Submit_Click(object sender, RoutedEventArgs e)
        {
            if (Qusitiondetail.Text == "")
            {
                Qusitiondetail.PlaceholderText = "没输入内容";
                Qusitiondetail.PlaceholderForeground = new SolidColorBrush(Colors.Red);
            }
            else if (QusitionDescribe.Text == "")
            {
                QusitionDescribe.PlaceholderForeground = new SolidColorBrush(Colors.Red);
                QusitionDescribe.PlaceholderText = "没输入内容";
            }
            else if (labels.Count == 0)
            {
                await ContentD();
            }
            if (QusitionDescribe.Text != "" && Qusitiondetail.Text != "" && labels.Count >= 1)
            {
                Model.Qusition qusition = new Model.Qusition
                {
                    Askerid = myStruct.id,
                    AskTime = DateTime.Now,
                    UpAskTime = DateTime.Now,
                    QusitionTitle = QusitionDescribe.Text,
                    QusitionContent = Qusitiondetail.Text,
                    Qlabel0 = labels[0].Labelname,

                };
                if (labels.Count == 1)
                {
                    qusition.Qlabel1 = "";
                    qusition.QLabel2 = "";
                }
                if (labels.Count == 2)
                {
                    qusition.Qlabel1 = labels[1].Labelname;
                    qusition.QLabel2 = "";
                }
                if (labels.Count == 3)
                {
                    qusition.Qlabel1 = labels[1].Labelname;
                    qusition.QLabel2 = labels[2].Labelname;
                }
                await SubQusition(qusition);
            }
        }

        private async Task ContentD()
        {
            ContentDialog contentDialog = new ContentDialog();
            contentDialog.Title = "你没有选择文章的标签,请选择";
            contentDialog.PrimaryButtonText = "去选择";
            contentDialog.PrimaryButtonClick += (_s, _e) =>
            {
                this.Frame.Navigate(typeof(SelectLabels), 0);
            };
            await contentDialog.ShowAsync();
        }
        private async Task SubQusition(Model.Qusition qusition)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var content = new StringContent(JsonConvert.SerializeObject(qusition), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("http://localhost:60671/api/qusition/AddQusition", content);
            var res = await response.Content.ReadAsAsync<bool>();
            if (res == true)
            {
                AcceptButton.IsEnabled = false;
            }
        }

        private void CancelFButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }
    }
}

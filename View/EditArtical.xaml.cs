using System;
using APP.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Storage;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage.Pickers;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace APP.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditArtical : Page
    {
        MyStruct myStruct=HomePage.Current.myStruct;
        ObservableCollection<Model.Label> labels;
        int FromPage = 0;
        public EditArtical()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if(e.Parameter is MyStruct)
            {
                myStruct = (MyStruct)e.Parameter;
            }
            
            if(e.Parameter is ObservableCollection<Model.Label>)
            {
                labels = (ObservableCollection<Model.Label>)e.Parameter;
                labelview.ItemsSource = labels;
            }
            if(e.Parameter is Draft draft)
            {
                FromPage = 1;
                ArticalContent.Text = draft.Content;
                ArticalTitle.Text = draft.Title;
            }
        }
        private void SelectLabel_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SelectLabels),1);
        }

        private async void SendArtical_Click(object sender, RoutedEventArgs e)
        {
            string s = string.Empty;
            if (ArticalTitle.Text.ToString() == "")
            {
                ArticalTitle.PlaceholderText = "文章标题不能为空";
                ArticalTitle.PlaceholderForeground = new SolidColorBrush(Colors.Red);
            }
            else
            {
                if (ArticalContent.Text.ToString() == "")
                {
                    ArticalContent.PlaceholderText = "文章内容不能为空";
                    ArticalContent.PlaceholderForeground = new SolidColorBrush(Colors.Red);
                }
                else
                {
                    if (labelview.Items.Count == 0)
                    {
                        await ContentD();
                    }
                    else
                    {
                        await ContentDialogshow();
                    }
                }
            }

        }
        private async Task PostArtical(Artical artical)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var content = new StringContent(JsonConvert.SerializeObject(artical), Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await httpClient.PostAsync("http://localhost:60671/api/artical/newartical", content);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var res = await httpResponseMessage.Content.ReadAsAsync<bool>();
                if (res == true)
                {
                    SendArtical.IsEnabled = false;
                }
            }
        }
       
        private async Task ContentD()
        {

            ContentDialog contentDialog = new ContentDialog();
                contentDialog.Title = "你没有选择文章的标签,请选择";
                contentDialog.PrimaryButtonText = "去选择";
                contentDialog.PrimaryButtonClick += (_s, _e) =>
                {
                    this.Frame.Navigate(typeof(SelectLabels), 1);
                };
            await contentDialog.ShowAsync();
        }
        private async Task ContentDialogshow()
        {
            ContentDialog contentDialog = new ContentDialog();

            contentDialog.Title = "确认提交??";
                contentDialog.PrimaryButtonText = "提交";
                contentDialog.SecondaryButtonText = "不提交";
            contentDialog.PrimaryButtonClick += async (_s, _e) =>
            {
                string articaltext = ArticalTitle.Text;
                string content = ArticalContent.Text;
                var gridview = (Label)labelview.Items[0];
                string label = gridview.Labelname;
                Artical artical = new Artical
                {
                    Userid = myStruct.id,
                    UseridName = HomePage.Current.people.Name,
                    ArticalTitle = articaltext,
                    ArticalContent = content,
                    Label0=label,
                };

                artical.ArticalTime = artical.ArticalUPTime = DateTime.Now;
                await PostArtical(artical);
            };
            contentDialog.SecondaryButtonClick += (_s, _e) =>
            {

            };
            await contentDialog.ShowAsync();
        }

        private async Task ContentDialogs()
        {
            ContentDialog contentDialog = new ContentDialog
            {
                Title = "未提交,是否退出",
                PrimaryButtonText = "保存并退出",
                SecondaryButtonText = "直接退出"
            };
            contentDialog.PrimaryButtonClick += (_s, _e) =>
            {
                Save.Click += Save_Click;
            };
            contentDialog.SecondaryButtonClick += (_s, _e) =>
            {
                this.Frame.Navigate(typeof(Blank));
            };
            await contentDialog.ShowAsync();
        }

        private async void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (SendButton.IsEnabled == true && SendArtical.IsEnabled == true)
            {
                await ContentDialogs();
            }
            else
            {
                if (FromPage == 1)
                {
                    this.Frame.Navigate(typeof(MyDraft));
                }
                else
                {
                    this.Frame.Navigate(typeof(Blank));
                }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            
            string text = ArticalTitle.Text;
            string content = ArticalContent.Text;
            string str = $"A---{DateTime.Now}---{text}---{content}---{myStruct.id}";
            Frame.Navigate(typeof(MyDraft), str);
        }

        private void PickPicure_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

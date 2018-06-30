using APP.Model;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Net.Cache;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace APP.View
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MyInfor : Page
    {
        MyStruct myStruct=HomePage.Current.myStruct;

        People people;

        public MyInfor()
        {
            this.InitializeComponent();
            int userid = HomePage.Current.myStruct.id;

        }
        protected async override  void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await Getinfo(myStruct.id);
        }
        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timage_ImageOpened(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker
            {
                ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail,
                SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary
            };
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".png");
        }

        private void Changeimage_Click(object sender, RoutedEventArgs e)
        {
            Showpicker();
        }

        private async void Showpicker()
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker
            {
                ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail,
                SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary
            };
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");
            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                Windows.Storage.Streams.IRandomAccessStream randomAccessStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                Windows.UI.Xaml.Media.Imaging.BitmapImage bitmap = new Windows.UI.Xaml.Media.Imaging.BitmapImage();
                await bitmap.SetSourceAsync(randomAccessStream);
                Personpictures.ProfilePicture = bitmap;
            }
        }

        private void Myqusition_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(UseridQusition), people);
        }

        private void Mycollection_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Frame.Navigate(typeof(UserFollowQ), people);

            }
            catch (Exception)
            {


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
                    this.Bindings.Update();
                }
            }
        }

        private void Myanswer_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SomeoneAnswerView),people);
        }

        private void Myartical_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SomeoneArtical), people);
        }

        private void Myconqusition_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(someonelabel), people);
        }

        private void Myconpeople_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Somepeople), people);
        }

        private void Concernme_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Fsomeone), people);
        }

        private void EditName_Click(object sender, RoutedEventArgs e)
        {
            DetailName.Visibility = Visibility.Visible;
            DetailName.Height = ActualHeight / 5;
        }

        private void Introedit_Click(object sender, RoutedEventArgs e)
        {
            DetailIntro.Visibility = Visibility.Visible;
            DetailIntro.Height = ActualHeight / 5;
        }

        private async void AcceptN_Click(object sender, RoutedEventArgs e)
        {
            await ContentDia(0);
        }

        private void CancelN_Click(object sender, RoutedEventArgs e)
        {
            DetailName.Visibility = Visibility.Collapsed;
        }

        private async void AcceptI_Click(object sender, RoutedEventArgs e)
        {
            await ContentDia(1);
        }

        private void CancelI_Click(object sender, RoutedEventArgs e)
        {
            DetailIntro.Visibility = Visibility.Collapsed;
        }

        private async Task ContentDia(int option)
        {
            ContentDialog contentDialog = new ContentDialog
            {
                Title = "确认修改",
                PrimaryButtonText = "确认",
                SecondaryButtonText = "取消"

            };
            contentDialog.PrimaryButtonClick += async(_s, _e) =>
            {
                if (option == 0)
                {
                    NU nU = new NU { Name = Nameedit.Text,Intorduction=people.Introduction, Userid = people.UserId };
                    await PutName(nU);
                }
                else
                {
                    NU nU = new NU { Userid = people.UserId, Intorduction = Introductedit.Text ,Name=people.Name};
                    await PutIntro(nU);
                }
            };
            contentDialog.SecondaryButtonClick += (_s, _e) =>
            {

            };
            await contentDialog.ShowAsync();
        }
        private async Task PutName(NU nU)
        {
            
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("applocation/json"));
            var content = new StringContent(JsonConvert.SerializeObject(nU), Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await httpClient.PutAsync("http://localhost:60671/api/values/putname", content);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var res = await httpResponseMessage.Content.ReadAsAsync<bool>();
                if (res == true)
                {
                    people.Name =nU.Name;
                    Bindings.Update();
                    DetailName.Visibility = Visibility.Collapsed;
                }
            }
        }

        private async Task PutIntro(NU nU)
        {

            HttpClient httpClient = new HttpClient();
            
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("applocation/json"));
            var content = new StringContent(JsonConvert.SerializeObject(nU), Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await httpClient.PutAsync("http://localhost:60671/api/values/putintroduction", content);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var res = await httpResponseMessage.Content.ReadAsAsync<bool>();
               
                if (res == true)
                {
                    people.Introduction = nU.Intorduction;
                    Bindings.Update();
                    DetailIntro.Visibility = Visibility.Collapsed;
                }
            }
        }

        struct NU
        {
            public string Intorduction;
            public string Name;
            public int Userid;
        }

        private void Draft_Click(object sender, RoutedEventArgs e)
        {
            HomePage.Current.ContentViewFrame.Navigate(typeof(MyDraft));
        }
    }
}

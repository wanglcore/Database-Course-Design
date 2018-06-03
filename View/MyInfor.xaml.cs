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
    public sealed partial class MyInfor : Page
    {
        MyStruct myStruct;
        People people;
        public MyInfor()
        {
            this.InitializeComponent();
        }
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            myStruct = (MyStruct)e.Parameter;
            await Getinfo(myStruct.id);
        }
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
            Frame.Navigate(typeof(UseridQusition), myStruct);
        }

        private void Mycollection_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Frame.Navigate(typeof(UserFollowQ), myStruct);

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
                var success = await httpResponseMessage.Content.ReadAsAsync<string>();
                if (success != string.Empty)
                {
                    people = JsonConvert.DeserializeObject<People>(success);

                }
            }
        }
    }
}

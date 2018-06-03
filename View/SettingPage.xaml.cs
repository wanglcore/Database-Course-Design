using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace APP.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingPage : Page
    {
        public SettingPage()
        {
            this.InitializeComponent();
        }

        private void Logoutbutton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialogshow();
        }
        private async void ContentDialogshow()
        {
            ContentDialog contentDialog = new ContentDialog
            {
                Title = "确认退出??",
                PrimaryButtonText = "退出",
                SecondaryButtonText = "返回"
            };
            contentDialog.PrimaryButtonClick += (_s, _e) => {
                HomePage.Current.HomeFrameView.Navigate(typeof(MainPage));
            };
            contentDialog.SecondaryButtonClick += (_s, _e) => { };
            await contentDialog.ShowAsync();

        }

        private void AccountandSecure_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(FindPasswd));
        }
    }

}

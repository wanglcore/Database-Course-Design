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
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace APP.View
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Qusitionviewpage : Page
    {
        List<Qusition> qusitions;
        MyStruct myStruct;
        public Qusitionviewpage()
        {
            this.InitializeComponent();
        }
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is MyStruct)
            {
                myStruct = (MyStruct)e.Parameter;
                await GetUserQusition(myStruct);
            }
            else if(e.Parameter is List<Qusition>)
            {
                qusitions = (List<Qusition>)e.Parameter;
            }
            else if(e.Parameter is Tuple<string, List<Qusition>>tuple)
            {
                AutoSuggest.Text = tuple.Item1;
                qusitions = tuple.Item2;
                
            }
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
        private void ItemListview_ItemClick(object sender, ItemClickEventArgs e)
        {
            var clickeditem = (Qusition)e.ClickedItem;
         
            HomePage.Current.ContentViewFrame.Navigate(typeof(AnswerQusition), clickeditem);
        }
        /// <summary>
        /// 得到listview下的所有子控件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dependencyObject"></param>
        /// <returns></returns>
        private List<T> GetChildObjects<T>(DependencyObject dependencyObject) where T : FrameworkElement
        {
            DependencyObject dependency = null;
            List<T> childlist = new List<T>();
            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(dependencyObject) - 1; i++)
            {
                dependency = VisualTreeHelper.GetChild(dependencyObject, i);
                if (dependency is T)
                {
                    childlist.Add((T)dependency);
                }
                childlist.AddRange(GetChildObjects<T>(dependency));

            }
            return childlist;
        }

        private void Label0_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string labelname = button.Content.ToString();
            HomePage.Current.ContentViewFrame.Navigate(typeof(LabelDetail), labelname);
        }

        private async void AutoSuggest_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (sender.Text.Length >= 1)
            {
                await GetQusiton(sender.Text);
            }
            else
            {
                sender.PlaceholderText = "查询问题";
            }
        }

        private async Task GetQusiton(string text)
        {
            HttpClient httpClient = new HttpClient
            {
                BaseAddress = new System.Uri("http://localhost:60671/")
            };
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            string sql = $"api/qusition/getbyname/{text}";
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(sql);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                qusitions = await httpResponseMessage.Content.ReadAsAsync<List<Qusition>>();
                if (qusitions != null)
                {
                    itemListview.ItemsSource = qusitions;
                }
            }
        }
    }
}

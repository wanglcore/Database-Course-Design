using APP.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace APP.View
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SelectLabels : Page
    {
        ObservableCollection<Model.Label> selectlabels = new ObservableCollection<Model.Label>();
        
        ObservableCollection<Model.Label> labels;
        int sourcepage;
        List<Button> buttons;
        public SelectLabels()
        {
            InitializeComponent();
        }
        private async void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            string text = sender.Text;
            Labellist.ItemsSource=await Task<List<Label>>.Run(()=> labels.Where(p => p.Labelname.Contains(text)));
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            await Getlabel();
            if (e.Parameter is int)
            {
                sourcepage = (int)e.Parameter;
            }

            base.OnNavigatedTo(e);
        }

        private async Task Getlabel()
        {
            HttpClient httpClient = new HttpClient
            {
                BaseAddress = new System.Uri("http://localhost:60671/")
            };
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            string str = "api/label/getlabel";
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(str);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var listlabel = await httpResponseMessage.Content.ReadAsAsync<string>();
                if (listlabel != string.Empty)
                {
                    labels =new ObservableCollection<Label>( JsonConvert.DeserializeObject<List<Model.Label>>(listlabel));
                    Labellist.ItemsSource = labels;
                }
            }
        }

        private void Labellist_ItemClick(object sender, ItemClickEventArgs e)
        {
            Label label = (Label)e.ClickedItem;
           
            this.Frame.Navigate(typeof(LabelDetail), label);
        }

        private void Haveselect_ItemClick(object sender, ItemClickEventArgs e)
        {
            Label label = (Label)e.ClickedItem;
           
            this.Frame.Navigate(typeof(LabelDetail), label);
        }

        private void Delselectitem_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            int lid = Convert.ToInt32(button.Tag);
            Model.Label content = selectlabels.Where(p => p.Labelid == lid).FirstOrDefault();
            selectlabels.Remove(content);
            labels.Add(content);
            Haveselect.ItemsSource = selectlabels;
            buttons = GetChildObjects<Button>(Labellist);
            buttons.ForEach(p => p.IsEnabled = true);
        }

        private void Selectitem_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            int lid = Convert.ToInt32(button.Tag);
            Model.Label content = labels.Where(p => p.Labelid == lid).FirstOrDefault();
            selectlabels.Add(content);
            labels.Remove(content);
            Haveselect.ItemsSource = selectlabels;
            if (selectlabels.Count == 3&&sourcepage==0)
            {
                buttons = GetChildObjects<Button>(Labellist);
                buttons.ForEach(p => p.IsEnabled = false);
            }
            else if (selectlabels.Count == 1 && sourcepage == 1)
            {
                buttons = GetChildObjects<Button>(Labellist);
                buttons.ForEach(p => p.IsEnabled = false);
            }
        }

        public List<T> GetChildObjects<T>(DependencyObject dependencyObject) where T : FrameworkElement
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

        private void Finish_Click(object sender, RoutedEventArgs e)
        {
            if (sourcepage == 0)
            {
                Frame.Navigate(typeof(MakeQusition), selectlabels);
            }
            else if (sourcepage == 1)
            {
                Frame.Navigate(typeof(EditArtical), selectlabels);
            }
        }

        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }
    }
}

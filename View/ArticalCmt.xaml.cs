using APP.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace APP.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 

    public sealed partial class ArticalCmt : Page
    {
        ObservableCollection<ArtComment>artComments;
        int Artid;
        int Userid;
        Artical artical;
        int Myid = HomePage.Current.myStruct.id;
        People My = HomePage.Current.people;
        string Names;
        int FromPage = 0;
        public ArticalCmt()
        {
            this.InitializeComponent();
        }
        protected override async void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            await  NameMap.TextWrapping();

        }


        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            await NameMap.Read();
            if (e.Parameter is Artical)
            {
                artical = (Artical)e.Parameter;
                await GetComment(artical.Userid, artical.Articalid);
            }
            else if (e.Parameter is ArtComment artComment)
            {
                await GetComment(artComment.Userid, artComment.Articalid,artComment);
                FromPage = 1;
            }
            base.OnNavigatedTo(e);
        }

        /// <summary>
        /// 发表评论
        /// </summary>
        /// <param name="articalCmt"></param>
        /// <returns></returns>
        private async Task PostComment(ArtComment articalCmt)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var content = new StringContent(JsonConvert.SerializeObject(articalCmt), Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await httpClient.PostAsync("http://localhost:60671/api/artcmt/addcomment", content);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var res = await httpResponseMessage.Content.ReadAsAsync<bool>();
                if (res == true)
                {
                    artComments.Insert(0, articalCmt);
                    CmtList.ItemsSource = artComments;
                    CmtGrid.Visibility = Visibility.Collapsed;
                    artical.ArtCmtnum++;

                }
            }
        }
        /// <summary>
        /// 返回文章页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private  void CancelCmtButton_Click(object sender,RoutedEventArgs e)
        {
            if (FromPage == 1)
            {
                Frame.Navigate(typeof(Notice), 0);
            }
            else
            {
                Frame.Navigate(typeof(ArticalShow),artical);
            }
        }
        /// <summary>
        /// 点击评论
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmtList_ItemClick(object sender,ItemClickEventArgs e)
        {
            foreach (var item in CmtList.Items)
            {
                var container = (ListViewItem)CmtList.ContainerFromItem(item);
                if (container != null)
                {

                    if ((container.ContentTemplateRoot as FrameworkElement)?.FindName("CmtContenttwo") is FrameworkElement submenu)
                    {
                        if ((container.ContentTemplateRoot as FrameworkElement)?.FindName("CmtContentone") is FrameworkElement submenus)
                        {
                            submenus.Visibility = e.ClickedItem == item ? Visibility.Collapsed : Visibility.Visible;
                        }
                        submenu.Visibility = e.ClickedItem == item ? Visibility.Visible : Visibility.Collapsed;
                    }
                }
            }
        }

        /// <summary>
        /// 点击添加评论
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddCmtBox.Visibility = Visibility.Visible;
            AddCmtBox.Height = ActualHeight / 4;
            AcceptButton.Visibility = Visibility.Visible;
            CancleButton.Visibility = Visibility.Visible;
        }



        /// <summary>
        /// 提交评论
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            string content = AddCmtBox.Text;
            if (content == "")
            {
                AddCmtBox.PlaceholderText = "没有输入评论";
                AddCmtBox.PlaceholderForeground = new SolidColorBrush(Colors.Red);
            }
            else
            {
               await Contentshow(content);
            }
        }


        private async Task Contentshow(string content)
        {
            ContentDialog contentDialog = new ContentDialog
            {
                Title = "确认",
                PrimaryButtonText = "确认",
                SecondaryButtonText = "退出",
            };
            contentDialog.PrimaryButtonClick += async (_s, _e) =>
             {
                 ArtComment artComment = new ArtComment
                 {
                     Articalid = artical.Articalid,
                     Userid = artical.Userid,
                     ArtCmtUserid = My.UserId,
                     ArtCmtedUserid = artical.Userid,
                     ArtCmtTime = DateTime.Now,
                     ArtCmtContent = content,
                     CmdUseridName = My.Name,
                     UseridName = artical.UseridName,
                     CmtedUseridName = artical.UseridName,
                  
                 };
                 await PostComment(artComment);
             };
            contentDialog.SecondaryButtonClick += (_s, _e) =>
            {

            };
            await contentDialog.ShowAsync();
        }

/// <summary>
/// 对评论赞同
/// </summary>
/// <param name="artCommandPlus"></param>
/// <param name="listViewItem"></param>
/// <returns></returns>
        private async Task PutCmdLike(ArtCommandPlus artCommandPlus,ListViewItem listViewItem)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var content = new StringContent(JsonConvert.SerializeObject(artCommandPlus), Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await httpClient.PutAsync($"http://localhost:60671/api/artcmt/putlike", content);
            var res = await httpResponseMessage.Content.ReadAsAsync<bool>();
            if (res == true)
            {
                Modifyinf(artCommandPlus, listViewItem, 0);
            }
        }
        /// <summary>
        /// 取消对文章的赞同
        /// </summary>
        /// <param name="artCommandPlus"></param>
        /// <param name="listViewItem"></param>
        /// <returns></returns>
        private async Task DelPutCmdLike(ArtCommandPlus artCommandPlus,ListViewItem listViewItem)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var content = new StringContent(JsonConvert.SerializeObject(artCommandPlus), Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await httpClient.PutAsync($"http://localhost:60671/api/artcmt/delputlike", content);
            var res = await httpResponseMessage.Content.ReadAsAsync<bool>();
            if (res == true)
            {
                Modifyinf(artCommandPlus, listViewItem, 0);
            }
        }
        /// <summary>
        /// 不赞同评论
        /// </summary>
        /// <param name="artCommandPlus"></param>
        /// <param name="listViewItem"></param>
        /// <returns></returns>
        private async Task PutDisCmdLike(ArtCommandPlus artCommandPlus,ListViewItem listViewItem)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var content = new StringContent(JsonConvert.SerializeObject(artCommandPlus), Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await httpClient.PutAsync($"http://localhost:60671/api/artcmt/putdislike", content);
            var res = await httpResponseMessage.Content.ReadAsAsync<bool>();
            if (res == true)
            {
                Modifyinf(artCommandPlus, listViewItem, 1);
            }
        }
        /// <summary>
        /// 取消对文章的反对
        /// </summary>
        /// <param name="artCommandPlus"></param>
        /// <param name="listViewItem"></param>
        /// <returns></returns>
        private async Task DelPutDisCmdLike(ArtCommandPlus artCommandPlus,ListViewItem listViewItem)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var content = new StringContent(JsonConvert.SerializeObject(artCommandPlus), Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await httpClient.PutAsync($"http://localhost:60671/api/artcmt/delputdislike", content);
            var res = await httpResponseMessage.Content.ReadAsAsync<bool>();
            if (res == true)
            {
                Modifyinf(artCommandPlus, listViewItem, 1);
            }
        }
        /// <summary>
        /// 修改按钮的颜色
        /// </summary>
        /// <param name="artCommandPlus"></param>
        /// <param name="listViewItem"></param>
        /// <param name="option"></param>
        private void Modifyinf(ArtCommandPlus artCommandPlus,ListViewItem listViewItem,int option)
        {
            var comment = artComments.Where(p => p.ArtCmtUserid == artCommandPlus.CmdUserid && p.ArtCmtedUserid == artCommandPlus.CmtedUserid).FirstOrDefault();
            if (listViewItem != null)
            {
                AppBarButton LikeButton = (listViewItem.ContentTemplateRoot as FrameworkElement)?.FindName("LikeButton") as AppBarButton;
                AppBarButton DisLikeButton = (listViewItem.ContentTemplateRoot as FrameworkElement)?.FindName("DislikeButton") as AppBarButton;
                if (option == 0)
                {
                    var fore = LikeButton.Foreground as SolidColorBrush;
                    if (fore.Color == Colors.Black)
                    {
                        comment.UpArtCmtnum++;
                        LikeButton.Foreground = new SolidColorBrush(Colors.Blue);
                        NameMap.UPArticalCmt.Add($"{artical.Articalid} {artical.Userid} {artCommandPlus.Datetime} {Myid} {artCommandPlus.CmdUserid} {artCommandPlus.CmtedUserid}");

                    }
                    else if (fore.Color == Colors.Blue)
                    {
                        comment.UpArtCmtnum--;
                        LikeButton.Foreground = new SolidColorBrush(Colors.Black);
                        NameMap.UPArticalCmt.Remove($"{artical.Articalid} {artical.Userid} {artCommandPlus.Datetime} {Myid} {artCommandPlus.CmdUserid} {artCommandPlus.CmtedUserid}");

                    }
                    LikeButton.Label = (comment.UpArtCmtnum).ToString();
                }
                else
                {
                    var fore = DisLikeButton.Foreground as SolidColorBrush;
                    if (fore.Color == Colors.Black)
                    {
                        comment.DownArtCmtnum++;
                        DisLikeButton.Foreground = new SolidColorBrush(Colors.Blue);
                        NameMap.DNArticalCmt.Add($"{artical.Articalid} {artical.Userid} {artCommandPlus.Datetime} {Myid} {artCommandPlus.CmdUserid} {artCommandPlus.CmtedUserid}");

                    }
                    else if (fore.Color == Colors.Blue)
                    {
                        comment.DownArtCmtnum--;
                        DisLikeButton.Foreground = new SolidColorBrush(Colors.Black);
                        NameMap.DNArticalCmt.Remove($"{artical.Articalid} {artical.Userid} {artCommandPlus.Datetime} {Myid} {artCommandPlus.CmdUserid} {artCommandPlus.CmtedUserid}");

                    }
                    DisLikeButton.Label = (comment.DownArtCmtnum).ToString();
                }
            }
        }
        /// <summary>
        /// 点击赞同按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void LikeButton_Click(object sender, RoutedEventArgs e)
        {
            AppBarButton appBarButton = (AppBarButton)sender;
            ListViewItem listViewItem = FindParent<ListViewItem>(appBarButton);
            if (listViewItem != null){
                ArtCommandPlus artCommandPlus = GetCmtInfor(listViewItem);
                var fore = appBarButton.Foreground as SolidColorBrush;
                if (fore.Color == Colors.Blue)
                {
                    await DelPutCmdLike(artCommandPlus, listViewItem);
                }
                else
                {
                    await PutCmdLike(artCommandPlus, listViewItem);
                }
            }
        }
         /// <summary>
         /// 点击不赞同按钮
         /// </summary>
         /// <param name="sender"></param>
         /// <param name="e"></param>
        private async void DislikeButton_Click(object sender, RoutedEventArgs e)
        {
            AppBarButton appBarButton = (AppBarButton)sender;
            ListViewItem listViewItem = FindParent<ListViewItem>(appBarButton);
            if (listViewItem != null)
            {
                ArtCommandPlus artCommandPlus = GetCmtInfor(listViewItem);
                var fore = appBarButton.Foreground as SolidColorBrush;
                if (fore.Color == Colors.Blue)
                {
                    await DelPutDisCmdLike(artCommandPlus, listViewItem);
                }
                else
                {
                    await PutDisCmdLike(artCommandPlus, listViewItem);
                }
            }
        }

        /// <summary>
        /// 获取一个评论的所有信息
        /// </summary>
        /// <param name="listViewItem"></param>
        /// <returns></returns>
        private ArtCommandPlus GetCmtInfor(ListViewItem listViewItem)
        {
            int CmdUserid, CmtedUserid;
            string dateTime;
            TextBlock CmtUseridBlock = (listViewItem.ContentTemplateRoot as FrameworkElement)?.FindName("CmtUseridBlock") as TextBlock;
            CmdUserid = Convert.ToInt32(CmtUseridBlock.Text);
            TextBlock CmtedUseridBlock = (listViewItem.ContentTemplateRoot as FrameworkElement)?.FindName("CmtedUseridBlock") as TextBlock;
            CmtedUserid = Convert.ToInt32(CmtedUseridBlock.Text);
            var CmtTime = (listViewItem.ContentTemplateRoot as FrameworkElement)?.FindName("CmtTime") as TextBlock;
            dateTime = (CmtTime.Text);
            ArtCommandPlus artCommandPlus = new ArtCommandPlus
            {
                Articalid = artical.Articalid,
                Userid = artical.Userid,
                CmdUserid = CmdUserid,
                CmtedUserid = CmtedUserid,
                Datetime = dateTime,
                Myid = My.UserId,
            };
            return artCommandPlus;
        }
        /// <summary>
        /// 获取评论列表
        /// </summary>
        /// <param name="Userid"></param>
        /// <param name="Articalid"></param>
        /// <returns></returns>
        private async Task GetComment(int Userid,int Articalid)
        {
            HttpClient httpClient = new HttpClient
            {
                BaseAddress = new System.Uri("http://localhost:60671/")
            };
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            string str = $"api/artcmt/getcomment/{Userid}/{Articalid}";
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(str);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var res = await httpResponseMessage.Content.ReadAsAsync<string>();
                if (res != string.Empty)
                {
                    artComments = new ObservableCollection<ArtComment>(JsonConvert.DeserializeObject<List<ArtComment>>(res));
                    CmtList.ItemsSource = artComments;
                    this.Bindings.Update();
                    foreach (var item in artComments)
                    {
                        if (NameMap.UPArticalCmt.Contains($"{item.Articalid} {item.Userid} {item.ArtCmtTime} {Myid} {item.ArtCmtUserid} {item.ArtCmtedUserid}"))
                        {
                            var container = (ListViewItem)CmtList.ContainerFromItem(item);
                            if (container == null)
                            {
                                CmtList.UpdateLayout();
                                CmtList.ScrollIntoView(CmtList.Items[artComments.IndexOf(item)]);
                                container = (ListViewItem)CmtList.ContainerFromItem(item);
                            }
                            var ups = (container.ContentTemplateRoot as FrameworkElement)?.FindName("LikeButton") as AppBarButton;
                            ups.Foreground = new SolidColorBrush(Colors.Blue);
                        }
                        if (NameMap.DNArticalCmt.Contains($"{item.Articalid} {item.Userid} {item.ArtCmtTime} {Myid} {item.ArtCmtUserid} {item.ArtCmtedUserid}"))
                        {
                            var container = (ListViewItem)CmtList.ContainerFromItem(item);
                            if (container == null)
                            {
                                CmtList.UpdateLayout();
                                CmtList.ScrollIntoView(CmtList.Items[artComments.IndexOf(item)]);
                                container = (ListViewItem)CmtList.ContainerFromItem(item);
                            }
                            var downs = (container.ContentTemplateRoot as FrameworkElement)?.FindName("DislikeButton") as AppBarButton;
                            downs.Foreground = new SolidColorBrush(Colors.Blue);

                        }


                    }
                   
                  

                }
            }

        }



        private async Task GetComment(int Userid, int Articalid, ArtComment artComment)
        {
            HttpClient httpClient = new HttpClient
            {
                BaseAddress = new System.Uri("http://localhost:60671/")
            };
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            string str = $"api/artcmt/getcomment/{Userid}/{Articalid}";
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(str);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var res = await httpResponseMessage.Content.ReadAsAsync<string>();
                if (res != string.Empty)
                {
                    artComments = new ObservableCollection<ArtComment>(JsonConvert.DeserializeObject<List<ArtComment>>(res));
                    CmtList.ItemsSource = artComments;
                    this.Bindings.Update();
                    foreach (var item in artComments)
                    {
                        if (NameMap.UPArticalCmt.Contains($"{item.Articalid} {item.Userid} {item.ArtCmtTime} {Myid} {item.ArtCmtUserid} {item.ArtCmtedUserid}"))
                        {
                            var container = (ListViewItem)CmtList.ContainerFromItem(item);
                            if (container == null)
                            {
                                CmtList.UpdateLayout();
                                CmtList.ScrollIntoView(CmtList.Items[artComments.IndexOf(item)]);
                                container = (ListViewItem)CmtList.ContainerFromItem(item);
                            }
                            var ups = (container.ContentTemplateRoot as FrameworkElement)?.FindName("LikeButton") as AppBarButton;
                            ups.Foreground = new SolidColorBrush(Colors.Blue);
                        }
                        if (NameMap.DNArticalCmt.Contains($"{item.Articalid} {item.Userid} {item.ArtCmtTime} {Myid} {item.ArtCmtUserid} {item.ArtCmtedUserid}"))
                        {
                            var container = (ListViewItem)CmtList.ContainerFromItem(item);
                            if (container == null)
                            {
                                CmtList.UpdateLayout();
                                CmtList.ScrollIntoView(CmtList.Items[artComments.IndexOf(item)]);
                                container = (ListViewItem)CmtList.ContainerFromItem(item);
                            }
                            var downs = (container.ContentTemplateRoot as FrameworkElement)?.FindName("DislikeButton") as AppBarButton;
                            downs.Foreground = new SolidColorBrush(Colors.Blue);

                        }


                    }
                }
            }

        }





        /// <summary>
        /// 寻找控件的父控件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dependencyObject"></param>
        /// <returns></returns>
        private T FindParent<T>(DependencyObject dependencyObject) where T : DependencyObject
        {
            var parent = VisualTreeHelper.GetParent(dependencyObject);
            if (parent == null)
                return null;
            var parentT = parent as T;
            return parentT ?? FindParent<T>(parent);
        }

        private void AddtoButton_Click(object sender, RoutedEventArgs e)
        {
            AppBarButton appBarButton = (AppBarButton)sender;
            ListViewItem listViewItem = FindParent<ListViewItem>(appBarButton);
            if (listViewItem != null)
            {
                if((listViewItem.ContentTemplateRoot as FrameworkElement).FindName("AddCmdGrid") is Grid AddCmdGrid)
                {
                    AddCmdGrid.Visibility = Visibility.Visible;
                    AddCmdGrid.Height = ActualHeight / 4;
                }
            }
        }

        private async void CAcceptButton_Click(object sender, RoutedEventArgs e)
        {
            AppBarButton appBarButton = (AppBarButton)sender;
            ListViewItem listViewItem = FindParent<ListViewItem>(appBarButton);
            ArtCommandPlus artCommandPlus = GetCmtInfor(listViewItem);
            var box = (listViewItem.ContentTemplateRoot as FrameworkElement)?.FindName("Commentadd") as TextBox;
            var CmdName = (listViewItem.ContentTemplateRoot as FrameworkElement)?.FindName("CmdName") as Button;
            var cmtedName = (listViewItem.ContentTemplateRoot as FrameworkElement)?.FindName("CmtedName") as Button;
            string Text = box.Text;
            ArtComment artComment = new ArtComment
            {
                Articalid = artical.Articalid,
                Userid = artical.Userid,
                ArtCmtUserid = My.UserId,
                ArtCmtedUserid = artical.Userid,
                ArtCmtContent = Text,
                ArtCmtTime = DateTime.Now,
                CmdUseridName = My.Name,
                UseridName = artical.UseridName,
                CmtedUseridName = cmtedName.Content.ToString(),
              
            };
            await PostComment(artComment);
            if ((listViewItem.ContentTemplateRoot as FrameworkElement)?.FindName("AddCmdGrid") is Grid AddCmdGrid)
            {
                AddCmdGrid.Visibility = Visibility.Collapsed;
            }
        }

        private void CmdName_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Frame.Navigate(typeof(UUserPage), Convert.ToInt32(button.Tag));
        }

        private void CmtedName_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Frame.Navigate(typeof(UUserPage), Convert.ToInt32(button.Tag));
        }
    }
}

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
    public sealed partial class AnswerCmt : Page
    {
        private int Qusitionid;
        private int Userid;
        private int Myid = HomePage.Current.myStruct.id;
        private People people = HomePage.Current.people;
        private string Names;
        Comment Comments;
        private ObservableCollection<Comment> commentPlus = new ObservableCollection<Comment>();
        int FromPage = 0;
        public AnswerCmt()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            await NameMap.TextWrapping();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            await NameMap.Read();

            if (e.Parameter is Dictionary<string, string> keyValues)
            {
                Qusitionid = Convert.ToInt32(keyValues["Qusitionid"]);
                Userid = Convert.ToInt32(keyValues["Userid"]);
                Names = keyValues["Name"];
                await GetComment(Qusitionid, Userid);
            }
            else if (e.Parameter is Comment comment)
            {
                Comments=comment;
                await GetComments(comment.Qusitionid, comment.Userid, comment);
                FromPage = 1;
            }
            base.OnNavigatedTo(e);
        }

        /// <summary>
        /// 添加评论
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddCmtBox.Height = ActualHeight / 2;
            AddCmtBox.Visibility = Visibility.Visible;
            AcceptButton.Visibility = Visibility.Visible;
            CancleButton.Visibility = Visibility.Visible;
        }

        private async void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            await ContentDia(0);
        }

        private async Task GetComment(int Qusitonid, int Userid)
        {
            HttpClient httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:60671/")
            };
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            string str = $"api/comment/getcomment/{Userid}/{Qusitonid}";
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(str);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var res = await httpResponseMessage.Content.ReadAsAsync<string>();
                if (res != string.Empty)
                {
                    commentPlus = new ObservableCollection<Comment>(JsonConvert.DeserializeObject<List<Comment>>(res));
                    CmtList.ItemsSource = commentPlus;
                    this.Bindings.Update();
                    foreach (var item in commentPlus)
                    {
                        if (NameMap.UPAnswerCmt.Contains($"{item.Qusitionid} {item.Userid} {item.CmtTime} {Myid} {item.CmdUserid} {item.CmtedUserid}"))
                        {
                            var container = (ListViewItem)CmtList.ContainerFromItem(item);
                            if (container == null)
                            {
                                CmtList.UpdateLayout();
                                CmtList.ScrollIntoView(CmtList.Items[commentPlus.IndexOf(item)]);
                                container = (ListViewItem)CmtList.ContainerFromItem(item);
                            }

                            var ups = (container.ContentTemplateRoot as FrameworkElement)?.FindName("LikeButton") as AppBarButton;
                            ups.Foreground = new SolidColorBrush(Colors.Blue);
                        }
                        if (NameMap.DNAnswerCmt.Contains($"{item.Qusitionid} {item.Userid} {item.CmtTime} {Myid} {item.CmdUserid} {item.CmtedUserid}"))
                        {
                            var container = (ListViewItem)CmtList.ContainerFromIndex(commentPlus.IndexOf(item));
                            if (container == null)
                            {
                                CmtList.UpdateLayout();
                                CmtList.ScrollIntoView(CmtList.Items[commentPlus.IndexOf(item)]);
                                container = (ListViewItem)CmtList.ContainerFromItem(item);
                            }
                            var downs = (container.ContentTemplateRoot as FrameworkElement)?.FindName("DislikeButton") as AppBarButton;
                            downs.Foreground = new SolidColorBrush(Colors.Blue);
                        }
                    }
                }
            }
        }

        private async Task GetComments(int Qusitonid, int Userid, Comment comment)
        {
            HttpClient httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:60671/")
            };
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            string str = $"api/comment/getcomment/{Userid}/{Qusitonid}";
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(str);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var res = await httpResponseMessage.Content.ReadAsAsync<string>();
                if (res != string.Empty)
                {
                    commentPlus = new ObservableCollection<Comment>(JsonConvert.DeserializeObject<List<Comment>>(res));
                    CmtList.ItemsSource = commentPlus;
                    this.Bindings.Update();
                    foreach (var item in commentPlus)
                    {
                        if (NameMap.UPAnswerCmt.Contains($"{item.Qusitionid} {item.Userid} {item.CmtTime} {Myid} {item.CmdUserid} {item.CmtedUserid}"))
                        {
                            var container = (ListViewItem)CmtList.ContainerFromItem(item);
                            if (container == null)
                            {
                                CmtList.UpdateLayout();
                                CmtList.ScrollIntoView(CmtList.Items[commentPlus.IndexOf(item)]);
                                container = (ListViewItem)CmtList.ContainerFromItem(item);
                            }

                            var ups = (container.ContentTemplateRoot as FrameworkElement)?.FindName("LikeButton") as AppBarButton;
                            ups.Foreground = new SolidColorBrush(Colors.Blue);
                        }
                        if (NameMap.DNAnswerCmt.Contains($"{item.Qusitionid} {item.Userid} {item.CmtTime} {Myid} {item.CmdUserid} {item.CmtedUserid}"))
                        {
                            var container = (ListViewItem)CmtList.ContainerFromIndex(commentPlus.IndexOf(item));
                            if (container == null)
                            {
                                CmtList.UpdateLayout();
                                CmtList.ScrollIntoView(CmtList.Items[commentPlus.IndexOf(item)]);
                                container = (ListViewItem)CmtList.ContainerFromItem(item);
                            }
                            var downs = (container.ContentTemplateRoot as FrameworkElement)?.FindName("DislikeButton") as AppBarButton;
                            downs.Foreground = new SolidColorBrush(Colors.Blue);
                        }
                    }

                }
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

        private async Task PostComment(Comment comment)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var content = new StringContent(JsonConvert.SerializeObject(comment), Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await httpClient.PostAsync("http://localhost:60671/api/comment/addcomment", content);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var res = await httpResponseMessage.Content.ReadAsAsync<bool>();
                if (res == true)
                {
                    commentPlus.Insert(0, comment);
                    CmtList.ItemsSource = commentPlus;
                    CmtGrid.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void CancelCmtButton_Click(object sender, RoutedEventArgs e)
        {
            if (FromPage == 1)
            {
                Frame.Navigate(typeof(Notice), 1);
            }
            else
            {
                Frame.Navigate(typeof(AnswerQusition));
            }
        }

        /// <summary>
        /// 列表中的某一项被点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmtList_ItemClick(object sender, ItemClickEventArgs e)
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
        /// 赞同按钮被点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void LikeButton_Click(object sender, RoutedEventArgs e)
        {
            AppBarButton appBarButton = (AppBarButton)sender;
            ListViewItem listViewItem = FindParent<ListViewItem>(appBarButton);
            CommentPlus commentPlus = GetCmtInfor(listViewItem);
            var fore = appBarButton.Foreground as SolidColorBrush;
            if (fore.Color == Colors.Blue)
            {
                await DelPutCmdLike(commentPlus, listViewItem);
            }
            else
            {
                await PutCmdLike(commentPlus, listViewItem);
            }
        }

        /// <summary>
        /// 得到listviewitem中的子空间
        /// </summary>
        /// <param name="listViewItem"></param>
        /// <returns></returns>
        private CommentPlus GetCmtInfor(ListViewItem listViewItem)
        {
            int CmtedUserid, CmdUserid;
            string dateTime;
            TextBlock CmtUseridBlock = (listViewItem.ContentTemplateRoot as FrameworkElement)?.FindName("CmtUseridBlock") as TextBlock;
            CmdUserid = Convert.ToInt32(CmtUseridBlock.Text);
            TextBlock CmtedUseridBlock = (listViewItem.ContentTemplateRoot as FrameworkElement)?.FindName("CmtedUseridBlock") as TextBlock;
            CmtedUserid = Convert.ToInt32(CmtedUseridBlock.Text);
            var CmtTime = (listViewItem.ContentTemplateRoot as FrameworkElement)?.FindName("CmtTime") as TextBlock;
            dateTime = (CmtTime.Text);
            CommentPlus comment = new CommentPlus
            {
                Qusitionid = Qusitionid,
                Userid = Userid,
                CmdUserid = CmdUserid,
                CmtedUserid = CmtedUserid,
                Datetime = dateTime,
                Myid = Myid
            };
            return comment;
        }

        /// <summary>
        /// 赞同评论
        /// </summary>
        /// <param name="CmtedUserid"></param>
        /// <param name="CmdUserid"></param>
        /// <param name="listViewItem"></param>
        /// <returns></returns>
        private async Task PutCmdLike(CommentPlus commentPlus, ListViewItem listViewItem)
        {
            int Myid = (HomePage.Current.myStruct.id);
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            string str = $"api/comment/putlike";
            var content = new StringContent(JsonConvert.SerializeObject(commentPlus), Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await httpClient.PutAsync($"http://localhost:60671/api/comment/putlike", content);
            var res = await httpResponseMessage.Content.ReadAsAsync<bool>();
            if (res == true)
            {
                Modifyinf(listViewItem, 0, commentPlus);
            }
        }

        private async Task DelPutCmdLike(CommentPlus commentPlus, ListViewItem listViewItem)
        {
            int Myid = (HomePage.Current.myStruct.id);
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            string str = $"api/comment/delputlike";
            var content = new StringContent(JsonConvert.SerializeObject(commentPlus), Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await httpClient.PutAsync($"http://localhost:60671/api/comment/delputlike", content);
            var res = await httpResponseMessage.Content.ReadAsAsync<bool>();
            if (res == true)
            {
                Modifyinf(listViewItem, 0, commentPlus);
            }
        }

        private async Task PutCmdDislike(CommentPlus commentPlus, ListViewItem listViewItem)
        {
            int Myid = (HomePage.Current.myStruct.id);
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var content = new StringContent(JsonConvert.SerializeObject(commentPlus), Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await httpClient.PutAsync($"http://localhost:60671/api/comment/putdislike", content);
            var res = await httpResponseMessage.Content.ReadAsAsync<bool>();
            if (res == true)
            {
                Modifyinf(listViewItem, 1, commentPlus);
            }
        }

        private async Task DelPutCmdDislike(CommentPlus commentPlus, ListViewItem listViewItem)
        {
            int Myid = (HomePage.Current.myStruct.id);
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var content = new StringContent(JsonConvert.SerializeObject(commentPlus), Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await httpClient.PutAsync($"http://localhost:60671/api/comment/delputdislike", content);
            var res = await httpResponseMessage.Content.ReadAsAsync<bool>();
            if (res == true)
            {
                Modifyinf(listViewItem, 1, commentPlus);
            }
        }

        /// <summary>
        /// 修改按钮的颜色
        /// </summary>
        /// <param name="listViewItem"></param>
        /// <param name="option"></param>
        /// <param name="CmdUserid"></param>
        /// <param name="CmtedUserid"></param>
        private void Modifyinf(ListViewItem listViewItem, int option, CommentPlus commentP)
        {
            var comment = commentPlus.Where(p => p.CmdUserid == commentP.CmdUserid && p.CmtedUserid == commentP.CmtedUserid).FirstOrDefault();
            if (listViewItem != null)
            {
                AppBarButton LikeButton = (listViewItem.ContentTemplateRoot as FrameworkElement)?.FindName("LikeButton") as AppBarButton;
                AppBarButton DisLikeButton = (listViewItem.ContentTemplateRoot as FrameworkElement)?.FindName("DislikeButton") as AppBarButton;
                ///点击like按钮,根据颜色来判断
                if (option == 0)
                {
                    var fore = LikeButton.Foreground as SolidColorBrush;
                    if (fore.Color == Colors.Black)
                    {
                        comment.UpCmtnum++;
                        LikeButton.Foreground = new SolidColorBrush(Colors.Blue);
                        NameMap.UPAnswerCmt.Add($"{commentP.Qusitionid} {commentP.Userid} {commentP.Datetime} {Myid} {commentP.CmdUserid} {commentP.CmtedUserid}");
                    }
                    else if (fore.Color == Colors.Blue)
                    {
                        comment.UpCmtnum--;
                        LikeButton.Foreground = new SolidColorBrush(Colors.Black);
                        NameMap.UPAnswerCmt.Remove($"{commentP.Qusitionid} {commentP.Userid} {commentP.Datetime} {Myid} {commentP.CmdUserid} {commentP.CmtedUserid}");
                    }
                    LikeButton.Label = (comment.UpCmtnum).ToString();
                }
                else///点击dislike按钮,根据颜色来判断
                {
                    var fore = DisLikeButton.Foreground as SolidColorBrush;
                    if (fore.Color == Colors.Black)
                    {
                        comment.DownCmtnum++;
                        DisLikeButton.Foreground = new SolidColorBrush(Colors.Blue);
                        NameMap.DNAnswerCmt.Add($"{commentP.Qusitionid} {commentP.Userid} {commentP.Datetime} {Myid} {commentP.CmdUserid} {commentP.CmtedUserid}");
                    }
                    else if (fore.Color == Colors.Blue)
                    {
                        comment.DownCmtnum--;
                        DisLikeButton.Foreground = new SolidColorBrush(Colors.Black);
                        NameMap.DNAnswerCmt.Remove($"{commentP.Qusitionid} {commentP.Userid} {commentP.Datetime} {Myid} {commentP.CmdUserid} {commentP.CmtedUserid}");
                    }
                    DisLikeButton.Label = (comment.DownCmtnum).ToString();
                }
            }
        }

        private async void DislikeButton_Click(object sender, RoutedEventArgs e)
        {
            AppBarButton appBarButton = (AppBarButton)sender;
            ListViewItem listViewItem = FindParent<ListViewItem>(appBarButton);
            if (listViewItem != null)
            {
                CommentPlus commentp = GetCmtInfor(listViewItem);
                var fore = appBarButton.Foreground as SolidColorBrush;
                if (fore.Color == Colors.Blue)
                {
                    await DelPutCmdDislike(commentp, listViewItem);
                }
                else
                {
                    await PutCmdDislike(commentp, listViewItem);
                }
            }
        }

        /// <summary>
        /// 找到一个控件的父控件
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
                if ((listViewItem.ContentTemplateRoot as FrameworkElement)?.FindName("AddCmdGrid") is Grid AddCmdGrid)
                {
                    AddCmdGrid.Visibility = Visibility.Visible;
                    AddCmdGrid.Height = this.ActualHeight / 4;
                }
            }
        }

        private async void CAcceptButton_Click(object sender, RoutedEventArgs e)
        {
            AppBarButton appBarButton = (AppBarButton)sender;
            ListViewItem listViewItem = FindParent<ListViewItem>(appBarButton);
            CommentPlus commentp = GetCmtInfor(listViewItem);
            var box = (listViewItem.ContentTemplateRoot as FrameworkElement)?.FindName("Commentadd") as TextBox;
            var CmdName = (listViewItem.ContentTemplateRoot as FrameworkElement)?.FindName("CmdName") as Button;
            var CmdsName = (listViewItem.ContentTemplateRoot as FrameworkElement)?.FindName("CmtUseridNameBlock") as TextBlock;
            var cmtedName = (listViewItem.ContentTemplateRoot as FrameworkElement)?.FindName("CmtedName") as Button;

            string Text = box.Text;
            Comment comment = new Comment
            {
                Qusitionid = Comments.Qusitionid,
                Userid = Comments.Userid,
                CmtContent = Text,
                CmdUserid=Myid,
                CmtedUserid = Comments.CmtedUserid,
                CmtTime = DateTime.Now,
                CmtedUseridName = CmdName.Content.ToString(),
                CmdUseridName = people.Name,
                UseridName = CmdsName.Text,
            };
            await PostComment(comment);
            if ((listViewItem.ContentTemplateRoot as FrameworkElement)?.FindName("AddCmdGrid") is Grid AddCmdGrid)
            {
                AddCmdGrid.Visibility = Visibility.Collapsed;
            }
        }

        private void CancleButton_Click(object sender, RoutedEventArgs e)
        {
            if (AddCmtBox.Text.ToString() == "")
            {
                CmtGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
            }
        }

        private async Task ContentDia(int option)
        {
            string text = option == 0 ? "确认发表?" : "评论未发送,取消会丢失";

            ContentDialog contentDialog = new ContentDialog
            {
                Title = text,
                PrimaryButtonText = "确认",
                SecondaryButtonText = "取消"
            };
            contentDialog.PrimaryButtonClick += async (_s, _e) =>
             {
                 if (option == 1)
                 {
                     CmtGrid.Visibility = Visibility.Collapsed;
                 }
                 else if (option == 0)
                 {
                     string content = AddCmtBox.Text;
                     Comment comment = new Comment
                     {
                         Qusitionid = Qusitionid,
                         Userid = Userid,
                         CmtContent = content,
                         CmdUserid = Myid,
                         CmtedUserid = Userid,
                         CmtTime = DateTime.Now,
                         CmtedUseridName = Names,
                         CmdUseridName = people.Name,
                         UseridName = Names,
                     };
                     await PostComment(comment);
                 }
             };
            contentDialog.SecondaryButtonClick += (_s, _e) =>
            {
            };
            await contentDialog.ShowAsync();
        }

        private void CmtedName_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            this.Frame.Navigate(typeof(UUserPage), Convert.ToInt32(button.Tag));
        }

        private void CmdName_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Frame.Navigate(typeof(UUserPage), Convert.ToInt32(button.Tag));
        }

        private void CCancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }

        }
    }
}
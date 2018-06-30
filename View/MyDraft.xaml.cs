using APP.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace APP.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MyDraft : Page
    {
        private ObservableCollection<Draft> drafts = new ObservableCollection<Draft>();
        private ObservableCollection<AnswerDraft> answerDrafts = new ObservableCollection<AnswerDraft>();
        StorageFile storageFile;
        MyStruct myStruct = HomePage.Current.myStruct;
        StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
        ListViewItem viewItem = new ListViewItem();
        ListViewItem vAnsItem = new ListViewItem();
        public MyDraft()
        {
            this.InitializeComponent();
        }


        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            
            await storageFolder.CreateFileAsync("Draft.txt", CreationCollisionOption.OpenIfExists);
            storageFile = await storageFolder.GetFileAsync("Draft.txt");
            Draft draft;
            AnswerDraft answerDraft;
            IList<string> File = Task.Run(async () => { return await FileIO.ReadLinesAsync(storageFile); }).Result;
            File = File.Where(p => p.Split("---").Last() == myStruct.id.ToString()).ToList();
            foreach (var item in File)
            {
                string[] a = item.Split("---");
                if (a[0] == "A")
                {
                    draft = new Draft
                    {
                        dateTime = Convert.ToDateTime(a[1]),
                        Title = a[2],
                        Content = a[3],
                    };
                    drafts.Add(draft);
                }
                else if (a[0] == "Q")
                {
                    answerDraft = new AnswerDraft
                    {
                        dateTime = Convert.ToDateTime(a[1]),
                        QName = a[2],
                        Qid = Convert.ToInt32(a[3]),
                        Content = a[4]
                    };
                    answerDrafts.Add(answerDraft);
                }
            }
            
            if (e.Parameter is string str)
            {
                string[] a = str.Split("---");
                if (a[0] == "A")
                {
                    draft = new Draft
                    {
                        dateTime = Convert.ToDateTime(a[1]),
                        Title = a[2],
                        Content = a[3],
                    };
                    drafts.Add(draft);
                    RootPivot.SelectedIndex = 0;
                }
                else if(a[0]=="Q")
                {
                    answerDraft = new AnswerDraft
                    {
                        dateTime = Convert.ToDateTime(a[1]),
                        QName = a[2],
                        Qid = Convert.ToInt32(a[3]),
                        Content = a[4],
                    };
                    answerDrafts.Add(answerDraft);
                    RootPivot.SelectedIndex = 1;
                }
            }
            Draftlist.ItemsSource = drafts;
            AnswerList.ItemsSource = answerDrafts;
            this.Bindings.Update();
        }

        protected override async void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            await Set(drafts);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
          
                Frame.Navigate(typeof(Blank));
            
        }
        private async Task Get()
        {
            Draft draft;
            List<string> File = new List<string>(await FileIO.ReadLinesAsync(storageFile));
            if (File.Count > 0)
            {
                foreach (var item in File)
                {
                    string[] a = item.Split("---");
                    if (a[0] == "A")
                    {
                        draft = new Draft
                        {
                            dateTime = Convert.ToDateTime(a[1]),
                            Title = a[2],
                            Content = a[3],
                        };
                        drafts.Add(draft);
                    }
                    else if (a[0] == "Q")
                    {
                        AnswerDraft answerDraft = new AnswerDraft
                        {
                            dateTime = Convert.ToDateTime(a[1]),
                            QName = a[1],
                            Qid = Convert.ToInt32(a[2]),
                            Content = a[3],
                        };
                        answerDrafts.Add(answerDraft);
                    }
                }
                Draftlist.ItemsSource = drafts;
                AnswerList.ItemsSource = answerDrafts;
            }
        }

        private async Task Set(ObservableCollection<Draft> drafts)
        {
            List<string> vs = new List<string>();
            foreach (var item in drafts)
            {
                string s = $"A---{item.dateTime}---{item.Title}---{item.Content}---{myStruct.id}";
                vs.Add(s);
            }
            foreach (var item in answerDrafts)
            {
                string s = $"Q---{item.dateTime}---{item.QName}---{item.Qid}---{item.Content}---{myStruct.id}";
                vs.Add(s);
            }
            await FileIO.WriteLinesAsync(storageFile, vs);
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            AppBarButton appBarButton = (AppBarButton)sender;
            await ContentDiashow(appBarButton);
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            AppBarButton appBarButton = (AppBarButton)sender;
            var list = NameMap.FindParent<ListViewItem>(appBarButton);
            if (RootPivot.SelectedIndex==0)
            {
                viewItem = list;
                Frame.Navigate(typeof(EditArtical), drafts[Draftlist.IndexFromContainer(list)]);
            }
            else if (RootPivot.SelectedIndex==1)
            {
                vAnsItem = list;
                Frame.Navigate(typeof(MakeAnswer), answerDrafts[AnswerList.IndexFromContainer(list)]);
            }
        }

        private async Task ContentDiashow(AppBarButton appBarButton)
        {
            ContentDialog contentDialog = new ContentDialog
            {
                Title = "是否删除?",
                PrimaryButtonText = "删除",
                SecondaryButtonText = "取消",
            };
            contentDialog.PrimaryButtonClick += (_s, _e) =>
            {
                var list = NameMap.FindParent<ListViewItem>(appBarButton);
                if (RootPivot.SelectedIndex == 0)
                {
                    drafts.RemoveAt(Draftlist.IndexFromContainer(list));
                }
                else if (RootPivot.SelectedIndex == 1)
                {
                    answerDrafts.RemoveAt(AnswerList.IndexFromContainer(list));
                }
            };
            contentDialog.SecondaryButtonClick += (_s, _e) =>
            {

            };
            await contentDialog.ShowAsync();
        }

      

        private async void DeleteAnswer_Click(object sender, RoutedEventArgs e)
        {
            AppBarButton appBarButton = (AppBarButton)sender;
            await ContentDiashow(appBarButton);
        }
    }
}

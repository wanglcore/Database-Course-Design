using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
namespace APP.Model
{
   public static class NameMap
    {
        static StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
       public static List<string> UPArtical;
       public static StorageFile sampleFile0, storageFile0, storageFile1, storageFile2, storageFile3, sampleFile1, sampleFile2, sampleFile3;
       public static List<string> DNArtical;
       public static List<string> DNAnswer;
       public static List<string> UPAnswer;
       public static List<string> UPAnswerCmt;
       public static List<string> DNAnswerCmt;
        public static List<string> UPArticalCmt;
       public static List<string> DNArticalCmt;
        public static async Task TextWrapping()
        {
            try
            {
                await FileIO.WriteLinesAsync(sampleFile0, UPArtical);
                await FileIO.WriteLinesAsync(sampleFile1, UPAnswer);
                await FileIO.WriteLinesAsync(sampleFile2, UPAnswerCmt);
                await FileIO.WriteLinesAsync(sampleFile3, UPArticalCmt);
                await FileIO.WriteLinesAsync(storageFile0, DNArtical);
                await FileIO.WriteLinesAsync(storageFile1, DNAnswer);
                await FileIO.WriteLinesAsync(storageFile2, DNAnswerCmt);
                await FileIO.WriteLinesAsync(storageFile3, DNArticalCmt);
            }
            catch (Exception)
            {

            }
        }
        public static async Task Read()
        {
            try
            {
                await storageFolder.CreateFileAsync("UserUPArtical.txt", CreationCollisionOption.OpenIfExists);
                await storageFolder.CreateFileAsync("UserDNArtical.txt", CreationCollisionOption.OpenIfExists);
                storageFile0 = await storageFolder.GetFileAsync("UserUPArtical.txt");
                sampleFile0 = await storageFolder.GetFileAsync("UserDNArtical.txt");
                UPArtical = new List<string>(await FileIO.ReadLinesAsync(sampleFile0));
                DNArtical = new List<string>(await FileIO.ReadLinesAsync(storageFile0));
                await storageFolder.CreateFileAsync("UserUPAnswer.txt", CreationCollisionOption.OpenIfExists);
                await storageFolder.CreateFileAsync("UserDNAnswer.txt", CreationCollisionOption.OpenIfExists);
                storageFile1 = await storageFolder.GetFileAsync("UserUPAnswer.txt");
                sampleFile1 = await storageFolder.GetFileAsync("UserDNAnswer.txt");
                UPAnswer = new List<string>(await FileIO.ReadLinesAsync(sampleFile1));
                DNAnswer = new List<string>(await FileIO.ReadLinesAsync(storageFile1));
                await storageFolder.CreateFileAsync("AnswerCmtUP.txt", CreationCollisionOption.OpenIfExists);
                await storageFolder.CreateFileAsync("AnswerCmtDN.txt", CreationCollisionOption.OpenIfExists);
                storageFile2 = await storageFolder.GetFileAsync("AnswerCmtUP.txt");
                sampleFile2 = await storageFolder.GetFileAsync("AnswerCmtDN.txt");
                UPAnswerCmt = new List<string>(await FileIO.ReadLinesAsync(sampleFile2));
                DNAnswerCmt = new List<string>(await FileIO.ReadLinesAsync(storageFile2));
                await storageFolder.CreateFileAsync("ArticalCmtUP.txt", CreationCollisionOption.OpenIfExists);
                await storageFolder.CreateFileAsync("ArticalCmtDN.txt", CreationCollisionOption.OpenIfExists);
                storageFile3 = await storageFolder.GetFileAsync("ArticalCmtUP.txt");
                sampleFile3 = await storageFolder.GetFileAsync("ArticalCmtDN.txt");
                UPArticalCmt = new List<string>(await FileIO.ReadLinesAsync(sampleFile3));
                DNArticalCmt = new List<string>(await FileIO.ReadLinesAsync(storageFile3));
            }
            catch (Exception)
            {

            }
        }
        public static Dictionary<char, string> Map = new Dictionary<char, string>()
        {
            {'+',"%2B" },
            { ' ',"%20"},
            { '/',"%2F"},
            {'?',"%3F" },
            {'#',"%23" },
            { '&',"%26"},
            { '=',"3D"},
        };
        static char[] Ptr = { '+',' ','/','?','#','&','=' };

        public static StorageFile SampleFile0 { get => sampleFile0; set => sampleFile0 = value; }

        public static string ChangeMap(string Text)
        {
            string text = "";
            foreach (var item in Text)
            {
                if (Ptr.Contains(item))
                {
                    text += Map[item];
                }
                else
                {
                    text += item;
                }
            }
            return text;

        }

        public static T FindParent<T>(DependencyObject dependencyObject) where T : DependencyObject
        {
            var parent = VisualTreeHelper.GetParent(dependencyObject);
            if (parent == null)
                return null;
            var parentT = parent as T;
            return parentT ?? FindParent<T>(parent);
        }
        public static List<T> GetChildObjects<T>(DependencyObject dependencyObject) where T : FrameworkElement
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
    }
}

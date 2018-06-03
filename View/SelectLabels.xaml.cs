using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class SelectLabels : Page
    {
        private List<Model.Label> labels;
        public SelectLabels()
        {
            labels = new List<Model.Label>();
            Initlist();
            this.InitializeComponent();
        }
        public void Initlist()
        {
            Model.Label label = new Model.Label
            {
                Labelname = "计算机专业"
            };
            labels.Add(label);
            label.Labelname = "计算机图形学";
            labels.Add(label);
        }

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            sender.ItemsSource = labels;
        }
    }
}

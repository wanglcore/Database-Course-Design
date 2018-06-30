using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
namespace APP.Model
{
    public class Label: INotifyPropertyChanged
    {
        public int Labelid { get; set; }
        public DateTime LabelCTime { get; set; }
        public string Labelname { get; set; }
        public string LabelBrief { get; set; }
        public int LabelQnum { get; set; }
        public int LabelUnum { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName]String propertyName = "")

        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

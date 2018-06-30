using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace APP.Model
{
    public class GetUserFollow:INotifyPropertyChanged
    {
        public int Qusitonid { get; set; }
        public int Askerid { get; set; }
        public DateTime AskTime { get; set; }
        public DateTime UpAskTime { get; set; }
        public string QusitionTitle { get; set; }
        public string QusitionContent { get; set; }
        public int Answerednum { get; set; }
        public int Followednum { get; set; }
        public string Qlabel0 { get; set; }
        public string Qlabel1 { get; set; }
        public string Qlabel2 { get; set; }
        public DateTime FollowQTime { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName]String propertyName = "")

        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

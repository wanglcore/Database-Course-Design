using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace APP.Model
{
    public class Answershow : INotifyPropertyChanged
    {
      
        public int Qusitionid { get; set; }
        public int Userid { get; set; }
        public DateTime AnswerTime { get; set; }
        public DateTime UpAnsTime { get; set; }
        public string AnswerContent { get; set; }
        public int UpAnsnum { get; set; }
        public int DownAnsnum { get; set; }
        public int AnsCmtnum { get; set; }
        public string Name { get; set; }
        public string QName { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName]String propertyName = "")

        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

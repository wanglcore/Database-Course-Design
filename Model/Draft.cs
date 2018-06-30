using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace APP.Model
{
   public class Draft: INotifyPropertyChanged
    {
       public DateTime dateTime { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName]String propertyName = "")

        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class AnswerDraft : INotifyPropertyChanged
    {
        public DateTime dateTime { get; set; }
        public string QName { get; set; }
        public int Qid { get; set; }
        public string Content { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName]String propertyName = "")

        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

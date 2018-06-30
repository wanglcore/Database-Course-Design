using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
namespace APP.Model
{
    public class People: INotifyPropertyChanged
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Passwd { get; set; }
        public string Introduction { get; set; }
        public int Follownum { get; set; }
        public int Focusednum { get; set; }
        public int Qusitionnum { get; set; }
        public int Publishnum { get; set; }
        public int Answernum { get; set; }
        public int FQusitionnum { get; set; }
        public int FLabelnum { get; set; }
        public DateTime CreateTime { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName]String propertyName = "")

        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

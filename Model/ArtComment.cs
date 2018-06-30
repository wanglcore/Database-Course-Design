using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace APP.Model
{
    public class ArtComment:INotifyPropertyChanged
    {
        public int Articalid { get; set; }
        public int Userid { get; set; }
        public int ArtCmtUserid { get; set; }
        public int ArtCmtedUserid { get; set; }
        public DateTime ArtCmtTime { get; set; }
        public string ArtCmtContent { get; set; }
        public int UpArtCmtnum { get; set; }
        public int DownArtCmtnum { get; set; }
        public string CmdUseridName { get; set; }
        public string ArticalName { get; set; }
     
        public string UseridName { get; set; }
        public string CmtedUseridName { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName]String propertyName = "")

        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

using System;

namespace APP.Model
{
    public class Artical
    {
        public string UseridName { get; set; }
        public int Articalid { get; set; }
        public int Userid { get; set; }
        public string ArticalTitle { get; set; }
        
        public string ArticalContent { get; set; }
        public string Label0 { get; set; }
        public DateTime ArticalTime { get; set; }
        public DateTime ArticalUPTime { get; set; }
        public int UpArtnum { get; set; }
        public int DownArtnum { get; set; }
        public int ArtCmtnum { get; set; }
    }
}

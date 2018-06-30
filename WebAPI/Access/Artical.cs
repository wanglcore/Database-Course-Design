using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models
{
    public class Artical
    {
        public int Articalid { get; set; }
        public int Userid { get; set; }
        public string ArticalTitle { get; set; }
        public string ArticalBrief { get; set; }
        public string ArticalContent { get; set; }
        public DateTime ArticalTime { get; set; }
        public DateTime ArticalUPTime { get; set; }
        public int UpArtnum { get; set; }
        public int DownArtnum { get; set; }
        public int ArtCmtnum { get; set; }
    }
}

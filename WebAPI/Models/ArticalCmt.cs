using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models
{
    public class ArticalCmt
    {
        public int Articalid { get; set; }
        public int ArtCmtUserid { get; set; }
        public int ArtCmtedUserid { get; set; }
        public DateTime ArtCmtTime { get; set; }
        public string ArtCmtContent { get; set; }
        public int UpArtCmtnum { get; set; }
        public int DownArtCmtnum { get; set; }
        public string CmdUseridName { get; set; }
        public string UseridName { get; set; }
        public string CmtedUseridName { get; set; }
        public string ArticalName { get; set; }
        public int Userid { get; set; }
    }
}

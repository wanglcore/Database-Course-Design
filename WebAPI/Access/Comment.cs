using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models
{
    public class Comment
    {
        public int Qusitionid { get; set; }
        public int Userid { get; set; }
        public int CmdUserid { get; set; }
        public int CmtedUserid { get; set; }
        public string CmtContent { get; set; }
        public DateTime CmtTime { get; set; }
        public int UpCmtnum { get; set; }
    }
}

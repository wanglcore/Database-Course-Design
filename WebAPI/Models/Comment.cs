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
        public int DownCmtnum { get; set; }
        public string CmdUseridName { get; set; }
        public string UseridName { get; set; }
        public string CmtedUseridName { get; set; }
        public string QusitionName { get; set; }
    }
}

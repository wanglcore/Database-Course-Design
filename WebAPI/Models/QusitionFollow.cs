using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models
{
    public class QusitionFollow
    {
        public int Qusitionid { get; set;}
        public int Userid { get; set; }
        public DateTime FollowQTime { get; set; }
    }
}

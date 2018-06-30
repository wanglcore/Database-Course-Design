using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models
{
    public class UserFollow
    {
        public int Userid { get; set; }
        public int FollowUserid { get; set; }
        public DateTime FollowTime { get; set; }
    }
}

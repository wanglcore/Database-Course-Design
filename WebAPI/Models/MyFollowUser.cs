using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models
{
    public class MyFollowUser
    {
        public int Userid { get; set; }
        public int FollowUserid { get; set; }
        public string Name { get; set; }
        public string Introduction { get; set; }
    }
}

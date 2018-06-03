using System;

namespace APP.Model
{
    public class UserFollow
    {
        public int Userid { get; set; }
        public int FollowUserid { get; set; }
        public DateTime FollowTime { get; set; }
    }
}

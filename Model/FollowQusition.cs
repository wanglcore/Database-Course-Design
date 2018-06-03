using System;
namespace APP.Model
{
    public class FollowQusition
    {
        public int QusitionId { get; set; }
        /// <summary>
        /// 关注者id
        /// </summary>
        public int FollowUserId { get; set; }
        /// <summary>
        /// 关注时间
        /// </summary>
        public int FollowTime { get; set; }
    }
}

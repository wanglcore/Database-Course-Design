using System;

namespace APP.Model
{
    public class User_tag
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 子标签
        /// </summary>
        public string ChildLabel { get; set; }
        /// <summary>
        /// 父标签
        /// </summary>
        public string ParentLabel { get; set; }
        public DateTime CreateTime { get; set; }
    }
}

using System;

namespace APP.Model
{
    public class Answer
    {
        public int Qusitionid { get; set; }
        public int Userid { get; set; }
        public DateTime AnswerTime { get; set; }
        public DateTime UpAnsTime { get; set; }
        public string AnswerContent { get; set; }
        public int UpAnsnum { get; set; }
        public int DownAnsnum { get; set; }
        public int AnsCmtnum { get; set; }
    }
}

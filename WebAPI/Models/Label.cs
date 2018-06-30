using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models
{
    public class Label
    {
        public int Labelid { get; set; }
        public DateTime LabelCTime { get; set; }
        public string Labelname { get; set; }
        public  string LabelBrief { get; set; }
        public int LabelQnum { get; set; }
        public int LabelUnum { get; set; }
    }
}

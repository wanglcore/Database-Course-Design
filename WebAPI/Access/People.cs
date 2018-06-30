using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models
{
    public class People
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string NickName { get; set; }
        public string Passwd { get; set; }
        public string Introduction { get; set; }
        public int Follownum { get; set; }
        public DateTime CreateTime { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Web.Access;
using Web.Models;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    public class ArticalController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [Route("newartical")]
        public bool Postartical([FromBody]Models.Artical artical)
        {
            ArticalAccess articalAccess = new ArticalAccess();
            return articalAccess.PostArtical(artical);
        }
        [HttpGet]
        [Route("getallartical")]
        public string Getallartical()
        {
            ArticalAccess artical = new ArticalAccess();
            return artical.GetallArtical();
        }

        [HttpGet("getbyname/{Text}")]
        public List<Artical> GetByName(string Text)
        {
            ArticalAccess articalAccess = new ArticalAccess();
            return articalAccess.Getbyname(Text);
        }
       [HttpPut("putlike/{Articalid}/{Myid}")]
       public bool Putlike(int Articalid,int Myid)
        {
            ArticalAccess articalAccess = new ArticalAccess();
            return articalAccess.Putlike(Articalid, Myid);
        }
        [HttpPut("putdislike/{Articalid}/{Myid}")]
        public bool Putdislike(int Articalid,int Myid)
        {
            ArticalAccess articalAccess = new ArticalAccess();
            return articalAccess.PutDislike(Articalid, Myid);
        }
        [HttpPut("delputlike/{Articalid}/{Myid}")]
        public bool Delputlike(int Articalid,int Myid)
        {
            ArticalAccess articalAccess = new ArticalAccess();
            return articalAccess.DelPutlike(Articalid, Myid);
        }
        [HttpPut("delputdislike/{Articalid}/{Myid}")]
        public bool Delputdislike(int Articalid,int Myid)
        {
            ArticalAccess articalAccess = new ArticalAccess();
            return articalAccess.DelPutdislike(Articalid, Myid);
        }
        [HttpGet("getartical/{Userid}")]
        public string GetArtical(int Userid)
        {
            ArticalAccess articalAccess = new ArticalAccess();
            return articalAccess.GetMyArtical(Userid);
        }

    }
}
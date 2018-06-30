using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Web.Access;
using Web.Models;
namespace Web.Controllers
{
    [Route("api/[controller]")]
    public class LabelController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [Route("addlabel")]
        public bool Postlabel([FromBody]Label label)
        {
            Labelaccess labelaccess = new Labelaccess();
            return labelaccess.Postlabel(label);
        }
        [HttpGet]
        [Route("getlabel")]
        public string Getlabel()
        {
            Labelaccess labelaccess = new Labelaccess();
            return labelaccess.GetallLabels();
        }

        [HttpGet("isfollow/{Userid}/{labelid}")]
        public bool Isfollow(int Userid,int labelid)
        {
            Labelaccess labelaccess = new Labelaccess();
            return labelaccess.IsFollow(Userid, labelid);
        }
        [HttpGet("getbyname/{Text}")]
        public List<Label>Getbyname(string Text)
        {
            Labelaccess labelaccess = new Labelaccess();
            return labelaccess.getbyname(Text);
        }
        [HttpGet("getflabel/{id}")]
        public string Getflabel(int id)
        {
            Labelaccess labelaccess = new Labelaccess();
            return labelaccess.GetfLabels(id);
        }
        [HttpPut("FLabel/{Userid}/{Labelid}")]
        public bool PutFLabel(int Userid,int Labelid)
        {
            Labelaccess labelaccess = new Labelaccess();
            return labelaccess.PutFLabel(Labelid, Userid);
        }
        [HttpPut("UFLabel/{Userid}/{Labelid}")]
        public bool PutUFLabel(int Userid,int Labelid)
        {
            Labelaccess labelaccess = new Labelaccess();
            return labelaccess.PutUFLabel(Labelid, Userid);
        }
        [HttpGet("getlabeldetail/{labelname}")]
        public Label GetLabel(string labelname)
        {
            Labelaccess labelaccess = new Labelaccess();
            return labelaccess.Getlabel(labelname);
        }
    }
}
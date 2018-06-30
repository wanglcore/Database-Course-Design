using Microsoft.AspNetCore.Mvc;
using Web.Access;
using Web.Models;
namespace Web.Controllers
{
    [Route("api/[controller]")]
    public class CommentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [Route("addcomment")]
        public bool PostComment([FromBody]Comment comment)
        {
            CommentAccess commentAccess = new CommentAccess();
            return commentAccess.PostnewCom(comment);
        }
        [HttpGet("getcomment/{Userid}/{Qusitionid}")]
        public string GetComment(int Userid,int Qusitionid)
        {
            CommentAccess comment = new CommentAccess();
            return comment.GetComment(Userid, Qusitionid);
        }

        [HttpGet("getcomment2/{Userid}")]
        public string GetComment2(int Userid)
        {
            CommentAccess comment = new CommentAccess();
            return comment.GetComment2(Userid);
        }
        [HttpPut]
        [Route("putlike")]
        public bool PutLike([FromBody]CommentPlus commentPlus)
        {
            CommentAccess comment = new CommentAccess();
            return comment.PutLike(commentPlus);
        }
        [HttpPut]
        [Route("delputlike")]
        public bool DelPutLike([FromBody]CommentPlus commentPlus)
        {
            CommentAccess commentAccess = new CommentAccess();
            return commentAccess.DelPutlike(commentPlus);
        }
        [HttpPut]
        [Route("putdislike")]
        public bool Putdislike([FromBody]CommentPlus commentPlus)
        {
            CommentAccess commentAccess = new CommentAccess();
            return commentAccess.PutDislike(commentPlus);
        }
        [HttpPut]
        [Route("delputdislike")]
        public bool DelPutdislike([FromBody]CommentPlus commentPlus)
        {
            CommentAccess commentAccess = new CommentAccess();
            return commentAccess.DelPutDisLike(commentPlus);
        }
    }
}
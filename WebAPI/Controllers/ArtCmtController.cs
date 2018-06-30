using Microsoft.AspNetCore.Mvc;
using Web.Access;
using Web.Models;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    public class ArtCmtController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [Route("addcomment")]
        public bool PostComment([FromBody]ArticalCmt articalCmt)
        {
            ArtCommentAccess artCommentAccess = new ArtCommentAccess();
            return artCommentAccess.PostArtComment(articalCmt);
        }
        [HttpGet("getcomment/{Userid}/{Articalid}")]
        public string GetComment(int Userid, int Articalid)
        {
            ArtCommentAccess artCommentAccess = new ArtCommentAccess();
            return artCommentAccess.GetArtCmt(Userid, Articalid);
        }
        [HttpGet("getcomment2/{Userid}")]
        public string GetComment2(int Userid)
        {
            ArtCommentAccess artCommentAccess = new ArtCommentAccess();
            return artCommentAccess.GetArtCmtandName(Userid);
        }
        [HttpPut]
        [Route("putlike")]
        public bool PutLike([FromBody]ArtCommentPlus artCommentPlus)
        {
            ArtCommentAccess artCommentAccess = new ArtCommentAccess();
            return artCommentAccess.PutLike(artCommentPlus);
        }
        [HttpPut]
        [Route("putdislike")]
        public bool PutDislike([FromBody]ArtCommentPlus artCommentPlus)
        {
            ArtCommentAccess artCommentAccess = new ArtCommentAccess();
            return artCommentAccess.PutDisLike(artCommentPlus);
        }
        [HttpPut]
        [Route("delputlike")]
        public bool Delputlike([FromBody]ArtCommentPlus artCommentPlus)
        {
            ArtCommentAccess artCommentAccess = new ArtCommentAccess();
            return artCommentAccess.DelPutLike(artCommentPlus);
        }
        [HttpPut]
        [Route("delputdislike")]
        public bool Delputdislike([FromBody]ArtCommentPlus artCommentPlus)
        {
            ArtCommentAccess artCommentAccess = new ArtCommentAccess();
            return artCommentAccess.DelPutDisLike(artCommentPlus);
        }
    }
}
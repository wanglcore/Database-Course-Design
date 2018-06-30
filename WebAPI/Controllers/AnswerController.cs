using Microsoft.AspNetCore.Mvc;
using Web.Access;
using Web.Models;
namespace Web.Controllers
{
    [Route("api/[controller]")]
    public class AnswerController : Controller
    {
        [HttpPost("addanswer")]
        public bool PostAns([FromBody]Answer answer)
        {
            AnswerAccess answerAccess = new AnswerAccess();
            return answerAccess.PostnewAnswer(answer);
        }

        [HttpPost("updateanswer")]
        public bool PostUPAns([FromBody]Answer answer)
        {
            AnswerAccess answers = new AnswerAccess();
            return answers.PostUPAns(answer);
        }
        [HttpGet("getallqa/{userid}/{qusitionid}")]
        public string GetAllQA(int userid, int qusitionid)
        {
            AnswerAccess answerAccess = new AnswerAccess();
            return answerAccess.GetAllQA(userid, qusitionid);
        }
        [HttpGet("getisanswer/{Userid}/{qusitionid}")]
        public bool GetisAnswer(int Userid, int qusitionid)
        {
            AnswerAccess answerAccess = new AnswerAccess();
            return answerAccess.GetisAnswer(Userid, qusitionid);
        }
        [HttpPut("putlike/{Userid}/{Qusitionid}/{Myid}")]
        public bool PutLike(int Userid, int Qusitionid, int Myid)
        {
            AnswerAccess answerAccess = new AnswerAccess();
            return answerAccess.Putlike(Userid, Qusitionid, Myid);
        }
        [HttpPut("putdislike/{Userid}/{Qusitionid}/{Myid}")]
        public bool PutDisLike(int Userid, int Qusitionid, int Myid)
        {
            AnswerAccess answerAccess = new AnswerAccess();
            return answerAccess.DownAns(Qusitionid, Userid, Myid);
        }
        [HttpPut("delputlike/{Userid}/{Qusitionid}/{Myid}")]
        public bool DelPutLike(int Userid, int Qusitionid, int Myid)
        {
            AnswerAccess answerAccess = new AnswerAccess();
            return answerAccess.Cancellike(Qusitionid, Userid, Myid);
        }
        [HttpPut("delputdislike/{Userid}/{Qusitionid}/{Myid}")]
        public bool Delputdislike(int Userid, int Qusitionid, int Myid)
        {
            AnswerAccess answerAccess = new AnswerAccess();
            return answerAccess.CancelDislike(Userid, Qusitionid, Myid);
        }
        [HttpGet("getisUP/{Qusitionid}/{Userid}/{Myid}")]
        public IActionResult GetAction(int Qusitionid, int Userid, int Myid)
        {
            AnswerAccess answerAccess = new AnswerAccess();
            return Ok(answerAccess.GetisUP(Qusitionid, Userid, Myid));
        }

        [HttpGet("gethisanswer/{Userid}")]
        public string GetHisAnswer(int Userid)
        {
            AnswerAccess answerAccess = new AnswerAccess();
            return answerAccess.GetHisAnswer(Userid);
        }

     
    }
}
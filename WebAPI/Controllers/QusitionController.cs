using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Web.Access;
using Web.Models;
namespace Web.Controllers
{
    [Route("api/[controller]")]
    public class QusitionController : Controller
    {
        /// <summary>
        /// 提问
        /// </summary>
        /// <param name="qusition"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddQusition")]
        public bool PostQusi([FromBody]Qusition qusition)
        {
            QusitionAccess qusitionAccess = new QusitionAccess();
            return qusitionAccess.PostnewQusition(qusition);
        }
        /// <summary>
        /// 修改问题
        /// </summary>
        /// <param name="qusition"></param>
        /// <returns></returns>
        [HttpPut("updateq")]
        public bool PutUpdateQ([FromBody]Qusition qusition)
        {
            QusitionAccess qusitionaccess = new QusitionAccess();
            return qusitionaccess.PutFixQ(qusition);
        }
        [HttpGet("getbyname/{text}")]
        public List<Qusition>Getbyname(string text)
        {
            QusitionAccess qusitionAccess = new QusitionAccess();
            return qusitionAccess.GetByname(text);
        }
        /// <summary>
        /// 初始化问题列表
        /// </summary>
        /// <param name="Userid"></param>
        /// <returns></returns>
        [HttpGet("userqusition/{Userid}")]
        public string GetQusition(int Userid)
        {
            QusitionAccess qusition = new QusitionAccess();
            string s = qusition.GetUserallQusition(Userid);
            if (s == string.Empty)
            {
                return "Empty";
            }
            else
            {
                return s;
            }
        }

        /// <summary>
        /// 关注问题
        /// </summary>
        /// <param name="Userid"></param>
        /// <param name="Qusitionid"></param>
        /// <returns></returns>/
        [HttpPut("addFollow/{Userid}/{Qusitionid}")]
        public bool PutFollow(int Userid, int Qusitionid)
        {
            QusitionAccess qusitionAccess = new QusitionAccess();
            return qusitionAccess.PutFollowQu(Userid, Qusitionid);
        }
        /// <summary>
        /// 取消关注问题
        /// </summary>
        /// <param name="Userid"></param>
        /// <param name="Qusitionid"></param>
        /// <returns></returns>
        [HttpDelete("deletefollow/{Userid}/{Qusitionid}")]
        public bool DeleteFollow(int Userid, int Qusitionid)
        {
            QusitionAccess qusitionAccess = new QusitionAccess();
            return qusitionAccess.DeleteFolloeQ(Userid, Qusitionid);
        }
        /// <summary>
        /// 得到关注的问题
        /// </summary>
        /// <param name="Userid"></param>
        /// <returns></returns>
        [HttpGet("getfollow/{Userid}")]
        public string GetFollow(int Userid)
        {
            QusitionAccess qusitionAccess = new QusitionAccess();
            return qusitionAccess.GetUserFQ(Userid);
        }
        /// <summary>
        /// 一个用户的全部问题
        /// </summary>
        /// <param name="Userid"></param>
        /// <returns></returns>
        [HttpGet("allqusition/{Userid}")]
        public string Getallqusition(int Userid)
        {
            QusitionAccess qusition = new QusitionAccess();
            string s = qusition.GetallQusition(Userid);
            if (s == string.Empty)
            {
                return "empty";
            }
            else
            {
                return s;
            }
        }
        /// <summary>
        /// 一个标签下已经回答的问题
        /// </summary>
        /// <param name="localid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("labelHQ/{localid}/{labelname}")]
        public string GetLabelHQ(int localid,string labelname)
        {
            QusitionAccess qusitionAccess = new QusitionAccess();
            return qusitionAccess.GetlabelHQ(localid,labelname);
        }
        /// <summary>
        /// 一个标签下的所有未回答问题
        /// </summary>
        /// <param name="labelid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("labelUQ/{labelid}/{labelname}")]
        public string GetLabelUQ(int labelid,string labelname)
        {
            QusitionAccess qusitionAccess = new QusitionAccess();
            return qusitionAccess.GetlabelUQ(labelid,labelname);
        }

        [HttpGet("getqusition/{Qusitionid}")]
        public IActionResult GetAction(int Qusitionid)
        {
            QusitionAccess qusitionAccess = new QusitionAccess();
            return Ok(qusitionAccess.GetQusition(Qusitionid));
        }
    }
}
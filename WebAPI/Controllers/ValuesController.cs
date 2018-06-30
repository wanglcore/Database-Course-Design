using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Web.Access;
using Web.Models;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// 验证登陆
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="Passwd"></param>
        /// <returns></returns>
        [HttpGet("{Email}/{Passwd}")]
        public IActionResult Get(string Email, string Passwd)
        {
            PeopleAccess people = new PeopleAccess();
            return Ok( people.GetName(Email, Passwd));
        }
        /// <summary>
        /// 得到用户的信息
        /// </summary>
        /// <param name="Userid"></param>
        /// <returns></returns>
        [HttpGet("UserInfor/{Userid}")]
        public People GetUserInfor(int Userid)
        {
            PeopleAccess peopleAccess = new PeopleAccess();
            return peopleAccess.GetUserInfor(Userid);
        }
        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        [HttpGet("Yan/{Email}")]
        public bool Getyan(string Email)
        {
            Sendmail sendmail = new Sendmail();
            return sendmail.Sendemail(Email);
        }
        /// <summary>
        /// 验证码是否正确
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet("validyan/{Email}/{code}")]
        public bool Getvalid(string Email,string code)
        {
            Sendmail sendmail = new Sendmail();
            return sendmail.IsValidcode(code);
        }
        /// <summary>
        /// 新用户注册
        /// </summary>
        /// <param name="people"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("newpeople")]
        public int Post([FromBody]People people)
        {
            PeopleAccess peopleAccess = new PeopleAccess();
            return peopleAccess.PostnewUser(people);
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="Passwd"></param>
        /// <returns></returns>
        [HttpPut("{Email}")]
        public bool Put(string Email, [FromBody]string Passwd)
        {
            PeopleAccess people = new PeopleAccess();
            return people.UpdatePasswd(Email, Passwd);
        }
        /// <summary>
        /// 添加关注
        /// </summary>
        /// <param name="Userid"></param>
        /// <param name="Followid"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("addfollow")]
        public bool Put([FromBody]UserFollow userFollow)
        {
            UserFollowAccess userFollowAccess = new UserFollowAccess();
            return userFollowAccess.AddFollow(userFollow);
        }
        /// <summary>
        /// 取消关注
        /// </summary>
        /// <param name="Userid"></param>
        /// <param name="Followid"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("deletefollow")]
        public bool Delete([FromBody]UserFollow userFollow)
        {
            UserFollowAccess userFollowAccess = new UserFollowAccess();
            return userFollowAccess.DeleteFollow(userFollow);
        }

        [HttpGet("isfollow/{Userid}/{Myid}")]
        public int GetisFollow(int Userid,int Myid)
        {
            UserFollowAccess userFollow = new UserFollowAccess();
            return userFollow.GetisFollow(Userid, Myid);
        }       
        [HttpGet("getfollow/{Userid}")]
        public string GetMyFollow(int Userid)
        {
            UserFollowAccess userFollowAccess = new UserFollowAccess();
            return userFollowAccess.GetMyFollow(Userid);
        }
        [HttpGet("getfollowme/{Userid}")]
        public string Getfollowme(int Userid)
        {
            UserFollowAccess userFollowAccess = new UserFollowAccess();
            return userFollowAccess.GetFollowme(Userid);
        }
        /// <summary>
        /// 判断用户是否关注一个问题
        /// </summary>
        /// <param name="Userid"></param>
        /// <param name="Qusitionid"></param>
        /// <returns></returns>
        [HttpGet("getisfollow/{Userid}/{Qusitionid}")]
        public bool Getisfollow(int Userid,int Qusitionid)
        {
            QusitionAccess qusitionAccess = new QusitionAccess();
            return qusitionAccess.Getisfollow(Userid, Qusitionid);
        }
        /// <summary>
        /// 得到一个标签下的所有用户
        /// </summary>
        /// <param name="labelid"></param>
        /// <returns></returns>
        [HttpGet("labelP/{labelid}/{labelname}")]
        public string GetlabelP(int labelid,string labelname)
        {
            PeopleAccess peopleAccess = new PeopleAccess();
            return peopleAccess.GetlabelP(labelid);
        }
        [HttpGet("getbyname/{text}")]
        public List<People> Getbyname(string text)
        {
            PeopleAccess peopleAccess = new PeopleAccess();
            return peopleAccess.Getbyname(text);
        }

        [HttpGet("getalluser/{Text}")]
        public List<People> GetallUser(string text)
        {
            PeopleAccess peopleAccess = new PeopleAccess();
            return peopleAccess.GetallUser(text);
        }
        [HttpPut]
        [Route("putname")]
        public bool PutName([FromBody]NU nU)
        {
            PeopleAccess peopleAccess = new PeopleAccess();
            return peopleAccess.PutName(nU);
        }
        [HttpPut]
        [Route("putintroduction")]
        public bool PutIntro([FromBody]NU nU)
        {
            PeopleAccess peopleAccess = new PeopleAccess();
            return peopleAccess.PutIntro(nU);
        }
        [HttpDelete("{id}")]
        public bool Delete(int id)
        {
            return true;
        }

    }
}

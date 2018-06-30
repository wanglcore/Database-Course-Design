using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Newtonsoft.Json;
using Web.Models;
namespace Web.Access
{
    public class CommentAccess
    {
        private const string connectionstring = "Data Source=127.0.0.1;Initial Catalog=test;Password=10086.;user id =wangl; Enlist=true";

        /// <summary>
        /// 发布评论
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public bool PostnewCom(Comment comment)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            string sql = "insert into CommentTable(Qusitonid,Userid,CmtUserid,CmtedUserid,CmtContent,CmtTime,CmdUseridName,CmtedUseridName,UseridName)" +
                "values(@Qusitionid,@Userid,@CmtUserid,@CmtedUserid,@CmtContent,@CmtTime,@CmdUseridName,@CmtedUseridName,@UseridName);" +
                "update AnswerTable set AnsCmtnum=AnsCmtnum+1 where Qusitonid=@Qusitionid and Userid=@Userid";
            sqlConnection.Open();
            using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
            {
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                try
                {
                    sqlCommand.CommandText = sql;
                    sqlCommand.Transaction = sqlTransaction;
                    sqlCommand.Parameters.AddWithValue("@Qusitionid", comment.Qusitionid);
                    sqlCommand.Parameters.AddWithValue("@Userid", comment.Userid);
                    sqlCommand.Parameters.AddWithValue("@CmtUserid", comment.CmdUserid);
                    sqlCommand.Parameters.AddWithValue("@CmtedUserid", comment.CmtedUserid);
                    sqlCommand.Parameters.AddWithValue("@CmtContent", comment.CmtContent);
                    sqlCommand.Parameters.AddWithValue("@CmtTime", comment.CmtTime.ToString());
                    sqlCommand.Parameters.AddWithValue("@CmdUseridName", comment.CmdUseridName.ToString());
                    sqlCommand.Parameters.AddWithValue("@CmtedUseridName", comment.CmtedUseridName.ToString());
                    sqlCommand.Parameters.AddWithValue("@UseridName", comment.UseridName.ToString());
                    int row = sqlCommand.ExecuteNonQuery();
                    sqlTransaction.Commit();
                    return true;
                }
                catch (SqlException)
                {
                    sqlTransaction.Rollback();
                    return false;
                }
                finally
                {
                    sqlConnection.Close();
                }
            }
        }

        public string GetComment(int Userid, int Qusitionid)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            sqlConnection.Open();
            using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
            {

                try
                {
                    string sql = "select *from CommentTable where Userid=@Userid and Qusitonid=@Qusitionid order by CmtTime desc";
                    sqlCommand.CommandText = sql;
                    sqlCommand.Parameters.AddWithValue("@Userid", Userid);
                    sqlCommand.Parameters.AddWithValue("@Qusitionid", Qusitionid);
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter
                    {
                        SelectCommand = sqlCommand
                    };
                    DataTable dataTable = new DataTable();
                    sqlDataAdapter.Fill(dataTable);
                    List<Comment> comments = new List<Comment>();
                    Comment comment;
                    foreach (DataRow item in dataTable.Rows)
                    {
                        comment = GetCommentRow(item);
                        comments.Add(comment);
                    }
                    return JsonConvert.SerializeObject(comments);
                }
                catch (SqlException)
                {
                    return string.Empty;
                }
                finally
                {
                    sqlConnection.Close();
                }
            }
        }
        public string GetComment2(int Userid)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            sqlConnection.Open();
            using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
            {

                try
                {
                    string sql = "select QusitonTitle,CommentTable.Qusitonid,Userid,CmtUserid,CmtedUserid,CmtContent,CmtTime,UpCmtnum,CmdUseridName,CmtedUseridName,UseridName,DownCmtnum from CommentTable inner join QusitonTable on CmtedUserid=@Userid and CommentTable.Qusitonid=QusitonTable.Qusitonid order by CmtTime desc";
                    sqlCommand.CommandText = sql;
                    sqlCommand.Parameters.AddWithValue("@Userid", Userid);
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter
                    {
                        SelectCommand = sqlCommand
                    };
                    DataTable dataTable = new DataTable();
                    sqlDataAdapter.Fill(dataTable);
                    List<Comment> comments = new List<Comment>();
                    Comment comment;
                    foreach (DataRow item in dataTable.Rows)
                    {
                        comment = GetCommentRow(item);
                        comments.Add(comment);
                    }
                    return JsonConvert.SerializeObject(comments);
                }
                catch (SqlException)
                {
                    return string.Empty;
                }
                finally
                {
                    sqlConnection.Close();
                }
            }
        }
        /// <summary>
        /// 赞同评论
        /// </summary>
        /// <param name="Qusitionid"></param>
        /// <param name="Userid"></param>
        /// <param name="CmdUserid"></param>
        /// <param name="CmtedUserid"></param>
        /// <returns></returns>
        public bool PutLike(CommentPlus commentPlus)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            string sql = "update CommentTable set UpCmtnum=UpCmtnum+1 where Qusitonid=@Qusitionid and Userid=@Userid and CmtUserid =@CmtUserid and CmtedUserid=@CmtedUserid and CmtTime=@datetime";
            sqlConnection.Open();
            using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
            {
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                try
                {
                    sqlCommand.CommandText = sql;
                    sqlCommand.Transaction = sqlTransaction;
                    sqlCommand.Parameters.AddWithValue("@Qusitionid", commentPlus.Qusitionid);
                    sqlCommand.Parameters.AddWithValue("@Userid", commentPlus.Userid);
                    sqlCommand.Parameters.AddWithValue("@CmtUserid", commentPlus.CmdUserid);
                    sqlCommand.Parameters.AddWithValue("@CmtedUserid", commentPlus.CmtedUserid);
                    sqlCommand.Parameters.AddWithValue("@datetime", commentPlus.Datetime);
                    int row = sqlCommand.ExecuteNonQuery();
                    sqlTransaction.Commit();
                    return true;
                }
                catch (SqlException)
                {
                    sqlTransaction.Rollback();
                    return false;
                }
                finally
                {
                    sqlConnection.Close();
                }
            }
        }
        /// <summary>
        /// 取消对文件的赞同
        /// </summary>
        /// <param name="Qusitionid"></param>
        /// <param name="Userid"></param>
        /// <param name="CmdUserid"></param>
        /// <param name="CmtedUserid"></param>
        /// <returns></returns>
        public bool DelPutlike(CommentPlus commentPlus)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            sqlConnection.Open();
            using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
            {
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                try
                {
                    string sql = "update CommentTable set UpCmtnum =UpCmtnum-1 where Qusitonid=@Qusitionid and Userid=@Userid and CmtUserid=@CmtUserid and CmtedUserid=@CmtedUserid and CmtTime=@datetime";
                    sqlCommand.CommandText = sql;
                    sqlCommand.Parameters.AddWithValue("@Qusitionid", commentPlus.Qusitionid);
                    sqlCommand.Parameters.AddWithValue("@Userid", commentPlus.Userid);
                    sqlCommand.Parameters.AddWithValue("@CmtUserid", commentPlus.CmtedUserid);
                    sqlCommand.Parameters.AddWithValue("@CmtedUserid", commentPlus.CmtedUserid);
                    sqlCommand.Parameters.AddWithValue("@datetime", commentPlus.Datetime);
                    sqlCommand.Transaction = sqlTransaction;
                    sqlCommand.ExecuteNonQuery();
                    sqlTransaction.Commit();
                    return true;
                }
                catch (SqlException)
                {
                    sqlTransaction.Rollback();
                    return false;

                }
                finally
                {
                    sqlConnection.Close();
                }
            }
        }
        /// <summary>
        /// 对评论不赞同
        /// </summary>
        /// <param name="Qusitionid"></param>
        /// <param name="Userid"></param>
        /// <param name="CmdUserid"></param>
        /// <param name="CmtedUserid"></param>
        /// <returns></returns>
        public bool PutDislike(CommentPlus commentPlus)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            sqlConnection.Open();
            using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
            {
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                try
                {
                    string sql = "update CommentTable set DownCmtnum=DownCmtnum+1 where Qusitonid=@Qusitionid and Userid =@Userid and CmtUserid=@CmtUserid and CmtedUserid=@CmtedUserid and CmtTime=@datetime";
                    sqlCommand.CommandText = sql;
                    sqlCommand.Transaction = sqlTransaction;
                    sqlCommand.Parameters.AddWithValue("@Qusitionid", commentPlus.Qusitionid);
                    sqlCommand.Parameters.AddWithValue("@Userid", commentPlus.Userid);
                    sqlCommand.Parameters.AddWithValue("@CmtedUserid", commentPlus.CmtedUserid);
                    sqlCommand.Parameters.AddWithValue("@CmtUserid", commentPlus.CmtedUserid);
                    sqlCommand.Parameters.AddWithValue("@datetime", commentPlus.Datetime);
                    sqlCommand.ExecuteNonQuery();
                    sqlTransaction.Commit();
                    return true;
                }
                catch (SqlException)
                {
                    sqlTransaction.Rollback();
                    return false;
                }
                finally
                {
                    sqlConnection.Close();
                }
            }
        }
        /// <summary>
        /// 取消对评论的不赞同
        /// </summary>
        /// <param name="Qusitionid"></param>
        /// <param name="Userid"></param>
        /// <param name="CmdUserid"></param>
        /// <param name="CmtedUserid"></param>
        /// <returns></returns>
        public bool DelPutDisLike(CommentPlus commentPlus)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            sqlConnection.Open();
            using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
            {
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                try
                {
                    string sql = "update CommentTable set DownCmtnum=DownCmtnum-1 where Qusitonid=@Qusitionid and Userid=@Userid and CmtUserid=@CmtUserid and CmtedUserid=@CmtedUserid and CmtTime=@datetime";
                    sqlCommand.CommandText = sql;
                    sqlCommand.Parameters.AddWithValue("@Qusitionid", commentPlus.Qusitionid);
                    sqlCommand.Parameters.AddWithValue("@Userid", commentPlus.Userid);
                    sqlCommand.Parameters.AddWithValue("@CmtUserid", commentPlus.CmtedUserid);
                    sqlCommand.Parameters.AddWithValue("@CmtedUserid", commentPlus.CmtedUserid);
                    sqlCommand.Parameters.AddWithValue("@datetime", commentPlus.Datetime);
                    sqlCommand.Transaction = sqlTransaction;
                    sqlCommand.ExecuteNonQuery();
                    sqlTransaction.Commit();
                    return true;
                }
                catch (SqlException)
                {

                    sqlTransaction.Rollback();
                    return false;
                }
                finally
                {
                    sqlConnection.Close();
                }
            }
        }
        private Comment GetCommentRow(DataRow dataRow)
        {
            Comment comment = new Comment
            {
                Qusitionid = Convert.ToInt32(dataRow["Qusitonid"]),
                Userid = Convert.ToInt32(dataRow["Userid"]),
                CmdUserid = Convert.ToInt32(dataRow["CmtUserid"]),
                CmtedUserid = Convert.ToInt32(dataRow["CmtedUserid"]),
                CmtContent = dataRow["CmtContent"].ToString(),
                CmtTime = Convert.ToDateTime(dataRow["CmtTime"]),
                UpCmtnum = Convert.ToInt32(dataRow["UpCmtnum"]),
                DownCmtnum = Convert.ToInt32(dataRow["DownCmtnum"]),
                CmtedUseridName = dataRow["CmtedUseridName"].ToString(),
                CmdUseridName = dataRow["CmdUseridName"].ToString(),
                UseridName = dataRow["UseridName"].ToString(),
            };
            return comment;
        }


        private Comment GetCommentRow2(DataRow dataRow)
        {
            Comment comment = new Comment
            {
                Qusitionid = Convert.ToInt32(dataRow["Qusitonid"]),
                Userid = Convert.ToInt32(dataRow["Userid"]),
                CmdUserid = Convert.ToInt32(dataRow["CmtUserid"]),
                CmtedUserid = Convert.ToInt32(dataRow["CmtedUserid"]),
                CmtContent = dataRow["CmtContent"].ToString(),
                CmtTime = Convert.ToDateTime(dataRow["CmtTime"]),
                UpCmtnum = Convert.ToInt32(dataRow["UpCmtnum"]),
                DownCmtnum = Convert.ToInt32(dataRow["DownCmtnum"]),
                CmtedUseridName = dataRow["CmtedUseridName"].ToString(),
                CmdUseridName = dataRow["CmdUseridName"].ToString(),
                UseridName = dataRow["UseridName"].ToString(),
                QusitionName = dataRow["QusitonTitle"].ToString()
            };
            return comment;
        }

    }
}

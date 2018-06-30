using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Transactions;
using System.Data.SqlClient;
using Web.Models;
namespace Web.Access
{
    public class AnswerAccess
    {
        private const string connectionstring = "Data Source=127.0.0.1;Initial Catalog=test;Password=10086.;user id =wangl; Enlist=true";
        /// <summary>
        /// 新回答
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public bool PostnewAnswer(Models.Answer answer)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            string datetime = DateTime.Now.ToString();
            string PutAns = "insert into AnswerTable (Qusitonid,Userid,AnswerTime,UpAnsTime,AnswerContent)values" +
                "(@Qusitonid,@Userid,@AnswerTime,@UpAnsTime,@AnswerContent);" +
                "update UserTable set Answernum =Answernum+1 " +
                    "where Userid = @Userid;update QusitonTable set Answerednum =Answerednum+1 " +
                    "where Qusitonid = @Qusitonid";
            sqlConnection.Open();
            using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
            {
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                try
                {
                    sqlCommand.CommandText = PutAns;
                    sqlCommand.Transaction = sqlTransaction;
                    sqlCommand.Parameters.AddWithValue("@Qusitonid", answer.Qusitionid);
                    sqlCommand.Parameters.AddWithValue("@Userid", answer.Userid);
                    sqlCommand.Parameters.AddWithValue("@AnswerTime", datetime);
                    sqlCommand.Parameters.AddWithValue("@UpAnsTime", datetime);
                    sqlCommand.Parameters.AddWithValue("@AnswerContent", answer.AnswerContent);
                    int rows = sqlCommand.ExecuteNonQuery();
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
        /// 更新答案
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public bool PostUPAns(Answer answer)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            string datetime = DateTime.Now.ToString();
            string Upsql = "Update AnswerTable set AnswerContent = @AnswerContent " +
                "where Qusitonid=@Qusitionid and Userid = @Userid";
            sqlConnection.Open();
            using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
            {
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                try
                {
                    sqlCommand.Parameters.Clear();
                    sqlCommand.CommandText = Upsql;
                    sqlCommand.Transaction = sqlTransaction;
                    sqlCommand.Parameters.AddWithValue("@AnswerContent", answer.AnswerContent);
                    sqlCommand.Parameters.AddWithValue("@Qusitionid", answer.Qusitionid);
                    sqlCommand.Parameters.AddWithValue("@Userid", answer.Userid);
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
        ///赞同回答
        /// </summary>
        /// <param name="Qusitionid"></param>
        /// <param name="Userid"></param>
        /// <returns></returns>
        public bool Putupans(int Qusitionid, int Userid)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            string sql = "update AnswerTable set UpAnsnum = UpAnsnum + 1 " +
                "where Qusitonid=@Qusitionid and Userid=@Userid";
            sqlConnection.Open();
            using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
            {
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                try
                {
                    sqlCommand.Transaction = sqlTransaction;
                    sqlCommand.CommandText = sql;
                    sqlCommand.Parameters.AddWithValue("@Qusitionid", Qusitionid);
                    sqlCommand.Parameters.AddWithValue("@Userid", Userid);
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
        /// 反对答案
        /// </summary>
        /// <param name="Qusitionid"></param>
        /// <param name="Userid"></param>
        /// <returns></returns>
        public bool DownAns(int Qusitionid, int Userid, int Myid)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);

            string sql = "update AnswerTable set DownAnsnum = DownAnsnum + 1 " +
                "where Qusitonid=@Qusitionid and Userid=@Userid";
            sqlConnection.Open();
            using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
            {
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                try
                {
                    sqlCommand.CommandText = sql;
                    sqlCommand.Transaction = sqlTransaction;
                    sqlCommand.Parameters.AddWithValue("@Qusitionid", Qusitionid);
                    sqlCommand.Parameters.AddWithValue("@Userid", Userid);
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

        public string GetAllQA(int Userid, int Qusitionid)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            sqlConnection.Open();
            try
            {
                string str = "select AnswerTable.Qusitonid,AnswerTable.Userid," +
                    "AnswerTime,UpAnsTime,AnswerContent,UpAnsnum,DownAnsnum,AnsCmtnum,Name from AnswerTable inner join UserTable on Qusitonid=@Qusitionid and AnswerTable.Userid=@Userid and AnswerTable.Userid = UserTable.Userid order by UpAnsTime desc";
                SqlCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandText = str;
                sqlCommand.Parameters.AddWithValue("@Qusitionid", Qusitionid);
                sqlCommand.Parameters.AddWithValue("@Userid", Userid);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter
                {
                    SelectCommand = sqlCommand
                };
                DataTable dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);
                List<Answershow> answershows = new List<Answershow>();
                Answershow answershow;
                foreach (DataRow item in dataTable.Rows)
                {
                    answershow = GetAnswershow(item);
                    answershows.Add(answershow);
                }
                str = "select AnswerTable.Qusitonid,AnswerTable.Userid,AnswerTime,UpAnsTime,AnswerContent,UpAnsnum,DownAnsnum,AnsCmtnum,Name from AnswerTable inner join UserTable on Qusitonid=@Qusitionid and AnswerTable.Userid<>@Userid and AnswerTable.Userid =UserTable.Userid order by UpAnsTime desc";
                sqlCommand.Parameters.Clear();
                sqlCommand.CommandText = str;
                sqlCommand.Parameters.AddWithValue("@Qusitionid", Qusitionid);
                sqlCommand.Parameters.AddWithValue("@Userid", Userid);
                SqlDataAdapter sqlData = new SqlDataAdapter
                {
                    SelectCommand = sqlCommand
                };
                DataTable table = new DataTable();
                sqlData.Fill(table);
                foreach (DataRow item in table.Rows)
                {
                    answershow = GetAnswershow(item);
                    answershows.Add(answershow);
                }
                return JsonConvert.SerializeObject(answershows);
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
        /// <summary>
        /// 得到一个人的回答
        /// </summary>
        /// <param name="Userid"></param>
        /// <returns></returns>
        public string GetHisAnswer(int Userid)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            sqlConnection.Open();
            using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
            {
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                try
                {
                    string sql = "select AnswerTable.Qusitonid,AnswerTable.Userid,AnswerTime,UpAnsTime,AnswerContent,UpAnsnum,DownAnsnum,AnsCmtnum,QusitonTitle from AnswerTable inner join QusitonTable on Userid=@Userid and AnswerTable.Qusitonid=QusitonTable.Qusitonid order by UpAnsTime desc";
                    sqlCommand.CommandText = sql;
                    sqlCommand.Transaction = sqlTransaction;
                    sqlCommand.Parameters.AddWithValue("@Userid", Userid);
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter
                    {
                        SelectCommand = sqlCommand
                    };
                    DataTable dataTable = new DataTable();
                    sqlDataAdapter.Fill(dataTable);
                    List<Answershow> answershows = new List<Answershow>();
                    Answershow answershow;
                    foreach (DataRow item in dataTable.Rows)
                    {
                        answershow = GetHisAnswershow(item);
                        answershows.Add(answershow);
                    }
                    return JsonConvert.SerializeObject(answershows);

                }
                catch (SqlException)
                {
                    sqlTransaction.Rollback();
                    return string.Empty;
                }
                finally
                {
                    sqlConnection.Close();
                }
            }
        }
        //// 判断一个人是否回答过
        public bool GetisAnswer(int Userid, int Qusitionid)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            sqlConnection.Open();
            try
            {
                using (SqlCommand sqlcommend = sqlConnection.CreateCommand())
                {
                    string sql = "select Userid from AnswerTable where Qusitonid =@Qusitionid and Userid=@Userid";
                    sqlcommend.CommandText = sql;
                    sqlcommend.Parameters.AddWithValue("@Qusitionid", Qusitionid);
                    sqlcommend.Parameters.AddWithValue("@Userid", Userid);
                    if (sqlcommend.ExecuteScalar() == null)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            catch (SqlException)
            {
                return false;
            }
            finally
            {
                sqlConnection.Close();
            }
        }
        /// <summary>
        /// 赞同回答
        /// </summary>
        /// <param name="Userid"></param>
        /// <param name="Qusitionid"></param>
        /// <param name="Myid"></param>
        /// <returns></returns>
        public bool Putlike(int Userid, int Qusitionid, int Myid)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            sqlConnection.Open();
            using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
            {
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                try
                {
                    string sql = "update AnswerTable set UpAnsnum =UpAnsnum +1 where Qusitonid=@Qusitionid and Userid =@Userid";
                    sqlCommand.CommandText = sql;
                    sqlCommand.Parameters.AddWithValue("@Qusitionid", Qusitionid);
                    sqlCommand.Parameters.AddWithValue("@Userid", Userid);
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

        public bool PutDisLike(int Userid, int Qusitionid, int Myid)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            sqlConnection.Open();
            using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
            {
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                try
                {
                    string sql = "update AnswerTable set DownAnsnum=DownAnsnum+1 where Qusitonid=@Qusitionid and Userid =@Userid ";
                    sqlCommand.CommandText = sql;
                    sqlCommand.Parameters.AddWithValue("@Qusitionid", Qusitionid);
                    sqlCommand.Parameters.AddWithValue("@Userid", Userid);
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

        public bool GetisUP(int Qusitionid, int Userid, int Myid)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            sqlConnection.Open();
            using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
            {
                try
                {
                    string sql = "select Qusitionid from Tabel where Qusitionid=@Qusitionid and Userid=@Userid and UPid=@Myid";
                    sqlCommand.CommandText = sql;
                    sqlCommand.Parameters.AddWithValue("@Qusitionid", Qusitionid);
                    if (sqlCommand.ExecuteScalar() != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (SqlException)
                {

                    return false;
                }
                finally
                {
                    sqlConnection.Close();
                }
            }
        }
        /// <summary>
        /// 取消对回答的关注
        /// </summary>
        /// <param name="Qusitionid"></param>
        /// <param name="Userid"></param>
        /// <param name="Myid"></param>
        /// <returns></returns>
        public bool Cancellike(int Qusitionid, int Userid, int Myid)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            sqlConnection.Open();
            using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
            {
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                try
                {
                    string sql = "update AnswerTable set UpAnsnum =UpAnsnum-1 where Qusitonid =@Qusitionid and Userid=@Userid";
                    sqlCommand.CommandText = sql;
                    sqlCommand.Transaction = sqlTransaction;
                    sqlCommand.Parameters.AddWithValue("@Userid", Userid);
                    sqlCommand.Parameters.AddWithValue("@Qusitionid", Qusitionid);
                    sqlCommand.ExecuteNonQuery();
                    sqlTransaction.Commit();
                    return true;

                }
                catch (SqlException)
                {
                    sqlTransaction.Rollback();
                    throw;
                }
                finally
                {
                    sqlConnection.Close();
                }
            }
        }
        /// <summary>
        /// 取消对答案的不赞同
        /// </summary>
        /// <param name="Userid"></param>
        /// <param name="Qusitionid"></param>
        /// <param name="Myid"></param>
        /// <returns></returns>
        public bool CancelDislike(int Userid, int Qusitionid, int Myid)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            sqlConnection.Open();
            using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
            {
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                try
                {
                    string sql = "update AnswerTable set DownAnsnum =DownAnsnum-1 where Qusitonid =@Qusitionid and Userid =@Userid";
                    sqlCommand.CommandText = sql;
                    sqlCommand.Transaction = sqlTransaction;
                    sqlCommand.Parameters.AddWithValue("@Qusitionid", Qusitionid);
                    sqlCommand.Parameters.AddWithValue("@Userid", Userid);
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


        private Answershow GetAnswershow(DataRow item)
        {
            Answershow answershow = new Answershow
            {
                Qusitionid = Convert.ToInt32(item["Qusitonid"]),
                Userid = Convert.ToInt32(item["Userid"]),
                AnswerTime = Convert.ToDateTime(item["AnswerTime"]),
                UpAnsTime = Convert.ToDateTime(item["UpAnsTime"]),
                AnswerContent = item["AnswerContent"].ToString(),
                UpAnsnum = Convert.ToInt32(item["UpAnsnum"]),
                DownAnsnum = Convert.ToInt32(item["DownAnsnum"]),
                AnsCmtnum = Convert.ToInt32(item["AnsCmtnum"]),
                Name = item["Name"].ToString(),
            };
            return answershow;
        }
        private Answershow GetHisAnswershow(DataRow item)
        {
            Answershow answershow = new Answershow
            {
                Qusitionid = Convert.ToInt32(item["Qusitonid"]),
                Userid = Convert.ToInt32(item["Userid"]),
                AnswerTime = Convert.ToDateTime(item["AnswerTime"]),
                UpAnsTime = Convert.ToDateTime(item["UpAnsTime"]),
                AnswerContent = item["AnswerContent"].ToString(),
                UpAnsnum = Convert.ToInt32(item["UpAnsnum"]),
                DownAnsnum = Convert.ToInt32(item["DownAnsnum"]),
                AnsCmtnum = Convert.ToInt32(item["AnsCmtnum"]),
                QName = item["QusitonTitle"].ToString()

            };
            return answershow;
        }
    }
}

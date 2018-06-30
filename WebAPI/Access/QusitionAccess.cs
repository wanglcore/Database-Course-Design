using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Web.Models;
namespace Web.Access
{
    public class QusitionAccess
    {
        private const string connectionstring = "Data Source=127.0.0.1;Initial Catalog=test;Password=10086.;user id =wangl; Enlist=true";
     
        /// <summary>
        /// 用户关注问题
        /// </summary>
        /// <param name="Userid"></param>
        /// <param name="Qusitionid"></param>
        /// <returns></returns>
        public bool PutFollowQu(int Userid, int Qusitionid)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            string insertstr = "insert into QusitonFollowTable(Qusitionid,Userid,FollowQTime) values(@Qusitionid,@Userid,@FollowQTime);" +
                "update QusitonTable set Followednum =Followednum+1 where Qusitonid = @Qusitionid";
            string datetime = DateTime.Now.ToString();
            sqlConnection.Open();
            using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
            {
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                try
                {
                    sqlCommand.CommandText = insertstr;
                    sqlCommand.Transaction = sqlTransaction;
                    sqlCommand.Parameters.Add(new SqlParameter("@Qusitionid", Qusitionid));
                    sqlCommand.Parameters.Add(new SqlParameter("@Userid", Userid));
                    sqlCommand.Parameters.Add(new SqlParameter("@FollowQTime", datetime));
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
        /// 用户取消对问题的关注
        /// </summary>
        /// <param name="Userid"></param>
        /// <param name="Qusitionid"></param>
        /// <returns></returns>
        public bool DeleteFolloeQ(int Userid, int Qusitionid)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);

                    string deletestr = "delete from QusitonFollowTable where Qusitionid=@Qusitionid and Userid=@Userid;update QusitonTable set Followednum = Followednum - 1 " +
                            " where Qusitonid=@Qusitionid";
            sqlConnection.Open();
            using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
            {
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                try
                {
                    sqlCommand.Transaction = sqlTransaction;
                    sqlCommand.CommandText = deletestr;
                    sqlCommand.Parameters.AddWithValue("@Qusitionid", Qusitionid);
                    sqlCommand.Parameters.AddWithValue("@Userid", Userid);
                    int t = sqlCommand.ExecuteNonQuery();
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

        public Qusition GetQusition(int Qusitionid)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            sqlConnection.Open();
            using(SqlCommand sqlCommand = sqlConnection.CreateCommand())
            {
                try
                {
                    string sql = "select * from QusitonTable where Qusitonid=@Qusitonid";
                    sqlCommand.CommandText = sql;
                    sqlCommand.Parameters.AddWithValue("@Qusitonid",Qusitionid);
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter
                    {
                        SelectCommand = sqlCommand,
                    };
                    DataTable dataTable = new DataTable();
                    sqlDataAdapter.Fill(dataTable);
                    Qusition qusition=new Qusition();
                    foreach (DataRow item in dataTable.Rows)
                    {
                        qusition = GetQusitionRow(item);
                    }
                    return qusition;
                }
                catch (SqlException)
                {

                    return null;
                }
                finally
                {
                    sqlConnection.Close();
                }
            }
        }

        public List<Qusition>GetByname(string Text)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            sqlConnection.Open();
            using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
            {
                try
                {
                    string sql = $"select * from QusitonTable where QusitonTitle like '%{Text}%'";
                    sqlCommand.CommandText = sql;
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter
                    {
                        SelectCommand = sqlCommand,
                    };
                    DataTable dataTable = new DataTable();
                    sqlDataAdapter.Fill(dataTable);
                    if (dataTable.Rows.Count == 0)
                    {
                        return null;
                    }
                    else
                    {
                        List<Qusition> qusitions=new List<Qusition>();
                        Qusition qusition;
                        foreach (DataRow item in dataTable.Rows)
                        {
                            qusition = GetQusitionRow(item);
                            qusitions.Add(qusition);
                        }
                        return qusitions;
                    }
                }
                catch (SqlException)
                {

                    return null;
                }
                finally
                {
                    sqlConnection.Close();
                }
            }
        }
        /// <summary>
        /// 用户提出问题
        /// </summary>
        /// <param name="Body"></param>
        /// <returns></returns>
        public bool PostnewQusition(Qusition qusition)
        {
            
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
           
                    string PostQ = "insert into QusitonTable (Askerid,AskTime,UpAskTime,QusitonTitle" +
                        ",QusitonContent,QLabel0,QLabel1,QLabel2) values(@Askerid,@AskTime,@UpAskTime,@QusitionTitle,@QusitionContent,@QLabel0,@QLabel1,@QLabel2);update UserTable set Qusitionnum = Qusitionnum+1 where Userid =@Userid;update LabelTable set LabelQnum=LabelQnum+1 where QLabel0=@Qlabel0"
                        ;
                sqlConnection.Open();
            using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
            {
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                try
                {
                    sqlCommand.CommandText = PostQ;
                    sqlCommand.Transaction = sqlTransaction;
                    sqlCommand.Parameters.AddWithValue("@Askerid", qusition.Askerid);
                    sqlCommand.Parameters.AddWithValue("@AskTime", qusition.AskTime.ToString());
                    sqlCommand.Parameters.AddWithValue("@UpAskTime", qusition.UpAskTime.ToString());
                    sqlCommand.Parameters.AddWithValue("@QusitionTitle", qusition.QusitionTitle);
                    sqlCommand.Parameters.AddWithValue("@QusitionContent", qusition.QusitionContent);
                    sqlCommand.Parameters.AddWithValue("@QLabel0", qusition.Qlabel0);
                    sqlCommand.Parameters.AddWithValue("@QLabel1", qusition.Qlabel1);
                    sqlCommand.Parameters.AddWithValue("@QLabel2", qusition.Qlabel2);
                    sqlCommand.Parameters.AddWithValue("@Userid", qusition.Askerid);
                    int rows = sqlCommand.ExecuteNonQuery();
                    PostQ = "update LabelTable set LabelQnum=LabelQnum+1 where QLabel0=@Qlabel0";
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
        /// 修改问题
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public bool PutFixQ(Models.Qusition qusition)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
           
                string sql = "update QusitonTable set UpAskTime = @UpAskTime where Qusitonid=@Qusitonid;update QusitonTable set QusitonContent =@QusitonContent where Qusitonid=@Qusitonid";
                sqlConnection.Open();
            using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
            {
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                try
                {
                    sqlCommand.CommandText = sql;
                    sqlCommand.Transaction = sqlTransaction;
                    sqlCommand.Parameters.AddWithValue("@UpAskTime", qusition.UpAskTime);
                    sqlCommand.Parameters.AddWithValue("@Qusitonid", qusition.Qusitionid);
                    sqlCommand.Parameters.AddWithValue("@QusitonContent", qusition.QusitionContent);
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
        /// 初始化列表
        /// </summary>
        /// <param name="Userid"></param>
        /// <returns></returns>
        public string GetallQusition(int Userid)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
                string sql = "select QusitonTable.Qusitonid,Askerid,AskTime,UpAskTime,QusitonTitle" +
                    ",QusitonContent,Answerednum,Followednum,QLabel0,QLabel1,QLabel2 " +
                    "from QusitonTable order by AskTime desc";
            sqlConnection.Open();
            using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
            {
               
                try
                {
                    sqlCommand.CommandText = sql;
                   
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                    sqlDataAdapter.SelectCommand = sqlCommand;
                    DataTable dataTable = new DataTable();
                    sqlDataAdapter.Fill(dataTable);
                    List<Qusition> qusitions = new List<Qusition>();
                    Qusition qusition;
                    foreach (DataRow item in dataTable.Rows)
                    {
                        qusition = GetQusitionRow(item);
                        qusitions.Add(qusition);
                    }
                    return JsonConvert.SerializeObject(qusitions);
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
        /// 得到一个人的提问所有问题
        /// </summary>
        /// <param name="Userid"></param>
        /// <returns></returns>
        public string GetUserallQusition(int Userid)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            sqlConnection.Open();
            try
            {
                string sql = "select Qusitonid,Askerid,AskTime,UpAskTime,QusitonTitle" +
                    ",QusitonContent,Answerednum,Followednum,QLabel0,QLabel1,QLabel2 " +
                    "from QusitonTable where Askerid= @Userid order by AskTime DESC";
                SqlCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandText = sql;
                sqlCommand.Parameters.AddWithValue("@Userid", Userid);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                sqlDataAdapter.SelectCommand = sqlCommand;
                DataTable dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);
                List<Qusition> qusitions = new List<Qusition>();
                Qusition qusition;
                foreach (DataRow item in dataTable.Rows)
                {
                    qusition = new Qusition();
                    qusition = GetQusitionRow(item);
                    qusitions.Add(qusition);
                }
                return JsonConvert.SerializeObject(qusitions);
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
        /// 得到某个人关注的全部问题
        /// </summary>
        /// <param name="Userid"></param>
        /// <returns></returns>
        public string GetUserFQ(int Userid)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            sqlConnection.Open();
            try
            {
                string sql = "select Qusitonid,FollowQTime,UpAskTime," +
                    " Askerid,AskTime,QusitonTitle,QusitonContent,Answerednum,Followednum,QLabel0,QLabel1,QLabel2 " +
                    "from QusitonFollowTable inner join QusitonTable" +
                    " on QusitonFollowTable.Userid=@Userid and QusitonFollowTable.Qusitionid = QusitonTable.Qusitonid and QusitonFollowTable.Qusitionid not in " +
                    "(select Qusitonid from AnswerTable where AnswerTable.Userid = @LUserid) ORDER BY FollowQTime desc";
                SqlCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandText = sql;
                sqlCommand.Parameters.AddWithValue("@Userid", Userid);
                sqlCommand.Parameters.AddWithValue("@LUserid", Userid);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter
                {
                    SelectCommand = sqlCommand
                };
                DataTable dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);
                List<GetUserFollow> qusitions = new List<GetUserFollow>();
                GetUserFollow getUserFollow;
                foreach (DataRow item in dataTable.Rows)
                {
                    getUserFollow = new GetUserFollow();
                    getUserFollow = GetGetUserFollow(item);
                    qusitions.Add(getUserFollow);
                }
                return JsonConvert.SerializeObject(qusitions);
                
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
        /// 当前还未使用,和上一个函数重复
        /// </summary>
        /// <param name="Userid"></param>
        /// <returns></returns>
        public string Getfqanswer(int Userid)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            sqlConnection.Open();
            try
            {
                string sql = "select Qusitonid,FollowQTime,UpAskTime," +
                    " Askerid,AskTime,QusitonTitle,QusitonContent,Answerednum,Followednum,QLabel0,QLabel1,QLabel2 " +
                    " from QusitonFollowTable inner join QusitonTable" +
                    " on QusitonFollowTable.Userid=@Userid and QusitonFollowTable.Qusitionid = QusitonTable.Qusitonid and QusitonFollowTable.Qusitionid in " +
                    "(select Qusitonid from AnswerTable where AnswerTable.Userid = @LUserid)";
                SqlCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandText = sql;
                sqlCommand.Parameters.AddWithValue("@Userid", Userid);
                sqlCommand.Parameters.AddWithValue("@LUserid", Userid);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                sqlDataAdapter.SelectCommand = sqlCommand;
                DataTable dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);
                List<GetUserFollow> qusitions = new List<GetUserFollow>();
                GetUserFollow getUserFollow;
                foreach (DataRow item in dataTable.Rows)
                {
                    getUserFollow = new GetUserFollow();
                    getUserFollow = GetGetUserFollow(item);
                    qusitions.Add(getUserFollow);
                }
                return JsonConvert.SerializeObject(qusitions);
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
        /// 得到某个人回答的全部问题
        /// </summary>
        /// <param name="Userid"></param>
        /// <returns></returns>
        public string GetUserAQ(int Userid)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            sqlConnection.Open();
            try
            {
                string sql = "select AnswerTable.Qusitonid, AnswerTime,UpAnsTime" +
                    " AnswerContent,UpAnsnum,DownAnsnum,AnsCmtnum,QusitonTable.QusitionTitle" +
                    " from AnswerTable inner join QusitonTable on" +
                    " AnswerTable.Qusitonid = QusitonTable.Qusitonid and AnswerTable.Userid=@Userid order by UpAnsnum desc";
                SqlCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandText = sql;
                sqlCommand.Parameters.AddWithValue("@Userid", Userid);
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                return JsonConvert.SerializeObject(sqlDataReader);

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
        /// 判断一个用户是否关注问题
        /// </summary>
        /// <param name="Userid"></param>
        /// <param name="Qusitionid"></param>
        /// <returns></returns>
        public bool Getisfollow(int Userid,int Qusitionid)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            sqlConnection.Open();
            try
            {
                string sql = "select Userid from QusitonFollowTable where Userid=@Userid and Qusitionid=@Qusitonid";
                SqlCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandText = sql;
                sqlCommand.Parameters.AddWithValue("@Userid", Userid);
                sqlCommand.Parameters.AddWithValue("@Qusitonid", Qusitionid);
                if (Convert.ToInt32(sqlCommand.ExecuteScalar()) == Userid)
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

        /// <summary>
        /// 得到一个标签下的所有已回答的问题
        /// </summary>
        /// <param name="labelid"></param>
        /// <returns></returns>
        public string GetlabelHQ(int labelid,string labelname)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            sqlConnection.Open();
            try
            {

                string sql = "select Qusitonid,Askerid,AskTime,UpAskTime,QusitonTitle,QusitonContent,Answerednum,Followednum,QLabel0,QLabel1,QLabel2 " +
                    " from QusitonTable inner join LabelTable on Labelid =@labelid and Answerednum >=1 and Labelname=QLabel0";
                SqlCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandText = sql;
                sqlCommand.Parameters.AddWithValue("@labelid", labelid);
                SqlDataAdapter dataAdapter = new SqlDataAdapter
                {
                    SelectCommand = sqlCommand
                };
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                List<Qusition> qusitions = new List<Qusition>();
                Qusition qusition;
                foreach (DataRow item in dataTable.Rows)
                {
                    qusition = GetQusitionRow(item);
                    qusitions.Add(qusition);
                }
                return JsonConvert.SerializeObject(qusitions);
            }catch(SqlException){
                return string.Empty;
            }
            finally
            {
                sqlConnection.Close();
            }
        }
        /// <summary>
        /// 一个标签下的未回答的答案
        /// </summary>
        /// <param name="labelid"></param>
        /// <returns></returns>
        public string GetlabelUQ(int labelid,string labelname)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            sqlConnection.Open();
            try
            {
                string sql = "select Qusitonid,Askerid,AskTime,UpAskTime,QusitonTitle,QusitonContent,Answerednum,Followednum,QLabel0,QLabel1,QLabel2 " +
                    " from QusitonTable inner join LabelTable on Labelid =@labelid and Answerednum =0 and Labelname=QLabel0";
                SqlCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandText = sql;
                sqlCommand.Parameters.AddWithValue("@labelid", labelid);
                SqlDataAdapter dataAdapter = new SqlDataAdapter
                {
                    SelectCommand = sqlCommand
                };
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                List<Qusition> qusitions = new List<Qusition>();
                Qusition qusition;
                foreach (DataRow item in dataTable.Rows)
                {
                    qusition = GetQusitionRow(item);
                    qusitions.Add(qusition);
                }
                return JsonConvert.SerializeObject(qusitions);
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
        /// convert DataTable to Json
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        private Qusition GetQusitionRow(DataRow dataRow)
        {
            Qusition qusition = new Qusition
            {
                Qusitionid = Convert.ToInt32(dataRow["Qusitonid"]),
                Askerid = Convert.ToInt32(dataRow["Askerid"]),
                AskTime = Convert.ToDateTime(dataRow["AskTime"].ToString()),
                UpAskTime = Convert.ToDateTime(dataRow["UpAskTime"].ToString()),
                QusitionTitle = dataRow["QusitonTitle"].ToString(),
                QusitionContent = dataRow["QusitonContent"].ToString(),
                Answerednum = Convert.ToInt32(dataRow["Answerednum"]),
                Followednum = Convert.ToInt32(dataRow["Followednum"]),
                Qlabel0 = dataRow["QLabel0"].ToString(),
                Qlabel1 = dataRow["QLabel1"].ToString(),
                Qlabel2 = dataRow["QLabel2"].ToString()
            };
            return qusition;
        }
        private GetUserFollow GetGetUserFollow(DataRow dataRow)
        {
            GetUserFollow getUserFollow = new GetUserFollow
            {
                Qusitonid = Convert.ToInt32(dataRow["Qusitonid"]),
                Askerid = Convert.ToInt32(dataRow["Askerid"]),
                AskTime = Convert.ToDateTime(dataRow["AskTime"].ToString()),
                UpAskTime = Convert.ToDateTime(dataRow["UpAskTime"].ToString()),
                QusitionTitle = dataRow["QusitonTitle"].ToString(),
                QusitionContent = dataRow["QusitonContent"].ToString(),
                Answerednum = Convert.ToInt32(dataRow["Answerednum"]),
                Followednum = Convert.ToInt32(dataRow["Followednum"]),
                Qlabel0 = dataRow["QLabel0"].ToString(),
                Qlabel1 = dataRow["QLabel1"].ToString(),
                Qlabel2 = dataRow["QLabel2"].ToString(),
                FollowQTime = Convert.ToDateTime(dataRow["FollowQTime"].ToString())
            };
            return getUserFollow;
        }



    }
}

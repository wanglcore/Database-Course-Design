using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using Web.Models;

namespace Web.Access
{
    public struct NU
    {
        public string Intorduction;
        public string Name;
        public int Userid;
    }
    public struct NI
    {
        public string Introduction;
        public int Userid;
    }
    public class PeopleAccess
    {
        private const string connectionstring = "Data Source=127.0.0.1;Initial Catalog=test;Password=10086.;user id =wangl; Enlist=true";
        /// <summary>
        /// 验证登录
        /// </summary>
        /// <param name="email"></param>
        /// <param name="passwd"></param>
        /// <returns></returns>
        public int GetName(string email, string passwd)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            try
            {
                sqlConnection.Open();
                string sql = "select Userid from UserTable where Email =@Email and Passwd=@Passwd";
                SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection);
                sqlCommand.Parameters.Add(new SqlParameter("@Email", email));
                sqlCommand.Parameters.AddWithValue("@Passwd", passwd);
                if (sqlCommand.ExecuteScalar() == null)
                {
                    return -1;
                }
                else
                {
                    return Convert.ToInt32(sqlCommand.ExecuteScalar().ToString());
                }
            }
            catch (Exception e)
            {
                return -1;
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        /// <summary>
        /// 更新密码
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="newpasswd"></param>
        /// <returns></returns>
        public bool UpdatePasswd(string Email, string newpasswd)
        {
            SqlConnection sqlconnection = new SqlConnection(connectionstring);
            sqlconnection.Open();
            using (SqlCommand sqlCommand = sqlconnection.CreateCommand())
            {

                try
                {
                    string isvaliduser = "select Email from UserTable where Email=@Email";
                    sqlCommand.CommandText = isvaliduser;
                    sqlCommand.Parameters.Add(new SqlParameter("@Email", Email));
                    if (sqlCommand.ExecuteScalar() == null)
                    {
                        return false;
                    }
                    else
                    {
                        SqlTransaction sqlTransaction = sqlconnection.BeginTransaction();
                        try
                        {
                            string sql = "update UserTable set Passwd =@passwd where Email=@Email";
                            sqlCommand.Parameters.Clear();
                            sqlCommand.CommandText = sql;
                            sqlCommand.Transaction = sqlTransaction;
                            sqlCommand.Parameters.Add(new SqlParameter("@passwd", newpasswd));
                            sqlCommand.Parameters.Add(new SqlParameter("@Email", Email));
                            int rows = sqlCommand.ExecuteNonQuery();
                            sqlTransaction.Commit();
                            return true;
                        }
                        catch (SqlException)
                        {
                            sqlTransaction.Rollback();
                            return false;
                        }
                    }
                }
                catch (SqlException)
                {
                    return false;
                }
                finally
                {
                    sqlconnection.Close();
                }
            }
        }


        public bool PutIntro(NU nU)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            sqlConnection.Open();
            using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
            {
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                try
                {
                    string sql = $"update UserTable set Introduction ='{nU.Intorduction}' where Userid='{nU.Userid}'";
                    sqlCommand.CommandText = sql;
                    sqlCommand.Transaction = sqlTransaction;
                    sqlCommand.ExecuteNonQuery();
                    sqlTransaction.Commit();
                    return true;
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
        /// 更新昵称
        /// </summary>
        /// <param name="nU"></param>
        /// <returns></returns>
        public bool PutName(NU nU)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            sqlConnection.Open();
            using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
            {
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                try
                {
                    string str = $"update UserTable set Name =@Name where Userid=@Userid;" +
                        $"update CommentTable set UseridName=@Name where Userid=@Userid;" +
                        $"update CommentTable set CmdUseridName=@Name where CmtUserid=@Userid;" +
                        $"update CommentTable set CmtedUseridName=@Name where CmtedUserid=@Userid;" +
                        $"update ArticalTable set Username=@Name where Userid=@Userid;" +
                        $"update ArticalCmtTable set UserName=@Name where Userid=@Userid;" +
                        $"update ArticalCmtTable set ArtCmtUserName=@Name where ArtCmtUserid=@Userid;" +
                        $"update ArticalCmtTable set ArtCmtedUserName =@Name where ArtCmtedUserid=@Userid";
                    sqlCommand.Parameters.AddWithValue("@Name", nU.Name);
                    sqlCommand.Parameters.AddWithValue("@Userid", nU.Userid);
                    sqlCommand.CommandText = str;
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
        /// 添加新用户
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="Name"></param>
        /// <param name="Passwd"></param>
        /// <returns></returns>
        public int PostnewUser(People people)
        {
            SqlConnection sqlconnection = new SqlConnection(connectionstring);
            sqlconnection.Open();
            try
            {
                using (SqlCommand sqlCommand = sqlconnection.CreateCommand())
                {
                    string sql = "select Passwd from UserTable where Email = @Email";
                    sqlCommand.CommandText = sql;
                    sqlCommand.Parameters.Add(new SqlParameter("@Email", people.Email));
                    if (sqlCommand.ExecuteScalar() == null)
                    {
                        sqlCommand.Parameters.Clear();
                        string datetime = DateTime.Now.ToString();
                        SqlTransaction sqlTransaction = sqlconnection.BeginTransaction();
                        try
                        {
                            string sqls = "insert into UserTable (Email,Name,Passwd,CreateTime)values(@Email,@Name,@Passwd,@CreateTime)";
                            sqlCommand.CommandText = sqls;
                            sqlCommand.Transaction = sqlTransaction;
                            sqlCommand.Parameters.Add(new SqlParameter("@Email", people.Email));
                            sqlCommand.Parameters.Add(new SqlParameter("@Name", people.Name));
                            sqlCommand.Parameters.Add(new SqlParameter("@Passwd", people.Passwd));
                            sqlCommand.Parameters.Add(new SqlParameter("@CreateTime", datetime));
                            int t = sqlCommand.ExecuteNonQuery();
                            sqlTransaction.Commit();
                            return 1;
                        }
                        catch (SqlException)
                        {
                            sqlTransaction.Rollback();
                            return 2;
                        }
                    }
                    return 0;
                }
            }
            catch (SqlException)
            {
                return 3;
            }
            finally
            {
                sqlconnection.Close();
            }
        }
        /// <summary>
        /// 得到用户的信息
        /// </summary>
        /// <param name="Userid"></param>
        /// <returns></returns>
        public People GetUserInfor(int Userid)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            sqlConnection.Open();
            try
            {
                string sql = "select Userid,Name,Introduction,Follownum,Focusednum," +
                    "Answernum,Qusitionnum,Publishnum,FQusitionnum,FLabelnum from UserTable" +
                    " where Userid=@Userid";
                SqlCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandText = sql;
                sqlCommand.Parameters.AddWithValue("@Userid", Userid);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                sqlDataAdapter.SelectCommand = sqlCommand;
                DataTable dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);
                People people = new People();
                foreach (DataRow item in dataTable.Rows)
                {
                    people = GetPeople(item);
                }
                return people;
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

        public string GetlabelP(int labelid)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            sqlConnection.Open();
            try
            {
                string sql = "select UserTable.Userid,Name,Email,Passwd,CreateTime,Introduction,Follownum,Focusednum,Answernum,Qusitionnum,Publishnum,FQusitionnum,FLabelnum " +
                    " from UserTable inner join UserLabel on UserLabel.Labelid=@labelid and UserLabel.Userid=UserTable.Userid";
                SqlCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandText = sql;
                sqlCommand.Parameters.AddWithValue("@labelid", labelid);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                sqlDataAdapter.SelectCommand = sqlCommand;
                DataTable dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);
                List<People> peoples = new List<People>();
                People people;
                foreach (DataRow item in dataTable.Rows)
                {
                    people = GetPeople(item);
                    peoples.Add(people);
                }
                return JsonConvert.SerializeObject(peoples);
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


        public List<People> Getbyname(string text)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            sqlConnection.Open();
            using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
            {
                try
                {
                    string sql = $"select * from UserTable where Name like '%{text}%'";
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
                        List<People> peoples = new List<People>();
                        People people;
                        foreach (DataRow item in dataTable.Rows)
                        {
                            people = GetPeople(item);
                            peoples.Add(people);
                        }
                        return peoples;
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
        public List<People> GetallUser(string text)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            sqlConnection.Open();
            using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
            {
                try
                {
                    string sql = $"select * from UserTable where Name like '%{text}%'";
                    sqlCommand.CommandText = sql;
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter
                    {
                        SelectCommand = sqlCommand
                    };
                    DataTable dataTable = new DataTable();
                    sqlDataAdapter.Fill(dataTable);
                    List<People> peoples = new List<People>();
                    People people;
                    foreach (DataRow item in dataTable.Rows)
                    {
                        people = GetPeople(item);
                        peoples.Add(people);
                    }
                    return peoples;

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
        private People GetPeople(DataRow dataRow)
        {
            People people = new People
            {
                UserId = Convert.ToInt32(dataRow["Userid"]),
                Name = dataRow["Name"].ToString(),
                Answernum = Convert.ToInt32(dataRow["Answernum"]),
                Follownum = Convert.ToInt32(dataRow["Follownum"]),
                Publishnum = Convert.ToInt32(dataRow["Publishnum"]),
                Qusitionnum = Convert.ToInt32(dataRow["Qusitionnum"]),
                Focusednum = Convert.ToInt32(dataRow["Focusednum"]),
                Introduction = dataRow["Introduction"].ToString(),
                FQusitionnum = Convert.ToInt32(dataRow["FQusitionnum"]),
                FLabelnum = Convert.ToInt32(dataRow["FLabelnum"])
            };
            return people;
        }
    }
}
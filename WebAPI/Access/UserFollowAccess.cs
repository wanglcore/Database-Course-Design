using Newtonsoft.Json;
using System.Data;
using System.Transactions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Access
{
    public class UserFollowAccess
    {
        private const string connectionstring = "Data Source=127.0.0.1;Initial Catalog=test;Password=10086.;user id =wangl; Enlist=true";

        /// <summary>
        /// 添加关注
        /// </summary>
        /// <param name="Userid"></param>
        /// <param name="FollowUserid"></param>
        /// <returns></returns>
        public bool AddFollow(UserFollow userFollow)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
                string datetime = DateTime.Now.ToString();
                string sql = "insert into UserFollowTable (Userid,FollowUserid,FollowTime)" +
                    "values(@Userid,@FollowUserid,@FollowTime);update UserTable set Follownum =Follownum+1 where Userid =@Userid;update UserTable set Focusednum =Focusednum+1 where Userid =@FollowUserid";
            sqlConnection.Open();
            using (SqlCommand sqlCommand = sqlConnection.CreateCommand()) {
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                try
                {
                    sqlCommand.CommandText = sql;
                    sqlCommand.Transaction = sqlTransaction;
                    sqlCommand.Parameters.AddWithValue("@Userid", userFollow.Userid);
                    sqlCommand.Parameters.AddWithValue("@FollowUserid", userFollow.FollowUserid);
                    sqlCommand.Parameters.AddWithValue("@FollowTime", datetime);
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
        /// 取消关注
        /// </summary>
        /// <param name="Userid"></param>
        /// <param name="FollowUserid"></param>
        /// <returns></returns>
        public bool DeleteFollow(UserFollow userFollow)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
                string sql = "delete from UserFollowTable where Userid = @Userid and FollowUserid =@FollowUserid;update UserTable set Follownum =Follownum - 1 where Userid =@Userid;update UserTable set Focusednum =Focusednum - 1 where Userid =@FollowUserid";
            sqlConnection.Open();
            using (SqlCommand sqlCommand = sqlConnection.CreateCommand()) {
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                try
                {
                    sqlCommand.CommandText = sql;
                    sqlCommand.Transaction = sqlTransaction;
                    sqlCommand.Parameters.AddWithValue("@Userid", userFollow.Userid);
                    sqlCommand.Parameters.AddWithValue("@FollowUserid", userFollow.FollowUserid);
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
        /// 判断是否关注
        /// </summary>
        /// <param name="Userid"></param>
        /// <param name="Myid"></param>
        /// <returns></returns>
        public int GetisFollow(int Userid,int Myid)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            sqlConnection.Open();
            using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
            {
                try
                {
                    string sql = "select Userid,FollowUserid from UserFollowTable where Userid=@Userid and FollowUserid=@FollowUserid";
                    sqlCommand.CommandText = sql;
                    sqlCommand.Parameters.AddWithValue("@Userid", Myid);
                    sqlCommand.Parameters.AddWithValue("@FollowUserid", Userid);
                    if (sqlCommand.ExecuteScalar() != null)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch (SqlException)
                {
                    return -1;

                }
                finally
                {
                    sqlConnection.Close();
                }

            }
        }
        /// <summary>
        /// 关注我的人
        /// </summary>
        /// <param name="Userid"></param>
        /// <returns></returns>
        public string GetFollowme(int Userid)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            sqlConnection.Open();
            using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
            {
                try
                {
                    string sql = "select UserTable.Userid,Name,Introduction from UserTable inner join UserFollowTable on UserFollowTable.FollowUserid=@Userid and UserFollowTable.Userid=UserTable.Userid";
                    sqlCommand.CommandText = sql;
                    sqlCommand.Parameters.AddWithValue("@Userid", Userid);
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter
                    {
                        SelectCommand = sqlCommand,
                    };
                    DataTable dataTable = new DataTable();
                    sqlDataAdapter.Fill(dataTable);
                    List<MyFollowUser> myFollowUsers = new List<MyFollowUser>();
                    MyFollowUser myFollowUser;
                    foreach (DataRow item in dataTable.Rows)
                    {
                        myFollowUser = GetMyFollows(item);
                        myFollowUsers.Add(myFollowUser);
                    }
                    return JsonConvert.SerializeObject(myFollowUsers);
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
        /// 我关注的人
        /// </summary>
        /// <param name="Userid"></param>
        /// <returns></returns>
        public string GetMyFollow(int Userid)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            sqlConnection.Open();
            using(SqlCommand sqlCommand = sqlConnection.CreateCommand())
            {
                try
                {
                    string sql = "select FollowUserid,Name,Introduction from UserFollowTable inner join UserTable on FollowUserid=UserTable.Userid and UserFollowTable.Userid=@Userid";
                    sqlCommand.CommandText = sql;
                    sqlCommand.Parameters.AddWithValue("@Userid", Userid);

                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter
                    {
                        SelectCommand = sqlCommand,
                    };
                    DataTable dataTable = new DataTable();
                    sqlDataAdapter.Fill(dataTable);
                    List<MyFollowUser> myFollowUsers = new List<MyFollowUser>();
                    MyFollowUser myFollowUser;
                    foreach (DataRow item in dataTable.Rows)
                    {
                        myFollowUser = GetMyFollow(item);
                        myFollowUsers.Add(myFollowUser);
                    }
                    return JsonConvert.SerializeObject(myFollowUsers);
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

        private MyFollowUser GetMyFollow(DataRow dataRow)
        {
            MyFollowUser myFollowUser = new MyFollowUser
            { 
                FollowUserid = Convert.ToInt32(dataRow["FollowUserid"]),
                Name = dataRow["Name"].ToString(),
                Introduction = dataRow["Introduction"].ToString(),
            };
            return myFollowUser;
        }
        private MyFollowUser GetMyFollows(DataRow dataRow)
        {
            MyFollowUser myFollowUser = new MyFollowUser
            {
                FollowUserid = Convert.ToInt32(dataRow["Userid"]),
                Name = dataRow["Name"].ToString(),
                Introduction = dataRow["Introduction"].ToString(),
            };
            return myFollowUser;
        }
    }
    
}

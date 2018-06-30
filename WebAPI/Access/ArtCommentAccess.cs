using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;
using Web.Models;
namespace Web.Access
{
    public class ArtCommentAccess
    {
        private const string connectionstring = "Data Source=127.0.0.1;Initial Catalog=test;Password=10086.;user id =wangl; Enlist=true";
        public bool PostArtComment(ArticalCmt articalCmt)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            sqlConnection.Open();
            using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
            {
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                try
                {
                    string sql = "insert into ArticalCmtTable (Articalid,ArtCmtUserid,ArtCmtedUserid,ArtCmtTime,ArtCmtContent,UserName,ArtCmtUserName,ArtCmtedUserName,Userid)values(@Articalid,@ArtCmtUserid,@ArtCmtedUserid,@ArtCmtTime,@ArtCmtContent,@UserName,@ArtCmtUserName,@ArtCmtedUserName,@Userid);update ArticalTable set ArtCmtnum =ArtCmtnum+1 where Articalid=@Articalid";
                    sqlCommand.CommandText = sql;
                    sqlCommand.Transaction = sqlTransaction;
                    sqlCommand.Parameters.AddWithValue("@Articalid", articalCmt.Articalid);
                    sqlCommand.Parameters.AddWithValue("@ArtCmtUserid", articalCmt.ArtCmtUserid);
                    sqlCommand.Parameters.AddWithValue("@ArtCmtedUserid", articalCmt.ArtCmtedUserid);
                    sqlCommand.Parameters.AddWithValue("@ArtCmtTime", articalCmt.ArtCmtTime);
                    sqlCommand.Parameters.AddWithValue("@ArtCmtContent", articalCmt.ArtCmtContent);
                    sqlCommand.Parameters.AddWithValue("@UserName", articalCmt.UseridName);
                    sqlCommand.Parameters.AddWithValue("@ArtCmtUserName", articalCmt.CmdUseridName);
                    sqlCommand.Parameters.AddWithValue("@ArtCmtedUserName", articalCmt.CmtedUseridName);
                    sqlCommand.Parameters.AddWithValue("@Userid", articalCmt.Userid);
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

        public bool PutLike(ArtCommentPlus artCommentPlus)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            sqlConnection.Open();
            using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
            {
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                try
                {
                    string sql = "update ArticalCmtTable set UpArtCmtnum =UpArtCmtnum+1 where Articalid=@Articalid and ArtCmtUserid=@ArtCmtUserid and ArtCmtedUserid=@ArtCmtedUserid and ArtCmtTime=@ArtCmtTime";
                    sqlCommand.CommandText = sql;
                    sqlCommand.Transaction = sqlTransaction;
                    sqlCommand.Parameters.AddWithValue("@Articalid", artCommentPlus.Articalid);
                    sqlCommand.Parameters.AddWithValue("@ArtCmtUserid", artCommentPlus.CmdUserid);
                    sqlCommand.Parameters.AddWithValue("@ArtCmtedUserid", artCommentPlus.CmtedUserid);
                    sqlCommand.Parameters.AddWithValue("ArtCmtTime", artCommentPlus.Datetime);
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

        public bool PutDisLike(ArtCommentPlus artCommentPlus)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            sqlConnection.Open();
            using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
            {
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                try
                {
                    string sql = "update ArticalCmtTable set DownArtCmtnum =DownArtCmtnum+1 where Articalid=@Articalid and ArtCmtUserid=@ArtCmtUserid and ArtCmtedUserid=@ArtCmtedUserid and ArtCmtTime=@ArtCmtTime";
                    sqlCommand.CommandText = sql;
                    sqlCommand.Transaction = sqlTransaction;
                    sqlCommand.Parameters.AddWithValue("@Articalid", artCommentPlus.Articalid);
                    sqlCommand.Parameters.AddWithValue("@ArtCmtUserid", artCommentPlus.CmdUserid);
                    sqlCommand.Parameters.AddWithValue("@ArtCmtedUserid", artCommentPlus.CmtedUserid);
                    sqlCommand.Parameters.AddWithValue("ArtCmtTime", artCommentPlus.Datetime);
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

        public bool DelPutLike(ArtCommentPlus artCommentPlus)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            sqlConnection.Open();
            using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
            {
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                try
                {
                    string sql = "update ArticalCmtTable set UpArtCmtnum =UpArtCmtnum-1 where Articalid=@Articalid and ArtCmtUserid=@ArtCmtUserid and ArtCmtedUserid=@ArtCmtedUserid and ArtCmtTime=@ArtCmtTime";
                    sqlCommand.CommandText = sql;
                    sqlCommand.Transaction = sqlTransaction;
                    sqlCommand.Parameters.AddWithValue("@Articalid", artCommentPlus.Articalid);
                    sqlCommand.Parameters.AddWithValue("@ArtCmtUserid", artCommentPlus.CmdUserid);
                    sqlCommand.Parameters.AddWithValue("@ArtCmtedUserid", artCommentPlus.CmtedUserid);
                    sqlCommand.Parameters.AddWithValue("ArtCmtTime", artCommentPlus.Datetime);
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

        public bool DelPutDisLike(ArtCommentPlus artCommentPlus)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            sqlConnection.Open();
            using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
            {
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                try
                {
                    string sql = "update ArticalCmtTable set DownArtCmtnum =DownArtCmtnum-1 where Articalid=@Articalid and ArtCmtUserid=@ArtCmtUserid and ArtCmtedUserid=@ArtCmtedUserid and ArtCmtTime=@ArtCmtTime";
                    sqlCommand.CommandText = sql;
                    sqlCommand.Transaction = sqlTransaction;
                    sqlCommand.Parameters.AddWithValue("@Articalid", artCommentPlus.Articalid);
                    sqlCommand.Parameters.AddWithValue("@ArtCmtUserid", artCommentPlus.CmdUserid);
                    sqlCommand.Parameters.AddWithValue("@ArtCmtedUserid", artCommentPlus.CmtedUserid);
                    sqlCommand.Parameters.AddWithValue("ArtCmtTime", artCommentPlus.Datetime);
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


        public string GetArtCmt(int Userid,int Articalid)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            sqlConnection.Open();
            using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
            {
                try
                {
                    string sql = "select * from ArticalCmtTable where Articalid=@Articalid order by ArtCmtTime desc";
                    sqlCommand.CommandText = sql;
                    sqlCommand.Parameters.AddWithValue("@Articalid", Articalid);
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter
                    {
                        SelectCommand = sqlCommand,
                    };
                    DataTable dataTable = new DataTable();
                    sqlDataAdapter.Fill(dataTable);
                    List<ArticalCmt> articalCmts = new List<ArticalCmt>();
                    ArticalCmt articalCmt;
                    foreach (DataRow item in dataTable.Rows)
                    {
                        articalCmt = GetCmtByRow(item);
                        articalCmts.Add(articalCmt);
                    }
                    return JsonConvert.SerializeObject(articalCmts);
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

        public string GetArtCmtandName(int Userid)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            sqlConnection.Open();
            using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
            {
                try
                {
                    string sql = "select ArticalTitle,ArticalCmtTable.Articalid,ArtCmtUserid,ArtCmtedUserid,ArtCmtContent,UpArtCmtnum,DownArtCmtnum,ArticalCmtTable.UserName,ArtCmtUserName,ArtCmtedUserName,ArticalCmtTable.Userid from ArticalCmtTable inner join ArticalTable on ArtCmtedUserid=@Userid order by ArtCmtTime desc";
                    sqlCommand.CommandText = sql;
                    sqlCommand.Parameters.AddWithValue("@Userid", Userid);
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter
                    {
                        SelectCommand = sqlCommand,
                    };
                    DataTable dataTable = new DataTable();
                    sqlDataAdapter.Fill(dataTable);
                    List<ArticalCmt> articalCmts = new List<ArticalCmt>();
                    ArticalCmt articalCmt;
                    foreach (DataRow item in dataTable.Rows)
                    {
                        articalCmt = GetCmtByRow2(item);
                        articalCmts.Add(articalCmt);
                    }
                    return JsonConvert.SerializeObject(articalCmts);
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

        private ArticalCmt GetCmtByRow(DataRow dataRow)
        {
            ArticalCmt articalCmt = new ArticalCmt
            {
                Articalid = Convert.ToInt32(dataRow["Articalid"]),
                ArtCmtUserid = Convert.ToInt32(dataRow["ArtCmtUserid"]),
                ArtCmtTime = Convert.ToDateTime(dataRow["ArtCmtTime"]),
                ArtCmtContent = dataRow["ArtCmtContent"].ToString(),
                UpArtCmtnum = Convert.ToInt32(dataRow["UpArtCmtnum"]),
                DownArtCmtnum = Convert.ToInt32(dataRow["DownArtCmtnum"]),
                ArtCmtedUserid = Convert.ToInt32(dataRow["ArtCmtedUserid"]),
                CmdUseridName = dataRow["ArtCmtUserName"].ToString(),
                CmtedUseridName = dataRow["ArtCmtedUserName"].ToString(),
                UseridName = dataRow["UserName"].ToString(),
            };
            return articalCmt;
        }

        private ArticalCmt GetCmtByRow2(DataRow dataRow)
        {
            ArticalCmt articalCmt = new ArticalCmt
            {
                Articalid = Convert.ToInt32(dataRow["Articalid"]),
                ArtCmtUserid = Convert.ToInt32(dataRow["ArtCmtUserid"]),
                ArtCmtTime = Convert.ToDateTime(dataRow["ArtCmtTime"]),
                ArtCmtContent = dataRow["ArtCmtContent"].ToString(),
                UpArtCmtnum = Convert.ToInt32(dataRow["UpArtCmtnum"]),
                DownArtCmtnum = Convert.ToInt32(dataRow["DownArtCmtnum"]),
                ArtCmtedUserid = Convert.ToInt32(dataRow["ArtCmtedUserid"]),
                CmdUseridName = dataRow["ArtCmtUserName"].ToString(),
                CmtedUseridName = dataRow["ArtCmtedUserName"].ToString(),
                UseridName = dataRow["UserName"].ToString(),
                ArticalName=dataRow["ArticalTitle"].ToString(),
            };
            return articalCmt;
        }
    }




}


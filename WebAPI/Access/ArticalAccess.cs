using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Web.Models;
using System.Data;

namespace Web.Access
{
    public class ArticalAccess
    {
        private const string connectionstring = "Data Source=127.0.0.1;Initial Catalog=test;Password=10086.;user id =wangl; Enlist=true";
        /// <summary>
        /// 发表文章
        /// </summary>
        /// <param name="artical"></param>
        /// <returns></returns>
        public bool PostArtical(Artical artical)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            sqlConnection.Open();
            using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
            {
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                try
                {
                    string sql = "insert into ArticalTable(Userid,ArticalTitle,ArticalContent,ArticalPTime,ArticalUPTime,Label0,Username) values(@Userid,@ArticalTitle,@ArticalContent,@ArticalPTime,@ArticalUPTime,@label0,@Username);" +
                        "update UserTable set Publishnum =Publishnum+1 where Userid=@Userid";
                    sqlCommand.CommandText = sql;
                    sqlCommand.Transaction = sqlTransaction;
                    sqlCommand.Parameters.AddWithValue("@Userid", artical.Userid);
                    sqlCommand.Parameters.AddWithValue("@ArticalTitle", artical.ArticalTitle);
                    sqlCommand.Parameters.AddWithValue("@ArticalContent", artical.ArticalContent.ToString());
                    sqlCommand.Parameters.AddWithValue("@ArticalPTime", artical.ArticalTime);
                    sqlCommand.Parameters.AddWithValue("@ArticalUPTime", artical.ArticalUPTime);
                    sqlCommand.Parameters.AddWithValue("@label0", artical.Label0);
                    sqlCommand.Parameters.AddWithValue("@Username", artical.UseridName);
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
        /// 按照名字得到文章
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Artical> Getbyname(string name)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            sqlConnection.Open();
            using (SqlCommand sqlCommand=sqlConnection.CreateCommand())
            {
                try
                {
                    string sql = $"select * from ArticalTable where ArticalTitle like '%{name}%'";
                    sqlCommand.CommandText = sql;
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter
                    {
                        SelectCommand = sqlCommand
                    };
                    DataTable dataTable = new DataTable();
                    sqlDataAdapter.Fill(dataTable);
                    if (dataTable.Rows.Count == 0)
                    {
                        return null;
                    }
                    else
                    {
                        List<Artical> articals = new List<Artical>();
                        Artical artical;
                        foreach (DataRow item in dataTable.Rows)
                        {
                            artical = GetArticalRow(item);
                            articals.Add(artical);
                        }
                        return articals;
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
        /// 得到所有的文章
        /// </summary>
        /// <returns></returns>
        public string GetallArtical()
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            sqlConnection.Open();
            using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
            {
                try
                {
                    string sql = "select Articalid,Userid,ArticalTitle,ArticalContent,ArticalPTime,ArticalUPTime,ArtCmtnum,DownArtnum,UpArtnum,Username,Label0 from ArticalTable order by ArticalPTime desc";
                    sqlCommand.CommandText = sql;
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter
                    {
                        SelectCommand = sqlCommand
                    };
                    DataTable dataTable = new DataTable();
                    sqlDataAdapter.Fill(dataTable);
                    List<Artical> articals = new List<Artical>();
                    Artical artical;
                    foreach (DataRow item in dataTable.Rows)
                    {
                        artical = GetArticalRow(item);
                        articals.Add(artical);
                    }
                    return JsonConvert.SerializeObject(articals);
                }
                catch (SqlException)
                {

                    return string.Empty;
                }   
            }
        }
        /// <summary>
        /// 得到我发表的文章
        /// </summary>
        /// <param name="Userid"></param>
        /// <returns></returns>
        public string GetMyArtical(int Userid)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            sqlConnection.Open();
            using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
            {
                try
                {
                    string sql = "select *from ArticalTable where Userid=@Userid order by ArticalPTime desc";
                    sqlCommand.CommandText = sql;
                    sqlCommand.Parameters.AddWithValue("@Userid", Userid);
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter
                    {
                        SelectCommand = sqlCommand,
                    };
                    DataTable dataTable = new DataTable();
                    sqlDataAdapter.Fill(dataTable);
                    List<Artical> articals = new List<Artical>();
                    Artical artical;
                    foreach (DataRow item in dataTable.Rows)
                    {
                        artical = GetArticalRow(item);
                        articals.Add(artical);
                    }
                    return JsonConvert.SerializeObject(articals);
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
        /// 对文章赞同
        /// </summary>
        /// <param name="Articalid"></param>
        /// <param name="Myid"></param>
        /// <returns></returns>
        public bool Putlike(int Articalid,int Myid)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            sqlConnection.Open();
            using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
            {
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                try
                {
                    string sql = "update ArticalTable set UpArtnum=UpArtnum+1 where Articalid=@Articalid";
                    sqlCommand.CommandText = sql;
                    sqlCommand.Transaction = sqlTransaction;
                    sqlCommand.Parameters.AddWithValue("@Articalid", Articalid);
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
        /// 对文章不喜欢
        /// </summary>
        /// <param name="Articalid"></param>
        /// <param name="Myid"></param>
        /// <returns></returns>
        public bool PutDislike(int Articalid, int Myid)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            sqlConnection.Open();
            using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
            {
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                try
                {
                    string sql = "update ArticalTable set DownArtnum=DownArtnum+1 where Articalid=@Articalid";
                    sqlCommand.CommandText = sql;
                    sqlCommand.Transaction = sqlTransaction;
                    sqlCommand.Parameters.AddWithValue("@Articalid", Articalid);
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
        /// 取消对文章的赞同
        /// </summary>
        /// <param name="Articalid"></param>
        /// <param name="Myid"></param>
        /// <returns></returns>
        public bool DelPutlike(int Articalid, int Myid)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            sqlConnection.Open();
            using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
            {
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                try
                {
                    string sql = "update ArticalTable set UpArtnum=UpArtnum-1 where Articalid=@Articalid";
                    sqlCommand.CommandText = sql;
                    sqlCommand.Transaction = sqlTransaction;
                    sqlCommand.Parameters.AddWithValue("@Articalid", Articalid);
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
        /// 取消对文章的不赞同
        /// </summary>
        /// <param name="Articalid"></param>
        /// <param name="Myid"></param>
        /// <returns></returns>
        public bool DelPutdislike(int Articalid, int Myid)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            sqlConnection.Open();
            using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
            {
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                try
                {
                    string sql = "update ArticalTable set DownArtnum=DownArtnum-1 where Articalid=@Articalid";
                    sqlCommand.CommandText = sql;
                    sqlCommand.Transaction = sqlTransaction;
                    sqlCommand.Parameters.AddWithValue("@Articalid", Articalid);
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
        
        private Artical GetArticalRow(DataRow dataRow)
        {
            Artical artical = new Artical
            {
                Articalid = Convert.ToInt32(dataRow["Articalid"]),
                Userid = Convert.ToInt32(dataRow["Userid"]),
                ArticalContent =dataRow["ArticalContent"].ToString(),
                ArticalTitle =dataRow["ArticalTitle"].ToString(),
                UseridName=dataRow["Username"].ToString(),
                ArtCmtnum=Convert.ToInt32(dataRow["ArtCmtnum"]),
                ArticalTime=Convert.ToDateTime(dataRow["ArticalPTime"]),
                ArticalUPTime=Convert.ToDateTime(dataRow["ArticalUPTime"]),
                DownArtnum=Convert.ToInt32(dataRow["DownArtnum"]),
                UpArtnum=Convert.ToInt32(dataRow["UpArtnum"]),
                Label0=dataRow["Label0"].ToString(),
            };
            return artical;
        }
    }
}

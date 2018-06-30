using System;
using System.Data.SqlClient;
using Web.Models;
using System.Collections.Generic;
using System.Data;
using Newtonsoft.Json;
namespace Web.Access
{
    public class Labelaccess
    {
        private const string connectionstring = "Data Source=127.0.0.1;Initial Catalog=test;Password=10086.;user id =wangl; Enlist=true";
        /// <summary>
        /// 发布标签
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        public bool Postlabel(Label label)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
         

                    string str = "insert into LabelTable(LabelCTime,Labelname,LabelBrief)values(@time,@name,@brief)";
                sqlConnection.Open();
                    using(SqlCommand sqlCommand = sqlConnection.CreateCommand()){
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                try
                {
                    sqlCommand.CommandText = str;
                    sqlCommand.Transaction = sqlTransaction;
                    sqlCommand.Parameters.AddWithValue("@time", DateTime.Now.ToString());
                    sqlCommand.Parameters.AddWithValue("@name", label.Labelname);
                    sqlCommand.Parameters.AddWithValue("@brief", label.LabelBrief);
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
        /// 得到所有的标签
        /// </summary>
        /// <returns></returns>
        public string GetallLabels()
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            sqlConnection.Open();
            try
            {
                string sql = "select * from LabelTable";
                SqlCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandText = sql;
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter
                {
                    SelectCommand = sqlCommand
                };
                DataTable dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);
                List<Label> labels = new List<Label>();
                Label label;
                foreach (DataRow item in dataTable.Rows)
                {
                    label = new Label();
                    label = GetLabel(item);
                    labels.Add(label);
                }
                return JsonConvert.SerializeObject(labels);
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
        /// 按照名字得到标签
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Label> getbyname(string name)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            sqlConnection.Open();
            using(SqlCommand sqlCommand = sqlConnection.CreateCommand())
            {
                try
                {
                    string sql = $"select *from LabelTable where Labelname like '%{name}%'";
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
                        List<Label> labels = new List<Label>();
                        Label label;
                        foreach (DataRow item in dataTable.Rows)
                        {
                            label = GetLabel(item);
                            labels.Add(label);
                        }

                        return labels;
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
        //得到一个人关注的标签
        public string GetfLabels(int Userid)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            sqlConnection.Open();
            try
            {
                string sql = "select LabelTable.Labelid,LabelCTime,Labelname,LabelBrief,LabelQnum,LabelUnum from LabelTable inner join UserLabel on UserLabel.Userid=@Userid and UserLabel.Labelid=LabelTable.Labelid";
                SqlCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandText = sql;
                sqlCommand.Parameters.AddWithValue("@Userid", Userid);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter
                {
                    SelectCommand = sqlCommand
                };
                DataTable dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);
                List<Label> labels = new List<Label>();
                Label label;
                foreach (DataRow item in dataTable.Rows)
                {
                    label = new Label();
                    label = GetLabel(item);
                    labels.Add(label);
                }
                return JsonConvert.SerializeObject(labels);
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
        /// 关注标签
        /// </summary>
        /// <param name="labelid"></param>
        /// <param name="Userid"></param>
        /// <returns></returns>
        public bool PutFLabel(int labelid, int Userid)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
         
                    string str = "insert into UserLabel(Userid,Labelid,UFollowTime) values(@Userid,@Labelid,@UFollowTime);UPDATE UserTable SET FLabelnum=FLabelnum+1 where Userid=@Userid;UPDATE LabelTable SET LabelUnum = LabelUnum+1 where Labelid=@Labelid";

                sqlConnection.Open();
            using (SqlCommand sqlCommand = sqlConnection.CreateCommand()) {
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                try
                {
                    sqlCommand.CommandText = str;
                    sqlCommand.Transaction = sqlTransaction;
                    sqlCommand.Parameters.AddWithValue("@Userid", Userid);
                    sqlCommand.Parameters.AddWithValue("@Labelid", labelid);
                    sqlCommand.Parameters.AddWithValue("@UFollowTime", DateTime.Now.ToString());
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
        /// 取消标签
        /// </summary>
        /// <param name="labelid"></param>
        /// <param name="Userid"></param>
        /// <returns></returns>
        public bool PutUFLabel(int labelid, int Userid)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
                    string str = "Delete from UserLabel where Userid=@Userid and Labelid=@Labelid;UPDATE UserTable SET FLabelnum=FLabelnum-1 where Userid=@Userid;UPDATE LabelTable SET LabelUnum = LabelUnum-1 where Labelid=@Labelid";
                sqlConnection.Open();
            using (SqlCommand sqlCommand = sqlConnection.CreateCommand()) {
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                try
                {
                    sqlCommand.CommandText = str;
                    sqlCommand.Transaction = sqlTransaction;
                    sqlCommand.Parameters.AddWithValue("@userid", Userid);
                    sqlCommand.Parameters.AddWithValue("@Labelid", labelid);
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
        /// 按照名字得到标签
        /// </summary>
        /// <param name="labelname"></param>
        /// <returns></returns>
        public Label Getlabel(string labelname)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            sqlConnection.Open();
            using (SqlCommand sqlCommand =sqlConnection.CreateCommand())
            {
                try
                {
                    string sql = "select *from LabelTable where Labelname=@labelname";
                    sqlCommand.CommandText = sql;
                    sqlCommand.Parameters.AddWithValue("@labelname", labelname);
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter
                    {
                        SelectCommand = sqlCommand,
                    };
                    DataTable dataTable = new DataTable();
                    sqlDataAdapter.Fill(dataTable);
                    Label label=new Label();
                    foreach (DataRow item in dataTable.Rows)
                    {
                        label = GetLabel(item);
                    }
                    return label;
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
        /// 判断一个人是否关注一个标签
        /// </summary>
        /// <param name="Userid"></param>
        /// <param name="labelid"></param>
        /// <returns></returns>
        public bool IsFollow(int Userid,int labelid)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            sqlConnection.Open();
            using (SqlCommand sqlCommand=sqlConnection.CreateCommand())
            {
                try
                {
                    string sql = $"select Labelid from UserLabel where Userid=@Userid and Labelid=@Labelid";
                    sqlCommand.CommandText = sql;
                    sqlCommand.Parameters.AddWithValue("@Userid", Userid);
                    sqlCommand.Parameters.AddWithValue("Labelid", labelid);
                    int label = Convert.ToInt32(sqlCommand.ExecuteScalar());
                    if (label == labelid)
                    {
                        return true;
                    }
                    return false;
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

        private Label GetLabel(DataRow dataRow)
        {
            Label label = new Label
            {
                Labelid = Convert.ToInt32(dataRow["Labelid"]),
                LabelCTime = Convert.ToDateTime(dataRow["LabelCTime"]),
                Labelname = dataRow["Labelname"].ToString(),
                LabelBrief = dataRow["LabelBrief"].ToString(),
                LabelUnum = Convert.ToInt32(dataRow["LabelUnum"]),
                LabelQnum = Convert.ToInt32(dataRow["LabelQnum"])
            };
            return label;
        }
    }
}
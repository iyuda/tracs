using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;

namespace DataLayer
{
    public class DataHelper
    {
        public static List<T> GetQueryList<T>(string query, string order_by=null)
        {   
            try
            {
            string strSQL = query.ToLower().StartsWith("select")? query : "SELECT * FROM rma." + query;
                if (order_by != null)
                    strSQL += " order by " + order_by;
            ExtendedDataTable tbl = new ExtendedDataTable();
            string conString = ConfigurationManager.ConnectionStrings["connectionString"].ToString();

            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                using (SqlCommand commd = new SqlCommand(strSQL, con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(commd);
                    da.Fill(tbl);
                }
                con.Close();
            }

            return tbl.toList<T>();
        }
            catch (Exception ex) { Logger.LogException(ex); return null; }
}
        public static DataTable GetQueryDataTable(string query)
        {
        try
        {
            string strSQL = query.ToLower().StartsWith("select") ? query : "SELECT * FROM " + query;
            ExtendedDataTable tbl = new ExtendedDataTable();
            string conString = ConfigurationManager.ConnectionStrings["connectionString"].ToString();

            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                using (SqlCommand commd = new SqlCommand(strSQL, con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(commd);
                    da.Fill(tbl);
                }
                con.Close();
            }

            return tbl;
}
            catch (Exception ex) { Logger.LogException(ex); return null; }
        }

        public static List<SqlParameter> GetSqlParameters(Dictionary<string, object[]> SQLParams)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            foreach (KeyValuePair<string, object[]> SQLParam in SQLParams)
            {
                var sqlParameter = new SqlParameter(SQLParam.Key, SQLParam.Value[1]);
                sqlParameter.Value = SQLParam.Value[0];
                sqlParameters.Add(sqlParameter);
            }
            return sqlParameters;
        }
        public static List<T> GetQueryListBySP<T>(string SpName, List<SqlParameter> SQLParams=null)
        {
            try
            {
                ExtendedDataTable tbl = new ExtendedDataTable();
                string conString = ConfigurationManager.ConnectionStrings["connectionString"].ToString();

                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("rma."+SpName, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (SQLParams!=null)
                            foreach (SqlParameter spp in SQLParams)
                            {
                                SqlParameter Param = new SqlParameter(spp.ParameterName, spp.Value);
                                Param.SqlDbType = spp.SqlDbType;
                                cmd.Parameters.Add(Param);
                            }
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(tbl);
                    }
                    con.Close();
                }

                return tbl.toList<T>();
            }
            catch (Exception ex) { Logger.LogException(ex); return null; }
        }
        public static dynamic  ActionQuery(string query)
        {
        try
        {
            string strSQL = query;
            ExtendedDataTable tbl = new ExtendedDataTable();
            string conString = ConfigurationManager.ConnectionStrings["connectionString"].ToString();
            int rows=0;
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(strSQL, con))
                {
                    rows = cmd.ExecuteNonQuery();
                }
                con.Close();
            }
            return rows > 0;    }
            catch (Exception ex) { Logger.LogException(ex); return ex.Message; }
        }
        public static List<object> GetCaseSummaries()
        {
            List<object> lst = new List<object>();

            DataTable tbl = DataHelper.GetQueryDataTable("select distinct case_no, email_name, status from RmaBase r left outer join RmaForms f on f.id=r.form_id  left outer join RmaStatuses s on s.id = r.status_id order by case_no");
            string case_no;
            string email_name;
            string status;
            if (tbl == null) return lst;
            foreach (DataRow dr in tbl.Rows)
            {
                case_no = dr["case_no"].ToString();
                email_name = dr["email_name"].ToString();
                status = dr["status"].ToString();
                lst.Add(new { case_no = case_no, form_name = email_name, status = status });
            }

            return lst;
        }
        

    }
}

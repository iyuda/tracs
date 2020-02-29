using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

    public class DbConnect
    {

     //   public static string DatabaseNewcems = "rma";
	    //public static string Server = "localhost";
//	    public static uint Port = 3306;
	//    public static string Username = "root";
	  //  public static string Pass = "";

    public static string ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ToString(); //GetFixedConnectionString(ConfigurationManager.ConnectionStrings["connectionString"].ToString());		
     
        public static DataTable GetDataTable(SqlConnection cn, string SelectQuery)
            {

            DataTable dt = new DataTable();
            SqlDataAdapter Adapter;
            Adapter = new SqlDataAdapter(SelectQuery,cn);
            SqlCommandBuilder cb = new SqlCommandBuilder(Adapter);
            Adapter.Fill(dt);
           
            Adapter.Dispose();
            return dt;


            }
      
    }


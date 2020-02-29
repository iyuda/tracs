using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;

namespace DataLayer
{
    public abstract class ItemDBHandler_old
    {
        private SqlConnection con;
        protected string TableName;
        public int? id { get; set; }
        public abstract void SaveChildren();
        public ItemDBHandler_old(int? id, string TableName="", string FieldName="id")
        {
            this.id = id;
            this.TableName = TableName==""? this.GetType().Name: TableName;
            this.Retrieve(FieldName);
            //    this.TableName = TableName;
        }

        public ItemDBHandler_old()
        {
            this.TableName = this.GetType().Name;
        }
        public Boolean Exists(int? id = 0)
        {
            try
            {

                string strConnect;
                //if (this.id + "" == "") this.id = Guid.NewGuid().ToString();
                if (id == 0 & this.id.HasValue) id = this.id;
                SqlConnection cn = null;
                strConnect = DbConnect.ConnectionString;
                

                bool CloseConnection = false;
                if (cn == null)
                {
                    cn = new SqlConnection(strConnect);
                    CloseConnection = true;
                }
                if (cn.State != ConnectionState.Open) cn.Open();

                string SqlString = "select count(*) from " + TableName + " where id = '" + id + "'"; 
                SqlCommand cmd = new SqlCommand(SqlString, cn);
                int rc = System.Convert.ToInt32(cmd.ExecuteScalar().ToString());
                if (CloseConnection) cn.Close();
                return rc > 0;
            }

            catch (Exception ex) { Logger.LogException(ex); return false; }
        }
        public Boolean Retrieve(string SearchField = "id")
        {
            try
            {
                string strConnect;
                strConnect = DbConnect.ConnectionString;
                using (SqlConnection cn = new SqlConnection(strConnect))
                {
                    cn.Open();

                    string SqlString = "select * from " + TableName + " where " + SearchField + " = '" + this.id.ToString() + "'";
                    // SqlCommand cmd = new SqlCommand(SqlString, cn);
                    DataTable dt = DbConnect.GetDataTable(cn, SqlString);
                    //dt.Load(cmd.ExecuteReader());

                    if (dt.Rows.Count > 0)
                    {
                        foreach (var prop in this.GetType().GetProperties().Where(prop => prop.PropertyType.FullName.StartsWith("System.") && !prop.Name.StartsWith("_")))
                        {
                            if (prop.PropertyType.FullName.StartsWith("System."))
                            {

                                string Value = dt.Rows[0][prop.Name].ToString();
                                if (Nullable.GetUnderlyingType(prop.PropertyType) == null)
                                    prop.SetValue(this, Value == "" ? null : Value);
                                else
                                    prop.SetValue(this, Value == "" ? null : (int?)int.Parse(Value));
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex) { Logger.LogException(ex); return false; }
        }

        public Boolean Retrieve(string[] SearchFields, string[] SearchValues)
        {
            try
            {
                string strConnect;
                StringBuilder Comparisons = new StringBuilder();


                strConnect = DbConnect.ConnectionString;

                using (SqlConnection cn = new SqlConnection(strConnect))
                {
                    cn.Open();
                    for (int i = 0; i <= SearchFields.Length - 1; i++)
                    {
                        Comparisons.Append(SearchFields[i] + " = '" + SearchValues[i] + "'" + (i < SearchValues.Length - 1 ? " and " : ""));
                    }
                    string SqlString = "select * from " + TableName + " where " + Comparisons.ToString();
                    // SqlCommand cmd = new SqlCommand(SqlString, cn);
                    DataTable dt = DbConnect.GetDataTable(cn, SqlString);
                    //dt.Load(cmd.ExecuteReader());

                    if (dt.Rows.Count > 0)
                    {
                        foreach (var prop in this.GetType().GetProperties().Where(prop => prop.PropertyType.FullName.StartsWith("System.") && !prop.Name.StartsWith("_")))
                        {
                            if (prop.PropertyType.FullName.StartsWith("System."))
                            {
                                string Value = dt.Rows[0][prop.Name].ToString();
                                if (Nullable.GetUnderlyingType(prop.PropertyType) == null)
                                    prop.SetValue(this, Value == "" ? null : Value);
                                else if (prop.PropertyType.FullName.Contains("Boolean"))
                                    prop.SetValue(this, Value == "" ? null : (bool?)bool.Parse(Value));
                                else
                                    prop.SetValue(this, Value == "" ? null : (int?)int.Parse(Value));
                            }
                        }
                    }
                }
                return true;
            }

            catch (Exception ex) { Logger.LogException(ex); return false; }
        }
        private void connection()
        {
            string constring = ConfigurationManager.ConnectionStrings["connectionString"].ToString();
            con = new SqlConnection(constring);
        }



        public Boolean Insert()
        {
            try
            {

                string strConnect;
                //if (this.id + "" == "") this.id = Guid.NewGuid().ToString();
                SqlConnection cn = null;
                strConnect = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
               


                bool CloseConnection = false;
                if (cn == null)
                {
                    cn = new SqlConnection(strConnect);
                    CloseConnection = true;
                }
                if (cn.State != ConnectionState.Open) cn.Open();
                //using (SqlConnection cn = new SqlConnection(strConnect))
                //    {
                //cn.Open();

                List<string> ColumnList = new List<string>();
                List<string> ValueList = new List<string>();

                foreach (var prop in this.GetType().GetProperties().Where(prop => prop.PropertyType.FullName.StartsWith("System.") && prop.Name!="id" && !prop.Name.StartsWith("_")))
                {
                    var Value = prop.GetValue(this, null);
                    //if (prop.GetType().Name=="Image")
                    //{
                    //    Value = this.ConvertToByte( (HttpPostedFileBase) Value);
                    //}
                    //if (prop.Name == "ImageBody")
                    //{
                    //    if(Value != null) SaveImageToFile(Value);
                    //    continue;
                    //}
                    //if (prop.GetType().Name.EndsWith("image_path") & Value != null)
                    //{
                    //    SaveImageToFile(Value);
                    //}
                    
                    if (prop.Name=="case_no")
                    {
                        Value = GetNextCaseNo();
                        prop.SetValue(this, Value.ToString());
                    }
                    if (prop.Name == "registration_no")
                    {
                        Value = GetNextRegNo();
                        prop.SetValue(this, Value.ToString());
                    }
                    var temp = Value + "";

                    if (Value != null) Value = Value.ToString().Replace("~", "");
                    ColumnList.Add(prop.Name);

                    Boolean boolValue;
                    if (Boolean.TryParse(Value == null ? "" : Value.ToString(), out boolValue))
                    {
                        ValueList.Add(Convert.ToInt16(Convert.ToBoolean(Value)).ToString());
                    }
                    else if (Value != null)
                    {
                        Value = Value.ToString().Replace(System.Environment.NewLine, "\r");
                        if (Value.ToString().Contains("()"))
                        {
                            ValueList.Add(Value.ToString());
                        }
                        else
                        {
                            if (Value.ToString().Contains("'")) Value = Value.ToString().Replace("'", "''");
                            ValueList.Add(Value == null ? "null" : "'" + Value + "'");
                        }
                    }
                    else
                    {
                        ValueList.Add("null");
                    }
                    
                }
                string InsertString = "insert into " + TableName + "(" + string.Join(",", ColumnList.ToArray()) + ")" + System.Environment.NewLine + "values" + System.Environment.NewLine + "(" + string.Join(",", ValueList.ToArray()) + ")";

                SqlCommand cmd = new SqlCommand(InsertString, cn);
                Logger.LogAction(InsertString);
                int rows = cmd.ExecuteNonQuery();
                cmd.CommandText = "select @@IDENTITY";
                dynamic return_value = cmd.ExecuteScalar();
                if (return_value.ToString() != "")
                {
                    int id = Convert.ToInt32(return_value ?? 0);
                    this.id = id;
                }
                //     }
                if (CloseConnection) cn.Close();
                return rows>0;
            }
            catch (Exception ex) { Logger.LogException(ex); return false; }
        }
        public Boolean Delete(string[] Fields, string[] Values)
        {
            try
            {
                StringBuilder Comparisons = new StringBuilder();
                SqlConnection cn = null;
                string strConnect = ConfigurationManager.ConnectionStrings["connectionString"].ToString();

                bool CloseConnection = false;
                if (cn == null)
                {
                    cn = new SqlConnection(strConnect);
                    CloseConnection = true;
                }
                if (cn.State != ConnectionState.Open) cn.Open();
                //using (SqlConnection cn = new SqlConnection(strConnect))
                //    {
                //cn.Open();
                for (int i = 0; i <= Fields.Length - 1; i++)
                {
                    Comparisons.Append(Fields[i] + " = '" + Values[i] + "'" + (i < Fields.Length - 1 ? " and " : ""));
                }


                string DeleteString = "delete from " + TableName + " where " + Comparisons.ToString(); // WorkObject.GetType().GetProperty("id").GetValue(WorkObject, null) + "'";
                SqlCommand cmd = new SqlCommand(DeleteString, cn);
                int rows = cmd.ExecuteNonQuery();
                if (CloseConnection) cn.Close();
                if (!this.GetType().Name.ToLower().StartsWith("json_")) Logger.LogAction(DeleteString);
                return rows > 0;
                // }

            }
            catch (Exception ex) { Logger.LogException(ex); return false; }
        }
        public void SaveImageToFile(object Image )
        {
            string FileName = this.GetType().GetProperties().FirstOrDefault(prop1 => prop1.Name == "ImagePath").GetValue(this, null).ToString();
            string ImagesDirectory = ConfigTools.GetConfigValue("Images Path", "U:\\temp\\Images\\");
            if (!Directory.Exists(ImagesDirectory))
                try
                {
                    Directory.CreateDirectory(ImagesDirectory);
                }
                catch
                {

                }
            FileName = ImagesDirectory + "\\" + FileName;
            ((HttpPostedFileBase)Image).SaveAs(FileName);
        }
        public Boolean Update(bool DynamicWhere = false, string[] WhereColumns = null, string[] WhereValues = null)
        {
            try
            {
                if (WhereColumns == null) WhereColumns = new string[] { "id" };
                string strConnect;
                SqlConnection cn = null;
                strConnect = ConfigurationManager.ConnectionStrings["connectionString"].ToString();


                bool CloseConnection = false;
                if (cn == null)
                {
                    cn = new SqlConnection(strConnect);
                    CloseConnection = true;
                }
                if (cn.State != ConnectionState.Open) cn.Open();
                string UpdateString = "update " + TableName + " set " + System.Environment.NewLine;
                List<string> Assignments = new List<string>();
                foreach (var prop in this.GetType().GetProperties().Where(prop => !WhereColumns.ToList().Contains(prop.Name) && prop.Name != "id" && prop.PropertyType.FullName.StartsWith("System.") && !prop.Name.StartsWith("_")))
                {
                    var Value = prop.GetValue(this, null);

                    //if (prop.Name == "ImageBody")
                    //{
                    //    if (Value != null) SaveImageToFile(Value);
                    //    continue;
                    //}
                    if (prop.Name == "case_no")
                    {
                        Value = GetNextCaseNo();
                        prop.SetValue(this, Value);
                    }
                    var temp = Value + "";
                    if (Value != null) Value = Value.ToString().Replace("~", "");
                    Boolean boolValue;
                    if (Boolean.TryParse(Value == null ? "" : Value.ToString(), out boolValue))
                        Value = Convert.ToInt16(Convert.ToBoolean(Value)).ToString();

                    string Element;
                    if (Value != null)
                    {
                        if (Value.ToString().Contains("()"))
                        {
                            Element = prop.Name + " = " + Value;
                        }
                        else
                        {
                            if (Value.ToString().Contains("'")) Value = Value.ToString().Replace("'", "''");
                            Element = prop.Name + " = '" + Value + "'";
                        }
                        //if (prop.Name != KeyField)
                        Assignments.Add(Element);
                    }
                    else
                    {
                        Element = prop.Name + " = null ";
                        Assignments.Add(Element);
                    }

                    //DateTime dateValue;
                    //bool IsDate = false;
                    //if (!temp.ToString().Contains("~")) //  having "~" in a value indicates that it should not be attemtped date parsing
                    //    if (!Utilities.IsNumeric(Value) && DateTime.TryParse(Value == null ? "" : Value.ToString(), out dateValue))
                    //    {
                    //        //  Value = "DATE_FORMAT('" + Convert.ToDateTime(Value).ToString("yyyy-MM-dd HH:mm:ss") + "', '%Y:%m:%d %H:%i:%s')";
                    //        IsDate = true;
                    //    }

                    // string MaybeComma = i < this.GetType().GetProperties().Length - 1 ? "," : "";



                    //if (Value != null)
                    //{
                    //    if (!IsDate)
                    //    {
                    //        if (Value.ToString().Contains("'")) Value = Value.ToString().Replace("'", "''");
                    //        string Element = prop.Name + " = " + (Value == null ? "null" : "'" + Value + "'");
                    //        //if (prop.Name != KeyField)
                    //        Assignments.Add(Element);
                    //    }
                    //    else
                    //    {
                    //        string Element = prop.Name + " = " + (Value.ToString());
                    //        //if (prop.Name != KeyField) 
                    //        Assignments.Add(Element);
                    //    }
                    //}

                }

                if (Assignments.Count > 0)
                {
                    UpdateString += string.Join("," + System.Environment.NewLine, Assignments.ToArray());
                    if (WhereColumns != null)
                    {
                        StringBuilder Comparisons = new StringBuilder();
                        for (int i = 0; i <= WhereColumns.Length - 1; i++)
                        {
                            string MaybeAnd = i < WhereColumns.Length - 1 ? " and " : "";
                            if (WhereValues == null)
                                Comparisons.Append(WhereColumns[i] + " = '" + this.GetType().GetProperties().First(prop => prop.Name == WhereColumns[i]).GetValue(this, null) + "'" + MaybeAnd + System.Environment.NewLine);
                            else
                                Comparisons.Append(WhereColumns[i] + " = '" + WhereValues[i] + "'" + MaybeAnd + System.Environment.NewLine);
                        }
                        if (WhereColumns.Length > 0)
                            UpdateString += " where " + Comparisons.ToString();
                        else
                            UpdateString += " where id ='" + this.id + "'";
                    }
                    else
                        UpdateString += " where id ='" + this.id + "'";
                    SqlCommand cmd = new SqlCommand(UpdateString, cn);
                    int rows = cmd.ExecuteNonQuery();
                    Logger.LogAction(UpdateString);
                    if (CloseConnection) cn.Close();
                    return rows > 0;
                }
                else
                    return true;
                //  }

            }
            catch (Exception ex) { Logger.LogException(ex); return false; }
        }

        public string GetNextCaseNo()
        {
            try
            {

                string strConnect;
                
                SqlConnection cn = null;
                strConnect = DbConnect.ConnectionString;


                bool CloseConnection = false;
                if (cn == null)
                {
                    cn = new SqlConnection(strConnect);
                    CloseConnection = true;
                }
                if (cn.State != ConnectionState.Open) cn.Open();
                string StartingNo = ConfigTools.GetConfigValue("Starting Case Number", "10000");
                SqlCommand cmd = new SqlCommand("GetNextCaseNo", cn);
                cmd.Parameters.Add(new SqlParameter("@StartingNo", SqlDbType.Int)).Value=StartingNo;
                cmd.CommandType = CommandType.StoredProcedure;
                string rv = cmd.ExecuteScalar().ToString();
                if (CloseConnection) cn.Close();
                return rv;
            }

            catch (Exception ex) { Logger.LogException(ex); return null; }
        }
        public string GetNextRegNo()
        {
            try
            {

                string strConnect;

                SqlConnection cn = null;
                strConnect = DbConnect.ConnectionString;


                bool CloseConnection = false;
                if (cn == null)
                {
                    cn = new SqlConnection(strConnect);
                    CloseConnection = true;
                }
                if (cn.State != ConnectionState.Open) cn.Open();
                string StartingNo = ConfigTools.GetConfigValue("Starting Registration Number", "10000");
                SqlCommand cmd = new SqlCommand("GetNextRegNo", cn);
                cmd.Parameters.Add(new SqlParameter("@StartingNo", SqlDbType.Int)).Value = StartingNo;
                cmd.CommandType = CommandType.StoredProcedure;
                string rv = cmd.ExecuteScalar().ToString();
                if (CloseConnection) cn.Close();
                return rv;
            }

            catch (Exception ex) { Logger.LogException(ex); return null; }
        }
        public Boolean Update(string[] Columns, string[] WhereColumns)
        {
            try
            {
                string strConnect;
               
               
                strConnect = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
               
                using (SqlConnection cn = new SqlConnection(strConnect))
                {
                    cn.Open();
                    string UpdateString = "update " + TableName + " set " + System.Environment.NewLine;
                    StringBuilder Assignments = new StringBuilder();
                    for (int i = 0; i <= Columns.Length - 1; i++)
                    {
                        var column = Columns[i];
                        var Value = this.GetType().GetProperties().First(prop => prop.Name == column).GetValue(this, null);
                        var temp = Value + "";
                        if (Value != null) Value = Value.ToString().Replace("~", "");
                        Boolean boolValue;
                        if (Boolean.TryParse(Value == null ? "" : Value.ToString(), out boolValue))
                            Value = Convert.ToInt16(Convert.ToBoolean(Value)).ToString();

                        //DateTime dateValue;
                        //if (!Utilities.IsNumeric(Value) && DateTime.TryParse(Value == null ? "" : Value.ToString(), out dateValue))
                        //    Value = Convert.ToDateTime(Value).ToString("yyyy-MM-dd hh:mm:ss");


                        string MaybeComma = i < Columns.Length - 1 ? "," : "";

                        if (column != "id") Assignments.Append(column + " = " + (Value == null ? "null" : "'" + Value + "'") + MaybeComma + System.Environment.NewLine);
                    }
                    StringBuilder Comparisons = new StringBuilder();
                    for (int i = 0; i <= WhereColumns.Length - 1; i++)
                    {
                        string MaybeAnd = i < WhereColumns.Length - 1 ? " and " : "";
                        Comparisons.Append(WhereColumns[i] + " = '" + this.GetType().GetProperties().First(prop => prop.Name == WhereColumns[i]).GetValue(this, null) + "'" + MaybeAnd + System.Environment.NewLine);
                    }
                    if (WhereColumns.Length > 0)
                        Assignments.Append("where " + Comparisons.ToString()); //.GetProperties()["id"].GetValue(WorkObject,null) + "'");
                    UpdateString += Assignments.ToString();
                    SqlCommand cmd = new SqlCommand(UpdateString, cn);
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }

            }
            catch (Exception ex) { Logger.LogException(ex); return false; }
        }
        
       
        public bool InsertUpdateAction(int InsertUpdate = 0, string[] WhereColumns = null, string[] WhereValues = null)
        {
            bool rv = false;
            switch (InsertUpdate)
            {
                case 0:
                    if (Exists())
                        rv = Update();
                    else
                        rv = Insert();
                    break;
                case 1:
                    rv = Insert();
                    break;
                case 2:
                    rv = Update();
                    break;
            }
            return rv;
        }

        public bool Save()
        {
            this.SaveChildren();
            return this.InsertUpdateAction(0);
        }
        public byte[] ConvertToByte(HttpPostedFileBase file)
        {
            byte[] imageByte = null;
            BinaryReader rdr = new BinaryReader(file.InputStream);
            imageByte = rdr.ReadBytes((int)file.ContentLength);
            return imageByte;
        }
        public dynamic GetValue(dynamic className, dynamic propertyName, dynamic index = null)
        {
            if (index == null)
                return this.GetType().GetProperty(propertyName.ToString()).GetValue(this, null);
            else
            {
                List<dynamic> list = (List<dynamic>)this.GetType().GetProperty(className.ToString());
                return list[index].GetType().GetProperty(propertyName.ToString()).GetValue(this, null);
            }
        }
       
        //// 1. ********** Insert Item **********
        //public bool InsertItem(ItemModel iList)
        //{
        //    connection();
        //    string query = "INSERT INTO ItemList VALUES('" + iList.Name + "','" + iList.Category + "'," + iList.Price + ")";
        //    SqlCommand cmd = new SqlCommand(query, con);
        //    con.Open();
        //    int i = cmd.ExecuteNonQuery();
        //    con.Close();

        //    if (i >= 1)
        //        return true;
        //    else
        //        return false;
        //}

        //// 2. ********** Get All Item List **********
        //public List<ItemModel> GetItemList()
        //{
        //    connection();
        //    List<ItemModel> iList = new List<ItemModel>();

        //    string query = "SELECT * FROM ItemList";
        //    SqlCommand cmd = new SqlCommand(query, con);
        //    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        //    DataTable dt = new DataTable();

        //    con.Open();
        //    adapter.Fill(dt);
        //    con.Close();

        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        iList.Add(new ItemModel
        //        {
        //            ID = Convert.ToInt32(dr["ID"]),
        //            Name = Convert.ToString(dr["Name"]),
        //            Category = Convert.ToString(dr["Category"]),
        //            Price = Convert.ToDecimal(dr["Price"])
        //        });
        //    }
        //    return iList;
        //}

        //// 3. ********** Update Item Details **********
        //public bool UpdateItem(RMATrackerViewModel iList)
        //{
        //    connection();
        //    string query = "UPDATE ItemList SET Name = '" + iList.Name + "', Category = '" + iList.Category + "', Price = " + iList.Price + " WHERE ID = " + iList.ID;
        //    SqlCommand cmd = new SqlCommand(query, con);
        //    con.Open();
        //    int i = cmd.ExecuteNonQuery();
        //    con.Close();

        //    if (i >= 1)
        //        return true;
        //    else
        //        return false;
        //}

        //// 4. ********** Delete Item **********
        //public bool DeleteItem(int id)
        //{
        //    connection();
        //    string query = "DELETE FROM ItemList WHERE ID = " + id;
        //    SqlCommand cmd = new SqlCommand(query, con);
        //    con.Open();
        //    int i = cmd.ExecuteNonQuery();
        //    con.Close();

        //    if (i >= 1)
        //        return true;
        //    else
        //        return false;
        //}
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace DataLayer
{

     public class ExtendedDataTable: DataTable
     {
        public List<object> toList()
        {
            List<object> list = new List<object>();
            foreach (DataRow dr in this.Rows)
            {
                list.Add(dr);
            }
            return list;
        }
        public  List<T> toList<T>()
        {
               DataTable table = this;
                List<T> list = new List<T>();

                T item ;
                Type listItemType = typeof(T);

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    item = (T) Activator.CreateInstance(listItemType);
                    mapRow(item, table, listItemType, i);
                    list.Add(item);
                }
            return list;
        }
        private static void mapRow(object vOb, System.Data.DataTable table, Type type, int row)
        {
            for (int col = 0; col < table.Columns.Count; col++)
            {
                var columnName = table.Columns[col].ColumnName;
                var prop = type.GetProperty(columnName);
                if (prop == null)
                {
                    string workColumnName = columnName.ToLower();
                    if (workColumnName.EndsWith("id") && workColumnName.Replace("id","")== type.Name.ToLower())
                        prop = type.GetProperty("id");
                    else
                        continue;
                }
                object data = table.Rows[row][col];     
                if (data == DBNull.Value)
                    if (prop.PropertyType.Name == "String")
                        prop.SetValue(vOb, data + "", null);
                    else
                        prop.SetValue(vOb, null, null);
                else 
                    if (prop.PropertyType.Name=="String")
                        prop.SetValue(vOb, data.ToString(), null);
                    else
                        prop.SetValue(vOb, data, null);
            }
        }

     }
}
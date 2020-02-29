using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DataLayer   

{

    public class CallSQL
    {
        //public static List<Parts>GetParts(string rma_details_id)
        //{
        //    List<Parts> lst = new List<Parts>();
        //    DataTable tbl = DataHelper.GetQueryResults("[Person].StateProvince");
        //    Parts part;

        //    foreach(DataRow dr in tbl.Rows)
        //    {
        //        part = new Parts();
        //        part.serial_no = dr["serial_no"].ToString();
        //        part.PartNo = dr["PartNo"].ToString();
        //        lst.Add(part);
        //    }
        //    return lst;
        //}

        public static List<PartTypes> GetPartTypes()
        {
            return DataHelper.GetQueryList <PartTypes>("PartTypes");
            //List<PartTypes> lst = new List<PartTypes>();
            //DataTable tbl = DataHelper.GetQueryDataTable("PartTypes");
            //PartTypes part;

            //foreach (DataRow dr in tbl.Rows)
            //{
            //    part = new PartTypes();
            //    part.PartNo = dr["PartNo"].ToString();
            //    part.PartDescription = dr["PartDescription"].ToString();
            //    lst.Add(part);
            //}
            //return lst;
        }
        
        public static List<object> GetCaseSummaries()
        {
            List<object> lst = new List<object>();

            DataTable tbl = DataHelper.GetQueryDataTable("select distinct case_no, email_name, status from RMABase r left outer join forms f on f.id=r.form_id  left outer join statuses s on s.id = r.status_id order by case_no");
            string case_no;
            string email_name;
            string status;
            foreach (DataRow dr in tbl.Rows)
            {
                case_no = dr["case_no"].ToString();
                email_name = dr["email_name"].ToString();
                status = dr["status"].ToString();
                lst.Add(new { case_no = case_no, form_name = email_name, status=status });
            }

            return lst;
        }
        
       

    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using DataLayer;

namespace RMATracker.Models
{
    public interface IEnvironmentModel
    {
        string userName { get; set; }
        string userPassword { get; set; }

        User user { get; set; }
        Firm firm { get; set; }
        List<Bank> banks { get; set; }
        List<DispatchReason> ispatchReason { get; set; }
        List<Part> parts { get; set; }

        //public int select_attribute int { get; set; }



        //public void LoadUserInfo()
        //{
        //    LoadRelUser();

        //    LoadAttributes();

        //    Dictionary<string, object[]> parameters = new Dictionary<string, object[]>();
        //    parameters.Add("UserId", new object[] { this.user.id.ToString(), SqlDbType.Int });
        //    List<SqlParameter> sqlParameters = DataHelper.GetSqlParameters(parameters);
        //    this.attributes = DataHelper.GetQueryListBySP<DataLayer.Attribute>("GetAttributesByUserId", sqlParameters);
        //}



    }
   
}
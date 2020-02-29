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

using DataLayer;

namespace DataLayer
{
    public class Globals
    {
        public static int init;
        public static string userName { get; set; }
        public static string userPassword { get; set; }

        public static User user { get; set; }
        //public static Firm firm { get; set; }

        public static List<Bank> banks { get; set; }
        public static List<Firm> firms { get; set; }
        public static List<Complaint> complaints { get; set; }
        public static List<Part> parts { get; set; }
        public static List<Fault> faults { get; set; }
        public static List<Attribute> attributes { get; set; }

        //public static Bank SelectedBank { get; set; }
        static Globals()
        {
            userName = "igory";
            userPassword = "secret";
          //  user = new User(1);
            //firm = new Firm(1);
            complaints = new List<Complaint>() { new Complaint(1) };
            parts = new List<Part>() {
                new Part{
                    id =1,
                    PartNumber="PartNumber1",
                    PartName = "Desciption1",
                },
                new Part{
                    id =2,
                    PartNumber="PartNumber2",
                    PartName = "Desciption2",
                }
            };
           faults = new List<Fault>() {
                new Fault{
                    id =1,
                    Description="Problem 1"
                },
                new Fault{
                    id =2,
                    Description="Problem 2"
                }
            };
            complaints = new List<Complaint>() {
                new Complaint{
                    id =1,
                    Name="Complaint1",
                    Description="Complaint Description 1"
                },
                new Complaint{
                    id =1,
                    Name="Complaint2",
                    Description="Complaint Description 2"
                }
            };
            //SelectedBank = banks[0];

            firms = new List<Firm>() { new Firm (1)  { Name = "Securitas"}, new Firm(2) { Name = "Parabit"} };
            foreach (Firm firm in firms)
            {
                firm.LoadRelations();
            }
            banks = new List<Bank>() { new Bank(1) { Name = "Chase", LongName = "Chase Long" }, new Bank(2) { Name = "BofA", LongName = "BofA Long" } };
            attributes = new List<Attribute>() { new Attribute(1) { Name = "Internal Tech (\"Parabit\")" }, new Attribute(2) { Name = "External Tech (\"Securitas\")" } };
            //            banks[0].LoadRelations();

        }
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
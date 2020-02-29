using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class User : ItemDBHandler
    {
        public User(int id)
        : base(id)
        {

        }
        public User() {
            }
        
        public string Name { get; set; }

        public string Phone { get; set; }
        public string Email { get; set; }
        public string FirmName { get; set; }
        public string Password { get; set; }
        public int? PasswordTypeId { get; set; }
        public int? FirmId { get; set; }


        //public Firm firm { get; set; }
        //public PasswordType passwordType { get; set; }
        //public TempPassword tempPassword { get; set; }

        public List<REL_User_Attribute> rel_User_Attributes { get; set; }
        //public List<ReturnAddress> ReturnAddresses { get; set; }



       

        public void LoadRelUserAttributes()
        {
            Test_LoadRelUserAttributes(); return;
           
        }

        //public void LoadFirm()
        //{
        //    firm = Firm.firms.FirstOrDefault(x => x.id == FirmId); return;

        //}
        //public void LoadPasswordType()
        //{
        //    passwordType = new PasswordType
        //    {
        //        id = this.PasswordTypeId,
        //        Description = "Normal",
        //        Duration=null
        //    };
        //    if (passwordType.Description.ToLower().Contains("temporary"))
        //    {
        //        LoadTempPassword();
        //    }
        //}
        //public void LoadTempPassword()
        //{
        //    tempPassword = new TempPassword
        //    {
        //        UserId = this.id,
        //        Password = "1234567"
        //    };

        //}
        //public static List<User> GetValidUsers(string userName, string userPassword)
        //{
        //    return Test_GetValidUsers().Where(x=>x.Password==userPassword && x.Email==userName).ToList();

        //    Dictionary<string, object[]> parameters = new Dictionary<string, object[]>();
        //    parameters.Add("Name", new object[] { userName, SqlDbType.VarChar });
        //    parameters.Add("Password", new object[] { userPassword, SqlDbType.VarChar });
        //    List<SqlParameter> sqlParameters = DataHelper.GetSqlParameters(parameters);

        //    List<User> users = DataHelper.GetQueryListBySP<User>("UserGetByLogin", sqlParameters);

        //    return users;
        //}
       public static List<User> GetAllUsers()
        {
            return new List<User>
            {
                new User{
                id = 1,
                Name = "igory",
                Password = "secret",
                Email="igory@parabit.com",
                Phone="5161111111",
                FirmId=1
                }
            };

        }

       
        private void Test_LoadRelUserAttributes()
        {
            rel_User_Attributes = new List<REL_User_Attribute>() { new REL_User_Attribute(1) { UserId = this.id, AttributeId=1 },
                                                         new REL_User_Attribute(2) { UserId = this.id, AttributeId=2}
                                                        };
            foreach (REL_User_Attribute rel_User_Attribute in rel_User_Attributes)
            {
                rel_User_Attribute.LoadRelations();
            }

        }
        private static List<User> Test_GetValidUsers()
        {
            return new List<User>
            {
                new User{
                 id = 1,
                Name = "igory",
                Password = "secret",
                Email="igory@parabit.com",
                Phone="5161111111",
                FirmId=1
                }
            };

        }
    }
}

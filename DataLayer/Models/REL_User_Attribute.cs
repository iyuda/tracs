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
    public class REL_User_Attribute : ItemDBHandler
    {
        public REL_User_Attribute(int id)
        : base(id)
        {

        }
        public REL_User_Attribute() { }

        public int? UserId { get; set; }
        public int? AttributeId { get; set; }

        public Attribute attribute { get; set; }
        public User user { get; set; }

        public List<REL_TechField_ReturnAddress> rel_TechField_ReturnAddresses { get; set; }

        public void LoadRelations()
        {
            LoadAttribute();
            LoadRelTechFieldReturnAddress();
        }
        public void LoadRelTechFieldReturnAddress()
        {
            rel_TechField_ReturnAddresses = new List<REL_TechField_ReturnAddress>()
            {
                new REL_TechField_ReturnAddress() {
                    id = 1,
                    REL_User_Attribute_Id = (int) (this.id ?? 0),
                    returnAddress= new ReturnAddress() { id=1, Street="Street1",City="Roosevelt", ReturnStateId=1, ZipCode="11111", FullAddress="full address 1"},
                    ReturnAddressId=1,
                },
                    new REL_TechField_ReturnAddress() {
                    id = 2,
                    REL_User_Attribute_Id = (int) (this.id ?? 0),
                    returnAddress= new ReturnAddress() { id=2, Street="Street2",City="Roosevelt2", ReturnStateId=2, ZipCode="22222", FullAddress="full address 2", IsDefault=true},
                    ReturnAddressId=2,
                }
            };
            return;

            
        }
        public void LoadAttribute()
        {
            attribute = Test_LoadAttribute();
            return;
            //parameters.Clear();
            //parameters.Add("id", new object[] { this.user.id.ToString(), SqlDbType.Int });
            //List<SqlParameter> sqlParameters = DataHelper.GetSqlParameters(parameters);
            //this.attributes =  DataHelper.GetQueryListBySP<DataLayer.Attribute>("AttributeGetById", sqlParameters);
            ////if (result.Count > 0)
            ////{
            ////    this.attribute = result[0];
            ////}
        }
      
        public void LoadUser()
        {
            parameters.Clear();
            parameters.Add("id", new object[] { this.UserId.ToString(), SqlDbType.Int });
            List<SqlParameter> sqlParameters = DataHelper.GetSqlParameters(parameters);
            var result = DataHelper.GetQueryListBySP<User>("UserGetById", sqlParameters);
            if (result.Count > 0)
            {
                this.user = result[0];
            }
        }


       
        private Attribute Test_LoadAttribute()
        {
            return Attribute.attributes.FirstOrDefault(x => x.id == this.AttributeId);
        }
    }

}
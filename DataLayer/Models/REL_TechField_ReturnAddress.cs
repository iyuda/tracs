using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class REL_TechField_ReturnAddress : ItemDBHandler
    {
        public REL_TechField_ReturnAddress(int id)
        : base(id)
        {

        }
        public REL_TechField_ReturnAddress()
        { }

        [Display(Name = "UserAttributeId")]
        [Required]
        public Int32 REL_User_Attribute_Id { get; set; }

        [Display(Name = "ReturnAddressId")]
        [Required]
        public Int32 ReturnAddressId { get; set; }


        public ReturnAddress returnAddress { get; set; }
        public REL_User_Attribute rel_User_Attribute { get; set; }
    }
}
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    [DataContract]
    [JsonObject(MemberSerialization.OptIn)]
    public class ReturnAddress : ItemDBHandler
    {
        public ReturnAddress(int id)
        : base(id)
        {

        }
        public ReturnAddress() { }

        [JsonProperty]
        public int ReturnAddressId { get; set; }
        [JsonProperty]
        public string FullAddress { get; set; }

        [JsonProperty]
        public string UserId { get; set; }


        [JsonProperty(PropertyName = "StreetAddress")]
        [DataMember(Name = "StreetAddress")]
        public string Street { get; set; }
        [JsonProperty]
        public string City { get; set; }

        [Display(Name = "ReturnStateId")]
        [JsonProperty(PropertyName = "StateId")]
        [DataMember]
        public int? ReturnStateId { get; set; }

        [Display(Name = "ReturnCountryId")]
        [JsonProperty(PropertyName = "CountryId")]
        [DataMember]
        public int? ReturnCountryId { get; set; }

        [JsonProperty]
        public string StateName { get; set; }

        [JsonProperty(PropertyName = "Zip")]
        [DataMember(Name = "ZipCode")]
        public string ZipCode { get; set; }

        public int? FirmId { get; set; }
        //[JsonProperty]
        [JsonConverter(typeof(BoolConverter))]
        [DataMember]
        public bool IsDefault { get; set; }
        
        public Firm firm { get; set; }
        
        public State state { get; set; }


        public static List<ReturnAddress> fullList { get; set; }
        public REL_TechField_ReturnAddress rel_TechField_ReturnAddress { get; set; }


        public void LoadRelations()
        {
            LoadRelTechFieldReturnAddress();
        }
        public void LoadRelTechFieldReturnAddress()
        {
            rel_TechField_ReturnAddress = new REL_TechField_ReturnAddress()
            {
                id = 1,
                ReturnAddressId = (int)this.id
            };
            return;

           
        }
        

    }
}
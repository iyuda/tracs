using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;


namespace DataLayer
{
    public class UserProfile
    {

        public UserProfile()
        {
        }

        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        public int ReturnAddressId { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string FirmId { get; set; }
        public string FirmTypeId { get; set; }
        public string FirmName { get; set; }
        public string Password { get; set; }
        public string StateName { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }

        [JsonProperty(PropertyName = "StateId")]
        public int? ProfileStateId { get; set; }
        [JsonProperty(PropertyName = "CountryId")]
        public int? ProfileCountryId { get; set; }
        public string Zip { get; set; }
        public bool IsSubcontractor { get; set; }
        public string PrimecontractorId { get; set; }
        public string PrimecontractorName { get; set; }




    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class UserRegistration
    {

        public UserRegistration()
        {
        }


        public string FirstName { get; set; }
        public string LastName { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "Phone Number Required!")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "###) ### - ####")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",
                   ErrorMessage = "Entered phone format is not valid.")]
        public string Phone { get; set; }
        public string Email { get; set; }
        public string FirmId { get; set; }
        public string Password { get; set; }

        public string StreetAddress { get; set; }
        public string City { get; set; }
        [JsonProperty(PropertyName = "StateId")]
        public int? ProfileStateId { get; set; }
        public string Zip { get; set; }

        public bool IsSubcontractor { get; set; }
        public string PrimecontractorId { get; set; }

    }
}

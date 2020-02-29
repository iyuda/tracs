using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class StateInfo
    {
        public string StateCode { get; set;}
        public string StateName { get; set;}
        public string CountryCode { get; set;}

        public override string ToString()
        {
            return StateName.Trim() + "  [" + CountryCode.Trim() + "]";
        }
    }

    
}

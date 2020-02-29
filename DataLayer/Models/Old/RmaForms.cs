using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class RmaForms : ItemDBHandler
    {
        public RmaForms(int? id = 0)
           : base(id)
        {

        }
        public RmaForms()
        {
        }

        public string form_name { get; set; }
        public string form_title { get; set; }
        public string email_address { get; set; }
        public string email_name { get; set; }

        public override void SaveChildren()
        {
        }
    }
}

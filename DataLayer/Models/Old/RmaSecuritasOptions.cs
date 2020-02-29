using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class RmaSecuritasOptions :ItemDBHandler
    {
        public RmaSecuritasOptions(int? id=0)
           : base(id)
        {
            GetReturnType();
            GetCreditReason();
        }
        public RmaSecuritasOptions() { }

        public string _ReturnTypeDescription { get; set; }
        public string _CreditReasonDescription { get; set; }
        public string ReturnType { get; set; }
        public string CreditReason { get; set; }

        public string GetReturnType()
        {

            switch (CreditReason)
            {
                case "0":
                    _ReturnTypeDescription = "Service request";
                    break;
                case "1":
                    _ReturnTypeDescription = "Credit request";
                    break;
                default:
                    _ReturnTypeDescription = "";
                    break;
            }
            return _ReturnTypeDescription;
        }
        public string GetCreditReason()
        {
            switch (CreditReason)
            {
                case "0":
                    _CreditReasonDescription = "Job cancelled";
                    break;
                case "1":
                    _CreditReasonDescription = "No longer required (excess inventory)";
                    break;
                case "2":
                    _CreditReasonDescription = "Wrong part ordered";
                    break;
                case "3":
                    _CreditReasonDescription = "Wrong color";
                    break;
                default:
                    _CreditReasonDescription = "";
                    break;
            }
            return _CreditReasonDescription;
        }
        public override void SaveChildren()
        {
        }

    }
}

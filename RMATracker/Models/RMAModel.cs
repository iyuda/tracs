using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using DataLayer;
namespace RMATracker.Models
{
    public class RMAModel : IDisposable
    {
        

        public RMA rma { get; set; }
        public TechInfo techInfo { get; set; }
        public DataLayer.Attribute attribute { get; set; }
        public int? BankIncludeType=0;
        public int? FirmTypeId;

        public bool IsQuery { get; set; }

       

       
        public RMAModel()
        {
    

        }
        public RMAModel(string token, User user, string PrimecontractorName = null, int? rmaId = null)
        {

            if (rmaId == null)
            {
                this.rma = new RMA();
            }
            else
                this.rma = new RMA((int)rmaId);
            this.rma.LoadRelations(token);
            this.techInfo = new TechInfo();
            this.techInfo.firmName = user.FirmName;
            this.techInfo.contractorName = PrimecontractorName;
            this.techInfo.user = user;




        }
        public RMAModel(DataLayer.Attribute attribute, User user=null, int? rmaId=null)
        {

            if (rmaId == null)
            {
                this.rma = new RMA();
                if (user != null)
                {
                    this.rma.technician = user.rel_User_Attributes.FirstOrDefault(x=>x.AttributeId==attribute.id);
                    if (this.rma.technician!=null)
                        this.rma.technician.user = user;
                }
            }
            else
                this.rma = new RMA((int)rmaId);
            
            this.attribute = attribute;

        }
        
        public RMAModel(string CaseNo)
        {
        }
        
        void IDisposable.Dispose()
        {

        }
      




    }
}
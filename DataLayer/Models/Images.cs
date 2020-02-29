using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DataLayer
{
    public class Images : ItemDBHandler
    {
        public Images(int? id=0)
           : base(id)
        {

        }
        public HttpPostedFileBase _ImageBody { get; set; }
        public Images() {   }
        public int? rma_id { get; set; }
        public int seq_no { get; set; }
        public string ImagePath{ get; set; }
        public string ImageDescription { get; set; }
        //public HttpPostedFileBase Image2 { get; set; }
        //public string ImageDescription2 { get; set; }
        //public HttpPostedFileBase Image3 { get; set; }
        //public string ImageDescription3 { get; set; }
        //public HttpPostedFileBase Image4 { get; set; }
        //public string ImageDescription4 { get; set; }
        //public HttpPostedFileBase Image5 { get; set; }
        //public string ImageDescription5 { get; set; }
        //public HttpPostedFileBase Image6 { get; set; }
        //public string ImageDescription6 { get; set; }
        //public HttpPostedFileBase Image7 { get; set; }
        //public string ImageDescription7 { get; set; }
        public override void SaveChildren()
        {
        }
        
    }
}

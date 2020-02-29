using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    
    public class RmaDates : ItemDBHandler
    {
        
        public RmaDates(int? id=0)
           : base(id)
        {

        }
        public RmaDates() {  }

        [DataType(DataType.Date)]
        public string ReceivedDate { get; set;}
        [DataType(DataType.Date)]
        public string ShippingDate { get; set; }
        [DataType(DataType.Date)]
        public string MFGDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date)]
        public string CaseDate { get; set; }
        [DataType(DataType.Date)]
        public string TestDate { get; set; }
        [DataType(DataType.Date)]
        public string ReworkDate { get; set; }
        [DataType(DataType.Date)]
        public string ParabitCalledDate { get; set; }
        public override void SaveChildren()
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ConfigProduct.DO
{
    public class DO_SpecialtyCodes
    {
        public int SpecialtyID { get; set; }
        public string SpecialtyDesc { get; set; }
        public string Gender { get; set; }
        public string SpecialtyType { get; set; } 
        public string SpecialtyGroup { get; set; }
        public string? MedicalIcon { get; set; }
        public string? FocusArea { get; set; }
        public int AgeRangeFrom { get; set; }
        public string RangePeriodFrom { get; set; } 
        public int AgeRangeTo { get; set; }
        public string? RangePeriodTo { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; }
        public int UserID { get; set; }
        public string TerminalID { get; set; }
    }
}

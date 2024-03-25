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
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; }
        public int UserID { get; set; }
        public string TerminalID { get; set; }
        public List<DO_AgeRangeMatrixSpecialtyLink>? lstAgerangeSpecilatyLink { get; set; }
    }
    public class DO_AgeRangeMatrixSpecialtyLink
    {
        public int SpecialtyId { get; set; }
        public int AgeRangeId { get; set; }
        public string? RangeDesc { get; set; } 
        public int AgeRangeFrom { get; set; }
        public string? RangeFromPeriod { get; set; }
        public int AgeRangeTo { get; set; }
        public string? RangeToPeriod { get; set; } 
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; }
        public int UserID { get; set; }
        public string TerminalID { get; set; }
    }
}

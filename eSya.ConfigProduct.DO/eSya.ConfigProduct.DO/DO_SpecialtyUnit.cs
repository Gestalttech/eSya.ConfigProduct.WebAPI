using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ConfigProduct.DO
{
    public class DO_SpecialtyUnit
    {
        public int BusinessKey { get; set; }
        public int SpecialtyID { get; set; }
        public string? SpecialtyDesc { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public int NoOfUnits { get; set; }
        public DateTime? EffectiveTill { get; set; }
        public int NewPatient { get; set; }
        public int RepeatPatient { get; set; }
        public int NoOfMaleBeds { get; set; }
        public int NoOfFemaleBeds { get; set; }
        public int NoOfCommonBeds { get; set; }
        public int MaxStayAllowed { get; set; }


        public bool ActiveStatus { get; set; }
        public string FormId { get; set; }
        public int UserID { get; set; }
        public string TerminalID { get; set; }
    }
}

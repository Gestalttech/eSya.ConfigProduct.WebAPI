using System;
using System.Collections.Generic;

namespace eSya.ConfigProduct.DL.Entities
{
    public partial class GtEsspmc
    {
        public int BusinessKey { get; set; }
        public int SpecialtyId { get; set; }
        public int NewPatient { get; set; }
        public int RepeatPatient { get; set; }
        public int NoOfMaleBeds { get; set; }
        public int NoOfFemaleBeds { get; set; }
        public int NoOfCommonBeds { get; set; }
        public int MaxStayAllowed { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; } = null!;
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; } = null!;
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedTerminal { get; set; }
    }
}

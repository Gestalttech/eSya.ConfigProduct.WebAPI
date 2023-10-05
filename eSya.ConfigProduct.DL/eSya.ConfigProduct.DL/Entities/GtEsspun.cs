using System;
using System.Collections.Generic;

namespace eSya.ConfigProduct.DL.Entities
{
    public partial class GtEsspun
    {
        public int BusinessKey { get; set; }
        public int SpecialtyId { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public int NoOfUnits { get; set; }
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

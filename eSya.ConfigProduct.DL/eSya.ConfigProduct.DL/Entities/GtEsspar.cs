using System;
using System.Collections.Generic;

namespace eSya.ConfigProduct.DL.Entities
{
    public partial class GtEsspar
    {
        public int SpecialtyId { get; set; }
        public int AgeRangeId { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; } = null!;
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; } = null!;
        public int? ModifiedBy { get; set; }
        public string? ModifiedOn { get; set; }
        public string? ModifiedTerminal { get; set; }
    }
}

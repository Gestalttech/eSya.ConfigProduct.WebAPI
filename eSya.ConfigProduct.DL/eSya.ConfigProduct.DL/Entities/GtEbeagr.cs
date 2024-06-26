﻿using System;
using System.Collections.Generic;

namespace eSya.ConfigProduct.DL.Entities
{
    public partial class GtEbeagr
    {
        public int AgeRangeId { get; set; }
        public string RangeDesc { get; set; } = null!;
        public int AgeRangeFrom { get; set; }
        public int RangeFromPeriod { get; set; }
        public int AgeRangeTo { get; set; }
        public int RangeToPeriod { get; set; }
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

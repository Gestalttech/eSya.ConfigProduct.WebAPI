using System;
using System.Collections.Generic;

namespace eSya.ConfigProduct.DL.Entities
{
    public partial class GtEssrcl
    {
        public GtEssrcl()
        {
            GtEspascs = new HashSet<GtEspasc>();
            GtEssrms = new HashSet<GtEssrm>();
        }

        public int ServiceClassId { get; set; }
        public int ServiceGroupId { get; set; }
        public string ServiceClassDesc { get; set; } = null!;
        public int ParentId { get; set; }
        public int PrintSequence { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; } = null!;
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; } = null!;
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedTerminal { get; set; }

        public virtual GtEssrgr ServiceGroup { get; set; } = null!;
        public virtual ICollection<GtEspasc> GtEspascs { get; set; }
        public virtual ICollection<GtEssrm> GtEssrms { get; set; }
    }
}

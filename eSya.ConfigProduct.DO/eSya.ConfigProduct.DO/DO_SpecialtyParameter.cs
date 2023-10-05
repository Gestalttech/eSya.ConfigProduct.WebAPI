using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ConfigProduct.DO
{
    public class DO_SpecialtyParameter
    {
        public int BusinessKey { get; set; }
        public int SpecialtyID { get; set; }
        public int ParameterID { get; set; }
        public decimal ParmPerc { get; set; }
        public bool ParmAction { get; set; }
        public string? ParmDesc { get; set; }
        public decimal ParmValue { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; }
        public int UserID { get; set; }
        public string TerminalID { get; set; }
    }

    public class DO_SpecialtyBusiness
    {
        public List<DO_SpecialtyParameter> SpecialtyParam { get; set; }
        public DO_SpecialtyBusinessLink SpecialtyBusiness { get; set; }
    }
}

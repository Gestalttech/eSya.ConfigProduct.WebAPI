using eSya.ConfigProduct.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ConfigProduct.IF
{
    public interface IClinicRepository
    {
        #region Consultant - ClinicLink
        Task<DO_ReturnParameter> InsertUpdateOPClinicLink(List<DO_OPClinic> obj);

        Task<List<DO_OPClinic>> GetClinicConsultantIdList(int businessKey);
        #endregion
    }
}

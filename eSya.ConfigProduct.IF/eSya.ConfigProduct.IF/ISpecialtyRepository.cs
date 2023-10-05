using eSya.ConfigProduct.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ConfigProduct.IF
{
    public interface ISpecialtyRepository
    {
        #region Specilalty Business Link
        Task<DO_ReturnParameter> InsertSpecialtyBusinessLink(DO_SpecialtyBusinessLink obj);
        Task<DO_ReturnParameter> InsertSpecialtyBusinessLinkList(DO_SpecialtyBusinessLink obj, List<DO_SpecialtyParameter> objPar);
        Task<DO_ReturnParameter> UpdateSpecialtyBusinessLink(DO_SpecialtyBusinessLink obj);
        Task<List<DO_SpecialtyBusinessLink>> GetSpecialtyBusinessList(int businessKey);
        Task<List<DO_SpecialtyBusinessLink>> GetSpecialtyListForBusinessKey(int businessKey);
        Task<List<DO_SpecialtyParameter>> GetSpecialtyParameterList(int businessKey, int specialtyId);
        #endregion
    }
}

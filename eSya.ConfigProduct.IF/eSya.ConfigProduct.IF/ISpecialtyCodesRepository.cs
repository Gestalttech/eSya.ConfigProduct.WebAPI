using eSya.ConfigProduct.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ConfigProduct.IF
{
    public interface ISpecialtyCodesRepository
    {
        Task<DO_ReturnParameter> InsertSpecialtyCodes(DO_SpecialtyCodes obj);
        Task<DO_ReturnParameter> UpdateSpecialtyCodes(DO_SpecialtyCodes obj);
        Task<DO_ReturnParameter> DeleteSpecialtyCodes(DO_SpecialtyCodes obj);
        Task<List<DO_SpecialtyCodes>> GetSpecialtyCodesList();
        Task<DO_SpecialtyCodes> GetSpecialtyCodes(int specialtyId);
    }
}

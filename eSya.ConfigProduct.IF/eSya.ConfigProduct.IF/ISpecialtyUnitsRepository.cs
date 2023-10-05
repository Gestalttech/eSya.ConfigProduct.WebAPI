using eSya.ConfigProduct.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ConfigProduct.IF
{
    public interface ISpecialtyUnitsRepository
    {
        Task<List<DO_SpecialtyUnit>> GetSpecialtyListByBusinessKey(int businessKey);
        Task<List<DO_SpecialtyUnit>> GetUnitsValidityBySpecialty(int businessKey, int specialtyId);
        Task<DO_ReturnParameter> InsertSpecialtyUnitsValidity(DO_SpecialtyUnit obj);
        Task<DO_SpecialtyUnit> GetSpecialtyIPInfo(int businessKey, int specialtyId);
        Task<DO_ReturnParameter> AddOrUpdateSpecialtyIPInfo(DO_SpecialtyUnit obj);
    }
}

using eSya.ConfigProduct.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ConfigProduct.IF
{
    public interface IAgeRangeRepository
    {
        #region AgeRange
        Task<List<DO_AgeRange>> GetAgeRanges();
        Task<DO_ReturnParameter> InsertOrUpdateAgeRange(DO_AgeRange obj);
        Task<DO_ReturnParameter> ActiveOrDeActiveAgeRange(bool status, int ageId);
        #endregion
    }
}

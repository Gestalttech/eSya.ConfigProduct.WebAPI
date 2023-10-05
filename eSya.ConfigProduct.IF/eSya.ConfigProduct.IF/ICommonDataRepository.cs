using eSya.ConfigProduct.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ConfigProduct.IF
{
    public interface ICommonDataRepository
    {
        #region Common Methods
        Task<List<DO_ApplicationCodes>> GetApplicationCodesByCodeType(int codeType);
        Task<List<DO_ApplicationCodes>> GetApplicationCodesByCodeTypeList(List<int> l_codeType);
        Task<List<DO_BusinessLocation>> GetBusinessKey();
        
        #endregion
    }
}

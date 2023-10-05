using eSya.ConfigProduct.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.ConfigProduct.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CommonMethodController : ControllerBase
    {
        private readonly ICommonDataRepository _commonDataRepository;

        #region Common Methods
        public CommonMethodController(ICommonDataRepository commonDataRepository)
        {
            _commonDataRepository = commonDataRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetBusinessKey()
        {
            var ac = await _commonDataRepository.GetBusinessKey();
            return Ok(ac);
        }
        [HttpGet]
        public async Task<IActionResult> GetApplicationCodesByCodeType(int codetype)
        {
            var ac = await _commonDataRepository.GetApplicationCodesByCodeType(codetype);
            return Ok(ac);
        }

        #endregion
    }
}

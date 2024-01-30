using eSya.ConfigProduct.DO;
using eSya.ConfigProduct.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.ConfigProduct.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AgeRangeController : ControllerBase
    {
        private readonly IAgeRangeRepository _ageRangeRepository;

        public AgeRangeController(IAgeRangeRepository ageRangeRepository)
        {
            _ageRangeRepository = ageRangeRepository;
        }
        #region Age Range
        /// <summary>
        /// Get Age Range
        /// UI Reffered - Age Range, 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAgeRanges()
        {
            var bcal = await _ageRangeRepository.GetAgeRanges();
            return Ok(bcal);
        }

        /// <summary>
        /// Insert into Age Range Table
        /// UI Reffered - Age Range,
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertOrUpdateAgeRange(DO_AgeRange obj)
        {
            var msg = await _ageRangeRepository.InsertOrUpdateAgeRange(obj);
            return Ok(msg);
        }

        /// <summary>
        /// Active Or De Active Age Range.
        /// UI Reffered - Age Range
        /// </summary>
        /// <param name="status-ageId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ActiveOrDeActiveAgeRange(bool status, int ageId)
        {
            var ac = await _ageRangeRepository.ActiveOrDeActiveAgeRange(status, ageId);
            return Ok(ac);
        }
        #endregion
    }
}

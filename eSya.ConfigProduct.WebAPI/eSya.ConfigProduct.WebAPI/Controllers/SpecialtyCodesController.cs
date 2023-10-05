using eSya.ConfigProduct.DO;
using eSya.ConfigProduct.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.ConfigProduct.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SpecialtyCodesController : ControllerBase
    {
        private readonly ISpecialtyCodesRepository _SpecialtyCodesRepository;

        #region Specialties
        public SpecialtyCodesController(ISpecialtyCodesRepository specialtyCodesRepository)
        {
            _SpecialtyCodesRepository = specialtyCodesRepository;
        }

        /// <summary>
        /// Insert into Specialty Codes Table
        /// UI Reffered - Specialty Codes,
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertSpecialtyCodes(DO_SpecialtyCodes obj)
        {
            var msg = await _SpecialtyCodesRepository.InsertSpecialtyCodes(obj);
            return Ok(msg);
        }

        /// <summary>
        /// Update into Specialty Codes Table
        /// UI Reffered - Specialty Codes,
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateSpecialtyCodes(DO_SpecialtyCodes obj)
        {
            var msg = await _SpecialtyCodesRepository.UpdateSpecialtyCodes(obj);
            return Ok(msg);
        }

        /// <summary>
        /// Delete into Specialty Codes Table
        /// UI Reffered - Specialty Codes,
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> DeleteSpecialtyCodes(DO_SpecialtyCodes obj)
        {
            var msg = await _SpecialtyCodesRepository.DeleteSpecialtyCodes(obj);
            return Ok(msg);
        }

        /// <summary>
        /// Get All Specialty Codes List
        /// UI Reffered - Specialty Codes,
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetSpecialtyCodesList()
        {
            var msg = await _SpecialtyCodesRepository.GetSpecialtyCodesList();
            return Ok(msg);
        }

        /// <summary>
        /// Get Specific Specialty Codes Data
        /// UI Reffered - Specialty Codes,
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetSpecialtyCodes(int specialtyId)
        {
            var msg = await _SpecialtyCodesRepository.GetSpecialtyCodes(specialtyId);
            return Ok(msg);
        }
        #endregion
    }
}

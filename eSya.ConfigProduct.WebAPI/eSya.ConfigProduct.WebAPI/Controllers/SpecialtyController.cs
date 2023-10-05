using eSya.ConfigProduct.DO;
using eSya.ConfigProduct.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.ConfigProduct.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SpecialtyController : ControllerBase
    {
        private readonly ISpecialtyRepository _SpecialtyRepository;

        #region Specialty Business Link
        public SpecialtyController(ISpecialtyRepository specialtyRepository)
        {
            _SpecialtyRepository = specialtyRepository;
        }

        /// <summary>
        /// Insert into Specialty Business Link Table
        /// UI Reffered - Specialty Business Link,
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertSpecialtyBusinessLink(DO_SpecialtyBusinessLink obj)
        {
            var msg = await _SpecialtyRepository.InsertSpecialtyBusinessLink(obj);
            return Ok(msg);
        }

        /// <summary>
        /// Insert List into Specialty Business Link Table
        /// UI Reffered - Specialty Business Link,
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertSpecialtyBusinessLinkList(DO_SpecialtyBusiness objBus)
        {
            DO_SpecialtyBusinessLink obj = objBus.SpecialtyBusiness;
            List<DO_SpecialtyParameter> objPar = objBus.SpecialtyParam;

            var msg = await _SpecialtyRepository.InsertSpecialtyBusinessLinkList(obj, objPar);
            return Ok(msg);
        }

        /// <summary>
        /// Update into Specialty Business Link Table
        /// UI Reffered - Specialty Business Link,
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateSpecialtyClinicLink(DO_SpecialtyBusinessLink obj)
        {
            var msg = await _SpecialtyRepository.UpdateSpecialtyBusinessLink(obj);
            return Ok(msg);
        }

        /// <summary>
        /// Get All Specialty List
        /// UI Reffered - Specialty Business Link,
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetSpecialtyBusinessList(int businessKey)
        {
            var msg = await _SpecialtyRepository.GetSpecialtyBusinessList(businessKey);
            return Ok(msg);
        }

        /// <summary>
        /// Get Distinct Specialty List For Business Key
        /// UI Reffered - Doctor Specialty Link,Clinic Doctor Link,Doctor Scheduler
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetSpecialtyListForBusinessKey(int businessKey)
        {
            var msg = await _SpecialtyRepository.GetSpecialtyListForBusinessKey(businessKey);
            return Ok(msg);
        }


        /// <summary>
        /// Get All Specialty Parameter List
        /// UI Reffered - Specialty Business Link,
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetSpecialtyParameterList(int businessKey, int specialtyId)
        {
            var msg = await _SpecialtyRepository.GetSpecialtyParameterList(businessKey, specialtyId);
            return Ok(msg);
        }
        #endregion
    }
}

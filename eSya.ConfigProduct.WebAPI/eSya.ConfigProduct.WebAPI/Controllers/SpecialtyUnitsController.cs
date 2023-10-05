using eSya.ConfigProduct.DO;
using eSya.ConfigProduct.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.ConfigProduct.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SpecialtyUnitsController : ControllerBase
    {
        private readonly ISpecialtyUnitsRepository _SpecialtyUnitsRepository;

        #region Specialty Units
        public SpecialtyUnitsController(ISpecialtyUnitsRepository specialtyUnitsRepository)
        {
            _SpecialtyUnitsRepository = specialtyUnitsRepository;
        }



        [HttpGet]
        public async Task<IActionResult> GetSpecialtyListByBusinessKey(int businessKey)
        {
            var msg = await _SpecialtyUnitsRepository.GetSpecialtyListByBusinessKey(businessKey);
            return Ok(msg);
        }


        [HttpGet]
        public async Task<IActionResult> GetUnitsValidityBySpecialty(int businessKey, int specialtyId)
        {
            var msg = await _SpecialtyUnitsRepository.GetUnitsValidityBySpecialty(businessKey, specialtyId);
            return Ok(msg);
        }
        [HttpPost]
        public async Task<IActionResult> InsertSpecialtyUnitsValidity(DO_SpecialtyUnit obj)
        {
            var msg = await _SpecialtyUnitsRepository.InsertSpecialtyUnitsValidity(obj);
            return Ok(msg);
        }

        [HttpGet]
        public async Task<IActionResult> GetSpecialtyIPInfo(int businessKey, int specialtyId)
        {
            var msg = await _SpecialtyUnitsRepository.GetSpecialtyIPInfo(businessKey, specialtyId);
            return Ok(msg);
        }
        [HttpPost]
        public async Task<IActionResult> AddOrUpdateSpecialtyIPInfo(DO_SpecialtyUnit obj)
        {
            var msg = await _SpecialtyUnitsRepository.AddOrUpdateSpecialtyIPInfo(obj);
            return Ok(msg);
        }
        #endregion
    }
}

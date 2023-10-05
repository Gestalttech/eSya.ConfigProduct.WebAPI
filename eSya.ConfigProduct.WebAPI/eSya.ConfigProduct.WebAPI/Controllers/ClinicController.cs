using eSya.ConfigProduct.DO;
using eSya.ConfigProduct.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.ConfigProduct.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ClinicController : ControllerBase
    {
        private readonly IClinicRepository _ClinicRepository;

        public ClinicController(IClinicRepository clinicRepository)
        {
            _ClinicRepository = clinicRepository;
        }

        #region Consultation - Clinic Link

        /// <summary>
        /// Insert / Update into OP Clinic Table
        /// UI Reffered - Clinic & Consultation,
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertUpdateOPClinicLink(List<DO_OPClinic> obj)
        {
            var msg = await _ClinicRepository.InsertUpdateOPClinicLink(obj);
            return Ok(msg);
        }

        /// <summary>
        /// Get All Clinic Consultant
        /// UI Reffered - Clinic & Consultation,Doctor Scheduler
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetClinicConsultantIdList(int businessKey)
        {
            var msg = await _ClinicRepository.GetClinicConsultantIdList(businessKey);
            return Ok(msg);
        }
        #endregion
    }
}

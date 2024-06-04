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

        #region Map Business Linked Specialaties to Clinic & Consultation Type

        /// <summary>
        /// Insert / Update into Speciality  Clinic & Consultation Type Table
        /// UI Reffered - Map Business Linked Specialaties to Clinic & Consultation Type
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertUpdateSpecialtyClinicConsultationTypeLink(List<DO_MapSpecialtyClinicConsultationType> obj)
        {
            var msg = await _ClinicRepository.InsertUpdateSpecialtyClinicConsultationTypeLink(obj);
            return Ok(msg);
        }

        /// <summary>
        /// Get All Mapped Specialty with Business Key for drop down
        /// UI Reffered - CMap Business Linked Specialaties to Clinic & Consultation Type
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetMappedSpecialtyListbyBusinessKey(int businessKey)
        {
            var msg = await _ClinicRepository.GetMappedSpecialtyListbyBusinessKey(businessKey);
            return Ok(msg);
        }
        /// <summary>
        /// Get All Clinic Consultant mapped with specialty and Business Key
        /// UI Reffered - Map Business Linked Specialaties to Clinic & Consultation Type
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetMapedSpecialtyClinicConsultationTypeBySpecialtyID(int businessKey, int specialtyId)
        {
            var msg = await _ClinicRepository.GetMapedSpecialtyClinicConsultationTypeBySpecialtyID(businessKey, specialtyId);
            return Ok(msg);
        }
        #endregion
    }
}

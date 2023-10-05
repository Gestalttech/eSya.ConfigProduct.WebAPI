using eSya.ConfigProduct.DO;
using eSya.ConfigProduct.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.ConfigProduct.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ServiceManagementController : ControllerBase
    {
        private readonly IServiceManagementRepository _ServiceManagementRepository;
        public ServiceManagementController(IServiceManagementRepository serviceManagementRepository)
        {
            _ServiceManagementRepository = serviceManagementRepository;
        }

        #region ServiceType
        [HttpGet]
        public async Task<IActionResult> GetServiceTypes()
        {
            var ac = await _ServiceManagementRepository.GetServiceTypes();
            return Ok(ac);
        }
        [HttpGet]
        public async Task<IActionResult> GetServiceTypeByID(int ServiceTypeID)
        {
            var ac = await _ServiceManagementRepository.GetServiceTypeByID(ServiceTypeID);
            return Ok(ac);
        }
        [HttpPost]
        public async Task<IActionResult> AddOrUpdateServiceType(DO_ServiceType obj)
        {
            var msg = await _ServiceManagementRepository.AddOrUpdateServiceType(obj);
            return Ok(msg);
        }
        [HttpGet]
        public async Task<IActionResult> UpdateServiceTypeIndex(int serviceTypeId, bool isMoveUp, bool isMoveDown)
        {
            var msg = await _ServiceManagementRepository.UpdateServiceTypeIndex(serviceTypeId, isMoveUp, isMoveDown);
            return Ok(msg);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteServiceType(int serviceTypeId)
        {
            var msg = await _ServiceManagementRepository.DeleteServiceType(serviceTypeId);
            return Ok(msg);
        }
        #endregion

        #region ServiceGroup
        [HttpGet]
        public async Task<IActionResult> GetServiceGroups()
        {
            var ac = await _ServiceManagementRepository.GetServiceGroups();
            return Ok(ac);
        }
        [HttpGet]
        public async Task<IActionResult> GetServiceGroupsByTypeID(int servicetype)
        {
            var ac = await _ServiceManagementRepository.GetServiceGroupsByTypeID(servicetype);
            return Ok(ac);
        }
        [HttpGet]
        public async Task<IActionResult> GetServiceGroupByID(int ServiceGroupID)
        {
            var ac = await _ServiceManagementRepository.GetServiceGroupByID(ServiceGroupID);
            return Ok(ac);
        }
        [HttpPost]
        public async Task<IActionResult> AddOrUpdateServiceGroup(DO_ServiceGroup obj)
        {
            var msg = await _ServiceManagementRepository.AddOrUpdateServiceGroup(obj);
            return Ok(msg);
        }
        [HttpGet]
        public async Task<IActionResult> UpdateServiceGroupIndex(int serviceTypeId, int serviceGroupId, bool isMoveUp, bool isMoveDown)
        {
            var msg = await _ServiceManagementRepository.UpdateServiceGroupIndex(serviceTypeId, serviceGroupId, isMoveUp, isMoveDown);
            return Ok(msg);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteServiceGroup(int serviceGroupId)
        {
            var msg = await _ServiceManagementRepository.DeleteServiceGroup(serviceGroupId);
            return Ok(msg);
        }
        #endregion

        #region ServiceClass
        [HttpGet]
        public async Task<IActionResult> GetServiceClasses()
        {
            var ac = await _ServiceManagementRepository.GetServiceClasses();
            return Ok(ac);
        }
        [HttpGet]
        public async Task<IActionResult> GetServiceClassesByGroupID(int servicegroup)
        {
            var ac = await _ServiceManagementRepository.GetServiceClassesByGroupID(servicegroup);
            return Ok(ac);
        }
        [HttpGet]
        public async Task<IActionResult> GetServiceClassByID(int ServiceClassID)
        {
            var ac = await _ServiceManagementRepository.GetServiceClassByID(ServiceClassID);
            return Ok(ac);
        }
        [HttpPost]
        public async Task<IActionResult> AddOrUpdateServiceClass(DO_ServiceClass obj)
        {
            var msg = await _ServiceManagementRepository.AddOrUpdateServiceClass(obj);
            return Ok(msg);
        }
        [HttpGet]
        public async Task<IActionResult> UpdateServiceClassIndex(int serviceGroupId, int serviceClassId, bool isMoveUp, bool isMoveDown)
        {
            var msg = await _ServiceManagementRepository.UpdateServiceClassIndex(serviceGroupId, serviceClassId, isMoveUp, isMoveDown);
            return Ok(msg);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteServiceClass(int serviceClassId)
        {
            var msg = await _ServiceManagementRepository.DeleteServiceClass(serviceClassId);
            return Ok(msg);
        }
        #endregion
    }
}

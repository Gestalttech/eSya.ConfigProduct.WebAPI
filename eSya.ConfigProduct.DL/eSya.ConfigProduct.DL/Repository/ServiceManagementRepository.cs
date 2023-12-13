﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eSya.ConfigProduct.DL.Entities;
using eSya.ConfigProduct.DO;
using eSya.ConfigProduct.IF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace eSya.ConfigProduct.DL.Repository
{
    public class ServiceManagementRepository : IServiceManagementRepository
    {
        private readonly IStringLocalizer<ServiceManagementRepository> _localizer;
        public ServiceManagementRepository(IStringLocalizer<ServiceManagementRepository> localizer)
        {
            _localizer = localizer;
        }
        #region ServiceTypes
        public async Task<List<DO_ServiceType>> GetServiceTypes()
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var result = db.GtEssrties
                                 .Select(x => new DO_ServiceType
                                 {
                                     ServiceTypeId = x.ServiceTypeId,
                                     ServiceTypeDesc = x.ServiceTypeDesc,
                                     PrintSequence = x.PrintSequence,
                                     ActiveStatus = x.ActiveStatus
                                 }
                        ).OrderBy(o => o.PrintSequence).ToListAsync();
                    return await result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DO_ServiceType> GetServiceTypeByID(int ServiceTypeID)
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var result = db.GtEssrties
                        .Where(i => i.ServiceTypeId == ServiceTypeID)
                                 .Select(x => new DO_ServiceType
                                 {
                                     ServiceTypeId = x.ServiceTypeId,
                                     ServiceTypeDesc = x.ServiceTypeDesc,
                                     ActiveStatus = x.ActiveStatus
                                 }
                        ).FirstOrDefaultAsync();
                    return await result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DO_ReturnParameter> AddOrUpdateServiceType(DO_ServiceType obj)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        if (obj.ServiceTypeId == 0)
                        {
                            var RecordExist = db.GtEssrties.Where(w => w.ServiceTypeDesc == obj.ServiceTypeDesc).Count();
                            if (RecordExist > 0)
                            {
                                return new DO_ReturnParameter() { Status = false, StatusCode = "W0077", Message = string.Format(_localizer[name: "W0077"]) };
                            }
                            else
                            {

                                var newServiceTypeId = db.GtEssrties.Select(a => (int)a.ServiceTypeId).DefaultIfEmpty().Max() + 1;

                                var servicetype = new GtEssrty
                                {
                                    ServiceTypeId = newServiceTypeId,
                                    ServiceTypeDesc = obj.ServiceTypeDesc,
                                    PrintSequence = newServiceTypeId,
                                    ActiveStatus = obj.ActiveStatus,
                                    FormId = obj.FormID,
                                    CreatedBy = obj.UserID,
                                    CreatedOn = obj.CreatedOn,
                                    CreatedTerminal = obj.TerminalID
                                };
                                db.GtEssrties.Add(servicetype);

                            }
                        }
                        else
                        {
                            if (!obj.ActiveStatus)
                            {
                                var LinkExist = db.GtEssrgrs.Where(w => w.ServiceTypeId == obj.ServiceTypeId && w.ActiveStatus).Count();
                                if (LinkExist > 0)
                                {
                                    return new DO_ReturnParameter() { Status = false, StatusCode = "W0078", Message = string.Format(_localizer[name: "W0078"]) };
                                }
                            }
                            var updatedServiceType = db.GtEssrties.Where(w => w.ServiceTypeId == obj.ServiceTypeId).FirstOrDefault();
                            if (updatedServiceType.ServiceTypeDesc != obj.ServiceTypeDesc)
                            {
                                var RecordExist = db.GtEssrties.Where(w => w.ServiceTypeDesc == obj.ServiceTypeDesc).Count();
                                if (RecordExist > 0)
                                {
                                    return new DO_ReturnParameter() { Status = false, StatusCode = "W0079", Message = string.Format(_localizer[name: "W0079"]) };

                                }

                            }

                            updatedServiceType.ServiceTypeDesc = obj.ServiceTypeDesc;
                            updatedServiceType.ActiveStatus = obj.ActiveStatus;
                            updatedServiceType.ModifiedBy = obj.UserID;
                            updatedServiceType.ModifiedOn = obj.CreatedOn;
                            updatedServiceType.ModifiedTerminal = obj.TerminalID;



                        }

                        await db.SaveChangesAsync();
                        dbContext.Commit();
                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };

                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }
        public async Task<DO_ReturnParameter> UpdateServiceTypeIndex(int serviceTypeId, bool isMoveUp, bool isMoveDown)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var stCurrent = db.GtEssrties.Where(w => w.ServiceTypeId == serviceTypeId).FirstOrDefault();
                        int switchIndex = 0;

                        if (isMoveUp)
                        {
                            var isTop = db.GtEssrties.Where(w => w.PrintSequence < stCurrent.PrintSequence).Count();
                            if (isTop > 0)
                            {
                                var stTarget = db.GtEssrties.Where(w => w.PrintSequence < stCurrent.PrintSequence).OrderByDescending(o => o.PrintSequence).FirstOrDefault();
                                switchIndex = stCurrent.PrintSequence;
                                stCurrent.PrintSequence = stTarget.PrintSequence;
                                stTarget.PrintSequence = switchIndex;
                            }
                            else
                            {
                                return new DO_ReturnParameter() { Status = false,StatusCode= "W0080", Message = stCurrent.ServiceTypeDesc + string.Format(_localizer[name: "W0080"]) };
                            }
                        }
                        else if (isMoveDown)
                        {
                            var isBottom = db.GtEssrties.Where(w => w.PrintSequence > stCurrent.PrintSequence).Count();
                            if (isBottom > 0)
                            {
                                var stTarget = db.GtEssrties.Where(w => w.PrintSequence > stCurrent.PrintSequence).OrderBy(o => o.PrintSequence).FirstOrDefault();
                                switchIndex = stCurrent.PrintSequence;
                                stCurrent.PrintSequence = stTarget.PrintSequence;
                                stTarget.PrintSequence = switchIndex;
                            }
                            else
                            {
                                return new DO_ReturnParameter() { Status = false,StatusCode= "W0081", Message = stCurrent.ServiceTypeDesc + string.Format(_localizer[name: "W0081"]) };
                            }
                        }




                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }
        public async Task<DO_ReturnParameter> DeleteServiceType(int serviceTypeId)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var LinkExist = db.GtEssrgrs.Where(w => w.ServiceTypeId == serviceTypeId).Count();
                        if (LinkExist > 0)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0082", Message = string.Format(_localizer[name: "W0082"]) };
                        }

                        var ServiceType = db.GtEssrties.Where(w => w.ServiceTypeId == serviceTypeId).FirstOrDefault();
                        if (ServiceType != null)
                        {
                            db.GtEssrties.Remove(ServiceType);
                        }

                        await db.SaveChangesAsync();
                        dbContext.Commit();
                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0005", Message = string.Format(_localizer[name: "S0005"]) };
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }
        #endregion

        #region ServiceGroups
        public async Task<List<DO_ServiceGroup>> GetServiceGroups()
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var result = db.GtEssrgrs
                                 .Select(x => new DO_ServiceGroup
                                 {
                                     ServiceTypeId = x.ServiceTypeId,
                                     ServiceGroupId = x.ServiceGroupId,
                                     ServiceGroupDesc = x.ServiceGroupDesc,
                                     ServiceCriteria = x.ServiceCriteria,
                                     PrintSequence = x.PrintSequence,
                                     ActiveStatus = x.ActiveStatus
                                 }
                        ).OrderBy(g => g.PrintSequence).ToListAsync();
                    return await result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<DO_ServiceGroup>> GetServiceGroupsByTypeID(int servicetype)
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var result = db.GtEssrgrs
                        .Where(w => w.ServiceTypeId == servicetype && w.ActiveStatus)
                                 .Select(x => new DO_ServiceGroup
                                 {
                                     ServiceGroupId = x.ServiceGroupId,
                                     ServiceGroupDesc = x.ServiceGroupDesc,
                                     ServiceCriteria = x.ServiceCriteria,
                                     PrintSequence = x.PrintSequence,
                                 }
                        ).OrderBy(g => g.PrintSequence).ToListAsync();
                    return await result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DO_ServiceGroup> GetServiceGroupByID(int ServiceGroupID)
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var result = db.GtEssrgrs
                        .Where(i => i.ServiceGroupId == ServiceGroupID)
                                 .Select(x => new DO_ServiceGroup
                                 {
                                     ServiceGroupId = x.ServiceGroupId,
                                     ServiceGroupDesc = x.ServiceGroupDesc,
                                     ServiceCriteria = x.ServiceCriteria,
                                     ActiveStatus = x.ActiveStatus
                                 }
                        ).FirstOrDefaultAsync();
                    return await result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DO_ReturnParameter> AddOrUpdateServiceGroup(DO_ServiceGroup obj)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        if (obj.ServiceGroupId == 0)
                        {
                            var RecordExist = db.GtEssrgrs.Where(w => w.ServiceGroupDesc == obj.ServiceGroupDesc).Count();
                            if (RecordExist > 0)
                            {
                                return new DO_ReturnParameter() { Status = false, StatusCode = "W0083", Message = string.Format(_localizer[name: "W0083"]) };
                            }
                            else
                            {


                                var newServiceGroupId = db.GtEssrgrs.Select(a => (int)a.ServiceGroupId).DefaultIfEmpty().Max() + 1;
                                var newPrintSequence = db.GtEssrgrs.Where(w => w.ServiceTypeId == obj.ServiceTypeId).Select(a => (int)a.PrintSequence).DefaultIfEmpty().Max() + 1;

                                var servicegroup = new GtEssrgr
                                {
                                    ServiceTypeId = obj.ServiceTypeId,
                                    ServiceGroupId = newServiceGroupId,
                                    ServiceGroupDesc = obj.ServiceGroupDesc,
                                    ServiceCriteria = obj.ServiceCriteria,
                                    PrintSequence = newPrintSequence,
                                    ActiveStatus = obj.ActiveStatus,
                                    FormId = obj.FormId,
                                    CreatedBy = obj.UserID,
                                    CreatedOn = obj.CreatedOn,
                                    CreatedTerminal = obj.TerminalID
                                };
                                db.GtEssrgrs.Add(servicegroup);

                            }
                        }
                        else
                        {
                            if (!obj.ActiveStatus)
                            {
                                var LinkExist = db.GtEssrcls.Where(w => w.ServiceGroupId == obj.ServiceGroupId && w.ActiveStatus).Count();
                                if (LinkExist > 0)
                                {
                                    return new DO_ReturnParameter() { Status = false, StatusCode = "W0084", Message = string.Format(_localizer[name: "W0084"]) };
                                }
                            }
                            var updatedServiceGroup = db.GtEssrgrs.Where(w => w.ServiceGroupId == obj.ServiceGroupId).FirstOrDefault();
                            if (updatedServiceGroup.ServiceGroupDesc != obj.ServiceGroupDesc)
                            {
                                var RecordExist = db.GtEssrgrs.Where(w => w.ServiceGroupDesc == obj.ServiceGroupDesc).Count();
                                if (RecordExist > 0)
                                {
                                    return new DO_ReturnParameter() { Status = false, StatusCode = "W0083", Message = string.Format(_localizer[name: "W0083"]) };
                                }
                            }
                            updatedServiceGroup.ServiceGroupDesc = obj.ServiceGroupDesc;
                            updatedServiceGroup.ServiceCriteria = obj.ServiceCriteria;
                            updatedServiceGroup.ActiveStatus = obj.ActiveStatus;
                            updatedServiceGroup.ModifiedBy = obj.UserID;
                            updatedServiceGroup.ModifiedOn = obj.CreatedOn;
                            updatedServiceGroup.ModifiedTerminal = obj.TerminalID;

                        }

                        await db.SaveChangesAsync();
                        dbContext.Commit();
                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };

                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }
        public async Task<DO_ReturnParameter> UpdateServiceGroupIndex(int serviceTypeId, int serviceGroupId, bool isMoveUp, bool isMoveDown)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var sgCurrent = db.GtEssrgrs.Where(w => w.ServiceTypeId == serviceTypeId && w.ServiceGroupId == serviceGroupId).FirstOrDefault();
                        int switchIndex = 0;
                        if (isMoveUp)
                        {
                            var isTop = db.GtEssrgrs.Where(w => w.PrintSequence < sgCurrent.PrintSequence && w.ServiceTypeId == serviceTypeId).Count();
                            if (isTop > 0)
                            {
                                var sgTarget = db.GtEssrgrs.Where(w => w.PrintSequence < sgCurrent.PrintSequence && w.ServiceTypeId == serviceTypeId).OrderByDescending(o => o.PrintSequence).FirstOrDefault();
                                switchIndex = sgCurrent.PrintSequence;
                                sgCurrent.PrintSequence = sgTarget.PrintSequence;
                                sgTarget.PrintSequence = switchIndex;
                            }
                            else
                            {
                                return new DO_ReturnParameter() { Status = false,StatusCode= "W0080", Message = sgCurrent.ServiceGroupDesc + string.Format(_localizer[name: "W0080"]) };
                            }
                        }
                        else if (isMoveDown)
                        {
                            var isBottom = db.GtEssrgrs.Where(w => w.PrintSequence > sgCurrent.PrintSequence && w.ServiceTypeId == serviceTypeId).Count();
                            if (isBottom > 0)
                            {
                                var sgTarget = db.GtEssrgrs.Where(w => w.PrintSequence > sgCurrent.PrintSequence && w.ServiceTypeId == serviceTypeId).OrderBy(o => o.PrintSequence).FirstOrDefault();
                                switchIndex = sgCurrent.PrintSequence;
                                sgCurrent.PrintSequence = sgTarget.PrintSequence;
                                sgTarget.PrintSequence = switchIndex;
                            }
                            else
                            {
                                return new DO_ReturnParameter() { Status = false, StatusCode = "W0081", Message = sgCurrent.ServiceGroupDesc + string.Format(_localizer[name: "W0081"]) };
                            }
                        }


                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }
        public async Task<DO_ReturnParameter> DeleteServiceGroup(int serviceGroupId)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {

                        var LinkExist = db.GtEssrcls.Where(w => w.ServiceGroupId == serviceGroupId).Count();
                        if (LinkExist > 0)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0085", Message = string.Format(_localizer[name: "W0085"]) };
                        }

                        var ServiceGroup = db.GtEssrgrs.Where(w => w.ServiceGroupId == serviceGroupId).FirstOrDefault();
                        if (ServiceGroup != null)
                        {
                            db.GtEssrgrs.Remove(ServiceGroup);
                        }

                        await db.SaveChangesAsync();
                        dbContext.Commit();
                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0005", Message = string.Format(_localizer[name: "S0005"]) };

                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }
        #endregion

        #region ServiceClass
        public async Task<List<DO_ServiceClass>> GetServiceClasses()
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var result = db.GtEssrcls
                                 .Select(x => new DO_ServiceClass
                                 {
                                     ServiceGroupId = x.ServiceGroupId,
                                     ServiceClassId = x.ServiceClassId,
                                     ServiceClassDesc = x.ServiceClassDesc,
                                     //IsBaseRateApplicable = x.IsBaseRateApplicable,
                                     ParentId = x.ParentId,
                                     PrintSequence = x.PrintSequence,
                                     ActiveStatus = x.ActiveStatus
                                 }
                        ).OrderBy(g => g.PrintSequence).ToListAsync();
                    return await result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<DO_ServiceClass>> GetServiceClassesByGroupID(int servicegroup)
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var result = db.GtEssrcls
                        .Where(w => w.ServiceGroupId == servicegroup && w.ActiveStatus)
                                 .Select(x => new DO_ServiceClass
                                 {
                                     ServiceClassId = x.ServiceClassId,
                                     ServiceClassDesc = x.ServiceClassDesc,
                                     PrintSequence = x.PrintSequence,
                                 }
                        ).OrderBy(g => g.PrintSequence).ToListAsync();
                    return await result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DO_ServiceClass> GetServiceClassByID(int ServiceClassID)
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var result = db.GtEssrcls
                        .Where(i => i.ServiceClassId == ServiceClassID)
                                 .Select(x => new DO_ServiceClass
                                 {
                                     ServiceClassId = x.ServiceClassId,
                                     ServiceClassDesc = x.ServiceClassDesc,
                                     //IsBaseRateApplicable = x.IsBaseRateApplicable,
                                     ActiveStatus = x.ActiveStatus,
                                     l_ClassParameter = x.GtEspascs.Select(p => new DO_eSyaParameter
                                     {
                                         ParameterID = p.ParameterId,
                                         ParmAction = p.ParmAction,
                                         ParmPerc = p.ParmPerc,
                                         ParmDesc = p.ParmDesc,
                                         ParmValue = p.ParmValue,
                                     }).ToList()
                                 }
                        ).FirstOrDefaultAsync();

                    return await result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DO_ReturnParameter> AddOrUpdateServiceClass(DO_ServiceClass obj)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        if (obj.ServiceClassId == 0)
                        {
                            var RecordExist = db.GtEssrcls.Where(w => w.ServiceClassDesc == obj.ServiceClassDesc).Count();
                            if (RecordExist > 0)
                            {
                                return new DO_ReturnParameter() { Status = false, StatusCode = "W0086", Message = string.Format(_localizer[name: "W0086"]) };
                            }
                            else
                            {


                                var newServiceClassId = db.GtEssrcls.Select(a => (int)a.ServiceClassId).DefaultIfEmpty().Max() + 1;
                                var parentId = obj.ParentId;
                                if (parentId == 0)
                                {
                                    parentId = newServiceClassId;
                                }
                                var newPrintSequence = db.GtEssrcls.Where(w => w.ServiceGroupId == obj.ServiceGroupId).Select(a => (int)a.PrintSequence).DefaultIfEmpty().Max() + 1;

                                var serviceclass = new GtEssrcl
                                {
                                    ServiceGroupId = obj.ServiceGroupId,
                                    ServiceClassId = newServiceClassId,
                                    ServiceClassDesc = obj.ServiceClassDesc,
                                    //IsBaseRateApplicable = obj.IsBaseRateApplicable,
                                    ParentId = parentId,
                                    PrintSequence = newPrintSequence,
                                    ActiveStatus = obj.ActiveStatus,
                                    FormId = obj.FormId,
                                    CreatedBy = obj.UserID,
                                    CreatedOn = obj.CreatedOn,
                                    CreatedTerminal = obj.TerminalID
                                };
                                db.GtEssrcls.Add(serviceclass);
                                foreach (DO_eSyaParameter cp in obj.l_ClassParameter)
                                {
                                    var cParameter = new GtEspasc
                                    {
                                        ServiceClassId = newServiceClassId,
                                        ParameterId = cp.ParameterID,
                                        ParmPerc = cp.ParmPerc,
                                        ParmAction = cp.ParmAction,
                                        ParmDesc = cp.ParmDesc,
                                        ParmValue = cp.ParmValue,
                                        ActiveStatus = cp.ActiveStatus,
                                        FormId = obj.FormId,
                                        CreatedBy = obj.UserID,
                                        CreatedOn = System.DateTime.Now,
                                        CreatedTerminal = obj.TerminalID,
                                    };
                                    db.GtEspascs.Add(cParameter);

                                }
                            }
                        }
                        else
                        {
                            if (!obj.ActiveStatus)
                            {
                                var cLinkExist = db.GtEssrcls.Where(w => w.ParentId == obj.ServiceClassId && w.ParentId != w.ServiceClassId && w.ActiveStatus).Count();
                                if (cLinkExist > 0)
                                {
                                    return new DO_ReturnParameter() { Status = false, StatusCode = "W0087", Message = string.Format(_localizer[name: "W0087"]) };
                                }
                                var sLinkExist = db.GtEssrms.Where(w => w.ServiceClassId == obj.ServiceClassId && w.ActiveStatus).Count();
                                if (sLinkExist > 0)
                                {
                                    return new DO_ReturnParameter() { Status = false, StatusCode = "W0088", Message = string.Format(_localizer[name: "W0088"]) };
                                }
                            }
                            var updatedServiceClass = db.GtEssrcls.Where(w => w.ServiceClassId == obj.ServiceClassId).FirstOrDefault();
                            if (updatedServiceClass.ServiceClassDesc != obj.ServiceClassDesc)
                            {
                                var RecordExist = db.GtEssrcls.Where(w => w.ServiceClassDesc == obj.ServiceClassDesc).Count();
                                if (RecordExist > 0)
                                {
                                    return new DO_ReturnParameter() { Status = false, StatusCode = "W0086", Message = string.Format(_localizer[name: "W0086"]) };
                                }
                            }
                            updatedServiceClass.ServiceClassDesc = obj.ServiceClassDesc;
                            //updatedServiceClass.IsBaseRateApplicable = obj.IsBaseRateApplicable;
                            updatedServiceClass.ActiveStatus = obj.ActiveStatus;
                            updatedServiceClass.ModifiedBy = obj.UserID;
                            updatedServiceClass.ModifiedOn = obj.CreatedOn;
                            updatedServiceClass.ModifiedTerminal = obj.TerminalID;

                            foreach (DO_eSyaParameter cp in obj.l_ClassParameter)
                            {
                                var cPar = db.GtEspascs.Where(x => x.ServiceClassId == obj.ServiceClassId && x.ParameterId == cp.ParameterID).FirstOrDefault();
                                if (cPar != null)
                                {
                                    cPar.ParmAction = cp.ParmAction;
                                    cPar.ParmDesc = cp.ParmDesc;
                                    cPar.ParmPerc = cp.ParmPerc;
                                    cPar.ParmValue = cp.ParmValue;
                                    cPar.ActiveStatus = obj.ActiveStatus;
                                    cPar.ModifiedBy = obj.UserID;
                                    cPar.ModifiedOn = System.DateTime.Now;
                                    cPar.ModifiedTerminal = obj.TerminalID;
                                }
                                else
                                {
                                    var cParameter = new GtEspasc
                                    {
                                        ServiceClassId = obj.ServiceClassId,
                                        ParameterId = cp.ParameterID,
                                        ParmPerc = cp.ParmPerc,
                                        ParmAction = cp.ParmAction,
                                        ParmDesc = cp.ParmDesc,
                                        ParmValue = cp.ParmValue,
                                        ActiveStatus = cp.ActiveStatus,
                                        FormId = obj.FormId,
                                        CreatedBy = obj.UserID,
                                        CreatedOn = System.DateTime.Now,
                                        CreatedTerminal = obj.TerminalID,

                                    };
                                    db.GtEspascs.Add(cParameter);
                                }

                            }
                        }

                        await db.SaveChangesAsync();
                        dbContext.Commit();
                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };

                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }
        public async Task<DO_ReturnParameter> UpdateServiceClassIndex(int serviceGroupId, int serviceClassId, bool isMoveUp, bool isMoveDown)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var scCurrent = db.GtEssrcls.Where(w => w.ServiceGroupId == serviceGroupId && w.ServiceClassId == serviceClassId).FirstOrDefault();
                        int switchIndex = 0;
                        if (scCurrent.ParentId == scCurrent.ServiceClassId)
                        {
                            if (isMoveUp)
                            {
                                var isTop = db.GtEssrcls.Where(w => w.PrintSequence < scCurrent.PrintSequence && w.ServiceGroupId == serviceGroupId && w.ServiceClassId == w.ParentId).Count();
                                if (isTop > 0)
                                {
                                    var scTarget = db.GtEssrcls.Where(w => w.PrintSequence < scCurrent.PrintSequence && w.ServiceGroupId == serviceGroupId && w.ServiceClassId == w.ParentId).OrderByDescending(o => o.PrintSequence).FirstOrDefault();
                                    switchIndex = scCurrent.PrintSequence;
                                    scCurrent.PrintSequence = scTarget.PrintSequence;
                                    scTarget.PrintSequence = switchIndex;
                                }
                                else
                                {
                                    return new DO_ReturnParameter() { Status = false,StatusCode= "W0080", Message = scCurrent.ServiceClassDesc + string.Format(_localizer[name: "W0080"]) };
                                }
                            }
                            else if (isMoveDown)
                            {
                                var isBottom = db.GtEssrcls.Where(w => w.PrintSequence > scCurrent.PrintSequence && w.ServiceGroupId == serviceGroupId && w.ServiceClassId == w.ParentId).Count();
                                if (isBottom > 0)
                                {
                                    var scTarget = db.GtEssrcls.Where(w => w.PrintSequence > scCurrent.PrintSequence && w.ServiceGroupId == serviceGroupId && w.ServiceClassId == w.ParentId).OrderBy(o => o.PrintSequence).FirstOrDefault();
                                    switchIndex = scCurrent.PrintSequence;
                                    scCurrent.PrintSequence = scTarget.PrintSequence;
                                    scTarget.PrintSequence = switchIndex;
                                }
                                else
                                {
                                    return new DO_ReturnParameter() { Status = false, StatusCode = "W0081", Message = scCurrent.ServiceClassDesc + string.Format(_localizer[name: "W0081"]) };
                                }
                            }
                        }
                        else
                        {
                            if (isMoveUp)
                            {
                                var isTop = db.GtEssrcls.Where(w => w.PrintSequence < scCurrent.PrintSequence && w.ServiceGroupId == serviceGroupId && w.ServiceClassId != w.ParentId && w.ParentId == scCurrent.ParentId).Count();
                                if (isTop > 0)
                                {
                                    var scTarget = db.GtEssrcls.Where(w => w.PrintSequence < scCurrent.PrintSequence && w.ServiceGroupId == serviceGroupId && w.ServiceClassId != w.ParentId && w.ParentId == scCurrent.ParentId).OrderByDescending(o => o.PrintSequence).FirstOrDefault();
                                    switchIndex = scCurrent.PrintSequence;
                                    scCurrent.PrintSequence = scTarget.PrintSequence;
                                    scTarget.PrintSequence = switchIndex;
                                }
                                else
                                {
                                    return new DO_ReturnParameter() { Status = false, StatusCode = "W0080", Message = scCurrent.ServiceClassDesc + string.Format(_localizer[name: "W0080"]) };
                                }
                            }
                            else if (isMoveDown)
                            {
                                var isBottom = db.GtEssrcls.Where(w => w.PrintSequence > scCurrent.PrintSequence && w.ServiceGroupId == serviceGroupId && w.ServiceClassId != w.ParentId && w.ParentId == scCurrent.ParentId).Count();
                                if (isBottom > 0)
                                {
                                    var scTarget = db.GtEssrcls.Where(w => w.PrintSequence > scCurrent.PrintSequence && w.ServiceGroupId == serviceGroupId && w.ServiceClassId != w.ParentId && w.ParentId == scCurrent.ParentId).OrderBy(o => o.PrintSequence).FirstOrDefault();
                                    switchIndex = scCurrent.PrintSequence;
                                    scCurrent.PrintSequence = scTarget.PrintSequence;
                                    scTarget.PrintSequence = switchIndex;
                                }
                                else
                                {
                                    return new DO_ReturnParameter() { Status = false, StatusCode = "W0081", Message = scCurrent.ServiceClassDesc + string.Format(_localizer[name: "W0081"]) };
                                }
                            }
                        }



                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }
        public async Task<DO_ReturnParameter> DeleteServiceClass(int serviceClassId)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {

                        var cLinkExist = db.GtEssrcls.Where(w => w.ParentId == serviceClassId && w.ParentId != w.ServiceClassId).Count();
                        if (cLinkExist > 0)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0087", Message = string.Format(_localizer[name: "W0087"]) };
                        }
                        var sLinkExist = db.GtEssrms.Where(w => w.ServiceClassId == serviceClassId).Count();
                        if (sLinkExist > 0)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0088", Message = string.Format(_localizer[name: "W0088"]) };
                        }

                        var ServiceClass = db.GtEssrcls.Where(w => w.ServiceClassId == serviceClassId).FirstOrDefault();
                        if (ServiceClass != null)
                        {
                            var classParam = db.GtEspascs.Where(w => w.ServiceClassId == serviceClassId).ToList();
                            foreach (GtEspasc p in classParam)
                            {
                                db.GtEspascs.Remove(p);
                            }
                            db.GtEssrcls.Remove(ServiceClass);
                        }

                        await db.SaveChangesAsync();
                        dbContext.Commit();
                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0005", Message = string.Format(_localizer[name: "S0005"]) };

                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }
        #endregion
    }
}

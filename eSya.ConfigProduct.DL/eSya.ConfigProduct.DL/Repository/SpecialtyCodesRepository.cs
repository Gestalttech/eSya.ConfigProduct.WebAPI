﻿using eSya.ConfigProduct.DL.Entities;
using eSya.ConfigProduct.DO;
using eSya.ConfigProduct.IF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ConfigProduct.DL.Repository
{
    public class SpecialtyCodesRepository : ISpecialtyCodesRepository
    {
        private readonly IStringLocalizer<SpecialtyCodesRepository> _localizer;
        public SpecialtyCodesRepository(IStringLocalizer<SpecialtyCodesRepository> localizer)
        {
            _localizer = localizer;
        }
        #region Specilities
        public async Task<DO_ReturnParameter> InsertSpecialtyCodes(DO_SpecialtyCodes obj)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    using (var dbContext = db.Database.BeginTransaction())
                    {
                        try
                        {
                            var isSpecialtyExist = db.GtEsspcds.Where(x => x.SpecialtyDesc.ToUpper().Trim() == obj.SpecialtyDesc.ToUpper().Trim()).Count();
                            if (isSpecialtyExist > 0)
                            {

                                return new DO_ReturnParameter() { Status = false, StatusCode = "W0089", Message = string.Format(_localizer[name: "W0089"]) };
                            }

                            int maxSpecialtyId = db.GtEsspcds.Select(d => d.SpecialtyId).DefaultIfEmpty().Max();
                            int SpecId = maxSpecialtyId + 1;

                            var sMaster = new GtEsspcd
                            {
                                SpecialtyId = SpecId,
                                SpecialtyDesc = obj.SpecialtyDesc.Trim(),
                                Gender = obj.Gender,
                                SpecialtyType = obj.SpecialtyType,
                                SpecialtyGroup = obj.SpecialtyGroup,
                                MedicalIcon=obj.MedicalIcon,
                                FocusArea=obj.FocusArea,
                                AgeRangeFrom=obj.AgeRangeFrom,
                                RangePeriodFrom=obj.RangePeriodFrom,
                                AgeRangeTo=obj.AgeRangeTo,
                                RangePeriodTo=obj.RangePeriodTo,
                                ActiveStatus = obj.ActiveStatus,
                                FormId = obj.FormId,
                                CreatedBy = obj.UserID,
                                CreatedOn = System.DateTime.Now,
                                CreatedTerminal = obj.TerminalID,

                            };
                            db.GtEsspcds.Add(sMaster);

                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };
                        }
                        catch (DbUpdateException ex)
                        {
                            dbContext.Rollback();
                            throw new Exception(CommonMethod.GetValidationMessageFromException(ex));
                        }
                        catch (Exception ex)
                        {
                            dbContext.Rollback();
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> UpdateSpecialtyCodes(DO_SpecialtyCodes obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEsspcd sp_ms = db.GtEsspcds.Where(w => w.SpecialtyId == obj.SpecialtyID).FirstOrDefault();
                        if (sp_ms == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0090", Message = string.Format(_localizer[name: "W0090"]) };
                        }

                        sp_ms.SpecialtyDesc = obj.SpecialtyDesc.Trim();
                        sp_ms.Gender = obj.Gender;
                        sp_ms.SpecialtyType = obj.SpecialtyType;
                        sp_ms.SpecialtyGroup = obj.SpecialtyGroup;
                        sp_ms.MedicalIcon = obj.MedicalIcon;
                        sp_ms.FocusArea = obj.FocusArea;
                        sp_ms.AgeRangeFrom = obj.AgeRangeFrom;
                        sp_ms.RangePeriodFrom = obj.RangePeriodFrom;
                        sp_ms.AgeRangeTo = obj.AgeRangeTo;
                        sp_ms.RangePeriodTo=obj.RangePeriodTo;
                        sp_ms.ActiveStatus = obj.ActiveStatus;
                        sp_ms.ModifiedBy = obj.UserID;
                        sp_ms.ModifiedOn = System.DateTime.Now;
                        sp_ms.ModifiedTerminal = obj.TerminalID;

                        await db.SaveChangesAsync();
                        dbContext.Commit();
                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0002", Message = string.Format(_localizer[name: "S0002"]) };
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public async Task<DO_ReturnParameter> DeleteSpecialtyCodes(DO_SpecialtyCodes obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEsspcd sp_ms = db.GtEsspcds.Where(w => w.SpecialtyId == obj.SpecialtyID).FirstOrDefault();
                        if (sp_ms == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0090", Message = string.Format(_localizer[name: "W0090"]) };
                        }

                        sp_ms.ActiveStatus = false;
                        sp_ms.ModifiedBy = obj.UserID;
                        sp_ms.ModifiedOn = System.DateTime.Now;
                        sp_ms.ModifiedTerminal = obj.TerminalID;

                        await db.SaveChangesAsync();
                        dbContext.Commit();
                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0005", Message = string.Format(_localizer[name: "S0005"]) };
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public async Task<List<DO_SpecialtyCodes>> GetSpecialtyCodesList()
        {
            using (var db = new eSyaEnterprise())
            {
                try
                {
                    var sp_ms = db.GtEsspcds
                        //.Where(w => w.ActiveStatus == true)
                        .AsNoTracking()
                        .Select(x => new DO_SpecialtyCodes
                        {
                            SpecialtyID = x.SpecialtyId,
                            SpecialtyDesc = x.SpecialtyDesc,
                            Gender = x.Gender,
                            SpecialtyType = x.SpecialtyType,
                            SpecialtyGroup = x.SpecialtyGroup,
                            ActiveStatus = x.ActiveStatus

                        }).OrderBy(x => x.SpecialtyDesc).ToListAsync();

                    return await sp_ms;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<DO_SpecialtyCodes> GetSpecialtyCodes(int specialtyId)
        {
            using (var db = new eSyaEnterprise())
            {
                try
                {
                    var sp_ms = db.GtEsspcds
                        .Where(w => w.SpecialtyId == specialtyId)
                        .AsNoTracking()
                        .Select(x => new DO_SpecialtyCodes
                        {
                            SpecialtyID = x.SpecialtyId,
                            SpecialtyDesc = x.SpecialtyDesc,
                            Gender = x.Gender,
                            SpecialtyType = x.SpecialtyType,
                            SpecialtyGroup = x.SpecialtyGroup,
                            MedicalIcon=x.MedicalIcon,
                            FocusArea=x.FocusArea,
                            AgeRangeFrom=x.AgeRangeFrom,
                            RangePeriodFrom=x.RangePeriodFrom,
                            AgeRangeTo=x.AgeRangeTo,
                            RangePeriodTo=x.RangePeriodTo,
                            ActiveStatus = x.ActiveStatus

                        }).FirstOrDefaultAsync();

                    return await sp_ms;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        #endregion
    }
}

using eSya.ConfigProduct.DL.Entities;
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
    public class SpecialtyUnitsRepository : ISpecialtyUnitsRepository
    {
        private readonly IStringLocalizer<SpecialtyUnitsRepository> _localizer;
        public SpecialtyUnitsRepository(IStringLocalizer<SpecialtyUnitsRepository> localizer)
        {
            _localizer = localizer;
        }
        #region Specialty Units
        public async Task<List<DO_SpecialtyUnit>> GetSpecialtyListByBusinessKey(int businessKey)
        {
            using (var db = new eSyaEnterprise())
            {
                try
                {
                    //var defaultDate = DateTime.Now.Date;
                    var sp_u = db.GtEsspcds
                        .Join(db.GtEsspbls,
                        s => new { s.SpecialtyId },
                        b => new { b.SpecialtyId },
                        (s, b) => new { s, b })
                        //.GroupJoin(db.GtEsspun.Where(w=> w.BusinessKey==businessKey && w.EffectiveFrom >= defaultDate).OrderByDescending(o => o.ActiveStatus),
                        //sb => sb.s.SpecialtyId,
                        //u => u.SpecialtyId,
                        //(sb, u) => new { sb, u = u.FirstOrDefault() })
                        .Where(w => w.b.BusinessKey == businessKey && w.s.ActiveStatus && w.b.ActiveStatus)
                        .AsNoTracking()
                        .Select(x => new DO_SpecialtyUnit
                        {
                            SpecialtyID = x.s.SpecialtyId,
                            SpecialtyDesc = x.s.SpecialtyDesc,
                            //EffectiveFrom= x.u != null ? x.u.EffectiveFrom : defaultDate,

                        })
                        .OrderBy(x => x.SpecialtyDesc).ToListAsync();

                    return await sp_u;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        //public async Task<List<DO_SpecialtyUnit>> GetUnitsValidityBySpecialty(int businessKey, int specialtyId)
        //{
        //    using (var db = new eSyaEnterprise())
        //    {
        //        try
        //        {
        //            var sp_u = db.GtEsspuns
        //                .Where(w => w.BusinessKey == businessKey && w.SpecialtyId == specialtyId && w.ActiveStatus)
        //                .AsNoTracking()
        //                .Select(x => new DO_SpecialtyUnit
        //                {
        //                    SpecialtyID = x.SpecialtyId,
        //                    EffectiveFrom = x.EffectiveFrom,
        //                    NoOfUnits = x.NoOfUnits,
        //                    ActiveStatus=x.ActiveStatus,
        //                    EffectiveTill=x.EffectiveTill
        //                })
        //                .OrderByDescending(x => x.EffectiveFrom).ToListAsync();

        //            return await sp_u;
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
        //}

        //public async Task<DO_ReturnParameter> InsertSpecialtyUnitsValidity(DO_SpecialtyUnit obj)
        //{
        //    try
        //    {
        //        using (var db = new eSyaEnterprise())
        //        {
        //            using (var dbContext = db.Database.BeginTransaction())
        //            {
        //                try
        //                {
        //                    var dateExists = db.GtEsspuns.Where(x => x.EffectiveFrom == obj.EffectiveFrom).FirstOrDefault();
        //                    if (dateExists != null)
        //                    {
        //                        return new DO_ReturnParameter() { Status = false, StatusCode = "W0091", Message = string.Format(_localizer[name: "W0091"]) };
        //                    }
        //                    var sUnitValidity = new GtEsspun
        //                    {
        //                        BusinessKey = obj.BusinessKey,
        //                        SpecialtyId = obj.SpecialtyID,
        //                        EffectiveFrom = obj.EffectiveFrom,
        //                        NoOfUnits = obj.NoOfUnits,
        //                        ActiveStatus = obj.ActiveStatus,
        //                        FormId = obj.FormId,
        //                        CreatedBy = obj.UserID,
        //                        CreatedOn = System.DateTime.Now,
        //                        CreatedTerminal = obj.TerminalID,

        //                    };
        //                    db.GtEsspuns.Add(sUnitValidity);

        //                    await db.SaveChangesAsync();
        //                    dbContext.Commit();
        //                    return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };
        //                }
        //                catch (DbUpdateException ex)
        //                {
        //                    dbContext.Rollback();
        //                    throw new Exception(CommonMethod.GetValidationMessageFromException(ex));
        //                }
        //                catch (Exception ex)
        //                {
        //                    dbContext.Rollback();
        //                    throw ex;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public async Task<List<DO_SpecialtyUnit>> GetUnitsValidityBySpecialty(int businessKey, int specialtyId)
        {
            using (var db = new eSyaEnterprise())
            {
                try
                {
                    var sp_u = db.GtEsspuns
                        .Where(w => w.BusinessKey == businessKey && w.SpecialtyId == specialtyId )
                        .AsNoTracking()
                        .Select(x => new DO_SpecialtyUnit
                        {
                            SpecialtyID = x.SpecialtyId,
                            EffectiveFrom = x.EffectiveFrom,
                            NoOfUnits = x.NoOfUnits,
                            ActiveStatus = x.ActiveStatus,
                            EffectiveTill = x.EffectiveTill
                        })
                        .OrderByDescending(x => x.EffectiveFrom).ToListAsync();

                    return await sp_u;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public async Task<DO_ReturnParameter> InsertSpecialtyUnitsValidity(DO_SpecialtyUnit obj)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {

                        var bcExist = db.GtEsspuns.Where(w => w.SpecialtyId == obj.SpecialtyID && w.BusinessKey == obj.BusinessKey && w.EffectiveTill == null).FirstOrDefault();
                        if (bcExist != null)
                        {
                            if (obj.EffectiveFrom != bcExist.EffectiveFrom)
                            {
                                if (obj.EffectiveFrom < bcExist.EffectiveFrom)
                                {
                                    return new DO_ReturnParameter() { Status = false, StatusCode = "W0183", Message = string.Format(_localizer[name: "W0183"]) };
                                }
                                bcExist.EffectiveTill = obj.EffectiveFrom.AddDays(-1);
                                bcExist.ModifiedBy = obj.UserID;
                                bcExist.ModifiedOn = DateTime.Now;
                                bcExist.ModifiedTerminal = obj.TerminalID;
                                bcExist.ActiveStatus = false;

                                var bc = new GtEsspun
                                {
                                    BusinessKey = obj.BusinessKey,
                                    SpecialtyId = obj.SpecialtyID,
                                    NoOfUnits = obj.NoOfUnits,
                                    EffectiveFrom = obj.EffectiveFrom,
                                    ActiveStatus = obj.ActiveStatus,
                                    FormId = obj.FormId,
                                    CreatedBy = obj.UserID,
                                    CreatedOn = DateTime.Now,
                                    CreatedTerminal = obj.TerminalID
                                };
                                db.GtEsspuns.Add(bc);


                            }
                            else
                            {
                                bcExist.NoOfUnits = obj.NoOfUnits;
                                bcExist.ActiveStatus = obj.ActiveStatus;
                                bcExist.ModifiedBy = obj.UserID;
                                bcExist.ModifiedOn = DateTime.Now;
                                bcExist.ModifiedTerminal = obj.TerminalID;
                            }

                        }
                        else
                        {
                            if (obj.NoOfUnits!=0)
                            {
                                var bcal = new GtEsspun
                                {
                                    BusinessKey = obj.BusinessKey,
                                    SpecialtyId = obj.SpecialtyID,
                                    NoOfUnits = obj.NoOfUnits,
                                    EffectiveFrom = obj.EffectiveFrom,
                                    ActiveStatus = obj.ActiveStatus,
                                    FormId = obj.FormId,
                                    CreatedBy = obj.UserID,
                                    CreatedOn = DateTime.Now,
                                    CreatedTerminal = obj.TerminalID
                                };
                                db.GtEsspuns.Add(bcal);
                            }

                        }
                        await db.SaveChangesAsync();
                        dbContext.Commit();
                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };

                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));
                    }
                }
            }
        }
        #endregion

        #region Specialty IP Info
        public async Task<DO_SpecialtyUnit> GetSpecialtyIPInfo(int businessKey, int specialtyId)
        {
            using (var db = new eSyaEnterprise())
            {
                try
                {
                    var sp_u = db.GtEsspmcs
                        .Where(w => w.BusinessKey == businessKey && w.SpecialtyId == specialtyId)
                        .AsNoTracking()
                        .Select(x => new DO_SpecialtyUnit
                        {
                            NewPatient = x.NewPatient,
                            RepeatPatient = x.RepeatPatient,
                            NoOfMaleBeds = x.NoOfMaleBeds,
                            NoOfFemaleBeds = x.NoOfFemaleBeds,
                            NoOfCommonBeds = x.NoOfCommonBeds,
                            MaxStayAllowed = x.MaxStayAllowed
                        })
                        .FirstOrDefaultAsync();

                    return await sp_u;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<DO_ReturnParameter> AddOrUpdateSpecialtyIPInfo(DO_SpecialtyUnit obj)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    using (var dbContext = db.Database.BeginTransaction())
                    {
                        try
                        {
                            var RecordExist = db.GtEsspmcs.Where(w => w.BusinessKey == obj.BusinessKey && w.SpecialtyId == obj.SpecialtyID).FirstOrDefault();
                            if (RecordExist == null)
                            {
                                var sIPInfo = new GtEsspmc
                                {
                                    BusinessKey = obj.BusinessKey,
                                    SpecialtyId = obj.SpecialtyID,
                                    NewPatient = obj.NewPatient,
                                    RepeatPatient = obj.RepeatPatient,
                                    NoOfMaleBeds = obj.NoOfMaleBeds,
                                    NoOfFemaleBeds = obj.NoOfFemaleBeds,
                                    NoOfCommonBeds = obj.NoOfCommonBeds,
                                    MaxStayAllowed = obj.MaxStayAllowed,

                                    ActiveStatus = true,
                                    FormId = obj.FormId,
                                    CreatedBy = obj.UserID,
                                    CreatedOn = System.DateTime.Now,
                                    CreatedTerminal = obj.TerminalID
                                };
                                db.GtEsspmcs.Add(sIPInfo);
                            }
                            else
                            {

                                RecordExist.NewPatient = obj.NewPatient;
                                RecordExist.RepeatPatient = obj.RepeatPatient;
                                RecordExist.NoOfMaleBeds = obj.NoOfMaleBeds;
                                RecordExist.NoOfFemaleBeds = obj.NoOfFemaleBeds;
                                RecordExist.NoOfCommonBeds = obj.NoOfCommonBeds;
                                RecordExist.MaxStayAllowed = obj.MaxStayAllowed;

                                RecordExist.ModifiedBy = obj.UserID;
                                RecordExist.ModifiedOn = System.DateTime.Now;
                                RecordExist.ModifiedTerminal = obj.TerminalID;
                            }
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

        #endregion
    }
}

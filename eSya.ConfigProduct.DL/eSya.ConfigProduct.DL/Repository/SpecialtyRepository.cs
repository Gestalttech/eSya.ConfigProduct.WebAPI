using eSya.ConfigProduct.DL.Entities;
using eSya.ConfigProduct.DO;
using eSya.ConfigProduct.IF;
using Microsoft.Data.SqlClient.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ConfigProduct.DL.Repository
{
    public class SpecialtyRepository : ISpecialtyRepository
    {
        private readonly IStringLocalizer<SpecialtyRepository> _localizer;
        public SpecialtyRepository(IStringLocalizer<SpecialtyRepository> localizer)
        {
            _localizer = localizer;
        }
        #region Specilalty Business Link
        public async Task<DO_ReturnParameter> InsertSpecialtyBusinessLink(DO_SpecialtyBusinessLink obj)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    using (var dbContext = db.Database.BeginTransaction())
                    {
                        try
                        {

                            var sMaster = new GtEsspbl
                            {
                                BusinessKey = obj.BusinessKey,
                                SpecialtyId = obj.SpecialtyID,
                                ActiveStatus = obj.ActiveStatus,
                                FormId = obj.FormId,
                                CreatedBy = obj.UserID,
                                CreatedOn = System.DateTime.Now,
                                CreatedTerminal = obj.TerminalID,

                            };
                            db.GtEsspbls.Add(sMaster);

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

        public async Task<DO_ReturnParameter> InsertSpecialtyBusinessLinkList(DO_SpecialtyBusinessLink obj, List<DO_SpecialtyParameter> objPar)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        //foreach (DO_SpecialtyBusinessLink sm in obj)
                        {
                            GtEsspbl spBl = db.GtEsspbls.Where(x => x.BusinessKey == obj.BusinessKey && x.SpecialtyId == obj.SpecialtyID).FirstOrDefault();
                            if (spBl != null)
                            {
                                spBl.ActiveStatus = obj.ActiveStatus;
                                spBl.ModifiedBy = obj.UserID;
                                spBl.ModifiedOn = System.DateTime.Now;
                                spBl.ModifiedTerminal = obj.TerminalID;
                            }
                            else
                            {
                                var sMaster = new GtEsspbl
                                {
                                    BusinessKey = obj.BusinessKey,
                                    SpecialtyId = obj.SpecialtyID,
                                    ActiveStatus = obj.ActiveStatus,
                                    FormId = obj.FormId,
                                    CreatedBy = obj.UserID,
                                    CreatedOn = System.DateTime.Now,
                                    CreatedTerminal = obj.TerminalID,

                                };
                                db.GtEsspbls.Add(sMaster);
                            }
                        }

                        foreach (DO_SpecialtyParameter sm in objPar)
                        {
                            GtEssppa sPar = db.GtEssppas.Where(x => x.BusinessKey == sm.BusinessKey && x.SpecialtyId == sm.SpecialtyID && x.ParameterId == sm.ParameterID).FirstOrDefault();
                            if (sPar != null)
                            {
                                sPar.ParmAction = sm.ParmAction;
                                sPar.ParmDesc = sm.ParmDesc;
                                sPar.ParmPerc = sm.ParmPerc;
                                sPar.ParmValue = sm.ParmValue;
                            }
                            else
                            {
                                var sMaster = new GtEssppa
                                {
                                    BusinessKey = sm.BusinessKey,
                                    SpecialtyId = sm.SpecialtyID,
                                    ParameterId = sm.ParameterID,
                                    ParmPerc = sm.ParmPerc,
                                    ParmAction = sm.ParmAction,
                                    ParmDesc = sm.ParmDesc,
                                    ParmValue = sm.ParmValue,
                                    ActiveStatus = sm.ActiveStatus,
                                    FormId = sm.FormId,
                                    CreatedBy = sm.UserID,
                                    CreatedOn = System.DateTime.Now,
                                    CreatedTerminal = sm.TerminalID,

                                };
                                db.GtEssppas.Add(sMaster);
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
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public async Task<DO_ReturnParameter> UpdateSpecialtyBusinessLink(DO_SpecialtyBusinessLink obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEsspbl sp_ms = db.GtEsspbls.Where(w => w.SpecialtyId == obj.SpecialtyID && w.BusinessKey == obj.BusinessKey).FirstOrDefault();
                        if (sp_ms == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0105", Message = string.Format(_localizer[name: "W0105"]) };
                        }

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

        public async Task<List<DO_SpecialtyBusinessLink>> GetSpecialtyBusinessList(int businessKey)
        {
            using (var db = new eSyaEnterprise())
            {
                try
                {
                    //var sp_ms = db.GtEsspcds.Where(w=>w.ActiveStatus==true)
                    //    .GroupJoin(db.GtEsspbls.Where(x => x.BusinessKey == businessKey && x.ActiveStatus),
                    //    s => new { s.SpecialtyId },
                    //    b => new { b.SpecialtyId },
                    //    (s, b) => new { s, b = b.FirstOrDefault() }).DefaultIfEmpty()
                    //    .AsNoTracking()
                    //    .Select(x => new DO_SpecialtyBusinessLink
                    //    {
                    //        BusinessKey = x.b != null ? x.b.BusinessKey : 0,
                    //        SpecialtyID = x.s.SpecialtyId,
                    //        SpecialtyDesc = x.s.SpecialtyDesc,
                    //        ActiveStatus = x.b != null ? x.b.ActiveStatus : false

                    //    }).OrderBy(x => x.SpecialtyDesc).ToListAsync();

                    var sp_ms = await db.GtEsspcds.Where(w => w.ActiveStatus == true)
                   .GroupJoin(db.GtEsspbls.Where(x => x.BusinessKey == businessKey && x.ActiveStatus),
                     d =>new { d.SpecialtyId },
                     l =>new { l.SpecialtyId },
                    (sp, spbl) => new { sp, spbl })
                   .SelectMany(z => z.spbl.DefaultIfEmpty(),
                    (a, b) => new DO_SpecialtyBusinessLink
                    {
                        SpecialtyID =a.sp.SpecialtyId,
                        SpecialtyDesc =a.sp.SpecialtyDesc,
                        BusinessKey = b == null ? 0 : b.BusinessKey,
                        ActiveStatus = b == null ? false : b.ActiveStatus
                    }).OrderBy(x => x.SpecialtyDesc).ToListAsync();


                    return  sp_ms;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<List<DO_SpecialtyBusinessLink>> GetSpecialtyListForBusinessKey(int businessKey)
        {
            using (var db = new eSyaEnterprise())
            {
                try
                {
                    var sp_ms = db.GtEsspcds
                        .Join(db.GtEsspbls,
                        s => new { s.SpecialtyId },
                        b => new { b.SpecialtyId },
                        (s, b) => new { s, b })
                        .Where(w => w.b.BusinessKey == businessKey && w.s.ActiveStatus && w.b.ActiveStatus)
                        .AsNoTracking()
                        .Select(x => new DO_SpecialtyBusinessLink
                        {
                            SpecialtyID = x.s.SpecialtyId,
                            SpecialtyDesc = x.s.SpecialtyDesc,

                        })
                        .GroupBy(y => y.SpecialtyID, (key, grp) => grp.FirstOrDefault())
                        .OrderBy(x => x.SpecialtyDesc).ToListAsync();

                    return await sp_ms;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public async Task<List<DO_SpecialtyParameter>> GetSpecialtyParameterList(int businessKey, int specialtyId)
        {
            using (var db = new eSyaEnterprise())
            {
                try
                {
                    var sp_ms = db.GtEssppas
                        .Where(w => w.SpecialtyId == specialtyId && w.BusinessKey == businessKey && w.ActiveStatus)
                        .AsNoTracking()
                        .Select(x => new DO_SpecialtyParameter
                        {
                            ParameterID = x.ParameterId,
                            ParmAction = x.ParmAction,
                            ParmDesc = x.ParmDesc,
                            ParmValue = x.ParmValue,
                            ParmPerc = x.ParmPerc

                        }).ToListAsync();

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

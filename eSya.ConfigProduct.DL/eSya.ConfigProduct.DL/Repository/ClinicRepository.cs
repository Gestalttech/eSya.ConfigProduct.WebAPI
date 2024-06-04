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
    public class ClinicRepository : IClinicRepository
    {
        private readonly IStringLocalizer<ClinicRepository> _localizer;
        public ClinicRepository(IStringLocalizer<ClinicRepository> localizer)
        {
            _localizer = localizer;
        }

        #region Consultant - ClinicLink
        public async Task<DO_ReturnParameter> InsertUpdateOPClinicLink(List<DO_OPClinic> obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (DO_OPClinic oc in obj)
                        {


                            var lst = db.GtEsopcls.Where(x => x.BusinessKey == oc.BusinessKey && x.ClinicId == oc.ClinicId && x.ConsultationId == oc.ConsultationId).ToList();
                            if (lst.Count > 0)
                            {
                                foreach (var i in lst)
                                {
                                    db.GtEsopcls.Remove(i);
                                }
                            }
                            if (oc.ActiveStatus)
                            {
                                var op_cl = new GtEsopcl
                                {
                                    BusinessKey = oc.BusinessKey,
                                    ClinicId = oc.ClinicId,
                                    ConsultationId = oc.ConsultationId,
                                    ActiveStatus = oc.ActiveStatus,
                                    FormId = oc.FormId,
                                    CreatedBy = oc.UserID,
                                    CreatedOn = System.DateTime.Now,
                                    CreatedTerminal = oc.TerminalID,
                                };
                                db.GtEsopcls.Add(op_cl);
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

        public async Task<List<DO_OPClinic>> GetClinicConsultantIdList(int businessKey)
        {
            using (var db = new eSyaEnterprise())
            {
                try
                {
                    //var cl_co = db.GtEcapcds
                    //    .Join(db.GtEcapcds.Where(w => w.CodeType == CodeTypeValue.ConsultationType),
                    //    l => true,
                    //    c => true,
                    //    (l, c) => new { l, c })
                    //    .GroupJoin(db.GtEsopcls.Where(w => w.BusinessKey == businessKey),
                    //    lc => new { ConsultationId = lc.c.ApplicationCode, ClinicId = lc.l.ApplicationCode },
                    //    o => new { o.ConsultationId, o.ClinicId },
                    //    (lc, o) => new { lc, o = o.FirstOrDefault() }).DefaultIfEmpty()
                    //    .Where(w => w.lc.l.CodeType == CodeTypeValue.Clinic)
                    //    .AsNoTracking()
                    //    .Select(r => new DO_OPClinic
                    //    {
                    //        BusinessKey = r.o != null ? r.o.BusinessKey : 0,
                    //        ClinicId = r.lc.l.ApplicationCode,
                    //        ClinicDesc = r.lc.l.CodeDesc,
                    //        ConsultationId = r.lc.c.ApplicationCode,
                    //        ConsultationDesc = r.lc.c.CodeDesc,
                    //        ActiveStatus = r.o != null ? r.o.ActiveStatus : false
                    //    }).OrderBy(x => x.ClinicDesc).ToListAsync();

                    //return await cl_co;


                    var cl_co = db.GtEcapcds.Where(x=>x.CodeType== CodeTypeValue.Clinic)
                       .Join(db.GtEcapcds.Where(w => w.CodeType == CodeTypeValue.ConsultationType),
                       l => true,
                       c => true,
                       (l, c) => new { l, c })
                       .GroupJoin(db.GtEsopcls.Where(w => w.BusinessKey == businessKey),
                       lc => new { ConsultationId = lc.c.ApplicationCode, ClinicId = lc.l.ApplicationCode },
                       o => new { o.ConsultationId, o.ClinicId },
                       (lc, o) => new { lc, o })
                        .SelectMany(z => z.o.DefaultIfEmpty(),
                        (a, b) => new DO_OPClinic
                       {
                          
                           ClinicId =a.lc.l.ApplicationCode,
                           ClinicDesc = a.lc.l.CodeDesc,
                           ConsultationId =a.lc.c.ApplicationCode,
                           ConsultationDesc = a.lc.c.CodeDesc,
                            BusinessKey = b != null ? b.BusinessKey : 0,
                            ActiveStatus = b!= null ? b.ActiveStatus : false
                       }).OrderBy(x => x.ClinicDesc).ToListAsync();

                    return await cl_co;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        #endregion

        #region Map Business Linked Specialaties to Clinic & Consultation Type
        public async Task<List<DO_SpecialtyBusinessLink>> GetMappedSpecialtyListbyBusinessKey(int businessKey)
        {
            using (var db = new eSyaEnterprise())
            {
                try
                {
                    var sp_ms = db.GtEsspbls
                        .Join(db.GtEsspcds,
                        b => new { b.SpecialtyId },
                        s => new { s.SpecialtyId },
                        (b, s) => new { b, s })
                        .Where(w => w.b.BusinessKey == businessKey && w.b.ActiveStatus && w.s.ActiveStatus)
                        .AsNoTracking()
                        .Select(x => new DO_SpecialtyBusinessLink
                        {
                            SpecialtyID = x.b.SpecialtyId,
                            SpecialtyDesc = x.s.SpecialtyDesc,

                        }).OrderBy(x => x.SpecialtyDesc).ToList();
                    var dist = sp_ms.GroupBy(y => y.SpecialtyID, (key, grp) => grp.FirstOrDefault());

                    return sp_ms;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
               
            }
        }

        public async Task<List<DO_MapSpecialtyClinicConsultationType>> GetMapedSpecialtyClinicConsultationTypeBySpecialtyID(int businessKey,int specialtyId)
        {
           
                using (var db = new eSyaEnterprise())
                {
                    try
                    {

                        var do_cl = db.GtEsopcls.Where(x => x.BusinessKey == businessKey && x.ActiveStatus)
                        .Join(db.GtEcapcds.Where(w => w.CodeType == CodeTypeValue.Clinic && w.ActiveStatus),
                              l => new { l.ClinicId },
                              c => new { ClinicId = c.ApplicationCode },
                              (l, c) => new { l, c })
                           .Join(db.GtEcapcds.Where(w => w.CodeType == CodeTypeValue.ConsultationType && w.ActiveStatus),
                               lc => new { lc.l.ConsultationId },
                               o => new { ConsultationId = o.ApplicationCode },
                               (lc, o) => new { lc, o })
                       .GroupJoin(db.GtEsspcls.Where(w => w.BusinessKey == businessKey && w.SpecialtyId == specialtyId ),
                                 lco => new { lco.lc.l.BusinessKey, lco.lc.l.ClinicId, lco.lc.l.ConsultationId },
                                 d => new { d.BusinessKey, d.ClinicId, d.ConsultationId },
                                 (lco, d) => new { lco, d })
                       .SelectMany(z => z.d.DefaultIfEmpty(),
                        (a, b) => new DO_MapSpecialtyClinicConsultationType
                        {
                            BusinessKey = b == null ? 0 : b.BusinessKey,
                            ClinicId = a.lco.lc.l.ClinicId,
                            ClinicDesc = a.lco.lc.c.CodeDesc,
                            ConsultationId = a.lco.lc.l.ConsultationId,
                            ConsultationDesc = a.lco.o.CodeDesc,
                            ActiveStatus = b == null ? false : b.ActiveStatus
                        }).ToList();
                        var Distinctclinics = do_cl.GroupBy(x => new { x.ClinicId, x.ConsultationId }).Select(g => g.First()).ToList();
                        return Distinctclinics.ToList();


                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            
        }

        public async Task<DO_ReturnParameter> InsertUpdateSpecialtyClinicConsultationTypeLink(List<DO_MapSpecialtyClinicConsultationType> obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (DO_MapSpecialtyClinicConsultationType dc in obj)
                        {
                            var lst = db.GtEsspcls.Where(x => x.BusinessKey == dc.BusinessKey && x.SpecialtyId == dc.SpecialtyId
                                                         && x.ClinicId == dc.ClinicId && x.ConsultationId == dc.ConsultationId).ToList();
                            if (lst.Count > 0)
                            {
                                foreach (var i in lst)
                                {
                                    db.GtEsspcls.Remove(i);
                                }
                            }

                            if (dc.ActiveStatus)
                            {
                                var sp_cl = new GtEsspcl
                                {
                                    BusinessKey = dc.BusinessKey,
                                    SpecialtyId = dc.SpecialtyId,
                                    ClinicId = dc.ClinicId,
                                    ConsultationId = dc.ConsultationId,
                                    ActiveStatus = dc.ActiveStatus,
                                    FormId = dc.FormId,
                                    CreatedBy = dc.UserID,
                                    CreatedOn = System.DateTime.Now,
                                    CreatedTerminal = dc.TerminalID,
                                };
                                db.GtEsspcls.Add(sp_cl);
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
        #endregion
    }
}

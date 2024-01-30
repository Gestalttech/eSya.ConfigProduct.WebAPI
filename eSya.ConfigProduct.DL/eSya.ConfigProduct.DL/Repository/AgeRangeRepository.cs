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
    public class AgeRangeRepository : IAgeRangeRepository
    {
        private readonly IStringLocalizer<AgeRangeRepository> _localizer;
        public AgeRangeRepository(IStringLocalizer<AgeRangeRepository> localizer)
        {
            _localizer = localizer;
        }
        #region AgeRange
        public async Task<List<DO_AgeRange>> GetAgeRanges()
        {
            using (var db = new eSyaEnterprise())
            {
                var age = db.GtEbeagrs
               .Select(s => new DO_AgeRange
               {
                   AgeRangeId = s.AgeRangeId,
                   RangeDesc = s.RangeDesc,
                   AgeRangeFrom = s.AgeRangeFrom,
                   RangeFromPeriod = s.RangeFromPeriod,
                   AgeRangeTo = s.AgeRangeTo,
                   RangeToPeriod = s.RangeToPeriod,
                   ActiveStatus = s.ActiveStatus
               })
               .ToListAsync();
                return await age;
            }
        }

        public async Task<DO_ReturnParameter> InsertOrUpdateAgeRange(DO_AgeRange obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var _age = db.GtEbeagrs.Where(w => w.AgeRangeId == obj.AgeRangeId).FirstOrDefault();
                        if (_age != null)
                        {
                            _age.RangeDesc = obj.RangeDesc;
                            _age.AgeRangeFrom = obj.AgeRangeFrom;
                            _age.RangeFromPeriod = obj.RangeFromPeriod;
                            _age.AgeRangeTo = obj.AgeRangeTo;
                            _age.RangeToPeriod = obj.RangeToPeriod;
                            _age.ActiveStatus = obj.ActiveStatus;
                            _age.ModifiedBy = obj.UserID;
                            _age.ModifiedOn = System.DateTime.Now;
                            _age.ModifiedTerminal = obj.TerminalID;
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0002", Message = string.Format(_localizer[name: "S0002"]) };
                        }
                        else
                        {
                            int max__ageId = db.GtEbeagrs.Select(c => c.AgeRangeId).DefaultIfEmpty().Max();
                            int _ageId = max__ageId + 1;
                            var _agerange = new GtEbeagr
                            {
                                AgeRangeId = _ageId,
                                RangeDesc = obj.RangeDesc,
                                AgeRangeFrom = obj.AgeRangeFrom,
                                RangeFromPeriod = obj.RangeFromPeriod,
                                AgeRangeTo = obj.AgeRangeTo,
                                RangeToPeriod = obj.RangeToPeriod,
                                ActiveStatus = obj.ActiveStatus,
                                FormId = obj.FormID,
                                CreatedBy = obj.UserID,
                                CreatedOn = System.DateTime.Now,
                                CreatedTerminal = obj.TerminalID
                            };
                            db.GtEbeagrs.Add(_agerange);
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };
                        }
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

        public async Task<DO_ReturnParameter> ActiveOrDeActiveAgeRange(bool status, int ageId)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEbeagr agerange = db.GtEbeagrs.Where(w => w.AgeRangeId == ageId).FirstOrDefault();
                        if (agerange == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0184", Message = string.Format(_localizer[name: "W0184"]) };
                        }

                        agerange.ActiveStatus = status;
                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        if (status == true)
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0003", Message = string.Format(_localizer[name: "S0003"]) };
                        else
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0004", Message = string.Format(_localizer[name: "S0004"]) };
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

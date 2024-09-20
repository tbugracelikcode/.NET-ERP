using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Models;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.CostManagement.CostPeriod.Validations;
using TsiErp.Business.Entities.CostManagement.StandartStationCostRecord.Validations;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.CostManagement.CostPeriod;
using TsiErp.Entities.Entities.CostManagement.CostPeriod.Dtos;
using TsiErp.Entities.Entities.CostManagement.CostPeriodLine;
using TsiErp.Entities.Entities.CostManagement.CostPeriodLine.Dtos;
using TsiErp.Entities.Entities.CostManagement.StandartStationCostRecord.Dtos;
using TsiErp.Entities.Entities.CostManagement.StandartStationCostRecord;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.CostPeriods.Page;
using TSI.QueryBuilder.Constants.Join;
using TsiErp.Entities.Entities.SalesManagement.OrderAcceptanceRecordLine.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StartingSalary.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StartingSalaryLine.Dtos;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.Entities.SalesManagement.OrderAcceptanceRecord.Dtos;
using TsiErp.Entities.Entities.FinanceManagement.PaymentPlan;
using TsiErp.Entities.Entities.SalesManagement.OrderAcceptanceRecord;
using TsiErp.Entities.Entities.SalesManagement.OrderAcceptanceRecordLine;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.Entities.StockManagement.ProductReferanceNumber;
using TsiErp.Entities.Entities.StockManagement.UnitSet;

namespace TsiErp.Business.Entities.CostManagement.CostPeriod.Services
{
    [ServiceRegistration(typeof(ICostPeriodsAppService), DependencyInjectionType.Scoped)]
    public class CostPeriodsAppService : ApplicationService<CostPeriodsResource>, ICostPeriodsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        public CostPeriodsAppService(IStringLocalizer<CostPeriodsResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
        }


        [ValidationAspect(typeof(CreateCostPeriodsValidator), Priority = 1)]
        public async Task<IDataResult<SelectCostPeriodsDto>> CreateAsync(CreateCostPeriodsDto input)
        {

            var listQuery = queryFactory.Query().From(Tables.CostPeriods).Select("Code").Where(new { Code = input.Code }, "");

            var list = queryFactory.ControlList<CostPeriods>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.CostPeriods).Insert(new CreateCostPeriodsDto
            {
                Code = input.Code,
                CreationTime = now,
                CreatorId = LoginedUserService.UserId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = Guid.Empty,
                DeletionTime = null,
                Id = addedEntityId,
                IsDeleted = false,
                LastModificationTime = null,
                LastModifierId = Guid.Empty,
                StartDate = now.Date,
                EndDate = input.CalculationEndDate,
                AnnualDistributionExpenseRate = input.AnnualDistributionExpenseRate,
                AnnualGeneralManagementExpenses = input.AnnualGeneralManagementExpenses,
                AnnualProductionExpenses = input.AnnualProductionExpenses,
                AnnualTurnover = input.AnnualTurnover,
                AnnualWorkingHours = input.AnnualWorkingHours,
                AnnualWorkingDay = input.AnnualWorkingDay,
                CalculationEndDate = input.CalculationEndDate,
                CalculationStartDate = input.CalculationStartDate,
                DailyGrossWorkingHours = input.DailyGrossWorkingHours,
                ElectricityAndWaterExpense = input.ElectricityAndWaterExpense,
                EmployeeSocialAssistanceExpenses = input.EmployeeSocialAssistanceExpenses,
                EURExchangeRate = input.EURExchangeRate,
                FinancialExpense = input.FinancialExpense,
                FinishedProductStorageExpenses = input.FinishedProductStorageExpenses,
                FoodExpense = input.FoodExpense,
                GeneralManagementExpenses = input.GeneralManagementExpenses,
                MarketingExpense = input.MarketingExpense,
                MaterialPurchaseTotal = input.MaterialPurchaseTotal,
                MonthlyRentAndDues = input.MonthlyRentAndDues,
                MontlyShiftWorkingHour = input.MontlyShiftWorkingHour,
                ServiceExpense = input.ServiceExpense,
                SGA = input.SGA,
                Shift = input.Shift,
                ShiftWorkingTime = input.ShiftWorkingTime,
                TotalProductionQuantity = input.TotalProductionQuantity,
                TotalRentedArea = input.TotalRentedArea,
                VariousDirectMaterials = input.VariousDirectMaterials,
                VariousIndirectMaterials = input.VariousIndirectMaterials,
                WorkClothesExpenses = input.WorkClothesExpenses,
                WorkplaceInsurance = input.WorkplaceInsurance,
                 Description_ = input.Description_
            });

            foreach (var item in input.SelectCostPeriodLines)
            {
                var queryLine = queryFactory.Query().From(Tables.CostPeriodLines).Insert(new CreateCostPeriodLinesDto
                {
                    CreationTime = now,
                    CreatorId = LoginedUserService.UserId,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = Guid.Empty,
                    DeletionTime = null,
                    Id = GuidGenerator.CreateGuid(),
                    IsDeleted = false,
                    LastModificationTime = null,
                    LastModifierId = Guid.Empty,
                     LineNr = item.LineNr,
                      CostPeriodID = addedEntityId,
                       Amount = item.Amount,
                        Title_ = item.Title_
                      
                });

                query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
            }

            var CostPeriods = queryFactory.Insert<SelectCostPeriodsDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("CostPeriodsChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.CostPeriods, LogType.Insert, addedEntityId);


            await Task.CompletedTask;
            return new SuccessDataResult<SelectCostPeriodsDto>(CostPeriods);
        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            var entity = (await GetAsync(id)).Data;
            var query = queryFactory.Query().From(Tables.CostPeriods).Select("*").Where(new { Id = id }, "");

            var CostPeriods = queryFactory.Get<SelectCostPeriodsDto>(query);

            if (CostPeriods.Id != Guid.Empty && CostPeriods != null)
            {
                var deleteQuery = queryFactory.Query().From(Tables.CostPeriods).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

                var lineDeleteQuery = queryFactory.Query().From(Tables.CostPeriodLines).Delete(LoginedUserService.UserId).Where(new { CostPeriodID = id }, "");

                deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                var CostPeriod = queryFactory.Update<SelectCostPeriodsDto>(deleteQuery, "Id", true);
                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.CostPeriods, LogType.Delete, id);

                await Task.CompletedTask;
                return new SuccessDataResult<SelectCostPeriodsDto>(CostPeriod);
            }
            else
            {
                var queryLine = queryFactory.Query().From(Tables.CostPeriodLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");
                var CostPeriodLines = queryFactory.Update<SelectCostPeriodLinesDto>(queryLine, "Id", true);
                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.CostPeriodLines, LogType.Delete, id);
                await Task.CompletedTask;
                return new SuccessDataResult<SelectCostPeriodLinesDto>(CostPeriodLines);
            }
        }

        public async Task<IDataResult<SelectCostPeriodsDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                    .Query().From(Tables.CostPeriods).Select("*")
                        .Where(new { Id = id }, "");

            var CostPeriods = queryFactory.Get<SelectCostPeriodsDto>(query);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.CostPeriodLines)
                   .Select("*")
                    .Where(new { CostPeriodID = id }, "");

            var CostPeriodLine = queryFactory.GetList<SelectCostPeriodLinesDto>(queryLines).ToList();

            CostPeriods.SelectCostPeriodLines = CostPeriodLine;

            LogsAppService.InsertLogToDatabase(CostPeriods, CostPeriods, LoginedUserService.UserId, Tables.CostPeriods, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectCostPeriodsDto>(CostPeriods);
        }

        public async Task<IDataResult<IList<ListCostPeriodsDto>>> GetListAsync(ListCostPeriodsParameterDto input)
        {
            var query = queryFactory
               .Query()
               .From(Tables.CostPeriods).Select("*")
                        .Where(null, Tables.CostPeriods);

            var CostPeriods = queryFactory.GetList<ListCostPeriodsDto>(query).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListCostPeriodsDto>>(CostPeriods);
        }

        [ValidationAspect(typeof(UpdateCostPeriodsValidator), Priority = 1)]
        public async Task<IDataResult<SelectCostPeriodsDto>> UpdateAsync(UpdateCostPeriodsDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.CostPeriods).Select<CostPeriods>(null).Where(new { Id = input.Id }, Tables.CostPeriods);
            var entity = queryFactory.Get<SelectCostPeriodsDto>(entityQuery);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.CostPeriodLines)
                   .Select<CostPeriodLines>(null)
                    .Where(new { CostPeriodID = input.Id }, Tables.CostPeriodLines);

            var CostPeriodLine = queryFactory.GetList<SelectCostPeriodLinesDto>(queryLines).ToList();

            entity.SelectCostPeriodLines = CostPeriodLine;

            #region Update Control

            var listQuery = queryFactory.Query().From(Tables.CostPeriods).Select("*").Where(new { Code = input.Code }, "");
            var list = queryFactory.GetList<CostPeriods>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }

            #endregion

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.CostPeriods).Update(new UpdateCostPeriodsDto
            {
                Id = input.Id,
                CreationTime = entity.CreationTime,
                CreatorId = entity.CreatorId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                IsDeleted = entity.IsDeleted,
                LastModificationTime = now,
                LastModifierId = LoginedUserService.UserId,
                Code = input.Code,
                StartDate = input.StartDate,
                EndDate = input.CalculationEndDate,
                 Shift = input.Shift,
                AnnualDistributionExpenseRate = input.AnnualDistributionExpenseRate,
                AnnualGeneralManagementExpenses = input.AnnualGeneralManagementExpenses,
                AnnualProductionExpenses = input.AnnualProductionExpenses,
                AnnualTurnover = input.AnnualTurnover,
                AnnualWorkingHours = input.AnnualWorkingHours,
                AnnualWorkingDay = input.AnnualWorkingDay,
                CalculationEndDate = input.CalculationEndDate,
                CalculationStartDate = input.CalculationStartDate,
                DailyGrossWorkingHours = input.DailyGrossWorkingHours,
                ElectricityAndWaterExpense = input.ElectricityAndWaterExpense,
                EmployeeSocialAssistanceExpenses = input.EmployeeSocialAssistanceExpenses,
                EURExchangeRate = input.EURExchangeRate,
                FinancialExpense = input.FinancialExpense,
                FinishedProductStorageExpenses = input.FinishedProductStorageExpenses,
                FoodExpense = input.FoodExpense,
                GeneralManagementExpenses = input.GeneralManagementExpenses,
                MarketingExpense = input.MarketingExpense,
                MaterialPurchaseTotal = input.MaterialPurchaseTotal,
                MonthlyRentAndDues = input.MonthlyRentAndDues,
                MontlyShiftWorkingHour = input.MontlyShiftWorkingHour,
                ServiceExpense = input.ServiceExpense,
                SGA = input.SGA,
                ShiftWorkingTime = input.ShiftWorkingTime,
                TotalProductionQuantity = input.TotalProductionQuantity,
                TotalRentedArea = input.TotalRentedArea,
                VariousDirectMaterials = input.VariousDirectMaterials,
                VariousIndirectMaterials = input.VariousIndirectMaterials,
                WorkClothesExpenses = input.WorkClothesExpenses,
                WorkplaceInsurance = input.WorkplaceInsurance,
                 Description_ = input.Description_
            }).Where(new { Id = input.Id }, "");

            foreach (var item in input.SelectCostPeriodLines)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.CostPeriodLines).Insert(new CreateCostPeriodLinesDto
                    {
                        CreationTime = now,
                        CreatorId = LoginedUserService.UserId,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = Guid.Empty,
                        DeletionTime = null,
                        Id = GuidGenerator.CreateGuid(),
                        IsDeleted = false,
                        LastModificationTime = null,
                        LastModifierId = Guid.Empty,
                         Title_ = item.Title_,
                          Amount = item.Amount,
                           CostPeriodID = item.CostPeriodID,
                            LineNr = item.LineNr,
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.CostPeriodLines).Select("*").Where(new { Id = item.Id }, "");

                    var line = queryFactory.Get<SelectCostPeriodLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.CostPeriodLines).Update(new UpdateCostPeriodLinesDto
                        {
                            CreationTime = line.CreationTime,
                            CreatorId = line.CreatorId,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = line.DeleterId.GetValueOrDefault(),
                            DeletionTime = line.DeletionTime.GetValueOrDefault(),
                            Id = item.Id,
                            IsDeleted = item.IsDeleted,
                            LastModificationTime = now,
                            LastModifierId = LoginedUserService.UserId,
                            Title_ = item.Title_,
                            Amount = item.Amount,
                            CostPeriodID = item.CostPeriodID,
                            LineNr = item.LineNr,
                        }).Where(new { Id = line.Id }, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }

            var CostPeriods = queryFactory.Update<SelectCostPeriodsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, CostPeriods, LoginedUserService.UserId, Tables.CostPeriods, LogType.Update, entity.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectCostPeriodsDto>(CostPeriods);

        }

        public async Task<IDataResult<SelectCostPeriodsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.CostPeriods).Select("*").Where(new { Id = id }, "");
            var entity = queryFactory.Get<CostPeriods>(entityQuery);

            var query = queryFactory.Query().From(Tables.CostPeriods).Update(new UpdateCostPeriodsDto
            {
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                IsDeleted = entity.IsDeleted,
                LastModificationTime = entity.LastModificationTime.GetValueOrDefault(),
                LastModifierId = entity.LastModifierId.GetValueOrDefault(),
                Id = id,
                DataOpenStatus = lockRow,
                DataOpenStatusUserId = userId,
                Code = entity.Code,
                EndDate = entity.EndDate,
                StartDate = entity.StartDate,
                AnnualDistributionExpenseRate = entity.AnnualDistributionExpenseRate,
                AnnualGeneralManagementExpenses = entity.AnnualGeneralManagementExpenses,
                AnnualProductionExpenses = entity.AnnualProductionExpenses,
                AnnualTurnover = entity.AnnualTurnover,
                AnnualWorkingHours = entity.AnnualWorkingHours,
                AnnualWorkingDay = entity.AnnualWorkingDay,
                CalculationEndDate = entity.CalculationEndDate,
                CalculationStartDate = entity.CalculationStartDate,
                DailyGrossWorkingHours = entity.DailyGrossWorkingHours,
                ElectricityAndWaterExpense = entity.ElectricityAndWaterExpense,
                EmployeeSocialAssistanceExpenses = entity.EmployeeSocialAssistanceExpenses,
                EURExchangeRate = entity.EURExchangeRate,
                FinancialExpense = entity.FinancialExpense,
                FinishedProductStorageExpenses = entity.FinishedProductStorageExpenses,
                FoodExpense = entity.FoodExpense,
                GeneralManagementExpenses = entity.GeneralManagementExpenses,
                MarketingExpense = entity.MarketingExpense,
                MaterialPurchaseTotal = entity.MaterialPurchaseTotal,
                MonthlyRentAndDues = entity.MonthlyRentAndDues,
                MontlyShiftWorkingHour = entity.MontlyShiftWorkingHour,
                ServiceExpense = entity.ServiceExpense,
                SGA = entity.SGA,
                Shift = entity.Shift,
                ShiftWorkingTime = entity.ShiftWorkingTime,
                TotalProductionQuantity = entity.TotalProductionQuantity,
                TotalRentedArea = entity.TotalRentedArea,
                VariousDirectMaterials = entity.VariousDirectMaterials,
                VariousIndirectMaterials = entity.VariousIndirectMaterials,
                WorkClothesExpenses = entity.WorkClothesExpenses,
                WorkplaceInsurance = entity.WorkplaceInsurance,
                Description_ = entity.Description_ 
            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id }, "");

            var CostPeriods = queryFactory.Update<SelectCostPeriodsDto>(query, "Id", true);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectCostPeriodsDto>(CostPeriods);


        }

    }
}

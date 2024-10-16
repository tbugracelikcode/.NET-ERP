using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TSI.QueryBuilder.Models;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.CostManagement.CPR.Validations;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.CostManagement.CPR;
using TsiErp.Entities.Entities.CostManagement.CPR.Dtos;
using TsiErp.Entities.Entities.CostManagement.CPRManufacturingCostLine;
using TsiErp.Entities.Entities.CostManagement.CPRManufacturingCostLine.Dtos;
using TsiErp.Entities.Entities.CostManagement.CPRMaterialCostLine;
using TsiErp.Entities.Entities.CostManagement.CPRMaterialCostLine.Dtos;
using TsiErp.Entities.Entities.CostManagement.CPRSetupCostLine;
using TsiErp.Entities.Entities.CostManagement.CPRSetupCostLine.Dtos;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Currency;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station;
using TsiErp.Entities.Entities.ProductionManagement.ProductsOperation;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.CPRs.Page;

namespace TsiErp.Business.Entities.CostManagement.CPR.Services
{
    [ServiceRegistration(typeof(ICPRsAppService), DependencyInjectionType.Scoped)]
    public class CPRsAppService : ApplicationService<CPRsResource>, ICPRsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        public CPRsAppService(IStringLocalizer<CPRsResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
        }

        [ValidationAspect(typeof(CreateCPRsValidator), Priority = 1)]
        public async Task<IDataResult<SelectCPRsDto>> CreateAsync(CreateCPRsDto input)
        {

            var listQuery = queryFactory.Query().From(Tables.CPRs).Select("Code").Where(new { Code = input.Code }, "");

            var list = queryFactory.ControlList<CPRs>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.CPRs).Insert(new CreateCPRsDto
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
                CurrencyID = input.CurrencyID.GetValueOrDefault(),
                Date_ = input.Date_,
                Incoterms = input.Incoterms,
                ManufacturingLocation = input.ManufacturingLocation,
                PartNo = input.PartNo,
                SubtotalManufacturingCost = input.SelectCPRManufacturingCostLines.Sum(t => t.ManufacuringStepCost),
                SubtotalManufacturingScrapCost = input.SelectCPRManufacturingCostLines.Sum(t => t.ScrapCost),
                SubtotalMaterialCost = input.SelectCPRMaterialCostLines.Sum(t => t.MaterialCost),
                SubtotalMaterialScrapCost = input.SelectCPRMaterialCostLines.Sum(T => T.ScrapCost),
                SubtotalSetupCost = input.SelectCPRSetupCostLines.Sum(T => T.SetupCost),
                PeakVolume = input.PeakVolume,
                PriceReductionSteps = input.PriceReductionSteps,
                ProductID = input.ProductID.GetValueOrDefault(),
                ProductionHoursperYear = input.ProductionHoursperYear,
                Quantity = input.Quantity,
                RecieverContact = input.RecieverContact,
                RecieverID = input.RecieverID.GetValueOrDefault(),
                SupplierContact = input.SupplierContact,
                SupplierID = input.SupplierID.GetValueOrDefault(),

            });

            foreach (var item in input.SelectCPRManufacturingCostLines)
            {
                var queryCostLine = queryFactory.Query().From(Tables.CPRManufacturingCostLines).Insert(new CreateCPRManufacturingCostLinesDto
                {
                    CPRID = addedEntityId,
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
                    StationID = item.StationID.GetValueOrDefault(),
                    ProductID = item.ProductID.GetValueOrDefault(),
                    DirectLaborHourlyRate = item.DirectLaborHourlyRate,
                    HeadCountatWorkingSystem = item.HeadCountatWorkingSystem,
                    LaborCostperPart = item.LaborCostperPart,
                    LineNr = item.LineNr,
                    ManufacturingSteps = (int)item.ManufacturingSteps,
                    ManufacuringStepCost = item.ManufacuringStepCost,
                    Material_ = item.Material_,
                    NetOutputDVOEE = item.NetOutputDVOEE,
                    ContractProduction = item.ContractProduction,
                    ContractUnitCost = item.ContractUnitCost,
                    IncludingOEE = item.IncludingOEE,
                    PartsperCycle = item.PartsperCycle,
                    ProductsOperationID = item.ProductsOperationID.GetValueOrDefault(),
                    ResidualManufacturingOverhead = item.ResidualManufacturingOverhead,
                    ScrapCost = item.ScrapCost,
                    ScrapRate = item.ScrapRate,
                    WorkingSystemCostperPart = item.WorkingSystemCostperPart,
                    WorkingSystemHourlyRate = item.WorkingSystemHourlyRate,
                    WorkingSystemInvest = item.WorkingSystemInvest
                });

                query.Sql = query.Sql + QueryConstants.QueryConstant + queryCostLine.Sql;
            }

            foreach (var item in input.SelectCPRMaterialCostLines)
            {
                var queryCostLine = queryFactory.Query().From(Tables.CPRMaterialCostLines).Insert(new CreateCPRMaterialCostLinesDto
                {
                    CPRID = addedEntityId,
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
                    ScrapCost = item.ScrapCost,
                    ScrapRate = item.ScrapRate,
                    BaseMaterialPrice = item.BaseMaterialPrice,
                    GrossWeightperPart = item.GrossWeightperPart,
                    MaterialCost = item.MaterialCost,
                    MaterialOverhead = item.MaterialOverhead,
                    NetWeightperPart = item.NetWeightperPart,
                    ProductID = item.ProductID.GetValueOrDefault(),
                    Quantity = item.Quantity,
                    Reimbursement = item.Reimbursement,
                    SurchargesMaterialPrice = item.SurchargesMaterialPrice,
                    UnitPrice = item.UnitPrice,
                });

                query.Sql = query.Sql + QueryConstants.QueryConstant + queryCostLine.Sql;
            }

            foreach (var item in input.SelectCPRSetupCostLines)
            {
                var queryCostLine = queryFactory.Query().From(Tables.CPRSetupCostLines).Insert(new CreateCPRSetupCostLinesDto
                {
                    CPRID = addedEntityId,
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
                    ManufacturingLotSize = item.ManufacturingLotSize,
                    ManufacturingSteps = (int)item.ManufacturingSteps,
                    ProductsOperationID = item.ProductsOperationID.GetValueOrDefault(),
                    ResidualManufacturingOverhead = item.ResidualManufacturingOverhead,
                    SetupCost = item.SetupCost,
                    SetupLaborHourlyRate = item.SetupLaborHourlyRate,
                    SetupTime = item.SetupTime,
                    UnitSetupCost = item.UnitSetupCost,
                    WorkingSystemHourlyRate = item.WorkingSystemHourlyRate,
                });

                query.Sql = query.Sql + QueryConstants.QueryConstant + queryCostLine.Sql;
            }

            var CPRs = queryFactory.Insert<SelectCPRsDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("CPRsChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.CPRs, LogType.Insert, addedEntityId);


            await Task.CompletedTask;
            return new SuccessDataResult<SelectCPRsDto>(CPRs);
        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            var entity = (await GetAsync(id)).Data;
            var query = queryFactory.Query().From(Tables.CPRs).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

            var lineDeleteQuery1 = queryFactory.Query().From(Tables.CPRManufacturingCostLines).Delete(LoginedUserService.UserId).Where(new { CPRID = id }, "");

            query.Sql = query.Sql + QueryConstants.QueryConstant + lineDeleteQuery1.Sql + " where " + lineDeleteQuery1.WhereSentence;

            var lineDeleteQuery2 = queryFactory.Query().From(Tables.CPRMaterialCostLines).Delete(LoginedUserService.UserId).Where(new { CPRID = id }, "");

            query.Sql = query.Sql + QueryConstants.QueryConstant + lineDeleteQuery2.Sql + " where " + lineDeleteQuery2.WhereSentence;

            var lineDeleteQuery3 = queryFactory.Query().From(Tables.CPRSetupCostLines).Delete(LoginedUserService.UserId).Where(new { CPRID = id }, "");

            query.Sql = query.Sql + QueryConstants.QueryConstant + lineDeleteQuery3.Sql + " where " + lineDeleteQuery3.WhereSentence;

            var CPRs = queryFactory.Update<SelectCPRsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.CPRs, LogType.Delete, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectCPRsDto>(CPRs);
        }

        public async Task<IResult> DeleteMaterialCostAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.CPRMaterialCostLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

            var CPRMaterialCostLines = queryFactory.Update<SelectCPRMaterialCostLinesDto>(query, "Id", true);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectCPRMaterialCostLinesDto>(CPRMaterialCostLines);
        }

        public async Task<IResult> DeleteManufacturingCostAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.CPRManufacturingCostLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

            var CPRManufacturingCostLines = queryFactory.Update<SelectCPRManufacturingCostLinesDto>(query, "Id", true);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectCPRManufacturingCostLinesDto>(CPRManufacturingCostLines);
        }

        public async Task<IResult> DeleteSetupCostAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.CPRSetupCostLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

            var CPRSetupCostLines = queryFactory.Update<SelectCPRSetupCostLinesDto>(query, "Id", true);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectCPRSetupCostLinesDto>(CPRSetupCostLines);
        }

        public async Task<IDataResult<SelectCPRsDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                    .Query().From(Tables.CPRs).Select<CPRs>(null)
                     .Join<Products>
                        (
                            p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                            nameof(CPRs.ProductID),
                            nameof(Products.Id),
                            JoinType.Left
                        )
                        .Join<CurrentAccountCards>
                        (
                            p => new { RecieverID = p.Id, RecieverCode = p.Code, RecieverName = p.Name },
                            nameof(CPRs.RecieverID),
                            nameof(Stations.Id),
                            JoinType.Left
                        )
                         .Join<CurrentAccountCards>
                        (
                            p => new { SupplierID = p.Id, SupplierCode = p.Code, SupplierName = p.Name },
                            nameof(CPRs.SupplierID),
                            nameof(Stations.Id),
                            "Supplier",
                            JoinType.Left
                        )
                        .Join<Currencies>
                        (
                            p => new { CurrencyID = p.Id, CurrencyCode = p.Code },
                            nameof(CPRs.CurrencyID),
                            nameof(Currencies.Id),
                            JoinType.Left
                        )
                        .Where(new { Id = id }, Tables.CPRs);

            var CPR = queryFactory.Get<SelectCPRsDto>(query);

            #region Manufacturing Cost

            var queryManufacturingLines = queryFactory
                  .Query()
                  .From(Tables.CPRManufacturingCostLines)
                  .Select<CPRManufacturingCostLines>(null)
                  .Join<ProductsOperations>
                   (
                       s => new { ProductsOperationName = s.Name, ProductsOperationID = s.Id },
                       nameof(CPRManufacturingCostLines.ProductsOperationID),
                       nameof(ProductsOperations.Id),
                       JoinType.Left
                   )
                  .Join<Stations>
                   (
                       sh => new { StationCode = sh.Code, StationName = sh.Name, StationID = sh.Id },
                       nameof(CPRManufacturingCostLines.StationID),
                       nameof(Stations.Id),
                       JoinType.Left
                   )
                    .Join<Products>
                           (
                               p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                               nameof(CPRManufacturingCostLines.ProductID),
                               nameof(Products.Id),
                               JoinType.Left
                           )
            .Where(new { CPRID = id }, Tables.CPRManufacturingCostLines);

            var ManufacturingCostLines = queryFactory.GetList<SelectCPRManufacturingCostLinesDto>(queryManufacturingLines).ToList();

            CPR.SelectCPRManufacturingCostLines = ManufacturingCostLines;
            #endregion

            #region Material Cost
            var queryMaterialLines = queryFactory
                    .Query()
                    .From(Tables.CPRMaterialCostLines)
                    .Select<CPRMaterialCostLines>(null)
                    .Join<Products>
                           (
                               p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                               nameof(CPRMaterialCostLines.ProductID),
                               nameof(Products.Id),
                               JoinType.Left
                           )
              .Where(new { CPRID = id }, Tables.CPRMaterialCostLines);

            var MaterialCostLines = queryFactory.GetList<SelectCPRMaterialCostLinesDto>(queryMaterialLines).ToList();

            CPR.SelectCPRMaterialCostLines = MaterialCostLines;
            #endregion

            #region Setup Cost
            var querySetupLines = queryFactory
                 .Query()
                 .From(Tables.CPRSetupCostLines)
                 .Select<CPRSetupCostLines>(null)
                 .Join<ProductsOperations>
                      (
                          s => new { ProductsOperationName = s.Name, ProductsOperationID = s.Id },
                          nameof(CPRSetupCostLines.ProductsOperationID),
                          nameof(ProductsOperations.Id),
                          JoinType.Left
                      )
           .Where(new { CPRID = id }, Tables.CPRSetupCostLines);

            var SetupCostLines = queryFactory.GetList<SelectCPRSetupCostLinesDto>(querySetupLines).ToList();

            CPR.SelectCPRSetupCostLines = SetupCostLines;
            #endregion

            LogsAppService.InsertLogToDatabase(CPR, CPR, LoginedUserService.UserId, Tables.CPRs, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectCPRsDto>(CPR);
        }

        public async Task<IDataResult<IList<ListCPRsDto>>> GetListAsync(ListCPRsParameterDto input)
        {
            var query = queryFactory
               .Query().From(Tables.CPRs).Select<CPRs>(null)
                     .Join<Products>
                        (
                            p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                            nameof(CPRs.ProductID),
                            nameof(Products.Id),
                            JoinType.Left
                        )
                        .Join<CurrentAccountCards>
                        (
                            p => new { RecieverID = p.Id, RecieverCode = p.Code, RecieverName = p.Name },
                            nameof(CPRs.RecieverID),
                            nameof(Stations.Id),
                            JoinType.Left
                        )
                         .Join<CurrentAccountCards>
                        (
                            p => new { SupplierID = p.Id, SupplierCode = p.Code, SupplierName = p.Name },
                            nameof(CPRs.SupplierID),
                            nameof(Stations.Id),
                            "Supplier",
                            JoinType.Left
                        )
                        .Join<Currencies>
                        (
                            p => new { CurrencyID = p.Id, CurrencyCode = p.Code },
                            nameof(CPRs.CurrencyID),
                            nameof(Currencies.Id),
                            JoinType.Left
                        ).Where(null, Tables.CPRs);

            var CPR = queryFactory.GetList<ListCPRsDto>(query).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListCPRsDto>>(CPR);
        }

        [ValidationAspect(typeof(UpdateCPRsValidator), Priority = 1)]
        public async Task<IDataResult<SelectCPRsDto>> UpdateAsync(UpdateCPRsDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.CPRs).Select("*").Where(
          new
          {
              Id = input.Id
          }, "");
            var entity = queryFactory.Get<SelectCPRsDto>(entityQuery);

            #region Line Entities
            var queryManufacturingLines = queryFactory
                     .Query()
                     .From(Tables.CPRManufacturingCostLines)
                     .Select<CPRManufacturingCostLines>(null)
                     .Join<ProductsOperations>
                      (
                          s => new { ProductsOperationName = s.Name, ProductsOperationID = s.Id },
                          nameof(CPRManufacturingCostLines.ProductsOperationID),
                          nameof(ProductsOperations.Id),
                          JoinType.Left
                      )
                     .Join<Stations>
                      (
                          sh => new { StationCode = sh.Code, StationName = sh.Name, StationID = sh.Id },
                          nameof(CPRManufacturingCostLines.StationID),
                          nameof(Stations.Id),
                          JoinType.Left
                      )
                    .Join<Products>
                           (
                               p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                               nameof(CPRManufacturingCostLines.ProductID),
                               nameof(Products.Id),
                               JoinType.Left
                           )
               .Where(new { CPRID = input.Id }, Tables.CPRManufacturingCostLines);

            var ManufacturingCostLines = queryFactory.GetList<SelectCPRManufacturingCostLinesDto>(queryManufacturingLines).ToList();

            entity.SelectCPRManufacturingCostLines = ManufacturingCostLines;

            var queryMaterialLines = queryFactory
                   .Query()
                   .From(Tables.CPRMaterialCostLines)
                   .Select<CPRMaterialCostLines>(null)
                   .Join<Products>
                          (
                              p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                              nameof(CPRMaterialCostLines.ProductID),
                              nameof(Products.Id),
                              JoinType.Left
                          )
             .Where(new { CPRID = input.Id }, Tables.CPRMaterialCostLines);

            var MaterialCostLines = queryFactory.GetList<SelectCPRMaterialCostLinesDto>(queryMaterialLines).ToList();

            entity.SelectCPRMaterialCostLines = MaterialCostLines;

            var querySetupLines = queryFactory
                 .Query()
                 .From(Tables.CPRSetupCostLines)
                 .Select<CPRSetupCostLines>(null)
                 .Join<ProductsOperations>
                      (
                          s => new { ProductsOperationName = s.Name, ProductsOperationID = s.Id },
                          nameof(CPRSetupCostLines.ProductsOperationID),
                          nameof(ProductsOperations.Id),
                          JoinType.Left
                      )
           .Where(new { CPRID = input.Id }, Tables.CPRSetupCostLines);

            var SetupCostLines = queryFactory.GetList<SelectCPRSetupCostLinesDto>(querySetupLines).ToList();

            entity.SelectCPRSetupCostLines = SetupCostLines;
            #endregion

            #region Update Control

            var listQuery = queryFactory.Query().From(Tables.CPRs).Select("*").Where(new { Code = input.Code }, "");
            var list = queryFactory.GetList<CPRs>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }

            #endregion

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.CPRs).Update(new UpdateCPRsDto
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
                SubtotalManufacturingCost = input.SelectCPRManufacturingCostLines.Sum(t => t.ManufacuringStepCost),
                SubtotalManufacturingScrapCost = input.SelectCPRManufacturingCostLines.Sum(t => t.ScrapCost),
                SubtotalMaterialCost = input.SelectCPRMaterialCostLines.Sum(t => t.MaterialCost),
                SubtotalMaterialScrapCost = input.SelectCPRMaterialCostLines.Sum(T => T.ScrapCost),
                SubtotalSetupCost = input.SelectCPRSetupCostLines.Sum(T => T.SetupCost),
                CurrencyID = input.CurrencyID.GetValueOrDefault(),
                Date_ = input.Date_,
                Incoterms = input.Incoterms,
                ManufacturingLocation = input.ManufacturingLocation,
                PartNo = input.PartNo,
                PeakVolume = input.PeakVolume,
                PriceReductionSteps = input.PriceReductionSteps,
                ProductID = input.ProductID.GetValueOrDefault(),
                ProductionHoursperYear = input.ProductionHoursperYear,
                Quantity = input.Quantity,
                RecieverContact = input.RecieverContact,
                RecieverID = input.RecieverID.GetValueOrDefault(),
                SupplierContact = input.SupplierContact,
                SupplierID = input.SupplierID.GetValueOrDefault(),
            }).Where(new { Id = input.Id }, "");

            foreach (var item in input.SelectCPRManufacturingCostLines)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.CPRManufacturingCostLines).Insert(new CreateCPRManufacturingCostLinesDto
                    {
                        CPRID = input.Id,
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
                        StationID = item.StationID.GetValueOrDefault(),
                        ProductID = item.ProductID.GetValueOrDefault(),
                        DirectLaborHourlyRate = item.DirectLaborHourlyRate,
                        HeadCountatWorkingSystem = item.HeadCountatWorkingSystem,
                        ContractProduction = item.ContractProduction,
                        ContractUnitCost = item.ContractUnitCost,
                        IncludingOEE = item.IncludingOEE,
                        LaborCostperPart = item.LaborCostperPart,
                        LineNr = item.LineNr,
                        ManufacturingSteps = (int)item.ManufacturingSteps,
                        ManufacuringStepCost = item.ManufacuringStepCost,
                        Material_ = item.Material_,
                        NetOutputDVOEE = item.NetOutputDVOEE,
                        PartsperCycle = item.PartsperCycle,
                        ProductsOperationID = item.ProductsOperationID.GetValueOrDefault(),
                        ResidualManufacturingOverhead = item.ResidualManufacturingOverhead,
                        ScrapCost = item.ScrapCost,
                        ScrapRate = item.ScrapRate,
                        WorkingSystemCostperPart = item.WorkingSystemCostperPart,
                        WorkingSystemHourlyRate = item.WorkingSystemHourlyRate,
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.CPRManufacturingCostLines).Select("*").Where(new { Id = item.Id }, "");

                    var line = queryFactory.Get<SelectCPRManufacturingCostLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.CPRManufacturingCostLines).Update(new UpdateCPRManufacturingCostLinesDto
                        {
                            CPRID = input.Id,
                            CreationTime = line.CreationTime,
                            CreatorId = line.CreatorId,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = line.DeleterId.GetValueOrDefault(),
                            DeletionTime = line.DeletionTime.GetValueOrDefault(),
                            ProductID = item.ProductID.GetValueOrDefault(),
                            Id = item.Id,
                            IsDeleted = item.IsDeleted,
                            LastModificationTime = now,
                            LastModifierId = LoginedUserService.UserId,
                            StationID = item.StationID.GetValueOrDefault(),
                            WorkingSystemCostperPart = item.WorkingSystemCostperPart,
                            DirectLaborHourlyRate = item.DirectLaborHourlyRate,
                            WorkingSystemHourlyRate = item.WorkingSystemHourlyRate,
                            ContractProduction = item.ContractProduction,
                            ContractUnitCost = item.ContractUnitCost,
                            IncludingOEE = item.IncludingOEE,
                            ScrapRate = item.ScrapRate,
                            ScrapCost = item.ScrapCost,
                            ResidualManufacturingOverhead = item.ResidualManufacturingOverhead,
                            HeadCountatWorkingSystem = item.HeadCountatWorkingSystem,
                            LaborCostperPart = item.LaborCostperPart,
                            LineNr = item.LineNr,
                            ManufacturingSteps = (int)item.ManufacturingSteps,
                            ManufacuringStepCost = item.ManufacuringStepCost,
                            Material_ = item.Material_,
                            NetOutputDVOEE = item.NetOutputDVOEE,
                            PartsperCycle = item.PartsperCycle,
                            ProductsOperationID = item.ProductsOperationID.GetValueOrDefault(),
                            WorkingSystemInvest = item.WorkingSystemInvest
                        }).Where(new { Id = line.Id }, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }

            foreach (var item in input.SelectCPRMaterialCostLines)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.CPRMaterialCostLines).Insert(new CreateCPRMaterialCostLinesDto
                    {
                        CPRID = input.Id,
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
                        BaseMaterialPrice = item.BaseMaterialPrice,
                        GrossWeightperPart = item.GrossWeightperPart,
                        LineNr = item.LineNr,
                        MaterialCost = item.MaterialCost,
                        MaterialOverhead = item.MaterialOverhead,
                        NetWeightperPart = item.NetWeightperPart,
                        ProductID = item.ProductID.GetValueOrDefault(),
                        Quantity = item.Quantity,
                        Reimbursement = item.Reimbursement,
                        ScrapCost = item.ScrapCost,
                        ScrapRate = item.ScrapRate,
                        SurchargesMaterialPrice = item.SurchargesMaterialPrice,
                        UnitPrice = item.UnitPrice,
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.CPRMaterialCostLines).Select("*").Where(new { Id = item.Id }, "");

                    var line = queryFactory.Get<SelectCPRMaterialCostLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.CPRMaterialCostLines).Update(new UpdateCPRMaterialCostLinesDto
                        {
                            CPRID = input.Id,
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
                            UnitPrice = item.UnitPrice,
                            SurchargesMaterialPrice = item.SurchargesMaterialPrice,
                            ScrapRate = item.ScrapRate,
                            ScrapCost = item.ScrapCost,
                            Reimbursement = item.Reimbursement,
                            Quantity = item.Quantity,
                            BaseMaterialPrice = item.BaseMaterialPrice,
                            GrossWeightperPart = item.GrossWeightperPart,
                            LineNr = item.LineNr,
                            MaterialCost = item.MaterialCost,
                            MaterialOverhead = item.MaterialOverhead,
                            NetWeightperPart = item.NetWeightperPart,
                            ProductID = item.ProductID.GetValueOrDefault(),
                        }).Where(new { Id = line.Id }, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }

            foreach (var item in input.SelectCPRSetupCostLines)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.CPRSetupCostLines).Insert(new CreateCPRSetupCostLinesDto
                    {
                        CPRID = input.Id,
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
                        ManufacturingLotSize = item.ManufacturingLotSize,
                        ManufacturingSteps = (int)item.ManufacturingSteps,
                        ProductsOperationID = item.ProductsOperationID.GetValueOrDefault(),
                        ResidualManufacturingOverhead = item.ResidualManufacturingOverhead,
                        SetupCost = item.SetupCost,
                        SetupLaborHourlyRate = item.SetupLaborHourlyRate,
                        SetupTime = item.SetupTime,
                        UnitSetupCost = item.UnitSetupCost,
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.CPRSetupCostLines).Select("*").Where(new { Id = item.Id }, "");

                    var line = queryFactory.Get<SelectCPRSetupCostLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.CPRSetupCostLines).Update(new UpdateCPRSetupCostLinesDto
                        {
                            CPRID = input.Id,
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
                            WorkingSystemHourlyRate = item.WorkingSystemHourlyRate,
                            UnitSetupCost = item.UnitSetupCost,
                            SetupTime = item.SetupTime,
                            SetupLaborHourlyRate = item.SetupLaborHourlyRate,
                            SetupCost = item.SetupCost,
                            ResidualManufacturingOverhead = item.ResidualManufacturingOverhead,
                            ProductsOperationID = item.ProductsOperationID.GetValueOrDefault(),
                            LineNr = item.LineNr,
                            ManufacturingLotSize = item.ManufacturingLotSize,
                            ManufacturingSteps = (int)item.ManufacturingSteps

                        }).Where(new { Id = line.Id }, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }

            var CPRs = queryFactory.Update<SelectCPRsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, CPRs, LoginedUserService.UserId, Tables.CPRs, LogType.Update, entity.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectCPRsDto>(CPRs);

        }

        public async Task<IDataResult<SelectCPRsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.CPRs).Select("*").Where(new { Id = id }, "");
            var entity = queryFactory.Get<CPRs>(entityQuery);

            var query = queryFactory.Query().From(Tables.CPRs).Update(new UpdateCPRsDto
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
                CurrencyID = entity.CurrencyID,
                Date_ = entity.Date_,
                Incoterms = (int)entity.Incoterms,
                ManufacturingLocation = entity.ManufacturingLocation,
                PartNo = entity.PartNo,
                PeakVolume = entity.PeakVolume,
                PriceReductionSteps = entity.PriceReductionSteps,
                ProductID = entity.ProductID,
                ProductionHoursperYear = entity.ProductionHoursperYear,
                Quantity = entity.Quantity,
                RecieverContact = entity.RecieverContact,
                RecieverID = entity.RecieverID,
                SupplierContact = entity.SupplierContact,
                SupplierID = entity.SupplierID,
            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id }, "");

            var CPRs = queryFactory.Update<SelectCPRsDto>(query, "Id", true);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectCPRsDto>(CPRs);


        }
    }
}

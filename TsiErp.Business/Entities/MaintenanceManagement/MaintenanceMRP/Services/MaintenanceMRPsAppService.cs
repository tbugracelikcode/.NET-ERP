using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.MaintenanceMRP.Validations;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station;
using TsiErp.Entities.Entities.MaintenanceManagement.MaintenanceMRP;
using TsiErp.Entities.Entities.MaintenanceManagement.MaintenanceMRP.Dtos;
using TsiErp.Entities.Entities.MaintenanceManagement.MaintenanceMRPLine;
using TsiErp.Entities.Entities.MaintenanceManagement.MaintenanceMRPLine.Dtos;
using TsiErp.Entities.Entities.MaintenanceManagement.MaintenancePeriod;
using TsiErp.Entities.Entities.Other.GrandTotalStockMovement;
using TsiErp.Entities.Entities.PlanningManagement.MRPLine.Dtos;
using TsiErp.Entities.Entities.PlanningManagement.MRPLine;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.Entities.StockManagement.UnitSet;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.MaintenanceMRPs.Page;
using TsiErp.Business.Extensions.DeleteControlExtension;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;

namespace TsiErp.Business.Entities.MaintenanceMRP.Services
{
    [ServiceRegistration(typeof(IMaintenanceMRPsAppService), DependencyInjectionType.Scoped)]
    public class MaintenanceMRPsAppService : ApplicationService<MaintenanceMRPsResource>, IMaintenanceMRPsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        public MaintenanceMRPsAppService(IStringLocalizer<MaintenanceMRPsResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
        }

        [ValidationAspect(typeof(CreateMaintenanceMRPsValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectMaintenanceMRPsDto>> CreateAsync(CreateMaintenanceMRPsDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.MaintenanceMRPs).Select("*").Where(new { Code = input.Code }, false, false, "");
            var list = queryFactory.ControlList<MaintenanceMRPs>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            var query = queryFactory.Query().From(Tables.MaintenanceMRPs).Insert(new CreateMaintenanceMRPsDto
            {
                Date_ = input.Date_,
                Description_ = input.Description_,
                FilterEndDate = input.FilterEndDate,
                FilterStartDate = input.FilterStartDate,
                IsMergeLines = input.IsMergeLines,
                TimeLeftforMaintenance = input.TimeLeftforMaintenance,
                Code = input.Code,
                CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
                CreatorId = LoginedUserService.UserId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = Guid.Empty,
                DeletionTime = null,
                Id = addedEntityId,
                IsDeleted = false,
                LastModificationTime = null,
                LastModifierId = Guid.Empty,
            });

            foreach (var item in input.SelectMaintenanceMRPLines)
            {
                var queryLine = queryFactory.Query().From(Tables.MaintenanceMRPLines).Insert(new CreateMaintenanceMRPLinesDto
                {
                    isStockUsage = item.isStockUsage,
                    RequirementAmount = item.RequirementAmount,
                    Amount = item.Amount,
                    MaintenanceMRPID = addedEntityId,
                    CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
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
                    ProductID = item.ProductID,
                    UnitSetID = item.UnitSetID,
                });

                query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
            }

            var maintenanceMRP = queryFactory.Insert<SelectMaintenanceMRPsDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("MainMatReqPlanningChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.MaintenanceMRPs, LogType.Insert, addedEntityId);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectMaintenanceMRPsDto>(maintenanceMRP);
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            DeleteControl.ControlList.Clear();


            DeleteControl.ControlList.Add("MaintenanceMRPID", new List<string>
            {
                Tables.PurchaseOrders,
                Tables.MRPs
            });

            bool control = DeleteControl.Control(queryFactory, id);

            if (!control)
            {
                throw new Exception(L["DeleteControlManager"]);
            }
            else
            {
                var query = queryFactory.Query().From(Tables.MaintenanceMRPs).Select("*").Where(new { Id = id }, false, false, "");

                var maintenanceMRPs = queryFactory.Get<SelectMaintenanceMRPsDto>(query);

                if (maintenanceMRPs.Id != Guid.Empty && maintenanceMRPs != null)
                {
                    var deleteQuery = queryFactory.Query().From(Tables.MaintenanceMRPs).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");

                    var lineDeleteQuery = queryFactory.Query().From(Tables.MaintenanceMRPLines).Delete(LoginedUserService.UserId).Where(new { MaintenanceMRPID = id }, false, false, "");

                    deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                    var maintenanceMRP = queryFactory.Update<SelectMaintenanceMRPsDto>(deleteQuery, "Id", true);
                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.MaintenanceMRPs, LogType.Delete, id);
                    await Task.CompletedTask;
                    return new SuccessDataResult<SelectMaintenanceMRPsDto>(maintenanceMRP);
                }
                else
                {
                    var queryLine = queryFactory.Query().From(Tables.MaintenanceMRPLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");
                    var maintenanceMRPLines = queryFactory.Update<SelectMaintenanceMRPLinesDto>(queryLine, "Id", true);
                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.MaintenanceMRPLines, LogType.Delete, id);
                    await Task.CompletedTask;
                    return new SuccessDataResult<SelectMaintenanceMRPLinesDto>(maintenanceMRPLines);
                }
            }
        }

        public async Task<IDataResult<SelectMaintenanceMRPsDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.MaintenanceMRPs)
                   .Select("*").Where(
            new
            {
                Id = id
            }, false, false, "");

            var maintenanceMRPs = queryFactory.Get<SelectMaintenanceMRPsDto>(query);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.MaintenanceMRPLines)

                   .Select<MaintenanceMRPLines, GrandTotalStockMovements>
              (
                  null
                , t => t.Amount
                , Tables.GrandTotalStockMovements
                , true
                , nameof(GrandTotalStockMovements.ProductID) + "=" + Tables.MaintenanceMRPLines + "." + nameof(MaintenanceMRPLines.ProductID))
                   .Join<Products>
                    (
                        p => new { ProductID = p.Code, ProductCode = p.Code, ProductName = p.Name },
                        nameof(MaintenanceMRPLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<UnitSets>
                    (
                        u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                        nameof(MaintenanceMRPLines.UnitSetID),
                        nameof(UnitSets.Id),
                        JoinType.Left
                    )
                    .Where(new { MaintenanceMRPID = id }, false, false, Tables.MaintenanceMRPLines);

            var maintenanceMRPLine = queryFactory.GetList<SelectMaintenanceMRPLinesDto>(queryLines).ToList();

            maintenanceMRPs.SelectMaintenanceMRPLines = maintenanceMRPLine;

            LogsAppService.InsertLogToDatabase(maintenanceMRPs, maintenanceMRPs, LoginedUserService.UserId, Tables.MaintenanceMRPs, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectMaintenanceMRPsDto>(maintenanceMRPs);
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListMaintenanceMRPsDto>>> GetListAsync(ListMaintenanceMRPsParameterDto input)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.MaintenanceMRPs).Select("*").Where(null, false, false, "");

            var maintenanceMRPs = queryFactory.GetList<ListMaintenanceMRPsDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListMaintenanceMRPsDto>>(maintenanceMRPs);
        }

        [ValidationAspect(typeof(UpdateMaintenanceMRPsValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectMaintenanceMRPsDto>> UpdateAsync(UpdateMaintenanceMRPsDto input)
        {
            var entityQuery = queryFactory
                   .Query()
                   .From(Tables.MaintenanceMRPs)
                   .Select("*").Where(
            new
            {
                Id = input.Id
            }, false, false, "");

            var entity = queryFactory.Get<SelectMaintenanceMRPsDto>(entityQuery);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.MaintenanceMRPLines)
                   .Select<MaintenanceMRPLines>(null)
                   .Join<Products>
                    (
                        p => new { ProductID = p.Code, ProductCode = p.Code, ProductName = p.Name },
                        nameof(MaintenanceMRPLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<UnitSets>
                    (
                        u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                        nameof(MaintenanceMRPLines.UnitSetID),
                        nameof(UnitSets.Id),
                        JoinType.Left
                    )
                    .Where(new { MaintenanceMRPID = input.Id }, false, false, Tables.MaintenanceMRPLines);

            var maintenanceMRPLine = queryFactory.GetList<SelectMaintenanceMRPLinesDto>(queryLines).ToList();

            entity.SelectMaintenanceMRPLines = maintenanceMRPLine;

            #region Update Control
            var listQuery = queryFactory
                           .Query()
                           .From(Tables.MaintenanceMRPs)
                           .Select("*").Where(null, false, false, "");

            var list = queryFactory.GetList<ListMaintenanceMRPsDto>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion

            var query = queryFactory.Query().From(Tables.MaintenanceMRPs).Update(new UpdateMaintenanceMRPsDto
            {
                Date_ = input.Date_,
                Description_ = input.Description_,
                FilterEndDate = input.FilterEndDate,
                FilterStartDate = input.FilterStartDate,
                IsMergeLines = input.IsMergeLines,
                TimeLeftforMaintenance = input.TimeLeftforMaintenance,
                Code = input.Code,
                CreationTime = entity.CreationTime,
                CreatorId = entity.CreatorId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                Id = input.Id,
                IsDeleted = entity.IsDeleted,
                LastModificationTime = _GetSQLDateAppService.GetDateFromSQL(),
                LastModifierId = LoginedUserService.UserId,
            }).Where(new { Id = input.Id }, false, false, "");

            foreach (var item in input.SelectMaintenanceMRPLines)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.MaintenanceMRPLines).Insert(new CreateMaintenanceMRPLinesDto
                    {
                        isStockUsage = item.isStockUsage,
                        RequirementAmount = item.RequirementAmount,
                        Amount = item.Amount,
                        DeletionTime = null,
                        MaintenanceMRPID = input.Id,
                        CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
                        CreatorId = LoginedUserService.UserId,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = Guid.Empty,
                        Id = GuidGenerator.CreateGuid(),
                        IsDeleted = false,
                        LastModificationTime = null,
                        LastModifierId = Guid.Empty,
                        LineNr = item.LineNr,
                        ProductID = item.ProductID,
                        UnitSetID = item.UnitSetID,
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.MaintenanceMRPLines).Select("*").Where(new { Id = item.Id }, false, false, "");

                    var line = queryFactory.Get<SelectMaintenanceMRPLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.MaintenanceMRPLines).Update(new UpdateMaintenanceMRPLinesDto
                        {
                            isStockUsage = item.isStockUsage,
                            RequirementAmount = item.RequirementAmount,
                            MaintenanceMRPID = input.Id,
                            Amount = item.Amount,
                            CreationTime = line.CreationTime,
                            CreatorId = line.CreatorId,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = line.DeleterId.GetValueOrDefault(),
                            DeletionTime = line.DeletionTime.GetValueOrDefault(),
                            Id = item.Id,
                            IsDeleted = item.IsDeleted,
                            LastModificationTime = _GetSQLDateAppService.GetDateFromSQL(),
                            LastModifierId = LoginedUserService.UserId,
                            LineNr = item.LineNr,
                            ProductID = item.ProductID,
                            UnitSetID = item.UnitSetID,
                        }).Where(new { Id = line.Id }, false, false, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }

            var maintenanceMRP = queryFactory.Update<SelectMaintenanceMRPsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.MaintenanceMRPs, LogType.Update, entity.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectMaintenanceMRPsDto>(maintenanceMRP);
        }

        public async Task<IDataResult<SelectMaintenanceMRPsDto>> GetbyPeriodStationAsync(Guid? stationID, Guid? periodID)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.MaintenanceMRPs)
                   .Select("*")
                   .Where(new { StationID = stationID, PeriodID = periodID }, false, false, "");

            var maintenanceMRPs = queryFactory.Get<SelectMaintenanceMRPsDto>(query);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.MaintenanceMRPLines)
                    .Select<MaintenanceMRPLines>(null)
                   .Join<Products>
                    (
                        p => new { ProductID = p.Code, ProductCode = p.Code, ProductName = p.Name },
                        nameof(MaintenanceMRPLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<UnitSets>
                    (
                        u => new { UnitSetID = u.Id, UnitSetCode = u.Code },
                        nameof(MaintenanceMRPLines.UnitSetID),
                        nameof(UnitSets.Id),
                        JoinType.Left
                    )
                    .Where(new { MaintenanceMRPID = maintenanceMRPs.Id }, false, false, Tables.MaintenanceMRPLines);

            var maintenanceMRPLine = queryFactory.GetList<SelectMaintenanceMRPLinesDto>(queryLines).ToList();

            maintenanceMRPs.SelectMaintenanceMRPLines = maintenanceMRPLine;

            LogsAppService.InsertLogToDatabase(maintenanceMRPs, maintenanceMRPs, LoginedUserService.UserId, Tables.MaintenanceMRPs, LogType.Get, maintenanceMRPs.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectMaintenanceMRPsDto>(maintenanceMRPs);
        }

        public async Task<IDataResult<SelectMaintenanceMRPsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.MaintenanceMRPs).Select("*").Where(new { Id = id }, false, false, "");

            var entity = queryFactory.Get<MaintenanceMRPs>(entityQuery);

            var query = queryFactory.Query().From(Tables.MaintenanceMRPs).Update(new UpdateMaintenanceMRPsDto
            {
                TimeLeftforMaintenance = entity.TimeLeftforMaintenance,
                IsMergeLines = entity.IsMergeLines,
                FilterStartDate = entity.FilterStartDate,
                FilterEndDate = entity.FilterEndDate,
                Date_ = entity.Date_,
                Description_ = entity.Description_,
                Code = entity.Code,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DataOpenStatus = lockRow,
                DataOpenStatusUserId = userId,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                Id = entity.Id,
                IsDeleted = entity.IsDeleted,
                LastModificationTime = entity.LastModificationTime.GetValueOrDefault(),
                LastModifierId = entity.LastModifierId.GetValueOrDefault(),
            }).Where(new { Id = id }, false, false, "");

            var maintenanceMRPsDto = queryFactory.Update<SelectMaintenanceMRPsDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectMaintenanceMRPsDto>(maintenanceMRPsDto);
        }
    }
}

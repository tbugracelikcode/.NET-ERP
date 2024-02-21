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
using TsiErp.Business.Entities.PlanningManagement.MRPII.Services;
using TsiErp.Business.Entities.PlanningManagement.MRPII.Validations;
using TsiErp.Business.Extensions.DeleteControlExtension;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch;
using TsiErp.Entities.Entities.Other.GrandTotalStockMovement;
using TsiErp.Entities.Entities.PlanningManagement.MRPII;
using TsiErp.Entities.Entities.PlanningManagement.MRPII.Dtos;
using TsiErp.Entities.Entities.PlanningManagement.MRPIILine;
using TsiErp.Entities.Entities.PlanningManagement.MRPIILine.Dtos;
using TsiErp.Entities.Entities.SalesManagement.OrderAcceptanceRecord;
using TsiErp.Entities.Entities.SalesManagement.SalesOrder;
using TsiErp.Entities.Entities.SalesManagement.SalesOrderLine;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.Entities.StockManagement.UnitSet;
using TsiErp.Entities.Entities.StockManagement.WareHouse;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.MRPIIs.Page;

namespace TsiErp.Business.Entities.MRPII.Services
{
    [ServiceRegistration(typeof(IMRPIIsAppService), DependencyInjectionType.Scoped)]
    public class MRPIIsAppService : ApplicationService<MRPIIsResource>, IMRPIIsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }

        public MRPIIsAppService(IStringLocalizer<MRPIIsResource> l, IFicheNumbersAppService ficheNumbersAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
        }



        [ValidationAspect(typeof(CreateMRPIIsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectMRPIIsDto>> CreateAsync(CreateMRPIIsDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.MRPIIs).Select("*").Where(new { Code = input.Code }, false, false, "");
            var list = queryFactory.ControlList<MRPIIs>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            var query = queryFactory.Query().From(Tables.MRPIIs).Insert(new CreateMRPIIsDto
            {
                Description_ = input.Description_,
                CalculationDate = input.CalculationDate,
                Code = input.Code,
                CreationTime = DateTime.Now,
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

            foreach (var item in input.SelectMRPIILines)
            {
                var queryLine = queryFactory.Query().From(Tables.MRPIILines).Insert(new CreateMRPIILinesDto
                {
                    EstimatedProductionEndDate = item.EstimatedProductionEndDate,
                    EstimatedProductionStartDate = item.EstimatedProductionStartDate,
                    EstimatedPurchaseSupplyDate = item.EstimatedPurchaseSupplyDate,
                    LinkedProductID = item.LinkedProductID,
                    OrderAcceptanceID = item.OrderAcceptanceID,
                    SalesOrderID = item.SalesOrderID,
                    LineNr = item.LineNr,
                    ProductID = item.ProductID,
                    MRPIIID = addedEntityId,
                    CreationTime = DateTime.Now,
                    CreatorId = LoginedUserService.UserId,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = Guid.Empty,
                    DeletionTime = null,
                    Id = GuidGenerator.CreateGuid(),
                    IsDeleted = false,
                    LastModificationTime = null,
                    LastModifierId = Guid.Empty,
                });

                query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
            }

            var MRPII = queryFactory.Insert<SelectMRPIIsDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("MRPIIChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.MRPIIs, LogType.Insert, addedEntityId);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectMRPIIsDto>(MRPII);

        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {

            var query = queryFactory.Query().From(Tables.MRPIIs).Select("*").Where(new { Id = id }, false, false, "");

            var MRPIIs = queryFactory.Get<SelectMRPIIsDto>(query);

            if (MRPIIs.Id != Guid.Empty && MRPIIs != null)
            {
                var deleteQuery = queryFactory.Query().From(Tables.MRPIIs).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");

                var lineDeleteQuery = queryFactory.Query().From(Tables.MRPIILines).Delete(LoginedUserService.UserId).Where(new { MRPIIID = id }, false, false, "");

                deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                var MRPII = queryFactory.Update<SelectMRPIIsDto>(deleteQuery, "Id", true);
                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.MRPIIs, LogType.Delete, id);
                await Task.CompletedTask;
                return new SuccessDataResult<SelectMRPIIsDto>(MRPII);
            }
            else
            {
                var queryLine = queryFactory.Query().From(Tables.MRPIILines).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");
                var MRPIILines = queryFactory.Update<SelectMRPIILinesDto>(queryLine, "Id", true);
                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.MRPIILines, LogType.Delete, id);
                await Task.CompletedTask;
                return new SuccessDataResult<SelectMRPIILinesDto>(MRPIILines);
            }

        }

        public async Task<IDataResult<SelectMRPIIsDto>> GetAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.MRPIIs).Select("*").Where(
            new
            {
                Id = id
            }, false, false, "");
            var MRPII = queryFactory.Get<SelectMRPIIsDto>(query);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.MRPIILines)
                   .Select<MRPIILines>(null)
                   .Join<Products>
                    (
                        s => new { ProductName = s.Name, ProductID = s.Id, ProductCode = s.Code },
                        nameof(MRPIILines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                    .Join<Products>
                    (
                        s => new { LinkedProductName = s.Name, LinkedProductID = s.Id, LinkedProductCode = s.Code },
                        nameof(MRPIILines.LinkedProductID),
                        nameof(Products.Id),
                         "LinkedProduct",
                        JoinType.Left
                    )
                    .Join<SalesOrders>
                    (
                        s => new { SalesOrderNo = s.FicheNo, SalesOrderID = s.Id },
                        nameof(MRPIILines.SalesOrderID),
                        nameof(SalesOrders.Id),
                        JoinType.Left
                    )
                    .Join<OrderAcceptanceRecords>
                    (
                        s => new { OrderAcceptanceNo = s.Code, OrderAcceptanceID = s.Id },
                        nameof(MRPIILines.OrderAcceptanceID),
                        nameof(OrderAcceptanceRecords.Id),
                        JoinType.Left
                    )

                    .Where(new { MRPIIID = id }, false, false, Tables.MRPIILines);

            var MRPIILine = queryFactory.GetList<SelectMRPIILinesDto>(queryLines).ToList();

            MRPII.SelectMRPIILines = MRPIILine;

            LogsAppService.InsertLogToDatabase(MRPII, MRPII, LoginedUserService.UserId, Tables.MRPIIs, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectMRPIIsDto>(MRPII);

        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListMRPIIsDto>>> GetListAsync(ListMRPIIsParameterDto input)
        {
            var query = queryFactory.Query().From(Tables.MRPIIs).Select("*").Where(null, false, false, "");
            var MRPIIs = queryFactory.GetList<ListMRPIIsDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListMRPIIsDto>>(MRPIIs);
        }

        [ValidationAspect(typeof(UpdateMRPIIsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectMRPIIsDto>> UpdateAsync(UpdateMRPIIsDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.MRPIIs).Select("*").Where(
            new
            {
                Id = input.Id
            }, false, false, "");
            var entity = queryFactory.Get<SelectMRPIIsDto>(entityQuery);

            var queryLines = queryFactory
                 .Query()
                 .From(Tables.MRPIILines)
                .Select<MRPIILines>(null)
                   .Join<Products>
                    (
                        s => new { ProductName = s.Name, ProductID = s.Id, ProductCode = s.Code },
                        nameof(MRPIILines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                    .Join<Products>
                    (
                        s => new { LinkedProductName = s.Name, LinkedProductID = s.Id, LinkedProductCode = s.Code },
                        nameof(MRPIILines.LinkedProductID),
                        nameof(Products.Id),
                         "LinkedProduct",
                        JoinType.Left
                    )
                      .Join<SalesOrders>
                    (
                        s => new { SalesOrderNo = s.FicheNo, SalesOrderID = s.Id },
                        nameof(MRPIILines.SalesOrderID),
                        nameof(SalesOrders.Id),
                        JoinType.Left
                    )
                    .Join<OrderAcceptanceRecords>
                    (
                        s => new { OrderAcceptanceNo = s.Code, OrderAcceptanceID = s.Id },
                        nameof(MRPIILines.OrderAcceptanceID),
                        nameof(OrderAcceptanceRecords.Id),
                        JoinType.Left
                    )
                  .Where(new { MRPIIID = input.Id }, false, false, Tables.MRPIILines);

            var MRPIILine = queryFactory.GetList<SelectMRPIILinesDto>(queryLines).ToList();

            entity.SelectMRPIILines = MRPIILine;

            #region Update Control
            var listQuery = queryFactory
                           .Query()
                           .From(Tables.MRPIIs).Select("*").Where(new { Code = input.Code }, false, false, Tables.MRPIIs);

            var list = queryFactory.GetList<ListMRPIIsDto>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion

            var query = queryFactory.Query().From(Tables.MRPIIs).Update(new UpdateMRPIIsDto
            {
                Description_ = input.Description_,
                CalculationDate = input.CalculationDate,
                Code = input.Code,
                CreationTime = entity.CreationTime,
                CreatorId = entity.CreatorId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                Id = input.Id,
                IsDeleted = entity.IsDeleted,
                LastModificationTime = DateTime.Now,
                LastModifierId = LoginedUserService.UserId,
            }).Where(new { Id = input.Id }, false, false, "");

            foreach (var item in input.SelectMRPIILines)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.MRPIILines).Insert(new CreateMRPIILinesDto
                    {
                        LineNr = item.LineNr,
                        LinkedProductID = item.LinkedProductID,
                        EstimatedPurchaseSupplyDate = item.EstimatedPurchaseSupplyDate,
                        EstimatedProductionStartDate = item.EstimatedProductionStartDate,
                        EstimatedProductionEndDate = item.EstimatedProductionEndDate,
                        OrderAcceptanceID = item.OrderAcceptanceID,
                        SalesOrderID = item.SalesOrderID,
                        ProductID = item.ProductID,
                        MRPIIID = input.Id,
                        CreationTime = DateTime.Now,
                        CreatorId = LoginedUserService.UserId,
                        DataOpenStatus = false,
                        DataOpenStatusUserId = Guid.Empty,
                        DeleterId = Guid.Empty,
                        DeletionTime = null,
                        Id = GuidGenerator.CreateGuid(),
                        IsDeleted = false,
                        LastModificationTime = null,
                        LastModifierId = Guid.Empty,
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.MRPIILines).Select("*").Where(new { Id = item.Id }, false, false, "");

                    var line = queryFactory.Get<SelectMRPIILinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.MRPIILines).Update(new UpdateMRPIILinesDto
                        {
                            LineNr = item.LineNr,
                            EstimatedProductionEndDate = item.EstimatedProductionEndDate,
                            EstimatedProductionStartDate = item.EstimatedProductionStartDate,
                            EstimatedPurchaseSupplyDate = item.EstimatedPurchaseSupplyDate,
                            LinkedProductID = item.LinkedProductID,
                            OrderAcceptanceID = item.OrderAcceptanceID,
                            SalesOrderID = item.SalesOrderID,
                            ProductID = item.ProductID,
                            MRPIIID = input.Id,
                            CreationTime = line.CreationTime,
                            CreatorId = line.CreatorId,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = line.DeleterId.GetValueOrDefault(),
                            DeletionTime = line.DeletionTime.GetValueOrDefault(),
                            Id = item.Id,
                            IsDeleted = item.IsDeleted,
                            LastModificationTime = DateTime.Now,
                            LastModifierId = LoginedUserService.UserId,
                        }).Where(new { Id = line.Id }, false, false, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }

            var MRPII = queryFactory.Update<SelectMRPIIsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.MRPIIs, LogType.Update, input.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectMRPIIsDto>(MRPII);

        }

        public async Task<IDataResult<SelectMRPIIsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.MRPIIs).Select("*").Where(new { Id = id }, false, false, "");

            var entity = queryFactory.Get<MRPIIs>(entityQuery);

            var query = queryFactory.Query().From(Tables.MRPIIs).Update(new UpdateMRPIIsDto
            {
                CalculationDate = entity.CalculationDate,
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

            var MRPIIsDto = queryFactory.Update<SelectMRPIIsDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectMRPIIsDto>(MRPIIsDto);


        }
    }
}

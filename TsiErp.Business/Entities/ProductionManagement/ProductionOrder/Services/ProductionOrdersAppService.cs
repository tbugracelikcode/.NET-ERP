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
using TsiErp.Business.Entities.ProductionOrder.Validations;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard;
using TsiErp.Entities.Entities.ProductionManagement.BillsofMaterial;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.Route;
using TsiErp.Entities.Entities.SalesManagement.SalesOrder;
using TsiErp.Entities.Entities.SalesManagement.SalesOrderLine;
using TsiErp.Entities.Entities.SalesManagement.SalesProposition;
using TsiErp.Entities.Entities.SalesManagement.SalesPropositionLine;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.Entities.StockManagement.UnitSet;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.ProductionOrders.Page;

namespace TsiErp.Business.Entities.ProductionOrder.Services
{
    [ServiceRegistration(typeof(IProductionOrdersAppService), DependencyInjectionType.Scoped)]
    public class ProductionOrdersAppService : ApplicationService<ProductionOrdersResource>, IProductionOrdersAppService
    {

        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private IFicheNumbersAppService FicheNumbersAppService { get; set; }

        public ProductionOrdersAppService(IStringLocalizer<ProductionOrdersResource> l, IFicheNumbersAppService ficheNumbersAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
        }


        [ValidationAspect(typeof(CreateProductionOrdersValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectProductionOrdersDto>> CreateAsync(CreateProductionOrdersDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var listQuery = queryFactory.Query().From(Tables.ProductionOrders).Select("*").Where(new { FicheNo = input.FicheNo }, false, false, "");

                var list = queryFactory.ControlList<ProductionOrders>(listQuery).ToList();

                #region Code Control 

                if (list.Count > 0)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["CodeControlManager"]);
                }

                #endregion

                Guid addedEntityId = GuidGenerator.CreateGuid();

                var query = queryFactory.Query().From(Tables.ProductionOrders).Insert(new CreateProductionOrdersDto
                {
                    BOMID = input.BOMID,
                    FicheNo = input.FicheNo,
                    CurrentAccountID = input.CurrentAccountID,
                    CustomerOrderNo = input.CustomerOrderNo,
                    EndDate = input.EndDate,
                    FinishedProductID = input.FinishedProductID,
                    LinkedProductID = input.LinkedProductID,
                    LinkedProductionOrderID = input.LinkedProductionOrderID,
                    OrderID = input.OrderID,
                    OrderLineID = input.OrderLineID,
                    PlannedQuantity = input.PlannedQuantity,
                    ProducedQuantity = input.ProducedQuantity,
                    ProductionOrderState = input.ProductionOrderState,
                    ProductTreeID = input.ProductTreeID,
                    ProductTreeLineID = input.ProductTreeLineID,
                    PropositionID = input.PropositionID,
                    PropositionLineID = input.PropositionLineID,
                    RouteID = input.RouteID,
                    StartDate = input.StartDate,
                    UnitSetID = input.UnitSetID,
                    Cancel_ = input.Cancel_,
                    Date_ = input.Date_,
                    Description_ = input.Description_,
                    Id = addedEntityId,
                    CreationTime = DateTime.Now,
                    CreatorId = LoginedUserService.UserId,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = Guid.Empty,
                    DeletionTime = null,
                    LastModificationTime = null,
                    LastModifierId = Guid.Empty,
                    IsDeleted = false
                });


                var productionOrders = queryFactory.Insert<SelectProductionOrdersDto>(query, "Id", true);

                await FicheNumbersAppService.UpdateFicheNumberAsync("ProductionOrdersChildMenu", input.FicheNo);

                LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.ProductionOrders, LogType.Insert, addedEntityId);


                return new SuccessDataResult<SelectProductionOrdersDto>(productionOrders);
            }
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory.Query().From(Tables.ProductionOrders).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");

                var productionOrders = queryFactory.Update<SelectProductionOrdersDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.ProductionOrders, LogType.Delete, id);

                return new SuccessDataResult<SelectProductionOrdersDto>(productionOrders);
            }

        }

        public async Task<IDataResult<SelectProductionOrdersDto>> GetAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory
                        .Query().From(Tables.ProductionOrders).Select<ProductionOrders>(po => new {po.PropositionID,po.BOMID,po.RouteID,po.PropositionLineID,po.OrderLineID,po.Cancel_,po.CurrentAccountID,po.CustomerOrderNo,po.DataOpenStatus,po.DataOpenStatusUserId,po.Date_,po.Description_,po.EndDate,po.FicheNo,po.FinishedProductID,po.Id,po.LinkedProductID,po.LinkedProductionOrderID,po.OrderID,po.PlannedQuantity,po.ProducedQuantity,po.ProductionOrderState,po.ProductTreeID,po.ProductTreeLineID,po.StartDate,po.UnitSetID})
                            .Join<SalesOrders>
                            (
                                so => new { OrderFicheNo = so.FicheNo, OrderID = so.Id },
                                nameof(ProductionOrders.OrderID),
                                nameof(SalesOrders.Id),
                                JoinType.Left
                            )
                            .Join<SalesOrderLines>
                            (
                                sol => new { OrderLineID = sol.Id},
                                nameof(ProductionOrders.OrderLineID),
                                nameof(SalesOrderLines.Id),
                                JoinType.Left
                            )
                             .Join<Products>
                            (
                                p => new { FinishedProductCode = p.Code , FinishedProductName = p.Name},
                                nameof(ProductionOrders.FinishedProductID),
                                nameof(Products.Id),
                                JoinType.Left
                            )
                             .Join<Products>
                            (
                                p => new { LinkedProductCode = p.Code, LinkedProductName = p.Name, LinkedProductID = p.Id },
                                nameof(ProductionOrders.LinkedProductID),
                                nameof(Products.Id),
                                JoinType.Left
                            )
                             .Join<UnitSets>
                            (
                                u => new { UnitSetCode = u.Code, UnitSetID = u.Id },
                                nameof(ProductionOrders.UnitSetID),
                                nameof(UnitSets.Id),
                                JoinType.Left
                            )
                             .Join<BillsofMaterials>
                            (
                                bom => new { BOMID = bom.Id, BOMCode = bom.Code, BOMName = bom.Name },
                                nameof(ProductionOrders.BOMID),
                                nameof(BillsofMaterials.Id),
                                JoinType.Left
                            )
                             .Join<Routes>
                            (
                                r => new { RouteID = r.Id, RouteCode = r.Code, RouteName = r.Name },
                                nameof(ProductionOrders.RouteID),
                                nameof(Routes.Id),
                                JoinType.Left
                            )
                             .Join<SalesPropositions>
                            (
                                sp => new { PropositionID = sp.Id, PropositionFicheNo = sp.FicheNo},
                                nameof(ProductionOrders.PropositionID),
                                nameof(SalesPropositions.Id),
                                JoinType.Left
                            )
                             .Join<SalesPropositionLines>
                            (
                                spl => new { PropositionLineID = spl.Id },
                                nameof(ProductionOrders.PropositionLineID),
                                nameof(SalesPropositionLines.Id),
                                JoinType.Left
                            )
                            .Join<CurrentAccountCards>
                            (
                                ca => new { CurrentAccountID = ca.Id, CurrentAccountCode = ca.Code, CurrentAccountName = ca.Name },
                                nameof(ProductionOrders.CurrentAccountID),
                                nameof(CurrentAccountCards.Id),
                                JoinType.Left
                            )
                            .Where(new { Id = id }, false, false, Tables.ProductionOrders);

                var productionOrder = queryFactory.Get<SelectProductionOrdersDto>(query);

                LogsAppService.InsertLogToDatabase(productionOrder, productionOrder, LoginedUserService.UserId, Tables.ProductionOrders, LogType.Get, id);

                return new SuccessDataResult<SelectProductionOrdersDto>(productionOrder);

            }

        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListProductionOrdersDto>>> GetListAsync(ListProductionOrdersParameterDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {

                var query = queryFactory
                   .Query()
                   .From(Tables.ProductionOrders).Select<ProductionOrders>(po => new { po.PropositionID, po.BOMID, po.RouteID, po.PropositionLineID, po.OrderLineID, po.Cancel_, po.CurrentAccountID, po.CustomerOrderNo, po.DataOpenStatus, po.DataOpenStatusUserId, po.Date_, po.Description_, po.EndDate, po.FicheNo, po.FinishedProductID, po.Id, po.LinkedProductID, po.LinkedProductionOrderID, po.OrderID, po.PlannedQuantity, po.ProducedQuantity, po.ProductionOrderState, po.ProductTreeID, po.ProductTreeLineID, po.StartDate, po.UnitSetID })
                            .Join<SalesOrders>
                            (
                                so => new { OrderFicheNo = so.FicheNo},
                                nameof(ProductionOrders.OrderID),
                                nameof(SalesOrders.Id),
                                JoinType.Left
                            )
                           
                             .Join<Products>
                            (
                                p => new { FinishedProductCode = p.Code, FinishedProductName = p.Name },
                                nameof(ProductionOrders.FinishedProductID),
                                nameof(Products.Id),
                                "FinishedProduct",
                                JoinType.Left
                            )
                             .Join<Products>
                            (
                                p => new { LinkedProductCode = p.Code, LinkedProductName = p.Name },
                                nameof(ProductionOrders.LinkedProductID),
                                nameof(Products.Id),
                                JoinType.Left
                            )
                             .Join<UnitSets>
                            (
                                u => new { UnitSetCode = u.Code },
                                nameof(ProductionOrders.UnitSetID),
                                nameof(UnitSets.Id),
                                JoinType.Left
                            )
                             .Join<BillsofMaterials>
                            (
                                bom => new {  BOMCode = bom.Code, BOMName = bom.Name },
                                nameof(ProductionOrders.BOMID),
                                nameof(BillsofMaterials.Id),
                                JoinType.Left
                            )
                             .Join<Routes>
                            (
                                r => new { RouteCode = r.Code, RouteName = r.Name },
                                nameof(ProductionOrders.RouteID),
                                nameof(Routes.Id),
                                JoinType.Left
                            )
                             .Join<SalesPropositions>
                            (
                                sp => new {  PropositionFicheNo = sp.FicheNo },
                                nameof(ProductionOrders.PropositionID),
                                nameof(SalesPropositions.Id),
                                JoinType.Left
                            )
                            
                            .Join<CurrentAccountCards>
                            (
                                ca => new { CurrentAccountCode = ca.Code, CurrentAccountName = ca.Name },
                                nameof(ProductionOrders.CurrentAccountID),
                                nameof(CurrentAccountCards.Id),
                                JoinType.Left
                            )
                       .Where(null, false, false, Tables.ProductionOrders);

                var productionOrders = queryFactory.GetList<ListProductionOrdersDto>(query).ToList();

                return new SuccessDataResult<IList<ListProductionOrdersDto>>(productionOrders);
            }
        }

        [ValidationAspect(typeof(UpdateProductionOrdersValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectProductionOrdersDto>> UpdateAsync(UpdateProductionOrdersDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.ProductionOrders).Select("*").Where(new { Id = input.Id }, false, false, "");
                var entity = queryFactory.Get<ProductionOrders>(entityQuery);

                #region Update Control

                var listQuery = queryFactory.Query().From(Tables.ProductionOrders).Select("*").Where(new { FicheNo = input.FicheNo }, false, false, "");
                var list = queryFactory.GetList<ProductionOrders>(listQuery).ToList();

                if (list.Count > 0 && entity.FicheNo != input.FicheNo)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["UpdateControlManager"]);
                }

                #endregion

                var query = queryFactory.Query().From(Tables.ProductionOrders).Update(new UpdateProductionOrdersDto
                {
                    BOMID = input.BOMID,
                    FicheNo = input.FicheNo,
                    CurrentAccountID = input.CurrentAccountID,
                    CustomerOrderNo = input.CustomerOrderNo,
                    EndDate = input.EndDate,
                    FinishedProductID = input.FinishedProductID,
                    LinkedProductID = input.LinkedProductID,
                    LinkedProductionOrderID = input.LinkedProductionOrderID,
                    OrderID = input.OrderID,
                    OrderLineID = input.OrderLineID,
                    PlannedQuantity = input.PlannedQuantity,
                    ProducedQuantity = input.ProducedQuantity,
                    ProductionOrderState = input.ProductionOrderState,
                    ProductTreeID = input.ProductTreeID,
                    ProductTreeLineID = input.ProductTreeLineID,
                    PropositionID = input.PropositionID,
                    PropositionLineID = input.PropositionLineID,
                    RouteID = input.RouteID,
                    StartDate = input.StartDate,
                    UnitSetID = input.UnitSetID,
                    Cancel_ = input.Cancel_,
                    Date_ = input.Date_,
                    Description_ = input.Description_,
                    Id = input.Id,
                    CreationTime = entity.CreationTime.Value,
                    CreatorId = entity.CreatorId.Value,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = entity.DeleterId.GetValueOrDefault(),
                    DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                    IsDeleted = entity.IsDeleted,
                    LastModificationTime = DateTime.Now,
                    LastModifierId = LoginedUserService.UserId
                }).Where(new { Id = input.Id }, false, false, "");

                var productionOrders = queryFactory.Update<SelectProductionOrdersDto>(query, "Id", true);


                LogsAppService.InsertLogToDatabase(entity, productionOrders, LoginedUserService.UserId, Tables.ProductionOrders, LogType.Update, entity.Id);


                return new SuccessDataResult<SelectProductionOrdersDto>(productionOrders);
            }
        }

        public async Task<IDataResult<SelectProductionOrdersDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.ProductionOrders).Select("*").Where(new { Id = id }, false, false, "");
                var entity = queryFactory.Get<ProductionOrders>(entityQuery);

                var query = queryFactory.Query().From(Tables.ProductionOrders).Update(new UpdateProductionOrdersDto
                {
                    BOMID = entity.BOMID,
                    FicheNo = entity.FicheNo,
                    CurrentAccountID = entity.CurrentAccountID,
                    CustomerOrderNo = entity.CustomerOrderNo,
                    EndDate = entity.EndDate,
                    FinishedProductID = entity.FinishedProductID,
                    LinkedProductID = entity.LinkedProductID,
                    LinkedProductionOrderID = entity.LinkedProductionOrderID,
                    OrderID = entity.OrderID,
                    OrderLineID = entity.OrderLineID,
                    PlannedQuantity = entity.PlannedQuantity,
                    ProducedQuantity = entity.ProducedQuantity,
                    ProductionOrderState = entity.ProductionOrderState,
                    ProductTreeID = entity.ProductTreeID,
                    ProductTreeLineID = entity.ProductTreeLineID,
                    PropositionID = entity.PropositionID,
                    PropositionLineID = entity.PropositionLineID,
                    RouteID = entity.RouteID,
                    StartDate = entity.StartDate,
                    UnitSetID = entity.UnitSetID,
                    Cancel_ = entity.Cancel_,
                    Date_ = entity.Date_,
                    Description_ = entity.Description_,
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

                }).Where(new { Id = id }, false, false, "");

                var productionOrders = queryFactory.Update<SelectProductionOrdersDto>(query, "Id", true);

                return new SuccessDataResult<SelectProductionOrdersDto>(productionOrders);

            }

        }
    }
}

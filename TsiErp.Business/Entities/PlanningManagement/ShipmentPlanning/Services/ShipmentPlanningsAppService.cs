﻿using AutoMapper.Internal.Mappers;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Localization;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TSI.QueryBuilder.Models;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.NotificationTemplate.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.Other.Notification.Services;
using TsiErp.Business.Entities.PlanningManagement.ShipmentPlanning.Services;
using TsiErp.Business.Entities.PlanningManagement.ShipmentPlanning.Validations;
using TsiErp.Business.Entities.ProductionOrder.Services;
using TsiErp.Business.Entities.SalesProposition.Services;
using TsiErp.Business.Extensions.DeleteControlExtension;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch;
using TsiErp.Entities.Entities.LeanProduction.OEEDetail;
using TsiErp.Entities.Entities.LeanProduction.OEEDetail.Dtos;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.Entities.PlanningManagement.ShipmentPlanning;
using TsiErp.Entities.Entities.PlanningManagement.ShipmentPlanning.Dtos;
using TsiErp.Entities.Entities.PlanningManagement.ShipmentPlanningLine;
using TsiErp.Entities.Entities.PlanningManagement.ShipmentPlanningLine.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesOrder;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.Entities.StockManagement.StockAddress.Dtos;
using TsiErp.Entities.Entities.StockManagement.StockFicheLine.Dtos;
using TsiErp.Entities.Entities.StockManagement.UnitSet.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.ShipmentPlannings.Page;

namespace TsiErp.Business.Entities.ShipmentPlanning.Services
{
    [ServiceRegistration(typeof(IShipmentPlanningsAppService), DependencyInjectionType.Scoped)]
    public class ShipmentPlanningsAppService : ApplicationService<ShipmentPlanningsResource>, IShipmentPlanningsAppService
    {
        private readonly IProductionOrdersAppService _productionOrdersAppService;
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;
        private readonly INotificationsAppService _NotificationsAppService;
        private readonly INotificationTemplatesAppService _NotificationTemplatesAppService;

        public ShipmentPlanningsAppService(IStringLocalizer<ShipmentPlanningsResource> l, IProductionOrdersAppService productionOrdersAppService, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService) : base(l)
        {
            _productionOrdersAppService = productionOrdersAppService;
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
            _NotificationsAppService = notificationsAppService;
            _NotificationTemplatesAppService = notificationTemplatesAppService;
        }

        public IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }

        [ValidationAspect(typeof(CreateShipmentPlanningsValidator), Priority = 1)]
        public async Task<IDataResult<SelectShipmentPlanningsDto>> CreateAsync(CreateShipmentPlanningsDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.ShipmentPlannings).Select("Code").Where(new { Code = input.Code }, "");
            var list = queryFactory.ControlList<ShipmentPlannings>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.ShipmentPlannings).Insert(new CreateShipmentPlanningsDto
            {
                PlannedAmount = input.PlannedAmount,
                ShipmentPlanningDate = input.ShipmentPlanningDate,
                PlannedLoadingTime = input.PlannedLoadingTime.GetValueOrDefault(),
                TotalAmount = input.TotalAmount,
                TotalGrossKG = input.TotalGrossKG,
                TotalNetKG = input.TotalNetKG,
                Description_ = input.Description_,
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
                ProductionDateReferenceID = input.ProductionDateReferenceID.GetValueOrDefault()

            });

            foreach (var item in input.SelectShipmentPlanningLines)
            {
                var queryLine = queryFactory.Query().From(Tables.ShipmentPlanningLines).Insert(new CreateShipmentPlanningLinesDto
                {
                    LineNr = item.LineNr,
                    ProductID = item.ProductID.GetValueOrDefault(),
                    GrossWeightKG = item.GrossWeightKG,
                    LineDescription_ = item.LineDescription_,
                    NetWeightKG = item.NetWeightKG,
                    PlannedEndDate = item.PlannedEndDate.GetValueOrDefault(),
                    PlannedStartDate = item.PlannedStartDate.GetValueOrDefault(),
                    PlannedQuantity = item.PlannedQuantity,
                    ProductionOrderID = item.ProductionOrderID.GetValueOrDefault(),
                    RequestedLoadingDate = item.RequestedLoadingDate,
                    SentQuantity = item.SentQuantity,
                    ShipmentQuantity = item.ShipmentQuantity,
                    UnitWeightKG = item.UnitWeightKG,
                    SalesOrderID = item.SalesOrderID.GetValueOrDefault(),
                    ShipmentPlanningID = addedEntityId,
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
                    ProductionDateReferenceID = item.ProductionDateReferenceID.GetValueOrDefault()
                });

                query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;

                #region Production Order Mamül-Yarı Mamül Update

                SelectProductionOrdersDto productionOrder = (await _productionOrdersAppService.GetAsync(item.ProductionOrderID.GetValueOrDefault())).Data;

                if (productionOrder != null && productionOrder.Id != Guid.Empty)
                {
                    productionOrder.ProductionDateReferenceID = input.ProductionDateReferenceID.GetValueOrDefault();
                    productionOrder.ShipmentDate = input.PlannedLoadingTime;


                    UpdateProductionOrdersDto updatedProdOrdInput = ObjectMapper.Map<SelectProductionOrdersDto, UpdateProductionOrdersDto>(productionOrder);

                    await _productionOrdersAppService.UpdateAsync(updatedProdOrdInput);


                    var orderList = (await _productionOrdersAppService.GetSelectListbyLinkedProductionOrder(productionOrder.Id)).Data;
                    foreach (var order in orderList)
                    {
                        order.ProductionDateReferenceID = input.ProductionDateReferenceID.GetValueOrDefault();
                        order.ShipmentDate = input.PlannedLoadingTime;

                        UpdateProductionOrdersDto updatedProdOrderInput = ObjectMapper.Map<SelectProductionOrdersDto, UpdateProductionOrdersDto>(order);

                        await _productionOrdersAppService.UpdateAsync(updatedProdOrderInput);
                    }

                }


                #endregion
            }




            var ShipmentPlanning = queryFactory.Insert<SelectShipmentPlanningsDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("ShipmentPlanningChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.ShipmentPlannings, LogType.Insert, addedEntityId);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["ShipmentPlanningChildMenu"], L["ProcessAdd"])).Data.FirstOrDefault();

            if (notTemplate != null && notTemplate.Id != Guid.Empty)
            {
                if (!string.IsNullOrEmpty(notTemplate.TargetUsersId))
                {
                    if (notTemplate.TargetUsersId.Contains("*Not*"))
                    {
                        string[] usersNot = notTemplate.TargetUsersId.Split("*Not*");

                        foreach (string user in usersNot)
                        {
                            CreateNotificationsDto createInput = new CreateNotificationsDto
                            {
                                ContextMenuName_ = notTemplate.ContextMenuName_,
                                IsViewed = false,

                                ModuleName_ = notTemplate.ModuleName_,
                                ProcessName_ = notTemplate.ProcessName_,
                                RecordNumber = input.Code,
                                NotificationDate = _GetSQLDateAppService.GetDateFromSQL(),
                                UserId = new Guid(user),
                                ViewDate = null,
                            };

                            await _NotificationsAppService.CreateAsync(createInput);
                        }
                    }
                    else
                    {
                        CreateNotificationsDto createInput = new CreateNotificationsDto
                        {
                            ContextMenuName_ = notTemplate.ContextMenuName_,
                            IsViewed = false,

                            ModuleName_ = notTemplate.ModuleName_,
                            ProcessName_ = notTemplate.ProcessName_,
                            RecordNumber = input.Code,
                            NotificationDate = _GetSQLDateAppService.GetDateFromSQL(),
                            UserId = new Guid(notTemplate.TargetUsersId),
                            ViewDate = null,
                        };

                        await _NotificationsAppService.CreateAsync(createInput);
                    }
                }

            }

            #endregion

            await Task.CompletedTask;
            return new SuccessDataResult<SelectShipmentPlanningsDto>(ShipmentPlanning);

        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            DeleteControl.ControlList.Clear();

            DeleteControl.ControlList.Add("ShipmentPlanningID", new List<string>
            {
                //Tables.PurchaseRequests
            });

            bool control = DeleteControl.Control(queryFactory, id);

            if (!control)
            {
                throw new Exception(L["DeleteControlManager"]);
            }
            else
            {
                var entity = (await GetAsync(id)).Data;
                var query = queryFactory.Query().From(Tables.ShipmentPlannings).Select("*").Where(new { Id = id }, "");

                var ShipmentPlannings = queryFactory.Get<SelectShipmentPlanningsDto>(query);

                if (ShipmentPlannings.Id != Guid.Empty && ShipmentPlannings != null)
                {

                    #region Production Order Mamül-Yarı Mamül Update

                    foreach (var item in entity.SelectShipmentPlanningLines)
                    {

                        SelectProductionOrdersDto productionOrder = (await _productionOrdersAppService.GetAsync(item.ProductionOrderID.GetValueOrDefault())).Data;
                        if (productionOrder != null && productionOrder.Id != Guid.Empty)
                        {
                            productionOrder.ProductionDateReferenceID = Guid.Empty;
                            productionOrder.ShipmentDate = null;


                            UpdateProductionOrdersDto updatedProdOrdInput = ObjectMapper.Map<SelectProductionOrdersDto, UpdateProductionOrdersDto>(productionOrder);

                            await _productionOrdersAppService.UpdateAsync(updatedProdOrdInput);

                            var orderList = (await _productionOrdersAppService.GetSelectListbyLinkedProductionOrder(productionOrder.Id)).Data;

                            foreach (var order in orderList)
                            {
                                order.ProductionDateReferenceID = Guid.Empty;
                                order.ShipmentDate = null;

                                UpdateProductionOrdersDto updatedProdOrderInput = ObjectMapper.Map<SelectProductionOrdersDto, UpdateProductionOrdersDto>(order);

                                await _productionOrdersAppService.UpdateAsync(updatedProdOrderInput);
                            }
                        }
                    }

                    #endregion


                    var deleteQuery = queryFactory.Query().From(Tables.ShipmentPlannings).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

                    var lineDeleteQuery = queryFactory.Query().From(Tables.ShipmentPlanningLines).Delete(LoginedUserService.UserId).Where(new { ShipmentPlanningID = id }, "");

                    deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                    var ShipmentPlanning = queryFactory.Update<SelectShipmentPlanningsDto>(deleteQuery, "Id", true);
                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.ShipmentPlannings, LogType.Delete, id);

                    #region Notification

                    var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["ShipmentPlanningChildMenu"], L["ProcessDelete"])).Data.FirstOrDefault();

                    if (notTemplate != null && notTemplate.Id != Guid.Empty)
                    {
                        if (!string.IsNullOrEmpty(notTemplate.TargetUsersId))
                        {
                            if (notTemplate.TargetUsersId.Contains("*Not*"))
                            {
                                string[] usersNot = notTemplate.TargetUsersId.Split("*Not*");

                                foreach (string user in usersNot)
                                {
                                    CreateNotificationsDto createInput = new CreateNotificationsDto
                                    {
                                        ContextMenuName_ = notTemplate.ContextMenuName_,
                                        IsViewed = false,

                                        ModuleName_ = notTemplate.ModuleName_,
                                        ProcessName_ = notTemplate.ProcessName_,
                                        RecordNumber = entity.Code,
                                        NotificationDate = _GetSQLDateAppService.GetDateFromSQL(),
                                        UserId = new Guid(user),
                                        ViewDate = null,
                                    };

                                    await _NotificationsAppService.CreateAsync(createInput);
                                }
                            }
                            else
                            {
                                CreateNotificationsDto createInput = new CreateNotificationsDto
                                {
                                    ContextMenuName_ = notTemplate.ContextMenuName_,
                                    IsViewed = false,

                                    ModuleName_ = notTemplate.ModuleName_,
                                    ProcessName_ = notTemplate.ProcessName_,
                                    RecordNumber = entity.Code,
                                    NotificationDate = _GetSQLDateAppService.GetDateFromSQL(),
                                    UserId = new Guid(notTemplate.TargetUsersId),
                                    ViewDate = null,
                                };

                                await _NotificationsAppService.CreateAsync(createInput);
                            }
                        }

                    }

                    #endregion


                    //var productionOrdersList = (await _productionOrdersAppService.GetListAsync(new ListProductionOrdersParameterDto())).Data.Where(t => t.Id == id).ToList();

                    //if (productionOrdersList != null && productionOrdersList.Count > 0)
                    //{
                    //    foreach (var productionOrders in productionOrdersList)
                    //    {
                    //        await _productionOrdersAppService.DeleteAsync(productionOrders.Id);
                    //    }
                    //}

                    await Task.CompletedTask;
                    return new SuccessDataResult<SelectShipmentPlanningsDto>(ShipmentPlanning);
                }
                else
                {

                    #region Production Order Mamül-Yarı Mamül Update

                    var ShipmentPlanning = (await GetLineAsync(id)).Data;

                    SelectProductionOrdersDto productionOrder = (await _productionOrdersAppService.GetAsync(ShipmentPlanning.ProductionOrderID.GetValueOrDefault())).Data;
                    if (productionOrder != null && productionOrder.Id != Guid.Empty)
                    {
                        productionOrder.ProductionDateReferenceID = Guid.Empty;
                        productionOrder.ShipmentDate = null;


                        UpdateProductionOrdersDto updatedProdOrdInput = ObjectMapper.Map<SelectProductionOrdersDto, UpdateProductionOrdersDto>(productionOrder);

                        await _productionOrdersAppService.UpdateAsync(updatedProdOrdInput);

                        var orderList = (await _productionOrdersAppService.GetSelectListbyLinkedProductionOrder(productionOrder.Id)).Data;

                        foreach (var order in orderList)
                        {
                            order.ProductionDateReferenceID = Guid.Empty;
                            order.ShipmentDate = null;

                            UpdateProductionOrdersDto updatedProdOrderInput = ObjectMapper.Map<SelectProductionOrdersDto, UpdateProductionOrdersDto>(order);

                            await _productionOrdersAppService.UpdateAsync(updatedProdOrderInput);
                        }
                    }
                    #endregion

                    var queryLine = queryFactory.Query().From(Tables.ShipmentPlanningLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

                    var ShipmentPlanningLines = queryFactory.Update<SelectShipmentPlanningLinesDto>(queryLine, "Id", true);
                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.ShipmentPlanningLines, LogType.Delete, id);
                    await Task.CompletedTask;
                    return new SuccessDataResult<SelectShipmentPlanningLinesDto>(ShipmentPlanningLines);

                }
            }
        }

        public async Task<IDataResult<SelectShipmentPlanningsDto>> GetAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.ShipmentPlannings).Select("*").Where(new { Id = id }, "");
            var ShipmentPlanning = queryFactory.Get<SelectShipmentPlanningsDto>(query);


            var queryLines = queryFactory
                   .Query()
                   .From(Tables.ShipmentPlanningLines)
                   .Select<ShipmentPlanningLines>(null)
                   .Join<Products>
                    (
                        s => new { UnitWeightKG = s.UnitWeight, ProductID = s.Id, ProductCode = s.Code, ProductType = s.ProductType },
                        nameof(ShipmentPlanningLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                    .Join<SalesOrders>
                    (
                        s => new { RequestedLoadingDate = s.CustomerRequestedDate, SalesOrderID = s.Id, CustomerOrderNr = s.CustomerOrderNr },
                        nameof(ShipmentPlanningLines.SalesOrderID),
                        nameof(SalesOrders.Id),
                        JoinType.Left
                    )

                    .Join<ProductionOrders>
                    (
                        s => new { PlannedQuantity = s.PlannedQuantity, ProductionOrderID = s.Id, LinkedProductionOrderID = s.LinkedProductionOrderID, ProductionDateReferenceID = s.ProductionDateReferenceID, PlannedLoadingTime = s.ShipmentDate },
                        nameof(ShipmentPlanningLines.ProductionOrderID),
                        nameof(ProductionOrders.Id),
                        JoinType.Left
                    )

                    .Where(new { ShipmentPlanningID = id }, Tables.ShipmentPlanningLines);

            var ShipmentPlanningLine = queryFactory.GetList<SelectShipmentPlanningLinesDto>(queryLines).ToList();

            ShipmentPlanning.SelectShipmentPlanningLines = ShipmentPlanningLine;

            LogsAppService.InsertLogToDatabase(ShipmentPlanning, ShipmentPlanning, LoginedUserService.UserId, Tables.ShipmentPlannings, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectShipmentPlanningsDto>(ShipmentPlanning);

        }
        public async Task<IDataResult<SelectShipmentPlanningsDto>> ODGetbyDateAsync(DateTime selectedDate)
        {
            var query = queryFactory.Query().From(Tables.ShipmentPlannings).Select("*").Where(new { ShipmentPlanningDate = selectedDate }, "");
            var ShipmentPlanning = queryFactory.Get<SelectShipmentPlanningsDto>(query);

            if (ShipmentPlanning != null)
            {

                var queryLines = queryFactory
                       .Query()
                       .From(Tables.ShipmentPlanningLines)
                       .Select<ShipmentPlanningLines>(null)
                       .Join<Products>
                        (
                            s => new { UnitWeightKG = s.UnitWeight, ProductID = s.Id, ProductCode = s.Code, ProductType = s.ProductType },
                            nameof(ShipmentPlanningLines.ProductID),
                            nameof(Products.Id),
                            JoinType.Left
                        )
                        .Join<SalesOrders>
                        (
                            s => new { RequestedLoadingDate = s.CustomerRequestedDate, SalesOrderID = s.Id, CustomerOrderNr = s.CustomerOrderNr },
                            nameof(ShipmentPlanningLines.SalesOrderID),
                            nameof(SalesOrders.Id),
                            JoinType.Left
                        )

                        .Join<ProductionOrders>
                        (
                            s => new { PlannedQuantity = s.PlannedQuantity, ProductionOrderID = s.Id, LinkedProductionOrderID = s.LinkedProductionOrderID, ProductionDateReferenceID = s.ProductionDateReferenceID, PlannedLoadingTime = s.ShipmentDate },
                            nameof(ShipmentPlanningLines.ProductionOrderID),
                            nameof(ProductionOrders.Id),
                            JoinType.Left
                        )

                        .Where(new { ShipmentPlanningID = ShipmentPlanning.Id }, Tables.ShipmentPlanningLines);

                var ShipmentPlanningLine = queryFactory.GetList<SelectShipmentPlanningLinesDto>(queryLines).ToList();

                ShipmentPlanning.SelectShipmentPlanningLines = ShipmentPlanningLine;

            }
            else
            {
                ShipmentPlanning = new SelectShipmentPlanningsDto();
            }

            await Task.CompletedTask;
            return new SuccessDataResult<SelectShipmentPlanningsDto>(ShipmentPlanning);

        }

        public async Task<IDataResult<SelectShipmentPlanningLinesDto>> GetLinebyProductionOrderAsync(Guid productionOrderID)
        {

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.ShipmentPlanningLines)
                   .Select<ShipmentPlanningLines>(s => new { s.Id, s.ShipmentPlanningID })
                   .Join<Products>
                    (
                        s => new { UnitWeightKG = s.UnitWeight, ProductID = s.Id, ProductCode = s.Code },
                        nameof(ShipmentPlanningLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                    .Join<SalesOrders>
                    (
                        s => new { RequestedLoadingDate = s.CustomerRequestedDate, SalesOrderID = s.Id, CustomerOrderNr = s.CustomerOrderNr },
                        nameof(ShipmentPlanningLines.SalesOrderID),
                        nameof(SalesOrders.Id),
                        JoinType.Left
                    )

                    .Join<ProductionOrders>
                    (
                        s => new { PlannedQuantity = s.PlannedQuantity, ProductionOrderID = s.Id, ProductionDateReferenceID = s.ProductionDateReferenceID, PlannedLoadingTime = s.ShipmentDate },
                        nameof(ShipmentPlanningLines.ProductionOrderID),
                        nameof(ProductionOrders.Id),
                        JoinType.Left
                    )

                    .Where(new { ProductionOrderID = productionOrderID }, Tables.ShipmentPlanningLines);

            var ShipmentPlanningLine = queryFactory.Get<SelectShipmentPlanningLinesDto>(queryLines);


            LogsAppService.InsertLogToDatabase(ShipmentPlanningLine, ShipmentPlanningLine, LoginedUserService.UserId, Tables.ShipmentPlanningLines, LogType.Get, ShipmentPlanningLine.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectShipmentPlanningLinesDto>(ShipmentPlanningLine);

        }


        public async Task<IDataResult<SelectShipmentPlanningLinesDto>> GetLineAsync(Guid Id)
        {

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.ShipmentPlanningLines)
                   .Select<ShipmentPlanningLines>(null)
                   .Join<Products>
                    (
                        s => new { UnitWeightKG = s.UnitWeight, ProductID = s.Id, ProductCode = s.Code },
                        nameof(ShipmentPlanningLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                    .Join<SalesOrders>
                    (
                        s => new { RequestedLoadingDate = s.CustomerRequestedDate, SalesOrderID = s.Id, CustomerOrderNr = s.CustomerOrderNr },
                        nameof(ShipmentPlanningLines.SalesOrderID),
                        nameof(SalesOrders.Id),
                        JoinType.Left
                    )

                    .Join<ProductionOrders>
                    (
                        s => new { PlannedQuantity = s.PlannedQuantity, ProductionOrderID = s.Id, ProductionDateReferenceID = s.ProductionDateReferenceID, PlannedLoadingTime = s.ShipmentDate },
                        nameof(ShipmentPlanningLines.ProductionOrderID),
                        nameof(ProductionOrders.Id),
                        JoinType.Left
                    )

                    .Where(new { Id = Id }, Tables.ShipmentPlanningLines);

            var ShipmentPlanningLine = queryFactory.Get<SelectShipmentPlanningLinesDto>(queryLines);


            LogsAppService.InsertLogToDatabase(ShipmentPlanningLine, ShipmentPlanningLine, LoginedUserService.UserId, Tables.ShipmentPlanningLines, LogType.Get, ShipmentPlanningLine.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectShipmentPlanningLinesDto>(ShipmentPlanningLine);

        }

        public async Task<IDataResult<SelectShipmentPlanningsDto>> GetbyProductionOrderAsync(Guid productionOrderID)
        {

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.ShipmentPlanningLines)
                   .Select<ShipmentPlanningLines>(s => new { s.Id, s.ShipmentPlanningID })
                   .Join<Products>
                    (
                        s => new { UnitWeightKG = s.UnitWeight, ProductID = s.Id, ProductCode = s.Code },
                        nameof(ShipmentPlanningLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                    .Join<SalesOrders>
                    (
                        s => new { RequestedLoadingDate = s.CustomerRequestedDate, SalesOrderID = s.Id, CustomerOrderNr = s.CustomerOrderNr },
                        nameof(ShipmentPlanningLines.SalesOrderID),
                        nameof(SalesOrders.Id),
                        JoinType.Left
                    )

                    .Join<ProductionOrders>
                    (
                        s => new { PlannedQuantity = s.PlannedQuantity, ProductionOrderID = s.Id, ProductionDateReferenceID = s.ProductionDateReferenceID, PlannedLoadingTime = s.ShipmentDate },
                        nameof(ShipmentPlanningLines.ProductionOrderID),
                        nameof(ProductionOrders.Id),
                        JoinType.Left
                    )

                    .Where(new { ProductionOrderID = productionOrderID }, Tables.ShipmentPlanningLines);

            var ShipmentPlanningLine = queryFactory.Get<SelectShipmentPlanningLinesDto>(queryLines);

            var query = queryFactory.Query().From(Tables.ShipmentPlannings).Select("*").Where(new { Id = ShipmentPlanningLine.ShipmentPlanningID }, "");
            var ShipmentPlanning = queryFactory.Get<SelectShipmentPlanningsDto>(query);


            await Task.CompletedTask;
            return new SuccessDataResult<SelectShipmentPlanningsDto>(ShipmentPlanning);

        }

        public async Task<IDataResult<IList<ListShipmentPlanningsDto>>> GetListAsync(ListShipmentPlanningsParameterDto input)
        {
            var query = queryFactory.Query().From(Tables.ShipmentPlannings).Select<ShipmentPlannings>(null).Where(null, "");
            var ShipmentPlannings = queryFactory.GetList<ListShipmentPlanningsDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListShipmentPlanningsDto>>(ShipmentPlannings);
        }


        /// <summary>
        /// Planlama Listeleri Operasyonel Dashboard
        /// </summary>
        public async Task<IDataResult<IList<ListShipmentPlanningsDto>>> ODGetListbyDateAsync(DateTime date)
        {
            string resultQuery = "SELECT * FROM " + Tables.ShipmentPlannings;

            string where = " (ShipmentPlanningDate>='" + date + "') ";

            Query query = new Query();
            query.Sql = resultQuery;
            query.WhereSentence = where;
            query.UseIsDeleteInQuery = false;
            var shipmentPlanningList = queryFactory.GetList<ListShipmentPlanningsDto>(query).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListShipmentPlanningsDto>>(shipmentPlanningList);
        }

        [ValidationAspect(typeof(UpdateShipmentPlanningsValidator), Priority = 1)]
        public async Task<IDataResult<SelectShipmentPlanningsDto>> UpdateAsync(UpdateShipmentPlanningsDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.ShipmentPlannings).Select("*").Where(new { Id = input.Id }, "");
            var entity = queryFactory.Get<SelectShipmentPlanningsDto>(entityQuery);

            var queryLines = queryFactory
                 .Query()
                 .From(Tables.ShipmentPlanningLines)
                 .Select<ShipmentPlanningLines>(null)
                  .Join<Products>
                    (
                        s => new { UnitWeightKG = s.UnitWeight, ProductID = s.Id, ProductCode = s.Code },
                        nameof(ShipmentPlanningLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                    .Join<SalesOrders>
                    (
                        s => new { RequestedLoadingDate = s.CustomerRequestedDate, SalesOrderID = s.Id, CustomerOrderNr = s.CustomerOrderNr },
                        nameof(ShipmentPlanningLines.SalesOrderID),
                        nameof(SalesOrders.Id),
                        JoinType.Left
                    )

                    .Join<ProductionOrders>
                    (
                        s => new { PlannedQuantity = s.PlannedQuantity, ProductionOrderID = s.Id, ProductionDateReferenceID = s.ProductionDateReferenceID, PlannedLoadingTime = s.ShipmentDate },
                        nameof(ShipmentPlanningLines.ProductionOrderID),
                        nameof(ProductionOrders.Id),
                        JoinType.Left
                    )
                  .Where(new { ShipmentPlanningID = input.Id }, Tables.ShipmentPlanningLines);

            var ShipmentPlanningLine = queryFactory.GetList<SelectShipmentPlanningLinesDto>(queryLines).ToList();

            entity.SelectShipmentPlanningLines = ShipmentPlanningLine;

            #region Update Control
            var listQuery = queryFactory
                           .Query()
                           .From(Tables.ShipmentPlannings).Select("*").Where(new { Code = input.Code }, Tables.ShipmentPlannings);

            var list = queryFactory.GetList<ListShipmentPlanningsDto>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion


            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.ShipmentPlannings).Update(new UpdateShipmentPlanningsDto
            {
                Description_ = input.Description_,
                PlannedAmount = input.PlannedAmount,
                ShipmentPlanningDate = input.ShipmentPlanningDate,
                PlannedLoadingTime = input.PlannedLoadingTime.GetValueOrDefault(),
                TotalAmount = input.TotalAmount,
                TotalGrossKG = input.TotalGrossKG,
                TotalNetKG = input.TotalNetKG,
                Code = input.Code,
                CreationTime = entity.CreationTime,
                CreatorId = entity.CreatorId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                Id = input.Id,
                IsDeleted = entity.IsDeleted,
                LastModificationTime = now,
                LastModifierId = LoginedUserService.UserId,
                ProductionDateReferenceID = input.ProductionDateReferenceID.GetValueOrDefault()
            }).Where(new { Id = input.Id }, "");

            foreach (var item in input.SelectShipmentPlanningLines)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.ShipmentPlanningLines).Insert(new CreateShipmentPlanningLinesDto
                    {
                        LineNr = item.LineNr,
                        ProductID = item.ProductID.GetValueOrDefault(),
                        SalesOrderID = item.SalesOrderID.GetValueOrDefault(),
                        PlannedQuantity = item.PlannedQuantity,
                        ProductionOrderID = item.ProductionOrderID.GetValueOrDefault(),
                        GrossWeightKG = item.GrossWeightKG,
                        LineDescription_ = item.LineDescription_,
                        PlannedStartDate = item.PlannedStartDate.GetValueOrDefault(),
                        PlannedEndDate = item.PlannedEndDate.GetValueOrDefault(),
                        NetWeightKG = item.NetWeightKG,
                        RequestedLoadingDate = item.RequestedLoadingDate,
                        SentQuantity = item.SentQuantity,
                        ShipmentQuantity = item.ShipmentQuantity,
                        UnitWeightKG = item.UnitWeightKG,
                        ShipmentPlanningID = input.Id,
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
                        ProductionDateReferenceID = item.ProductionDateReferenceID.GetValueOrDefault()
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.ShipmentPlanningLines).Select("*").Where(new { Id = item.Id }, "");

                    var line = queryFactory.Get<SelectShipmentPlanningLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.ShipmentPlanningLines).Update(new UpdateShipmentPlanningLinesDto
                        {
                            LineNr = item.LineNr,
                            UnitWeightKG = item.SentQuantity,
                            SentQuantity = item.SentQuantity,
                            ShipmentQuantity = item.ShipmentQuantity,
                            RequestedLoadingDate = item.RequestedLoadingDate,
                            NetWeightKG = item.NetWeightKG,
                            LineDescription_ = item.LineDescription_,
                            PlannedEndDate = item.PlannedEndDate.GetValueOrDefault(),
                            PlannedStartDate = item.PlannedStartDate.GetValueOrDefault(),
                            GrossWeightKG = item.GrossWeightKG,
                            ProductionOrderID = item.ProductionOrderID.GetValueOrDefault(),
                            PlannedQuantity = item.PlannedQuantity,
                            ProductID = item.ProductID.GetValueOrDefault(),
                            SalesOrderID = item.SalesOrderID.GetValueOrDefault(),
                            ShipmentPlanningID = input.Id,
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
                            ProductionDateReferenceID = item.ProductionDateReferenceID.GetValueOrDefault()
                        }).Where(new { Id = line.Id }, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;

                    }
                }

                #region Production Order Mamül-Yarı Mamül Update

                SelectProductionOrdersDto productionOrder = (await _productionOrdersAppService.GetAsync(item.ProductionOrderID.GetValueOrDefault())).Data;

                if (productionOrder != null && productionOrder.Id != Guid.Empty)
                {
                    productionOrder.ShipmentDate = input.PlannedLoadingTime;
                    productionOrder.ProductionDateReferenceID = input.ProductionDateReferenceID.GetValueOrDefault();

                    UpdateProductionOrdersDto updatedProdOrdInput = ObjectMapper.Map<SelectProductionOrdersDto, UpdateProductionOrdersDto>(productionOrder);

                    await _productionOrdersAppService.UpdateAsync(updatedProdOrdInput);
                }

                #endregion
            }

            var ShipmentPlanning = queryFactory.Update<SelectShipmentPlanningsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.ShipmentPlannings, LogType.Update, input.Id);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["ShipmentPlanningChildMenu"], L["ProcessRefresh"])).Data.FirstOrDefault();

            if (notTemplate != null && notTemplate.Id != Guid.Empty)
            {
                if (!string.IsNullOrEmpty(notTemplate.TargetUsersId))
                {
                    if (notTemplate.TargetUsersId.Contains("*Not*"))
                    {
                        string[] usersNot = notTemplate.TargetUsersId.Split("*Not*");

                        foreach (string user in usersNot)
                        {
                            CreateNotificationsDto createInput = new CreateNotificationsDto
                            {
                                ContextMenuName_ = notTemplate.ContextMenuName_,
                                IsViewed = false,

                                ModuleName_ = notTemplate.ModuleName_,
                                ProcessName_ = notTemplate.ProcessName_,
                                RecordNumber = input.Code,
                                NotificationDate = _GetSQLDateAppService.GetDateFromSQL(),
                                UserId = new Guid(user),
                                ViewDate = null,
                            };

                            await _NotificationsAppService.CreateAsync(createInput);
                        }
                    }
                    else
                    {
                        CreateNotificationsDto createInput = new CreateNotificationsDto
                        {
                            ContextMenuName_ = notTemplate.ContextMenuName_,
                            IsViewed = false,

                            ModuleName_ = notTemplate.ModuleName_,
                            ProcessName_ = notTemplate.ProcessName_,
                            RecordNumber = input.Code,
                            NotificationDate = _GetSQLDateAppService.GetDateFromSQL(),
                            UserId = new Guid(notTemplate.TargetUsersId),
                            ViewDate = null,
                        };

                        await _NotificationsAppService.CreateAsync(createInput);
                    }
                }

            }

            #endregion

            await Task.CompletedTask;
            return new SuccessDataResult<SelectShipmentPlanningsDto>(ShipmentPlanning);

        }

        public async Task<IDataResult<SelectShipmentPlanningsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.ShipmentPlannings).Select("*").Where(new { Id = id }, "");

            var entity = queryFactory.Get<ShipmentPlannings>(entityQuery);

            var query = queryFactory.Query().From(Tables.ShipmentPlannings).Update(new UpdateShipmentPlanningsDto
            {
                Description_ = entity.Description_,
                PlannedAmount = entity.PlannedAmount,
                ShipmentPlanningDate = entity.ShipmentPlanningDate,
                TotalAmount = entity.TotalAmount,
                PlannedLoadingTime = entity.PlannedLoadingTime.GetValueOrDefault(),
                TotalGrossKG = entity.TotalGrossKG,
                TotalNetKG = entity.TotalNetKG,
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
                ProductionDateReferenceID = entity.ProductionDateReferenceID.GetValueOrDefault()
            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id }, "");

            var ShipmentPlanningsDto = queryFactory.Update<SelectShipmentPlanningsDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectShipmentPlanningsDto>(ShipmentPlanningsDto);


        }
    }
}

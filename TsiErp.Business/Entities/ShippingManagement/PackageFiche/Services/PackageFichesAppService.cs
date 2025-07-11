﻿using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TSI.QueryBuilder.Models;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.NotificationTemplate.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.Other.Notification.Services;
using TsiErp.Business.Entities.ShippingManagement.PackageFiche.Validations;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder;
using TsiErp.Entities.Entities.SalesManagement.SalesOrder;
using TsiErp.Entities.Entities.ShippingManagement.PackageFiche;
using TsiErp.Entities.Entities.ShippingManagement.PackageFiche.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.PackageFicheLine;
using TsiErp.Entities.Entities.ShippingManagement.PackageFicheLine.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.Entities.StockManagement.TechnicalDrawing.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.PackageFiches.Page;

namespace TsiErp.Business.Entities.PackageFiche.Services
{
    [ServiceRegistration(typeof(IPackageFichesAppService), DependencyInjectionType.Scoped)]
    public class PackageFichesAppService : ApplicationService<PackageFichesResource>, IPackageFichesAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;
        private readonly INotificationsAppService _NotificationsAppService;
        private readonly INotificationTemplatesAppService _NotificationTemplatesAppService;

        public PackageFichesAppService(IStringLocalizer<PackageFichesResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
            _NotificationsAppService = notificationsAppService;
            _NotificationTemplatesAppService = notificationTemplatesAppService;
        }




        [ValidationAspect(typeof(CreatePackageFichesValidator), Priority = 1)]
        public async Task<IDataResult<SelectPackageFichesDto>> CreateAsync(CreatePackageFichesDto input)
        {

            if (input.SelectPackageFicheLines != null && input.SelectPackageFicheLines.Count > 0)
            {
                foreach (var line in input.SelectPackageFicheLines)
                {
                    string code = FicheNumbersAppService.GetFicheNumberAsync("PackageFichesChildMenu");

                    Guid addedEntityId = GuidGenerator.CreateGuid();
                    DateTime now = _GetSQLDateAppService.GetDateFromSQL();

                    var query = queryFactory.Query().From(Tables.PackageFiches).Insert(new CreatePackageFichesDto
                    {
                        CurrentAccountID = input.CurrentAccountID.GetValueOrDefault(),
                        ProductID = input.ProductID.GetValueOrDefault(),
                        SalesOrderID = input.SalesOrderID.GetValueOrDefault(),
                        ProductionOrderID = input.ProductionOrderID.GetValueOrDefault(),
                        PackingListID = input.PackingListID.GetValueOrDefault(),
                        Date_ = input.Date_,
                        NumberofPackage = input.NumberofPackage,
                        ProductionOrderReferenceNo = input.ProductionOrderReferenceNo,
                        PackageContent = input.PackageContent,
                        PackageType = input.PackageType,
                        PalletNumber = input.PalletNumber,
                        ProductPalletOrder = input.ProductPalletOrder,
                        UnitWeight = input.UnitWeight,
                        Code = code,
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
                    });

                    var queryLine = queryFactory.Query().From(Tables.PackageFicheLines).Insert(new CreatePackageFicheLinesDto
                    {
                        PackingDate = line.PackingDate,
                        ProductionOrderID = line.ProductionOrderID.GetValueOrDefault(),
                        PackageFicheID = addedEntityId,
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
                        LineNr = 1,
                        ProductID = line.ProductID.GetValueOrDefault(),
                        Quantity = line.Quantity,
                        NumberofPackage = line.NumberofPackage,
                        PackageContent = line.PackageContent,
                        ProductionOrderFicheNo = line.ProductionOrderFicheNo,
                        Status_ = line.Status_,

                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;

                    var packageFiche = queryFactory.Insert<SelectPackageFichesDto>(query, "Id", true);

                    await FicheNumbersAppService.UpdateFicheNumberAsync("PackageFichesChildMenu", code);

                    LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.PackageFiches, LogType.Insert, addedEntityId);
                    #region Notification

                    var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["PackageFichesChildMenu"], L["ProcessAdd"])).Data.FirstOrDefault();

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
                }


            }

            SelectPackageFichesDto packageTemp = new SelectPackageFichesDto();

            await Task.CompletedTask;
            return new SuccessDataResult<SelectPackageFichesDto>(packageTemp);

        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            var entity = (await GetAsync(id)).Data;
            var query = queryFactory.Query().From(Tables.PackageFiches).Select("*").Where(new { Id = id }, "");

            var PackageFiches = queryFactory.Get<SelectPackageFichesDto>(query);

            if (PackageFiches.Id != Guid.Empty && PackageFiches != null)
            {
                var deleteQuery = queryFactory.Query().From(Tables.PackageFiches).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

                var lineDeleteQuery = queryFactory.Query().From(Tables.PackageFicheLines).Delete(LoginedUserService.UserId).Where(new { PackageFicheID = id }, "");

                deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                var packageFiche = queryFactory.Update<SelectPackageFichesDto>(deleteQuery, "Id", true);
                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.PackageFiches, LogType.Delete, id);
                #region Notification

                var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["PackageFichesChildMenu"], L["ProcessDelete"])).Data.FirstOrDefault();

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
                await Task.CompletedTask;
                return new SuccessDataResult<SelectPackageFichesDto>(packageFiche);
            }
            else
            {
                var queryLine = queryFactory.Query().From(Tables.PackageFicheLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");
                var packageFicheLines = queryFactory.Update<SelectPackageFicheLinesDto>(queryLine, "Id", true);
                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.PackageFicheLines, LogType.Delete, id);
                await Task.CompletedTask;
                return new SuccessDataResult<SelectPackageFicheLinesDto>(packageFicheLines);
            }

        }

        public async Task<IDataResult<SelectPackageFichesDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.PackageFiches)
                   .Select<PackageFiches>(null)
                   .Join<Products>
                    (
                        pr => new { ProductCode = pr.Code, ProductName = pr.Name, ProductID = pr.Id, ProductUnitWeight = pr.UnitWeight },
                        nameof(PackageFiches.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                     .Join<SalesOrders>
                    (
                        pr => new { SalesOrderFicheNo = pr.FicheNo, SalesOrderCustomerOrderNo = pr.CustomerOrderNr, SalesOrderID = pr.Id },
                        nameof(PackageFiches.SalesOrderID),
                        nameof(SalesOrders.Id),
                        JoinType.Left
                    )
                     .Join<ProductionOrders>
                    (
                        pr => new { ProductionOrderFicheNo = pr.FicheNo, ProductionOrderID = pr.Id },
                        nameof(PackageFiches.ProductionOrderID),
                        nameof(ProductionOrders.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        pr => new { CustomerCode = pr.CustomerCode, CurrentAccountID = pr.Id },
                        nameof(PackageFiches.CurrentAccountID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = id }, Tables.PackageFiches);

            var packageFiches = queryFactory.Get<SelectPackageFichesDto>(query);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.PackageFicheLines)
                   .Select<PackageFicheLines>(null)
                   .Join<ProductionOrders>
                    (
                        p => new { ProductionOrderID = p.Id, ProductionOrderFicheNo = p.FicheNo },
                        nameof(PackageFicheLines.ProductionOrderID),
                        nameof(ProductionOrders.Id),
                        JoinType.Left
                    )
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(PackageFicheLines.ProductID),
                        nameof(Products.Id),
                        "ProductLine",
                        JoinType.Left
                    )
                    .Where(new { PackageFicheID = id }, Tables.PackageFicheLines);

            var PackageFicheLine = queryFactory.GetList<SelectPackageFicheLinesDto>(queryLines).ToList();

            packageFiches.SelectPackageFicheLines = PackageFicheLine;

            LogsAppService.InsertLogToDatabase(packageFiches, packageFiches, LoginedUserService.UserId, Tables.PackageFiches, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectPackageFichesDto>(packageFiches);

        }

        public async Task<IDataResult<IList<ListPackageFichesDto>>> GetListAsync(ListPackageFichesParameterDto input)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.PackageFiches)
                   .Select<PackageFiches>(null)
                   .Join<Products>
                    (
                        pr => new { ProductCode = pr.Code, ProducName = pr.Name, ProductID = pr.Id, ProductUnitWeight = pr.UnitWeight },
                        nameof(PackageFiches.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )

                     .Join<ProductionOrders>
                    (
                        pr => new { ProductionOrderFicheNo = pr.FicheNo, ProductionOrderID = pr.Id },
                        nameof(PackageFiches.ProductionOrderID),
                        nameof(ProductionOrders.Id),
                        JoinType.Left
                    )
                     .Join<SalesOrders>
                    (
                        pr => new { SalesOrderFicheNo = pr.FicheNo, SalesOrderCustomerOrderNo = pr.CustomerOrderNr, SalesOrderID = pr.Id },
                        nameof(PackageFiches.SalesOrderID),
                        nameof(SalesOrders.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        pr => new { CustomerCode = pr.CustomerCode, CurrentAccountID = pr.Id },
                        nameof(PackageFiches.CurrentAccountID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Where(null, Tables.PackageFiches);

            var packageFiches = queryFactory.GetList<ListPackageFichesDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListPackageFichesDto>>(packageFiches);

        }

        public async Task<IDataResult<IList<SelectPackageFichesDto>>> GetSelectListbyCurrentAccountandPackageTypeAsync(Guid currentAccountID, string packageType)
        {
            var query = queryFactory
                    .Query()
                    .From(Tables.PackageFiches)
                    .Select<PackageFiches>(null)
                    .Join<Products>
                     (
                         pr => new { ProductCode = pr.Code, ProductName = pr.Name, ProductID = pr.Id, ProductUnitWeight = pr.UnitWeight },
                         nameof(PackageFiches.ProductID),
                         nameof(Products.Id),
                         JoinType.Left
                     )
                      .Join<SalesOrders>
                     (
                         pr => new { SalesOrderFicheNo = pr.FicheNo, SalesOrderCustomerOrderNo = pr.CustomerOrderNr, SalesOrderID = pr.Id },
                         nameof(PackageFiches.SalesOrderID),
                         nameof(SalesOrders.Id),
                         JoinType.Left
                     )
                      .Join<ProductionOrders>
                     (
                         pr => new { ProductionOrderFicheNo = pr.FicheNo, ProductionOrderID = pr.Id },
                         nameof(PackageFiches.ProductionOrderID),
                         nameof(ProductionOrders.Id),
                         JoinType.Left
                     )
                     .Join<CurrentAccountCards>
                     (
                         pr => new { CustomerCode = pr.CustomerCode, CurrentAccountID = pr.Id },
                         nameof(PackageFiches.CurrentAccountID),
                         nameof(CurrentAccountCards.Id),
                         JoinType.Left
                     )
                     .Where(new { CurrentAccountID = currentAccountID, PackageType = packageType }, Tables.PackageFiches);

            var packageFiches = queryFactory.GetList<SelectPackageFichesDto>(query).ToList();


            foreach (var packageFiche in packageFiches)
            {
                var queryLines = queryFactory
               .Query()
               .From(Tables.PackageFicheLines)
               .Select<PackageFicheLines>(null)
               .Join<ProductionOrders>
                (
                    p => new { ProductionOrderID = p.Id, ProductionOrderFicheNo = p.FicheNo },
                    nameof(PackageFicheLines.ProductionOrderID),
                    nameof(ProductionOrders.Id),
                    JoinType.Left
                )
               .Join<Products>
                (
                    p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                    nameof(PackageFicheLines.ProductID),
                    nameof(Products.Id),
                    "ProductLine",
                    JoinType.Left
                )
                .Where(new { PackageFicheID = packageFiche.Id }, Tables.PackageFicheLines);

                var PackageFicheLine = queryFactory.GetList<SelectPackageFicheLinesDto>(queryLines).ToList();

                packageFiche.SelectPackageFicheLines = PackageFicheLine;
            }


            await Task.CompletedTask;
            return new SuccessDataResult<IList<SelectPackageFichesDto>>(packageFiches);

        }

        [ValidationAspect(typeof(UpdatePackageFichesValidator), Priority = 1)]
        public async Task<IDataResult<SelectPackageFichesDto>> UpdateAsync(UpdatePackageFichesDto input)
        {
            var entityQuery = queryFactory
                   .Query()
                   .From(Tables.PackageFiches)
                   .Select<PackageFiches>(null)
                   .Join<Products>
                    (
                        pr => new { ProductCode = pr.Code, ProducName = pr.Name, ProductID = pr.Id, ProductUnitWeight = pr.UnitWeight },
                        nameof(PackageFiches.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )

                     .Join<ProductionOrders>
                    (
                        pr => new { ProductionOrderFicheNo = pr.FicheNo, ProductionOrderID = pr.Id },
                        nameof(PackageFiches.ProductionOrderID),
                        nameof(ProductionOrders.Id),
                        JoinType.Left
                    )
                     .Join<SalesOrders>
                    (
                        pr => new { SalesOrderFicheNo = pr.FicheNo, SalesOrderCustomerOrderNo = pr.CustomerOrderNr, SalesOrderID = pr.Id },
                        nameof(PackageFiches.SalesOrderID),
                        nameof(SalesOrders.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        pr => new { CustomerCode = pr.CustomerCode, CurrentAccountID = pr.Id },
                        nameof(PackageFiches.CurrentAccountID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = input.Id }, Tables.PackageFiches);

            var entity = queryFactory.Get<SelectPackageFichesDto>(entityQuery);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.PackageFicheLines)
                   .Select<PackageFicheLines>(null)
                   .Join<ProductionOrders>
                    (
                        p => new { ProductionOrderID = p.Id, ProductionOrderFicheNo = p.FicheNo },
                        nameof(PackageFicheLines.ProductionOrderID),
                        nameof(ProductionOrders.Id),
                        JoinType.Left
                    )
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(PackageFicheLines.ProductID),
                        nameof(Products.Id),
                        "ProductLine",
                        JoinType.Left
                    )
                    .Where(new { PackageFicheID = input.Id }, Tables.PackageFicheLines);

            var PackageFicheLine = queryFactory.GetList<SelectPackageFicheLinesDto>(queryLines).ToList();

            entity.SelectPackageFicheLines = PackageFicheLine;

            #region Update Control
            var listQuery = queryFactory
                           .Query()
                           .From(Tables.PackageFiches)
                   .Select<PackageFiches>(null)
                   .Join<Products>
                    (
                        pr => new { ProductCode = pr.Code, ProducName = pr.Name, ProductID = pr.Id, ProductUnitWeight = pr.UnitWeight },
                        nameof(PackageFiches.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                     .Join<SalesOrders>
                    (
                        pr => new { SalesOrderFicheNo = pr.FicheNo, SalesOrderCustomerOrderNo = pr.CustomerOrderNr, SalesOrderID = pr.Id },
                        nameof(PackageFiches.SalesOrderID),
                        nameof(SalesOrders.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        pr => new { CustomerCode = pr.CustomerCode, CurrentAccountID = pr.Id },
                        nameof(PackageFiches.CurrentAccountID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                            .Where(new { Code = input.Code }, Tables.PackageFiches);

            var list = queryFactory.GetList<ListPackageFichesDto>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.PackageFiches).Update(new UpdatePackageFichesDto
            {
                CurrentAccountID = input.CurrentAccountID.GetValueOrDefault(),
                ProductID = input.ProductID.GetValueOrDefault(),
                PackingListID = input.PackingListID.GetValueOrDefault(),
                SalesOrderID = input.SalesOrderID.GetValueOrDefault(),
                ProductionOrderID = input.ProductionOrderID.GetValueOrDefault(),
                Date_ = input.Date_,
                NumberofPackage = input.NumberofPackage,
                ProductionOrderReferenceNo = input.ProductionOrderReferenceNo,
                PackageContent = input.PackageContent,
                PackageType = input.PackageType,
                PalletNumber = input.PalletNumber,
                ProductPalletOrder = input.ProductPalletOrder,
                UnitWeight = input.UnitWeight,
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
            }).Where(new { Id = input.Id }, "");

            foreach (var item in input.SelectPackageFicheLines)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.PackageFicheLines).Insert(new CreatePackageFicheLinesDto
                    {
                        PackageFicheID = input.Id,
                        PackingDate = item.PackingDate,
                        ProductionOrderFicheNo = item.ProductionOrderFicheNo,
                        PackageContent = item.PackageContent,
                        NumberofPackage = item.NumberofPackage,
                        ProductionOrderID = item.ProductionOrderID.GetValueOrDefault(),
                        Status_ = item.Status_,
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
                        ProductID = item.ProductID.GetValueOrDefault(),
                        Quantity = item.Quantity,
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.PackageFicheLines).Select("*").Where(new { Id = item.Id }, "");

                    var line = queryFactory.Get<SelectPackageFicheLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.PackageFicheLines).Update(new UpdatePackageFicheLinesDto
                        {
                            PackageFicheID = input.Id,
                            PackingDate = item.PackingDate,
                            ProductionOrderID = item.ProductionOrderID.GetValueOrDefault(),
                            NumberofPackage = item.NumberofPackage,
                            PackageContent = item.PackageContent,
                            ProductionOrderFicheNo = item.ProductionOrderFicheNo,
                            Status_ = item.Status_,
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
                            LineNr = item.LineNr,
                            ProductID = item.ProductID.GetValueOrDefault(),
                            Quantity = item.Quantity,
                        }).Where(new { Id = line.Id }, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }

            var billOfMaterial = queryFactory.Update<SelectPackageFichesDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.PackageFiches, LogType.Update, billOfMaterial.Id);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["PackageFichesChildMenu"], L["ProcessRefresh"])).Data.FirstOrDefault();

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
            return new SuccessDataResult<SelectPackageFichesDto>(billOfMaterial);

        }

        public async Task<IDataResult<SelectPackageFichesDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.PackageFiches).Select("*").Where(new { Id = id }, "");

            var entity = queryFactory.Get<PackageFiches>(entityQuery);

            var query = queryFactory.Query().From(Tables.PackageFiches).Update(new UpdatePackageFichesDto
            {
                CurrentAccountID = entity.CurrentAccountID,
                PackingListID = entity.PackingListID,
                ProductionOrderID = entity.ProductionOrderID,
                ProductID = entity.ProductID,
                ProductionOrderReferenceNo = entity.ProductionOrderReferenceNo,
                SalesOrderID = entity.SalesOrderID,
                Date_ = entity.Date_,
                NumberofPackage = entity.NumberofPackage,
                PackageContent = entity.PackageContent,
                PackageType = entity.PackageType,
                PalletNumber = entity.PalletNumber,
                ProductPalletOrder = entity.ProductPalletOrder,
                UnitWeight = entity.UnitWeight,
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
            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id }, "");

            var PackageFichesDto = queryFactory.Update<SelectPackageFichesDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectPackageFichesDto>(PackageFichesDto);


        }
    }
}

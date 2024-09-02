using Microsoft.Extensions.Localization;
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
using TsiErp.Business.Entities.PurchasePrice.Validations;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Currency;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchasePrice;
using TsiErp.Entities.Entities.PurchaseManagement.PurchasePrice.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchasePriceLine;
using TsiErp.Entities.Entities.PurchaseManagement.PurchasePriceLine.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.Entities.StockManagement.TechnicalDrawing.Dtos;
using TsiErp.Entities.Entities.StockManagement.WareHouse;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.PurchasePrices.Page;

namespace TsiErp.Business.Entities.PurchasePrice.Services
{
    [ServiceRegistration(typeof(IPurchasePricesAppService), DependencyInjectionType.Scoped)]
    public class PurchasePricesAppService : ApplicationService<PurchasePricesResource>, IPurchasePricesAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;
        private readonly INotificationsAppService _NotificationsAppService;
        private readonly INotificationTemplatesAppService _NotificationTemplatesAppService;

        public PurchasePricesAppService(IStringLocalizer<PurchasePricesResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
            _NotificationsAppService = notificationsAppService;
            _NotificationTemplatesAppService = notificationTemplatesAppService;
        }




        [ValidationAspect(typeof(CreatePurchasePricesValidatorDto), Priority = 1)]
        public async Task<IDataResult<SelectPurchasePricesDto>> CreateAsync(CreatePurchasePricesDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.PurchasePrices).Select("Code").Where(new { Code = input.Code }, "");
            var list = queryFactory.ControlList<PurchasePrices>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();
            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.PurchasePrices).Insert(new CreatePurchasePricesDto
            {
                BranchID = input.BranchID.GetValueOrDefault(),
                CurrencyID = input.CurrencyID.GetValueOrDefault(),
                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                EndDate = input.EndDate,
                IsApproved = input.IsApproved,
                StartDate = input.StartDate,
                WarehouseID = input.WarehouseID.GetValueOrDefault(),
                Code = input.Code,
                CreationTime = now,
                CreatorId = LoginedUserService.UserId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = Guid.Empty,
                DeletionTime = null,
                Id = addedEntityId,
                IsActive = true,
                IsDeleted = false,
                LastModificationTime = null,
                LastModifierId = Guid.Empty,
                Name = input.Name,
            });

            foreach (var item in input.SelectPurchasePriceLines)
            {
                var queryLine = queryFactory.Query().From(Tables.PurchasePriceLines).Insert(new CreatePurchasePriceLinesDto
                {
                    StartDate = input.StartDate,
                    EndDate = input.EndDate,
                    CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                    CurrencyID = input.CurrencyID.GetValueOrDefault(),
                    SupplyDateDay = item.SupplyDateDay,
                    Linenr = item.Linenr,
                    Price = item.Price,
                    PurchasePriceID = addedEntityId,
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
                    ProductID = item.ProductID.GetValueOrDefault(),
                     IsApproved = input.IsApproved
                });

                query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
            }

            var purchasePrice = queryFactory.Insert<SelectPurchasePricesDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("PurchasePricesChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.PurchasePrices, LogType.Insert, addedEntityId);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["PurchasePricesChildMenu"], L["ProcessAdd"])).Data.FirstOrDefault();

            if (notTemplate != null && notTemplate.Id != Guid.Empty)
            {
                if (!string.IsNullOrEmpty(notTemplate.TargetUsersId))
                {
                    if (notTemplate.TargetUsersId.Contains(","))
                    {
                        string[] usersNot = notTemplate.TargetUsersId.Split(',');

                        foreach (string user in usersNot)
                        {
                            CreateNotificationsDto createInput = new CreateNotificationsDto
                            {
                                ContextMenuName_ = notTemplate.ContextMenuName_,
                                IsViewed = false,
                                Message_ = notTemplate.Message_,
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
                            Message_ = notTemplate.Message_,
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

            return new SuccessDataResult<SelectPurchasePricesDto>(purchasePrice);

        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            var entity = (await GetAsync(id)).Data;
            var query = queryFactory.Query().From(Tables.PurchasePrices).Select("*").Where(new { Id = id }, "");

            var purchasePrices = queryFactory.Get<SelectPurchasePricesDto>(query);

            if (purchasePrices.Id != Guid.Empty && purchasePrices != null)
            {
                var deleteQuery = queryFactory.Query().From(Tables.PurchasePrices).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

                var lineDeleteQuery = queryFactory.Query().From(Tables.PurchasePriceLines).Delete(LoginedUserService.UserId).Where(new { PurchasePriceID = id }, "");

                deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                var purchasePrice = queryFactory.Update<SelectPurchasePricesDto>(deleteQuery, "Id", true);
                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.PurchasePrices, LogType.Delete, id);
                #region Notification

                var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["PurchasePricesChildMenu"], L["ProcessDelete"])).Data.FirstOrDefault();

                if (notTemplate != null && notTemplate.Id != Guid.Empty)
                {
                    if (!string.IsNullOrEmpty(notTemplate.TargetUsersId))
                    {
                        if (notTemplate.TargetUsersId.Contains(","))
                        {
                            string[] usersNot = notTemplate.TargetUsersId.Split(',');

                            foreach (string user in usersNot)
                            {
                                CreateNotificationsDto createInput = new CreateNotificationsDto
                                {
                                    ContextMenuName_ = notTemplate.ContextMenuName_,
                                    IsViewed = false,
                                    Message_ = notTemplate.Message_,
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
                                Message_ = notTemplate.Message_,
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
                return new SuccessDataResult<SelectPurchasePricesDto>(purchasePrice);
            }
            else
            {
                var queryLine = queryFactory.Query().From(Tables.PurchasePriceLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");
                var billOfMaterialLines = queryFactory.Update<SelectPurchasePriceLinesDto>(queryLine, "Id", true);
                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.PurchasePriceLines, LogType.Delete, id);
                await Task.CompletedTask;
                return new SuccessDataResult<SelectPurchasePriceLinesDto>(billOfMaterialLines);
            }

        }

        public async Task<IDataResult<SelectPurchasePricesDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.PurchasePrices)
                   .Select<PurchasePrices>(null)
                   .Join<Currencies>
                    (
                        c => new { CurrencyID = c.Id, CurrencyCode = c.Code },
                        nameof(PurchasePrices.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchID = b.Id, BranchCode = b.Code },
                        nameof(PurchasePrices.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                    .Join<Warehouses>
                    (
                        w => new { WarehouseID = w.Id, WarehouseCode = w.Code },
                        nameof(PurchasePrices.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(PurchasePrices.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = id }, Tables.PurchasePrices);

            var purchasePrices = queryFactory.Get<SelectPurchasePricesDto>(query);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.PurchasePriceLines)
                   .Select<PurchasePriceLines>(null)
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(PurchasePriceLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<Currencies>
                    (
                        cr => new { CurrencyID = cr.Id, CurrencyCode = cr.Code },
                        nameof(PurchasePriceLines.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )

                    .Join<CurrentAccountCards>
                    (
                        cr => new { CurrentAccountCardID = cr.Id, CurrentAccountCardName = cr.Name },
                        nameof(PurchasePriceLines.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        "CurrentAccountCardLine",
                        JoinType.Left
                    )
                    .Where(new { PurchasePriceID = id }, Tables.PurchasePriceLines);

            var purchasePriceLine = queryFactory.GetList<SelectPurchasePriceLinesDto>(queryLines).ToList();

            purchasePrices.SelectPurchasePriceLines = purchasePriceLine;

            LogsAppService.InsertLogToDatabase(purchasePrices, purchasePrices, LoginedUserService.UserId, Tables.PurchasePrices, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectPurchasePricesDto>(purchasePrices);

        }

        public async Task<IDataResult<IList<ListPurchasePricesDto>>> GetListAsync(ListPurchasePricesParameterDto input)
        {
            var query = queryFactory
                   .Query()
                    .From(Tables.PurchasePrices)
                   .Select<PurchasePrices>(s => new { s.StartDate, s.EndDate, s.IsApproved, s.Name, s.Code, s.Id })
                   .Join<Currencies>
                    (
                        c => new { CurrencyCode = c.Code },
                        nameof(PurchasePrices.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchCode = b.Code },
                        nameof(PurchasePrices.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                    .Join<Warehouses>
                    (
                        w => new { WarehouseCode = w.Code },
                        nameof(PurchasePrices.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(PurchasePrices.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Where(null, Tables.PurchasePrices);

            var purchasePrices = queryFactory.GetList<ListPurchasePricesDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListPurchasePricesDto>>(purchasePrices);

        }

        [ValidationAspect(typeof(UpdatePurchasePricesValidatorDto), Priority = 1)]
        public async Task<IDataResult<SelectPurchasePricesDto>> UpdateAsync(UpdatePurchasePricesDto input)
        {
            var entityQuery = queryFactory
                   .Query()
                  .From(Tables.PurchasePrices)
                   .Select<PurchasePrices>(null)
                   .Join<Currencies>
                    (
                        c => new { CurrencyID = c.Id, CurrencyCode = c.Code },
                        nameof(PurchasePrices.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchID = b.Id, BranchCode = b.Code },
                        nameof(PurchasePrices.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                    .Join<Warehouses>
                    (
                        w => new { WarehouseID = w.Id, WarehouseCode = w.Code },
                        nameof(PurchasePrices.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(PurchasePrices.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = input.Id }, Tables.PurchasePrices);

            var entity = queryFactory.Get<SelectPurchasePricesDto>(entityQuery);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.PurchasePriceLines)
                   .Select<PurchasePriceLines>(null)
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(PurchasePriceLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<Currencies>
                    (
                        cr => new { CurrencyID = cr.Id, CurrencyCode = cr.Code },
                        nameof(PurchasePriceLines.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                    .Where(new { PurchasePriceID = input.Id }, Tables.PurchasePriceLines);

            var purchasePriceLines = queryFactory.GetList<SelectPurchasePriceLinesDto>(queryLines).ToList();

            entity.SelectPurchasePriceLines = purchasePriceLines;

            #region Update Control
            var listQuery = queryFactory
                           .Query()
                            .From(Tables.PurchasePrices)
                   .Select<PurchasePrices>(null)
                   .Join<Currencies>
                    (
                        c => new { CurrencyCode = c.Code },
                        nameof(PurchasePrices.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                    .Join<Branches>
                    (
                        b => new { BranchCode = b.Code },
                        nameof(PurchasePrices.BranchID),
                        nameof(Branches.Id),
                        JoinType.Left
                    )
                    .Join<Warehouses>
                    (
                        w => new { WarehouseCode = w.Code },
                        nameof(PurchasePrices.WarehouseID),
                        nameof(Warehouses.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                        nameof(PurchasePrices.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Where(new { Code = input.Code }, Tables.PurchasePrices);

            var list = queryFactory.GetList<ListPurchasePricesDto>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.PurchasePrices).Update(new UpdatePurchasePricesDto
            {
                BranchID = input.BranchID.GetValueOrDefault(),
                CurrencyID = input.CurrencyID.GetValueOrDefault(),
                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                EndDate = input.EndDate,
                IsApproved = input.IsApproved,
                StartDate = input.StartDate,
                WarehouseID = input.WarehouseID.GetValueOrDefault(),
                Code = input.Code,
                CreationTime = entity.CreationTime,
                CreatorId = entity.CreatorId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                Id = input.Id,
                IsActive = input.IsActive,
                IsDeleted = entity.IsDeleted,
                LastModificationTime = now,
                LastModifierId = LoginedUserService.UserId,
                Name = input.Name,
            }).Where(new { Id = input.Id }, "");

            foreach (var item in input.SelectPurchasePriceLines)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.PurchasePriceLines).Insert(new CreatePurchasePriceLinesDto
                    {
                        StartDate = input.StartDate,
                        EndDate = input.EndDate,
                        CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                        CurrencyID = input.CurrencyID.GetValueOrDefault(),
                        Linenr = item.Linenr,
                        SupplyDateDay = item.SupplyDateDay,
                        Price = item.Price,
                        PurchasePriceID = input.Id,
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
                        ProductID = item.ProductID.GetValueOrDefault(),
                         IsApproved = input.IsApproved
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.PurchasePriceLines).Select("*").Where(new { Id = item.Id }, "");

                    var line = queryFactory.Get<SelectPurchasePriceLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.PurchasePriceLines).Update(new UpdatePurchasePriceLinesDto
                        {
                            StartDate = input.StartDate,
                            ProductID = item.ProductID.GetValueOrDefault(),
                            EndDate = input.EndDate,
                            SupplyDateDay = item.SupplyDateDay,
                            CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                            CurrencyID = input.CurrencyID.GetValueOrDefault(),
                            Linenr = item.Linenr,
                            Price = item.Price,
                            PurchasePriceID = input.Id,
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
                             IsApproved = input.IsApproved
                        }).Where(new { Id = line.Id }, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }

            var purchasePrice = queryFactory.Update<SelectPurchasePricesDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.PurchasePrices, LogType.Update, entity.Id);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["PurchasePricesChildMenu"], L["ProcessRefresh"])).Data.FirstOrDefault();

            if (notTemplate != null && notTemplate.Id != Guid.Empty)
            {
                if (!string.IsNullOrEmpty(notTemplate.TargetUsersId))
                {
                    if (notTemplate.TargetUsersId.Contains(","))
                    {
                        string[] usersNot = notTemplate.TargetUsersId.Split(',');

                        foreach (string user in usersNot)
                        {
                            CreateNotificationsDto createInput = new CreateNotificationsDto
                            {
                                ContextMenuName_ = notTemplate.ContextMenuName_,
                                IsViewed = false,
                                Message_ = notTemplate.Message_,
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
                            Message_ = notTemplate.Message_,
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
            return new SuccessDataResult<SelectPurchasePricesDto>(purchasePrice);

        }

        public async Task<IDataResult<IList<SelectPurchasePriceLinesDto>>> GetSelectLineListAsync(Guid productId)
        {
            var queryLines = queryFactory
                   .Query()
                   .From(Tables.PurchasePriceLines)
                   .Select<PurchasePriceLines>(null)
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(PurchasePriceLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<Currencies>
                    (
                        cr => new { CurrencyID = cr.Id, CurrencyCode = cr.Code },
                        nameof(PurchasePriceLines.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        cr => new { CurrentAccountCardID = cr.Id, CurrentAccountCardName = cr.Name },
                        nameof(PurchasePriceLines.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Where(new { ProductID = productId }, Tables.PurchasePriceLines);

            var purchasePriceLine = queryFactory.GetList<SelectPurchasePriceLinesDto>(queryLines).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<SelectPurchasePriceLinesDto>>(purchasePriceLine);

        }

        public async Task<IDataResult<SelectPurchasePricesDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.PurchasePrices).Select("*").Where(new { Id = id }, "");

            var entity = queryFactory.Get<PurchasePrices>(entityQuery);

            var query = queryFactory.Query().From(Tables.PurchasePrices).Update(new UpdatePurchasePricesDto
            {
                BranchID = entity.BranchID,
                CurrencyID = entity.CurrencyID,
                CurrentAccountCardID = entity.CurrentAccountCardID,
                EndDate = entity.EndDate,
                IsApproved = entity.IsApproved,
                StartDate = entity.StartDate,
                WarehouseID = entity.WarehouseID,
                Code = entity.Code,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DataOpenStatus = lockRow,
                DataOpenStatusUserId = userId,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                Id = entity.Id,
                IsActive = entity.IsActive,
                IsDeleted = entity.IsDeleted,
                LastModificationTime = entity.LastModificationTime.GetValueOrDefault(),
                LastModifierId = entity.LastModifierId.GetValueOrDefault(),
                Name = entity.Name,
            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id }, "");

            var purchasePricesDto = queryFactory.Update<SelectPurchasePricesDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectPurchasePricesDto>(purchasePricesDto);

        }

        public async Task<IDataResult<SelectPurchasePriceLinesDto>> GetDefinedProductPriceAsync(Guid productId, Guid currentAccountCardId, Guid currencyId, bool isApproved, DateTime date)
        {
            var queryLines = queryFactory
                   .Query()
                   .From(Tables.PurchasePriceLines)
                   .Select<PurchasePriceLines>(null)
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(PurchasePriceLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<Currencies>
                    (
                        cr => new { CurrencyID = cr.Id, CurrencyCode = cr.Code },
                        nameof(PurchasePriceLines.CurrencyID),
                        nameof(Currencies.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        cr => new { CurrentAccountCardID = cr.Id, CurrentAccountCardName = cr.Name },
                        nameof(PurchasePriceLines.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Join<PurchasePrices>
                    (
                        cr => new { CurrentAccountCardID = cr.Id, CurrentAccountCardName = cr.Name },
                        nameof(PurchasePriceLines.PurchasePriceID),
                        nameof(PurchasePrices.Id),
                        "PurchasePrices",
                        JoinType.Left
                    )
                    .Where(new
                    {
                        ProductID = productId,
                        CurrentAccountCardID = currentAccountCardId,
                        CurrencyID = currencyId,
                        IsApproved = isApproved
                    }, Tables.PurchasePriceLines)
                    .AndWhereRange("StartDate", "EndDate", date, Tables.PurchasePriceLines);

            var purchasePriceLine = queryFactory.Get<SelectPurchasePriceLinesDto>(queryLines);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectPurchasePriceLinesDto>(purchasePriceLine);
        }
    }
}
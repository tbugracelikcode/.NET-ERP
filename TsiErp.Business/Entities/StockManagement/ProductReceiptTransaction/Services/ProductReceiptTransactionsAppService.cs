using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
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
using TsiErp.Business.Entities.PurchaseOrdersAwaitingApproval.Services;
using TsiErp.Business.Entities.QualityControl.PurchaseQualityPlan.Services;
using TsiErp.Business.Entities.StockManagement.ProductReceiptTransaction.Validations;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseInvoice;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrder;
using TsiErp.Entities.Entities.QualityControl.PurchaseOrdersAwaitingApproval.Dtos;
using TsiErp.Entities.Entities.QualityControl.PurchaseOrdersAwaitingApprovalLine.Dtos;
using TsiErp.Entities.Entities.QualityControl.PurchaseQualityPlan.Dtos;
using TsiErp.Entities.Entities.QualityControl.PurchaseQualityPlanLine.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.Entities.StockManagement.ProductReceiptTransaction;
using TsiErp.Entities.Entities.StockManagement.ProductReceiptTransaction.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.ProductReceiptTransactions.Page;

namespace TsiErp.Business.Entities.ProductReceiptTransaction.Services
{
    [ServiceRegistration(typeof(IProductReceiptTransactionsAppService), DependencyInjectionType.Scoped)]
    public class ProductReceiptTransactionsAppService : ApplicationService<ProductReceiptTransactionsResource>, IProductReceiptTransactionsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;
        private readonly IPurchaseOrdersAwaitingApprovalsAppService _PurchaseOrdersAwaitingApprovalsAppService;
        private readonly INotificationsAppService _NotificationsAppService;
        private readonly INotificationTemplatesAppService _NotificationTemplatesAppService;
        private readonly IPurchaseQualityPlansAppService _PurchaseQualityPlansAppService;

        public ProductReceiptTransactionsAppService(IStringLocalizer<ProductReceiptTransactionsResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService, IPurchaseOrdersAwaitingApprovalsAppService purchaseOrdersAwaitingApprovalsAppService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService, IPurchaseQualityPlansAppService purchaseQualityPlansAppService) : base(l)
        {

            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
            _PurchaseOrdersAwaitingApprovalsAppService = purchaseOrdersAwaitingApprovalsAppService; ;
            _NotificationsAppService = notificationsAppService;
            _NotificationTemplatesAppService = notificationTemplatesAppService;
            _PurchaseQualityPlansAppService = purchaseQualityPlansAppService;
        }


        [ValidationAspect(typeof(CreateProductReceiptTransactionsValidator), Priority = 1)]
        public async Task<IDataResult<SelectProductReceiptTransactionsDto>> CreateAsync(CreateProductReceiptTransactionsDto input)
        {

            Guid addedEntityId = GuidGenerator.CreateGuid();
            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.ProductReceiptTransactions).Insert(new CreateProductReceiptTransactionsDto
            {
                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                ProductID = input.ProductID.GetValueOrDefault(),
                PurchaseOrderID = input.PurchaseOrderID.GetValueOrDefault(),
                ProductReceiptTransactionStateEnum = 1,
                PurchaseOrderLineID = input.PurchaseOrderLineID.GetValueOrDefault(),
                PurchaseOrderQuantity = input.PurchaseOrderQuantity,
                SupplierProductCode = input.SupplierProductCode,
                WarehouseReceiptQuantity = input.WarehouseReceiptQuantity,
                WaybillDate = input.WaybillDate,
                WaybillNo = input.WaybillNo,
                PartyNo = input.PartyNo,
                WaybillQuantity = input.WaybillQuantity,
                CreationTime = now,
                CreatorId = LoginedUserService.UserId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = Guid.Empty,
                DeletionTime = null,
                Description_ = input.Description_,
                Id = addedEntityId,
                IsDeleted = false,
                LastModificationTime = null,
                LastModifierId = Guid.Empty,
                Code = input.Code
            });

            var ProductReceiptTransactions = queryFactory.Insert<SelectProductReceiptTransactionsDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("ProductReceiptTransactionsChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.ProductReceiptTransactions, LogType.Insert, addedEntityId);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["ProductReceiptTransactionsChildMenu"], L["ProcessAdd"])).Data.FirstOrDefault();

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

            #region Onay Bekleyen Satın Alma Siparişleri Create İşlemi

            CreatePurchaseOrdersAwaitingApprovalsDto createOrderApprovalModel = new CreatePurchaseOrdersAwaitingApprovalsDto
            {
                PurchaseOrdersAwaitingApprovalStateEnum = 1,
                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                PurchaseOrderLineID = input.PurchaseOrderLineID.GetValueOrDefault(),
                PurchaseOrderID = input.PurchaseOrderID.GetValueOrDefault(),
                ProductID = input.ProductID.GetValueOrDefault(),
                ProductReceiptTransactionID = addedEntityId,
                Code = FicheNumbersAppService.GetFicheNumberAsync("PurchaseOrdersAwaitingApprovalsChildMenu")
            };

            createOrderApprovalModel.SelectPurchaseOrdersAwaitingApprovalLines = new List<SelectPurchaseOrdersAwaitingApprovalLinesDto>();

            SelectPurchaseQualityPlansDto purchaseQualityPlan = (await _PurchaseQualityPlansAppService.GetbyCurrentAccountandProductAsync(input.CurrentAccountCardID.GetValueOrDefault(), input.ProductID.GetValueOrDefault())).Data;

            if (purchaseQualityPlan != null && purchaseQualityPlan.Id != Guid.Empty && purchaseQualityPlan.SelectPurchaseQualityPlanLines != null && purchaseQualityPlan.SelectPurchaseQualityPlanLines.Count > 0)
            {
                foreach (var planLine in purchaseQualityPlan.SelectPurchaseQualityPlanLines)
                {
                    SelectPurchaseOrdersAwaitingApprovalLinesDto awaitingLineModel = new SelectPurchaseOrdersAwaitingApprovalLinesDto
                    {
                        BottomTolerance = planLine.BottomTolerance,
                        ControlFrequency = planLine.ControlFrequency,
                        ControlTypesID = planLine.ControlTypesID,
                        ControlTypesName = planLine.ControlTypesName,
                        IdealMeasure = planLine.IdealMeasure,
                        MeasureValue = 0,
                        UpperTolerance = planLine.UpperTolerance,
                        LineNr = createOrderApprovalModel.SelectPurchaseOrdersAwaitingApprovalLines.Count + 1,
                    };

                    createOrderApprovalModel.SelectPurchaseOrdersAwaitingApprovalLines.Add(awaitingLineModel);
                }
            }


            await _PurchaseOrdersAwaitingApprovalsAppService.CreateAsync(createOrderApprovalModel);

            await FicheNumbersAppService.UpdateFicheNumberAsync("PurchaseOrdersAwaitingApprovalsChildMenu", createOrderApprovalModel.Code);

            #endregion

            await Task.CompletedTask;
            return new SuccessDataResult<SelectProductReceiptTransactionsDto>(ProductReceiptTransactions);

        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            var entity = (await GetAsync(id)).Data;
            var query = queryFactory.Query().From(Tables.ProductReceiptTransactions).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

            var ProductReceiptTransactions = queryFactory.Update<SelectProductReceiptTransactionsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.ProductReceiptTransactions, LogType.Delete, id);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["ProductReceiptTransactionsChildMenu"], L["ProcessDelete"])).Data.FirstOrDefault();

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
            return new SuccessDataResult<SelectProductReceiptTransactionsDto>(ProductReceiptTransactions);

        }

        public async Task<IResult> DeletebyPurchaseOrderLineIDAsync(Guid purchaseOrderLineID)
        {
            var entity = (await GetbyPurchaseOrderLineIDAsync(purchaseOrderLineID)).Data;
            var query = queryFactory.Query().From(Tables.ProductReceiptTransactions).Delete(LoginedUserService.UserId).Where(new { PurchaseOrderLineID = purchaseOrderLineID }, "");

            var ProductReceiptTransactions = queryFactory.Update<SelectProductReceiptTransactionsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity.Id, entity.Id, LoginedUserService.UserId, Tables.ProductReceiptTransactions, LogType.Delete, entity.Id);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["ProductReceiptTransactionsChildMenu"], L["ProcessDelete"])).Data.FirstOrDefault();

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
            return new SuccessDataResult<SelectProductReceiptTransactionsDto>(ProductReceiptTransactions);

        }

        public async Task<IDataResult<SelectProductReceiptTransactionsDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                    .Query().From(Tables.ProductReceiptTransactions).Select<ProductReceiptTransactions>(null)
                    .Join<PurchaseOrders>
                        (
                            p => new { PurchaseOrderID = p.Id, PurchaseOrderFicheNo = p.FicheNo, PurchaseOrderDate = p.Date_ },
                            nameof(ProductReceiptTransactions.PurchaseOrderID),
                            nameof(PurchaseOrders.Id),
                            JoinType.Left
                        )
                         .Join<CurrentAccountCards>
                        (
                            ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                            nameof(ProductReceiptTransactions.CurrentAccountCardID),
                            nameof(CurrentAccountCards.Id),
                            JoinType.Left
                        )
                        .Join<Products>
                        (
                            p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                            nameof(ProductReceiptTransactions.ProductID),
                            nameof(Products.Id),
                            JoinType.Left
                        )

                        .Where(new { Id = id }, Tables.ProductReceiptTransactions);

            var ProductReceiptTransaction = queryFactory.Get<SelectProductReceiptTransactionsDto>(query);

            LogsAppService.InsertLogToDatabase(ProductReceiptTransaction, ProductReceiptTransaction, LoginedUserService.UserId, Tables.ProductReceiptTransactions, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectProductReceiptTransactionsDto>(ProductReceiptTransaction);

        }

        public async Task<IDataResult<SelectProductReceiptTransactionsDto>> GetbyPurchaseOrderLineIDAsync(Guid purchaseOrderLineID)
        {
            var query = queryFactory
                    .Query().From(Tables.ProductReceiptTransactions).Select<ProductReceiptTransactions>(null)
                    .Join<PurchaseOrders>
                        (
                            p => new { PurchaseOrderID = p.Id, PurchaseOrderFicheNo = p.FicheNo, PurchaseOrderDate = p.Date_ },
                            nameof(ProductReceiptTransactions.PurchaseOrderID),
                            nameof(PurchaseOrders.Id),
                            JoinType.Left
                        )
                         .Join<CurrentAccountCards>
                        (
                            ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                            nameof(ProductReceiptTransactions.CurrentAccountCardID),
                            nameof(CurrentAccountCards.Id),
                            JoinType.Left
                        )
                        .Join<Products>
                        (
                            p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                            nameof(ProductReceiptTransactions.ProductID),
                            nameof(Products.Id),
                            JoinType.Left
                        )

                        .Where(new { PurchaseOrderLineID = purchaseOrderLineID }, Tables.ProductReceiptTransactions);

            var ProductReceiptTransaction = queryFactory.Get<SelectProductReceiptTransactionsDto>(query);

            LogsAppService.InsertLogToDatabase(ProductReceiptTransaction, ProductReceiptTransaction, LoginedUserService.UserId, Tables.ProductReceiptTransactions, LogType.Get, ProductReceiptTransaction.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectProductReceiptTransactionsDto>(ProductReceiptTransaction);

        }

        public async Task<IDataResult<IList<ListProductReceiptTransactionsDto>>> GetListAsync(ListProductReceiptTransactionsParameterDto input)
        {
            var query = queryFactory
               .Query()
               .From(Tables.ProductReceiptTransactions).Select<ProductReceiptTransactions>(s => new { s.Description_, s.PartyNo, s.WaybillDate, s.WaybillQuantity, s.WarehouseReceiptQuantity, s.SupplierProductCode, s.WaybillNo, s.ProductReceiptTransactionStateEnum, s.PurchaseOrderQuantity, s.Id, s.Code })
                        .Join<PurchaseOrders>
                        (
                            p => new { PurchaseOrderID = p.Id, PurchaseOrderFicheNo = p.FicheNo, PurchaseOrderDate = p.Date_ },
                            nameof(ProductReceiptTransactions.PurchaseOrderID),
                            nameof(PurchaseOrders.Id),
                            JoinType.Left
                        )
                         .Join<CurrentAccountCards>
                        (
                            ca => new { CurrentAccountCardID = ca.Id, CurrentAccountCardCode = ca.Code, CurrentAccountCardName = ca.Name },
                            nameof(ProductReceiptTransactions.CurrentAccountCardID),
                            nameof(CurrentAccountCards.Id),
                            JoinType.Left
                        )
                        .Join<Products>
                        (
                            p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                            nameof(ProductReceiptTransactions.ProductID),
                            nameof(Products.Id),
                            JoinType.Left
                        )
               .Where(null, Tables.ProductReceiptTransactions);

            var productReceiptTransactions = queryFactory.GetList<ListProductReceiptTransactionsDto>(query).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListProductReceiptTransactionsDto>>(productReceiptTransactions);

        }

        [ValidationAspect(typeof(UpdateProductReceiptTransactionsValidator), Priority = 1)]
        public async Task<IDataResult<SelectProductReceiptTransactionsDto>> UpdateAsync(UpdateProductReceiptTransactionsDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.ProductReceiptTransactions).Select("*").Where(new { Id = input.Id }, "");
            var entity = queryFactory.Get<ProductReceiptTransactions>(entityQuery);

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.ProductReceiptTransactions).Update(new UpdateProductReceiptTransactionsDto
            {
                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                ProductID = input.ProductID.GetValueOrDefault(),
                Description_ = input.Description_,
                PurchaseOrderQuantity = input.PurchaseOrderQuantity,
                WaybillQuantity = input.WaybillQuantity,
                WaybillNo = input.WaybillNo,
                PartyNo = input.PartyNo,
                WaybillDate = input.WaybillDate,
                WarehouseReceiptQuantity = input.WarehouseReceiptQuantity,
                SupplierProductCode = input.SupplierProductCode,
                PurchaseOrderLineID = input.PurchaseOrderLineID.GetValueOrDefault(),
                ProductReceiptTransactionStateEnum = input.ProductReceiptTransactionStateEnum,
                PurchaseOrderID = input.PurchaseOrderID.GetValueOrDefault(),
                Id = input.Id,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                IsDeleted = entity.IsDeleted,
                LastModificationTime = now,
                LastModifierId = LoginedUserService.UserId,
                Code = input.Code,
            }).Where(new { Id = input.Id }, "");

            var ProductReceiptTransactions = queryFactory.Update<SelectProductReceiptTransactionsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, ProductReceiptTransactions, LoginedUserService.UserId, Tables.ProductReceiptTransactions, LogType.Update, entity.Id);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["ProductReceiptTransactionsChildMenu"], L["ProcessRefresh"])).Data.FirstOrDefault();

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
            return new SuccessDataResult<SelectProductReceiptTransactionsDto>(ProductReceiptTransactions);

        }

        public async Task<IDataResult<SelectProductReceiptTransactionsDto>> UpdateApproveIncomingQuantityAsync(UpdateProductReceiptTransactionsDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.ProductReceiptTransactions).Select("*").Where(new { Id = input.Id }, "");
            var entity = queryFactory.Get<ProductReceiptTransactions>(entityQuery);

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.ProductReceiptTransactions).Update(new UpdateProductReceiptTransactionsDto
            {
                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                ProductID = input.ProductID.GetValueOrDefault(),
                Description_ = input.Description_,
                PurchaseOrderQuantity = input.PurchaseOrderQuantity,
                WaybillQuantity = input.WaybillQuantity,
                WaybillNo = input.WaybillNo,
                PartyNo = input.PartyNo,
                WaybillDate = input.WaybillDate,
                WarehouseReceiptQuantity = input.WarehouseReceiptQuantity,
                SupplierProductCode = input.SupplierProductCode,
                PurchaseOrderLineID = input.PurchaseOrderLineID.GetValueOrDefault(),
                ProductReceiptTransactionStateEnum = input.ProductReceiptTransactionStateEnum,
                PurchaseOrderID = input.PurchaseOrderID.GetValueOrDefault(),
                Id = input.Id,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                IsDeleted = entity.IsDeleted,
                LastModificationTime = now,
                LastModifierId = LoginedUserService.UserId,
                Code = input.Code,
            }).Where(new { Id = input.Id }, "");

            var ProductReceiptTransactions = queryFactory.Update<SelectProductReceiptTransactionsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, ProductReceiptTransactions, LoginedUserService.UserId, Tables.ProductReceiptTransactions, LogType.Update, entity.Id);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessContextAsync(L["ProductReceiptTransactionsChildMenu"], L["ProductReceiptTransactionsContextApproveIncomingQuantity"])).Data.FirstOrDefault();

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
                                ContextMenuName_ = L["ProductReceiptTransactionsContextApproveIncomingQuantity"],
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
                            ContextMenuName_ = L["ProductReceiptTransactionsContextApproveIncomingQuantity"],
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
            return new SuccessDataResult<SelectProductReceiptTransactionsDto>(ProductReceiptTransactions);

        }

        public async Task<IDataResult<SelectProductReceiptTransactionsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.ProductReceiptTransactions).Select("*").Where(new { Id = id }, "");
            var entity = queryFactory.Get<ProductReceiptTransactions>(entityQuery);

            var query = queryFactory.Query().From(Tables.ProductReceiptTransactions).Update(new UpdateProductReceiptTransactionsDto
            {
                CurrentAccountCardID = entity.CurrentAccountCardID,
                ProductID = entity.ProductID,
                Description_ = entity.Description_,
                CreationTime = entity.CreationTime.Value,
                PartyNo = entity.PartyNo,
                PurchaseOrderQuantity = entity.PurchaseOrderQuantity,
                PurchaseOrderID = entity.PurchaseOrderID,
                ProductReceiptTransactionStateEnum = (int)entity.ProductReceiptTransactionStateEnum,
                PurchaseOrderLineID = entity.PurchaseOrderLineID,
                SupplierProductCode = entity.SupplierProductCode,
                WarehouseReceiptQuantity = entity.WarehouseReceiptQuantity,
                WaybillDate = entity.WaybillDate,
                WaybillNo = entity.WaybillNo,
                WaybillQuantity = entity.WaybillQuantity,
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

            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id }, "");

            var ProductReceiptTransactions = queryFactory.Update<SelectProductReceiptTransactionsDto>(query, "Id", true);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectProductReceiptTransactionsDto>(ProductReceiptTransactions);


        }
    }
}

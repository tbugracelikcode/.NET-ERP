using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Entities;
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
using TsiErp.Business.Entities.PurchaseUnsuitabilityReport.Validations;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrder;
using TsiErp.Entities.Entities.QualityControl.PurchaseUnsuitabilityReport;
using TsiErp.Entities.Entities.QualityControl.PurchaseUnsuitabilityReport.Dtos;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItem;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.PurchaseUnsuitabilityReports.Page;

namespace TsiErp.Business.Entities.PurchaseUnsuitabilityReport.Services
{
    [ServiceRegistration(typeof(IPurchaseUnsuitabilityReportsAppService), DependencyInjectionType.Scoped)]
    public class PurchaseUnsuitabilityReportsAppService : ApplicationService<PurchaseUnsuitabilityReportsResource>, IPurchaseUnsuitabilityReportsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;
        private readonly INotificationsAppService _NotificationsAppService;
        private readonly INotificationTemplatesAppService _NotificationTemplatesAppService;

        public PurchaseUnsuitabilityReportsAppService(IStringLocalizer<PurchaseUnsuitabilityReportsResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService; ;
            _NotificationsAppService = notificationsAppService;
            _NotificationTemplatesAppService = notificationTemplatesAppService;
        }

        [ValidationAspect(typeof(CreatePurchaseUnsuitabilityReportsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPurchaseUnsuitabilityReportsDto>> CreateAsync(CreatePurchaseUnsuitabilityReportsDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.PurchaseUnsuitabilityReports).Select("FicheNo").Where(new { FicheNo = input.FicheNo },  "");

            var list = queryFactory.ControlList<PurchaseUnsuitabilityReports>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();
            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.PurchaseUnsuitabilityReports).Insert(new CreatePurchaseUnsuitabilityReportsDto
            {
                FicheNo = input.FicheNo,
                CreationTime = now,
                CreatorId = LoginedUserService.UserId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = Guid.Empty,
                Date_ = input.Date_,
                DeletionTime = null,
                Id = addedEntityId,
                IsDeleted = false,
                LastModificationTime = null,
                LastModifierId = Guid.Empty,
                Action_ = input.Action_,
                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                Description_ = input.Description_,
                IsUnsuitabilityWorkOrder = input.IsUnsuitabilityWorkOrder,
                OrderID = input.OrderID.GetValueOrDefault(),
                PartyNo = input.PartyNo,
                ProductID = input.ProductID.GetValueOrDefault(),
                UnsuitableAmount = input.UnsuitableAmount,
                UnsuitabilityItemsID = input.UnsuitabilityItemsID.GetValueOrDefault()
            });


            var purchaseUnsuitabilityReport = queryFactory.Insert<SelectPurchaseUnsuitabilityReportsDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("PurchUnsRecordsChildMenu", input.FicheNo);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.PurchaseUnsuitabilityReports, LogType.Insert, addedEntityId);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["PurchUnsRecordsChildMenu"], L["ProcessAdd"])).Data.FirstOrDefault();

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
                                RecordNumber = input.FicheNo,
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
                            RecordNumber = input.FicheNo,
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
            return new SuccessDataResult<SelectPurchaseUnsuitabilityReportsDto>(purchaseUnsuitabilityReport);

        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var entity = (await GetAsync(id)).Data;
            var query = queryFactory.Query().From(Tables.PurchaseUnsuitabilityReports).Delete(LoginedUserService.UserId).Where(new { Id = id },  "");

            var purchaseUnsuitabilityReport = queryFactory.Update<SelectPurchaseUnsuitabilityReportsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.PurchaseUnsuitabilityReports, LogType.Delete, id);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["PurchUnsRecordsChildMenu"], L["ProcessDelete"])).Data.FirstOrDefault();

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
                                RecordNumber = entity.FicheNo,
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
                            RecordNumber = entity.FicheNo,
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
            return new SuccessDataResult<SelectPurchaseUnsuitabilityReportsDto>(purchaseUnsuitabilityReport);

        }

        public async Task<IDataResult<SelectPurchaseUnsuitabilityReportsDto>> GetAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.PurchaseUnsuitabilityReports).Select<PurchaseUnsuitabilityReports>(null)
                .Join<PurchaseOrders>
                (
                   d => new { OrderFicheNo = d.FicheNo }, nameof(PurchaseUnsuitabilityReports.OrderID), nameof(PurchaseOrders.Id), JoinType.Left
                )
                .Join<Products>
                (
                   d => new { ProductCode = d.Code, ProductName = d.Name }, nameof(PurchaseUnsuitabilityReports.ProductID), nameof(Products.Id), JoinType.Left
                )
                .Join<CurrentAccountCards>
                (
                   d => new { CurrentAccountCardCode = d.Code, CurrentAccountCardName = d.Name }, nameof(PurchaseUnsuitabilityReports.CurrentAccountCardID), nameof(CurrentAccountCards.Id), JoinType.Left
                )
                .Join<UnsuitabilityItems>
                (
                   d => new { UnsuitabilityItemsName = d.Name }, nameof(PurchaseUnsuitabilityReports.UnsuitabilityItemsID), nameof(UnsuitabilityItems.Id), JoinType.Left
                )
                .Where(null, Tables.PurchaseUnsuitabilityReports);

            var purchaseUnsuitabilityReport = queryFactory.Get<SelectPurchaseUnsuitabilityReportsDto>(query);

            LogsAppService.InsertLogToDatabase(purchaseUnsuitabilityReport, purchaseUnsuitabilityReport, LoginedUserService.UserId, Tables.PurchaseUnsuitabilityReports, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectPurchaseUnsuitabilityReportsDto>(purchaseUnsuitabilityReport);


        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListPurchaseUnsuitabilityReportsDto>>> GetListAsync(ListPurchaseUnsuitabilityReportsParameterDto input)
        {
            var query = queryFactory.Query().From(Tables.PurchaseUnsuitabilityReports).Select<PurchaseUnsuitabilityReports>(r => new { r.Id, r.FicheNo, r.PartyNo, r.Date_, r.Description_, r.UnsuitableAmount, r.Action_ })
                .Join<PurchaseOrders>
                (
                   d => new { OrderFicheNo = d.FicheNo }, 
                   nameof(PurchaseUnsuitabilityReports.OrderID), 
                   nameof(PurchaseOrders.Id), 
                   JoinType.Left
                )
                .Join<Products>
                (
                   d => new { ProductCode = d.Code, ProductName = d.Name }, nameof(PurchaseUnsuitabilityReports.ProductID), nameof(Products.Id), JoinType.Left
                )
                .Join<CurrentAccountCards>
                (
                   d => new { CurrentAccountCardCode = d.Code, CurrentAccountCardName = d.Name }, nameof(PurchaseUnsuitabilityReports.CurrentAccountCardID), nameof(CurrentAccountCards.Id), JoinType.Left
                )
                .Join<UnsuitabilityItems>
                (
                   d => new { UnsuitabilityItemsName = d.Name }, nameof(PurchaseUnsuitabilityReports.UnsuitabilityItemsID), nameof(UnsuitabilityItems.Id), JoinType.Left
                )
                .Where(null, Tables.PurchaseUnsuitabilityReports);

            var purchaseUnsuitabilityReports = queryFactory.GetList<ListPurchaseUnsuitabilityReportsDto>(query).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListPurchaseUnsuitabilityReportsDto>>(purchaseUnsuitabilityReports);


        }


        [ValidationAspect(typeof(UpdatePurchaseUnsuitabilityReportsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPurchaseUnsuitabilityReportsDto>> UpdateAsync(UpdatePurchaseUnsuitabilityReportsDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.PurchaseUnsuitabilityReports).Select("*").Where(new { Id = input.Id },  "");
            var entity = queryFactory.Get<PurchaseUnsuitabilityReports>(entityQuery);

            #region Update Control

            var listQuery = queryFactory.Query().From(Tables.PurchaseUnsuitabilityReports).Select("*").Where(new { FicheNo = input.FicheNo },  "");
            var list = queryFactory.GetList<PurchaseUnsuitabilityReports>(listQuery).ToList();

            if (list.Count > 0 && entity.FicheNo != input.FicheNo)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }

            #endregion

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.PurchaseUnsuitabilityReports).Update(new UpdatePurchaseUnsuitabilityReportsDto
            {
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
                Action_ = input.Action_,
                FicheNo = input.FicheNo,
                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                Date_ = input.Date_,
                Description_ = input.Description_,
                IsUnsuitabilityWorkOrder = input.IsUnsuitabilityWorkOrder,
                OrderID = input.OrderID.GetValueOrDefault(),
                PartyNo = input.PartyNo,
                ProductID = input.ProductID.GetValueOrDefault(),
                UnsuitableAmount = input.UnsuitableAmount,
                UnsuitabilityItemsID = input.UnsuitabilityItemsID.GetValueOrDefault()
            }).Where(new { Id = input.Id },  "");

            var purchaseUnsuitabilityReport = queryFactory.Update<SelectPurchaseUnsuitabilityReportsDto>(query, "Id", true);


            LogsAppService.InsertLogToDatabase(entity, purchaseUnsuitabilityReport, LoginedUserService.UserId, Tables.PurchaseUnsuitabilityReports, LogType.Update, entity.Id);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["PurchUnsRecordsChildMenu"], L["ProcessRefresh"])).Data.FirstOrDefault();

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
                                RecordNumber = input.FicheNo,
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
                            RecordNumber = input.FicheNo,
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
            return new SuccessDataResult<SelectPurchaseUnsuitabilityReportsDto>(purchaseUnsuitabilityReport);

        }

        public async Task<IDataResult<SelectPurchaseUnsuitabilityReportsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.PurchaseUnsuitabilityReports).Select("*").Where(new { Id = id },  "");
            var entity = queryFactory.Get<PurchaseUnsuitabilityReports>(entityQuery);

            var query = queryFactory.Query().From(Tables.PurchaseUnsuitabilityReports).Update(new UpdatePurchaseUnsuitabilityReportsDto
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
                Action_ = entity.Action_,
                UnsuitableAmount = entity.UnsuitableAmount,
                ProductID = entity.ProductID,
                PartyNo = entity.PartyNo,
                OrderID = entity.OrderID,
                IsUnsuitabilityWorkOrder = entity.IsUnsuitabilityWorkOrder,
                FicheNo = entity.FicheNo,
                Description_ = entity.Description_,
                Date_ = entity.Date_,
                CurrentAccountCardID = entity.CurrentAccountCardID,
                UnsuitabilityItemsID = entity.UnsuitabilityItemsID
            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id },  "");

            var purchaseUnsuitabilityReport = queryFactory.Update<SelectPurchaseUnsuitabilityReportsDto>(query, "Id", true);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectPurchaseUnsuitabilityReportsDto>(purchaseUnsuitabilityReport);


        }
    }
}

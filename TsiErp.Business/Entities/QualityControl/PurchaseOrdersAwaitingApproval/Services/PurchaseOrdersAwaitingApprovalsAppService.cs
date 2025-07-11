﻿using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
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
using TsiErp.Business.Entities.QualityControl.PurchaseQualityPlan.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrder;
using TsiErp.Entities.Entities.QualityControl.ControlType;
using TsiErp.Entities.Entities.QualityControl.PurchaseOrdersAwaitingApproval;
using TsiErp.Entities.Entities.QualityControl.PurchaseOrdersAwaitingApproval.Dtos;
using TsiErp.Entities.Entities.QualityControl.PurchaseOrdersAwaitingApprovalLine;
using TsiErp.Entities.Entities.QualityControl.PurchaseOrdersAwaitingApprovalLine.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.PurchaseOrdersAwaitingApprovals.Page;

namespace TsiErp.Business.Entities.PurchaseOrdersAwaitingApproval.Services
{
    [ServiceRegistration(typeof(IPurchaseOrdersAwaitingApprovalsAppService), DependencyInjectionType.Scoped)]
    public class PurchaseOrdersAwaitingApprovalsAppService : ApplicationService<PurchaseOrdersAwaitingApprovalsResource>, IPurchaseOrdersAwaitingApprovalsAppService
    {

        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private readonly IGetSQLDateAppService _GetSQLDateAppService;
        private readonly IPurchaseQualityPlansAppService _PurchaseQualityPlansAppService;
        private readonly INotificationsAppService _NotificationsAppService;
        private readonly INotificationTemplatesAppService _NotificationTemplatesAppService;

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }


        public PurchaseOrdersAwaitingApprovalsAppService(IStringLocalizer<PurchaseOrdersAwaitingApprovalsResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService, IPurchaseQualityPlansAppService purchaseQualityPlansAppService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
            _PurchaseQualityPlansAppService = purchaseQualityPlansAppService;
            _NotificationsAppService = notificationsAppService;
            _NotificationTemplatesAppService = notificationTemplatesAppService;
        }




        public async Task<IDataResult<SelectPurchaseOrdersAwaitingApprovalsDto>> CreateAsync(CreatePurchaseOrdersAwaitingApprovalsDto input)
        {

            Guid addedEntityId = GuidGenerator.CreateGuid();
            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.PurchaseOrdersAwaitingApprovals).Insert(new CreatePurchaseOrdersAwaitingApprovalsDto
            {
                ProductID = input.ProductID.GetValueOrDefault(),
                PurchaseOrdersAwaitingApprovalStateEnum = 1,
                ApprovedQuantity = input.ApprovedQuantity,
                Description_ = input.Description_,
                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                ProductReceiptTransactionID = input.ProductReceiptTransactionID.GetValueOrDefault(),
                PurchaseOrderID = input.PurchaseOrderID.GetValueOrDefault(),
                PurchaseOrderLineID = input.PurchaseOrderLineID.GetValueOrDefault(),
                CreationTime = now,
                ControlQuantity = 0,
                CreatorId = input.CreatorId != Guid.Empty ? input.CreatorId : LoginedUserService.UserId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                ApproverID = LoginedUserService.UserId,
                QualityApprovalDate = _GetSQLDateAppService.GetDateFromSQL(),
                DeleterId = Guid.Empty,
                SelectPurchaseOrdersAwaitingApprovalLines = new List<SelectPurchaseOrdersAwaitingApprovalLinesDto>(),
                DeletionTime = null,
                Id = addedEntityId,
                IsDeleted = false,
                LastModificationTime = null,
                LastModifierId = Guid.Empty,
                 Code = input.Code,
            });

            //foreach (var item in input.SelectPurchaseOrdersAwaitingApprovalLines)
            //{
            //    var queryLine = queryFactory.Query().From(Tables.PurchaseOrdersAwaitingApprovalLines).Insert(new CreatePurchaseOrdersAwaitingApprovalLinesDto
            //    {
            //        BottomTolerance = item.BottomTolerance,
            //        IdealMeasure = item.IdealMeasure,
            //        ControlFrequency = item.ControlFrequency,
            //        ControlTypesID = item.ControlTypesID.GetValueOrDefault(),
            //        MeasureValue = item.MeasureValue,
            //        UpperTolerance = item.UpperTolerance,
            //        LineNr = item.LineNr,
            //        PurchaseOrdersAwaitingApprovalID = addedEntityId,
            //        CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
            //        CreatorId = item.CreatorId != Guid.Empty ? item.CreatorId : LoginedUserService.UserId,
            //        DataOpenStatus = false,
            //        DataOpenStatusUserId = Guid.Empty,
            //        DeleterId = Guid.Empty,
            //        DeletionTime = null,
            //        Id = GuidGenerator.CreateGuid(),
            //        IsDeleted = false,
            //        LastModificationTime = null,
            //        LastModifierId = Guid.Empty
            //    });

            //    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
            //}

            var PurchaseOrdersAwaitingApproval = queryFactory.Insert<SelectPurchaseOrdersAwaitingApprovalsDto>(query, "Id", true); ;

            await FicheNumbersAppService.UpdateFicheNumberAsync("PurchaseOrdersAwaitingApprovalsChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.PurchaseOrdersAwaitingApprovals, LogType.Insert, addedEntityId);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["PurchaseOrdersAwaitingApprovalsChildMenu"], L["ProcessAdd"])).Data.FirstOrDefault();

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

            return new SuccessDataResult<SelectPurchaseOrdersAwaitingApprovalsDto>(PurchaseOrdersAwaitingApproval);

        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            var entity = (await GetAsync(id)).Data;
            var query = queryFactory.Query().From(Tables.PurchaseOrdersAwaitingApprovals).Select("*").Where(new { Id = id },  "");

            var PurchaseOrdersAwaitingApprovals = queryFactory.Get<SelectPurchaseOrdersAwaitingApprovalsDto>(query);

            if (PurchaseOrdersAwaitingApprovals.Id != Guid.Empty && PurchaseOrdersAwaitingApprovals != null)
            {
                var deleteQuery = queryFactory.Query().From(Tables.PurchaseOrdersAwaitingApprovals).Delete(LoginedUserService.UserId).Where(new { Id = id },  "");

                var lineDeleteQuery = queryFactory.Query().From(Tables.PurchaseOrdersAwaitingApprovalLines).Delete(LoginedUserService.UserId).Where(new { PurchaseOrdersAwaitingApprovalID = id },  "");

                deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                var purchaseOrdersAwaitingApproval = queryFactory.Update<SelectPurchaseOrdersAwaitingApprovalsDto>(deleteQuery, "Id", true);
                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.PurchaseOrdersAwaitingApprovals, LogType.Delete, id);
                #region Notification

                var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["PurchaseOrdersAwaitingApprovalsChildMenu"], L["ProcessDelete"])).Data.FirstOrDefault();

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
                return new SuccessDataResult<SelectPurchaseOrdersAwaitingApprovalsDto>(purchaseOrdersAwaitingApproval);
            }
            else
            {
                var queryLine = queryFactory.Query().From(Tables.PurchaseOrdersAwaitingApprovalLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");
                var purchaseOrdersAwaitingApprovalLines = queryFactory.Update<SelectPurchaseOrdersAwaitingApprovalLinesDto>(queryLine, "Id", true);
                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.PurchaseOrdersAwaitingApprovalLines, LogType.Delete, id);
                await Task.CompletedTask;
                return new SuccessDataResult<SelectPurchaseOrdersAwaitingApprovalLinesDto>(purchaseOrdersAwaitingApprovalLines);
            }

        }

        public async Task<IDataResult<SelectPurchaseOrdersAwaitingApprovalsDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.PurchaseOrdersAwaitingApprovals)
                   .Select<PurchaseOrdersAwaitingApprovals>(null)
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(PurchaseOrdersAwaitingApprovals.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        p => new { CurrentAccountCardID = p.Id, CurrentAccountCardCode = p.Code, CurrentAccountCardName = p.Name },
                        nameof(PurchaseOrdersAwaitingApprovals.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Join<PurchaseOrders>
                    (
                        p => new { PurchaseOrderID = p.Id, PurchaseOrderFicheNo = p.FicheNo, PurchaseOrderDate = p.Date_ },
                        nameof(PurchaseOrdersAwaitingApprovals.PurchaseOrderID),
                        nameof(PurchaseOrders.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = id }, Tables.PurchaseOrdersAwaitingApprovals);

            var purchaseOrdersAwaitingApprovals = queryFactory.Get<SelectPurchaseOrdersAwaitingApprovalsDto>(query);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.PurchaseOrdersAwaitingApprovalLines)
                   .Select<PurchaseOrdersAwaitingApprovalLines>(null)
                    .Join<ControlTypes>
                    (
                        p => new { ControlTypesID = p.Id, ControlTypesName = p.Name },
                        nameof(PurchaseOrdersAwaitingApprovalLines.ControlTypesID),
                        nameof(ControlTypes.Id),
                        JoinType.Left
                    )
                    .Where(new { PurchaseOrdersAwaitingApprovalID = id }, Tables.PurchaseOrdersAwaitingApprovalLines);

            var PurchaseOrdersAwaitingApprovalLine = queryFactory.GetList<SelectPurchaseOrdersAwaitingApprovalLinesDto>(queryLines).ToList();

            purchaseOrdersAwaitingApprovals.SelectPurchaseOrdersAwaitingApprovalLines = PurchaseOrdersAwaitingApprovalLine;

            LogsAppService.InsertLogToDatabase(purchaseOrdersAwaitingApprovals, purchaseOrdersAwaitingApprovals, LoginedUserService.UserId, Tables.PurchaseOrdersAwaitingApprovals, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectPurchaseOrdersAwaitingApprovalsDto>(purchaseOrdersAwaitingApprovals);

        }

        public async Task<IDataResult<IList<ListPurchaseOrdersAwaitingApprovalsDto>>> GetListAsync(ListPurchaseOrdersAwaitingApprovalsParameterDto input)
        {
            var query = queryFactory
                   .Query()
                    .From(Tables.PurchaseOrdersAwaitingApprovals)
                   .Select<PurchaseOrdersAwaitingApprovals>(s => new { s.Code, s.PurchaseOrdersAwaitingApprovalStateEnum, s.Id })
                    .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(PurchaseOrdersAwaitingApprovals.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        p => new { CurrentAccountCardID = p.Id, CurrentAccountCardCode = p.Code, CurrentAccountCardName = p.Name },
                        nameof(PurchaseOrdersAwaitingApprovals.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Join<PurchaseOrders>
                    (
                        p => new { PurchaseOrderID = p.Id, PurchaseOrderFicheNo = p.FicheNo, PurchaseOrderDate = p.Date_ },
                        nameof(PurchaseOrdersAwaitingApprovals.PurchaseOrderID),
                        nameof(PurchaseOrders.Id),
                        JoinType.Left
                    )
                    .Where(null,  Tables.PurchaseOrdersAwaitingApprovals);

            var purchaseOrdersAwaitingApprovals = queryFactory.GetList<ListPurchaseOrdersAwaitingApprovalsDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListPurchaseOrdersAwaitingApprovalsDto>>(purchaseOrdersAwaitingApprovals);

        }

        public async Task<IDataResult<SelectPurchaseOrdersAwaitingApprovalsDto>> UpdateAsync(UpdatePurchaseOrdersAwaitingApprovalsDto input)
        {
            var entityQuery = queryFactory
                   .Query()
                   .From(Tables.PurchaseOrdersAwaitingApprovals)
                   .Select<PurchaseOrdersAwaitingApprovals>(null)
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(PurchaseOrdersAwaitingApprovals.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        p => new { CurrentAccountCardID = p.Id, CurrentAccountCardCode = p.Code, CurrentAccountCardName = p.Name },
                        nameof(PurchaseOrdersAwaitingApprovals.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Join<PurchaseOrders>
                    (
                        p => new { PurchaseOrderID = p.Id, PurchaseOrderFicheNo = p.FicheNo, PurchaseOrderDate = p.Date_ },
                        nameof(PurchaseOrdersAwaitingApprovals.PurchaseOrderID),
                        nameof(PurchaseOrders.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = input.Id }, Tables.PurchaseOrdersAwaitingApprovals);

            var entity = queryFactory.Get<SelectPurchaseOrdersAwaitingApprovalsDto>(entityQuery);

            var queryLines = queryFactory
                   .Query()
                    .From(Tables.PurchaseOrdersAwaitingApprovalLines)
                   .Select<PurchaseOrdersAwaitingApprovalLines>(null)
                    .Join<ControlTypes>
                    (
                        p => new { ControlTypesID = p.Id, ControlTypesName = p.Name },
                        nameof(PurchaseOrdersAwaitingApprovalLines.ControlTypesID),
                        nameof(ControlTypes.Id),
                        JoinType.Left
                    )
                    .Where(new { PurchaseOrdersAwaitingApprovalID = input.Id },  Tables.PurchaseOrdersAwaitingApprovalLines);

            var PurchaseOrdersAwaitingApprovalLine = queryFactory.GetList<SelectPurchaseOrdersAwaitingApprovalLinesDto>(queryLines).ToList();

            entity.SelectPurchaseOrdersAwaitingApprovalLines = PurchaseOrdersAwaitingApprovalLine;

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.PurchaseOrdersAwaitingApprovals).Update(new UpdatePurchaseOrdersAwaitingApprovalsDto
            {
                ProductID = input.ProductID.GetValueOrDefault(),
                ApproverID = input.ApproverID.GetValueOrDefault(),
                PurchaseOrderLineID = input.PurchaseOrderLineID.GetValueOrDefault(),
                PurchaseOrderID = input.PurchaseOrderID.GetValueOrDefault(),
                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                QualityApprovalDate = input.QualityApprovalDate,
                ProductReceiptTransactionID = input.ProductReceiptTransactionID.GetValueOrDefault(),
                PurchaseOrdersAwaitingApprovalStateEnum = input.PurchaseOrdersAwaitingApprovalStateEnum,
                CreationTime = entity.CreationTime,
                ApprovedQuantity = input.ApprovedQuantity,
                Description_ = input.Description_,
                CreatorId = entity.CreatorId,
                ControlQuantity = input.ControlQuantity,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                Id = input.Id,
                IsDeleted = entity.IsDeleted,
                LastModificationTime = now,
                LastModifierId = LoginedUserService.UserId,
                 Code = input.Code,
            }).Where(new { Id = input.Id },  "");

            foreach (var item in input.SelectPurchaseOrdersAwaitingApprovalLines)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.PurchaseOrdersAwaitingApprovalLines).Insert(new CreatePurchaseOrdersAwaitingApprovalLinesDto
                    {
                        BottomTolerance = item.BottomTolerance,
                        IdealMeasure = item.IdealMeasure,
                        ControlFrequency = item.ControlFrequency,
                        ControlTypesID = item.ControlTypesID.GetValueOrDefault(),
                        MeasureValue = item.MeasureValue,
                        UpperTolerance = item.UpperTolerance,
                        LineNr = item.LineNr,
                        PurchaseOrdersAwaitingApprovalID = input.Id,
                        CreationTime =now,
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
                    var lineGetQuery = queryFactory.Query().From(Tables.PurchaseOrdersAwaitingApprovalLines).Select("*").Where(new { Id = item.Id },  "");

                    var line = queryFactory.Get<SelectPurchaseOrdersAwaitingApprovalLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.PurchaseOrdersAwaitingApprovalLines).Update(new UpdatePurchaseOrdersAwaitingApprovalLinesDto
                        {
                            BottomTolerance = item.BottomTolerance,
                            IdealMeasure = item.IdealMeasure,
                            MeasureValue = item.MeasureValue,
                            ControlTypesID = item.ControlTypesID.GetValueOrDefault(),
                            ControlFrequency = item.ControlFrequency,
                            UpperTolerance = item.UpperTolerance,
                            LineNr = item.LineNr,
                            PurchaseOrdersAwaitingApprovalID = input.Id,
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
                             
                        }).Where(new { Id = line.Id },  "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }

            var PurchaseOrdersAwaitingApproval = queryFactory.Update<SelectPurchaseOrdersAwaitingApprovalsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.PurchaseOrdersAwaitingApprovals, LogType.Update, PurchaseOrdersAwaitingApproval.Id);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["PurchaseOrdersAwaitingApprovalsChildMenu"], L["ProcessRefresh"])).Data.FirstOrDefault();

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
            return new SuccessDataResult<SelectPurchaseOrdersAwaitingApprovalsDto>(PurchaseOrdersAwaitingApproval);

        }

        public async Task<IDataResult<SelectPurchaseOrdersAwaitingApprovalsDto>> UpdateCancelQCApprovalAsync(UpdatePurchaseOrdersAwaitingApprovalsDto input)
        {
            var entityQuery = queryFactory
                   .Query()
                   .From(Tables.PurchaseOrdersAwaitingApprovals)
                   .Select<PurchaseOrdersAwaitingApprovals>(null)
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(PurchaseOrdersAwaitingApprovals.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        p => new { CurrentAccountCardID = p.Id, CurrentAccountCardCode = p.Code, CurrentAccountCardName = p.Name },
                        nameof(PurchaseOrdersAwaitingApprovals.CurrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Join<PurchaseOrders>
                    (
                        p => new { PurchaseOrderID = p.Id, PurchaseOrderFicheNo = p.FicheNo, PurchaseOrderDate = p.Date_ },
                        nameof(PurchaseOrdersAwaitingApprovals.PurchaseOrderID),
                        nameof(PurchaseOrders.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = input.Id }, Tables.PurchaseOrdersAwaitingApprovals);

            var entity = queryFactory.Get<SelectPurchaseOrdersAwaitingApprovalsDto>(entityQuery);

            var queryLines = queryFactory
                   .Query()
                    .From(Tables.PurchaseOrdersAwaitingApprovalLines)
                   .Select<PurchaseOrdersAwaitingApprovalLines>(null)
                    .Join<ControlTypes>
                    (
                        p => new { ControlTypesID = p.Id, ControlTypesName = p.Name },
                        nameof(PurchaseOrdersAwaitingApprovalLines.ControlTypesID),
                        nameof(ControlTypes.Id),
                        JoinType.Left
                    )
                    .Where(new { PurchaseOrdersAwaitingApprovalID = input.Id }, Tables.PurchaseOrdersAwaitingApprovalLines);

            var PurchaseOrdersAwaitingApprovalLine = queryFactory.GetList<SelectPurchaseOrdersAwaitingApprovalLinesDto>(queryLines).ToList();

            entity.SelectPurchaseOrdersAwaitingApprovalLines = PurchaseOrdersAwaitingApprovalLine;

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.PurchaseOrdersAwaitingApprovals).Update(new UpdatePurchaseOrdersAwaitingApprovalsDto
            {
                ProductID = input.ProductID.GetValueOrDefault(),
                ApproverID = input.ApproverID.GetValueOrDefault(),
                PurchaseOrderLineID = input.PurchaseOrderLineID.GetValueOrDefault(),
                PurchaseOrderID = input.PurchaseOrderID.GetValueOrDefault(),
                CurrentAccountCardID = input.CurrentAccountCardID.GetValueOrDefault(),
                QualityApprovalDate = input.QualityApprovalDate,
                ProductReceiptTransactionID = input.ProductReceiptTransactionID.GetValueOrDefault(),
                PurchaseOrdersAwaitingApprovalStateEnum = input.PurchaseOrdersAwaitingApprovalStateEnum,
                CreationTime = entity.CreationTime,
                ApprovedQuantity = input.ApprovedQuantity,
                Description_ = input.Description_,
                CreatorId = entity.CreatorId,
                ControlQuantity = input.ControlQuantity,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                Id = input.Id,
                IsDeleted = entity.IsDeleted,
                LastModificationTime = now,
                LastModifierId = LoginedUserService.UserId,
                Code = input.Code,
            }).Where(new { Id = input.Id }, "");

            foreach (var item in input.SelectPurchaseOrdersAwaitingApprovalLines)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.PurchaseOrdersAwaitingApprovalLines).Insert(new CreatePurchaseOrdersAwaitingApprovalLinesDto
                    {
                        BottomTolerance = item.BottomTolerance,
                        IdealMeasure = item.IdealMeasure,
                        ControlFrequency = item.ControlFrequency,
                        ControlTypesID = item.ControlTypesID.GetValueOrDefault(),
                        MeasureValue = item.MeasureValue,
                        UpperTolerance = item.UpperTolerance,
                        LineNr = item.LineNr,
                        PurchaseOrdersAwaitingApprovalID = input.Id,
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

                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.PurchaseOrdersAwaitingApprovalLines).Select("*").Where(new { Id = item.Id }, "");

                    var line = queryFactory.Get<SelectPurchaseOrdersAwaitingApprovalLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.PurchaseOrdersAwaitingApprovalLines).Update(new UpdatePurchaseOrdersAwaitingApprovalLinesDto
                        {
                            BottomTolerance = item.BottomTolerance,
                            IdealMeasure = item.IdealMeasure,
                            MeasureValue = item.MeasureValue,
                            ControlTypesID = item.ControlTypesID.GetValueOrDefault(),
                            ControlFrequency = item.ControlFrequency,
                            UpperTolerance = item.UpperTolerance,
                            LineNr = item.LineNr,
                            PurchaseOrdersAwaitingApprovalID = input.Id,
                            CreationTime = line.CreationTime,
                            CreatorId = line.CreatorId,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = line.DeleterId.GetValueOrDefault(),
                            DeletionTime = line.DeletionTime.GetValueOrDefault(),
                            Id = item.Id,
                            IsDeleted = item.IsDeleted,
                            LastModificationTime =now,
                            LastModifierId = LoginedUserService.UserId,

                        }).Where(new { Id = line.Id }, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }

            var PurchaseOrdersAwaitingApproval = queryFactory.Update<SelectPurchaseOrdersAwaitingApprovalsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.PurchaseOrdersAwaitingApprovals, LogType.Update, PurchaseOrdersAwaitingApproval.Id);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessContextAsync(L["PurchaseOrdersAwaitingApprovalsChildMenu"],  L["PurchaseOrdersAwaitingApprovalsContextQualityApprovalCancel"])).Data.FirstOrDefault();

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
                                ContextMenuName_ = L["PurchaseOrdersAwaitingApprovalsContextQualityApprovalCancel"],
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
                            ContextMenuName_ = L["PurchaseOrdersAwaitingApprovalsContextQualityApprovalCancel"],
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
            return new SuccessDataResult<SelectPurchaseOrdersAwaitingApprovalsDto>(PurchaseOrdersAwaitingApproval);

        }

        public async Task<IDataResult<SelectPurchaseOrdersAwaitingApprovalsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.PurchaseOrdersAwaitingApprovals).Select("*").Where(new { Id = id }, "");

            var entity = queryFactory.Get<PurchaseOrdersAwaitingApprovals>(entityQuery);

            var query = queryFactory.Query().From(Tables.PurchaseOrdersAwaitingApprovals).Update(new UpdatePurchaseOrdersAwaitingApprovalsDto
            {
                ProductID = entity.ProductID,
                ApprovedQuantity = entity.ApprovedQuantity,
                Description_ = entity.Description_,
                QualityApprovalDate = entity.QualityApprovalDate,
                ApproverID = entity.ApproverID,
                CreationTime = entity.CreationTime.Value,
                ControlQuantity = entity.ControlQuantity,
                CreatorId = entity.CreatorId.Value,
                DataOpenStatus = lockRow,
                DataOpenStatusUserId = userId,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                Id = entity.Id,
                IsDeleted = entity.IsDeleted,
                PurchaseOrdersAwaitingApprovalStateEnum = (int)entity.PurchaseOrdersAwaitingApprovalStateEnum,
                ProductReceiptTransactionID = entity.ProductReceiptTransactionID,
                CurrentAccountCardID = entity.CurrentAccountCardID,
                PurchaseOrderID = entity.PurchaseOrderID,
                PurchaseOrderLineID = entity.PurchaseOrderLineID,
                LastModificationTime = entity.LastModificationTime.GetValueOrDefault(),
                LastModifierId = entity.LastModifierId.GetValueOrDefault(),
                 Code = entity.Code,
            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id }, "");

            var PurchaseOrdersAwaitingApprovalsDto = queryFactory.Update<SelectPurchaseOrdersAwaitingApprovalsDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectPurchaseOrdersAwaitingApprovalsDto>(PurchaseOrdersAwaitingApprovalsDto);


        }




    }
}

﻿using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.ProductionOrderChangeReport.Validations;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.QualityControl.ProductionOrderChangeReport;
using TsiErp.Entities.Entities.QualityControl.ProductionOrderChangeReport.Dtos;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItem;
using TsiErp.Entities.Entities.SalesManagement.SalesOrder;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.ProductionOrderChangeReports.Page;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder;
using TSI.QueryBuilder.Models;
using TsiErp.Business.Entities.GeneralSystemIdentifications.NotificationTemplate.Services;
using TsiErp.Business.Entities.Other.Notification.Services;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using Tsi.Core.Entities;

namespace TsiErp.Business.Entities.ProductionOrderChangeReport.Services
{
    [ServiceRegistration(typeof(IProductionOrderChangeReportsAppService), DependencyInjectionType.Scoped)]
    public class ProductionOrderChangeReportsAppService : ApplicationService<ProductionOrderChangeReportsResource>, IProductionOrderChangeReportsAppService
    {

        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;
        private readonly INotificationsAppService _NotificationsAppService;
        private readonly INotificationTemplatesAppService _NotificationTemplatesAppService;

        public ProductionOrderChangeReportsAppService(IStringLocalizer<ProductionOrderChangeReportsResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
            _NotificationsAppService = notificationsAppService;
            _NotificationTemplatesAppService = notificationTemplatesAppService;
        }




        [ValidationAspect(typeof(CreateProductionOrderChangeReportsValidator), Priority = 1)]
        public async Task<IDataResult<SelectProductionOrderChangeReportsDto>> CreateAsync(CreateProductionOrderChangeReportsDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.ProductionOrderChangeReports).Select("FicheNo").Where(new { FicheNo = input.FicheNo }, "");

            var list = queryFactory.ControlList<ProductionOrderChangeReports>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();
            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.ProductionOrderChangeReports).Insert(new CreateProductionOrderChangeReportsDto
            {
                SalesOrderID = input.SalesOrderID.GetValueOrDefault(),
                ProductID = input.ProductID.GetValueOrDefault(),
                FicheNo = input.FicheNo,
                ProductionOrderID = input.ProductionOrderID.GetValueOrDefault(),
                Action_ = input.Action_,
                Date_ = input.Date_,
                Description_ = input.Description_,
                LinkedProductionOrderID = input.LinkedProductionOrderID.GetValueOrDefault(),
                UnsuitabilityItemsID = input.UnsuitabilityItemsID.GetValueOrDefault(),
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


            var ProductionOrderChangeReport = queryFactory.Insert<SelectProductionOrderChangeReportsDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("ProdOrderChangeRecordsChildMenu", input.FicheNo);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.ProductionOrderChangeReports, LogType.Insert, addedEntityId);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["ProdOrderChangeRecordsChildMenu"], L["ProcessAdd"])).Data.FirstOrDefault();

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
            return new SuccessDataResult<SelectProductionOrderChangeReportsDto>(ProductionOrderChangeReport);

        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            var entity = (await GetAsync(id)).Data;
            var query = queryFactory.Query().From(Tables.ProductionOrderChangeReports).Delete(LoginedUserService.UserId).Where(new { Id = id },  "");

            var ProductionOrderChangeReport = queryFactory.Update<SelectProductionOrderChangeReportsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.ProductionOrderChangeReports, LogType.Delete, id);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["ProdOrderChangeRecordsChildMenu"], L["ProcessDelete"])).Data.FirstOrDefault();

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
            return new SuccessDataResult<SelectProductionOrderChangeReportsDto>(ProductionOrderChangeReport);

        }

        public async Task<IDataResult<SelectProductionOrderChangeReportsDto>> GetAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.ProductionOrderChangeReports).Select<ProductionOrderChangeReports>(null)
                .Join<SalesOrders>
                (
                   d => new { SalesOrderFicheNo = d.FicheNo, SalesOrderID = d.Id }, nameof(ProductionOrderChangeReports.SalesOrderID), nameof(SalesOrders.Id), JoinType.Left
                )
                .Join<Products>
                (
                   d => new { ProductName = d.Name, ProductCode = d.Code, ProductID = d.Id }, nameof(ProductionOrderChangeReports.ProductID), nameof(Products.Id), JoinType.Left
                )
                .Join<UnsuitabilityItems>
                (
                   d => new { UnsuitabilityItemsName = d.Name, UnsuitabilityItemsID = d.Id }, nameof(ProductionOrderChangeReports.UnsuitabilityItemsID), nameof(UnsuitabilityItems.Id), JoinType.Left
                )
                .Join<ProductionOrders>
                (
                   d => new { ProductionOrderFicheNo = d.FicheNo, ProductionOrderID = d.Id }, nameof(ProductionOrderChangeReports.ProductionOrderID), nameof(ProductionOrders.Id), JoinType.Left
                )
                .Where(new { Id = id }, Tables.ProductionOrderChangeReports);

            var ProductionOrderChangeReport = queryFactory.Get<SelectProductionOrderChangeReportsDto>(query);

            LogsAppService.InsertLogToDatabase(ProductionOrderChangeReport, ProductionOrderChangeReport, LoginedUserService.UserId, Tables.ProductionOrderChangeReports, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectProductionOrderChangeReportsDto>(ProductionOrderChangeReport);

        }

        public async Task<IDataResult<IList<ListProductionOrderChangeReportsDto>>> GetListAsync(ListProductionOrderChangeReportsParameterDto input)
        {
            var query = queryFactory.Query().From(Tables.ProductionOrderChangeReports).Select<ProductionOrderChangeReports>(s => new { s.FicheNo, s.Date_, s.Id })
                .Join<SalesOrders>
                (
                   d => new { SalesOrderFicheNo = d.FicheNo, SalesOrderID = d.Id }, nameof(ProductionOrderChangeReports.SalesOrderID), nameof(SalesOrders.Id), JoinType.Left
                )
                .Join<Products>
                (
                   d => new { ProductName = d.Name, ProductCode = d.Code, ProductID = d.Id }, nameof(ProductionOrderChangeReports.ProductID), nameof(Products.Id), JoinType.Left
                )
                .Join<UnsuitabilityItems>
                (
                   d => new { UnsuitabilityItemsName = d.Name, UnsuitabilityItemsID = d.Id }, nameof(ProductionOrderChangeReports.UnsuitabilityItemsID), nameof(UnsuitabilityItems.Id), JoinType.Left
                )
                .Join<ProductionOrders>
                (
                   d => new { ProductionOrderFicheNo = d.FicheNo, ProductionOrderID = d.Id }, nameof(ProductionOrderChangeReports.ProductionOrderID), nameof(ProductionOrders.Id), JoinType.Left
                )
                .Where(null, Tables.ProductionOrderChangeReports);

            var productionOrderChangeReports = queryFactory.GetList<ListProductionOrderChangeReportsDto>(query).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListProductionOrderChangeReportsDto>>(productionOrderChangeReports);


        }

        [ValidationAspect(typeof(UpdateProductionOrderChangeReportsValidator), Priority = 1)]
        public async Task<IDataResult<SelectProductionOrderChangeReportsDto>> UpdateAsync(UpdateProductionOrderChangeReportsDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.ProductionOrderChangeReports).Select("*").Where(new { Id = input.Id }, "");
            var entity = queryFactory.Get<ProductionOrderChangeReports>(entityQuery);

            #region Update Control

            var listQuery = queryFactory.Query().From(Tables.ProductionOrderChangeReports).Select("*").Where(new { FicheNo = input.FicheNo },  "");
            var list = queryFactory.GetList<ProductionOrderChangeReports>(listQuery).ToList();

            if (list.Count > 0 && entity.FicheNo != input.FicheNo)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }

            #endregion

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.ProductionOrderChangeReports).Update(new UpdateProductionOrderChangeReportsDto
            {
                SalesOrderID = input.SalesOrderID.GetValueOrDefault(),
                ProductID = input.ProductID.GetValueOrDefault(),
                FicheNo = input.FicheNo,
                ProductionOrderID = input.ProductionOrderID.GetValueOrDefault(),
                UnsuitabilityItemsID = input.UnsuitabilityItemsID.GetValueOrDefault(),
                LinkedProductionOrderID = input.LinkedProductionOrderID.GetValueOrDefault(),
                Description_ = input.Description_,
                Date_ = input.Date_,
                Action_ = input.Action_,
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
            }).Where(new { Id = input.Id },"");

            var ProductionOrderChangeReport = queryFactory.Update<SelectProductionOrderChangeReportsDto>(query, "Id", true);


            LogsAppService.InsertLogToDatabase(entity, ProductionOrderChangeReport, LoginedUserService.UserId, Tables.ProductionOrderChangeReports, LogType.Update, entity.Id);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["ProdOrderChangeRecordsChildMenu"], L["ProcessRefresh"])).Data.FirstOrDefault();

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
            return new SuccessDataResult<SelectProductionOrderChangeReportsDto>(ProductionOrderChangeReport);

        }

        public async Task<IDataResult<SelectProductionOrderChangeReportsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.ProductionOrderChangeReports).Select("*").Where(new { Id = id }, "");
            var entity = queryFactory.Get<ProductionOrderChangeReports>(entityQuery);

            var query = queryFactory.Query().From(Tables.ProductionOrderChangeReports).Update(new UpdateProductionOrderChangeReportsDto
            {
                SalesOrderID = entity.SalesOrderID,
                ProductID = entity.ProductID,
                Description_ = entity.Description_,
                Action_ = entity.Action_,
                Date_ = entity.Date_,
                LinkedProductionOrderID = entity.LinkedProductionOrderID,
                UnsuitabilityItemsID = entity.UnsuitabilityItemsID,
                ProductionOrderID = entity.ProductionOrderID,
                FicheNo = entity.FicheNo,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                IsDeleted = entity.IsDeleted,
                LastModificationTime = entity.LastModificationTime.GetValueOrDefault(),
                LastModifierId = entity.LastModifierId.GetValueOrDefault(),
                Id = id,
                DataOpenStatus = lockRow,
                DataOpenStatusUserId = userId
            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id }, "");

            var ProductionOrderChangeReport = queryFactory.Update<SelectProductionOrderChangeReportsDto>(query, "Id", true);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectProductionOrderChangeReportsDto>(ProductionOrderChangeReport);

        }
    }
}

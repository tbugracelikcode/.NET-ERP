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
using TsiErp.Business.Entities.CustomerComplaintReport.Validations;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.NotificationTemplate.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.Other.Notification.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.Entities.QualityControl.CustomerComplaintReport;
using TsiErp.Entities.Entities.QualityControl.CustomerComplaintReport.Dtos;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItem;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityTypesItem.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesOrder;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.CustomerComplaintReports.Page;

namespace TsiErp.Business.Entities.CustomerComplaintReport.Services
{
    [ServiceRegistration(typeof(ICustomerComplaintReportsAppService), DependencyInjectionType.Scoped)]
    public class CustomerComplaintReportsAppService : ApplicationService<CustomerComplaintReportsResource>, ICustomerComplaintReportsAppService
    {

        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;
        private readonly INotificationsAppService _NotificationsAppService;
        private readonly INotificationTemplatesAppService _NotificationTemplatesAppService;

        public CustomerComplaintReportsAppService(IStringLocalizer<CustomerComplaintReportsResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService; ;
            _NotificationsAppService = notificationsAppService;
            _NotificationTemplatesAppService = notificationTemplatesAppService;
        }




        [ValidationAspect(typeof(CreateCustomerComplaintReportsValidator), Priority = 1)]
        public async Task<IDataResult<SelectCustomerComplaintReportsDto>> CreateAsync(CreateCustomerComplaintReportsDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.CustomerComplaintReports).Select("ReportNo").Where(new { ReportNo = input.ReportNo }, "");

            var list = queryFactory.ControlList<CustomerComplaintReports>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();
            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.CustomerComplaintReports).Insert(new CreateCustomerComplaintReportsDto
            {
                ReportNo = input.ReportNo,
                UnsuitqabilityItemsID = input.UnsuitqabilityItemsID.GetValueOrDefault(),
                SalesOrderID = input.SalesOrderID.GetValueOrDefault(),
                ProductID = input.ProductID.GetValueOrDefault(),
                DefectedQuantity = input.DefectedQuantity,
                DeliveredQuantity = input.DeliveredQuantity,
                Description_ = input.Description_,
                ProductionReferanceNumber = input.ProductionReferanceNumber,
                Domain_ = input.Domain_,
                FilePath = input.FilePath,
                is8DReport = input.is8DReport,
                ReportDate = input.ReportDate,
                ReportResult = input.ReportResult,
                ReportState = input.ReportState,
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


            var CustomerComplaintReport = queryFactory.Insert<SelectCustomerComplaintReportsDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("CustCompRecordsChildMenu", input.ReportNo);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.CustomerComplaintReports, LogType.Insert, addedEntityId);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["CustCompRecordsChildMenu"], L["ProcessAdd"])).Data.FirstOrDefault();

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
                                RecordNumber = string.Empty,
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
                            RecordNumber = string.Empty,
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
            return new SuccessDataResult<SelectCustomerComplaintReportsDto>(CustomerComplaintReport);

        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            var entity = (await GetAsync(id)).Data;
            var query = queryFactory.Query().From(Tables.CustomerComplaintReports).Delete(LoginedUserService.UserId).Where(new { Id = id },  "");

            var CustomerComplaintReport = queryFactory.Update<SelectCustomerComplaintReportsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.CustomerComplaintReports, LogType.Delete, id);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["CustCompRecordsChildMenu"], L["ProcessDelete"])).Data.FirstOrDefault();

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
                                RecordNumber = string.Empty,
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
                            RecordNumber = string.Empty,
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
            return new SuccessDataResult<SelectCustomerComplaintReportsDto>(CustomerComplaintReport);

        }

        public async Task<IDataResult<SelectCustomerComplaintReportsDto>> GetAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.CustomerComplaintReports).Select<CustomerComplaintReports>(null)
                .Join<SalesOrders>
                (
                   d => new { SalesOrderFicheNo = d.FicheNo, SalesOrderID = d.Id}, nameof(CustomerComplaintReports.SalesOrderID), nameof(SalesOrders.Id), JoinType.Left
                )
                .Join<Products>
                (
                   d => new { ProductName = d.Name, ProductCode = d.Code, ProductID = d.Id }, nameof(CustomerComplaintReports.ProductID), nameof(Products.Id), JoinType.Left
                )
                .Join<UnsuitabilityItems>
                (
                   d => new { UnsuitqabilityItemsName = d.Name, UnsuitqabilityItemsID = d.Id }, nameof(CustomerComplaintReports.UnsuitqabilityItemsID), nameof(UnsuitabilityItems.Id), JoinType.Left
                )
                .Where(new { Id = id },  Tables.CustomerComplaintReports);

            var CustomerComplaintReport = queryFactory.Get<SelectCustomerComplaintReportsDto>(query);

            LogsAppService.InsertLogToDatabase(CustomerComplaintReport, CustomerComplaintReport, LoginedUserService.UserId, Tables.CustomerComplaintReports, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectCustomerComplaintReportsDto>(CustomerComplaintReport);

        }

        public async Task<IDataResult<IList<ListCustomerComplaintReportsDto>>> GetListAsync(ListCustomerComplaintReportsParameterDto input)
        {
            var query = queryFactory.Query().From(Tables.CustomerComplaintReports).Select<CustomerComplaintReports>(s => new { s.ReportNo, s.ReportDate, s.ReportState, s.Id })
                .Join<SalesOrders>
                (
                   d => new { SalesOrderFicheNo = d.FicheNo, SalesOrderID = d.Id }, nameof(CustomerComplaintReports.SalesOrderID), nameof(SalesOrders.Id), JoinType.Left
                )
                .Join<Products>
                (
                   d => new { ProductName = d.Name, ProductCode = d.Code, ProductID = d.Id }, nameof(CustomerComplaintReports.ProductID), nameof(Products.Id), JoinType.Left
                )
                .Join<UnsuitabilityItems>
                (
                   d => new { UnsuitqabilityItemsName = d.Name, UnsuitqabilityItemsID = d.Id }, nameof(CustomerComplaintReports.UnsuitqabilityItemsID), nameof(UnsuitabilityItems.Id), JoinType.Left
                )
                .Where(null, Tables.CustomerComplaintReports);

            var customerComplaintReports = queryFactory.GetList<ListCustomerComplaintReportsDto>(query).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListCustomerComplaintReportsDto>>(customerComplaintReports);


        }

        [ValidationAspect(typeof(UpdateCustomerComplaintReportsValidator), Priority = 1)]
        public async Task<IDataResult<SelectCustomerComplaintReportsDto>> UpdateAsync(UpdateCustomerComplaintReportsDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.CustomerComplaintReports).Select("*").Where(new { Id = input.Id }, "");
            var entity = queryFactory.Get<CustomerComplaintReports>(entityQuery);

            #region Update Control

            var listQuery = queryFactory.Query().From(Tables.CustomerComplaintReports).Select("*").Where(new { ReportNo = input.ReportNo },  "");
            var list = queryFactory.GetList<CustomerComplaintReports>(listQuery).ToList();

            if (list.Count > 0 && entity.ReportNo != input.ReportNo)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }

            #endregion

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.CustomerComplaintReports).Update(new UpdateCustomerComplaintReportsDto
            {
                ReportNo = input.ReportNo,
                UnsuitqabilityItemsID = input.UnsuitqabilityItemsID.GetValueOrDefault(),
                SalesOrderID = input.SalesOrderID.GetValueOrDefault(),
                ProductID = input.ProductID.GetValueOrDefault(),
                DefectedQuantity = input.DefectedQuantity,
                DeliveredQuantity = input.DeliveredQuantity,
                Description_ = input.Description_,
                ProductionReferanceNumber = input.ProductionReferanceNumber,
                Domain_ = input.Domain_,
                FilePath = input.FilePath,
                is8DReport = input.is8DReport,
                ReportDate = input.ReportDate,
                ReportResult = input.ReportResult,
                ReportState = input.ReportState,
                Id = input.Id,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                IsDeleted = entity.IsDeleted,
                LastModificationTime =now,
                LastModifierId = LoginedUserService.UserId,
            }).Where(new { Id = input.Id },"");

            var CustomerComplaintReport = queryFactory.Update<SelectCustomerComplaintReportsDto>(query, "Id", true);


            LogsAppService.InsertLogToDatabase(entity, CustomerComplaintReport, LoginedUserService.UserId, Tables.CustomerComplaintReports, LogType.Update, entity.Id);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["CustCompRecordsChildMenu"], L["ProcessRefresh"])).Data.FirstOrDefault();

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
                                RecordNumber = string.Empty,
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
                            RecordNumber = string.Empty,
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
            return new SuccessDataResult<SelectCustomerComplaintReportsDto>(CustomerComplaintReport);

        }

        public async Task<IDataResult<SelectCustomerComplaintReportsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.CustomerComplaintReports).Select("*").Where(new { Id = id }, "");
            var entity = queryFactory.Get<CustomerComplaintReports>(entityQuery);

            var query = queryFactory.Query().From(Tables.CustomerComplaintReports).Update(new UpdateCustomerComplaintReportsDto
            {
                ReportNo = entity.ReportNo,
                UnsuitqabilityItemsID = entity.UnsuitqabilityItemsID,
                SalesOrderID = entity.SalesOrderID,
                ProductID = entity.ProductID,
                DefectedQuantity = entity.DefectedQuantity,
                DeliveredQuantity = entity.DeliveredQuantity,
                Description_ = entity.Description_,
                ProductionReferanceNumber = entity.ProductionReferanceNumber,
                Domain_ = entity.Domain_,
                FilePath = entity.FilePath,
                is8DReport = entity.is8DReport,
                ReportDate = entity.ReportDate,
                ReportResult = entity.ReportResult,
                ReportState = entity.ReportState,
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
            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id },  "");

            var CustomerComplaintReport = queryFactory.Update<SelectCustomerComplaintReportsDto>(query, "Id", true);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectCustomerComplaintReportsDto>(CustomerComplaintReport);

        }

        public async Task<IDataResult<SelectCustomerComplaintReportsDto>> GetWithUnsuitabilityItemDescriptionAsync(string description)
        {
            var query = queryFactory.Query().From(Tables.UnsuitabilityTypesItems).Select("*").Where(
            new
            {
                UnsuitabilityTypesDescription = description
            },  "");
            var unsuitabilityTypesItems = queryFactory.Get<SelectCustomerComplaintReportsDto>(query);


            LogsAppService.InsertLogToDatabase(unsuitabilityTypesItems, unsuitabilityTypesItems, LoginedUserService.UserId, Tables.UnsuitabilityTypesItems, LogType.Get, unsuitabilityTypesItems.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectCustomerComplaintReportsDto>(unsuitabilityTypesItems);

        }
    }
}

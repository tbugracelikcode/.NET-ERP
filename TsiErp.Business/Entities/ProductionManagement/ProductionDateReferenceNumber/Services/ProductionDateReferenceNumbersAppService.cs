using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Models;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.NotificationTemplate.Services;
using TsiErp.Business.Entities.HaltReason.Services;
using TsiErp.Business.Entities.HaltReason.Validations;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.Other.Notification.Services;
using TsiErp.Business.Entities.ProductionManagement.ProductionDateReferenceNumber.Validations;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserGroup.Dtos;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductionDateReferenceNumber;
using TsiErp.Entities.Entities.ProductionManagement.ProductionDateReferenceNumber.Dtos;
using TsiErp.Entities.Entities.StockManagement.UnitSet;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.HaltReasons.Page;
using TsiErp.Localizations.Resources.ProductionDateReferenceNumbers.Page;

namespace TsiErp.Business.Entities.ProductionManagement.ProductionDateReferenceNumber.Services
{
    [ServiceRegistration(typeof(IProductionDateReferenceNumbersAppService), DependencyInjectionType.Scoped)]
    public class ProductionDateReferenceNumbersAppService : ApplicationService<ProductionDateReferenceNumbersResource>, IProductionDateReferenceNumbersAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private readonly IGetSQLDateAppService _GetSQLDateAppService;
        private readonly INotificationsAppService _NotificationsAppService;
        private readonly INotificationTemplatesAppService _NotificationTemplatesAppService;

        public ProductionDateReferenceNumbersAppService(IStringLocalizer<ProductionDateReferenceNumbersResource> l, IGetSQLDateAppService getSQLDateAppService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService) : base(l)
        {
            _GetSQLDateAppService = getSQLDateAppService;
            _NotificationsAppService = notificationsAppService;
            _NotificationTemplatesAppService = notificationTemplatesAppService;
        }

        [ValidationAspect(typeof(CreateProductionDateReferenceNumbersValidator), Priority = 1)]
        public async Task<IDataResult<SelectProductionDateReferenceNumbersDto>> CreateAsync(CreateProductionDateReferenceNumbersDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.ProductionDateReferenceNumbers).Select("ProductionDateReferenceNo").Where(new { ProductionDateReferenceNo = input.ProductionDateReferenceNo }, "");

            var list = queryFactory.ControlList<ProductionDateReferenceNumbers>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();
            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.ProductionDateReferenceNumbers).Insert(new CreateProductionDateReferenceNumbersDto
            {
                Id = addedEntityId,
                CreationTime = now,
                CreatorId = LoginedUserService.UserId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = Guid.Empty,
                DeletionTime = null,
                LastModificationTime = null,
                LastModifierId = Guid.Empty,
                IsDeleted = false,
                ProductionDateReferenceNo = input.ProductionDateReferenceNo,
                Confirmation = input.Confirmation,
                _Description = input._Description
            });

            var productionDateReferenceNos = queryFactory.Insert<SelectProductionDateReferenceNumbersDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.ProductionDateReferenceNumbers, LogType.Insert, addedEntityId);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["ProductionDateReferenceNumbersChildMenu"], L["ProcessAdd"])).Data.FirstOrDefault();

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
                                RecordNumber = input.ProductionDateReferenceNo,
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
                            RecordNumber = input.ProductionDateReferenceNo,
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

            return new SuccessDataResult<SelectProductionDateReferenceNumbersDto>(productionDateReferenceNos);
        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            var entity = (await GetAsync(id)).Data;
            var query = queryFactory.Query().From(Tables.ProductionDateReferenceNumbers).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

            var productionDateReferenceNos = queryFactory.Update<SelectProductionDateReferenceNumbersDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.ProductionDateReferenceNumbers, LogType.Delete, id);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["ProductionDateReferenceNumbersChildMenu"], L["ProcessDelete"])).Data.FirstOrDefault();

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
                                RecordNumber = entity.ProductionDateReferenceNo,
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
                            RecordNumber = entity.ProductionDateReferenceNo,
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
            return new SuccessDataResult<SelectProductionDateReferenceNumbersDto>(productionDateReferenceNos);

        }

        public async Task<IDataResult<SelectProductionDateReferenceNumbersDto>> GetAsync(Guid id)
        {

            var query = queryFactory.Query().From(Tables.ProductionDateReferenceNumbers).Select("*").Where(
            new
            {
                Id = id
            }, "");
            var productionDateReferenceNo = queryFactory.Get<SelectProductionDateReferenceNumbersDto>(query);


            LogsAppService.InsertLogToDatabase(productionDateReferenceNo, productionDateReferenceNo, LoginedUserService.UserId, Tables.ProductionDateReferenceNumbers, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectProductionDateReferenceNumbersDto>(productionDateReferenceNo);

        }

        public async Task<IDataResult<IList<ListProductionDateReferenceNumbersDto>>> GetListAsync(ListProductionDateReferenceNumbersParameterDto input)
        {

            var query = queryFactory.Query().From(Tables.ProductionDateReferenceNumbers).Select<ProductionDateReferenceNumbers>(s => new { s.ProductionDateReferenceNo, s.Confirmation, s._Description, s.Id}).Where(null, "");
            var productionDateReferenceNos = queryFactory.GetList<ListProductionDateReferenceNumbersDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListProductionDateReferenceNumbersDto>>(productionDateReferenceNos);
        }


        [ValidationAspect(typeof(UpdateProductionDateReferenceNumbersValidator), Priority = 1)]
        public async Task<IDataResult<SelectProductionDateReferenceNumbersDto>> UpdateAsync(UpdateProductionDateReferenceNumbersDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.ProductionDateReferenceNumbers).Select("*").Where(new { Id = input.Id }, "");
            var entity = queryFactory.Get<ProductionDateReferenceNumbers>(entityQuery);

            #region Update Control

            var listQuery = queryFactory.Query().From(Tables.ProductionDateReferenceNumbers).Select("*").Where(new { ProductionDateReferenceNo = input.ProductionDateReferenceNo }, "");
            var list = queryFactory.GetList<ProductionDateReferenceNumbers>(listQuery).ToList();

            if (list.Count > 0 && entity.ProductionDateReferenceNo != input.ProductionDateReferenceNo)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }

            #endregion

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.ProductionDateReferenceNumbers).Update(new UpdateProductionDateReferenceNumbersDto
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
                ProductionDateReferenceNo = input.ProductionDateReferenceNo,
                _Description = input._Description,
                Confirmation = input.Confirmation
            }).Where(new { Id = input.Id }, "");

            var productionDateReferenceNos = queryFactory.Update<SelectProductionDateReferenceNumbersDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, productionDateReferenceNos, LoginedUserService.UserId, Tables.ProductionDateReferenceNumbers, LogType.Update, entity.Id);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["ProductionDateReferenceNumbersChildMenu"], L["ProcessRefresh"])).Data.FirstOrDefault();

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
                                RecordNumber = input.ProductionDateReferenceNo,
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
                            RecordNumber = input.ProductionDateReferenceNo,
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
            return new SuccessDataResult<SelectProductionDateReferenceNumbersDto>(productionDateReferenceNos);
        }


        public async Task<IDataResult<SelectProductionDateReferenceNumbersDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.ProductionDateReferenceNumbers).Select("*").Where(new { Id = id }, "");

            var entity = queryFactory.Get<ProductionDateReferenceNumbers>(entityQuery);

            var query = queryFactory.Query().From(Tables.ProductionDateReferenceNumbers).Update(new UpdateProductionDateReferenceNumbersDto
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
                Confirmation = entity.Confirmation,
                _Description = entity._Description,
                ProductionDateReferenceNo = entity.ProductionDateReferenceNo,

            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id }, "");

            var productionDateReferenceNos = queryFactory.Update<SelectProductionDateReferenceNumbersDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectProductionDateReferenceNumbersDto>(productionDateReferenceNos);
        }

    }
}

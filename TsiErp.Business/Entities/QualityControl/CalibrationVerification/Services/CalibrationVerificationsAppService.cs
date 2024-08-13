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
using TsiErp.Business.Entities.CalibrationVerification.Validations;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.NotificationTemplate.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.Other.Notification.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.Entities.QualityControl.CalibrationVerification;
using TsiErp.Entities.Entities.QualityControl.CalibrationVerification.Dtos;
using TsiErp.Entities.Entities.QualityControl.EquipmentRecord;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.CalibrationVerifications.Page;

namespace TsiErp.Business.Entities.CalibrationVerification.Services
{
    [ServiceRegistration(typeof(ICalibrationVerificationsAppService), DependencyInjectionType.Scoped)]
    public class CalibrationVerificationsAppService : ApplicationService<CalibrationVerificationsResource>, ICalibrationVerificationsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;
        private readonly INotificationsAppService _NotificationsAppService;
        private readonly INotificationTemplatesAppService _NotificationTemplatesAppService;

        public CalibrationVerificationsAppService(IStringLocalizer<CalibrationVerificationsResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
            _NotificationsAppService = notificationsAppService;
            _NotificationTemplatesAppService = notificationTemplatesAppService;
        }


        [ValidationAspect(typeof(CreateCalibrationVerifcationsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectCalibrationVerificationsDto>> CreateAsync(CreateCalibrationVerificationsDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.CalibrationVerifications).Select("Code").Where(new { Code = input.Code }, "");

            var list = queryFactory.ControlList<CalibrationVerifications>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();


            var query = queryFactory.Query().From(Tables.CalibrationVerifications).Insert(new CreateCalibrationVerificationsDto
            {
                Code = input.Code,
                Date_ = input.Date_,
                EquipmentID = input.EquipmentID.GetValueOrDefault(),
                InfinitiveCertificateNo = input.InfinitiveCertificateNo,
                ReceiptNo = input.ReceiptNo,
                Result = input.Result,
                NextControl = input.NextControl,
                CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
                CreatorId = LoginedUserService.UserId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = Guid.Empty,
                DeletionTime = null,
                Id = addedEntityId,
                IsDeleted = false,
                LastModificationTime = null,
                LastModifierId = Guid.Empty,
                Name = input.Name
            });

            var calibrationVerifications = queryFactory.Insert<SelectCalibrationVerificationsDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("CalVerificationsChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.CalibrationVerifications, LogType.Insert, addedEntityId);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["CalVerificationsChildMenu"], L["ProcessAdd"])).Data.FirstOrDefault();

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
            return new SuccessDataResult<SelectCalibrationVerificationsDto>(calibrationVerifications);

        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var entity = (await GetAsync(id)).Data;
            var query = queryFactory.Query().From(Tables.CalibrationVerifications).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

            var calibrationVerifications = queryFactory.Update<SelectCalibrationVerificationsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.CalibrationVerifications, LogType.Delete, id);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["CalVerificationsChildMenu"], L["ProcessDelete"])).Data.FirstOrDefault();

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
            return new SuccessDataResult<SelectCalibrationVerificationsDto>(calibrationVerifications);

        }


        public async Task<IDataResult<SelectCalibrationVerificationsDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                    .Query().From(Tables.CalibrationVerifications).Select<CalibrationVerifications>(null)
                        .Join<EquipmentRecords>
                        (
                            e => new { Equipment = e.Code, EquipmentID = e.Id },
                            nameof(CalibrationVerifications.EquipmentID),
                            nameof(EquipmentRecords.Id),
                            JoinType.Left
                        )
                        .Where(new { Id = id }, Tables.CalibrationVerifications);

            var calibrationVerification = queryFactory.Get<SelectCalibrationVerificationsDto>(query);

            LogsAppService.InsertLogToDatabase(calibrationVerification, calibrationVerification, LoginedUserService.UserId, Tables.CalibrationVerifications, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectCalibrationVerificationsDto>(calibrationVerification);


        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListCalibrationVerificationsDto>>> GetListAsync(ListCalibrationVerificationsParameterDto input)
        {
            var query = queryFactory
                    .Query()
                    .From(Tables.CalibrationVerifications)
                    .Select<CalibrationVerifications>(s => new { s.Code, s.Name, s.ReceiptNo, s.Date_, s.InfinitiveCertificateNo, s.NextControl, s.Result, s.Id })
                        .Join<EquipmentRecords>
                        (
                            e => new { Equipment = e.Code },
                            nameof(CalibrationVerifications.EquipmentID),
                            nameof(EquipmentRecords.Id),
                            JoinType.Left
                        ).Where(null, Tables.CalibrationVerifications);


            var calibrationVerifications = queryFactory.GetList<ListCalibrationVerificationsDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListCalibrationVerificationsDto>>(calibrationVerifications);

        }


        [ValidationAspect(typeof(UpdateCalibrationVerificationsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectCalibrationVerificationsDto>> UpdateAsync(UpdateCalibrationVerificationsDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.CalibrationVerifications).Select("*").Where(new { Id = input.Id }, "");
            var entity = queryFactory.Get<CalibrationVerifications>(entityQuery);

            #region Update Control

            var listQuery = queryFactory.Query().From(Tables.CalibrationVerifications).Select("*").Where(new { Code = input.Code }, "");
            var list = queryFactory.GetList<CalibrationVerifications>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }

            #endregion

            var query = queryFactory.Query().From(Tables.CalibrationVerifications).Update(new UpdateCalibrationVerificationsDto
            {
                Code = input.Code,
                Name = input.Name,
                EquipmentID = input.EquipmentID.GetValueOrDefault(),
                Date_ = input.Date_,
                Result = input.Result,
                ReceiptNo = input.ReceiptNo,
                InfinitiveCertificateNo = input.InfinitiveCertificateNo,
                NextControl = input.NextControl,
                Id = input.Id,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                IsDeleted = entity.IsDeleted,
                LastModificationTime = _GetSQLDateAppService.GetDateFromSQL(),
                LastModifierId = LoginedUserService.UserId
            }).Where(new { Id = input.Id }, "");

            var calibrationVerifications = queryFactory.Update<SelectCalibrationVerificationsDto>(query, "Id", true);


            LogsAppService.InsertLogToDatabase(entity, calibrationVerifications, LoginedUserService.UserId, Tables.CalibrationVerifications, LogType.Update, entity.Id);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["CalVerificationsChildMenu"], L["ProcessRefresh"])).Data.FirstOrDefault();

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
            return new SuccessDataResult<SelectCalibrationVerificationsDto>(calibrationVerifications);

        }

        public async Task<IDataResult<SelectCalibrationVerificationsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.CalibrationVerifications).Select("Id").Where(new { Id = id },  "");
            var entity = queryFactory.Get<CalibrationVerifications>(entityQuery);

            var query = queryFactory.Query().From(Tables.CalibrationVerifications).Update(new UpdateCalibrationVerificationsDto
            {
                Code = entity.Code,
                Name = entity.Name,
                NextControl = entity.NextControl,
                InfinitiveCertificateNo = entity.InfinitiveCertificateNo,
                ReceiptNo = entity.ReceiptNo,
                EquipmentID = entity.EquipmentID,
                Result = entity.Result,
                Date_ = entity.Date_,
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

            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id }, "");

            var calibrationVerifications = queryFactory.Update<SelectCalibrationVerificationsDto>(query, "Id", true);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectCalibrationVerificationsDto>(calibrationVerifications);


        }
    }
}

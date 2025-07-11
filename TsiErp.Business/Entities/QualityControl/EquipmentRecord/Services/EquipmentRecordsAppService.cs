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
using TsiErp.Business.Entities.EquipmentRecord.Validations;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.NotificationTemplate.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.Other.Notification.Services;
using TsiErp.Business.Extensions.DeleteControlExtension;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Department;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.Entities.QualityControl.EquipmentRecord;
using TsiErp.Entities.Entities.QualityControl.EquipmentRecord.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.EquipmentRecords.Page;

namespace TsiErp.Business.Entities.EquipmentRecord.Services
{
    [ServiceRegistration(typeof(IEquipmentRecordsAppService), DependencyInjectionType.Scoped)]
    public class EquipmentRecordsAppService : ApplicationService<EquipmentRecordsResource>, IEquipmentRecordsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;
        private readonly INotificationsAppService _NotificationsAppService;
        private readonly INotificationTemplatesAppService _NotificationTemplatesAppService;

        public EquipmentRecordsAppService(IStringLocalizer<EquipmentRecordsResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
            _NotificationsAppService = notificationsAppService;
            _NotificationTemplatesAppService = notificationTemplatesAppService;
        }





        [ValidationAspect(typeof(CreateEquipmentRecorsValidator), Priority = 1)]
        public async Task<IDataResult<SelectEquipmentRecordsDto>> CreateAsync(CreateEquipmentRecordsDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.EquipmentRecords).Select("Code").Where(new { Code = input.Code }, "");

            var list = queryFactory.ControlList<EquipmentRecords>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();
            DateTime now = _GetSQLDateAppService.GetDateFromSQL();


            var query = queryFactory.Query().From(Tables.EquipmentRecords).Insert(new CreateEquipmentRecordsDto
            {
                Code = input.Code,
                Department = input.Department.GetValueOrDefault(),
                Cancel = input.Cancel,
                CancellationDate = input.CancellationDate,
                CancellationReason = input.CancellationReason,
                EquipmentSerialNo = input.EquipmentSerialNo,
                Frequency = input.Frequency,
                MeasuringAccuracy = input.MeasuringAccuracy,
                MeasuringRange = input.MeasuringRange,
                RecordDate = input.RecordDate,
                Responsible = input.Responsible,
                CreationTime =now,
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

            var equipmentRecords = queryFactory.Insert<SelectEquipmentRecordsDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("EquipmentRecordsChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.EquipmentRecords, LogType.Insert, addedEntityId);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["EquipmentRecordsChildMenu"], L["ProcessAdd"])).Data.FirstOrDefault();

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
            return new SuccessDataResult<SelectEquipmentRecordsDto>(equipmentRecords);

        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            DeleteControl.ControlList.Clear();


            DeleteControl.ControlList.Add("EquipmentID", new List<string>
            {
                Tables.CalibrationRecords,
                Tables.CalibrationVerifications
            });


            bool control = DeleteControl.Control(queryFactory, id);

            if (!control)
            {
                throw new Exception(L["DeleteControlManager"]);
            }
            else
            {
                var entity = (await GetAsync(id)).Data;
                var query = queryFactory.Query().From(Tables.EquipmentRecords).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

                var equipmentRecords = queryFactory.Update<SelectEquipmentRecordsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.EquipmentRecords, LogType.Delete, id);
                #region Notification

                var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["EquipmentRecordsChildMenu"], L["ProcessDelete"])).Data.FirstOrDefault();

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
                return new SuccessDataResult<SelectEquipmentRecordsDto>(equipmentRecords);
            }
        }

        public async Task<IDataResult<SelectEquipmentRecordsDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                    .Query().From(Tables.EquipmentRecords).Select<EquipmentRecords>(null)
                        .Join<Departments>
                        (
                            d => new { DepartmentName = d.Name, Department = d.Id },
                            nameof(EquipmentRecords.Department),
                            nameof(Departments.Id),
                            JoinType.Left
                        )
                        .Where(new { Id = id }, Tables.EquipmentRecords);

            var equipmentRecord = queryFactory.Get<SelectEquipmentRecordsDto>(query);

            LogsAppService.InsertLogToDatabase(equipmentRecord, equipmentRecord, LoginedUserService.UserId, Tables.EquipmentRecords, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectEquipmentRecordsDto>(equipmentRecord);


        }

        public async Task<IDataResult<IList<ListEquipmentRecordsDto>>> GetListAsync(ListEquipmentRecordsParameterDto input)
        {

            var query = queryFactory
               .Query()
               .From(Tables.EquipmentRecords)
               .Select<EquipmentRecords>(s => new { s.Code, s.Name, s.Responsible, s.RecordDate, s.Id })
                   .Join<Departments>
                   (
                       d => new { DepartmentName = d.Name },
                       nameof(EquipmentRecords.Department),
                       nameof(Departments.Id),
                       JoinType.Left
                   ).Where(null, Tables.EquipmentRecords);

            var equipmentRecords = queryFactory.GetList<ListEquipmentRecordsDto>(query).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListEquipmentRecordsDto>>(equipmentRecords);

        }

        [ValidationAspect(typeof(UpdateEquipmentRecordsValidator), Priority = 1)]
        public async Task<IDataResult<SelectEquipmentRecordsDto>> UpdateAsync(UpdateEquipmentRecordsDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.EquipmentRecords).Select("*").Where(new { Id = input.Id },  "");
            var entity = queryFactory.Get<EquipmentRecords>(entityQuery);

            #region Update Control

            var listQuery = queryFactory.Query().From(Tables.EquipmentRecords).Select("*").Where(new { Code = input.Code },  "");
            var list = queryFactory.GetList<EquipmentRecords>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }

            #endregion

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.EquipmentRecords).Update(new UpdateEquipmentRecordsDto
            {
                Code = input.Code,
                Name = input.Name,
                Id = input.Id,
                Responsible = input.Responsible,
                MeasuringRange = input.MeasuringRange,
                MeasuringAccuracy = input.MeasuringAccuracy,
                Frequency = input.Frequency,
                Cancel = input.Cancel,
                CancellationDate = input.CancellationDate,
                CancellationReason = input.CancellationReason,
                Department = input.Department.GetValueOrDefault(),
                EquipmentSerialNo = input.EquipmentSerialNo,
                RecordDate = input.RecordDate,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                IsDeleted = entity.IsDeleted,
                LastModificationTime =now,
                LastModifierId = LoginedUserService.UserId
            }).Where(new { Id = input.Id }, "");

            var equipmentRecords = queryFactory.Update<SelectEquipmentRecordsDto>(query, "Id", true);


            LogsAppService.InsertLogToDatabase(entity, equipmentRecords, LoginedUserService.UserId, Tables.EquipmentRecords, LogType.Update, entity.Id);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["EquipmentRecordsChildMenu"], L["ProcessRefresh"])).Data.FirstOrDefault();

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

            return new SuccessDataResult<SelectEquipmentRecordsDto>(equipmentRecords);

        }

        public async Task<IDataResult<SelectEquipmentRecordsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.EquipmentRecords).Select("*").Where(new { Id = id },  "");
            var entity = queryFactory.Get<EquipmentRecords>(entityQuery);

            var query = queryFactory.Query().From(Tables.EquipmentRecords).Update(new UpdateEquipmentRecordsDto
            {
                Code = entity.Code,
                Name = entity.Name,
                Responsible = entity.Responsible,
                MeasuringRange = entity.MeasuringRange,
                MeasuringAccuracy = entity.MeasuringAccuracy,
                Frequency = entity.Frequency,
                Cancel = entity.Cancel,
                CancellationDate = entity.CancellationDate,
                CancellationReason = entity.CancellationReason,
                Department = entity.Department,
                EquipmentSerialNo = entity.EquipmentSerialNo,
                RecordDate = entity.RecordDate,
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

            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id },  "");

            var equipmentRecords = queryFactory.Update<SelectEquipmentRecordsDto>(query, "Id", true);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectEquipmentRecordsDto>(equipmentRecords);


        }
    }
}

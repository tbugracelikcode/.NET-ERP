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
using TsiErp.Business.Entities.PurchaseRequest.Services;
using TsiErp.Business.Entities.UnsuitabilityItemSPC.Validations;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.BankAccount;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationGroup;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItem;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItemSPC;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItemSPC.Dtos;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItemSPCLine;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItemSPCLine.Dtos;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityTypesItem;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.UnsuitabilityItemSPCs.Page;

namespace TsiErp.Business.Entities.UnsuitabilityItemSPC.Services
{
    [ServiceRegistration(typeof(IUnsuitabilityItemSPCsAppService), DependencyInjectionType.Scoped)]
    public class UnsuitabilityItemSPCsAppService : ApplicationService<UnsuitabilityItemSPCsResource>, IUnsuitabilityItemSPCsAppService
    {
        private readonly IPurchaseRequestsAppService _PurchaseRequestsAppService;

        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly INotificationsAppService _NotificationsAppService;
        private readonly INotificationTemplatesAppService _NotificationTemplatesAppService;


        public UnsuitabilityItemSPCsAppService(IStringLocalizer<UnsuitabilityItemSPCsResource> l, IGetSQLDateAppService getSQLDateAppService, IPurchaseRequestsAppService PurchaseRequestsAppService, IFicheNumbersAppService ficheNumbersAppService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService) : base(l)
        {
            _PurchaseRequestsAppService = PurchaseRequestsAppService;
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
            _NotificationsAppService = notificationsAppService;
            _NotificationTemplatesAppService = notificationTemplatesAppService;
        }

        [ValidationAspect(typeof(CreateUnsuitabilityItemSPCsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectUnsuitabilityItemSPCsDto>> CreateAsync(CreateUnsuitabilityItemSPCsDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.UnsuitabilityItemSPCs).Select("Code").Where(new { Code = input.Code }, "");
            var list = queryFactory.ControlList<UnsuitabilityItemSPCs>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            string now = _GetSQLDateAppService.GetDateFromSQL().ToString();

            string[] timeSplit = now.Split(" ");

            string time = timeSplit[1];

            var query = queryFactory.Query().From(Tables.UnsuitabilityItemSPCs).Insert(new CreateUnsuitabilityItemSPCsDto
            {
                Code = input.Code,
                Date_ = input.Date_,
                Description_ = input.Description_,
                MeasurementEndDate = input.MeasurementEndDate,
                MeasurementStartDate = input.MeasurementStartDate,
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
            });

            foreach (var item in input.SelectUnsuitabilityItemSPCLines)
            {
                var queryLine = queryFactory.Query().From(Tables.UnsuitabilityItemSPCLines).Insert(new CreateUnsuitabilityItemSPCLinesDto
                {
                    TotalUnsuitableReport = item.TotalUnsuitableReport,
                    UnsuitabilityItemID = item.UnsuitabilityItemID.GetValueOrDefault(),
                    UnsuitabilityTypeID = item.UnsuitabilityTypeID.GetValueOrDefault(),
                    UnsuitableComponentPerReport = item.UnsuitableComponentPerReport,
                    Detectability = item.Detectability,
                    Frequency = item.Frequency,
                    LineNr = item.LineNr,
                    RPN = item.RPN,
                    TotalUnsuitableComponent = item.TotalUnsuitableComponent,
                    WorkCenterID = item.WorkCenterID.GetValueOrDefault(),
                    UnsuitabilitySPCID = addedEntityId,
                    CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
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

            var UnsuitabilityItemSPC = queryFactory.Insert<SelectUnsuitabilityItemSPCsDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("UnsuitabilityItemSPSChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.UnsuitabilityItemSPCs, LogType.Insert, addedEntityId);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["UnsuitabilityItemSPSChildMenu"], L["ProcessAdd"])).Data.FirstOrDefault();

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
            return new SuccessDataResult<SelectUnsuitabilityItemSPCsDto>(UnsuitabilityItemSPC);

        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var entity = (await GetAsync(id)).Data;
            var query = queryFactory.Query().From(Tables.UnsuitabilityItemSPCs).Select("*").Where(new { Id = id },  "");

            var UnsuitabilityItemSPCs = queryFactory.Get<SelectUnsuitabilityItemSPCsDto>(query);

            if (UnsuitabilityItemSPCs.Id != Guid.Empty && UnsuitabilityItemSPCs != null)
            {

                var deleteQuery = queryFactory.Query().From(Tables.UnsuitabilityItemSPCs).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

                var lineDeleteQuery = queryFactory.Query().From(Tables.UnsuitabilityItemSPCLines).Delete(LoginedUserService.UserId).Where(new { UnsuitabilitySPCID = id }, "");

                deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                var UnsuitabilityItemSPC = queryFactory.Update<SelectUnsuitabilityItemSPCsDto>(deleteQuery, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.UnsuitabilityItemSPCs, LogType.Delete, id);
                #region Notification

                var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["UnsuitabilityItemSPSChildMenu"], L["ProcessDelete"])).Data.FirstOrDefault();

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

                return new SuccessDataResult<SelectUnsuitabilityItemSPCsDto>(UnsuitabilityItemSPC);
            }
            else
            {
                var queryLineGet = queryFactory.Query().From(Tables.UnsuitabilityItemSPCLines).Select("*").Where(new { Id = id }, "");

                var UnsuitabilityItemSPCsLineGet = queryFactory.Get<SelectUnsuitabilityItemSPCLinesDto>(queryLineGet);

                var queryLine = queryFactory.Query().From(Tables.UnsuitabilityItemSPCLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

                var UnsuitabilityItemSPCLines = queryFactory.Update<SelectUnsuitabilityItemSPCLinesDto>(queryLine, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.UnsuitabilityItemSPCLines, LogType.Delete, id);
                await Task.CompletedTask;

                return new SuccessDataResult<SelectUnsuitabilityItemSPCLinesDto>(UnsuitabilityItemSPCLines);
            }

        }

        public async Task<IDataResult<SelectUnsuitabilityItemSPCsDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.UnsuitabilityItemSPCs)
                   .Select("*")
                    .Where(new { Id = id }, Tables.UnsuitabilityItemSPCs);

            var UnsuitabilityItemSPCs = queryFactory.Get<SelectUnsuitabilityItemSPCsDto>(query);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.UnsuitabilityItemSPCLines)
                   .Select<UnsuitabilityItemSPCLines>(null)
                   .Join<StationGroups>
                    (
                        p => new { WorkCenterID = p.Id, WorkCenterName = p.Name },
                        nameof(UnsuitabilityItemSPCLines.WorkCenterID),
                        nameof(StationGroups.Id),
                        JoinType.Left
                    )
                   .Join<UnsuitabilityTypesItems>
                    (
                        u => new { UnsuitabilityTypeID = u.Id, UnsuitabilityTypeName = u.Name },
                        nameof(UnsuitabilityItemSPCLines.UnsuitabilityTypeID),
                        nameof(UnsuitabilityTypesItems.Id),
                        JoinType.Left
                    )
                    .Join<UnsuitabilityItems>
                    (
                        u => new { UnsuitabilityItemID = u.Id, UnsuitabilityItemName = u.Name, UnsuitabilityItemIntensityCoefficient = u.IntensityCoefficient },
                        nameof(UnsuitabilityItemSPCLines.UnsuitabilityItemID),
                        nameof(UnsuitabilityItems.Id),
                        JoinType.Left
                    )
                    .Where(new { UnsuitabilitySPCID = id }, Tables.UnsuitabilityItemSPCLines);

            var UnsuitabilityItemSPCLine = queryFactory.GetList<SelectUnsuitabilityItemSPCLinesDto>(queryLines).ToList();

            UnsuitabilityItemSPCs.SelectUnsuitabilityItemSPCLines = UnsuitabilityItemSPCLine;

            LogsAppService.InsertLogToDatabase(UnsuitabilityItemSPCs, UnsuitabilityItemSPCs, LoginedUserService.UserId, Tables.UnsuitabilityItemSPCs, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectUnsuitabilityItemSPCsDto>(UnsuitabilityItemSPCs);

        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListUnsuitabilityItemSPCsDto>>> GetListAsync(ListUnsuitabilityItemSPCsParameterDto input)
        {
            var query = queryFactory
                   .Query()
                    .From(Tables.UnsuitabilityItemSPCs)
                   .Select<UnsuitabilityItemSPCs>(s => new { s.Code, s.Date_, s.MeasurementStartDate, s.MeasurementEndDate })
                    .Where(null,  Tables.UnsuitabilityItemSPCs);

            var UnsuitabilityItemSPCs = queryFactory.GetList<ListUnsuitabilityItemSPCsDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListUnsuitabilityItemSPCsDto>>(UnsuitabilityItemSPCs);

        }

        [ValidationAspect(typeof(UpdateUnsuitabilityItemSPCsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectUnsuitabilityItemSPCsDto>> UpdateAsync(UpdateUnsuitabilityItemSPCsDto input)
        {
            var entityQuery = queryFactory
                   .Query()
                   .From(Tables.UnsuitabilityItemSPCs)
                   .Select("*")
                    .Where(new { Id = input.Id }, Tables.UnsuitabilityItemSPCs);

            var entity = queryFactory.Get<SelectUnsuitabilityItemSPCsDto>(entityQuery);

            var queryLines = queryFactory
                   .Query()
                    .From(Tables.UnsuitabilityItemSPCLines)
                   .Select<UnsuitabilityItemSPCLines>(null)
                  .Join<StationGroups>
                    (
                        p => new { WorkCenterID = p.Id, WorkCenterName = p.Name },
                        nameof(UnsuitabilityItemSPCLines.WorkCenterID),
                        nameof(StationGroups.Id),
                        JoinType.Left
                    )
                   .Join<UnsuitabilityTypesItems>
                    (
                        u => new { UnsuitabilityTypeID = u.Id, UnsuitabilityTypeName = u.Name },
                        nameof(UnsuitabilityItemSPCLines.UnsuitabilityTypeID),
                        nameof(UnsuitabilityTypesItems.Id),
                        JoinType.Left
                    )
                    .Join<UnsuitabilityItems>
                    (
                        u => new { UnsuitabilityItemID = u.Id, UnsuitabilityItemName = u.Name, UnsuitabilityItemIntensityCoefficient = u.IntensityCoefficient },
                        nameof(UnsuitabilityItemSPCLines.UnsuitabilityItemID),
                        nameof(UnsuitabilityItems.Id),
                        JoinType.Left
                    )
                    .Where(new { UnsuitabilitySPCID = input.Id }, Tables.UnsuitabilityItemSPCLines);

            var UnsuitabilityItemSPCLine = queryFactory.GetList<SelectUnsuitabilityItemSPCLinesDto>(queryLines).ToList();

            entity.SelectUnsuitabilityItemSPCLines = UnsuitabilityItemSPCLine;

            #region Update Control
            var listQuery = queryFactory
                           .Query()
                           .From(Tables.UnsuitabilityItemSPCs)
                   .Select("*")
                    .Where(new { Code = input.Code },  Tables.UnsuitabilityItemSPCs);

            var list = queryFactory.GetList<ListUnsuitabilityItemSPCsDto>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion

            var query = queryFactory.Query().From(Tables.UnsuitabilityItemSPCs).Update(new UpdateUnsuitabilityItemSPCsDto
            {
                Code = input.Code,
                Date_ = input.Date_,
                Description_ = input.Description_,
                MeasurementEndDate = input.MeasurementEndDate,
                MeasurementStartDate = input.MeasurementStartDate,
                CreationTime = entity.CreationTime,
                CreatorId = entity.CreatorId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                Id = input.Id,
                IsDeleted = entity.IsDeleted,
                LastModificationTime = _GetSQLDateAppService.GetDateFromSQL(),
                LastModifierId = LoginedUserService.UserId,
            }).Where(new { Id = input.Id },  "");

            foreach (var item in input.SelectUnsuitabilityItemSPCLines)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.UnsuitabilityItemSPCLines).Insert(new CreateUnsuitabilityItemSPCLinesDto
                    {
                        TotalUnsuitableReport = item.TotalUnsuitableReport,
                        UnsuitabilityItemID = item.UnsuitabilityItemID.GetValueOrDefault(),
                        UnsuitabilityTypeID = item.UnsuitabilityTypeID.GetValueOrDefault(),
                        UnsuitableComponentPerReport = item.UnsuitableComponentPerReport,
                        Detectability = item.Detectability,
                        Frequency = item.Frequency,
                        LineNr = item.LineNr,
                        RPN = item.RPN,
                        TotalUnsuitableComponent = item.TotalUnsuitableComponent,
                        WorkCenterID = item.WorkCenterID.GetValueOrDefault(),
                        UnsuitabilitySPCID = input.Id,
                        CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
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
                    var lineGetQuery = queryFactory.Query().From(Tables.UnsuitabilityItemSPCLines).Select("*").Where(new { Id = item.Id },  "");

                    var line = queryFactory.Get<SelectUnsuitabilityItemSPCLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.UnsuitabilityItemSPCLines).Update(new UpdateUnsuitabilityItemSPCLinesDto
                        {
                            TotalUnsuitableReport = item.TotalUnsuitableReport,
                            UnsuitabilityItemID = item.UnsuitabilityItemID.GetValueOrDefault(),
                            UnsuitabilityTypeID = item.UnsuitabilityTypeID.GetValueOrDefault(),
                            UnsuitableComponentPerReport = item.UnsuitableComponentPerReport,
                            Detectability = item.Detectability,
                            Frequency = item.Frequency,
                            LineNr = item.LineNr,
                            RPN = item.RPN,
                            TotalUnsuitableComponent = item.TotalUnsuitableComponent,
                            WorkCenterID = item.WorkCenterID.GetValueOrDefault(),
                            UnsuitabilitySPCID = input.Id,
                            CreationTime = line.CreationTime,
                            CreatorId = line.CreatorId,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = line.DeleterId.GetValueOrDefault(),
                            DeletionTime = line.DeletionTime.GetValueOrDefault(),
                            Id = item.Id,
                            IsDeleted = item.IsDeleted,
                            LastModificationTime = _GetSQLDateAppService.GetDateFromSQL(),
                            LastModifierId = LoginedUserService.UserId,
                        }).Where(new { Id = line.Id },  "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }

            var UnsuitabilityItemSPC = queryFactory.Update<SelectUnsuitabilityItemSPCsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.UnsuitabilityItemSPCs, LogType.Update, UnsuitabilityItemSPC.Id);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["UnsuitabilityItemSPSChildMenu"], L["ProcessRefresh"])).Data.FirstOrDefault();

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
            return new SuccessDataResult<SelectUnsuitabilityItemSPCsDto>(UnsuitabilityItemSPC);

        }

        public async Task<IDataResult<SelectUnsuitabilityItemSPCsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.UnsuitabilityItemSPCs).Select("Id").Where(new { Id = id }, "");

            var entity = queryFactory.Get<UnsuitabilityItemSPCs>(entityQuery);

            var query = queryFactory.Query().From(Tables.UnsuitabilityItemSPCs).Update(new UpdateUnsuitabilityItemSPCsDto
            {
                Code = entity.Code,
                Date_ = entity.Date_,
                Description_ = entity.Description_,
                MeasurementEndDate = entity.MeasurementEndDate,
                MeasurementStartDate = entity.MeasurementStartDate,
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

            var UnsuitabilityItemSPCsDto = queryFactory.Update<SelectUnsuitabilityItemSPCsDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectUnsuitabilityItemSPCsDto>(UnsuitabilityItemSPCsDto);


        }
    }
}

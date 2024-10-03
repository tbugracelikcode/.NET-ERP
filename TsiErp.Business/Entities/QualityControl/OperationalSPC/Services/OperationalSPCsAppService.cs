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
using TsiErp.Business.Entities.OperationalSPC.Validations;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.Other.Notification.Services;
using TsiErp.Business.Entities.PurchaseRequest.Services;
using TsiErp.Business.Extensions.DeleteControlExtension;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.BankAccount;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationGroup;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductsOperation;
using TsiErp.Entities.Entities.QualityControl.OperationalSPC;
using TsiErp.Entities.Entities.QualityControl.OperationalSPC.Dtos;
using TsiErp.Entities.Entities.QualityControl.OperationalSPCLine;
using TsiErp.Entities.Entities.QualityControl.OperationalSPCLine.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.OperationalSPCs.Page;

namespace TsiErp.Business.Entities.OperationalSPC.Services
{
    [ServiceRegistration(typeof(IOperationalSPCsAppService), DependencyInjectionType.Scoped)]
    public class OperationalSPCsAppService : ApplicationService<OperationalSPCsResource>, IOperationalSPCsAppService
    {
        private readonly IPurchaseRequestsAppService _PurchaseRequestsAppService;

        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly INotificationsAppService _NotificationsAppService;
        private readonly INotificationTemplatesAppService _NotificationTemplatesAppService;


        public OperationalSPCsAppService(IStringLocalizer<OperationalSPCsResource> l, IGetSQLDateAppService getSQLDateAppService, IPurchaseRequestsAppService PurchaseRequestsAppService, IFicheNumbersAppService ficheNumbersAppService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService) : base(l)
        {
            _PurchaseRequestsAppService = PurchaseRequestsAppService;
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
            _NotificationsAppService = notificationsAppService;
            _NotificationTemplatesAppService = notificationTemplatesAppService;
        }




        [ValidationAspect(typeof(CreateOperationalSPCsValidator), Priority = 1)]
        public async Task<IDataResult<SelectOperationalSPCsDto>> CreateAsync(CreateOperationalSPCsDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.OperationalSPCs).Select("Code").Where(new { Code = input.Code }, "");
            var list = queryFactory.ControlList<OperationalSPCs>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            string time = now.ToString().Split(" ").LastOrDefault();

            var query = queryFactory.Query().From(Tables.OperationalSPCs).Insert(new CreateOperationalSPCsDto
            {
                Code = input.Code,
                Date_ = input.Date_,
                Description_ = input.Description_,
                MeasurementEndDate = input.MeasurementEndDate,
                MeasurementStartDate = input.MeasurementStartDate,
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

            foreach (var item in input.SelectOperationalSPCLines)
            {
                var queryLine = queryFactory.Query().From(Tables.OperationalSPCLines).Insert(new CreateOperationalSPCLinesDto
                {
                    Detectability = item.Detectability,
                    Frequency = item.Frequency,
                    LineNr = item.LineNr,
                    OperationBasedMidControlFrequency = item.OperationBasedMidControlFrequency,
                    OperationID = item.OperationID.GetValueOrDefault(),
                    RPN = item.RPN,
                    Severity = item.Severity,
                    TotalComponent = item.TotalComponent,
                    TotalOccuredOperation = item.TotalOccuredOperation,
                    TotalUnsuitableComponent = item.TotalUnsuitableComponent,
                    TotalUnsuitableOperation = item.TotalUnsuitableOperation,
                    UnsuitableComponentPerOperation = item.UnsuitableComponentPerOperation,
                    UnsuitableComponentRate = item.UnsuitableComponentRate,
                    UnsuitableOperationRate = item.UnsuitableOperationRate,
                    WorkCenterID = item.WorkCenterID.GetValueOrDefault(),
                    OperationalSPCID = addedEntityId,
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

            var OperationalSPC = queryFactory.Insert<SelectOperationalSPCsDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("OperationalSPCChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.OperationalSPCs, LogType.Insert, addedEntityId);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["OperationalSPCChildMenu"], L["ProcessAdd"])).Data.FirstOrDefault();

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
            return new SuccessDataResult<SelectOperationalSPCsDto>(OperationalSPC);

        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            DeleteControl.ControlList.Clear();

            DeleteControl.ControlList.Add("FirstOperationalSPCID", new List<string>
            {
                Tables.PFMEAs
            });

            DeleteControl.ControlList.Add("SecondOperationalSPCID", new List<string>
            {
                Tables.PFMEAs
            });


            bool control = DeleteControl.Control(queryFactory, id);

            if (!control)
            {
                throw new Exception(L["DeleteControlManager"]);
            }
            else
            {
                var entity = (await GetAsync(id)).Data;
                var query = queryFactory.Query().From(Tables.OperationalSPCs).Select("*").Where(new { Id = id }, "");

                var OperationalSPCs = queryFactory.Get<SelectOperationalSPCsDto>(query);

                if (OperationalSPCs.Id != Guid.Empty && OperationalSPCs != null)
                {

                    var deleteQuery = queryFactory.Query().From(Tables.OperationalSPCs).Delete(LoginedUserService.UserId).Where(new { Id = id },  "");

                    var lineDeleteQuery = queryFactory.Query().From(Tables.OperationalSPCLines).Delete(LoginedUserService.UserId).Where(new { OperationalSPCID = id },  "");

                    deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                    var OperationalSPC = queryFactory.Update<SelectOperationalSPCsDto>(deleteQuery, "Id", true);

                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.OperationalSPCs, LogType.Delete, id);
                    #region Notification

                    var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["OperationalSPCChildMenu"], L["ProcessDelete"])).Data.FirstOrDefault();

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
                    return new SuccessDataResult<SelectOperationalSPCsDto>(OperationalSPC);
                }
                else
                {
                    var queryLineGet = queryFactory.Query().From(Tables.OperationalSPCLines).Select("*").Where(new { Id = id },  "");

                    var OperationalSPCsLineGet = queryFactory.Get<SelectOperationalSPCLinesDto>(queryLineGet);

                    var queryLine = queryFactory.Query().From(Tables.OperationalSPCLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

                    var OperationalSPCLines = queryFactory.Update<SelectOperationalSPCLinesDto>(queryLine, "Id", true);

                    LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.OperationalSPCLines, LogType.Delete, id);

                    await Task.CompletedTask;
                    return new SuccessDataResult<SelectOperationalSPCLinesDto>(OperationalSPCLines);
                }
            }
        }

        public async Task<IDataResult<SelectOperationalSPCsDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.OperationalSPCs)
                   .Select("*")
                    .Where(new { Id = id },  Tables.OperationalSPCs);

            var OperationalSPCs = queryFactory.Get<SelectOperationalSPCsDto>(query);

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.OperationalSPCLines)
                   .Select<OperationalSPCLines>(null)
                   .Join<StationGroups>
                    (
                        p => new { WorkCenterID = p.Id, WorkCenterName = p.Name },
                        nameof(OperationalSPCLines.WorkCenterID),
                        nameof(StationGroups.Id),
                        JoinType.Left
                    )
                   .Join<ProductsOperations>
                    (
                        u => new { OperationID = u.Id, OperationCode = u.Code, OperationName = u.Name },
                        nameof(OperationalSPCLines.OperationID),
                        nameof(ProductsOperations.Id),
                        JoinType.Left
                    )
                    .Where(new { OperationalSPCID = id }, Tables.OperationalSPCLines);

            var OperationalSPCLine = queryFactory.GetList<SelectOperationalSPCLinesDto>(queryLines).ToList();

            OperationalSPCs.SelectOperationalSPCLines = OperationalSPCLine;

            LogsAppService.InsertLogToDatabase(OperationalSPCs, OperationalSPCs, LoginedUserService.UserId, Tables.OperationalSPCs, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectOperationalSPCsDto>(OperationalSPCs);

        }

        public async Task<IDataResult<IList<ListOperationalSPCsDto>>> GetListAsync(ListOperationalSPCsParameterDto input)
        {
            var query = queryFactory
                   .Query()
                    .From(Tables.OperationalSPCs)
                   .Select<OperationalSPCs>(null)
                    .Where(null,Tables.OperationalSPCs);

            var OperationalSPCs = queryFactory.GetList<ListOperationalSPCsDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListOperationalSPCsDto>>(OperationalSPCs);

        }

        [ValidationAspect(typeof(UpdateOperationalSPCsValidator), Priority = 1)]
        public async Task<IDataResult<SelectOperationalSPCsDto>> UpdateAsync(UpdateOperationalSPCsDto input)
        {
            var entityQuery = queryFactory
                   .Query()
                   .From(Tables.OperationalSPCs)
                   .Select("*")
                    .Where(new { Id = input.Id },Tables.OperationalSPCs);

            var entity = queryFactory.Get<SelectOperationalSPCsDto>(entityQuery);

            var queryLines = queryFactory
                   .Query()
                    .From(Tables.OperationalSPCLines)
                   .Select<OperationalSPCLines>(null)
                  .Join<StationGroups>
                    (
                        p => new { WorkCenterID = p.Id, WorkCenterName = p.Name },
                        nameof(OperationalSPCLines.WorkCenterID),
                        nameof(StationGroups.Id),
                        JoinType.Left
                    )
                   .Join<ProductsOperations>
                    (
                        u => new { OperationID = u.Id, OperationCode = u.Code, OperationName = u.Name },
                        nameof(OperationalSPCLines.OperationID),
                        nameof(ProductsOperations.Id),
                        JoinType.Left
                    )
                    .Where(new { OperationalSPCID = input.Id },  Tables.OperationalSPCLines);

            var OperationalSPCLine = queryFactory.GetList<SelectOperationalSPCLinesDto>(queryLines).ToList();

            entity.SelectOperationalSPCLines = OperationalSPCLine;

            #region Update Control
            var listQuery = queryFactory
                           .Query()
                           .From(Tables.OperationalSPCs)
                   .Select("*")
                    .Where(new { Code = input.Code },  Tables.OperationalSPCs);

            var list = queryFactory.GetList<ListOperationalSPCsDto>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion


            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.OperationalSPCs).Update(new UpdateOperationalSPCsDto
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
                LastModificationTime = now,
                LastModifierId = LoginedUserService.UserId,
            }).Where(new { Id = input.Id }, "");

            foreach (var item in input.SelectOperationalSPCLines)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.OperationalSPCLines).Insert(new CreateOperationalSPCLinesDto
                    {
                        Detectability = item.Detectability,
                        Frequency = item.Frequency,
                        LineNr = item.LineNr,
                        OperationBasedMidControlFrequency = item.OperationBasedMidControlFrequency,
                        OperationID = item.OperationID.GetValueOrDefault(),
                        RPN = item.RPN,
                        Severity = item.Severity,
                        TotalComponent = item.TotalComponent,
                        TotalOccuredOperation = item.TotalOccuredOperation,
                        TotalUnsuitableComponent = item.TotalUnsuitableComponent,
                        TotalUnsuitableOperation = item.TotalUnsuitableOperation,
                        UnsuitableComponentPerOperation = item.UnsuitableComponentPerOperation,
                        UnsuitableComponentRate = item.UnsuitableComponentRate,
                        UnsuitableOperationRate = item.UnsuitableOperationRate,
                        WorkCenterID = item.WorkCenterID.GetValueOrDefault(),
                        OperationalSPCID = input.Id,
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
                    var lineGetQuery = queryFactory.Query().From(Tables.OperationalSPCLines).Select("*").Where(new { Id = item.Id },  "");

                    var line = queryFactory.Get<SelectOperationalSPCLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.OperationalSPCLines).Update(new UpdateOperationalSPCLinesDto
                        {
                            Detectability = item.Detectability,
                            Frequency = item.Frequency,
                            LineNr = item.LineNr,
                            OperationBasedMidControlFrequency = item.OperationBasedMidControlFrequency,
                            OperationID = item.OperationID.GetValueOrDefault(),
                            RPN = item.RPN,
                            Severity = item.Severity,
                            TotalComponent = item.TotalComponent,
                            TotalOccuredOperation = item.TotalOccuredOperation,
                            TotalUnsuitableComponent = item.TotalUnsuitableComponent,
                            TotalUnsuitableOperation = item.TotalUnsuitableOperation,
                            UnsuitableComponentPerOperation = item.UnsuitableComponentPerOperation,
                            UnsuitableComponentRate = item.UnsuitableComponentRate,
                            UnsuitableOperationRate = item.UnsuitableOperationRate,
                            WorkCenterID = item.WorkCenterID,
                            OperationalSPCID = input.Id,
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
                        }).Where(new { Id = line.Id }, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }

            var OperationalSPC = queryFactory.Update<SelectOperationalSPCsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.OperationalSPCs, LogType.Update, OperationalSPC.Id);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["OperationalSPCChildMenu"], L["ProcessRefresh"])).Data.FirstOrDefault();

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
            return new SuccessDataResult<SelectOperationalSPCsDto>(OperationalSPC);

        }

        public async Task<IDataResult<SelectOperationalSPCsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.OperationalSPCs).Select("*").Where(new { Id = id },  "");

            var entity = queryFactory.Get<OperationalSPCs>(entityQuery);

            var query = queryFactory.Query().From(Tables.OperationalSPCs).Update(new UpdateOperationalSPCsDto
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
            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id },  "");

            var OperationalSPCsDto = queryFactory.Update<SelectOperationalSPCsDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectOperationalSPCsDto>(OperationalSPCsDto);


        }
    }
}

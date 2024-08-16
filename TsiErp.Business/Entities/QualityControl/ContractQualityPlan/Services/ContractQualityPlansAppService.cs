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
using TsiErp.Business.Entities.QualityControl.ContractQualityPlan.Services;
using TsiErp.Business.Entities.QualityControl.ContractQualityPlan.Validations;
using TsiErp.Business.Extensions.DeleteControlExtension;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationGroup;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductsOperation;
using TsiErp.Entities.Entities.QualityControl.ContractOperationPicture.Dtos;
using TsiErp.Entities.Entities.QualityControl.ContractQualityPlan;
using TsiErp.Entities.Entities.QualityControl.ContractQualityPlan.Dtos;
using TsiErp.Entities.Entities.QualityControl.ContractQualityPlanLine;
using TsiErp.Entities.Entities.QualityControl.ContractQualityPlanLine.Dtos;
using TsiErp.Entities.Entities.QualityControl.ContractQualityPlanOperation;
using TsiErp.Entities.Entities.QualityControl.ContractQualityPlanOperation.Dtos;
using TsiErp.Entities.Entities.QualityControl.ControlCondition;
using TsiErp.Entities.Entities.QualityControl.ControlType;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.ContractQualityPlans.Page;

namespace TsiErp.Business.Entities.ContractQualityPlan.Services
{
    [ServiceRegistration(typeof(IContractQualityPlansAppService), DependencyInjectionType.Scoped)]
    public class ContractQualityPlansAppService : ApplicationService<ContractQualityPlansResource>, IContractQualityPlansAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;
        private readonly INotificationsAppService _NotificationsAppService;
        private readonly INotificationTemplatesAppService _NotificationTemplatesAppService;

        public ContractQualityPlansAppService(IStringLocalizer<ContractQualityPlansResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
            _NotificationsAppService = notificationsAppService;
            _NotificationTemplatesAppService = notificationTemplatesAppService;
        }

        [ValidationAspect(typeof(CreateContractQualityPlansValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectContractQualityPlansDto>> CreateAsync(CreateContractQualityPlansDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.ContractQualityPlans).Select("DocumentNumber").Where(new { DocumentNumber = input.DocumentNumber },  "");
            var list = queryFactory.ControlList<ContractQualityPlans>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();
            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.ContractQualityPlans).Insert(new CreateContractQualityPlansDto
            {
                CreationTime = now,
                CreatorId = LoginedUserService.UserId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = Guid.Empty,
                DeletionTime = null,
                Id = addedEntityId,
                Description_ = input.Description_,
                RevisionNo = input.RevisionNo,
                DocumentNumber = input.DocumentNumber,
                ProductID = input.ProductID.GetValueOrDefault(),
                CurrrentAccountCardID = input.CurrrentAccountCardID.GetValueOrDefault(),
                AcceptableNumberofDefectiveProduct = input.AcceptableNumberofDefectiveProduct,
                NumberofSampleinPart = input.NumberofSampleinPart,
                IsDeleted = false,
                LastModificationTime = null,
                LastModifierId = Guid.Empty,
            });

            foreach (var item in input.SelectContractQualityPlanLines)
            {
                var queryLine = queryFactory.Query().From(Tables.ContractQualityPlanLines).Insert(new CreateContractQualityPlanLinesDto
                {
                    ContractQualityPlanID = addedEntityId,
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
                    ProductID = item.ProductID,
                    Description_ = item.Description_,
                    BottomTolerance = item.BottomTolerance,
                    Code = item.Code,
                    ControlConditionsID = item.ControlConditionsID,
                    ControlFrequency = item.ControlFrequency,
                    ControlManager = item.ControlManager,
                    ControlTypesID = item.ControlTypesID,
                    Date_ = item.Date_,
                    Equipment = item.Equipment,
                    IdealMeasure = item.IdealMeasure,
                    LineNr = item.LineNr,
                    MeasureNumberInPicture = item.MeasureNumberInPicture,
                    PeriodicControlMeasure = item.PeriodicControlMeasure,
                    UpperTolerance = item.UpperTolerance,
                    WorkCenterID = item.WorkCenterID,

                });

                query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
            }

            foreach (var item in input.SelectContractOperationPictures)
            {
                var queryPicture = queryFactory.Query().From(Tables.ContractOperationPictures).Insert(new CreateContractOperationPicturesDto
                {
                    ContractQualityPlanID = addedEntityId,
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
                    LineNr = item.LineNr,
                    Approver = item.Approver,
                    CreationDate_ = _GetSQLDateAppService.GetDateFromSQL(),
                    Description_ = item.Description_,
                    Drawer = item.Drawer,
                    IsApproved = item.IsApproved,

                });

                query.Sql = query.Sql + QueryConstants.QueryConstant + queryPicture.Sql;
            }

            foreach (var item in input.SelectContractQualityPlanOperations)
            {
                var queryOperation = queryFactory.Query().From(Tables.ContractQualityPlanOperations).Insert(new CreateContractQualityPlanOperationsDto
                {
                    ContractQualityPlanID = addedEntityId,
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
                    LineNr = item.LineNr,
                    OperationID = item.OperationID.GetValueOrDefault()

                });

                query.Sql = query.Sql + QueryConstants.QueryConstant + queryOperation.Sql;
            }

            var ContractQualityPlan = queryFactory.Insert<SelectContractQualityPlansDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("ContractQualityPlansChildMenu", input.DocumentNumber);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.ContractQualityPlans, LogType.Insert, addedEntityId);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["ContractQualityPlansChildMenu"], L["ProcessAdd"])).Data.FirstOrDefault();

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
                                RecordNumber = input.RevisionNo,
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
                            RecordNumber = input.RevisionNo,
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
            return new SuccessDataResult<SelectContractQualityPlansDto>(ContractQualityPlan);

        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            DeleteControl.ControlList.Clear();

            DeleteControl.ControlList.Add("ContractQualityPlanID", new List<string>
            {
                Tables.ContractTrackingFiches
            });


            bool control = DeleteControl.Control(queryFactory, id);

            if (!control)
            {
                throw new Exception(L["DeleteControlManager"]);
            }
            else
            {
                var entity = (await GetAsync(id)).Data;
                var deleteQuery = queryFactory.Query().From(Tables.ContractQualityPlans).Delete(LoginedUserService.UserId).Where(new { Id = id },  "");

                var lineDeleteQuery = queryFactory.Query().From(Tables.ContractQualityPlanLines).Delete(LoginedUserService.UserId).Where(new { ContractQualityPlanID = id },  "");

                deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;


                var contractPictureDeleteQuery = queryFactory.Query().From(Tables.OperationPictures).Delete(LoginedUserService.UserId).Where(new { ContractQualityPlanID = id },  "");

                deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + contractPictureDeleteQuery.Sql + " where " + contractPictureDeleteQuery.WhereSentence;


                var contractOperationDeleteQuery = queryFactory.Query().From(Tables.ContractQualityPlanOperations).Delete(LoginedUserService.UserId).Where(new { ContractQualityPlanID = id },  "");

                deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + contractOperationDeleteQuery.Sql + " where " + contractOperationDeleteQuery.WhereSentence;

                var ContractQualityPlan = queryFactory.Update<SelectContractQualityPlansDto>(deleteQuery, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.ContractQualityPlans, LogType.Delete, id);
                #region Notification

                var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["ContractQualityPlansChildMenu"], L["ProcessDelete"])).Data.FirstOrDefault();

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
                                    RecordNumber = entity.RevisionNo,
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
                                RecordNumber = entity.RevisionNo,
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
                return new SuccessDataResult<SelectContractQualityPlansDto>(ContractQualityPlan);
            }
        }

        public async Task<IResult> DeleteLineAsync(Guid id)
        {
            var queryLine = queryFactory.Query().From(Tables.ContractQualityPlanLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");
            var ContractQualityPlanLines = queryFactory.Update<SelectContractQualityPlanLinesDto>(queryLine, "Id", true);
            LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.ContractQualityPlanLines, LogType.Delete, id);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectContractQualityPlanLinesDto>(ContractQualityPlanLines);

        }

        public async Task<IResult> DeleteContractPictureAsync(Guid id)
        {
            var queryContractPicture = queryFactory.Query().From(Tables.ContractOperationPictures).Delete(LoginedUserService.UserId).Where(new { Id = id },  "");
            var contractPictures = queryFactory.Update<SelectContractOperationPicturesDto>(queryContractPicture, "Id", true);
            LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.ContractOperationPictures, LogType.Delete, id);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectContractOperationPicturesDto>(contractPictures);

        }

        public async Task<IResult> DeleteContractOperationAsync(Guid id)
        {
            var queryContractOperation = queryFactory.Query().From(Tables.ContractQualityPlanOperations).Delete(LoginedUserService.UserId).Where(new { Id = id },  "");
            var contractOperations = queryFactory.Update<SelectContractQualityPlanOperationsDto>(queryContractOperation, "Id", true);
            LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.ContractQualityPlanOperations, LogType.Delete, id);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectContractQualityPlanOperationsDto>(contractOperations);

        }

        public async Task<IDataResult<SelectContractQualityPlansDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.ContractQualityPlans)
                   .Select<ContractQualityPlans>(null)
                   .Join<Products>
                    (
                        pr => new { ProductCode = pr.Code, ProductName = pr.Name, ProductID = pr.Id },
                        nameof(ContractQualityPlans.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        cac => new { CurrrentAccountCardCode = cac.Code, CurrrentAccountCardName = cac.Name, CurrrentAccountCardID = cac.Id },
                        nameof(ContractQualityPlans.CurrrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = id }, Tables.ContractQualityPlans);

            var contractQualityPlans = queryFactory.Get<SelectContractQualityPlansDto>(query);

            #region Satır Get

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.ContractQualityPlanLines)
                   .Select<ContractQualityPlanLines>(null)
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(ContractQualityPlanLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                    .Join<ControlTypes>
                    (
                        ct => new { ControlTypesName = ct.Name, ControlTypesID = ct.Id },
                        nameof(ContractQualityPlanLines.ControlTypesID),
                        nameof(ControlTypes.Id),
                        JoinType.Left
                    )
                     .Join<StationGroups>
                    (
                        sg => new { WorkCenterName = sg.Name, WorkCenterID = sg.Id },
                        nameof(ContractQualityPlanLines.WorkCenterID),
                        nameof(StationGroups.Id),
                        JoinType.Left
                    )
                     .Join<ControlConditions>
                    (
                        cc => new { ControlConditionsName = cc.Name, ControlConditionsID = cc.Id },
                        nameof(ContractQualityPlanLines.ControlConditionsID),
                        nameof(ControlConditions.Id),
                        JoinType.Left
                    )
                    .Where(new { ContractQualityPlanID = id }, Tables.ContractQualityPlanLines);

            var contractQualityPlanLine = queryFactory.GetList<SelectContractQualityPlanLinesDto>(queryLines).ToList();

            contractQualityPlans.SelectContractQualityPlanLines = contractQualityPlanLine;

            #endregion

            #region Operasyon Resmi Get

            var queryOperationPicture = queryFactory.Query().From(Tables.ContractOperationPictures).Select("*").Where(
           new
           {
               ContractQualityPlanID = id
           }, "");

            var operationPictures = queryFactory.GetList<SelectContractOperationPicturesDto>(queryOperationPicture).ToList();

            contractQualityPlans.SelectContractOperationPictures = operationPictures;

            #endregion

            #region Operasyon Satır Get

            var queryContractOperation = queryFactory.Query().From(Tables.ContractQualityPlanOperations).Select<ContractQualityPlanOperations>(null)
                  .Join<ProductsOperations>
                    (
                        sg => new { Name = sg.Name, Code = sg.Code, OperationID = sg.Id },
                        nameof(ContractQualityPlanOperations.OperationID),
                        nameof(ProductsOperations.Id),
                        JoinType.Left
                    )
                .Where(
           new
           {
               ContractQualityPlanID = id
           },  Tables.ContractQualityPlanOperations);

            var contractOperations = queryFactory.GetList<SelectContractQualityPlanOperationsDto>(queryContractOperation).ToList();

            contractQualityPlans.SelectContractQualityPlanOperations = contractOperations;

            #endregion

            LogsAppService.InsertLogToDatabase(contractQualityPlans, contractQualityPlans, LoginedUserService.UserId, Tables.ContractQualityPlans, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectContractQualityPlansDto>(contractQualityPlans);

        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListContractQualityPlansDto>>> GetListAsync(ListContractQualityPlansParameterDto input)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.ContractQualityPlans)
                   .Select<ContractQualityPlans>(s => new { s.RevisionNo, s.DocumentNumber, s.Id })
                   .Join<Products>
                    (
                        pr => new { ProductCode = pr.Code, ProductName = pr.Name },
                        nameof(ContractQualityPlans.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        cac => new { CurrrentAccountCardCode = cac.Code, CurrrentAccountCardName = cac.Name },
                        nameof(ContractQualityPlans.CurrrentAccountCardID),
                        nameof(ProductsOperations.Id),
                        JoinType.Left
                    )
                    .Where(null,  Tables.ContractQualityPlans);

            var contractQualityPlans = queryFactory.GetList<ListContractQualityPlansDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListContractQualityPlansDto>>(contractQualityPlans);

        }

        [ValidationAspect(typeof(UpdateContractQualityPlansValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectContractQualityPlansDto>> UpdateAsync(UpdateContractQualityPlansDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.ContractQualityPlans).Select("*").Where(new { Id = input.Id },  "");
            var entity = queryFactory.Get<ContractQualityPlans>(entityQuery);

            #region Update Control

            var listQuery = queryFactory.Query().From(Tables.ContractQualityPlans).Select("*").Where(new { DocumentNumber = input.DocumentNumber },  "");
            var list = queryFactory.GetList<ContractQualityPlans>(listQuery).ToList();

            if (list.Count > 0 && entity.DocumentNumber != input.DocumentNumber)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion


            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.ContractQualityPlans).Update(new UpdateContractQualityPlansDto
            {
                CreationTime = entity.CreationTime.GetValueOrDefault(),
                CreatorId = entity.CreatorId.GetValueOrDefault(),
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                Id = input.Id,
                IsDeleted = entity.IsDeleted,
                LastModificationTime = now,
                Description_ = input.Description_,
                RevisionNo = input.RevisionNo,
                DocumentNumber = input.DocumentNumber,
                ProductID = input.ProductID.GetValueOrDefault(),
                CurrrentAccountCardID = input.CurrrentAccountCardID.GetValueOrDefault(),
                NumberofSampleinPart = input.NumberofSampleinPart,
                AcceptableNumberofDefectiveProduct = input.AcceptableNumberofDefectiveProduct,
                LastModifierId = LoginedUserService.UserId,
            }).Where(new { Id = input.Id },  "");

            foreach (var item in input.SelectContractQualityPlanLines)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.ContractQualityPlanLines).Insert(new CreateContractQualityPlanLinesDto
                    {
                        ContractQualityPlanID = input.Id,
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
                        LineNr = item.LineNr,
                        ProductID = item.ProductID,
                        BottomTolerance = item.BottomTolerance,
                        Code = item.Code,
                        Description_ = item.Description_,
                        ControlConditionsID = item.ControlConditionsID,
                        ControlFrequency = item.ControlFrequency,
                        ControlManager = item.ControlManager,
                        ControlTypesID = item.ControlTypesID,
                        Date_ = item.Date_,
                        Equipment = item.Equipment,
                        IdealMeasure = item.IdealMeasure,
                        MeasureNumberInPicture = item.MeasureNumberInPicture,
                        PeriodicControlMeasure = item.PeriodicControlMeasure,
                        UpperTolerance = item.UpperTolerance,
                        WorkCenterID = item.WorkCenterID,
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql;
                }
                else
                {
                    var lineGetQuery = queryFactory.Query().From(Tables.ContractQualityPlanLines).Select("*").Where(new { Id = item.Id },  "");

                    var line = queryFactory.Get<SelectContractQualityPlanLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.ContractQualityPlanLines).Update(new UpdateContractQualityPlanLinesDto
                        {
                            ContractQualityPlanID = input.Id,
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
                            LineNr = item.LineNr,
                            ProductID = item.ProductID,
                            BottomTolerance = item.BottomTolerance,
                            Code = item.Code,
                            Description_ = item.Description_,
                            ControlConditionsID = item.ControlConditionsID,
                            ControlFrequency = item.ControlFrequency,
                            ControlManager = item.ControlManager,
                            ControlTypesID = item.ControlTypesID,
                            Date_ = item.Date_,
                            Equipment = item.Equipment,
                            IdealMeasure = item.IdealMeasure,
                            MeasureNumberInPicture = item.MeasureNumberInPicture,
                            PeriodicControlMeasure = item.PeriodicControlMeasure,
                            UpperTolerance = item.UpperTolerance,
                            WorkCenterID = item.WorkCenterID,
                        }).Where(new { Id = line.Id }, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }

            foreach (var item in input.SelectContractOperationPictures)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryOperaionPicture = queryFactory.Query().From(Tables.ContractOperationPictures).Insert(new CreateContractOperationPicturesDto
                    {
                        ContractQualityPlanID = input.Id,
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
                        LineNr = item.LineNr,
                        Approver = item.Approver,
                        CreationDate_ = item.CreationDate_,
                        Description_ = item.Description_,
                        Drawer = item.Drawer,
                        IsApproved = item.IsApproved,
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryOperaionPicture.Sql;
                }
                else
                {
                    var operationPictureGetQuery = queryFactory.Query().From(Tables.ContractOperationPictures).Select("*").Where(new { Id = item.Id }, "");

                    var operationPicture = queryFactory.Get<SelectContractOperationPicturesDto>(operationPictureGetQuery);

                    if (operationPicture != null)
                    {
                        var queryOperaionPicture = queryFactory.Query().From(Tables.ContractOperationPictures).Update(new UpdateContractOperationPicturesDto
                        {
                            ContractQualityPlanID = input.Id,
                            CreationTime = operationPicture.CreationTime,
                            CreatorId = operationPicture.CreatorId,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = operationPicture.DeleterId.GetValueOrDefault(),
                            DeletionTime = operationPicture.DeletionTime.GetValueOrDefault(),
                            Id = item.Id,
                            IsDeleted = item.IsDeleted,
                            LastModificationTime = now,
                            LastModifierId = LoginedUserService.UserId,
                            LineNr = item.LineNr,
                            Approver = item.Approver,
                            CreationDate_ = item.CreationDate_,
                            Description_ = item.Description_,
                            Drawer = item.Drawer,
                            IsApproved = item.IsApproved,
                        }).Where(new { Id = operationPicture.Id },"");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryOperaionPicture.Sql + " where " + queryOperaionPicture.WhereSentence;
                    }
                }


            }

            foreach (var item in input.SelectContractQualityPlanOperations)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryContractOperations = queryFactory.Query().From(Tables.ContractQualityPlanOperations).Insert(new CreateContractQualityPlanOperationsDto
                    {
                        ContractQualityPlanID = input.Id,
                        OperationID = item.OperationID,
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
                        LineNr = item.LineNr,
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryContractOperations.Sql;
                }
                else
                {
                    var contractOperationGetQuery = queryFactory.Query().From(Tables.ContractQualityPlanOperations).Select("*").Where(new { Id = item.Id },  "");

                    var contractOperation = queryFactory.Get<SelectContractQualityPlanOperationsDto>(contractOperationGetQuery);

                    if (contractOperation != null)
                    {
                        var queryContractOperations = queryFactory.Query().From(Tables.ContractQualityPlanOperations).Update(new UpdateContractQualityPlanOperationsDto
                        {
                            ContractQualityPlanID = input.Id,
                            OperationID = item.OperationID,
                            CreationTime = contractOperation.CreationTime,
                            CreatorId = contractOperation.CreatorId,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = contractOperation.DeleterId.GetValueOrDefault(),
                            DeletionTime = contractOperation.DeletionTime.GetValueOrDefault(),
                            Id = item.Id,
                            IsDeleted = item.IsDeleted,
                            LastModificationTime =now,
                            LastModifierId = LoginedUserService.UserId,
                            LineNr = item.LineNr,
                        }).Where(new { Id = contractOperation.Id }, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryContractOperations.Sql + " where " + queryContractOperations.WhereSentence;
                    }
                }


            }

            var ContractQualityPlan = queryFactory.Update<SelectContractQualityPlansDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.ContractQualityPlans, LogType.Update, entity.Id);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["ContractQualityPlansChildMenu"], L["ProcessRefresh"])).Data.FirstOrDefault();

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
                                RecordNumber = input.RevisionNo,
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
                            RecordNumber = input.RevisionNo,
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
            return new SuccessDataResult<SelectContractQualityPlansDto>(ContractQualityPlan);

        }

        public async Task<IDataResult<SelectContractQualityPlansDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.ContractQualityPlans).Select("*").Where(new { Id = id }, "");

            var entity = queryFactory.Get<ContractQualityPlans>(entityQuery);

            var query = queryFactory.Query().From(Tables.ContractQualityPlans).Update(new UpdateContractQualityPlansDto
            {
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DataOpenStatus = lockRow,
                DataOpenStatusUserId = userId,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                RevisionNo = entity.RevisionNo,
                Id = entity.Id,
                IsDeleted = entity.IsDeleted,
                LastModificationTime = entity.LastModificationTime.GetValueOrDefault(),
                LastModifierId = entity.LastModifierId.GetValueOrDefault(),
                Description_ = entity.Description_,
                DocumentNumber = entity.DocumentNumber,
                ProductID = entity.ProductID,
                AcceptableNumberofDefectiveProduct = entity.AcceptableNumberofDefectiveProduct,
                CurrrentAccountCardID = entity.CurrrentAccountCardID,
                NumberofSampleinPart = entity.NumberofSampleinPart,

            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id }, "");

            var ContractQualityPlansDto = queryFactory.Update<SelectContractQualityPlansDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectContractQualityPlansDto>(ContractQualityPlansDto);


        }
    }
}

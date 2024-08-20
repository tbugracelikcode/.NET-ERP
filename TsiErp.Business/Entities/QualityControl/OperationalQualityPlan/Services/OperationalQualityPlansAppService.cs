using Microsoft.CodeAnalysis;
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
using TsiErp.Business.Entities.QualityControl.OperationalQualityPlan.Services;
using TsiErp.Business.Entities.QualityControl.OperationalQualityPlan.Validations;
using TsiErp.Business.Extensions.DeleteControlExtension;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationGroup;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductsOperation;
using TsiErp.Entities.Entities.QualityControl.ControlCondition;
using TsiErp.Entities.Entities.QualityControl.ControlType;
using TsiErp.Entities.Entities.QualityControl.OperationalQualityPlan;
using TsiErp.Entities.Entities.QualityControl.OperationalQualityPlan.Dtos;
using TsiErp.Entities.Entities.QualityControl.OperationalQualityPlanLine;
using TsiErp.Entities.Entities.QualityControl.OperationalQualityPlanLine.Dtos;
using TsiErp.Entities.Entities.QualityControl.OperationPicture.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.OperationalQualityPlans.Page;

namespace TsiErp.Business.Entities.OperationalQualityPlan.Services
{
    [ServiceRegistration(typeof(IOperationalQualityPlansAppService), DependencyInjectionType.Scoped)]
    public class OperationalQualityPlansAppService : ApplicationService<OperationalQualityPlansResource>, IOperationalQualityPlansAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;
        private readonly INotificationsAppService _NotificationsAppService;
        private readonly INotificationTemplatesAppService _NotificationTemplatesAppService;


        public OperationalQualityPlansAppService(IStringLocalizer<OperationalQualityPlansResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
            _NotificationsAppService = notificationsAppService;
            _NotificationTemplatesAppService = notificationTemplatesAppService;
        }

        [ValidationAspect(typeof(CreateOperationalQualityPlansValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectOperationalQualityPlansDto>> CreateAsync(CreateOperationalQualityPlansDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.OperationalQualityPlans).Select("DocumentNumber").Where(new { DocumentNumber = input.DocumentNumber }, "");
            var list = queryFactory.ControlList<OperationalQualityPlans>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();
            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.OperationalQualityPlans).Insert(new CreateOperationalQualityPlansDto
            {
                CreationTime = now,
                CreatorId = LoginedUserService.UserId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = Guid.Empty,
                DeletionTime = null,
                Id = addedEntityId,
                Description_ = input.Description_,
                DocumentNumber = input.DocumentNumber,
                ProductID = input.ProductID.GetValueOrDefault(),
                ProductsOperationID = input.ProductsOperationID.GetValueOrDefault(),
                IsDeleted = false,
                LastModificationTime = null,
                LastModifierId = Guid.Empty,
            });

            foreach (var item in input.SelectOperationalQualityPlanLines)
            {
                var queryLine = queryFactory.Query().From(Tables.OperationalQualityPlanLines).Insert(new CreateOperationalQualityPlanLinesDto
                {
                    OperationalQualityPlanID = addedEntityId,
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
                    ProductsOperationID = item.ProductsOperationID,
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

            foreach (var item in input.SelectOperationPictures)
            {
                var queryPicture = queryFactory.Query().From(Tables.OperationPictures).Insert(new CreateOperationPicturesDto
                {
                    OperationalQualityPlanID = addedEntityId,
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
                    CreationDate_ = _GetSQLDateAppService.GetDateFromSQL(),
                    Description_ = item.Description_,
                    Drawer = item.Drawer,
                    IsApproved = item.IsApproved,
                    DrawingDomain = item.DrawingDomain,
                    DrawingFilePath = item.DrawingFilePath,
                    UploadedFileName = item.UploadedFileName,
                     RevisionNo= item.RevisionNo
                });

                query.Sql = query.Sql + QueryConstants.QueryConstant + queryPicture.Sql;
            }

            var operationalQualityPlan = queryFactory.Insert<SelectOperationalQualityPlansDto>(query, "Id", true);

            var logInput = input;

            logInput.SelectOperationPictures.Clear();
            logInput.SelectOperationPictures = new List<SelectOperationPicturesDto>();



            await FicheNumbersAppService.UpdateFicheNumberAsync("OperationalQualityPlansChildMenu", input.DocumentNumber);

            LogsAppService.InsertLogToDatabase(logInput, logInput, LoginedUserService.UserId, Tables.OperationalQualityPlans, LogType.Insert, addedEntityId);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["OperationalQualityPlansChildMenu"], L["ProcessAdd"])).Data.FirstOrDefault();

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
            return new SuccessDataResult<SelectOperationalQualityPlansDto>(operationalQualityPlan);

        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            DeleteControl.ControlList.Clear();

            DeleteControl.ControlList.Add("OperationQualityPlanID", new List<string>
            {
                Tables.FirstProductApprovals
            });


            bool control = DeleteControl.Control(queryFactory, id);

            if (!control)
            {
                throw new Exception(L["DeleteControlManager"]);
            }
            else
            {
                var entity = (await GetAsync(id)).Data;
                var deleteQuery = queryFactory.Query().From(Tables.OperationalQualityPlans).Delete(LoginedUserService.UserId).Where(new { Id = id },  "");

                var lineDeleteQuery = queryFactory.Query().From(Tables.OperationalQualityPlanLines).Delete(LoginedUserService.UserId).Where(new { OperationalQualityPlanID = id },  "");

                deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + lineDeleteQuery.Sql + " where " + lineDeleteQuery.WhereSentence;

                var operationPictureDeleteQuery = queryFactory.Query().From(Tables.OperationPictures).Delete(LoginedUserService.UserId).Where(new { OperationalQualityPlanID = id }, "");

                deleteQuery.Sql = deleteQuery.Sql + QueryConstants.QueryConstant + operationPictureDeleteQuery.Sql + " where " + operationPictureDeleteQuery.WhereSentence;

                var operationalQualityPlan = queryFactory.Update<SelectOperationalQualityPlansDto>(deleteQuery, "Id", true);
                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.OperationalQualityPlans, LogType.Delete, id);
                #region Notification

                var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["OperationalQualityPlansChildMenu"], L["ProcessDelete"])).Data.FirstOrDefault();

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
                return new SuccessDataResult<SelectOperationalQualityPlansDto>(operationalQualityPlan);
            }
        }

        public async Task<IResult> DeleteLineAsync(Guid id)
        {
            var queryLine = queryFactory.Query().From(Tables.OperationalQualityPlanLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

            var operationalQualityPlanLines = queryFactory.Update<SelectOperationalQualityPlanLinesDto>(queryLine, "Id", true);

            LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.OperationalQualityPlanLines, LogType.Delete, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectOperationalQualityPlanLinesDto>(operationalQualityPlanLines);

        }

        public async Task<IResult> DeleteOperationPictureAsync(Guid id)
        {
            var queryOperationPicture = queryFactory.Query().From(Tables.OperationPictures).Delete(LoginedUserService.UserId).Where(new { Id = id },  "");

            var operationPictures = queryFactory.Update<SelectOperationPicturesDto>(queryOperationPicture, "Id", true);

            LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.OperationPictures, LogType.Delete, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectOperationPicturesDto>(operationPictures);

        }

        public async Task<IDataResult<SelectOperationalQualityPlansDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.OperationalQualityPlans)
                   .Select<OperationalQualityPlans>(null)
                   .Join<Products>
                    (
                        pr => new { ProductCode = pr.Code, ProductName = pr.Name, ProductID = pr.Id },
                        nameof(OperationalQualityPlans.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                    .Join<ProductsOperations>
                    (
                        pro => new { OperationCode = pro.Code, OperationName = pro.Name, ProductsOperationID = pro.Id },
                        nameof(OperationalQualityPlans.ProductsOperationID),
                        nameof(ProductsOperations.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = id },Tables.OperationalQualityPlans);

            var operationalQualityPlans = queryFactory.Get<SelectOperationalQualityPlansDto>(query);

            #region Satır Get

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.OperationalQualityPlanLines)
                   .Select<OperationalQualityPlanLines>(null)
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(OperationalQualityPlanLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<ProductsOperations>
                    (
                        po => new { OperationCode = po.Code, OperationName = po.Name, ProductsOperationID = po.Id },
                        nameof(OperationalQualityPlanLines.ProductsOperationID),
                        nameof(ProductsOperations.Id),
                        JoinType.Left
                    )
                    .Join<ControlTypes>
                    (
                        ct => new { ControlTypesName = ct.Name, ControlTypesID = ct.Id },
                        nameof(OperationalQualityPlanLines.ControlTypesID),
                        nameof(ControlTypes.Id),
                        JoinType.Left
                    )
                     .Join<StationGroups>
                    (
                        sg => new { WorkCenterName = sg.Name, WorkCenterID = sg.Id },
                        nameof(OperationalQualityPlanLines.WorkCenterID),
                        nameof(StationGroups.Id),
                        JoinType.Left
                    )
                     .Join<ControlConditions>
                    (
                        cc => new { ControlConditionsName = cc.Name, ControlConditionsID = cc.Id },
                        nameof(OperationalQualityPlanLines.ControlConditionsID),
                        nameof(ControlConditions.Id),
                        JoinType.Left
                    )
                    .Where(new { OperationalQualityPlanID = id },  Tables.OperationalQualityPlanLines);

            var OperationalQualityPlanLine = queryFactory.GetList<SelectOperationalQualityPlanLinesDto>(queryLines).ToList();

            operationalQualityPlans.SelectOperationalQualityPlanLines = OperationalQualityPlanLine;

            #endregion

            #region Operasyon Resmi Get

            var queryOperationPicture = queryFactory.Query().From(Tables.OperationPictures).Select("*").Where(
           new
           {
               OperationalQualityPlanID = id
           }, "");

            var operationPictures = queryFactory.GetList<SelectOperationPicturesDto>(queryOperationPicture).ToList();

            operationalQualityPlans.SelectOperationPictures = operationPictures;

            #endregion

            LogsAppService.InsertLogToDatabase(operationalQualityPlans, operationalQualityPlans, LoginedUserService.UserId, Tables.OperationalQualityPlans, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectOperationalQualityPlansDto>(operationalQualityPlans);

        }

        public async Task<IDataResult<SelectOperationalQualityPlansDto>> GetbyOperationProductAsync(Guid operationID, Guid productID)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.OperationalQualityPlans)
                   .Select<OperationalQualityPlans>(null)
                   .Join<Products>
                    (
                        pr => new { ProductCode = pr.Code, ProductName = pr.Name, ProductID = pr.Id },
                        nameof(OperationalQualityPlans.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                    .Join<ProductsOperations>
                    (
                        pro => new { OperationCode = pro.Code, OperationName = pro.Name, ProductsOperationID = pro.Id },
                        nameof(OperationalQualityPlans.ProductsOperationID),
                        nameof(ProductsOperations.Id),
                        JoinType.Left
            )
                    .Where(new { ProductsOperationID = operationID , ProductID = productID }, Tables.OperationalQualityPlans);

            var operationalQualityPlans = queryFactory.Get<SelectOperationalQualityPlansDto>(query);

            #region Satır Get

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.OperationalQualityPlanLines)
                   .Select<OperationalQualityPlanLines>(null)
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(OperationalQualityPlanLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                   .Join<ProductsOperations>
                    (
                        po => new { OperationCode = po.Code, OperationName = po.Name, ProductsOperationID = po.Id },
                        nameof(OperationalQualityPlanLines.ProductsOperationID),
                        nameof(ProductsOperations.Id),
                        JoinType.Left
                    )
                    .Join<ControlTypes>
                    (
                        ct => new { ControlTypesName = ct.Name, ControlTypesID = ct.Id },
                        nameof(OperationalQualityPlanLines.ControlTypesID),
                        nameof(ControlTypes.Id),
                        JoinType.Left
                    )
                     .Join<StationGroups>
                    (
                        sg => new { WorkCenterName = sg.Name, WorkCenterID = sg.Id },
                        nameof(OperationalQualityPlanLines.WorkCenterID),
                        nameof(StationGroups.Id),
                        JoinType.Left
                    )
                     .Join<ControlConditions>
                    (
                        cc => new { ControlConditionsName = cc.Name, ControlConditionsID = cc.Id },
                        nameof(OperationalQualityPlanLines.ControlConditionsID),
                        nameof(ControlConditions.Id),
                        JoinType.Left
                    )
                    .Where(new { OperationalQualityPlanID = operationalQualityPlans.Id }, Tables.OperationalQualityPlanLines);

            var OperationalQualityPlanLine = queryFactory.GetList<SelectOperationalQualityPlanLinesDto>(queryLines).ToList();

            operationalQualityPlans.SelectOperationalQualityPlanLines = OperationalQualityPlanLine;

            #endregion

            #region Operasyon Resmi Get

            var queryOperationPicture = queryFactory.Query().From(Tables.OperationPictures).Select("*").Where(
           new
           {
               OperationalQualityPlanID = operationalQualityPlans.Id
           }, "");

            var operationPictures = queryFactory.GetList<SelectOperationPicturesDto>(queryOperationPicture).ToList();

            operationalQualityPlans.SelectOperationPictures = operationPictures;

            #endregion

            LogsAppService.InsertLogToDatabase(operationalQualityPlans, operationalQualityPlans, LoginedUserService.UserId, Tables.OperationalQualityPlans, LogType.Get, operationalQualityPlans.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectOperationalQualityPlansDto>(operationalQualityPlans);

        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListOperationalQualityPlansDto>>> GetListAsync(ListOperationalQualityPlansParameterDto input)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.OperationalQualityPlans)
                   .Select<OperationalQualityPlans>(s => new { s.DocumentNumber, s.Id })
                   .Join<Products>
                    (
                        pr => new { ProductCode = pr.Code, ProductName = pr.Name, ProductID = pr.Id },
                        nameof(OperationalQualityPlans.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                    .Join<ProductsOperations>
                    (
                        pro => new { OperationCode = pro.Code, OperationName = pro.Name, ProductsOperationID = pro.Id },
                        nameof(OperationalQualityPlans.ProductsOperationID),
                        nameof(ProductsOperations.Id),
                        JoinType.Left
                    )
                    .Where(null, Tables.OperationalQualityPlans);

            var operationalQualityPlans = queryFactory.GetList<ListOperationalQualityPlansDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListOperationalQualityPlansDto>>(operationalQualityPlans);

        }

        [ValidationAspect(typeof(UpdateOperationalQualityPlansValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectOperationalQualityPlansDto>> UpdateAsync(UpdateOperationalQualityPlansDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.OperationalQualityPlans).Select("*").Where(new { Id = input.Id },"");
            var entity = queryFactory.Get<OperationalQualityPlans>(entityQuery);

            #region Update Control

            var listQuery = queryFactory.Query().From(Tables.OperationalQualityPlans).Select("*").Where(new { DocumentNumber = input.DocumentNumber },  "");
            var list = queryFactory.GetList<OperationalQualityPlans>(listQuery).ToList();

            if (list.Count > 0 && entity.DocumentNumber != input.DocumentNumber)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.OperationalQualityPlans).Update(new UpdateOperationalQualityPlansDto
            {
                CreationTime = entity.CreationTime.GetValueOrDefault(),
                CreatorId = entity.CreatorId.GetValueOrDefault(),
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                Id = input.Id,
                IsDeleted = entity.IsDeleted,
                LastModificationTime =now,
                Description_ = input.Description_,
                DocumentNumber = input.DocumentNumber,
                ProductID = input.ProductID.GetValueOrDefault(),
                ProductsOperationID = input.ProductsOperationID.GetValueOrDefault(),
                LastModifierId = LoginedUserService.UserId,
            }).Where(new { Id = input.Id }, "");

            foreach (var item in input.SelectOperationalQualityPlanLines)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.OperationalQualityPlanLines).Insert(new CreateOperationalQualityPlanLinesDto
                    {
                        OperationalQualityPlanID = input.Id,
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
                        ProductsOperationID = item.ProductsOperationID,
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
                    var lineGetQuery = queryFactory.Query().From(Tables.OperationalQualityPlanLines).Select("*").Where(new { Id = item.Id }, "");

                    var line = queryFactory.Get<SelectOperationalQualityPlanLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.OperationalQualityPlanLines).Update(new UpdateOperationalQualityPlanLinesDto
                        {
                            OperationalQualityPlanID = input.Id,
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
                            ProductsOperationID = item.ProductsOperationID,
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

            foreach (var item in input.SelectOperationPictures)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryOperaionPicture = queryFactory.Query().From(Tables.OperationPictures).Insert(new CreateOperationPicturesDto
                    {
                        OperationalQualityPlanID = input.Id,
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
                        DrawingDomain = item.DrawingDomain,
                        DrawingFilePath = item.DrawingFilePath,
                        UploadedFileName = item.UploadedFileName,
                         RevisionNo = item.RevisionNo
                    });

                    query.Sql = query.Sql + QueryConstants.QueryConstant + queryOperaionPicture.Sql;
                }
                else
                {
                    var operationPictureGetQuery = queryFactory.Query().From(Tables.OperationPictures).Select("*").Where(new { Id = item.Id }, "");

                    var operationPicture = queryFactory.Get<SelectOperationPicturesDto>(operationPictureGetQuery);

                    if (operationPicture != null)
                    {
                        var queryOperaionPicture = queryFactory.Query().From(Tables.OperationPictures).Update(new UpdateOperationPicturesDto
                        {
                            OperationalQualityPlanID = input.Id,
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
                            DrawingDomain = item.DrawingDomain,
                            DrawingFilePath = item.DrawingFilePath,
                            UploadedFileName = item.UploadedFileName,
                             RevisionNo = item.RevisionNo
                        }).Where(new { Id = operationPicture.Id }, "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryOperaionPicture.Sql + " where " + queryOperaionPicture.WhereSentence;
                    }
                }
            }

            var operationalQualityPlan = queryFactory.Update<SelectOperationalQualityPlansDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.OperationalQualityPlans, LogType.Update, entity.Id);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["OperationalQualityPlansChildMenu"], L["ProcessRefresh"])).Data.FirstOrDefault();

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
            return new SuccessDataResult<SelectOperationalQualityPlansDto>(operationalQualityPlan);

        }

        public async Task<IDataResult<SelectOperationalQualityPlansDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.OperationalQualityPlans).Select("*").Where(new { Id = id }, "");

            var entity = queryFactory.Get<OperationalQualityPlans>(entityQuery);

            var query = queryFactory.Query().From(Tables.OperationalQualityPlans).Update(new UpdateOperationalQualityPlansDto
            {
                CreationTime = entity.CreationTime.GetValueOrDefault(),
                CreatorId = entity.CreatorId.GetValueOrDefault(),
                DataOpenStatus = lockRow,
                DataOpenStatusUserId = userId,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                Id = entity.Id,
                IsDeleted = entity.IsDeleted,
                LastModificationTime = entity.LastModificationTime.GetValueOrDefault(),
                LastModifierId = entity.LastModifierId.GetValueOrDefault(),
                Description_ = entity.Description_,
                DocumentNumber = entity.DocumentNumber,
                ProductID = entity.ProductID,
                ProductsOperationID = entity.ProductsOperationID

            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id }, "");

            var operationalQualityPlansDto = queryFactory.Update<SelectOperationalQualityPlansDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectOperationalQualityPlansDto>(operationalQualityPlansDto);

        }

        public async Task<int> RevisionNoControlAsync(Guid operationQualityControlPlanId, string revisionNo)
        {
            var query = queryFactory.Query().From(Tables.OperationPictures).Count("Id").Where(new { OperationalQualityPlanID= operationQualityControlPlanId, RevisionNo= revisionNo },"");

            var result = queryFactory.Get<int>(query);

            await Task.CompletedTask;

            return result;
        }
    }
}

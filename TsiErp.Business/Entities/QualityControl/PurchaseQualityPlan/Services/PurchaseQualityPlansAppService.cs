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
using TsiErp.Business.Entities.QualityControl.PurchaseQualityPlan.Services;
using TsiErp.Business.Entities.QualityControl.PurchaseQualityPlan.Validations;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationGroup;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductsOperation;
using TsiErp.Entities.Entities.QualityControl.ControlCondition;
using TsiErp.Entities.Entities.QualityControl.ControlType;
using TsiErp.Entities.Entities.QualityControl.PurchaseQualityPlan;
using TsiErp.Entities.Entities.QualityControl.PurchaseQualityPlan.Dtos;
using TsiErp.Entities.Entities.QualityControl.PurchaseQualityPlanLine;
using TsiErp.Entities.Entities.QualityControl.PurchaseQualityPlanLine.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.PurchaseQualityPlans.Page;

namespace TsiErp.Business.Entities.PurchaseQualityPlan.Services
{
    [ServiceRegistration(typeof(IPurchaseQualityPlansAppService), DependencyInjectionType.Scoped)]
    public class PurchaseQualityPlansAppService : ApplicationService<PurchaseQualityPlansResource>, IPurchaseQualityPlansAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private readonly IGetSQLDateAppService _GetSQLDateAppService;
        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly INotificationsAppService _NotificationsAppService;
        private readonly INotificationTemplatesAppService _NotificationTemplatesAppService;

        public PurchaseQualityPlansAppService(IStringLocalizer<PurchaseQualityPlansResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
            _NotificationsAppService = notificationsAppService;
            _NotificationTemplatesAppService = notificationTemplatesAppService;
        }

        [ValidationAspect(typeof(CreatePurchaseQualityPlansValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPurchaseQualityPlansDto>> CreateAsync(CreatePurchaseQualityPlansDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.PurchaseQualityPlans).Select("DocumentNumber").Where(new { DocumentNumber = input.DocumentNumber },  "");
            var list = queryFactory.ControlList<PurchaseQualityPlans>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            var query = queryFactory.Query().From(Tables.PurchaseQualityPlans).Insert(new CreatePurchaseQualityPlansDto
            {
                CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
                CreatorId = LoginedUserService.UserId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = Guid.Empty,
                DeletionTime = null,
                RevisionNo = input.RevisionNo,
                Id = addedEntityId,
                Description_ = input.Description_,
                DocumentNumber = input.DocumentNumber,
                ProductID = input.ProductID.GetValueOrDefault(),
                CurrrentAccountCardID = input.CurrrentAccountCardID.GetValueOrDefault(),
                AcceptableNumberofDefectiveProduct = input.AcceptableNumberofDefectiveProduct,
                NumberofSampleinPart = input.NumberofSampleinPart,
                IsDeleted = false,
                LastModificationTime = null,
                LastModifierId = Guid.Empty,
            });

            foreach (var item in input.SelectPurchaseQualityPlanLines)
            {
                var queryLine = queryFactory.Query().From(Tables.PurchaseQualityPlanLines).Insert(new CreatePurchaseQualityPlanLinesDto
                {
                    PurchaseQualityPlanID = addedEntityId,
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

            var PurchaseQualityPlan = queryFactory.Insert<SelectPurchaseQualityPlansDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("PurchaseQualityPlansChildMenu", input.DocumentNumber);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.PurchaseQualityPlans, LogType.Insert, addedEntityId);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["PurchaseQualityPlansChildMenu"], L["ProcessAdd"])).Data.FirstOrDefault();

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
            return new SuccessDataResult<SelectPurchaseQualityPlansDto>(PurchaseQualityPlan);

        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var entity = (await GetAsync(id)).Data;
            var deleteQuery = queryFactory.Query().From(Tables.PurchaseQualityPlans).Delete(LoginedUserService.UserId).Where(new { Id = id },  "");

            var lineDeleteQuery = queryFactory.Query().From(Tables.PurchaseQualityPlanLines).Delete(LoginedUserService.UserId).Where(new { PurchaseQualityPlanID = id },  "");

            var PurchaseQualityPlan = queryFactory.Update<SelectPurchaseQualityPlansDto>(deleteQuery, "Id", true);
            var PurchaseQualityPlanLines = queryFactory.Update<SelectPurchaseQualityPlanLinesDto>(lineDeleteQuery, "Id", true);
            LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.PurchaseQualityPlans, LogType.Delete, id);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["PurchaseQualityPlansChildMenu"], L["ProcessDelete"])).Data.FirstOrDefault();

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
            return new SuccessDataResult<SelectPurchaseQualityPlansDto>(PurchaseQualityPlan);

        }

        public async Task<IResult> DeleteLineAsync(Guid id)
        {
            var queryLine = queryFactory.Query().From(Tables.PurchaseQualityPlanLines).Delete(LoginedUserService.UserId).Where(new { Id = id },  "");
            var PurchaseQualityPlanLines = queryFactory.Update<SelectPurchaseQualityPlanLinesDto>(queryLine, "Id", true);
            LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.PurchaseQualityPlanLines, LogType.Delete, id);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectPurchaseQualityPlanLinesDto>(PurchaseQualityPlanLines);

        }

        public async Task<IDataResult<SelectPurchaseQualityPlansDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.PurchaseQualityPlans)
                   .Select<PurchaseQualityPlans>(null)
                   .Join<Products>
                    (
                        pr => new { ProductCode = pr.Code, ProductName = pr.Name, ProductID = pr.Id },
                        nameof(PurchaseQualityPlans.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        cac => new { CurrrentAccountCardCode = cac.Code, CurrrentAccountCardName = cac.Name, CurrrentAccountCardID = cac.Id },
                        nameof(PurchaseQualityPlans.CurrrentAccountCardID),
                        nameof(CurrentAccountCards.Id),
                        JoinType.Left
                    )
                    .Where(new { Id = id },  Tables.PurchaseQualityPlans);

            var purchaseQualityPlans = queryFactory.Get<SelectPurchaseQualityPlansDto>(query);

            #region Satır Get

            var queryLines = queryFactory
                   .Query()
                   .From(Tables.PurchaseQualityPlanLines)
                   .Select<PurchaseQualityPlanLines>(null)
                   .Join<Products>
                    (
                        p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                        nameof(PurchaseQualityPlanLines.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                    .Join<ControlTypes>
                    (
                        ct => new { ControlTypesName = ct.Name, ControlTypesID = ct.Id },
                        nameof(PurchaseQualityPlanLines.ControlTypesID),
                        nameof(ControlTypes.Id),
                        JoinType.Left
                    )
                     .Join<StationGroups>
                    (
                        sg => new { WorkCenterName = sg.Name, WorkCenterID = sg.Id },
                        nameof(PurchaseQualityPlanLines.WorkCenterID),
                        nameof(StationGroups.Id),
                        JoinType.Left
                    )
                     .Join<ControlConditions>
                    (
                        cc => new { ControlConditionsName = cc.Name, ControlConditionsID = cc.Id },
                        nameof(PurchaseQualityPlanLines.ControlConditionsID),
                        nameof(ControlConditions.Id),
                        JoinType.Left
                    )
                    .Where(new { PurchaseQualityPlanID = id },  Tables.PurchaseQualityPlanLines);

            var purchaseQualityPlanLine = queryFactory.GetList<SelectPurchaseQualityPlanLinesDto>(queryLines).ToList();

            purchaseQualityPlans.SelectPurchaseQualityPlanLines = purchaseQualityPlanLine;

            #endregion



            LogsAppService.InsertLogToDatabase(purchaseQualityPlans, purchaseQualityPlans, LoginedUserService.UserId, Tables.PurchaseQualityPlans, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectPurchaseQualityPlansDto>(purchaseQualityPlans);

        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListPurchaseQualityPlansDto>>> GetListAsync(ListPurchaseQualityPlansParameterDto input)
        {
            var query = queryFactory
                   .Query()
                   .From(Tables.PurchaseQualityPlans)
                   .Select<PurchaseQualityPlans>(s => new { s.RevisionNo, s.DocumentNumber, s.Id })
                   .Join<Products>
                    (
                        pr => new { ProductCode = pr.Code, ProductName = pr.Name },
                        nameof(PurchaseQualityPlans.ProductID),
                        nameof(Products.Id),
                        JoinType.Left
                    )
                    .Join<CurrentAccountCards>
                    (
                        cac => new { CurrrentAccountCardCode = cac.Code, CurrrentAccountCardName = cac.Name },
                        nameof(PurchaseQualityPlans.CurrrentAccountCardID),
                        nameof(ProductsOperations.Id),
                        JoinType.Left
                    )
                    .Where(null, Tables.PurchaseQualityPlans);

            var purchaseQualityPlans = queryFactory.GetList<ListPurchaseQualityPlansDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListPurchaseQualityPlansDto>>(purchaseQualityPlans);

        }

        [ValidationAspect(typeof(UpdatePurchaseQualityPlansValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPurchaseQualityPlansDto>> UpdateAsync(UpdatePurchaseQualityPlansDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.PurchaseQualityPlans).Select("*").Where(new { Id = input.Id },  "");
            var entity = queryFactory.Get<PurchaseQualityPlans>(entityQuery);

            #region Update Control

            var listQuery = queryFactory.Query().From(Tables.PurchaseQualityPlans).Select("*").Where(new { DocumentNumber = input.DocumentNumber }, "");
            var list = queryFactory.GetList<PurchaseQualityPlans>(listQuery).ToList();

            if (list.Count > 0 && entity.DocumentNumber != input.DocumentNumber)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
            #endregion

            var query = queryFactory.Query().From(Tables.PurchaseQualityPlans).Update(new UpdatePurchaseQualityPlansDto
            {
                CreationTime = entity.CreationTime.GetValueOrDefault(),
                CreatorId = entity.CreatorId.GetValueOrDefault(),
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                RevisionNo = input.RevisionNo,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                Id = input.Id,
                IsDeleted = entity.IsDeleted,
                LastModificationTime = _GetSQLDateAppService.GetDateFromSQL(),
                Description_ = input.Description_,
                DocumentNumber = input.DocumentNumber,
                ProductID = input.ProductID.GetValueOrDefault(),
                CurrrentAccountCardID = input.CurrrentAccountCardID.GetValueOrDefault(),
                NumberofSampleinPart = input.NumberofSampleinPart,
                AcceptableNumberofDefectiveProduct = input.AcceptableNumberofDefectiveProduct,
                LastModifierId = LoginedUserService.UserId,
            }).Where(new { Id = input.Id },  "");

            foreach (var item in input.SelectPurchaseQualityPlanLines)
            {
                if (item.Id == Guid.Empty)
                {
                    var queryLine = queryFactory.Query().From(Tables.PurchaseQualityPlanLines).Insert(new CreatePurchaseQualityPlanLinesDto
                    {
                        PurchaseQualityPlanID = input.Id,
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
                    var lineGetQuery = queryFactory.Query().From(Tables.PurchaseQualityPlanLines).Select("*").Where(new { Id = item.Id }, "");

                    var line = queryFactory.Get<SelectPurchaseQualityPlanLinesDto>(lineGetQuery);

                    if (line != null)
                    {
                        var queryLine = queryFactory.Query().From(Tables.PurchaseQualityPlanLines).Update(new UpdatePurchaseQualityPlanLinesDto
                        {
                            PurchaseQualityPlanID = input.Id,
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
                        }).Where(new { Id = line.Id },  "");

                        query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                    }
                }
            }


            var PurchaseQualityPlan = queryFactory.Update<SelectPurchaseQualityPlansDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.PurchaseQualityPlans, LogType.Update, entity.Id);

            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["PurchaseQualityPlansChildMenu"], L["ProcessRefresh"])).Data.FirstOrDefault();

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
            return new SuccessDataResult<SelectPurchaseQualityPlansDto>(PurchaseQualityPlan);

        }

        public async Task<IDataResult<SelectPurchaseQualityPlansDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.PurchaseQualityPlans).Select("Id").Where(new { Id = id }, "");

            var entity = queryFactory.Get<PurchaseQualityPlans>(entityQuery);

            var query = queryFactory.Query().From(Tables.PurchaseQualityPlans).Update(new UpdatePurchaseQualityPlansDto
            {
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DataOpenStatus = lockRow,
                DataOpenStatusUserId = userId,
                RevisionNo = entity.RevisionNo,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
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

            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id },  "");

            var PurchaseQualityPlansDto = queryFactory.Update<SelectPurchaseQualityPlansDto>(query, "Id", true);
            await Task.CompletedTask;
            return new SuccessDataResult<SelectPurchaseQualityPlansDto>(PurchaseQualityPlansDto);

        }

        public async Task<IDataResult<SelectPurchaseQualityPlansDto>> GetbyCurrentAccountandProductAsync(Guid CurrentAccountCardID, Guid ProductID)
        {
            var query = queryFactory
                  .Query()
                  .From(Tables.PurchaseQualityPlans)
                  .Select<PurchaseQualityPlans>(null)
                  .Join<Products>
                   (
                       pr => new { ProductCode = pr.Code, ProductName = pr.Name, ProductID = pr.Id },
                       nameof(PurchaseQualityPlans.ProductID),
                       nameof(Products.Id),
                       JoinType.Left
                   )
                   .Join<CurrentAccountCards>
                   (
                       cac => new { CurrrentAccountCardCode = cac.Code, CurrrentAccountCardName = cac.Name, CurrrentAccountCardID = cac.Id },
                       nameof(PurchaseQualityPlans.CurrrentAccountCardID),
                       nameof(CurrentAccountCards.Id),
                       JoinType.Left
                   )
                   .Where(new { CurrrentAccountCardID = CurrentAccountCardID },  Tables.PurchaseQualityPlans)
                   .Where(new { ProductID = ProductID },  Tables.PurchaseQualityPlans);

            var purchaseQualityPlans = queryFactory.Get<SelectPurchaseQualityPlansDto>(query);

            if(purchaseQualityPlans != null && purchaseQualityPlans.Id != Guid.Empty)
            {
                #region Satır Get

                var queryLines = queryFactory
                       .Query()
                       .From(Tables.PurchaseQualityPlanLines)
                       .Select<PurchaseQualityPlanLines>(null)
                       .Join<Products>
                        (
                            p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                            nameof(PurchaseQualityPlanLines.ProductID),
                            nameof(Products.Id),
                            JoinType.Left
                        )
                        .Join<ControlTypes>
                        (
                            ct => new { ControlTypesName = ct.Name, ControlTypesID = ct.Id },
                            nameof(PurchaseQualityPlanLines.ControlTypesID),
                            nameof(ControlTypes.Id),
                            JoinType.Left
                        )
                         .Join<StationGroups>
                        (
                            sg => new { WorkCenterName = sg.Name, WorkCenterID = sg.Id },
                            nameof(PurchaseQualityPlanLines.WorkCenterID),
                            nameof(StationGroups.Id),
                            JoinType.Left
                        )
                         .Join<ControlConditions>
                        (
                            cc => new { ControlConditionsName = cc.Name, ControlConditionsID = cc.Id },
                            nameof(PurchaseQualityPlanLines.ControlConditionsID),
                            nameof(ControlConditions.Id),
                            JoinType.Left
                        )
                        .Where(new { PurchaseQualityPlanID = purchaseQualityPlans.Id }, Tables.PurchaseQualityPlanLines);

                var purchaseQualityPlanLine = queryFactory.GetList<SelectPurchaseQualityPlanLinesDto>(queryLines).ToList();

                purchaseQualityPlans.SelectPurchaseQualityPlanLines = purchaseQualityPlanLine;

                #endregion
            }



            await Task.CompletedTask;
            return new SuccessDataResult<SelectPurchaseQualityPlansDto>(purchaseQualityPlans);

        }
    }
}

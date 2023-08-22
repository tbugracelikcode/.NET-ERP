using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.QualityControl.PurchaseQualityPlan.Services;
using TsiErp.Business.Entities.QualityControl.PurchaseQualityPlan.Validations;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationGroup;
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

        public PurchaseQualityPlansAppService(IStringLocalizer<PurchaseQualityPlansResource> l) : base(l)
        {
        }

        [ValidationAspect(typeof(CreatePurchaseQualityPlansValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPurchaseQualityPlansDto>> CreateAsync(CreatePurchaseQualityPlansDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var listQuery = queryFactory.Query().From(Tables.PurchaseQualityPlans).Select("*").Where(new { DocumentNumber = input.DocumentNumber }, false, false, "");
                var list = queryFactory.ControlList<PurchaseQualityPlans>(listQuery).ToList();

                #region Code Control 

                if (list.Count > 0)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["CodeControlManager"]);
                }

                #endregion

                Guid addedEntityId = GuidGenerator.CreateGuid();

                var query = queryFactory.Query().From(Tables.PurchaseQualityPlans).Insert(new CreatePurchaseQualityPlansDto
                {
                    CreationTime = DateTime.Now,
                    CreatorId = LoginedUserService.UserId,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = Guid.Empty,
                    DeletionTime = null,
                    Id = addedEntityId,
                    Description_ = input.Description_,
                    DocumentNumber = input.DocumentNumber,
                    ProductID = input.ProductID,
                    CurrrentAccountCardID = input.CurrrentAccountCardID,
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
                        CreationTime = DateTime.Now,
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

                LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.PurchaseQualityPlans, LogType.Insert, addedEntityId);

                return new SuccessDataResult<SelectPurchaseQualityPlansDto>(PurchaseQualityPlan);
            }
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {

                var deleteQuery = queryFactory.Query().From(Tables.PurchaseQualityPlans).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");

                var lineDeleteQuery = queryFactory.Query().From(Tables.PurchaseQualityPlanLines).Delete(LoginedUserService.UserId).Where(new { PurchaseQualityPlanID = id }, false, false, "");

                var PurchaseQualityPlan = queryFactory.Update<SelectPurchaseQualityPlansDto>(deleteQuery, "Id", true);
                var PurchaseQualityPlanLines = queryFactory.Update<SelectPurchaseQualityPlanLinesDto>(lineDeleteQuery, "Id", true);
                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.PurchaseQualityPlans, LogType.Delete, id);
                return new SuccessDataResult<SelectPurchaseQualityPlansDto>(PurchaseQualityPlan);

            }
        }

        public async Task<IResult> DeleteLineAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {


                var queryLine = queryFactory.Query().From(Tables.PurchaseQualityPlanLines).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");
                var PurchaseQualityPlanLines = queryFactory.Update<SelectPurchaseQualityPlanLinesDto>(queryLine, "Id", true);
                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.PurchaseQualityPlanLines, LogType.Delete, id);
                return new SuccessDataResult<SelectPurchaseQualityPlanLinesDto>(PurchaseQualityPlanLines);

            }
        }

        public async Task<IDataResult<SelectPurchaseQualityPlansDto>> GetAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory
                       .Query()
                       .From(Tables.PurchaseQualityPlans)
                       .Select<PurchaseQualityPlans>(cqp => new { cqp.CurrrentAccountCardID, cqp.ProductID, cqp.Id, cqp.DocumentNumber, cqp.Description_, cqp.DataOpenStatusUserId, cqp.DataOpenStatus, cqp.AcceptableNumberofDefectiveProduct, cqp.NumberofSampleinPart })
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
                        .Where(new { Id = id }, false, false, Tables.PurchaseQualityPlans);

                var purchaseQualityPlans = queryFactory.Get<SelectPurchaseQualityPlansDto>(query);

                #region Satır Get

                var queryLines = queryFactory
                       .Query()
                       .From(Tables.PurchaseQualityPlanLines)
                       .Select<PurchaseQualityPlanLines>(cqpl => new { cqpl.WorkCenterID, cqpl.UpperTolerance, cqpl.PeriodicControlMeasure, cqpl.PurchaseQualityPlanID, cqpl.MeasureNumberInPicture, cqpl.LineNr, cqpl.IdealMeasure, cqpl.Id, cqpl.Equipment, cqpl.Description_, cqpl.Date_, cqpl.DataOpenStatusUserId, cqpl.DataOpenStatus, cqpl.ControlTypesID, cqpl.ControlManager, cqpl.ControlFrequency, cqpl.ControlConditionsID, cqpl.Code, cqpl.BottomTolerance })
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
                        .Where(new { PurchaseQualityPlanID = id }, false, false, Tables.PurchaseQualityPlanLines);

                var purchaseQualityPlanLine = queryFactory.GetList<SelectPurchaseQualityPlanLinesDto>(queryLines).ToList();

                purchaseQualityPlans.SelectPurchaseQualityPlanLines = purchaseQualityPlanLine;

                #endregion

               

                LogsAppService.InsertLogToDatabase(purchaseQualityPlans, purchaseQualityPlans, LoginedUserService.UserId, Tables.PurchaseQualityPlans, LogType.Get, id);

                return new SuccessDataResult<SelectPurchaseQualityPlansDto>(purchaseQualityPlans);
            }
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListPurchaseQualityPlansDto>>> GetListAsync(ListPurchaseQualityPlansParameterDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory
                       .Query()
                       .From(Tables.PurchaseQualityPlans)
                       .Select<PurchaseQualityPlans>(cqp => new { cqp.CurrrentAccountCardID, cqp.ProductID, cqp.Id, cqp.DocumentNumber, cqp.Description_, cqp.DataOpenStatusUserId, cqp.DataOpenStatus, cqp.AcceptableNumberofDefectiveProduct, cqp.NumberofSampleinPart })
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
                        .Where(null, false, false, Tables.PurchaseQualityPlans);

                var purchaseQualityPlans = queryFactory.GetList<ListPurchaseQualityPlansDto>(query).ToList();
                return new SuccessDataResult<IList<ListPurchaseQualityPlansDto>>(purchaseQualityPlans);
            }
        }

        [ValidationAspect(typeof(UpdatePurchaseQualityPlansValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPurchaseQualityPlansDto>> UpdateAsync(UpdatePurchaseQualityPlansDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.PurchaseQualityPlans).Select("*").Where(new { Id = input.Id }, false, false, "");
                var entity = queryFactory.Get<PurchaseQualityPlans>(entityQuery);

                #region Update Control

                var listQuery = queryFactory.Query().From(Tables.PurchaseQualityPlans).Select("*").Where(new { DocumentNumber = input.DocumentNumber }, false, false, "");
                var list = queryFactory.GetList<PurchaseQualityPlans>(listQuery).ToList();

                if (list.Count > 0 && entity.DocumentNumber != input.DocumentNumber)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["UpdateControlManager"]);
                }
                #endregion

                var query = queryFactory.Query().From(Tables.PurchaseQualityPlans).Update(new UpdatePurchaseQualityPlansDto
                {
                    CreationTime = entity.CreationTime.GetValueOrDefault(),
                    CreatorId = entity.CreatorId.GetValueOrDefault(),
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = entity.DeleterId.GetValueOrDefault(),
                    DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                    Id = input.Id,
                    IsDeleted = entity.IsDeleted,
                    LastModificationTime = DateTime.Now,
                    Description_ = input.Description_,
                    DocumentNumber = input.DocumentNumber,
                    ProductID = input.ProductID,
                    CurrrentAccountCardID = input.CurrrentAccountCardID,
                    NumberofSampleinPart = input.NumberofSampleinPart,
                    AcceptableNumberofDefectiveProduct = input.AcceptableNumberofDefectiveProduct,
                    LastModifierId = LoginedUserService.UserId,
                }).Where(new { Id = input.Id }, false, false, "");

                foreach (var item in input.SelectPurchaseQualityPlanLines)
                {
                    if (item.Id == Guid.Empty)
                    {
                        var queryLine = queryFactory.Query().From(Tables.PurchaseQualityPlanLines).Insert(new CreatePurchaseQualityPlanLinesDto
                        {
                            PurchaseQualityPlanID = input.Id,
                            CreationTime = DateTime.Now,
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
                        var lineGetQuery = queryFactory.Query().From(Tables.PurchaseQualityPlanLines).Select("*").Where(new { Id = item.Id }, false, false, "");

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
                                LastModificationTime = DateTime.Now,
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
                            }).Where(new { Id = line.Id }, false, false, "");

                            query.Sql = query.Sql + QueryConstants.QueryConstant + queryLine.Sql + " where " + queryLine.WhereSentence;
                        }
                    }
                }


                var PurchaseQualityPlan = queryFactory.Update<SelectPurchaseQualityPlansDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(entity, input, LoginedUserService.UserId, Tables.PurchaseQualityPlans, LogType.Update, entity.Id);

                return new SuccessDataResult<SelectPurchaseQualityPlansDto>(PurchaseQualityPlan);
            }
        }

        public async Task<IDataResult<SelectPurchaseQualityPlansDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.PurchaseQualityPlans).Select("*").Where(new { Id = id }, false, false, "");

                var entity = queryFactory.Get<PurchaseQualityPlans>(entityQuery);

                var query = queryFactory.Query().From(Tables.PurchaseQualityPlans).Update(new UpdatePurchaseQualityPlansDto
                {
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
                    Description_ = entity.Description_,
                    DocumentNumber = entity.DocumentNumber,
                    ProductID = entity.ProductID,
                    AcceptableNumberofDefectiveProduct = entity.AcceptableNumberofDefectiveProduct,
                    CurrrentAccountCardID = entity.CurrrentAccountCardID,
                    NumberofSampleinPart = entity.NumberofSampleinPart,

                }).Where(new { Id = id }, false, false, "");

                var PurchaseQualityPlansDto = queryFactory.Update<SelectPurchaseQualityPlansDto>(query, "Id", true);
                return new SuccessDataResult<SelectPurchaseQualityPlansDto>(PurchaseQualityPlansDto);

            }
        }
    }
}



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
using TsiErp.Business.Entities.QualityControl.OperationalQualityPlan.Validations;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationGroup;
using TsiErp.Entities.Entities.ProductionManagement.ProductsOperation;
using TsiErp.Entities.Entities.QualityControl.ControlCondition;
using TsiErp.Entities.Entities.QualityControl.ControlType;
using TsiErp.Entities.Entities.QualityControl.OperationalQualityPlan;
using TsiErp.Entities.Entities.QualityControl.OperationalQualityPlan.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.OperationalQualityPlan.Page;

namespace TsiErp.Business.Entities.OperationalQualityPlan.Services
{
    [ServiceRegistration(typeof(IOperationalQualityPlansAppService), DependencyInjectionType.Scoped)]
    public class OperationalQualityPlansAppService : ApplicationService<OperationalQualityPlansResource>, IOperationalQualityPlansAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        public OperationalQualityPlansAppService(IStringLocalizer<OperationalQualityPlansResource> l) : base(l)
        {
        }

        [ValidationAspect(typeof(CreateOperationalQualityPlansValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectOperationalQualityPlansDto>> CreateAsync(CreateOperationalQualityPlansDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var listQuery = queryFactory.Query().From(Tables.OperationalQualityPlans).Select("*").Where(new { Code = input.Code }, false, false, "");
                var list = queryFactory.ControlList<OperationalQualityPlans>(listQuery).ToList();

                #region Code Control 

                if (list.Count > 0)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["CodeControlManager"]);
                }

                #endregion

                Guid addedEntityId = GuidGenerator.CreateGuid();

                var query = queryFactory.Query().From(Tables.OperationalQualityPlans).Insert(new CreateOperationalQualityPlansDto
                {
                    Code = input.Code,
                    CreationTime = DateTime.Now,
                    CreatorId = LoginedUserService.UserId,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = Guid.Empty,
                    DeletionTime = null,
                    Id = addedEntityId,
                    IsDeleted = false,
                    LastModificationTime = null,
                    LastModifierId = Guid.Empty,
                    WorkCenterID = input.WorkCenterID,
                    ProductsOperationID = input.ProductsOperationID,
                    ProductID = input.ProductID,
                    BottomTolerance = input.BottomTolerance,
                    ControlConditionsID = input.ControlConditionsID,
                    ControlFrequency = input.ControlFrequency,
                    ControlManager = input.ControlManager,
                    ControlTypesID = input.ControlTypesID,
                    Date_ = input.Date_,
                    Description_ = input.Description_,
                    Equipment = input.Equipment,
                    IdealMeasure = input.IdealMeasure,
                    MeasureNumberInPicture = input.MeasureNumberInPicture,
                    PeriodicControlMeasure = input.PeriodicControlMeasure,
                    UpperTolerance = input.UpperTolerance,
                });

                var operationalQualityPlans = queryFactory.Insert<SelectOperationalQualityPlansDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.OperationalQualityPlans, LogType.Insert, operationalQualityPlans.Id);


                return new SuccessDataResult<SelectOperationalQualityPlansDto>(operationalQualityPlans);
            }
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory.Query().From(Tables.OperationalQualityPlans).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");

                var operationalQualityPlans = queryFactory.Update<SelectOperationalQualityPlansDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.OperationalQualityPlans, LogType.Delete, id);

                return new SuccessDataResult<SelectOperationalQualityPlansDto>(operationalQualityPlans);
            }
        }

        public async Task<IDataResult<SelectOperationalQualityPlansDto>> GetAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory
                       .Query()
                       .From(Tables.OperationalQualityPlans)
                       .Select<OperationalQualityPlans>(oqp => new { oqp.ProductID, oqp.Id, oqp.WorkCenterID, oqp.UpperTolerance, oqp.ProductsOperationID, oqp.PeriodicControlMeasure, oqp.MeasureNumberInPicture, oqp.IdealMeasure, oqp.Description_, oqp.Date_, oqp.DataOpenStatusUserId, oqp.DataOpenStatus, oqp.ControlTypesID, oqp.ControlManager, oqp.ControlFrequency, oqp.ControlConditionsID, oqp.Code, oqp.BottomTolerance })
                       .Join<Products>
                        (
                            pr => new { ProductCode = pr.Code, ProductName = pr.Name, ProductID = pr.Id },
                            nameof(OperationalQualityPlans.ProductID),
                            nameof(Products.Id),
                            JoinType.Left
                        )
                        .Join<ProductsOperations>
                        (
                            po => new { OperationCode = po.Code, OperationName = po.Name, ProductsOperationID = po.Id },
                            nameof(OperationalQualityPlans.ProductsOperationID),
                            nameof(ProductsOperations.Id),
                            JoinType.Left
                        )
                        .Join<ControlTypes>
                        (
                            ct => new { ControlTypesName = ct.Name, ControlTypesID = ct.Id },
                            nameof(OperationalQualityPlans.ControlTypesID),
                            nameof(ControlTypes.Id),
                            JoinType.Left
                        )
                         .Join<StationGroups>
                        (
                            sg => new { WorkCenterName = sg.Name, WorkCenterID = sg.Id },
                            nameof(OperationalQualityPlans.WorkCenterID),
                            nameof(StationGroups.Id),
                            JoinType.Left
                        )
                         .Join<ControlConditions>
                        (
                            cc => new { ControlConditionsName = cc.Name, ControlConditionsID = cc.Id },
                            nameof(OperationalQualityPlans.ControlConditionsID),
                            nameof(ControlConditions.Id),
                            JoinType.Left
                        )
                        .Where(new { Id = id }, false, false, Tables.OperationalQualityPlans);

                var operationalQualityPlans = queryFactory.Get<SelectOperationalQualityPlansDto>(query);



                LogsAppService.InsertLogToDatabase(operationalQualityPlans, operationalQualityPlans, LoginedUserService.UserId, Tables.OperationalQualityPlans, LogType.Get, id);

                return new SuccessDataResult<SelectOperationalQualityPlansDto>(operationalQualityPlans);
            }
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListOperationalQualityPlansDto>>> GetListAsync(ListOperationalQualityPlansParameterDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory
                       .Query()
                       .From(Tables.OperationalQualityPlans)
                       .Select<OperationalQualityPlans>(oqp => new { oqp.ProductID, oqp.Id, oqp.WorkCenterID, oqp.UpperTolerance, oqp.ProductsOperationID, oqp.PeriodicControlMeasure, oqp.MeasureNumberInPicture, oqp.IdealMeasure, oqp.Description_, oqp.Date_, oqp.DataOpenStatusUserId, oqp.DataOpenStatus, oqp.ControlTypesID, oqp.ControlManager, oqp.ControlFrequency, oqp.ControlConditionsID, oqp.Code, oqp.BottomTolerance })
                       .Join<Products>
                        (
                            pr => new { ProductCode = pr.Code, ProductName = pr.Name },
                            nameof(OperationalQualityPlans.ProductID),
                            nameof(Products.Id),
                            JoinType.Left
                        )
                        .Join<ProductsOperations>
                        (
                            po => new { OperationCode = po.Code, OperationName = po.Name },
                            nameof(OperationalQualityPlans.ProductsOperationID),
                            nameof(ProductsOperations.Id),
                            JoinType.Left
                        )
                        .Join<ControlTypes>
                        (
                            ct => new { ControlTypesName = ct.Name },
                            nameof(OperationalQualityPlans.ControlTypesID),
                            nameof(ControlTypes.Id),
                            JoinType.Left
                        )
                         .Join<StationGroups>
                        (
                            sg => new { WorkCenterName = sg.Name },
                            nameof(OperationalQualityPlans.WorkCenterID),
                            nameof(StationGroups.Id),
                            JoinType.Left
                        )
                        .Where(null, false, false, Tables.OperationalQualityPlans);

                var operationalQualityPlans = queryFactory.GetList<ListOperationalQualityPlansDto>(query).ToList();
                return new SuccessDataResult<IList<ListOperationalQualityPlansDto>>(operationalQualityPlans);
            }
        }

        [ValidationAspect(typeof(UpdateOperationalQualityPlansValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectOperationalQualityPlansDto>> UpdateAsync(UpdateOperationalQualityPlansDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.OperationalQualityPlans).Select("*").Where(new { Id = input.Id }, false, false, "");
                var entity = queryFactory.Get<OperationalQualityPlans>(entityQuery);

                #region Update Control

                var listQuery = queryFactory.Query().From(Tables.OperationalQualityPlans).Select("*").Where(new { Code = input.Code }, false, false, "");
                var list = queryFactory.GetList<OperationalQualityPlans>(listQuery).ToList();

                if (list.Count > 0 && entity.Code != input.Code)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["UpdateControlManager"]);
                }

                #endregion

                var query = queryFactory.Query().From(Tables.OperationalQualityPlans).Update(new UpdateOperationalQualityPlansDto
                {
                    Code = input.Code,
                    Description_ = input.Description_,
                    Id = input.Id,
                    BottomTolerance = input.BottomTolerance,
                    ControlConditionsID = input.ControlConditionsID,
                    ControlFrequency = input.ControlFrequency,
                    ControlManager = input.ControlManager,
                    ControlTypesID = input.ControlTypesID,
                    Date_ = input.Date_,
                    Equipment = input.Equipment,
                    IdealMeasure = input.IdealMeasure,
                    MeasureNumberInPicture = input.MeasureNumberInPicture,
                    PeriodicControlMeasure = input.PeriodicControlMeasure,
                    ProductID = input.ProductID,
                    ProductsOperationID = input.ProductsOperationID,
                    UpperTolerance = input.UpperTolerance,
                    WorkCenterID = input.WorkCenterID,
                    CreationTime = entity.CreationTime.Value,
                    CreatorId = entity.CreatorId.Value,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = entity.DeleterId.GetValueOrDefault(),
                    DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                    IsDeleted = entity.IsDeleted,
                    LastModificationTime = DateTime.Now,
                    LastModifierId = LoginedUserService.UserId
                }).Where(new { Id = input.Id }, false, false, "");

                var operationalQualityPlans = queryFactory.Update<SelectOperationalQualityPlansDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(entity, operationalQualityPlans, LoginedUserService.UserId, Tables.OperationalQualityPlans, LogType.Update, entity.Id);

                return new SuccessDataResult<SelectOperationalQualityPlansDto>(operationalQualityPlans);
            }
        }

        public async Task<IDataResult<SelectOperationalQualityPlansDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.BillsofMaterials).Select("*").Where(new { Id = id }, true, true, "");

                var entity = queryFactory.Get<OperationalQualityPlans>(entityQuery);

                var query = queryFactory.Query().From(Tables.OperationalQualityPlans).Update(new UpdateOperationalQualityPlansDto
                {
                    Code = entity.Code,
                    CreationTime = entity.CreationTime.Value,
                    CreatorId = entity.CreatorId.Value,
                    DataOpenStatus = lockRow,
                    DataOpenStatusUserId = userId,
                    DeleterId = entity.DeleterId.GetValueOrDefault(),
                    DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                    BottomTolerance = entity.BottomTolerance,
                    ControlConditionsID = entity.ControlConditionsID,
                    ControlFrequency = entity.ControlFrequency,
                    ControlManager = entity.ControlManager,
                    ControlTypesID = entity.ControlTypesID,
                    Date_ = entity.Date_,
                    Equipment = entity.Equipment,
                    IdealMeasure = entity.IdealMeasure,
                    MeasureNumberInPicture = entity.MeasureNumberInPicture,
                    PeriodicControlMeasure = entity.PeriodicControlMeasure,
                    ProductID = entity.ProductID,
                    ProductsOperationID = entity.ProductsOperationID,
                    UpperTolerance = entity.UpperTolerance,
                    WorkCenterID = entity.WorkCenterID,
                    Id = entity.Id,
                    IsDeleted = entity.IsDeleted,
                    LastModificationTime = entity.LastModificationTime.GetValueOrDefault(),
                    LastModifierId = entity.LastModifierId.GetValueOrDefault(),
                }).Where(new { Id = id }, false, false, "");

                var operationalQualityPlansDto = queryFactory.Update<SelectOperationalQualityPlansDto>(query, "Id", true);
                return new SuccessDataResult<SelectOperationalQualityPlansDto>(operationalQualityPlansDto);

            }
        }
    }
}

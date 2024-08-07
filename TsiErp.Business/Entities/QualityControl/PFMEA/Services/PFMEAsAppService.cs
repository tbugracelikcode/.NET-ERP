using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TSI.QueryBuilder.Models;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.QualityControl.PFMEA.Validations;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationGroup;
using TsiErp.Entities.Entities.ProductionManagement.ProductsOperation;
using TsiErp.Entities.Entities.QualityControl.OperationalSPC;
using TsiErp.Entities.Entities.QualityControl.PFMEA;
using TsiErp.Entities.Entities.QualityControl.PFMEA.Dtos;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItem;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.PFMEAs.Page;

namespace TsiErp.Business.Entities.PFMEA.Services
{
    [ServiceRegistration(typeof(IPFMEAsAppService), DependencyInjectionType.Scoped)]
    public class PFMEAsAppService : ApplicationService<PFMEAsResource>, IPFMEAsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        public PFMEAsAppService(IStringLocalizer<PFMEAsResource> l, IGetSQLDateAppService getSQLDateAppService) : base(l)
        {
            _GetSQLDateAppService = getSQLDateAppService;
        }


        [ValidationAspect(typeof(CreatePFMEAsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPFMEAsDto>> CreateAsync(CreatePFMEAsDto input)
        {
            Guid addedEntityId = GuidGenerator.CreateGuid();

            var query = queryFactory.Query().From(Tables.PFMEAs).Insert(new CreatePFMEAsDto
            {
                ActionCompletionDate = input.ActionCompletionDate,
                UnsuitabilityItemID = input.UnsuitabilityItemID.GetValueOrDefault(),
                WorkCenterID = input.WorkCenterID.GetValueOrDefault(),
                OperationID = input.OperationID.GetValueOrDefault(),
                FirstOperationalSPCID = input.FirstOperationalSPCID.GetValueOrDefault(),
                ControlMechanism = input.ControlMechanism,
                ControlMethod = input.ControlMethod,
                CurrentDetectability = input.CurrentDetectability,
                CurrentFrequency = input.CurrentFrequency,
                CurrentRPN = input.CurrentRPN,
                CurrentSeverity = input.CurrentSeverity,
                Description_ = input.Description_,
                ImpactofError = input.ImpactofError,
                InhibitorAction = input.InhibitorAction,
                LineNr = input.LineNr,
                NewDetectability = input.NewDetectability,
                NewFrequency = input.NewFrequency,
                NewRPN = input.NewRPN,
                NewSeverity = input.NewSeverity,
                OperationRequirement = input.OperationRequirement,
                PotentialErrorReason = input.PotentialErrorReason,
                SafetyClass = input.SafetyClass,
                SecondOperationalSPCID = input.SecondOperationalSPCID.GetValueOrDefault(),
                State = input.State,
                TargetEndDate = input.TargetEndDate,
                CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
                CreatorId = LoginedUserService.UserId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = Guid.Empty,
                Date_ = input.Date_,
                DeletionTime = null,
                Id = addedEntityId,
                IsDeleted = false,
                LastModificationTime = null,
                LastModifierId = Guid.Empty,
            });

            var PFMEAs = queryFactory.Insert<SelectPFMEAsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.PFMEAs, LogType.Insert, addedEntityId);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectPFMEAsDto>(PFMEAs);

        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.PFMEAs).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

            var PFMEAs = queryFactory.Update<SelectPFMEAsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.PFMEAs, LogType.Delete, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectPFMEAsDto>(PFMEAs);

        }


        public async Task<IDataResult<SelectPFMEAsDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                    .Query().From(Tables.PFMEAs).Select<PFMEAs>(null)
                        .Join<OperationalSPCs>
                        (
                            e => new { FirstOperationalSPCCode = e.Code, FirstOperationalSPCID = e.Id },
                            nameof(PFMEAs.FirstOperationalSPCID),
                            nameof(OperationalSPCs.Id),
                            JoinType.Left
                        )
                         .Join<OperationalSPCs>
                        (
                            e => new { SecondOperationalSPCCode = e.Code, SecondOperationalSPCID = e.Id },
                            nameof(PFMEAs.SecondOperationalSPCID),
                            nameof(OperationalSPCs.Id),
                            "SecondOperationalSPC",
                            JoinType.Left
                        )
                         .Join<StationGroups>
                        (
                            e => new { WorkCenterName = e.Name, WorkCenterID = e.Id },
                            nameof(PFMEAs.WorkCenterID),
                            nameof(StationGroups.Id),
                            JoinType.Left
                        )
                           .Join<ProductsOperations>
                        (
                            e => new { OperationName = e.Name, OperationID = e.Id },
                            nameof(PFMEAs.OperationID),
                            nameof(ProductsOperations.Id),
                            JoinType.Left
                        )
                          .Join<UnsuitabilityItems>
                        (
                            e => new { UnsuitabilityItemName = e.Name, UnsuitabilityItemID = e.Id },
                            nameof(PFMEAs.UnsuitabilityItemID),
                            nameof(UnsuitabilityItems.Id),
                            JoinType.Left
                        )
                        .Where(new { Id = id }, Tables.PFMEAs);

            var PFMEA = queryFactory.Get<SelectPFMEAsDto>(query);

            LogsAppService.InsertLogToDatabase(PFMEA, PFMEA, LoginedUserService.UserId, Tables.PFMEAs, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectPFMEAsDto>(PFMEA);

        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListPFMEAsDto>>> GetListAsync(ListPFMEAsParameterDto input)
        {
            var query = queryFactory
                    .Query()
                    .From(Tables.PFMEAs)
                    .Select<PFMEAs>(null)
                        .Join<OperationalSPCs>
                        (
                            e => new { FirstOperationalSPCCode = e.Code, FirstOperationalSPCID = e.Id },
                            nameof(PFMEAs.FirstOperationalSPCID),
                            nameof(OperationalSPCs.Id),
                            JoinType.Left
                        )
                         .Join<OperationalSPCs>
                        (
                            e => new { SecondOperationalSPCCode = e.Code, SecondOperationalSPCID = e.Id },
                            nameof(PFMEAs.SecondOperationalSPCID),
                            nameof(OperationalSPCs.Id),
                            "SecondOperationalSPC",
                            JoinType.Left
                        )
                         .Join<StationGroups>
                        (
                            e => new { WorkCenterName = e.Name, WorkCenterID = e.Id },
                            nameof(PFMEAs.WorkCenterID),
                            nameof(StationGroups.Id),
                            JoinType.Left
                        )
                           .Join<ProductsOperations>
                        (
                            e => new { OperationName = e.Name, OperationID = e.Id },
                            nameof(PFMEAs.OperationID),
                            nameof(ProductsOperations.Id),
                            JoinType.Left
                        )
                          .Join<UnsuitabilityItems>
                        (
                            e => new { UnsuitabilityItemName = e.Name, UnsuitabilityItemID = e.Id },
                            nameof(PFMEAs.UnsuitabilityItemID),
                            nameof(UnsuitabilityItems.Id),
                            JoinType.Left
                        ).Where(null,  Tables.PFMEAs);


            var pFMEAs = queryFactory.GetList<ListPFMEAsDto>(query).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListPFMEAsDto>>(pFMEAs);

        }


        [ValidationAspect(typeof(UpdatePFMEAsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPFMEAsDto>> UpdateAsync(UpdatePFMEAsDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.PFMEAs).Select("*").Where(new { Id = input.Id },  "");
            var entity = queryFactory.Get<PFMEAs>(entityQuery);

            var query = queryFactory.Query().From(Tables.PFMEAs).Update(new UpdatePFMEAsDto
            {
                ActionCompletionDate = input.ActionCompletionDate,
                UnsuitabilityItemID = input.UnsuitabilityItemID.GetValueOrDefault(),
                WorkCenterID = input.WorkCenterID.GetValueOrDefault(),
                OperationID = input.OperationID.GetValueOrDefault(),
                FirstOperationalSPCID = input.FirstOperationalSPCID.GetValueOrDefault(),
                ControlMechanism = input.ControlMechanism,
                ControlMethod = input.ControlMethod,
                CurrentDetectability = input.CurrentDetectability,
                CurrentFrequency = input.CurrentFrequency,
                CurrentRPN = input.CurrentRPN,
                CurrentSeverity = input.CurrentSeverity,
                Description_ = input.Description_,
                ImpactofError = input.ImpactofError,
                InhibitorAction = input.InhibitorAction,
                LineNr = input.LineNr,
                NewDetectability = input.NewDetectability,
                NewFrequency = input.NewFrequency,
                NewRPN = input.NewRPN,
                NewSeverity = input.NewSeverity,
                OperationRequirement = input.OperationRequirement,
                PotentialErrorReason = input.PotentialErrorReason,
                SafetyClass = input.SafetyClass,
                SecondOperationalSPCID = input.SecondOperationalSPCID.GetValueOrDefault(),
                State = input.State,
                TargetEndDate = input.TargetEndDate,
                Id = input.Id,
                Date_ = input.Date_,
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

            var PFMEAs = queryFactory.Update<SelectPFMEAsDto>(query, "Id", true);


            LogsAppService.InsertLogToDatabase(entity, PFMEAs, LoginedUserService.UserId, Tables.PFMEAs, LogType.Update, entity.Id);


            await Task.CompletedTask;
            return new SuccessDataResult<SelectPFMEAsDto>(PFMEAs);

        }

        public async Task<IDataResult<SelectPFMEAsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.PFMEAs).Select("*").Where(new { Id = id }, "");
            var entity = queryFactory.Get<PFMEAs>(entityQuery);

            var query = queryFactory.Query().From(Tables.PFMEAs).Update(new UpdatePFMEAsDto
            {
                ActionCompletionDate = entity.ActionCompletionDate,
                UnsuitabilityItemID = entity.UnsuitabilityItemID,
                WorkCenterID = entity.WorkCenterID,
                OperationID = entity.OperationID,
                FirstOperationalSPCID = entity.FirstOperationalSPCID,
                ControlMechanism = entity.ControlMechanism,
                ControlMethod = entity.ControlMethod,
                CurrentDetectability = entity.CurrentDetectability,
                CurrentFrequency = entity.CurrentFrequency,
                CurrentRPN = entity.CurrentRPN,
                CurrentSeverity = entity.CurrentSeverity,
                Description_ = entity.Description_,
                ImpactofError = entity.ImpactofError,
                InhibitorAction = entity.InhibitorAction,
                LineNr = entity.LineNr,
                NewDetectability = entity.NewDetectability,
                NewFrequency = entity.NewFrequency,
                NewRPN = entity.NewRPN,
                NewSeverity = entity.NewSeverity,
                OperationRequirement = entity.OperationRequirement,
                PotentialErrorReason = entity.PotentialErrorReason,
                SafetyClass = entity.SafetyClass,
                SecondOperationalSPCID = entity.SecondOperationalSPCID,
                State = entity.State,
                TargetEndDate = entity.TargetEndDate,
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

            var PFMEAs = queryFactory.Update<SelectPFMEAsDto>(query, "Id", true);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectPFMEAsDto>(PFMEAs);


        }
    }
}

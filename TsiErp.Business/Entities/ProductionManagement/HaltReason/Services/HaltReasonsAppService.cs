using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Models;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.HaltReason.Validations;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Extensions.DeleteControlExtension;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.ProductionManagement.HaltReason;
using TsiErp.Entities.Entities.ProductionManagement.HaltReason.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.HaltReasons.Page;

namespace TsiErp.Business.Entities.HaltReason.Services
{
    [ServiceRegistration(typeof(IHaltReasonsAppService), DependencyInjectionType.Scoped)]
    public class HaltReasonsAppService : ApplicationService<HaltReasonsResource>, IHaltReasonsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        public HaltReasonsAppService(IStringLocalizer<HaltReasonsResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
        }

        [ValidationAspect(typeof(CreateHaltReasonsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectHaltReasonsDto>> CreateAsync(CreateHaltReasonsDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.HaltReasons).Select("*").Where(new { Code = input.Code }, false, false, "");

            var list = queryFactory.ControlList<HaltReasons>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            var query = queryFactory.Query().From(Tables.HaltReasons).Insert(new CreateHaltReasonsDto
            {
                Code = input.Code,
                Name = input.Name,
                IsMachine = input.IsMachine,
                IsIncidentalHalt = input.IsIncidentalHalt,
                IsManagement = input.IsManagement,
                IsOperator = input.IsOperator,
                IsPlanned = input.IsPlanned,
                Id = addedEntityId,
                CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
                CreatorId = LoginedUserService.UserId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = Guid.Empty,
                DeletionTime = null,
                LastModificationTime = null,
                LastModifierId = Guid.Empty,
                IsDeleted = false
            });


            var haltReasons = queryFactory.Insert<SelectHaltReasonsDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("HaltReasonsChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.HaltReasons, LogType.Insert, addedEntityId);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectHaltReasonsDto>(haltReasons);


        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            DeleteControl.ControlList.Clear();

            DeleteControl.ControlList.Add("HaltID", new List<string>
            {
                Tables.ProductionTrackingHaltLines
            });

            bool control = DeleteControl.Control(queryFactory, id);

            if (!control)
            {
                throw new Exception(L["DeleteControlManager"]);
            }
            else
            {
                var query = queryFactory.Query().From(Tables.HaltReasons).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");

                var haltReasons = queryFactory.Update<SelectHaltReasonsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.HaltReasons, LogType.Delete, id);

                await Task.CompletedTask;
                return new SuccessDataResult<SelectHaltReasonsDto>(haltReasons);
            }
        }

        public async Task<IDataResult<SelectHaltReasonsDto>> GetAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.HaltReasons).Select("*").Where(
            new
            {
                Id = id
            }, false, false, "");
            var haltReason = queryFactory.Get<SelectHaltReasonsDto>(query);


            LogsAppService.InsertLogToDatabase(haltReason, haltReason, LoginedUserService.UserId, Tables.HaltReasons, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectHaltReasonsDto>(haltReason);

        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListHaltReasonsDto>>> GetListAsync(ListHaltReasonsParameterDto input)
        {
            var query = queryFactory.Query().From(Tables.HaltReasons).Select("*").Where(null, false, false, "");
            var haltReasons = queryFactory.GetList<ListHaltReasonsDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListHaltReasonsDto>>(haltReasons);
        }

        [ValidationAspect(typeof(UpdateHaltReasonsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectHaltReasonsDto>> UpdateAsync(UpdateHaltReasonsDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.HaltReasons).Select("*").Where(new { Id = input.Id }, false, false, "");
            var entity = queryFactory.Get<HaltReasons>(entityQuery);

            #region Update Control

            var listQuery = queryFactory.Query().From(Tables.HaltReasons).Select("*").Where(new { Code = input.Code }, false, false, "");
            var list = queryFactory.GetList<HaltReasons>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }

            #endregion

            var query = queryFactory.Query().From(Tables.HaltReasons).Update(new UpdateHaltReasonsDto
            {
                Code = input.Code,
                Name = input.Name,
                IsPlanned = input.IsPlanned,
                IsOperator = input.IsOperator,
                IsManagement = input.IsManagement,
                IsIncidentalHalt = input.IsIncidentalHalt,
                IsMachine = input.IsMachine,
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
            }).Where(new { Id = input.Id }, false, false, "");

            var haltReasons = queryFactory.Update<SelectHaltReasonsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, haltReasons, LoginedUserService.UserId, Tables.HaltReasons, LogType.Update, entity.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectHaltReasonsDto>(haltReasons);


        }

        public async Task<IDataResult<SelectHaltReasonsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.HaltReasons).Select("*").Where(new { Id = id }, false, false, "");

            var entity = queryFactory.Get<HaltReasons>(entityQuery);

            var query = queryFactory.Query().From(Tables.HaltReasons).Update(new UpdateHaltReasonsDto
            {
                Code = entity.Code,
                Name = entity.Name,
                IsMachine = entity.IsMachine,
                IsManagement = entity.IsManagement,
                IsIncidentalHalt = entity.IsIncidentalHalt,
                IsOperator = entity.IsOperator,
                IsPlanned = entity.IsPlanned,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                IsDeleted = entity.IsDeleted,
                LastModificationTime = entity.LastModificationTime.GetValueOrDefault(),
                LastModifierId = entity.LastModifierId.GetValueOrDefault(),
                Id = id,
                DataOpenStatus = lockRow,
                DataOpenStatusUserId = userId

            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id }, false, false, "");

            var haltReasons = queryFactory.Update<SelectHaltReasonsDto>(query, "Id", true);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectHaltReasonsDto>(haltReasons);

        }
    }
}

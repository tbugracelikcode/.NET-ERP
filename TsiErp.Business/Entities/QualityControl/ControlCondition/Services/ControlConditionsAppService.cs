using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.QualityControl.ControlCondition.Validations;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.QualityControl.ControlCondition;
using TsiErp.Entities.Entities.QualityControl.ControlCondition.Dtos;
using TsiErp.Entities.Entities.QualityControl.ControlType;
using TsiErp.Entities.Entities.QualityControl.ControlType.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.ControlConditions.Page;

namespace TsiErp.Business.Entities.QualityControl.ControlCondition.Services
{
    [ServiceRegistration(typeof(IControlConditionsAppService), DependencyInjectionType.Scoped)]
    public class ControlConditionsAppService : ApplicationService<ControlConditionResources>, IControlConditionsAppService
    {

        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }

        public ControlConditionsAppService(IStringLocalizer<ControlConditionResources> l, IFicheNumbersAppService ficheNumbersAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
        }

        [ValidationAspect(typeof(CreateControlConditionsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectControlConditionsDto>> CreateAsync(CreateControlConditionsDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var listQuery = queryFactory.Query().From(Tables.ControlConditions).Select("*").Where(new { Code = input.Code }, false, false, "");

                var list = queryFactory.ControlList<ControlConditions>(listQuery).ToList();

                #region Code Control 

                if (list.Count > 0)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["CodeControlManager"]);
                }

                #endregion


                var query = queryFactory.Query().From(Tables.ControlConditions).Insert(new CreateControlConditionsDto
                {
                    Code = input.Code,
                    Description_ = input.Description_,
                    Name = input.Name,
                    IsActive = true,
                    Id = GuidGenerator.CreateGuid(),
                    CreationTime = DateTime.Now,
                    CreatorId = LoginedUserService.UserId,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = Guid.Empty,
                    DeletionTime = null,
                    LastModificationTime = null,
                    LastModifierId = Guid.Empty,
                    IsDeleted = false,
                    QualityPlanTypes = input.QualityPlanTypes
                });


                var controlConditions = queryFactory.Insert<SelectControlConditionsDto>(query, "Id", true);

                await FicheNumbersAppService.UpdateFicheNumberAsync("ControlConditionsChildMenu", input.Code);

                LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.ControlConditions, LogType.Insert, controlConditions.Id);


                return new SuccessDataResult<SelectControlConditionsDto>(controlConditions);
            }
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory.Query().From(Tables.ControlConditions).Delete(LoginedUserService.UserId).Where(new { Id = id }, true, true, "");

                var controlConditions = queryFactory.Update<SelectControlConditionsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.ControlConditions, LogType.Delete, id);

                return new SuccessDataResult<SelectControlConditionsDto>(controlConditions);
            }
        }

        public async Task<IDataResult<SelectControlConditionsDto>> GetAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {

                var query = queryFactory.Query().From(Tables.ControlConditions).Select("*").Where(
                new
                {
                    Id = id
                }, true, true, "");

                var controlConditions = queryFactory.Get<SelectControlConditionsDto>(query);


                LogsAppService.InsertLogToDatabase(controlConditions, controlConditions, LoginedUserService.UserId, Tables.ControlConditions, LogType.Get, id);

                return new SuccessDataResult<SelectControlConditionsDto>(controlConditions);

            }
        }

        public async Task<IDataResult<IList<ListControlConditionsDto>>> GetListAsync(ListControlConditionsParameterDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory.Query().From(Tables.ControlConditions).Select("*").Where(null, true, true, "");

                var controlConditions = queryFactory.GetList<ListControlConditionsDto>(query).ToList();

                return new SuccessDataResult<IList<ListControlConditionsDto>>(controlConditions);
            }
        }

        [ValidationAspect(typeof(UpdateControlConditionsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectControlConditionsDto>> UpdateAsync(UpdateControlConditionsDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.ControlConditions).Select("*").Where(new { Id = input.Id }, true, true, "");
                var entity = queryFactory.Get<ControlConditions>(entityQuery);

                #region Update Control

                var listQuery = queryFactory.Query().From(Tables.ControlConditions).Select("*").Where(new { Code = input.Code }, false, false, "");
                var list = queryFactory.GetList<ControlConditions>(listQuery).ToList();

                if (list.Count > 0 && entity.Code != input.Code)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["UpdateControlManager"]);
                }

                #endregion

                var query = queryFactory.Query().From(Tables.ControlConditions).Update(new UpdateControlConditionsDto
                {
                    Code = input.Code,
                    Description_ = input.Description_,
                    Name = input.Name,
                    Id = input.Id,
                    IsActive = input.IsActive,
                    CreationTime = entity.CreationTime.Value,
                    CreatorId = entity.CreatorId.Value,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = entity.DeleterId.GetValueOrDefault(),
                    DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                    IsDeleted = entity.IsDeleted,
                    LastModificationTime = DateTime.Now,
                    LastModifierId = LoginedUserService.UserId,
                    QualityPlanTypes = input.QualityPlanTypes
                }).Where(new { Id = input.Id }, true, true, "");

                var controlConditions = queryFactory.Update<SelectControlConditionsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(entity, controlConditions, LoginedUserService.UserId, Tables.ControlConditions, LogType.Update, entity.Id);

                return new SuccessDataResult<SelectControlConditionsDto>(controlConditions);
            }
        }

        public async Task<IDataResult<SelectControlConditionsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.ControlConditions).Select("*").Where(new { Id = id }, true, true, "");

                var entity = queryFactory.Get<ControlConditions>(entityQuery);

                var query = queryFactory.Query().From(Tables.ControlConditions).Update(new UpdateControlConditionsDto
                {
                    Code = entity.Code,
                    Description_ = entity.Description_,
                    Name = entity.Name,
                    IsActive = entity.IsActive,
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
                    QualityPlanTypes = entity.QualityPlanTypes
                }).Where(new { Id = id }, true, true, "");

                var controlConditions = queryFactory.Update<SelectControlConditionsDto>(query, "Id", true);

                return new SuccessDataResult<SelectControlConditionsDto>(controlConditions);

            }
        }
    }
}

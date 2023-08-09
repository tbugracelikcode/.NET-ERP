using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.QualityControl.ControlType.Validations;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.QualityControl.ControlType;
using TsiErp.Entities.Entities.QualityControl.ControlType.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.ControlTypes.Page;

namespace TsiErp.Business.Entities.QualityControl.ControlType.Services
{
    [ServiceRegistration(typeof(IControlTypesAppService), DependencyInjectionType.Scoped)]
    public class ControlTypesAppService : ApplicationService<ControlTypeResources>, IControlTypesAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        public ControlTypesAppService(IStringLocalizer<ControlTypeResources> l) : base(l)
        {
        }


        [ValidationAspect(typeof(CreateControlTypesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectControlTypesDto>> CreateAsync(CreateControlTypesDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var listQuery = queryFactory.Query().From(Tables.ControlTypes).Select("*").Where(new { Code = input.Code }, false, false, "");

                var list = queryFactory.ControlList<ControlTypes>(listQuery).ToList();

                #region Code Control 

                if (list.Count > 0)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["CodeControlManager"]);
                }

                #endregion


                var query = queryFactory.Query().From(Tables.ControlTypes).Insert(new CreateControlTypesDto
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
                    IsDeleted = false
                });


                var controlTypes = queryFactory.Insert<SelectControlTypesDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.ControlTypes, LogType.Insert, controlTypes.Id);


                return new SuccessDataResult<SelectControlTypesDto>(controlTypes);
            }
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory.Query().From(Tables.ControlTypes).Delete(LoginedUserService.UserId).Where(new { Id = id }, true, true, "");

                var controlTypes = queryFactory.Update<SelectControlTypesDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.ControlTypes, LogType.Delete, id);

                return new SuccessDataResult<SelectControlTypesDto>(controlTypes);
            }
        }

        public async Task<IDataResult<SelectControlTypesDto>> GetAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {

                var query = queryFactory.Query().From(Tables.ControlTypes).Select("*").Where(
                new
                {
                    Id = id
                }, true, true, "");

                var controlTypes = queryFactory.Get<SelectControlTypesDto>(query);


                LogsAppService.InsertLogToDatabase(controlTypes, controlTypes, LoginedUserService.UserId, Tables.ControlTypes, LogType.Get, id);

                return new SuccessDataResult<SelectControlTypesDto>(controlTypes);

            }
        }

        public async Task<IDataResult<IList<ListControlTypesDto>>> GetListAsync(ListControlTypesParameterDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory.Query().From(Tables.ControlTypes).Select("*").Where(null, true, true, "");

                var controlTypes = queryFactory.GetList<ListControlTypesDto>(query).ToList();

                return new SuccessDataResult<IList<ListControlTypesDto>>(controlTypes);
            }
        }

        [ValidationAspect(typeof(UpdateControlTypesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectControlTypesDto>> UpdateAsync(UpdateControlTypesDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.ControlTypes).Select("*").Where(new { Id = input.Id }, true, true, "");
                var entity = queryFactory.Get<ControlTypes>(entityQuery);

                #region Update Control

                var listQuery = queryFactory.Query().From(Tables.ControlTypes).Select("*").Where(new { Code = input.Code }, false, false, "");
                var list = queryFactory.GetList<ControlTypes>(listQuery).ToList();

                if (list.Count > 0 && entity.Code != input.Code)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["UpdateControlManager"]);
                }

                #endregion

                var query = queryFactory.Query().From(Tables.ControlTypes).Update(new UpdateControlTypesDto
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
                    LastModifierId = LoginedUserService.UserId
                }).Where(new { Id = input.Id }, true, true, "");

                var controlTypes = queryFactory.Update<SelectControlTypesDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(entity, controlTypes, LoginedUserService.UserId, Tables.ControlTypes, LogType.Update, entity.Id);

                return new SuccessDataResult<SelectControlTypesDto>(controlTypes);
            }
        }

        public async Task<IDataResult<SelectControlTypesDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.ControlTypes).Select("*").Where(new { Id = id }, true, true, "");

                var entity = queryFactory.Get<ControlTypes>(entityQuery);

                var query = queryFactory.Query().From(Tables.ControlTypes).Update(new UpdateControlTypesDto
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
                    DataOpenStatusUserId = userId

                }).Where(new { Id = id }, true, true, "");

                var controlTypes = queryFactory.Update<SelectControlTypesDto>(query, "Id", true);

                return new SuccessDataResult<SelectControlTypesDto>(controlTypes);

            }
        }
    }
}

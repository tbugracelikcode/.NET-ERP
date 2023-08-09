using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.QualityControl.UnsuitabilityTypesItem.Validations;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityTypesItem;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityTypesItem.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.UnsuitabilityTypesItem.Page;

namespace TsiErp.Business.Entities.QualityControl.UnsuitabilityTypesItem.Services
{
    [ServiceRegistration(typeof(IUnsuitabilityTypesItemsAppService), DependencyInjectionType.Scoped)]
    public class UnsuitabilityTypesItemsAppService : ApplicationService<UnsuitabilityTypesItemResources>, IUnsuitabilityTypesItemsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        public UnsuitabilityTypesItemsAppService(IStringLocalizer<UnsuitabilityTypesItemResources> l) : base(l)
        {
        }

        [ValidationAspect(typeof(CreateUnsuitabilityTypesItemsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectUnsuitabilityTypesItemsDto>> CreateAsync(CreateUnsuitabilityTypesItemsDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var listQuery = queryFactory.Query().From(Tables.UnsuitabilityTypesItems).Select("*").Where(new { Code = input.Code }, false, false, "");

                var list = queryFactory.ControlList<UnsuitabilityTypesItems>(listQuery).ToList();

                #region Code Control 

                if (list.Count > 0)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["CodeControlManager"]);
                }

                #endregion


                var query = queryFactory.Query().From(Tables.UnsuitabilityTypesItems).Insert(new CreateUnsuitabilityTypesItemsDto
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


                var unsuitabilityTypesItems = queryFactory.Insert<SelectUnsuitabilityTypesItemsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.UnsuitabilityTypesItems, LogType.Insert, unsuitabilityTypesItems.Id);


                return new SuccessDataResult<SelectUnsuitabilityTypesItemsDto>(unsuitabilityTypesItems);
            }
        }

        [ValidationAspect(typeof(UpdateUnsuitabilityTypesItemsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectUnsuitabilityTypesItemsDto>> UpdateAsync(UpdateUnsuitabilityTypesItemsDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.UnsuitabilityTypesItems).Select("*").Where(new { Id = input.Id }, true, true, "");
                var entity = queryFactory.Get<UnsuitabilityTypesItems>(entityQuery);

                #region Update Control

                var listQuery = queryFactory.Query().From(Tables.UnsuitabilityTypesItems).Select("*").Where(new { Code = input.Code }, false, false, "");
                var list = queryFactory.GetList<UnsuitabilityTypesItems>(listQuery).ToList();

                if (list.Count > 0 && entity.Code != input.Code)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["UpdateControlManager"]);
                }

                #endregion

                var query = queryFactory.Query().From(Tables.UnsuitabilityTypesItems).Update(new UpdateUnsuitabilityTypesItemsDto
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

                var unsuitabilityTypesItems = queryFactory.Update<SelectUnsuitabilityTypesItemsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(entity, unsuitabilityTypesItems, LoginedUserService.UserId, Tables.UnsuitabilityTypesItems, LogType.Update, entity.Id);

                return new SuccessDataResult<SelectUnsuitabilityTypesItemsDto>(unsuitabilityTypesItems);
            }
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory.Query().From(Tables.UnsuitabilityTypesItems).Delete(LoginedUserService.UserId).Where(new { Id = id }, true, true, "");

                var unsuitabilityTypesItems = queryFactory.Update<SelectUnsuitabilityTypesItemsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.UnsuitabilityTypesItems, LogType.Delete, id);

                return new SuccessDataResult<SelectUnsuitabilityTypesItemsDto>(unsuitabilityTypesItems);
            }
        }

        public async Task<IDataResult<SelectUnsuitabilityTypesItemsDto>> GetAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {

                var query = queryFactory.Query().From(Tables.UnsuitabilityTypesItems).Select("*").Where(
                new
                {
                    Id = id
                }, true, true, "");
                var unsuitabilityTypesItems = queryFactory.Get<SelectUnsuitabilityTypesItemsDto>(query);


                LogsAppService.InsertLogToDatabase(unsuitabilityTypesItems, unsuitabilityTypesItems, LoginedUserService.UserId, Tables.UnsuitabilityTypesItems, LogType.Get, id);

                return new SuccessDataResult<SelectUnsuitabilityTypesItemsDto>(unsuitabilityTypesItems);

            }
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListUnsuitabilityTypesItemsDto>>> GetListAsync(ListUnsuitabilityTypesItemsParameterDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory.Query().From(Tables.UnsuitabilityTypesItems).Select("*").Where(null, true, true, "");
                var unsuitabilityTypesItems = queryFactory.GetList<ListUnsuitabilityTypesItemsDto>(query).ToList();
                return new SuccessDataResult<IList<ListUnsuitabilityTypesItemsDto>>(unsuitabilityTypesItems);
            }
        }

        public async Task<IDataResult<SelectUnsuitabilityTypesItemsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.UnsuitabilityTypesItems).Select("*").Where(new { Id = id }, true, true, "");

                var entity = queryFactory.Get<UnsuitabilityTypesItems>(entityQuery);

                var query = queryFactory.Query().From(Tables.UnsuitabilityTypesItems).Update(new UpdateUnsuitabilityTypesItemsDto
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

                var unsuitabilityTypesItems = queryFactory.Update<SelectUnsuitabilityTypesItemsDto>(query, "Id", true);
                return new SuccessDataResult<SelectUnsuitabilityTypesItemsDto>(unsuitabilityTypesItems);

            }
        }
    }
}

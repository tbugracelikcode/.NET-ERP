using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.Branches.Page;

namespace TsiErp.Business.Entities.Menu.Services
{
    [ServiceRegistration(typeof(IMenusAppService), DependencyInjectionType.Scoped)]
    public class MenusAppService : ApplicationService<BranchesResource>, IMenusAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        public MenusAppService(IStringLocalizer<BranchesResource> l) : base(l)
        {
        }

        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectMenusDto>> CreateAsync(CreateMenusDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                Guid addedEntityId = GuidGenerator.CreateGuid();

                var query = queryFactory.Query().From(Tables.Menus).Insert(new CreateMenusDto
                {
                    Id = addedEntityId,
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


                var menus = queryFactory.Insert<SelectMenusDto>(query, "Id", true);

                return new SuccessDataResult<SelectMenusDto>(menus);
            }

        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {

                var query = queryFactory.Query().From(Tables.Menus).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");

                var menus = queryFactory.Update<SelectMenusDto>(query, "Id", true);

                return new SuccessDataResult<SelectMenusDto>(menus);
            }

        }


        public async Task<IDataResult<SelectMenusDto>> GetAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {

                var query = queryFactory.Query().From(Tables.Menus).Select("*").Where(
                new
                {
                    Id = id
                }, false, false, "");

                var menus = queryFactory.Get<SelectMenusDto>(query);

                return new SuccessDataResult<SelectMenusDto>(menus);

            }

        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListMenusDto>>> GetListAsync(ListMenusParameterDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory.Query().From(Tables.Menus).Select("*").Where(null, false, false, "");
                var menus = queryFactory.GetList<ListMenusDto>(query).ToList();
                return new SuccessDataResult<IList<ListMenusDto>>(menus);
            }

        }


        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectMenusDto>> UpdateAsync(UpdateMenusDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.Menus).Select("*").Where(new { Id = input.Id }, false, false, "");
                var entity = queryFactory.Get<Menus>(entityQuery);

                var query = queryFactory.Query().From(Tables.Menus).Update(new UpdateMenusDto
                {
                    Id = input.Id,
                    CreationTime = entity.CreationTime.Value,
                    CreatorId = entity.CreatorId.Value,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = entity.DeleterId.Value,
                    DeletionTime = entity.DeletionTime.Value,
                    IsDeleted = entity.IsDeleted,
                    LastModificationTime = DateTime.Now,
                    LastModifierId = LoginedUserService.UserId
                }).Where(new { Id = input.Id }, false, false, "");

                var menus = queryFactory.Update<SelectMenusDto>(query, "Id", true);

                return new SuccessDataResult<SelectMenusDto>(menus);
            }
        }

        public async Task<IDataResult<SelectMenusDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.Menus).Select("*").Where(new { Id = id }, false, false, "");

                var entity = queryFactory.Get<Menus>(entityQuery);

                var query = queryFactory.Query().From(Tables.Menus).Update(new UpdateMenusDto
                {
                    
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

                }).Where(new { Id = id }, false, false, "");

                var menus = queryFactory.Update<SelectMenusDto>(query, "Id", true);
                return new SuccessDataResult<SelectMenusDto>(menus);

            }

        }
    }
}

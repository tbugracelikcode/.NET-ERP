using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
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
        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        public MenusAppService(IStringLocalizer<BranchesResource> l, IGetSQLDateAppService getSQLDateAppService) : base(l)
        {
            _GetSQLDateAppService = getSQLDateAppService;
        }

        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectMenusDto>> CreateAsync(CreateMenusDto input)
        {
            Guid addedEntityId = GuidGenerator.CreateGuid();

            var query = queryFactory.Query().From(Tables.Menus).Insert(new CreateMenusDto
            {
                Id = addedEntityId,
                MenuName = input.MenuName,
                ContextOrderNo = input.ContextOrderNo,
                MenuURL = input.MenuURL,
                ParentMenuId = input.ParentMenuId
            });


            var menus = queryFactory.Insert<SelectMenusDto>(query, "Id", true);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectMenusDto>(menus);

        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.Menus).Delete(LoginedUserService.UserId).Where(new { Id = id }, false, false, "");

            var menus = queryFactory.Update<SelectMenusDto>(query, "Id", true);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectMenusDto>(menus);

        }


        public async Task<IDataResult<SelectMenusDto>> GetAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.Menus).Select("*").Where(
            new
            {
                Id = id
            }, false, false, "").UseIsDelete(false);

            var menus = queryFactory.Get<SelectMenusDto>(query);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectMenusDto>(menus);

        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListMenusDto>>> GetListAsync(ListMenusParameterDto input)
        {
            var query = queryFactory.Query().From(Tables.Menus).Select("*").Where(null, false, false, "").UseIsDelete(false);
            var menus = queryFactory.GetList<ListMenusDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListMenusDto>>(menus);

        }

        public async Task<IDataResult<IList<SelectMenusDto>>> GetListbyParentIDAsync(Guid parentID)
        {
            var query = queryFactory.Query().From(Tables.Menus).Select("*").Where(new { ParentMenuId = parentID }, false, false, "").UseIsDelete(false);
            var menus = queryFactory.GetList<SelectMenusDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<SelectMenusDto>>(menus);

        }


        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectMenusDto>> UpdateAsync(UpdateMenusDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.Menus).Select("*").Where(new { Id = input.Id }, false, false, "");
            var entity = queryFactory.Get<Menus>(entityQuery);

            var query = queryFactory.Query().From(Tables.Menus).Update(new UpdateMenusDto
            {
                Id = input.Id,
                ParentMenuId = input.ParentMenuId,
                MenuURL = input.MenuURL,
                ContextOrderNo = input.ContextOrderNo,
                MenuName = input.MenuName
            }).Where(new { Id = input.Id }, false, false, "");

            var menus = queryFactory.Update<SelectMenusDto>(query, "Id", true);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectMenusDto>(menus);
        }

        public async Task<IDataResult<SelectMenusDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }
    }
}

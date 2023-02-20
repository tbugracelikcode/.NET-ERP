using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.Entities.Entities.Menu;
using TsiErp.Entities.Entities.Menu.Dtos;

namespace TsiErp.Business.Entities.Menu.Services
{
    [ServiceRegistration(typeof(IMenusAppService), DependencyInjectionType.Scoped)]
    public class MenusAppService : ApplicationService, IMenusAppService
    {
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectMenusDto>> CreateAsync(CreateMenusDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = ObjectMapper.Map<CreateMenusDto, Menus>(input);

                var addedEntity = await _uow.MenusRepository.InsertAsync(entity);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectMenusDto>(ObjectMapper.Map<Menus, SelectMenusDto>(addedEntity));
            }
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _uow.MenusRepository.DeleteAsync(id);
                await _uow.SaveChanges();
                return new SuccessResult("Silme işlemi başarılı.");
            }
        }


        public async Task<IDataResult<SelectMenusDto>> GetAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.MenusRepository.GetAsync(t => t.Id == id);
                var mappedEntity = ObjectMapper.Map<Menus, SelectMenusDto>(entity);
                return new SuccessDataResult<SelectMenusDto>(mappedEntity);
            }
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListMenusDto>>> GetListAsync(ListMenusParameterDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var list = await _uow.MenusRepository.GetListAsync(null);

                var mappedEntity = ObjectMapper.Map<List<Menus>, List<ListMenusDto>>(list.ToList());

                return new SuccessDataResult<IList<ListMenusDto>>(mappedEntity);
            }
        }


        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectMenusDto>> UpdateAsync(UpdateMenusDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.MenusRepository.GetAsync(x => x.Id == input.Id);

                var mappedEntity = ObjectMapper.Map<UpdateMenusDto, Menus>(input);

                await _uow.MenusRepository.UpdateAsync(mappedEntity);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectMenusDto>(ObjectMapper.Map<Menus, SelectMenusDto>(mappedEntity));
            }
        }

        public async Task<IDataResult<SelectMenusDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.MenusRepository.GetAsync(x => x.Id == id);

                var updatedEntity = await _uow.MenusRepository.LockRow(entity.Id, lockRow, userId);

                await _uow.SaveChanges();

                var mappedEntity = ObjectMapper.Map<Menus, SelectMenusDto>(updatedEntity);

                return new SuccessDataResult<SelectMenusDto>(mappedEntity);
            }
        }
    }
}

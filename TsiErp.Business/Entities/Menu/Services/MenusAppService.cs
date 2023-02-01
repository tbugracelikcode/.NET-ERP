using Tsi.Application.Contract.Services.EntityFrameworkCore;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Menu;
using TsiErp.Entities.Entities.Menu.Dtos;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Entities.Entities.Menu;

namespace TsiErp.Business.Entities.Menu.Services
{
    [ServiceRegistration(typeof(IMenusAppService), DependencyInjectionType.Scoped)]
    public class MenusAppService : ApplicationService, IMenusAppService
    {
        private readonly IMenusRepository _repository;


        public MenusAppService(IMenusRepository repository)
        {
            _repository = repository;
        }

        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectMenusDto>> CreateAsync(CreateMenusDto input)
        {

            var entity = ObjectMapper.Map<CreateMenusDto, Menus>(input);

            var addedEntity = await _repository.InsertAsync(entity);
            await _repository.SaveChanges();

            return new SuccessDataResult<SelectMenusDto>(ObjectMapper.Map<Menus, SelectMenusDto>(addedEntity));
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
            await _repository.SaveChanges();
            return new SuccessResult("Silme işlemi başarılı.");
        }


        public async Task<IDataResult<SelectMenusDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id);
            var mappedEntity = ObjectMapper.Map<Menus, SelectMenusDto>(entity);
            return new SuccessDataResult<SelectMenusDto>(mappedEntity);
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListMenusDto>>> GetListAsync(ListMenusParameterDto input)
        {
            var list = await _repository.GetListAsync(null);

            var mappedEntity = ObjectMapper.Map<List<Menus>, List<ListMenusDto>>(list.ToList());

            return new SuccessDataResult<IList<ListMenusDto>>(mappedEntity);
        }


        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectMenusDto>> UpdateAsync(UpdateMenusDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            var mappedEntity = ObjectMapper.Map<UpdateMenusDto, Menus>(input);

            await _repository.UpdateAsync(mappedEntity);
            await _repository.SaveChanges();

            return new SuccessDataResult<SelectMenusDto>(ObjectMapper.Map<Menus, SelectMenusDto>(mappedEntity));
        }
    }
}

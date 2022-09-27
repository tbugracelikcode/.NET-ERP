using System.Reflection;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.DependencyResolvers.Autofac;
using TsiErp.Business.Entities.Route.Validations;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Route;
using TsiErp.Entities.Entities.Route;
using TsiErp.Entities.Entities.Route.Dtos;

namespace TsiErp.Business.Entities.Route.Services
{
    [ServiceRegistration(typeof(IRoutesAppService), DependencyInjectionType.Scoped)]
    public class RoutesAppService : ApplicationService, IRoutesAppService
    {
        private readonly IRoutesRepository _repository;

        public RoutesAppService(IRoutesRepository repository)
        {
            _repository = repository;
        }


        [ValidationAspect(typeof(CreateRoutesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectRoutesDto>> CreateAsync(CreateRoutesDto input)
        {
            var entity = ObjectMapper.Map<CreateRoutesDto, Routes>(input);

            var addedEntity = await _repository.InsertAsync(entity);

            return new SuccessDataResult<SelectRoutesDto>(ObjectMapper.Map<Routes, SelectRoutesDto>(addedEntity));
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
            return new SuccessResult("Silme işlemi başarılı.");
        }


        public async Task<IDataResult<SelectRoutesDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id);
            var mappedEntity = ObjectMapper.Map<Routes, SelectRoutesDto>(entity);
            return new SuccessDataResult<SelectRoutesDto>(mappedEntity);
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListRoutesDto>>> GetListAsync(ListRoutesParameterDto input)
        {
            var list = await _repository.GetListAsync(t => t.IsActive == input.IsActive);

            var mappedEntity = ObjectMapper.Map<List<Routes>, List<ListRoutesDto>>(list.ToList());

            return new SuccessDataResult<IList<ListRoutesDto>>(mappedEntity);
        }


        [ValidationAspect(typeof(UpdateRoutesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectRoutesDto>> UpdateAsync(UpdateRoutesDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            var mappedEntity = ObjectMapper.Map<UpdateRoutesDto, Routes>(input);

            await _repository.UpdateAsync(mappedEntity);

            return new SuccessDataResult<SelectRoutesDto>(ObjectMapper.Map<Routes, SelectRoutesDto>(mappedEntity));
        }
    }
}

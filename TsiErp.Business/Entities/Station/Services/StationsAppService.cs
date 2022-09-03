using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.Entities.Branch.Validations;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Station;
using TsiErp.Entities.Entities.Branch.Dtos;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Entities.Entities.Station.Dtos;
using TsiErp.EntityContracts.Station;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Entities.Entities.Station;

namespace TsiErp.Business.Entities.Station.Services
{
    [ServiceRegistration(typeof(IStationsAppService), DependencyInjectionType.Scoped)]
    public class StationsAppService : ApplicationService, IStationsAppService
    {
        private readonly IStationsRepository _repository;

        public StationsAppService(IStationsRepository repository)
        {
            _repository = repository;
        }

        [ValidationAspect(typeof(CreateStationsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectStationsDto>> CreateAsync(CreateStationsDto input)
        {
            var entity = ObjectMapper.Map<CreateStationsDto, Stations>(input);

            var addedEntity = await _repository.InsertAsync(entity);

            return new SuccessDataResult<SelectStationsDto>(ObjectMapper.Map<Stations, SelectStationsDto>(addedEntity));
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
            return new SuccessResult("Silme işlemi başarılı.");
        }

        public async Task<IDataResult<SelectStationsDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id, t => t.StationGroups);
            var mappedEntity = ObjectMapper.Map<Stations, SelectStationsDto>(entity);
            return new SuccessDataResult<SelectStationsDto>(mappedEntity);
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListStationsDto>>> GetListAsync(ListStationsParameterDto input)
        {
            var list = await _repository.GetListAsync(t => t.IsActive == input.IsActive, t => t.StationGroups);

            var mappedEntity = ObjectMapper.Map<List<Stations>, List<ListStationsDto>>(list.ToList());

            return new SuccessDataResult<IList<ListStationsDto>>(mappedEntity);
        }


        [ValidationAspect(typeof(UpdateStationsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectStationsDto>> UpdateAsync(UpdateStationsDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            var mappedEntity = ObjectMapper.Map<UpdateStationsDto, Stations>(input);

            await _repository.UpdateAsync(mappedEntity);

            return new SuccessDataResult<SelectStationsDto>(ObjectMapper.Map<Stations, SelectStationsDto>(mappedEntity));
        }
    }
}

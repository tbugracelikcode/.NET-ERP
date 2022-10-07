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
using TsiErp.Business.Entities.Station.Services;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Station;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.StationGroup;
using TsiErp.Entities.Entities.Station.Dtos;
using TsiErp.Entities.Entities.Station;
using TsiErp.EntityContracts.Station;
using TsiErp.EntityContracts.StationGroup;
using TsiErp.Entities.Entities.StationGroup.Dtos;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Entities.Entities.StationGroup;
using TsiErp.Business.Entities.StationGroup.BusinessRules;

namespace TsiErp.Business.Entities.StationGroup.Services
{
    [ServiceRegistration(typeof(IStationGroupsAppService), DependencyInjectionType.Scoped)]
    public class StationGroupsAppService : ApplicationService, IStationGroupsAppService
    {

        private readonly IStationGroupsRepository _repository;

        StationGroupManager _manager { get; set; } = new StationGroupManager();

        public StationGroupsAppService(IStationGroupsRepository repository)
        {
            _repository = repository;
        }

        [ValidationAspect(typeof(CreateStationGroupsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectStationGroupsDto>> CreateAsync(CreateStationGroupsDto input)
        {
            await _manager.CodeControl(_repository, input.Code);

            var entity = ObjectMapper.Map<CreateStationGroupsDto, StationGroups>(input);

            var addedEntity = await _repository.InsertAsync(entity);

            return new SuccessDataResult<SelectStationGroupsDto>(ObjectMapper.Map<StationGroups, SelectStationGroupsDto>(addedEntity));
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            await _manager.DeleteControl(_repository, id);
            await _repository.DeleteAsync(id);
            return new SuccessResult("Silme işlemi başarılı.");
        }

        public async Task<IDataResult<SelectStationGroupsDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id, t => t.Stations);
            var mappedEntity = ObjectMapper.Map<StationGroups, SelectStationGroupsDto>(entity);
            return new SuccessDataResult<SelectStationGroupsDto>(mappedEntity);
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListStationGroupsDto>>> GetListAsync(ListStationGroupsParameterDto input)
        {
            var list = await _repository.GetListAsync(t=>t.IsActive==input.IsActive, t => t.Stations);

            var mappedEntity = ObjectMapper.Map<List<StationGroups>, List<ListStationGroupsDto>>(list.ToList());

            return new SuccessDataResult<IList<ListStationGroupsDto>>(mappedEntity);
        }


        [ValidationAspect(typeof(UpdateStationGroupsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectStationGroupsDto>> UpdateAsync(UpdateStationGroupsDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            await _manager.UpdateControl(_repository, input.Code, input.Id, entity);

            var mappedEntity = ObjectMapper.Map<UpdateStationGroupsDto, StationGroups>(input);

            await _repository.UpdateAsync(mappedEntity);

            return new SuccessDataResult<SelectStationGroupsDto>(ObjectMapper.Map<StationGroups, SelectStationGroupsDto>(mappedEntity));
        }
    }
}

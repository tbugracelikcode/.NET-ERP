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
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.Entities.Entities.WorkOrder.Dtos;
using TsiErp.Entities.Entities.WorkOrder;

namespace TsiErp.Business.Entities.StationGroup.Services
{
    [ServiceRegistration(typeof(IStationGroupsAppService), DependencyInjectionType.Scoped)]
    public class StationGroupsAppService : ApplicationService, IStationGroupsAppService
    {
        StationGroupManager _manager { get; set; } = new StationGroupManager();


        [ValidationAspect(typeof(CreateStationGroupsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectStationGroupsDto>> CreateAsync(CreateStationGroupsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.CodeControl(_uow.StationGroupsRepository, input.Code);

                var entity = ObjectMapper.Map<CreateStationGroupsDto, StationGroups>(input);

                var addedEntity = await _uow.StationGroupsRepository.InsertAsync(entity);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectStationGroupsDto>(ObjectMapper.Map<StationGroups, SelectStationGroupsDto>(addedEntity));
            }
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.DeleteControl(_uow.StationGroupsRepository, id);
                await _uow.StationGroupsRepository.DeleteAsync(id);
                await _uow.SaveChanges();
                return new SuccessResult("Silme işlemi başarılı.");
            }
        }

        public async Task<IDataResult<SelectStationGroupsDto>> GetAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.StationGroupsRepository.GetAsync(t => t.Id == id, t => t.Stations);
                var mappedEntity = ObjectMapper.Map<StationGroups, SelectStationGroupsDto>(entity);
                return new SuccessDataResult<SelectStationGroupsDto>(mappedEntity);
            }
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListStationGroupsDto>>> GetListAsync(ListStationGroupsParameterDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var list = await _uow.StationGroupsRepository.GetListAsync(t => t.IsActive == input.IsActive, t => t.Stations);

                var mappedEntity = ObjectMapper.Map<List<StationGroups>, List<ListStationGroupsDto>>(list.ToList());

                return new SuccessDataResult<IList<ListStationGroupsDto>>(mappedEntity);
            }
        }


        [ValidationAspect(typeof(UpdateStationGroupsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectStationGroupsDto>> UpdateAsync(UpdateStationGroupsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.StationGroupsRepository.GetAsync(x => x.Id == input.Id);

                await _manager.UpdateControl(_uow.StationGroupsRepository, input.Code, input.Id, entity);

                var mappedEntity = ObjectMapper.Map<UpdateStationGroupsDto, StationGroups>(input);

                await _uow.StationGroupsRepository.UpdateAsync(mappedEntity);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectStationGroupsDto>(ObjectMapper.Map<StationGroups, SelectStationGroupsDto>(mappedEntity));
            }
        }

        public async Task<IDataResult<SelectStationGroupsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.StationGroupsRepository.GetAsync(x => x.Id == id);

                var updatedEntity = await _uow.StationGroupsRepository.LockRow(entity.Id, lockRow, userId);

                await _uow.SaveChanges();

                var mappedEntity = ObjectMapper.Map<StationGroups, SelectStationGroupsDto>(updatedEntity);

                return new SuccessDataResult<SelectStationGroupsDto>(mappedEntity);
            }
        }
    }
}

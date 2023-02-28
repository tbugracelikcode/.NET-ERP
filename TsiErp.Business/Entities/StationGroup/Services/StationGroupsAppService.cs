using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results; using TsiErp.Localizations.Resources.Branches.Page;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.StationGroup.BusinessRules;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.StationGroup;
using TsiErp.Entities.Entities.StationGroup.Dtos;
using TsiErp.EntityContracts.StationGroup;
using Microsoft.Extensions.Localization;

namespace TsiErp.Business.Entities.StationGroup.Services
{
    [ServiceRegistration(typeof(IStationGroupsAppService), DependencyInjectionType.Scoped)]
    public class StationGroupsAppService : ApplicationService<BranchesResource>, IStationGroupsAppService
    {
        public StationGroupsAppService(IStringLocalizer<BranchesResource> l) : base(l)
        {
        }

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
                input.Id = addedEntity.Id;
                var log = LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, "StationGroups", LogType.Insert, addedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);
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
                var log = LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, "StationGroups", LogType.Delete, id);
                await _uow.LogsRepository.InsertAsync(log);
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
                var log = LogsAppService.InsertLogToDatabase(mappedEntity, mappedEntity, LoginedUserService.UserId, "StationGroups", LogType.Get, id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();
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
                var before = ObjectMapper.Map<StationGroups, UpdateStationGroupsDto>(entity);
                var log = LogsAppService.InsertLogToDatabase(before, input, LoginedUserService.UserId, "StationGroups", LogType.Update, mappedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);
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

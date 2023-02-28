using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results; using TsiErp.Localizations.Resources.Branches.Page;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Warehouse.BusinessRules;
using TsiErp.Business.Entities.Warehouse.Validations;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.WareHouse;
using TsiErp.Entities.Entities.WareHouse.Dtos;
using Microsoft.Extensions.Localization;

namespace TsiErp.Business.Entities.Warehouse.Services
{
    [ServiceRegistration(typeof(IWarehousesAppService), DependencyInjectionType.Scoped)]
    public class WarehousesAppService : ApplicationService<BranchesResource>, IWarehousesAppService
    {
        public WarehousesAppService(IStringLocalizer<BranchesResource> l) : base(l)
        {
        }

        WarehouseManager _manager { get; set; } = new WarehouseManager();


        [ValidationAspect(typeof(CreateWarehousesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectWarehousesDto>> CreateAsync(CreateWarehousesDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.CodeControl(_uow.WarehousesRepository, input.Code);

                var entity = ObjectMapper.Map<CreateWarehousesDto, Warehouses>(input);

                var addedEntity = await _uow.WarehousesRepository.InsertAsync(entity);
                input.Id = addedEntity.Id;
                var log = LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, "Warehouses", LogType.Insert, addedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectWarehousesDto>(ObjectMapper.Map<Warehouses, SelectWarehousesDto>(addedEntity));
            }
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.DeleteControl(_uow.WarehousesRepository, id);
                await _uow.WarehousesRepository.DeleteAsync(id);
                var log = LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, "Warehouses", LogType.Delete, id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();
                return new SuccessResult("Silme işlemi başarılı.");
            }
        }

        public async Task<IDataResult<SelectWarehousesDto>> GetAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.WarehousesRepository.GetAsync(t => t.Id == id, t => t.SalesPropositions);
                var mappedEntity = ObjectMapper.Map<Warehouses, SelectWarehousesDto>(entity);
                var log = LogsAppService.InsertLogToDatabase(mappedEntity, mappedEntity, LoginedUserService.UserId, "Warehouses", LogType.Get, id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();
                return new SuccessDataResult<SelectWarehousesDto>(mappedEntity);
            }
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListWarehousesDto>>> GetListAsync(ListWarehousesParameterDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var list = await _uow.WarehousesRepository.GetListAsync(t => t.IsActive == input.IsActive, t => t.SalesPropositions);

                var mappedEntity = ObjectMapper.Map<List<Warehouses>, List<ListWarehousesDto>>(list.ToList());

                return new SuccessDataResult<IList<ListWarehousesDto>>(mappedEntity);
            }
        }


        [ValidationAspect(typeof(UpdateWarehousesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectWarehousesDto>> UpdateAsync(UpdateWarehousesDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.WarehousesRepository.GetAsync(x => x.Id == input.Id);

                await _manager.UpdateControl(_uow.WarehousesRepository, input.Code, input.Id, entity);

                var mappedEntity = ObjectMapper.Map<UpdateWarehousesDto, Warehouses>(input);

                await _uow.WarehousesRepository.UpdateAsync(mappedEntity);

                var before = ObjectMapper.Map<Warehouses, UpdateWarehousesDto>(entity);
                var log = LogsAppService.InsertLogToDatabase(before, input, LoginedUserService.UserId, "Warehouses", LogType.Update, mappedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectWarehousesDto>(ObjectMapper.Map<Warehouses, SelectWarehousesDto>(mappedEntity));
            }
        }

        public async Task<IDataResult<SelectWarehousesDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.WarehousesRepository.GetAsync(x => x.Id == id);

                var updatedEntity = await _uow.WarehousesRepository.LockRow(entity.Id, lockRow, userId);

                await _uow.SaveChanges();

                var mappedEntity = ObjectMapper.Map<Warehouses, SelectWarehousesDto>(updatedEntity);

                return new SuccessDataResult<SelectWarehousesDto>(mappedEntity);
            }
        }
    }
}

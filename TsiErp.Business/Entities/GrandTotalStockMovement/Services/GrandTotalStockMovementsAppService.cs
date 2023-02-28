using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results; using TsiErp.Localizations.Resources.Branches.Page;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.GrandTotalStockMovement.BusinessRules;
using TsiErp.Business.Entities.GrandTotalStockMovement.Validations;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GrandTotalStockMovement;
using TsiErp.Entities.Entities.GrandTotalStockMovement.Dtos;
using Microsoft.Extensions.Localization;

namespace TsiErp.Business.Entities.GrandTotalStockMovement.Services
{
    [ServiceRegistration(typeof(IGrandTotalStockMovementsAppService), DependencyInjectionType.Scoped)]
    public class GrandTotalStockMovementsAppService : ApplicationService<BranchesResource>, IGrandTotalStockMovementsAppService
    {
        public GrandTotalStockMovementsAppService(IStringLocalizer<BranchesResource> l) : base(l)
        {
        }

        GrandTotalStockMovementManager _manager { get; set; } = new GrandTotalStockMovementManager();

        [ValidationAspect(typeof(CreateGrandTotalStockMovementsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectGrandTotalStockMovementsDto>> CreateAsync(CreateGrandTotalStockMovementsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = ObjectMapper.Map<CreateGrandTotalStockMovementsDto, GrandTotalStockMovements>(input);

                var addedEntity = await _uow.GrandTotalStockMovementsRepository.InsertAsync(entity);
                input.Id = addedEntity.Id;
                var log = LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, "GrandTotalStockMovements", LogType.Insert, addedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectGrandTotalStockMovementsDto>(ObjectMapper.Map<GrandTotalStockMovements, SelectGrandTotalStockMovementsDto>(addedEntity));
            }
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _uow.GrandTotalStockMovementsRepository.DeleteAsync(id);

                var log = LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, "GrandTotalStockMovements", LogType.Delete, id);
                await _uow.LogsRepository.InsertAsync(log);

                await _uow.SaveChanges();
                return new SuccessResult("Silme işlemi başarılı.");
            }
        }


        public async Task<IDataResult<SelectGrandTotalStockMovementsDto>> GetAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.GrandTotalStockMovementsRepository.GetAsync(t => t.Id == id, t => t.Branches, t => t.Warehouses, t => t.Products);
                var mappedEntity = ObjectMapper.Map<GrandTotalStockMovements, SelectGrandTotalStockMovementsDto>(entity);
                var log = LogsAppService.InsertLogToDatabase(mappedEntity, mappedEntity, LoginedUserService.UserId, "GrandTotalStockMovements", LogType.Get, id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();
                return new SuccessDataResult<SelectGrandTotalStockMovementsDto>(mappedEntity);
            }
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListGrandTotalStockMovementsDto>>> GetListAsync(ListGrandTotalStockMovementsParameterDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var list = await _uow.GrandTotalStockMovementsRepository.GetListAsync(null, t => t.Branches, t => t.Warehouses, t => t.Products);

                var mappedEntity = ObjectMapper.Map<List<GrandTotalStockMovements>, List<ListGrandTotalStockMovementsDto>>(list.ToList());

                return new SuccessDataResult<IList<ListGrandTotalStockMovementsDto>>(mappedEntity);
            }
        }


        [ValidationAspect(typeof(UpdateGrandTotalStockMovementsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectGrandTotalStockMovementsDto>> UpdateAsync(UpdateGrandTotalStockMovementsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.GrandTotalStockMovementsRepository.GetAsync(x => x.Id == input.Id);

                var mappedEntity = ObjectMapper.Map<UpdateGrandTotalStockMovementsDto, GrandTotalStockMovements>(input);

                await _uow.GrandTotalStockMovementsRepository.UpdateAsync(mappedEntity);
                var before = ObjectMapper.Map<GrandTotalStockMovements, UpdateGrandTotalStockMovementsDto>(entity);
                var log = LogsAppService.InsertLogToDatabase(before, input, LoginedUserService.UserId, "GrandTotalStockMovements", LogType.Update, mappedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);

                await _uow.SaveChanges();

                return new SuccessDataResult<SelectGrandTotalStockMovementsDto>(ObjectMapper.Map<GrandTotalStockMovements, SelectGrandTotalStockMovementsDto>(mappedEntity));
            }
        }

        public Task<IDataResult<SelectGrandTotalStockMovementsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}

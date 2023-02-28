using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using TsiErp.Localizations.Resources.ByDateStockMovements.Page;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.ByDateStockMovement.BusinessRules;
using TsiErp.Business.Entities.ByDateStockMovement.Validations;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.ByDateStockMovement;
using TsiErp.Entities.Entities.ByDateStockMovement.Dtos;
using Microsoft.Extensions.Localization;

namespace TsiErp.Business.Entities.ByDateStockMovement.Services
{
    [ServiceRegistration(typeof(IByDateStockMovementsAppService), DependencyInjectionType.Scoped)]
    public class ByDateStockMovementsAppService : ApplicationService<ByDateStockMovementsResource>, IByDateStockMovementsAppService
    {
        public ByDateStockMovementsAppService(IStringLocalizer<ByDateStockMovementsResource> l) : base(l)
        {
        }

        ByDateStockMovementManager _manager { get; set; } = new ByDateStockMovementManager();


        [ValidationAspect(typeof(CreateByDateStockMovementsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectByDateStockMovementsDto>> CreateAsync(CreateByDateStockMovementsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = ObjectMapper.Map<CreateByDateStockMovementsDto, ByDateStockMovements>(input);

                var addedEntity = await _uow.ByDateStockMovementsRepository.InsertAsync(entity);

                input.Id = addedEntity.Id;
                var log = LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, "ByDateStockMovements", LogType.Insert, addedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);

                await _uow.SaveChanges();

                return new SuccessDataResult<SelectByDateStockMovementsDto>(ObjectMapper.Map<ByDateStockMovements, SelectByDateStockMovementsDto>(addedEntity));
            }
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _uow.ByDateStockMovementsRepository.DeleteAsync(id);

                var log = LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, "ByDateStockMovements", LogType.Delete, id);
                await _uow.LogsRepository.InsertAsync(log);

                await _uow.SaveChanges();
                return new SuccessResult(L["DeleteSuccessMessage"]);
            }
        }


        public async Task<IDataResult<SelectByDateStockMovementsDto>> GetAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.ByDateStockMovementsRepository.GetAsync(t => t.Id == id, t => t.Branches, t => t.Warehouses, t => t.Products);
                var mappedEntity = ObjectMapper.Map<ByDateStockMovements, SelectByDateStockMovementsDto>(entity);

                var log = LogsAppService.InsertLogToDatabase(mappedEntity, mappedEntity, LoginedUserService.UserId, "ByDateStockMovements", LogType.Get, id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectByDateStockMovementsDto>(mappedEntity);
            }
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListByDateStockMovementsDto>>> GetListAsync(ListByDateStockMovementsParameterDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var list = await _uow.ByDateStockMovementsRepository.GetListAsync(null, t => t.Branches, t => t.Warehouses, t => t.Products);

                var mappedEntity = ObjectMapper.Map<List<ByDateStockMovements>, List<ListByDateStockMovementsDto>>(list.ToList());

                return new SuccessDataResult<IList<ListByDateStockMovementsDto>>(mappedEntity);
            }
        }


        [ValidationAspect(typeof(UpdateByDateStockMovementsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectByDateStockMovementsDto>> UpdateAsync(UpdateByDateStockMovementsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.ByDateStockMovementsRepository.GetAsync(x => x.Id == input.Id);

                var mappedEntity = ObjectMapper.Map<UpdateByDateStockMovementsDto, ByDateStockMovements>(input);

                await _uow.ByDateStockMovementsRepository.UpdateAsync(mappedEntity);

                var before = ObjectMapper.Map<ByDateStockMovements, UpdateByDateStockMovementsDto>(entity);
                var log = LogsAppService.InsertLogToDatabase(before, input, LoginedUserService.UserId, "ByDateStockMovements", LogType.Update, mappedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);

                await _uow.SaveChanges();

                return new SuccessDataResult<SelectByDateStockMovementsDto>(ObjectMapper.Map<ByDateStockMovements, SelectByDateStockMovementsDto>(mappedEntity));
            }
        }

        public Task<IDataResult<SelectByDateStockMovementsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}

using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Period.BusinessRules;
using TsiErp.Business.Entities.Period.Validations;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.Period;
using TsiErp.Entities.Entities.Period.Dtos;

namespace TsiErp.Business.Entities.Period.Services
{
    [ServiceRegistration(typeof(IPeriodsAppService), DependencyInjectionType.Scoped)]
    public class PeriodsAppService :  IPeriodsAppService
    {
        PeriodManager _manager { get; set; } = new PeriodManager();

        [ValidationAspect(typeof(CreatePeriodsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPeriodsDto>> CreateAsync(CreatePeriodsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.CodeControl(_uow.PeriodsRepository, input.Code);

                var entity = ObjectMapper.Map<CreatePeriodsDto, Periods>(input);

                var addedEntity = await _uow.PeriodsRepository.InsertAsync(entity);
                input.Id = addedEntity.Id;
                var log = LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, "Periods", LogType.Insert, addedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();


                return new SuccessDataResult<SelectPeriodsDto>(ObjectMapper.Map<Periods, SelectPeriodsDto>(addedEntity));
            }
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _uow.PeriodsRepository.DeleteAsync(id);
                var log = LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, "Periods", LogType.Delete, id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();
                return new SuccessResult("Silme işlemi başarılı.");
            }
        }

        public async Task<IDataResult<SelectPeriodsDto>> GetAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.PeriodsRepository.GetAsync(t => t.Id == id, t => t.Branches);
                var mappedEntity = ObjectMapper.Map<Periods, SelectPeriodsDto>(entity);
                var log = LogsAppService.InsertLogToDatabase(mappedEntity, mappedEntity, LoginedUserService.UserId, "Periods", LogType.Get, id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();
                return new SuccessDataResult<SelectPeriodsDto>(mappedEntity);
            }
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListPeriodsDto>>> GetListAsync(ListPeriodsParameterDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var list = await _uow.PeriodsRepository.GetListAsync(t => t.IsActive == input.IsActive, t => t.Branches);

                var mappedEntity = ObjectMapper.Map<List<Periods>, List<ListPeriodsDto>>(list.ToList());

                return new SuccessDataResult<IList<ListPeriodsDto>>(mappedEntity);
            }
        }

        [ValidationAspect(typeof(UpdatePeriodsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPeriodsDto>> UpdateAsync(UpdatePeriodsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.PeriodsRepository.GetAsync(x => x.Id == input.Id);

                await _manager.UpdateControl(_uow.PeriodsRepository, input.Code, input.Id, entity);

                var mappedEntity = ObjectMapper.Map<UpdatePeriodsDto, Periods>(input);

                await _uow.PeriodsRepository.UpdateAsync(mappedEntity);
                var before = ObjectMapper.Map<Periods, UpdatePeriodsDto>(entity);
                var log = LogsAppService.InsertLogToDatabase(before, input, LoginedUserService.UserId, "Periods", LogType.Update, mappedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();
                return new SuccessDataResult<SelectPeriodsDto>(ObjectMapper.Map<Periods, SelectPeriodsDto>(mappedEntity));
            }
        }

        public async Task<IDataResult<SelectPeriodsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.PeriodsRepository.GetAsync(x => x.Id == id);

                var updatedEntity = await _uow.PeriodsRepository.LockRow(entity.Id, lockRow, userId);

                await _uow.SaveChanges();

                var mappedEntity = ObjectMapper.Map<Periods, SelectPeriodsDto>(updatedEntity);

                return new SuccessDataResult<SelectPeriodsDto>(mappedEntity);
            }
        }
    }
}

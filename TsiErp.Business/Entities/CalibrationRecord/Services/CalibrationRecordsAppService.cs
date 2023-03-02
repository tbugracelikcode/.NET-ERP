using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using TsiErp.Localizations.Resources.CalibrationRecords.Page;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.CalibrationRecord.BusinessRules;
using TsiErp.Business.Entities.CalibrationRecord.Validations;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.CalibrationRecord;
using TsiErp.Entities.Entities.CalibrationRecord.Dtos;
using Microsoft.Extensions.Localization;

namespace TsiErp.Business.Entities.CalibrationRecord.Services
{
    [ServiceRegistration(typeof(ICalibrationRecordsAppService), DependencyInjectionType.Scoped)]
    public class CalibrationRecordsAppService : ApplicationService<CalibrationRecordsResource> , ICalibrationRecordsAppService
    {
        public CalibrationRecordsAppService(IStringLocalizer<CalibrationRecordsResource> l) : base(l)
        {
        }

        CalibrationRecordsManager _manager { get; set; } = new CalibrationRecordsManager();

        [ValidationAspect(typeof(CreateCalibrationRecordsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectCalibrationRecordsDto>> CreateAsync(CreateCalibrationRecordsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.CodeControl(_uow.CalibrationRecordsRepository, input.Code,L);

                var entity = ObjectMapper.Map<CreateCalibrationRecordsDto, CalibrationRecords>(input);

                var addedEntity = await _uow.CalibrationRecordsRepository.InsertAsync(entity);
                input.Id = addedEntity.Id;
                var log = LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, "CalibrationRecords", LogType.Insert, addedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);

                await _uow.SaveChanges();

                return new SuccessDataResult<SelectCalibrationRecordsDto>(ObjectMapper.Map<CalibrationRecords, SelectCalibrationRecordsDto>(addedEntity));
            }
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.DeleteControl(_uow.CalibrationRecordsRepository, id);
                await _uow.CalibrationRecordsRepository.DeleteAsync(id);

                var log = LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, "CalibrationRecords", LogType.Delete, id);

                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();
                return new SuccessResult(L["DeleteSuccessMessage"]);
            }
        }


        public async Task<IDataResult<SelectCalibrationRecordsDto>> GetAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.CalibrationRecordsRepository.GetAsync(t => t.Id == id, t => t.EquipmentRecords);
                var mappedEntity = ObjectMapper.Map<CalibrationRecords, SelectCalibrationRecordsDto>(entity);
                var log = LogsAppService.InsertLogToDatabase(mappedEntity, mappedEntity, LoginedUserService.UserId, "CalibrationRecords", LogType.Get, id);
                await _uow.LogsRepository.InsertAsync(log);


                await _uow.SaveChanges();

                return new SuccessDataResult<SelectCalibrationRecordsDto>(mappedEntity);
            }
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListCalibrationRecordsDto>>> GetListAsync(ListCalibrationRecordsParameterDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var list = await _uow.CalibrationRecordsRepository.GetListAsync(null, t => t.EquipmentRecords);

                var mappedEntity = ObjectMapper.Map<List<CalibrationRecords>, List<ListCalibrationRecordsDto>>(list.ToList());

                return new SuccessDataResult<IList<ListCalibrationRecordsDto>>(mappedEntity);
            }
        }


        [ValidationAspect(typeof(UpdateCalibrationRecordsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectCalibrationRecordsDto>> UpdateAsync(UpdateCalibrationRecordsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.CalibrationRecordsRepository.GetAsync(x => x.Id == input.Id);

                await _manager.UpdateControl(_uow.CalibrationRecordsRepository, input.Code, input.Id, entity, L);

                var mappedEntity = ObjectMapper.Map<UpdateCalibrationRecordsDto, CalibrationRecords>(input);

                await _uow.CalibrationRecordsRepository.UpdateAsync(mappedEntity);
                var before = ObjectMapper.Map<CalibrationRecords, UpdateCalibrationRecordsDto>(entity);
                var log = LogsAppService.InsertLogToDatabase(before, input, LoginedUserService.UserId, "CalibrationRecords", LogType.Update, mappedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);

                await _uow.SaveChanges();

                return new SuccessDataResult<SelectCalibrationRecordsDto>(ObjectMapper.Map<CalibrationRecords, SelectCalibrationRecordsDto>(mappedEntity));
            }
        }

        public async Task<IDataResult<SelectCalibrationRecordsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.CalibrationRecordsRepository.GetAsync(x => x.Id == id);

                var updatedEntity = await _uow.CalibrationRecordsRepository.LockRow(entity.Id, lockRow, userId);

                await _uow.SaveChanges();

                var mappedEntity = ObjectMapper.Map<CalibrationRecords, SelectCalibrationRecordsDto>(updatedEntity);

                return new SuccessDataResult<SelectCalibrationRecordsDto>(mappedEntity);
            }
        }
    }
}

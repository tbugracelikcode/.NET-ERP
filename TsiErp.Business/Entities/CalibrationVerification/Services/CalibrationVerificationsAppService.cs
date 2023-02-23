using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.CalibrationVerification.BusinessRules;
using TsiErp.Business.Entities.CalibrationVerification.Validations;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.CalibrationVerification;
using TsiErp.Entities.Entities.CalibrationVerification.Dtos;

namespace TsiErp.Business.Entities.CalibrationVerification.Services
{
    [ServiceRegistration(typeof(ICalibrationVerificationsAppService), DependencyInjectionType.Scoped)]
    public class CalibrationVerificationsAppService : ApplicationService, ICalibrationVerificationsAppService
    {
        CalibrationVerificationManager _manager { get; set; } = new CalibrationVerificationManager();

        [ValidationAspect(typeof(CreateCalibrationVerifcationsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectCalibrationVerificationsDto>> CreateAsync(CreateCalibrationVerificationsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.CodeControl(_uow.CalibrationVerificationsRepository, input.Code);

                var entity = ObjectMapper.Map<CreateCalibrationVerificationsDto, CalibrationVerifications>(input);

                var addedEntity = await _uow.CalibrationVerificationsRepository.InsertAsync(entity);
                input.Id = addedEntity.Id;
                var log = LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, "CalibrationVerifications", LogType.Insert, addedEntity.Id);

                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectCalibrationVerificationsDto>(ObjectMapper.Map<CalibrationVerifications, SelectCalibrationVerificationsDto>(addedEntity));
            }
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _uow.CalibrationVerificationsRepository.DeleteAsync(id);
                var log = LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, "CalibrationVerifications", LogType.Delete, id);
                await _uow.LogsRepository.InsertAsync(log);

                await _uow.SaveChanges();
                return new SuccessResult("Silme işlemi başarılı.");
            }
        }


        public async Task<IDataResult<SelectCalibrationVerificationsDto>> GetAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.CalibrationVerificationsRepository.GetAsync(t => t.Id == id, t => t.EquipmentRecords);
                var mappedEntity = ObjectMapper.Map<CalibrationVerifications, SelectCalibrationVerificationsDto>(entity);
                var log = LogsAppService.InsertLogToDatabase(mappedEntity, mappedEntity, LoginedUserService.UserId, "CalibrationVerifications", LogType.Get, id);
                await _uow.LogsRepository.InsertAsync(log);


                await _uow.SaveChanges();
                return new SuccessDataResult<SelectCalibrationVerificationsDto>(mappedEntity);
            }
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListCalibrationVerificationsDto>>> GetListAsync(ListCalibrationVerificationsParameterDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var list = await _uow.CalibrationVerificationsRepository.GetListAsync(null, t => t.EquipmentRecords);

                var mappedEntity = ObjectMapper.Map<List<CalibrationVerifications>, List<ListCalibrationVerificationsDto>>(list.ToList());

                return new SuccessDataResult<IList<ListCalibrationVerificationsDto>>(mappedEntity);
            }
        }


        [ValidationAspect(typeof(UpdateCalibrationVerificationsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectCalibrationVerificationsDto>> UpdateAsync(UpdateCalibrationVerificationsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.CalibrationVerificationsRepository.GetAsync(x => x.Id == input.Id);

                await _manager.UpdateControl(_uow.CalibrationVerificationsRepository, input.Code, input.Id, entity);

                var mappedEntity = ObjectMapper.Map<UpdateCalibrationVerificationsDto, CalibrationVerifications>(input);

                await _uow.CalibrationVerificationsRepository.UpdateAsync(mappedEntity);
                var before = ObjectMapper.Map<CalibrationVerifications, UpdateCalibrationVerificationsDto>(entity);
                var log = LogsAppService.InsertLogToDatabase(before, input, LoginedUserService.UserId, "CalibrationVerifications", LogType.Update, mappedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);

                await _uow.SaveChanges();

                return new SuccessDataResult<SelectCalibrationVerificationsDto>(ObjectMapper.Map<CalibrationVerifications, SelectCalibrationVerificationsDto>(mappedEntity));
            }
        }

        public async Task<IDataResult<SelectCalibrationVerificationsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.CalibrationVerificationsRepository.GetAsync(x => x.Id == id);

                var updatedEntity = await _uow.CalibrationVerificationsRepository.LockRow(entity.Id, lockRow, userId);

                await _uow.SaveChanges();

                var mappedEntity = ObjectMapper.Map<CalibrationVerifications, SelectCalibrationVerificationsDto>(updatedEntity);

                return new SuccessDataResult<SelectCalibrationVerificationsDto>(mappedEntity);
            }
        }
    }
}

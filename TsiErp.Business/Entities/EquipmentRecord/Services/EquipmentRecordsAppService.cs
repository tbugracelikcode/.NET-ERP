using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results; using TsiErp.Localizations.Resources.Branches.Page;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.EquipmentRecord.BusinessRules;
using TsiErp.Business.Entities.EquipmentRecord.Validations;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.EquipmentRecord;
using TsiErp.Entities.Entities.EquipmentRecord.Dtos;
using Microsoft.Extensions.Localization;

namespace TsiErp.Business.Entities.EquipmentRecord.Services
{
    [ServiceRegistration(typeof(IEquipmentRecordsAppService), DependencyInjectionType.Scoped)]
    public class EquipmentRecordsAppService : ApplicationService<BranchesResource>, IEquipmentRecordsAppService
    {
        public EquipmentRecordsAppService(IStringLocalizer<BranchesResource> l) : base(l)
        {
        }

        EquipmentRecordManager _manager { get; set; } = new EquipmentRecordManager();


        [ValidationAspect(typeof(CreateEquipmentRecorsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectEquipmentRecordsDto>> CreateAsync(CreateEquipmentRecordsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.CodeControl(_uow.EquipmentRecordsRepository, input.Code);

                var entity = ObjectMapper.Map<CreateEquipmentRecordsDto, EquipmentRecords>(input);

                var addedEntity = await _uow.EquipmentRecordsRepository.InsertAsync(entity);
                input.Id = addedEntity.Id;
                var log = LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, "EquipmentRecords", LogType.Insert, addedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectEquipmentRecordsDto>(ObjectMapper.Map<EquipmentRecords, SelectEquipmentRecordsDto>(addedEntity));
            }
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.DeleteControl(_uow.EquipmentRecordsRepository, id);
                await _uow.EquipmentRecordsRepository.DeleteAsync(id);
                var log = LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, "EquipmentRecords", LogType.Delete, id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();
                return new SuccessResult("Silme işlemi başarılı.");
            }
        }


        public async Task<IDataResult<SelectEquipmentRecordsDto>> GetAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.EquipmentRecordsRepository.GetAsync(t => t.Id == id, t => t.Departments, t => t.CalibrationRecords, t => t.CalibrationVerifications);
                var mappedEntity = ObjectMapper.Map<EquipmentRecords, SelectEquipmentRecordsDto>(entity);
                var log = LogsAppService.InsertLogToDatabase(mappedEntity, mappedEntity, LoginedUserService.UserId, "EquipmentRecords", LogType.Get, id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectEquipmentRecordsDto>(mappedEntity);
            }
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListEquipmentRecordsDto>>> GetListAsync(ListEquipmentRecordsParameterDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var list = await _uow.EquipmentRecordsRepository.GetListAsync(t => t.IsActive == input.IsActive, t => t.Departments, t => t.CalibrationRecords, t => t.CalibrationVerifications);

                var mappedEntity = ObjectMapper.Map<List<EquipmentRecords>, List<ListEquipmentRecordsDto>>(list.ToList());

                return new SuccessDataResult<IList<ListEquipmentRecordsDto>>(mappedEntity);
            }
        }


        [ValidationAspect(typeof(UpdateEquipmentRecordsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectEquipmentRecordsDto>> UpdateAsync(UpdateEquipmentRecordsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.EquipmentRecordsRepository.GetAsync(x => x.Id == input.Id);

                await _manager.UpdateControl(_uow.EquipmentRecordsRepository, input.Code, input.Id, entity);

                var mappedEntity = ObjectMapper.Map<UpdateEquipmentRecordsDto, EquipmentRecords>(input);

                await _uow.EquipmentRecordsRepository.UpdateAsync(mappedEntity);
                var before = ObjectMapper.Map<EquipmentRecords, UpdateEquipmentRecordsDto>(entity);
                var log = LogsAppService.InsertLogToDatabase(before, input, LoginedUserService.UserId, "EquipmentRecords", LogType.Update, mappedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectEquipmentRecordsDto>(ObjectMapper.Map<EquipmentRecords, SelectEquipmentRecordsDto>(mappedEntity));
            }
        }

        public async Task<IDataResult<SelectEquipmentRecordsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.EquipmentRecordsRepository.GetAsync(x => x.Id == id);

                var updatedEntity = await _uow.EquipmentRecordsRepository.LockRow(entity.Id, lockRow, userId);

                await _uow.SaveChanges();

                var mappedEntity = ObjectMapper.Map<EquipmentRecords, SelectEquipmentRecordsDto>(updatedEntity);

                return new SuccessDataResult<SelectEquipmentRecordsDto>(mappedEntity);
            }
        }
    }
}

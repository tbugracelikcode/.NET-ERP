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
using TsiErp.Business.Entities.Branch.Services;
using TsiErp.Business.Entities.Branch.Validations;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Branch;
using TsiErp.Entities.Entities.Branch.Dtos;
using TsiErp.Entities.Entities.Branch;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.CalibrationRecord;
using TsiErp.Business.Entities.CalibrationRecord.Validations;
using TsiErp.Entities.Entities.CalibrationRecord.Dtos;
using TsiErp.Entities.Entities.CalibrationRecord;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Business.Entities.CalibrationRecord.BusinessRules;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.Entities.Entities.WorkOrder.Dtos;
using TsiErp.Entities.Entities.WorkOrder;

namespace TsiErp.Business.Entities.CalibrationRecord.Services
{
    [ServiceRegistration(typeof(ICalibrationRecordsAppService), DependencyInjectionType.Scoped)]
    public class CalibrationRecordsAppService : ApplicationService , ICalibrationRecordsAppService
    {
        CalibrationRecordsManager _manager { get; set; } = new CalibrationRecordsManager();

        [ValidationAspect(typeof(CreateCalibrationRecordsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectCalibrationRecordsDto>> CreateAsync(CreateCalibrationRecordsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.CodeControl(_uow.CalibrationRecordsRepository, input.Code);

                var entity = ObjectMapper.Map<CreateCalibrationRecordsDto, CalibrationRecords>(input);

                var addedEntity = await _uow.CalibrationRecordsRepository.InsertAsync(entity);

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
                await _uow.SaveChanges();
                return new SuccessResult("Silme işlemi başarılı.");
            }
        }


        public async Task<IDataResult<SelectCalibrationRecordsDto>> GetAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.CalibrationRecordsRepository.GetAsync(t => t.Id == id, t => t.EquipmentRecords);
                var mappedEntity = ObjectMapper.Map<CalibrationRecords, SelectCalibrationRecordsDto>(entity);
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

                await _manager.UpdateControl(_uow.CalibrationRecordsRepository, input.Code, input.Id, entity);

                var mappedEntity = ObjectMapper.Map<UpdateCalibrationRecordsDto, CalibrationRecords>(input);

                await _uow.CalibrationRecordsRepository.UpdateAsync(mappedEntity);
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

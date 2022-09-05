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

namespace TsiErp.Business.Entities.CalibrationRecord.Services
{
    [ServiceRegistration(typeof(ICalibrationRecordsAppService), DependencyInjectionType.Scoped)]
    public class CalibrationRecordsAppService : ApplicationService , ICalibrationRecordsAppService
    {
        private readonly ICalibrationRecordsRepository _repository;

        public CalibrationRecordsAppService(ICalibrationRecordsRepository repository)
        {
            _repository = repository;
        }


        [ValidationAspect(typeof(CreateCalibrationRecordsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectCalibrationRecordsDto>> CreateAsync(CreateCalibrationRecordsDto input)
        {
            var entity = ObjectMapper.Map<CreateCalibrationRecordsDto, CalibrationRecords>(input);

            var addedEntity = await _repository.InsertAsync(entity);

            return new SuccessDataResult<SelectCalibrationRecordsDto>(ObjectMapper.Map<CalibrationRecords, SelectCalibrationRecordsDto>(addedEntity));
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
            return new SuccessResult("Silme işlemi başarılı.");
        }


        public async Task<IDataResult<SelectCalibrationRecordsDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id, t => t.EquipmentRecords);
            var mappedEntity = ObjectMapper.Map<CalibrationRecords, SelectCalibrationRecordsDto>(entity);
            return new SuccessDataResult<SelectCalibrationRecordsDto>(mappedEntity);
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListCalibrationRecordsDto>>> GetListAsync(ListCalibrationRecordsParameterDto input)
        {
            var list = await _repository.GetListAsync(null, t => t.EquipmentRecords);

            var mappedEntity = ObjectMapper.Map<List<CalibrationRecords>, List<ListCalibrationRecordsDto>>(list.ToList());

            return new SuccessDataResult<IList<ListCalibrationRecordsDto>>(mappedEntity);
        }


        [ValidationAspect(typeof(UpdateCalibrationRecordsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectCalibrationRecordsDto>> UpdateAsync(UpdateCalibrationRecordsDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            var mappedEntity = ObjectMapper.Map<UpdateCalibrationRecordsDto, CalibrationRecords>(input);

            await _repository.UpdateAsync(mappedEntity);

            return new SuccessDataResult<SelectCalibrationRecordsDto>(ObjectMapper.Map<CalibrationRecords, SelectCalibrationRecordsDto>(mappedEntity));
        }
    }
}

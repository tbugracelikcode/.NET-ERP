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
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.EquipmentRecord;
using TsiErp.Business.Entities.EquipmentRecord.Validations;
using TsiErp.Entities.Entities.EquipmentRecord.Dtos;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Entities.Entities.EquipmentRecord;
using TsiErp.Business.Entities.EquipmentRecord.BusinessRules;

namespace TsiErp.Business.Entities.EquipmentRecord.Services
{
    [ServiceRegistration(typeof(IEquipmentRecordsAppService), DependencyInjectionType.Scoped)]
    public class EquipmentRecordsAppService : ApplicationService, IEquipmentRecordsAppService
    {
        private readonly IEquipmentRecordsRepository _repository;

        EquipmentRecordManager _manager { get; set; } = new EquipmentRecordManager();

        public EquipmentRecordsAppService(IEquipmentRecordsRepository repository)
        {
            _repository = repository;
        }


        [ValidationAspect(typeof(CreateEquipmentRecorsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectEquipmentRecordsDto>> CreateAsync(CreateEquipmentRecordsDto input)
        {
            await _manager.CodeControl(_repository, input.Code);

            var entity = ObjectMapper.Map<CreateEquipmentRecordsDto, EquipmentRecords>(input);

            var addedEntity = await _repository.InsertAsync(entity);
            await _repository.SaveChanges();

            return new SuccessDataResult<SelectEquipmentRecordsDto>(ObjectMapper.Map<EquipmentRecords, SelectEquipmentRecordsDto>(addedEntity));
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            await _manager.DeleteControl(_repository, id);
            await _repository.DeleteAsync(id);
            await _repository.SaveChanges();
            return new SuccessResult("Silme işlemi başarılı.");
        }


        public async Task<IDataResult<SelectEquipmentRecordsDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id, t => t.Departments, t => t.CalibrationRecords, t => t.CalibrationVerifications);
            var mappedEntity = ObjectMapper.Map<EquipmentRecords, SelectEquipmentRecordsDto>(entity);
            return new SuccessDataResult<SelectEquipmentRecordsDto>(mappedEntity);
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListEquipmentRecordsDto>>> GetListAsync(ListEquipmentRecordsParameterDto input)
        {
            var list = await _repository.GetListAsync(t => t.IsActive == input.IsActive, t => t.Departments, t => t.CalibrationRecords, t => t.CalibrationVerifications);

            var mappedEntity = ObjectMapper.Map<List<EquipmentRecords>, List<ListEquipmentRecordsDto>>(list.ToList());

            return new SuccessDataResult<IList<ListEquipmentRecordsDto>>(mappedEntity);
        }


        [ValidationAspect(typeof(UpdateEquipmentRecordsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectEquipmentRecordsDto>> UpdateAsync(UpdateEquipmentRecordsDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            await _manager.UpdateControl(_repository, input.Code, input.Id, entity);

            var mappedEntity = ObjectMapper.Map<UpdateEquipmentRecordsDto, EquipmentRecords>(input);

            await _repository.UpdateAsync(mappedEntity);
            await _repository.SaveChanges();

            return new SuccessDataResult<SelectEquipmentRecordsDto>(ObjectMapper.Map<EquipmentRecords, SelectEquipmentRecordsDto>(mappedEntity));
        }
    }
}

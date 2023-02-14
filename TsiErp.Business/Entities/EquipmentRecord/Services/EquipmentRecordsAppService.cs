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
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;

namespace TsiErp.Business.Entities.EquipmentRecord.Services
{
    [ServiceRegistration(typeof(IEquipmentRecordsAppService), DependencyInjectionType.Scoped)]
    public class EquipmentRecordsAppService : ApplicationService, IEquipmentRecordsAppService
    {
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
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectEquipmentRecordsDto>(ObjectMapper.Map<EquipmentRecords, SelectEquipmentRecordsDto>(mappedEntity));
            }
        }
    }
}

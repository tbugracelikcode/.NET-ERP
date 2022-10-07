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
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.CalibrationVerification;
using TsiErp.Business.Entities.CalibrationVerification.Validations;
using TsiErp.Entities.Entities.CalibrationVerification.Dtos;
using TsiErp.Entities.Entities.CalibrationVerification;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Business.Entities.CalibrationVerification.BusinessRules;

namespace TsiErp.Business.Entities.CalibrationVerification.Services
{
    [ServiceRegistration(typeof(ICalibrationVerificationsAppService), DependencyInjectionType.Scoped)]
    public class CalibrationVerificationsAppService : ApplicationService, ICalibrationVerificationsAppService
    {
        private readonly ICalibrationVerificationsRepository _repository;

        CalibrationVerificationManager _manager { get; set; } = new CalibrationVerificationManager();

        public CalibrationVerificationsAppService(ICalibrationVerificationsRepository repository)
        {
            _repository = repository;
        }


        [ValidationAspect(typeof(CreateCalibrationVerifcationsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectCalibrationVerificationsDto>> CreateAsync(CreateCalibrationVerificationsDto input)
        {

            await _manager.CodeControl(_repository, input.Code);

            var entity = ObjectMapper.Map<CreateCalibrationVerificationsDto, CalibrationVerifications>(input);

            var addedEntity = await _repository.InsertAsync(entity);

            return new SuccessDataResult<SelectCalibrationVerificationsDto>(ObjectMapper.Map<CalibrationVerifications, SelectCalibrationVerificationsDto>(addedEntity));
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
            return new SuccessResult("Silme işlemi başarılı.");
        }


        public async Task<IDataResult<SelectCalibrationVerificationsDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id, t => t.EquipmentRecords);
            var mappedEntity = ObjectMapper.Map<CalibrationVerifications, SelectCalibrationVerificationsDto>(entity);
            return new SuccessDataResult<SelectCalibrationVerificationsDto>(mappedEntity);
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListCalibrationVerificationsDto>>> GetListAsync(ListCalibrationVerificationsParameterDto input)
        {
            var list = await _repository.GetListAsync(null, t => t.EquipmentRecords);

            var mappedEntity = ObjectMapper.Map<List<CalibrationVerifications>, List<ListCalibrationVerificationsDto>>(list.ToList());

            return new SuccessDataResult<IList<ListCalibrationVerificationsDto>>(mappedEntity);
        }


        [ValidationAspect(typeof(UpdateCalibrationVerificationsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectCalibrationVerificationsDto>> UpdateAsync(UpdateCalibrationVerificationsDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            await _manager.UpdateControl(_repository, input.Code, input.Id, entity);

            var mappedEntity = ObjectMapper.Map<UpdateCalibrationVerificationsDto, CalibrationVerifications>(input);

            await _repository.UpdateAsync(mappedEntity);

            return new SuccessDataResult<SelectCalibrationVerificationsDto>(ObjectMapper.Map<CalibrationVerifications, SelectCalibrationVerificationsDto>(mappedEntity));
        }
    }
}

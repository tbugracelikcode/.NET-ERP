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
using TsiErp.Business.Entities.Operation.Validations;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Operation;
using TsiErp.Entities.Entities.Operation;
using TsiErp.Entities.Entities.Operation.Dtos;

namespace TsiErp.Business.Entities.Operation.Services
{
    [ServiceRegistration(typeof(IOperationsAppService), DependencyInjectionType.Scoped)]
    public class OperationsAppService : IOperationsAppService
    {

        private readonly IOperationsRepository _repository;

        public OperationsAppService(IOperationsRepository repository)
        {
            _repository = repository;
        }


        [ValidationAspect(typeof(CreateOperationsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectOperationsDto>> CreateAsync(CreateOperationsDto input)
        {
            var entity = ObjectMapper.Map<CreateOperationsDto, Operations>(input);

            var addedEntity = await _repository.InsertAsync(entity);

            return new SuccessDataResult<SelectOperationsDto>(ObjectMapper.Map<Operations, SelectOperationsDto>(addedEntity));
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
            return new SuccessResult("Silme işlemi başarılı.");
        }

        public async Task<IDataResult<SelectOperationsDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id);
            var mappedEntity = ObjectMapper.Map<Operations, SelectOperationsDto>(entity);
            return new SuccessDataResult<SelectOperationsDto>(mappedEntity);
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListOperationsDto>>> GetListAsync(ListOperationsParameterDto input)
        {
            var list = await _repository.GetListAsync(t => t.IsActive == input.IsActive);

            var mappedEntity = ObjectMapper.Map<List<Operations>, List<ListOperationsDto>>(list.ToList());

            return new SuccessDataResult<IList<ListOperationsDto>>(mappedEntity);
        }

        [ValidationAspect(typeof(UpdateOperationsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectOperationsDto>> UpdateAsync(UpdateOperationsDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            var mappedEntity = ObjectMapper.Map<UpdateOperationsDto, Operations>(input);

            await _repository.UpdateAsync(mappedEntity);
            return new SuccessDataResult<SelectOperationsDto>(ObjectMapper.Map<Operations, SelectOperationsDto>(mappedEntity));
        }
    }
}

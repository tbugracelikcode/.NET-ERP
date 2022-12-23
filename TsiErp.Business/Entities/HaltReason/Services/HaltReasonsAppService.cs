using AutoMapper.Internal.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.Entities.HaltReason.BusinessRules;
using TsiErp.Business.Entities.HaltReason.Validations;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.HaltReason;
using TsiErp.Entities.Entities.HaltReason;
using TsiErp.Entities.Entities.HaltReason.Dtos;

namespace TsiErp.Business.Entities.HaltReason.Services
{
    [ServiceRegistration(typeof(IHaltReasonsAppService), DependencyInjectionType.Scoped)]
    public class HaltReasonsAppService : IHaltReasonsAppService
    {
        private readonly IHaltReasonsRepository _repository;

        HaltReasonManager _manager { get; set; } = new HaltReasonManager();

        public HaltReasonsAppService(IHaltReasonsRepository repository)
        {
            _repository = repository;
        }


        [ValidationAspect(typeof(CreateHaltReasonsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectHaltReasonsDto>> CreateAsync(CreateHaltReasonsDto input)
        {
            await _manager.CodeControl(_repository, input.Code);

            var entity = ObjectMapper.Map<CreateHaltReasonsDto, HaltReasons>(input);

            var addedEntity = await _repository.InsertAsync(entity);
            await _repository.SaveChanges();

            return new SuccessDataResult<SelectHaltReasonsDto>(ObjectMapper.Map<HaltReasons, SelectHaltReasonsDto>(addedEntity));
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
            await _repository.SaveChanges();
            return new SuccessResult("Silme işlemi başarılı.");
        }

        public async Task<IDataResult<SelectHaltReasonsDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id);
            var mappedEntity = ObjectMapper.Map<HaltReasons, SelectHaltReasonsDto>(entity);
            return new SuccessDataResult<SelectHaltReasonsDto>(mappedEntity);
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListHaltReasonsDto>>> GetListAsync(ListHaltReasonsParameterDto input)
        {
            var list = await _repository.GetListAsync();

            var mappedEntity = ObjectMapper.Map<List<HaltReasons>, List<ListHaltReasonsDto>>(list.ToList());

            return new SuccessDataResult<IList<ListHaltReasonsDto>>(mappedEntity);
        }

        [ValidationAspect(typeof(UpdateHaltReasonsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectHaltReasonsDto>> UpdateAsync(UpdateHaltReasonsDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            await _manager.UpdateControl(_repository, input.Code, input.Id, entity);

            var mappedEntity = ObjectMapper.Map<UpdateHaltReasonsDto, HaltReasons>(input);

            await _repository.UpdateAsync(mappedEntity);
            await _repository.SaveChanges();
            return new SuccessDataResult<SelectHaltReasonsDto>(ObjectMapper.Map<HaltReasons, SelectHaltReasonsDto>(mappedEntity));
        }
    }
}

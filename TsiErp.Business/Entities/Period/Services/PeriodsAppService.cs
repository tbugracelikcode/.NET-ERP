using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using Tsi.Guids;
using Tsi.IoC.Tsi.DependencyResolvers;
using Tsi.Results;
using Tsi.Validation.Validations.FluentValidation.Aspect;
using TsiErp.Business.Entities.Period.Validations;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Period;
using TsiErp.Entities.Entities.Period;
using TsiErp.Entities.Entities.Period.Dtos;

namespace TsiErp.Business.Entities.Period.Services
{
    [ServiceRegistration(typeof(IPeriodsAppService), DependencyInjectionType.Scoped)]
    public class PeriodsAppService : ApplicationService,IPeriodsAppService
    {
        private readonly IPeriodsRepository _repository;

        public PeriodsAppService(IPeriodsRepository repository)
        {
            _repository = repository;
        }

        [ValidationAspect(typeof(CreatePeriodsValidator), Priority = 1)]
        public async Task<IDataResult<SelectPeriodsDto>> CreateAsync(CreatePeriodsDto input)
        {
            var entity = ObjectMapper.Map<CreatePeriodsDto, Periods>(input);

            entity.Id = GuidGenerator.CreateGuid();
            entity.CreatorId = Guid.NewGuid();
            entity.CreationTime = DateTime.Now;
            entity.IsDeleted = false;
            entity.DeleterId = null;
            entity.DeletionTime = null;
            entity.LastModifierId = null;
            entity.LastModificationTime = null;

            var addedEntity = await _repository.InsertAsync(entity);

            return new SuccessDataResult<SelectPeriodsDto>(ObjectMapper.Map<Periods, SelectPeriodsDto>(addedEntity));
        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
            return new SuccessResult("Silme işlemi başarılı.");
        }

        public async Task<IDataResult<SelectPeriodsDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id,t=>t.Branches);
            var mappedEntity = ObjectMapper.Map<Periods, SelectPeriodsDto>(entity);
            return new SuccessDataResult<SelectPeriodsDto>(mappedEntity);
        }

        public async Task<IDataResult<IList<ListPeriodsDto>>> GetListAsync()
        {
            var list = await _repository.GetListAsync();

            var mappedEntity = ObjectMapper.Map<List<Periods>, List<ListPeriodsDto>>(list.ToList());

            return new SuccessDataResult<IList<ListPeriodsDto>>(mappedEntity);
        }

        [ValidationAspect(typeof(UpdatePeriodsValidator), Priority = 1)]
        public async Task<IDataResult<SelectPeriodsDto>> UpdateAsync(UpdatePeriodsDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            var mappedEntity = ObjectMapper.Map<UpdatePeriodsDto, Periods>(input);

            mappedEntity.Id = input.Id;
            mappedEntity.LastModifierId = Guid.NewGuid();
            mappedEntity.LastModificationTime = DateTime.Now;
            mappedEntity.CreatorId = entity.CreatorId;
            mappedEntity.CreationTime = entity.CreationTime;
            mappedEntity.IsDeleted = false;
            mappedEntity.DeleterId = null;
            mappedEntity.DeletionTime = null;

            await _repository.UpdateAsync(mappedEntity);
            return new SuccessDataResult<SelectPeriodsDto>(ObjectMapper.Map<Periods, SelectPeriodsDto>(mappedEntity));
        }
    }
}

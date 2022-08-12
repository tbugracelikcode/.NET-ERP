using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Guids;
using Tsi.Validation.Validations.FluentValidation.CrossCuttingConcerns;
using TsiErp.Business.Entities.Period.Validations;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Period;
using TsiErp.Entities.Entities.Period;
using TsiErp.Entities.Entities.Period.Dtos;

namespace TsiErp.Business.Entities.Period.Services
{
    public class PeriodsAppService : IPeriodsAppService
    {
        private readonly IPeriodsRepository _repository;

        private readonly IGuidGenerator _guidGenerator;

        public PeriodsAppService(IPeriodsRepository repository, IGuidGenerator guidGenerator)
        {
            _repository = repository;
            _guidGenerator = guidGenerator;
        }

        [ValidationAspect(typeof(CreatePeriodsValidator), Priority = 1)]
        public async Task<SelectPeriodsDto> CreateAsync(CreatePeriodsDto input)
        {
            var entity = ObjectMapper.Map<CreatePeriodsDto, Periods>(input);

            entity.Id = _guidGenerator.CreateGuid();
            entity.CreatorId = Guid.NewGuid();
            entity.CreationTime = DateTime.Now;
            entity.IsDeleted = false;
            entity.DeleterId = null;
            entity.DeletionTime = null;
            entity.LastModifierId = null;
            entity.LastModificationTime = null;

            var addedEntity = await _repository.InsertAsync(entity);

            return ObjectMapper.Map<Periods, SelectPeriodsDto>(addedEntity);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<SelectPeriodsDto> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id,t=>t.Branches);
            var mappedEntity = ObjectMapper.Map<Periods, SelectPeriodsDto>(entity);
            return mappedEntity;
        }

        public async Task<IList<ListPeriodsDto>> GetListAsync()
        {
            var list = await _repository.GetListAsync();

            var mappedEntity = ObjectMapper.Map<List<Periods>, List<ListPeriodsDto>>(list.ToList());

            return mappedEntity;
        }

        [ValidationAspect(typeof(UpdatePeriodsValidator), Priority = 1)]
        public async Task<SelectPeriodsDto> UpdateAsync(UpdatePeriodsDto input)
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
            return ObjectMapper.Map<Periods, SelectPeriodsDto>(mappedEntity);
        }
    }
}

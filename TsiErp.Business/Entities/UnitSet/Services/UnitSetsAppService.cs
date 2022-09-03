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
using TsiErp.Business.Entities.UnitSet.Validations;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.UnitSet;
using TsiErp.Entities.Entities.Branch.Dtos;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Entities.Entities.UnitSet.Dtos;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Entities.Entities.UnitSet;

namespace TsiErp.Business.Entities.UnitSet.Services
{
    [ServiceRegistration(typeof(IUnitSetsAppService), DependencyInjectionType.Scoped)]
    public class UnitSetsAppService : ApplicationService, IUnitSetsAppService
    {
        private readonly IUnitSetsRepository _repository;

        public UnitSetsAppService(IUnitSetsRepository repository)
        {
            _repository = repository;
        }

        [ValidationAspect(typeof(CreateUnitSetsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectUnitSetsDto>> CreateAsync(CreateUnitSetsDto input)
        {
            var entity = ObjectMapper.Map<CreateUnitSetsDto, UnitSets>(input);

            var addedEntity = await _repository.InsertAsync(entity);

            return new SuccessDataResult<SelectUnitSetsDto>(ObjectMapper.Map<UnitSets, SelectUnitSetsDto>(addedEntity));
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
            return new SuccessResult("Silme işlemi başarılı.");
        }

        public async Task<IDataResult<SelectUnitSetsDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id);
            var mappedEntity = ObjectMapper.Map<UnitSets, SelectUnitSetsDto>(entity);
            return new SuccessDataResult<SelectUnitSetsDto>(mappedEntity);
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListUnitSetsDto>>> GetListAsync()
        {
            var list = await _repository.GetListAsync(null);

            var mappedEntity = ObjectMapper.Map<List<UnitSets>, List<ListUnitSetsDto>>(list.ToList());

            return new SuccessDataResult<IList<ListUnitSetsDto>>(mappedEntity);
        }


        [ValidationAspect(typeof(UpdateUnitSetsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectUnitSetsDto>> UpdateAsync(UpdateUnitSetsDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            var mappedEntity = ObjectMapper.Map<UpdateUnitSetsDto, UnitSets>(input);

            await _repository.UpdateAsync(mappedEntity);

            return new SuccessDataResult<SelectUnitSetsDto>(ObjectMapper.Map<UnitSets, SelectUnitSetsDto>(mappedEntity));
        }
    }
}

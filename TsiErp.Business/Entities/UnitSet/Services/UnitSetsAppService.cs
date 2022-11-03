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
using TsiErp.Business.Entities.UnitSet.BusinessRules;

namespace TsiErp.Business.Entities.UnitSet.Services
{
    [ServiceRegistration(typeof(IUnitSetsAppService), DependencyInjectionType.Scoped)]
    public class UnitSetsAppService : ApplicationService, IUnitSetsAppService
    {
        private readonly IUnitSetsRepository _repository;

        UnitSetManager _manager { get; set; } = new UnitSetManager();

        public UnitSetsAppService(IUnitSetsRepository repository)
        {
            _repository = repository;
        }

        [ValidationAspect(typeof(CreateUnitSetsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectUnitSetsDto>> CreateAsync(CreateUnitSetsDto input)
        {
            await _manager.CodeControl(_repository, input.Code);

            var entity = ObjectMapper.Map<CreateUnitSetsDto, UnitSets>(input);

            var addedEntity = await _repository.InsertAsync(entity);
            await _repository.SaveChanges();

            return new SuccessDataResult<SelectUnitSetsDto>(ObjectMapper.Map<UnitSets, SelectUnitSetsDto>(addedEntity));
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            await _manager.DeleteControl(_repository, id);
            await _repository.DeleteAsync(id);
            await _repository.SaveChanges();
            return new SuccessResult("Silme işlemi başarılı.");
        }

        public async Task<IDataResult<SelectUnitSetsDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id, t => t.Products, t => t.SalesPropositionLines);
            var mappedEntity = ObjectMapper.Map<UnitSets, SelectUnitSetsDto>(entity);
            return new SuccessDataResult<SelectUnitSetsDto>(mappedEntity);
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListUnitSetsDto>>> GetListAsync(ListUnitSetsParameterDto input)
        {
            var list = await _repository.GetListAsync(t => t.IsActive == input.IsActive, t => t.Products, t => t.SalesPropositionLines);

            var mappedEntity = ObjectMapper.Map<List<UnitSets>, List<ListUnitSetsDto>>(list.ToList());

            return new SuccessDataResult<IList<ListUnitSetsDto>>(mappedEntity);
        }


        [ValidationAspect(typeof(UpdateUnitSetsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectUnitSetsDto>> UpdateAsync(UpdateUnitSetsDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            await _manager.UpdateControl(_repository, input.Code, input.Id, entity);

            var mappedEntity = ObjectMapper.Map<UpdateUnitSetsDto, UnitSets>(input);

            await _repository.UpdateAsync(mappedEntity);
            await _repository.SaveChanges();

            return new SuccessDataResult<SelectUnitSetsDto>(ObjectMapper.Map<UnitSets, SelectUnitSetsDto>(mappedEntity));
        }
    }
}

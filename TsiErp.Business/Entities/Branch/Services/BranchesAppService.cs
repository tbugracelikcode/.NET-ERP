using System.Reflection;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.DependencyResolvers.Autofac;
using TsiErp.Business.Entities.Branch.Validations;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Branch;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Entities.Entities.Branch.Dtos;

namespace TsiErp.Business.Entities.Branch.Services
{
    [ServiceRegistration(typeof(IBranchesAppService), DependencyInjectionType.Scoped)]
    public class BranchesAppService : ApplicationService, IBranchesAppService
    {
        private readonly IBranchesRepository _repository;

        public BranchesAppService(IBranchesRepository repository)
        {
            _repository = repository;
        }


        [ValidationAspect(typeof(CreateBranchesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectBranchesDto>> CreateAsync(CreateBranchesDto input)
        {
            var entity = ObjectMapper.Map<CreateBranchesDto, Branches>(input);

            var addedEntity = await _repository.InsertAsync(entity);

            return new SuccessDataResult<SelectBranchesDto>(ObjectMapper.Map<Branches, SelectBranchesDto>(addedEntity));
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
            return new SuccessResult("Silme işlemi başarılı.");
        }


        public async Task<IDataResult<SelectBranchesDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id,  t => t.Periods);
            var mappedEntity = ObjectMapper.Map<Branches, SelectBranchesDto>(entity);
            return new SuccessDataResult<SelectBranchesDto>(mappedEntity);
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListBranchesDto>>> GetListAsync(ListBranchesParameterDto input)
        {
            var list = await _repository.GetListAsync(t=>t.IsActive == input.IsActive,  t => t.Periods);

            var mappedEntity = ObjectMapper.Map<List<Branches>, List<ListBranchesDto>>(list.ToList());

            return new SuccessDataResult<IList<ListBranchesDto>>(mappedEntity);
        }


        [ValidationAspect(typeof(UpdateBranchesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectBranchesDto>> UpdateAsync(UpdateBranchesDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            var mappedEntity = ObjectMapper.Map<UpdateBranchesDto, Branches>(input);

            await _repository.UpdateAsync(mappedEntity);

            return new SuccessDataResult<SelectBranchesDto>(ObjectMapper.Map<Branches, SelectBranchesDto>(mappedEntity));
        }

    }
}

using System.Reflection;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using Tsi.EntityFrameworkCore.Respositories.UnitOfWork;
using TsiErp.Business.DependencyResolvers.Autofac;
using TsiErp.Business.Entities.Branch.BusinessRules;
using TsiErp.Business.Entities.Branch.Validations;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Branch;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Entities.Entities.Branch.Dtos;

namespace TsiErp.Business.Entities.Branch.Services
{
    [ServiceRegistration(typeof(IBranchesAppService), DependencyInjectionType.Scoped)]
    public class BranchesAppService : ApplicationService, IBranchesAppService
    {
        //private readonly IBranchesRepository _repository;

        BranchesManager _manager { get; set; } = new BranchesManager();

        UnitOfWork _uow { get; set; } = new UnitOfWork();

        public BranchesAppService()
        {
        }


        [ValidationAspect(typeof(CreateBranchesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectBranchesDto>> CreateAsync(CreateBranchesDto input)
        {
            await _manager.CodeControl(_uow.BranchRepository, input.Code);

            var entity = ObjectMapper.Map<CreateBranchesDto, Branches>(input);

            var addedEntity = await _uow.BranchRepository.InsertAsync(entity);

            await _uow.SaveChanges();

            return new SuccessDataResult<SelectBranchesDto>(ObjectMapper.Map<Branches, SelectBranchesDto>(addedEntity));
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            await _manager.DeleteControl(_uow.BranchRepository, id);
            await _uow.BranchRepository.DeleteAsync(id);
            await _uow.SaveChanges();
            return new SuccessResult("Silme işlemi başarılı.");
        }


        public async Task<IDataResult<SelectBranchesDto>> GetAsync(Guid id)
        {
            var entity = await _uow.BranchRepository.GetAsync(t => t.Id == id, t => t.Periods, t => t.SalesPropositions);
            var mappedEntity = ObjectMapper.Map<Branches, SelectBranchesDto>(entity);
            return new SuccessDataResult<SelectBranchesDto>(mappedEntity);
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListBranchesDto>>> GetListAsync(ListBranchesParameterDto input)
        {
            var list = await _uow.BranchRepository.GetListAsync(t => t.IsActive == input.IsActive, t => t.Periods, t => t.SalesPropositions);

            var mappedEntity = ObjectMapper.Map<List<Branches>, List<ListBranchesDto>>(list.ToList());

            return new SuccessDataResult<IList<ListBranchesDto>>(mappedEntity);
        }


        [ValidationAspect(typeof(UpdateBranchesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectBranchesDto>> UpdateAsync(UpdateBranchesDto input)
        {

            var entity = await _uow.BranchRepository.GetAsync(x => x.Id == input.Id);

            await _manager.UpdateControl(_uow.BranchRepository, input.Code, input.Id, entity);

            var mappedEntity = ObjectMapper.Map<UpdateBranchesDto, Branches>(input);

            await _uow.BranchRepository.UpdateAsync(mappedEntity);

            await _uow.SaveChanges();

            return new SuccessDataResult<SelectBranchesDto>(ObjectMapper.Map<Branches, SelectBranchesDto>(mappedEntity));
        }

    }
}

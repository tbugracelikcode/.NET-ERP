using System.Reflection;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.DependencyResolvers.Autofac;
using TsiErp.Business.Entities.FinalControlUnsuitabilityItem.BusinessRules;
using TsiErp.Business.Entities.FinalControlUnsuitabilityItem.Validations;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.FinalControlUnsuitabilityItem;
using TsiErp.Entities.Entities.FinalControlUnsuitabilityItem;
using TsiErp.Entities.Entities.FinalControlUnsuitabilityItem.Dtos;

namespace TsiErp.Business.Entities.FinalControlUnsuitabilityItem.Services
{
    [ServiceRegistration(typeof(IFinalControlUnsuitabilityItemsAppService), DependencyInjectionType.Scoped)]
    public class FinalControlUnsuitabilityItemsAppService : ApplicationService, IFinalControlUnsuitabilityItemsAppService
    {
        FinalControlUnsuitabilityItemManager _manager { get; set; } = new FinalControlUnsuitabilityItemManager();

        [ValidationAspect(typeof(CreateFinalControlUnsuitabilityItemsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectFinalControlUnsuitabilityItemsDto>> CreateAsync(CreateFinalControlUnsuitabilityItemsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.CodeControl(_uow.FinalControlUnsuitabilityItemsRepository, input.Code);

                var entity = ObjectMapper.Map<CreateFinalControlUnsuitabilityItemsDto, FinalControlUnsuitabilityItems>(input);

                var addedEntity = await _uow.FinalControlUnsuitabilityItemsRepository.InsertAsync(entity);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectFinalControlUnsuitabilityItemsDto>(ObjectMapper.Map<FinalControlUnsuitabilityItems, SelectFinalControlUnsuitabilityItemsDto>(addedEntity));
            }
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _uow.FinalControlUnsuitabilityItemsRepository.DeleteAsync(id);
                await _uow.SaveChanges();
                return new SuccessResult("Silme işlemi başarılı.");
            }
        }


        public async Task<IDataResult<SelectFinalControlUnsuitabilityItemsDto>> GetAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.FinalControlUnsuitabilityItemsRepository.GetAsync(t => t.Id == id, null);
                var mappedEntity = ObjectMapper.Map<FinalControlUnsuitabilityItems, SelectFinalControlUnsuitabilityItemsDto>(entity);
                return new SuccessDataResult<SelectFinalControlUnsuitabilityItemsDto>(mappedEntity);
            }
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListFinalControlUnsuitabilityItemsDto>>> GetListAsync(ListFinalControlUnsuitabilityItemsParameterDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var list = await _uow.FinalControlUnsuitabilityItemsRepository.GetListAsync(t => t.IsActive == input.IsActive, null);

                var mappedEntity = ObjectMapper.Map<List<FinalControlUnsuitabilityItems>, List<ListFinalControlUnsuitabilityItemsDto>>(list.ToList());

                return new SuccessDataResult<IList<ListFinalControlUnsuitabilityItemsDto>>(mappedEntity);
            }
        }


        [ValidationAspect(typeof(UpdateFinalControlUnsuitabilityItemsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectFinalControlUnsuitabilityItemsDto>> UpdateAsync(UpdateFinalControlUnsuitabilityItemsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.FinalControlUnsuitabilityItemsRepository.GetAsync(x => x.Id == input.Id);

                await _manager.UpdateControl(_uow.FinalControlUnsuitabilityItemsRepository, input.Code, input.Id, entity);

                var mappedEntity = ObjectMapper.Map<UpdateFinalControlUnsuitabilityItemsDto, FinalControlUnsuitabilityItems>(input);

                await _uow.FinalControlUnsuitabilityItemsRepository.UpdateAsync(mappedEntity);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectFinalControlUnsuitabilityItemsDto>(ObjectMapper.Map<FinalControlUnsuitabilityItems, SelectFinalControlUnsuitabilityItemsDto>(mappedEntity));
            }
        }
    }
}

using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.UnitSet.BusinessRules;
using TsiErp.Business.Entities.UnitSet.Validations;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.Entities.Entities.UnitSet;
using TsiErp.Entities.Entities.UnitSet.Dtos;

namespace TsiErp.Business.Entities.UnitSet.Services
{
    [ServiceRegistration(typeof(IUnitSetsAppService), DependencyInjectionType.Scoped)]
    public class UnitSetsAppService : ApplicationService, IUnitSetsAppService
    {
        UnitSetManager _manager { get; set; } = new UnitSetManager();

        [ValidationAspect(typeof(CreateUnitSetsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectUnitSetsDto>> CreateAsync(CreateUnitSetsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.CodeControl(_uow.UnitSetsRepository, input.Code);

                var entity = ObjectMapper.Map<CreateUnitSetsDto, UnitSets>(input);

                var addedEntity = await _uow.UnitSetsRepository.InsertAsync(entity);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectUnitSetsDto>(ObjectMapper.Map<UnitSets, SelectUnitSetsDto>(addedEntity));
            }
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.DeleteControl(_uow.UnitSetsRepository, id);
                await _uow.UnitSetsRepository.DeleteAsync(id);
                await _uow.SaveChanges();
                return new SuccessResult("Silme işlemi başarılı.");
            }
        }

        public async Task<IDataResult<SelectUnitSetsDto>> GetAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.UnitSetsRepository.GetAsync(t => t.Id == id,
                t => t.Products);
                var mappedEntity = ObjectMapper.Map<UnitSets, SelectUnitSetsDto>(entity);
                return new SuccessDataResult<SelectUnitSetsDto>(mappedEntity);
            }
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListUnitSetsDto>>> GetListAsync(ListUnitSetsParameterDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var list = await _uow.UnitSetsRepository.GetListAsync(t => t.IsActive == input.IsActive,
                t => t.Products);

                var mappedEntity = ObjectMapper.Map<List<UnitSets>, List<ListUnitSetsDto>>(list.ToList());

                return new SuccessDataResult<IList<ListUnitSetsDto>>(mappedEntity);
            }
        }


        [ValidationAspect(typeof(UpdateUnitSetsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectUnitSetsDto>> UpdateAsync(UpdateUnitSetsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.UnitSetsRepository.GetAsync(x => x.Id == input.Id);

                await _manager.UpdateControl(_uow.UnitSetsRepository, input.Code, input.Id, entity);

                var mappedEntity = ObjectMapper.Map<UpdateUnitSetsDto, UnitSets>(input);

                await _uow.UnitSetsRepository.UpdateAsync(mappedEntity);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectUnitSetsDto>(ObjectMapper.Map<UnitSets, SelectUnitSetsDto>(mappedEntity));
            }
        }

        public async Task<IDataResult<SelectUnitSetsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.UnitSetsRepository.GetAsync(x => x.Id == id);

                var updatedEntity = await _uow.UnitSetsRepository.LockRow(entity.Id, lockRow, userId);

                await _uow.SaveChanges();

                var mappedEntity = ObjectMapper.Map<UnitSets, SelectUnitSetsDto>(updatedEntity);

                return new SuccessDataResult<SelectUnitSetsDto>(mappedEntity);
            }
        }
    }
}

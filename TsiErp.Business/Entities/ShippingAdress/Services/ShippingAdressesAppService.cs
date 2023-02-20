using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.ShippingAdress.BusinessRules;
using TsiErp.Business.Entities.ShippingAdress.Validations;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.Entities.Entities.ShippingAdress;
using TsiErp.Entities.Entities.ShippingAdress.Dtos;

namespace TsiErp.Business.Entities.ShippingAdress.Services
{
    [ServiceRegistration(typeof(IShippingAdressesAppService), DependencyInjectionType.Scoped)]
    public class ShippingAdressesAppService : ApplicationService, IShippingAdressesAppService
    {
        ShippingAdressesManager _manager { get; set; } = new ShippingAdressesManager();

        [ValidationAspect(typeof(CreateShippingAdressesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectShippingAdressesDto>> CreateAsync(CreateShippingAdressesDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.CodeControl(_uow.ShippingAdressesRepository, input.Code);

                var entity = ObjectMapper.Map<CreateShippingAdressesDto, ShippingAdresses>(input);

                var addedEntity = await _uow.ShippingAdressesRepository.InsertAsync(entity);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectShippingAdressesDto>(ObjectMapper.Map<ShippingAdresses, SelectShippingAdressesDto>(addedEntity));
            }
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.DeleteControl(_uow.ShippingAdressesRepository, id);
                await _uow.ShippingAdressesRepository.DeleteAsync(id);
                await _uow.SaveChanges();
                return new SuccessResult("Silme işlemi başarılı.");
            }
        }


        public async Task<IDataResult<SelectShippingAdressesDto>> GetAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.ShippingAdressesRepository.GetAsync(t => t.Id == id, t => t.CurrentAccountCards, t => t.SalesPropositions);
                var mappedEntity = ObjectMapper.Map<ShippingAdresses, SelectShippingAdressesDto>(entity);
                return new SuccessDataResult<SelectShippingAdressesDto>(mappedEntity);
            }
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListShippingAdressesDto>>> GetListAsync(ListShippingAdressesParameterDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var list = await _uow.ShippingAdressesRepository.GetListAsync(null, x => x.CurrentAccountCards, t => t.SalesPropositions);

                var mappedEntity = ObjectMapper.Map<List<ShippingAdresses>, List<ListShippingAdressesDto>>(list.ToList());

                return new SuccessDataResult<IList<ListShippingAdressesDto>>(mappedEntity);
            }
        }


        [ValidationAspect(typeof(UpdateShippingAdressesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectShippingAdressesDto>> UpdateAsync(UpdateShippingAdressesDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.ShippingAdressesRepository.GetAsync(x => x.Id == input.Id);

                await _manager.UpdateControl(_uow.ShippingAdressesRepository, input.Code, input.Id, entity);

                var mappedEntity = ObjectMapper.Map<UpdateShippingAdressesDto, ShippingAdresses>(input);

                await _uow.ShippingAdressesRepository.UpdateAsync(mappedEntity);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectShippingAdressesDto>(ObjectMapper.Map<ShippingAdresses, SelectShippingAdressesDto>(mappedEntity));
            }
        }

        public async Task<IDataResult<SelectShippingAdressesDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.ShippingAdressesRepository.GetAsync(x => x.Id == id);

                var updatedEntity = await _uow.ShippingAdressesRepository.LockRow(entity.Id, lockRow, userId);

                await _uow.SaveChanges();

                var mappedEntity = ObjectMapper.Map<ShippingAdresses, SelectShippingAdressesDto>(updatedEntity);

                return new SuccessDataResult<SelectShippingAdressesDto>(mappedEntity);
            }
        }
    }
}

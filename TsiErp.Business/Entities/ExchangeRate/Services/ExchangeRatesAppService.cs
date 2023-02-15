using System.Reflection;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.DependencyResolvers.Autofac;
using TsiErp.Business.Entities.ExchangeRate.Validations;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.ExchangeRate;
using TsiErp.Entities.Entities.ExchangeRate;
using TsiErp.Entities.Entities.ExchangeRate.Dtos;
using TsiErp.Entities.Entities.WorkOrder.Dtos;
using TsiErp.Entities.Entities.WorkOrder;

namespace TsiErp.Business.Entities.ExchangeRate.Services
{
    [ServiceRegistration(typeof(IExchangeRatesAppService), DependencyInjectionType.Scoped)]
    public class ExchangeRatesAppService : ApplicationService, IExchangeRatesAppService
    {

        [ValidationAspect(typeof(CreateExchangeRatesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectExchangeRatesDto>> CreateAsync(CreateExchangeRatesDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = ObjectMapper.Map<CreateExchangeRatesDto, ExchangeRates>(input);

                var addedEntity = await _uow.ExchangeRatesRepository.InsertAsync(entity);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectExchangeRatesDto>(ObjectMapper.Map<ExchangeRates, SelectExchangeRatesDto>(addedEntity));
            }
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _uow.ExchangeRatesRepository.DeleteAsync(id);
                await _uow.SaveChanges();
                return new SuccessResult("Silme işlemi başarılı.");
            }
        }


        public async Task<IDataResult<SelectExchangeRatesDto>> GetAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.ExchangeRatesRepository.GetAsync(t => t.Id == id, t => t.Currencies);
                var mappedEntity = ObjectMapper.Map<ExchangeRates, SelectExchangeRatesDto>(entity);
                return new SuccessDataResult<SelectExchangeRatesDto>(mappedEntity);
            }
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListExchangeRatesDto>>> GetListAsync(ListExchangeRatesParameterDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var list = await _uow.ExchangeRatesRepository.GetListAsync(null, t => t.Currencies);

                var mappedEntity = ObjectMapper.Map<List<ExchangeRates>, List<ListExchangeRatesDto>>(list.ToList());

                return new SuccessDataResult<IList<ListExchangeRatesDto>>(mappedEntity);
            }
        }


        [ValidationAspect(typeof(UpdateExchangeRatesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectExchangeRatesDto>> UpdateAsync(UpdateExchangeRatesDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.ExchangeRatesRepository.GetAsync(x => x.Id == input.Id);

                var mappedEntity = ObjectMapper.Map<UpdateExchangeRatesDto, ExchangeRates>(input);

                await _uow.ExchangeRatesRepository.UpdateAsync(mappedEntity);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectExchangeRatesDto>(ObjectMapper.Map<ExchangeRates, SelectExchangeRatesDto>(mappedEntity));
            }
        }

        public async Task<IDataResult<SelectExchangeRatesDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.ExchangeRatesRepository.GetAsync(x => x.Id == id);

                var updatedEntity = await _uow.ExchangeRatesRepository.LockRow(entity.Id, lockRow, userId);

                await _uow.SaveChanges();

                var mappedEntity = ObjectMapper.Map<ExchangeRates, SelectExchangeRatesDto>(updatedEntity);

                return new SuccessDataResult<SelectExchangeRatesDto>(mappedEntity);
            }
        }
    }
}

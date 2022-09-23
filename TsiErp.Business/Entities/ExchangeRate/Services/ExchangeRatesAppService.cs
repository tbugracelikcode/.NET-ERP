using System.Reflection;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.DependencyResolvers.Autofac;
using TsiErp.Business.Entities.ExchangeRate.Validations;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.ExchangeRate;
using TsiErp.Entities.Entities.ExchangeRate;
using TsiErp.Entities.Entities.ExchangeRate.Dtos;

namespace TsiErp.Business.Entities.ExchangeRate.Services
{
    [ServiceRegistration(typeof(IExchangeRatesAppService), DependencyInjectionType.Scoped)]
    public class ExchangeRatesAppService : ApplicationService, IExchangeRatesAppService
    {
        private readonly IExchangeRatesRepository _repository;

        public ExchangeRatesAppService(IExchangeRatesRepository repository)
        {
            _repository = repository;
        }


        [ValidationAspect(typeof(CreateExchangeRatesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectExchangeRatesDto>> CreateAsync(CreateExchangeRatesDto input)
        {
            var entity = ObjectMapper.Map<CreateExchangeRatesDto, ExchangeRates>(input);

            var addedEntity = await _repository.InsertAsync(entity);

            return new SuccessDataResult<SelectExchangeRatesDto>(ObjectMapper.Map<ExchangeRates, SelectExchangeRatesDto>(addedEntity));
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
            return new SuccessResult("Silme işlemi başarılı.");
        }


        public async Task<IDataResult<SelectExchangeRatesDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id, t => t.Currencies);
            var mappedEntity = ObjectMapper.Map<ExchangeRates, SelectExchangeRatesDto>(entity);
            return new SuccessDataResult<SelectExchangeRatesDto>(mappedEntity);
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListExchangeRatesDto>>> GetListAsync(ListExchangeRatesParameterDto input)
        {
            var list = await _repository.GetListAsync(null, t => t.Currencies);

            var mappedEntity = ObjectMapper.Map<List<ExchangeRates>, List<ListExchangeRatesDto>>(list.ToList());

            return new SuccessDataResult<IList<ListExchangeRatesDto>>(mappedEntity);
        }


        [ValidationAspect(typeof(UpdateExchangeRatesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectExchangeRatesDto>> UpdateAsync(UpdateExchangeRatesDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            var mappedEntity = ObjectMapper.Map<UpdateExchangeRatesDto, ExchangeRates>(input);

            await _repository.UpdateAsync(mappedEntity);

            return new SuccessDataResult<SelectExchangeRatesDto>(ObjectMapper.Map<ExchangeRates, SelectExchangeRatesDto>(mappedEntity));
        }
    }
}

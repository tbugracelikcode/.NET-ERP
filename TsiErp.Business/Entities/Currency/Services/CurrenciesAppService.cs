using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Currency;
using TsiErp.Business.Entities.Currency.Validations;
using TsiErp.Entities.Entities.Currency.Dtos;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Entities.Entities.Currency;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;

namespace TsiErp.Business.Entities.Currency.Services
{
    [ServiceRegistration(typeof(ICurrenciesAppService), DependencyInjectionType.Scoped)]
    public class CurrenciesAppService : ApplicationService, ICurrenciesAppService
    {
        private readonly ICurrenciesRepository _repository;

        public CurrenciesAppService(ICurrenciesRepository repository)
        {
            _repository = repository;
        }


        [ValidationAspect(typeof(CreateCurrenciesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectCurrenciesDto>> CreateAsync(CreateCurrenciesDto input)
        {
            var entity = ObjectMapper.Map<CreateCurrenciesDto, Currencies>(input);

            var addedEntity = await _repository.InsertAsync(entity);

            return new SuccessDataResult<SelectCurrenciesDto>(ObjectMapper.Map<Currencies, SelectCurrenciesDto>(addedEntity));
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
            return new SuccessResult("Silme işlemi başarılı.");
        }


        public async Task<IDataResult<SelectCurrenciesDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id/*,*/ /*x => x.CurrentAccountCards, y => y.ExchangeRates*/);
            var mappedEntity = ObjectMapper.Map<Currencies, SelectCurrenciesDto>(entity);
            return new SuccessDataResult<SelectCurrenciesDto>(mappedEntity);
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListCurrenciesDto>>> GetListAsync(ListCurrenciesParameterDto input)
        {
            var list = await _repository.GetListAsync(t => t.IsActive == input.IsActive /*,*/ /*x => x.CurrentAccountCards, y => y.ExchangeRates*/);

            var mappedEntity = ObjectMapper.Map<List<Currencies>, List<ListCurrenciesDto>>(list.ToList());

            return new SuccessDataResult<IList<ListCurrenciesDto>>(mappedEntity);
        }


        [ValidationAspect(typeof(UpdateCurrenciesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectCurrenciesDto>> UpdateAsync(UpdateCurrenciesDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            var mappedEntity = ObjectMapper.Map<UpdateCurrenciesDto, Currencies>(input);

            await _repository.UpdateAsync(mappedEntity);

            return new SuccessDataResult<SelectCurrenciesDto>(ObjectMapper.Map<Currencies, SelectCurrenciesDto>(mappedEntity));
        }
    }
}

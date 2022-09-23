using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.ShippingAdress;
using TsiErp.Business.Entities.ShippingAdress.Validations;
using TsiErp.Entities.Entities.ShippingAdress.Dtos;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Entities.Entities.ShippingAdress;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;

namespace TsiErp.Business.Entities.ShippingAdress.Services
{
    [ServiceRegistration(typeof(IShippingAdressesAppService), DependencyInjectionType.Scoped)]
    public class ShippingAdressesAppService : ApplicationService, IShippingAdressesAppService
    {
        private readonly IShippingAdressesRepository _repository;

        public ShippingAdressesAppService(IShippingAdressesRepository repository)
        {
            _repository = repository;
        }


        [ValidationAspect(typeof(CreateShippingAdressesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectShippingAdressesDto>> CreateAsync(CreateShippingAdressesDto input)
        {
            var entity = ObjectMapper.Map<CreateShippingAdressesDto, ShippingAdresses>(input);

            var addedEntity = await _repository.InsertAsync(entity);

            return new SuccessDataResult<SelectShippingAdressesDto>(ObjectMapper.Map<ShippingAdresses, SelectShippingAdressesDto>(addedEntity));
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
            return new SuccessResult("Silme işlemi başarılı.");
        }


        public async Task<IDataResult<SelectShippingAdressesDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id, t => t.CurrentAccountCards);
            var mappedEntity = ObjectMapper.Map<ShippingAdresses, SelectShippingAdressesDto>(entity);
            return new SuccessDataResult<SelectShippingAdressesDto>(mappedEntity);
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListShippingAdressesDto>>> GetListAsync(ListShippingAdressesParameterDto input)
        {
            var list = await _repository.GetListAsync(null, x => x.CurrentAccountCards);

            var mappedEntity = ObjectMapper.Map<List<ShippingAdresses>, List<ListShippingAdressesDto>>(list.ToList());

            return new SuccessDataResult<IList<ListShippingAdressesDto>>(mappedEntity);
        }


        [ValidationAspect(typeof(UpdateShippingAdressesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectShippingAdressesDto>> UpdateAsync(UpdateShippingAdressesDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            var mappedEntity = ObjectMapper.Map<UpdateShippingAdressesDto, ShippingAdresses>(input);

            await _repository.UpdateAsync(mappedEntity);

            return new SuccessDataResult<SelectShippingAdressesDto>(ObjectMapper.Map<ShippingAdresses, SelectShippingAdressesDto>(mappedEntity));
        }
    }
}

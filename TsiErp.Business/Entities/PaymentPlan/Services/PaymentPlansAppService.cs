using System.Reflection;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.DependencyResolvers.Autofac;
using TsiErp.Business.Entities.PaymentPlan.Validations;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.PaymentPlan;
using TsiErp.Entities.Entities.PaymentPlan;
using TsiErp.Entities.Entities.PaymentPlan.Dtos;

namespace TsiErp.Business.Entities.PaymentPlan.Services
{
    [ServiceRegistration(typeof(IPaymentPlansAppService), DependencyInjectionType.Scoped)]
    public class PaymentPlansAppService : ApplicationService, IPaymentPlansAppService
    {
        private readonly IPaymentPlansRepository _repository;

        public PaymentPlansAppService(IPaymentPlansRepository repository)
        {
            _repository = repository;
        }


        [ValidationAspect(typeof(CreatePaymentPlansValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPaymentPlansDto>> CreateAsync(CreatePaymentPlansDto input)
        {
            var entity = ObjectMapper.Map<CreatePaymentPlansDto, PaymentPlans>(input);

            var addedEntity = await _repository.InsertAsync(entity);

            return new SuccessDataResult<SelectPaymentPlansDto>(ObjectMapper.Map<PaymentPlans, SelectPaymentPlansDto>(addedEntity));
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
            return new SuccessResult("Silme işlemi başarılı.");
        }


        public async Task<IDataResult<SelectPaymentPlansDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id, null);
            var mappedEntity = ObjectMapper.Map<PaymentPlans, SelectPaymentPlansDto>(entity);
            return new SuccessDataResult<SelectPaymentPlansDto>(mappedEntity);
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListPaymentPlansDto>>> GetListAsync(ListPaymentPlansParameterDto input)
        {
            var list = await _repository.GetListAsync(t => t.IsActive == input.IsActive, null);

            var mappedEntity = ObjectMapper.Map<List<PaymentPlans>, List<ListPaymentPlansDto>>(list.ToList());

            return new SuccessDataResult<IList<ListPaymentPlansDto>>(mappedEntity);
        }


        [ValidationAspect(typeof(UpdatePaymentPlansValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPaymentPlansDto>> UpdateAsync(UpdatePaymentPlansDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            var mappedEntity = ObjectMapper.Map<UpdatePaymentPlansDto, PaymentPlans>(input);

            await _repository.UpdateAsync(mappedEntity);

            return new SuccessDataResult<SelectPaymentPlansDto>(ObjectMapper.Map<PaymentPlans, SelectPaymentPlansDto>(mappedEntity));
        }
    }
}

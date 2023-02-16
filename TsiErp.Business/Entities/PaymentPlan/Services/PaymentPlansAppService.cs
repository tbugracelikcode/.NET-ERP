using System.Reflection;
using Tsi.Core.Services.BusinessCoreServices;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.DependencyResolvers.Autofac;
using TsiErp.Business.Entities.PaymentPlan.BusinessRules;
using TsiErp.Business.Entities.PaymentPlan.Validations;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.PaymentPlan;
using TsiErp.Entities.Entities.PaymentPlan;
using TsiErp.Entities.Entities.PaymentPlan.Dtos;
using TsiErp.Entities.Entities.WorkOrder.Dtos;
using TsiErp.Entities.Entities.WorkOrder;

namespace TsiErp.Business.Entities.PaymentPlan.Services
{
    [ServiceRegistration(typeof(IPaymentPlansAppService), DependencyInjectionType.Scoped)]
    public class PaymentPlansAppService : ApplicationService, IPaymentPlansAppService
    {
        PaymentPlanManager _manager { get; set; } = new PaymentPlanManager();


        [ValidationAspect(typeof(CreatePaymentPlansValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPaymentPlansDto>> CreateAsync(CreatePaymentPlansDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.CodeControl(_uow.PaymentPlansRepository, input.Code);

                var entity = ObjectMapper.Map<CreatePaymentPlansDto, PaymentPlans>(input);

                var addedEntity = await _uow.PaymentPlansRepository.InsertAsync(entity);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectPaymentPlansDto>(ObjectMapper.Map<PaymentPlans, SelectPaymentPlansDto>(addedEntity));
            }
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.DeleteControl(_uow.PaymentPlansRepository, id);
                await _uow.PaymentPlansRepository.DeleteAsync(id);
                await _uow.SaveChanges();
                return new SuccessResult("Silme işlemi başarılı.");
            }
        }


        public async Task<IDataResult<SelectPaymentPlansDto>> GetAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.PaymentPlansRepository.GetAsync(t => t.Id == id, t => t.SalesPropositions, t => t.SalesPropositionLines);
                var mappedEntity = ObjectMapper.Map<PaymentPlans, SelectPaymentPlansDto>(entity);
                return new SuccessDataResult<SelectPaymentPlansDto>(mappedEntity);
            }
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListPaymentPlansDto>>> GetListAsync(ListPaymentPlansParameterDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var list = await _uow.PaymentPlansRepository.GetListAsync(t => t.IsActive == input.IsActive, t => t.SalesPropositions, t => t.SalesPropositionLines);

                var mappedEntity = ObjectMapper.Map<List<PaymentPlans>, List<ListPaymentPlansDto>>(list.ToList());

                return new SuccessDataResult<IList<ListPaymentPlansDto>>(mappedEntity);
            }
        }


        [ValidationAspect(typeof(UpdatePaymentPlansValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPaymentPlansDto>> UpdateAsync(UpdatePaymentPlansDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.PaymentPlansRepository.GetAsync(x => x.Id == input.Id);

                await _manager.UpdateControl(_uow.PaymentPlansRepository, input.Code, input.Id, entity);

                var mappedEntity = ObjectMapper.Map<UpdatePaymentPlansDto, PaymentPlans>(input);

                await _uow.PaymentPlansRepository.UpdateAsync(mappedEntity);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectPaymentPlansDto>(ObjectMapper.Map<PaymentPlans, SelectPaymentPlansDto>(mappedEntity));
            }
        }

        public async Task<IDataResult<SelectPaymentPlansDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.PaymentPlansRepository.GetAsync(x => x.Id == id);

                var updatedEntity = await _uow.PaymentPlansRepository.LockRow(entity.Id, lockRow, userId);

                await _uow.SaveChanges();

                var mappedEntity = ObjectMapper.Map<PaymentPlans, SelectPaymentPlansDto>(updatedEntity);

                return new SuccessDataResult<SelectPaymentPlansDto>(mappedEntity);
            }
        }
    }
}

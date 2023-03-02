using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using TsiErp.Localizations.Resources.PaymentPlans.Page;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.PaymentPlan.BusinessRules;
using TsiErp.Business.Entities.PaymentPlan.Validations;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.PaymentPlan;
using TsiErp.Entities.Entities.PaymentPlan.Dtos;
using Microsoft.Extensions.Localization;

namespace TsiErp.Business.Entities.PaymentPlan.Services
{
    [ServiceRegistration(typeof(IPaymentPlansAppService), DependencyInjectionType.Scoped)]
    public class PaymentPlansAppService : ApplicationService<PaymentPlansResource>, IPaymentPlansAppService
    {
        public PaymentPlansAppService(IStringLocalizer<PaymentPlansResource> l) : base(l)
        {
        }

        PaymentPlanManager _manager { get; set; } = new PaymentPlanManager();


        [ValidationAspect(typeof(CreatePaymentPlansValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPaymentPlansDto>> CreateAsync(CreatePaymentPlansDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.CodeControl(_uow.PaymentPlansRepository, input.Code,L);

                var entity = ObjectMapper.Map<CreatePaymentPlansDto, PaymentPlans>(input);

                var addedEntity = await _uow.PaymentPlansRepository.InsertAsync(entity);
                input.Id = addedEntity.Id;
                var log = LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, "PaymentPlans", LogType.Insert, addedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectPaymentPlansDto>(ObjectMapper.Map<PaymentPlans, SelectPaymentPlansDto>(addedEntity));
            }
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.DeleteControl(_uow.PaymentPlansRepository, id,L);
                await _uow.PaymentPlansRepository.DeleteAsync(id);
                var log = LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, "PaymentPlans", LogType.Delete, id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();
                return new SuccessResult(L["DeleteSuccessMessage"]);
            }
        }


        public async Task<IDataResult<SelectPaymentPlansDto>> GetAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.PaymentPlansRepository.GetAsync(t => t.Id == id, t => t.SalesPropositions, t => t.SalesPropositionLines);
                var mappedEntity = ObjectMapper.Map<PaymentPlans, SelectPaymentPlansDto>(entity);
                var log = LogsAppService.InsertLogToDatabase(mappedEntity, mappedEntity, LoginedUserService.UserId, "PaymentPlans", LogType.Get, id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();
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

                await _manager.UpdateControl(_uow.PaymentPlansRepository, input.Code, input.Id, entity,L);

                var mappedEntity = ObjectMapper.Map<UpdatePaymentPlansDto, PaymentPlans>(input);

                await _uow.PaymentPlansRepository.UpdateAsync(mappedEntity);
                var before = ObjectMapper.Map<PaymentPlans, UpdatePaymentPlansDto>(entity);
                var log = LogsAppService.InsertLogToDatabase(before, input, LoginedUserService.UserId, "PaymentPlans", LogType.Update, mappedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);
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

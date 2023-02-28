using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results; using TsiErp.Localizations.Resources.Branches.Page;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Currency.BusinessRules;
using TsiErp.Business.Entities.Currency.Validations;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.Currency;
using TsiErp.Entities.Entities.Currency.Dtos;
using Microsoft.Extensions.Localization;

namespace TsiErp.Business.Entities.Currency.Services
{
    [ServiceRegistration(typeof(ICurrenciesAppService), DependencyInjectionType.Scoped)]
    public class CurrenciesAppService : ApplicationService<BranchesResource>, ICurrenciesAppService
    {
        public CurrenciesAppService(IStringLocalizer<BranchesResource> l) : base(l)
        {
        }

        CurrencyManager _manager { get; set; } = new CurrencyManager();

        [ValidationAspect(typeof(CreateCurrenciesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectCurrenciesDto>> CreateAsync(CreateCurrenciesDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.CodeControl(_uow.CurrenciesRepository, input.Code);

                var entity = ObjectMapper.Map<CreateCurrenciesDto, Currencies>(input);

                var addedEntity = await _uow.CurrenciesRepository.InsertAsync(entity);
                input.Id = addedEntity.Id;
                var log = LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, "Currencies", LogType.Insert, addedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectCurrenciesDto>(ObjectMapper.Map<Currencies, SelectCurrenciesDto>(addedEntity));
            }
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.DeleteControl(_uow.CurrenciesRepository, id);
                await _uow.CurrenciesRepository.DeleteAsync(id);
                var log = LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, "Currencies", LogType.Delete, id);
                await _uow.LogsRepository.InsertAsync(log);

                await _uow.SaveChanges();
                return new SuccessResult("Silme işlemi başarılı.");
            }
        }


        public async Task<IDataResult<SelectCurrenciesDto>> GetAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.CurrenciesRepository.GetAsync(t => t.Id == id, t => t.CurrentAccountCards, y => y.ExchangeRates, y => y.SalesPropositions);
                var mappedEntity = ObjectMapper.Map<Currencies, SelectCurrenciesDto>(entity);

                var log = LogsAppService.InsertLogToDatabase(mappedEntity, mappedEntity, LoginedUserService.UserId, "Currencies", LogType.Get, id);
                await _uow.LogsRepository.InsertAsync(log);



                await _uow.SaveChanges();
                return new SuccessDataResult<SelectCurrenciesDto>(mappedEntity);
            }
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListCurrenciesDto>>> GetListAsync(ListCurrenciesParameterDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var list = await _uow.CurrenciesRepository.GetListAsync(t => t.IsActive == input.IsActive, x => x.CurrentAccountCards, y => y.ExchangeRates, y => y.SalesPropositions);

                var mappedEntity = ObjectMapper.Map<List<Currencies>, List<ListCurrenciesDto>>(list.ToList());

                return new SuccessDataResult<IList<ListCurrenciesDto>>(mappedEntity);
            }
        }


        [ValidationAspect(typeof(UpdateCurrenciesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectCurrenciesDto>> UpdateAsync(UpdateCurrenciesDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.CurrenciesRepository.GetAsync(x => x.Id == input.Id);

                await _manager.UpdateControl(_uow.CurrenciesRepository, input.Code, input.Id, entity);

                var mappedEntity = ObjectMapper.Map<UpdateCurrenciesDto, Currencies>(input);

                await _uow.CurrenciesRepository.UpdateAsync(mappedEntity);
                var before = ObjectMapper.Map<Currencies, UpdateCurrenciesDto>(entity);
                var log = LogsAppService.InsertLogToDatabase(before, input, LoginedUserService.UserId, "Currencies", LogType.Update, mappedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);

                await _uow.SaveChanges();

                return new SuccessDataResult<SelectCurrenciesDto>(ObjectMapper.Map<Currencies, SelectCurrenciesDto>(mappedEntity));
            }
        }

        public async Task<IDataResult<SelectCurrenciesDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.CurrenciesRepository.GetAsync(x => x.Id == id);

                var updatedEntity = await _uow.CurrenciesRepository.LockRow(entity.Id, lockRow, userId);

                await _uow.SaveChanges();

                var mappedEntity = ObjectMapper.Map<Currencies, SelectCurrenciesDto>(updatedEntity);

                return new SuccessDataResult<SelectCurrenciesDto>(mappedEntity);
            }
        }
    }
}

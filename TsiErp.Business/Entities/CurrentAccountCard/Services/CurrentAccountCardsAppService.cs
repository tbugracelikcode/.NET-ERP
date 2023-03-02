using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results; 
using TsiErp.Localizations.Resources.CurrentAccountCards.Page;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.CurrentAccountCard.BusinessRules;
using TsiErp.Business.Entities.CurrentAccountCard.Validations;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.CurrentAccountCard;
using TsiErp.Entities.Entities.CurrentAccountCard.Dtos;
using Microsoft.Extensions.Localization;

namespace TsiErp.Business.Entities.CurrentAccountCard.Services
{
    [ServiceRegistration(typeof(ICurrentAccountCardsAppService), DependencyInjectionType.Scoped)]
    public class CurrentAccountCardsAppService : ApplicationService<CurrentAccountCardsResource>, ICurrentAccountCardsAppService
    {
        public CurrentAccountCardsAppService(IStringLocalizer<CurrentAccountCardsResource> l) : base(l)
        {
        }

        CurrentAccountCardManager _manager { get; set; } = new CurrentAccountCardManager();

        [ValidationAspect(typeof(CreateCurrentAccountCardsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectCurrentAccountCardsDto>> CreateAsync(CreateCurrentAccountCardsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.CodeControl(_uow.CurrentAccountCardsRepository, input.Code,L);

                var entity = ObjectMapper.Map<CreateCurrentAccountCardsDto, CurrentAccountCards>(input);

                var addedEntity = await _uow.CurrentAccountCardsRepository.InsertAsync(entity);
                input.Id = addedEntity.Id;
                var log = LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, "CurrentAccountCards", LogType.Insert, addedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectCurrentAccountCardsDto>(ObjectMapper.Map<CurrentAccountCards, SelectCurrentAccountCardsDto>(addedEntity));
            }
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.DeleteControl(_uow.CurrentAccountCardsRepository, id,L);
                await _uow.CurrentAccountCardsRepository.DeleteAsync(id);

                var log = LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, "CurrentAccountCards", LogType.Delete, id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();
                return new SuccessResult(L["DeleteSuccessMessage"]);
            }
        }


        public async Task<IDataResult<SelectCurrentAccountCardsDto>> GetAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.CurrentAccountCardsRepository.GetAsync(t => t.Id == id, t => t.Currencies, y => y.ShippingAdresses, y => y.SalesPropositions);
                var mappedEntity = ObjectMapper.Map<CurrentAccountCards, SelectCurrentAccountCardsDto>(entity);
                var log = LogsAppService.InsertLogToDatabase(mappedEntity, mappedEntity, LoginedUserService.UserId, "CurrentAccountCards", LogType.Get, id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();
                return new SuccessDataResult<SelectCurrentAccountCardsDto>(mappedEntity);
            }
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListCurrentAccountCardsDto>>> GetListAsync(ListCurrentAccountCardsParameterDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var list = await _uow.CurrentAccountCardsRepository.GetListAsync(t => t.IsActive == input.IsActive, t => t.Currencies, y => y.ShippingAdresses, y => y.SalesPropositions);

                var mappedEntity = ObjectMapper.Map<List<CurrentAccountCards>, List<ListCurrentAccountCardsDto>>(list.ToList());

                return new SuccessDataResult<IList<ListCurrentAccountCardsDto>>(mappedEntity);
            }
        }


        [ValidationAspect(typeof(UpdateCurrentAccountCardsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectCurrentAccountCardsDto>> UpdateAsync(UpdateCurrentAccountCardsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.CurrentAccountCardsRepository.GetAsync(x => x.Id == input.Id);

                await _manager.UpdateControl(_uow.CurrentAccountCardsRepository, input.Code, input.Id, entity,L);

                var mappedEntity = ObjectMapper.Map<UpdateCurrentAccountCardsDto, CurrentAccountCards>(input);

                await _uow.CurrentAccountCardsRepository.UpdateAsync(mappedEntity);
                var before = ObjectMapper.Map<CurrentAccountCards, UpdateCurrentAccountCardsDto>(entity);
                var log = LogsAppService.InsertLogToDatabase(before, input, LoginedUserService.UserId, "CurrentAccountCards", LogType.Update, mappedEntity.Id);

                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectCurrentAccountCardsDto>(ObjectMapper.Map<CurrentAccountCards, SelectCurrentAccountCardsDto>(mappedEntity));
            }
        }

        public async Task<IDataResult<SelectCurrentAccountCardsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.CurrentAccountCardsRepository.GetAsync(x => x.Id == id);

                var updatedEntity = await _uow.CurrentAccountCardsRepository.LockRow(entity.Id, lockRow, userId);

                await _uow.SaveChanges();

                var mappedEntity = ObjectMapper.Map<CurrentAccountCards, SelectCurrentAccountCardsDto>(updatedEntity);

                return new SuccessDataResult<SelectCurrentAccountCardsDto>(mappedEntity);
            }
        }
    }
}

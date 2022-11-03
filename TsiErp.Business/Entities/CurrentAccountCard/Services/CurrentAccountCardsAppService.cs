using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.CurrentAccountCard;
using TsiErp.Business.Entities.CurrentAccountCard.Validations;
using TsiErp.Entities.Entities.CurrentAccountCard.Dtos;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Entities.Entities.CurrentAccountCard;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.Entities.CurrentAccountCard.BusinessRules;

namespace TsiErp.Business.Entities.CurrentAccountCard.Services
{
    [ServiceRegistration(typeof(ICurrentAccountCardsAppService), DependencyInjectionType.Scoped)]
    public class CurrentAccountCardsAppService : ApplicationService, ICurrentAccountCardsAppService
    {
        private readonly ICurrentAccountCardsRepository _repository;

        CurrentAccountCardManager _manager { get; set; } = new CurrentAccountCardManager();

        public CurrentAccountCardsAppService(ICurrentAccountCardsRepository repository)
        {
            _repository = repository;
        }


        [ValidationAspect(typeof(CreateCurrentAccountCardsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectCurrentAccountCardsDto>> CreateAsync(CreateCurrentAccountCardsDto input)
        {
            await _manager.CodeControl(_repository, input.Code);

            var entity = ObjectMapper.Map<CreateCurrentAccountCardsDto, CurrentAccountCards>(input);

            var addedEntity = await _repository.InsertAsync(entity);
            await _repository.SaveChanges();

            return new SuccessDataResult<SelectCurrentAccountCardsDto>(ObjectMapper.Map<CurrentAccountCards, SelectCurrentAccountCardsDto>(addedEntity));
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            await _manager.DeleteControl(_repository, id);
            await _repository.DeleteAsync(id);
            await _repository.SaveChanges();
            return new SuccessResult("Silme işlemi başarılı.");
        }


        public async Task<IDataResult<SelectCurrentAccountCardsDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id, t => t.Currencies, y => y.ShippingAdresses, y => y.SalesPropositions);
            var mappedEntity = ObjectMapper.Map<CurrentAccountCards, SelectCurrentAccountCardsDto>(entity);
            return new SuccessDataResult<SelectCurrentAccountCardsDto>(mappedEntity);
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListCurrentAccountCardsDto>>> GetListAsync(ListCurrentAccountCardsParameterDto input)
        {
            var list = await _repository.GetListAsync(t => t.IsActive == input.IsActive, t => t.Currencies, y => y.ShippingAdresses, y => y.SalesPropositions);

            var mappedEntity = ObjectMapper.Map<List<CurrentAccountCards>, List<ListCurrentAccountCardsDto>>(list.ToList());

            return new SuccessDataResult<IList<ListCurrentAccountCardsDto>>(mappedEntity);
        }


        [ValidationAspect(typeof(UpdateCurrentAccountCardsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectCurrentAccountCardsDto>> UpdateAsync(UpdateCurrentAccountCardsDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            await _manager.UpdateControl(_repository, input.Code, input.Id, entity);

            var mappedEntity = ObjectMapper.Map<UpdateCurrentAccountCardsDto, CurrentAccountCards>(input);

            await _repository.UpdateAsync(mappedEntity);
            await _repository.SaveChanges();

            return new SuccessDataResult<SelectCurrentAccountCardsDto>(ObjectMapper.Map<CurrentAccountCards, SelectCurrentAccountCardsDto>(mappedEntity));
        }
    }
}

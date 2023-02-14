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
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;

namespace TsiErp.Business.Entities.CurrentAccountCard.Services
{
    [ServiceRegistration(typeof(ICurrentAccountCardsAppService), DependencyInjectionType.Scoped)]
    public class CurrentAccountCardsAppService : ApplicationService, ICurrentAccountCardsAppService
    {
        CurrentAccountCardManager _manager { get; set; } = new CurrentAccountCardManager();

        [ValidationAspect(typeof(CreateCurrentAccountCardsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectCurrentAccountCardsDto>> CreateAsync(CreateCurrentAccountCardsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.CodeControl(_uow.CurrentAccountCardsRepository, input.Code);

                var entity = ObjectMapper.Map<CreateCurrentAccountCardsDto, CurrentAccountCards>(input);

                var addedEntity = await _uow.CurrentAccountCardsRepository.InsertAsync(entity);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectCurrentAccountCardsDto>(ObjectMapper.Map<CurrentAccountCards, SelectCurrentAccountCardsDto>(addedEntity));
            }
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.DeleteControl(_uow.CurrentAccountCardsRepository, id);
                await _uow.CurrentAccountCardsRepository.DeleteAsync(id);
                await _uow.SaveChanges();
                return new SuccessResult("Silme işlemi başarılı.");
            }
        }


        public async Task<IDataResult<SelectCurrentAccountCardsDto>> GetAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.CurrentAccountCardsRepository.GetAsync(t => t.Id == id, t => t.Currencies, y => y.ShippingAdresses, y => y.SalesPropositions);
                var mappedEntity = ObjectMapper.Map<CurrentAccountCards, SelectCurrentAccountCardsDto>(entity);
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

                await _manager.UpdateControl(_uow.CurrentAccountCardsRepository, input.Code, input.Id, entity);

                var mappedEntity = ObjectMapper.Map<UpdateCurrentAccountCardsDto, CurrentAccountCards>(input);

                await _uow.CurrentAccountCardsRepository.UpdateAsync(mappedEntity);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectCurrentAccountCardsDto>(ObjectMapper.Map<CurrentAccountCards, SelectCurrentAccountCardsDto>(mappedEntity));
            }
        }
    }
}

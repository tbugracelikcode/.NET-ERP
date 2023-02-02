using Tsi.Application.Contract.Services.EntityFrameworkCore;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.Entities.PurchasePrice.BusinessRules;
using TsiErp.Business.Entities.PurchasePrice.Validations;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.PurchasePrice;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.PurchasePriceLine;
using TsiErp.Entities.Entities.PurchasePrice;
using TsiErp.Entities.Entities.PurchasePrice.Dtos;
using TsiErp.Entities.Entities.PurchasePriceLine;
using TsiErp.Entities.Entities.PurchasePriceLine.Dtos;

namespace TsiErp.Business.Entities.PurchasePrice.Services
{
    [ServiceRegistration(typeof(IPurchasePricesAppService), DependencyInjectionType.Scoped)]
    public class PurchasePricesAppService : ApplicationService, IPurchasePricesAppService
    {
        private readonly IPurchasePricesRepository _repository;
        private readonly IPurchasePriceLinesRepository _lineRepository;


        PurchasePriceManager _manager { get; set; } = new PurchasePriceManager();
        public PurchasePricesAppService(IPurchasePricesRepository repository, IPurchasePriceLinesRepository lineRepository)
        {
            _repository = repository;
            _lineRepository = lineRepository;
        }


        [ValidationAspect(typeof(CreatePurchasePricesValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPurchasePricesDto>> CreateAsync(CreatePurchasePricesDto input)
        {
            await _manager.CodeControl(_repository, input.Code);

            var entity = ObjectMapper.Map<CreatePurchasePricesDto, PurchasePrices>(input);

            var addedEntity = await _repository.InsertAsync(entity);

            foreach (var item in input.SelectPurchasePriceLines)
            {
                var lineEntity = ObjectMapper.Map<SelectPurchasePriceLinesDto, PurchasePriceLines>(item);
                lineEntity.Id = GuidGenerator.CreateGuid();
                lineEntity.PurchasePriceID = addedEntity.Id;
                await _lineRepository.InsertAsync(lineEntity);
            }

            await _repository.SaveChanges();
            await _lineRepository.SaveChanges();
            return new SuccessDataResult<SelectPurchasePricesDto>(ObjectMapper.Map<PurchasePrices, SelectPurchasePricesDto>(addedEntity));
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var lines = (await _lineRepository.GetAsync(t => t.Id == id));

            if (lines != null)
            {
                await _manager.DeleteControl(_repository, lines.PurchasePriceID, lines.Id, true);
                await _lineRepository.DeleteAsync(id);
                await _repository.SaveChanges();
                await _lineRepository.SaveChanges();
                return new SuccessResult("Silme işlemi başarılı.");
            }
            else
            {
                await _manager.DeleteControl(_repository, id, Guid.Empty, false);
                await _repository.DeleteAsync(id);
                await _repository.SaveChanges();
                await _lineRepository.SaveChanges();
                return new SuccessResult("Silme işlemi başarılı.");
            }
        }

        public async Task<IDataResult<SelectPurchasePricesDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id,
                t => t.PurchasePriceLines,
                t => t.Branches,
                t => t.Warehouses,
                t => t.Currencies,
                t => t.CurrentAccountCards);

            var mappedEntity = ObjectMapper.Map<PurchasePrices, SelectPurchasePricesDto>(entity);

            mappedEntity.SelectPurchasePriceLines = ObjectMapper.Map<List<PurchasePriceLines>, List<SelectPurchasePriceLinesDto>>(entity.PurchasePriceLines.ToList());

            return new SuccessDataResult<SelectPurchasePricesDto>(mappedEntity);
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListPurchasePricesDto>>> GetListAsync(ListPurchasePricesParameterDto input)
        {
            var list = await _repository.GetListAsync(null,
                t => t.PurchasePriceLines,
                t => t.Branches,
                t => t.Warehouses,
                t => t.Currencies,
                t => t.CurrentAccountCards);

            var mappedEntity = ObjectMapper.Map<List<PurchasePrices>, List<ListPurchasePricesDto>>(list.ToList());

            return new SuccessDataResult<IList<ListPurchasePricesDto>>(mappedEntity);
        }

        [ValidationAspect(typeof(UpdatePurchasePricesValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPurchasePricesDto>> UpdateAsync(UpdatePurchasePricesDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            await _manager.UpdateControl(_repository, input.Code, input.Id, entity);

            var mappedEntity = ObjectMapper.Map<UpdatePurchasePricesDto, PurchasePrices>(input);

            await _repository.UpdateAsync(mappedEntity);

            foreach (var item in input.SelectPurchasePriceLines)
            {
                var lineEntity = ObjectMapper.Map<SelectPurchasePriceLinesDto, PurchasePriceLines>(item);
                lineEntity.PurchasePriceID = mappedEntity.Id;

                if (lineEntity.Id == Guid.Empty)
                {
                    lineEntity.Id = GuidGenerator.CreateGuid();
                    await _lineRepository.InsertAsync(lineEntity);
                }
                else
                {
                    await _lineRepository.UpdateAsync(lineEntity);
                }
            }

            await _repository.SaveChanges();
            await _lineRepository.SaveChanges();

            return new SuccessDataResult<SelectPurchasePricesDto>(ObjectMapper.Map<PurchasePrices, SelectPurchasePricesDto>(mappedEntity));
        }
    }
}

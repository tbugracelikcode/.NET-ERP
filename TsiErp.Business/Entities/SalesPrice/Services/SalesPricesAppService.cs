using Tsi.Application.Contract.Services.EntityFrameworkCore;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.Entities.SalesPrice.BusinessRules;
using TsiErp.Business.Entities.SalesPrice.Validations;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.SalesPrice;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.SalesPriceLine;
using TsiErp.Entities.Entities.SalesPrice;
using TsiErp.Entities.Entities.SalesPrice.Dtos;
using TsiErp.Entities.Entities.SalesPriceLine;
using TsiErp.Entities.Entities.SalesPriceLine.Dtos;

namespace TsiErp.Business.Entities.SalesPrice.Services
{
    [ServiceRegistration(typeof(ISalesPricesAppService), DependencyInjectionType.Scoped)]
    public class SalesPricesAppService : ApplicationService, ISalesPricesAppService
    {
        private readonly ISalesPricesRepository _repository;
        private readonly ISalesPriceLinesRepository _lineRepository;

        SalesPriceManager _manager { get; set; } = new SalesPriceManager();
        public SalesPricesAppService(ISalesPricesRepository repository, ISalesPriceLinesRepository lineRepository)
        {
            _repository = repository;
            _lineRepository = lineRepository;
        }


        [ValidationAspect(typeof(CreateSalesPricesValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectSalesPricesDto>> CreateAsync(CreateSalesPricesDto input)
        {
            await _manager.CodeControl(_repository, input.Code);

            var entity = ObjectMapper.Map<CreateSalesPricesDto, SalesPrices>(input);

            var addedEntity = await _repository.InsertAsync(entity);

            foreach (var item in input.SelectSalesPriceLines)
            {
                var lineEntity = ObjectMapper.Map<SelectSalesPriceLinesDto, SalesPriceLines>(item);
                lineEntity.Id = GuidGenerator.CreateGuid();
                lineEntity.SalesPriceID = addedEntity.Id;
                await _lineRepository.InsertAsync(lineEntity);
            }

            await _repository.SaveChanges();
            await _lineRepository.SaveChanges();
            return new SuccessDataResult<SelectSalesPricesDto>(ObjectMapper.Map<SalesPrices, SelectSalesPricesDto>(addedEntity));
        }
        

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var lines = (await _lineRepository.GetAsync(t => t.Id == id));

            if (lines != null)
            {
                await _manager.DeleteControl(_repository, lines.SalesPriceID, lines.Id, true);
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

        public async Task<IDataResult<SelectSalesPricesDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id,
                t => t.SalesPriceLines,
                t=>t.Warehouses,
                t=>t.Branches,
                t => t.Currencies);

            var mappedEntity = ObjectMapper.Map<SalesPrices, SelectSalesPricesDto>(entity);

            mappedEntity.SelectSalesPriceLines = ObjectMapper.Map<List<SalesPriceLines>, List<SelectSalesPriceLinesDto>>(entity.SalesPriceLines.ToList());

            return new SuccessDataResult<SelectSalesPricesDto>(mappedEntity);
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListSalesPricesDto>>> GetListAsync(ListSalesPricesParameterDto input)
        {
            var list = await _repository.GetListAsync(t=>t.IsActive == input.IsActive,
                t => t.SalesPriceLines,
                t => t.Warehouses,
                t => t.Branches,
                t => t.Currencies);

            var mappedEntity = ObjectMapper.Map<List<SalesPrices>, List<ListSalesPricesDto>>(list.ToList());

            return new SuccessDataResult<IList<ListSalesPricesDto>>(mappedEntity);
        }

        [ValidationAspect(typeof(UpdateSalesPricesValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectSalesPricesDto>> UpdateAsync(UpdateSalesPricesDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            await _manager.UpdateControl(_repository, input.Code, input.Id, entity);

            var mappedEntity = ObjectMapper.Map<UpdateSalesPricesDto, SalesPrices>(input);

            await _repository.UpdateAsync(mappedEntity);

            foreach (var item in input.SelectSalesPriceLines)
            {
                var lineEntity = ObjectMapper.Map<SelectSalesPriceLinesDto, SalesPriceLines>(item);
                lineEntity.SalesPriceID = mappedEntity.Id;

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

            return new SuccessDataResult<SelectSalesPricesDto>(ObjectMapper.Map<SalesPrices, SelectSalesPricesDto>(mappedEntity));
        }

        public async Task<IDataResult<IList<SelectSalesPriceLinesDto>>> GetSelectLineListAsync(Guid productId)
        {
            var list = await _lineRepository.GetListAsync(t => t.ProductID == productId, t => t.Products);

            var mappedEntity = ObjectMapper.Map<List<SalesPriceLines>, List<SelectSalesPriceLinesDto>>(list.ToList());

            return new SuccessDataResult<IList<SelectSalesPriceLinesDto>>(mappedEntity);
        }
    }
}

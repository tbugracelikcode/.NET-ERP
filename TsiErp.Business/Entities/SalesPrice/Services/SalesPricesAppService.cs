using Tsi.Core.Services.BusinessCoreServices;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.Entities.SalesPrice.BusinessRules;
using TsiErp.Business.Entities.SalesPrice.Validations;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
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
        SalesPriceManager _manager { get; set; } = new SalesPriceManager();


        [ValidationAspect(typeof(CreateSalesPricesValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectSalesPricesDto>> CreateAsync(CreateSalesPricesDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.CodeControl(_uow.SalesPricesRepository, input.Code);

                var entity = ObjectMapper.Map<CreateSalesPricesDto, SalesPrices>(input);

                var addedEntity = await _uow.SalesPricesRepository.InsertAsync(entity);

                foreach (var item in input.SelectSalesPriceLines)
                {
                    var lineEntity = ObjectMapper.Map<SelectSalesPriceLinesDto, SalesPriceLines>(item);
                    lineEntity.Id = GuidGenerator.CreateGuid();
                    lineEntity.SalesPriceID = addedEntity.Id;
                    await _uow.SalesPriceLinesRepository.InsertAsync(lineEntity);
                }

                await _uow.SaveChanges();
                return new SuccessDataResult<SelectSalesPricesDto>(ObjectMapper.Map<SalesPrices, SelectSalesPricesDto>(addedEntity));
            }
        }
        

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var lines = (await _uow.SalesPriceLinesRepository.GetAsync(t => t.Id == id));

                if (lines != null)
                {
                    await _manager.DeleteControl(_uow.SalesPricesRepository, lines.SalesPriceID, lines.Id, true);
                    await _uow.SalesPriceLinesRepository.DeleteAsync(id);
                    await _uow.SaveChanges();
                    return new SuccessResult("Silme işlemi başarılı.");
                }
                else
                {
                    await _manager.DeleteControl(_uow.SalesPricesRepository, id, Guid.Empty, false);
                    await _uow.SalesPricesRepository.DeleteAsync(id);
                    await _uow.SaveChanges();
                    return new SuccessResult("Silme işlemi başarılı.");
                }
            }
        }

        public async Task<IDataResult<SelectSalesPricesDto>> GetAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.SalesPricesRepository.GetAsync(t => t.Id == id,
                t => t.SalesPriceLines,
                t => t.Warehouses,
                t => t.Branches,
                t => t.Currencies);

                var mappedEntity = ObjectMapper.Map<SalesPrices, SelectSalesPricesDto>(entity);

                mappedEntity.SelectSalesPriceLines = ObjectMapper.Map<List<SalesPriceLines>, List<SelectSalesPriceLinesDto>>(entity.SalesPriceLines.ToList());

                return new SuccessDataResult<SelectSalesPricesDto>(mappedEntity);
            }
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListSalesPricesDto>>> GetListAsync(ListSalesPricesParameterDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var list = await _uow.SalesPricesRepository.GetListAsync(t => t.IsActive == input.IsActive,
                t => t.SalesPriceLines,
                t => t.Warehouses,
                t => t.Branches,
                t => t.Currencies);

                var mappedEntity = ObjectMapper.Map<List<SalesPrices>, List<ListSalesPricesDto>>(list.ToList());

                return new SuccessDataResult<IList<ListSalesPricesDto>>(mappedEntity);
            }
        }

        [ValidationAspect(typeof(UpdateSalesPricesValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectSalesPricesDto>> UpdateAsync(UpdateSalesPricesDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.SalesPricesRepository.GetAsync(x => x.Id == input.Id);

                await _manager.UpdateControl(_uow.SalesPricesRepository, input.Code, input.Id, entity);

                var mappedEntity = ObjectMapper.Map<UpdateSalesPricesDto, SalesPrices>(input);

                await _uow.SalesPricesRepository.UpdateAsync(mappedEntity);

                foreach (var item in input.SelectSalesPriceLines)
                {
                    var lineEntity = ObjectMapper.Map<SelectSalesPriceLinesDto, SalesPriceLines>(item);
                    lineEntity.SalesPriceID = mappedEntity.Id;

                    if (lineEntity.Id == Guid.Empty)
                    {
                        lineEntity.Id = GuidGenerator.CreateGuid();
                        await _uow.SalesPriceLinesRepository.InsertAsync(lineEntity);
                    }
                    else
                    {
                        await _uow.SalesPriceLinesRepository.UpdateAsync(lineEntity);
                    }
                }

                await _uow.SaveChanges();

                return new SuccessDataResult<SelectSalesPricesDto>(ObjectMapper.Map<SalesPrices, SelectSalesPricesDto>(mappedEntity));
            }
        }

        public async Task<IDataResult<IList<SelectSalesPriceLinesDto>>> GetSelectLineListAsync(Guid productId)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var list = await _uow.SalesPriceLinesRepository.GetListAsync(t => t.ProductID == productId, t => t.Products);

                var mappedEntity = ObjectMapper.Map<List<SalesPriceLines>, List<SelectSalesPriceLinesDto>>(list.ToList());

                return new SuccessDataResult<IList<SelectSalesPriceLinesDto>>(mappedEntity);
            }
        }

        public async Task<IDataResult<SelectSalesPricesDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.SalesPricesRepository.GetAsync(x => x.Id == id);

                var updatedEntity = await _uow.SalesPricesRepository.LockRow(entity.Id, lockRow, userId);

                await _uow.SaveChanges();

                var mappedEntity = ObjectMapper.Map<SalesPrices, SelectSalesPricesDto>(updatedEntity);

                return new SuccessDataResult<SelectSalesPricesDto>(mappedEntity);
            }
        }
    }
}

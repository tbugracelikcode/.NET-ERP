using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.PurchasePrice.BusinessRules;
using TsiErp.Business.Entities.PurchasePrice.Validations;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.PurchasePrice;
using TsiErp.Entities.Entities.PurchasePrice.Dtos;
using TsiErp.Entities.Entities.PurchasePriceLine;
using TsiErp.Entities.Entities.PurchasePriceLine.Dtos;

namespace TsiErp.Business.Entities.PurchasePrice.Services
{
    [ServiceRegistration(typeof(IPurchasePricesAppService), DependencyInjectionType.Scoped)]
    public class PurchasePricesAppService : ApplicationService, IPurchasePricesAppService
    {
        PurchasePriceManager _manager { get; set; } = new PurchasePriceManager();

        [ValidationAspect(typeof(CreatePurchasePricesValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPurchasePricesDto>> CreateAsync(CreatePurchasePricesDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.CodeControl(_uow.PurchasePricesRepository, input.Code);

                var entity = ObjectMapper.Map<CreatePurchasePricesDto, PurchasePrices>(input);

                var addedEntity = await _uow.PurchasePricesRepository.InsertAsync(entity);

                foreach (var item in input.SelectPurchasePriceLines)
                {
                    var lineEntity = ObjectMapper.Map<SelectPurchasePriceLinesDto, PurchasePriceLines>(item);
                    lineEntity.PurchasePriceID = addedEntity.Id;
                    await _uow.PurchasePriceLinesRepository.InsertAsync(lineEntity);
                }
                input.Id = addedEntity.Id;
                var log = LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, "PurchasePrices", LogType.Insert, addedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);

                await _uow.SaveChanges();
                return new SuccessDataResult<SelectPurchasePricesDto>(ObjectMapper.Map<PurchasePrices, SelectPurchasePricesDto>(addedEntity));
            }
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var lines = (await _uow.PurchasePriceLinesRepository.GetAsync(t => t.Id == id));

                if (lines != null)
                {
                    await _manager.DeleteControl(_uow.PurchasePricesRepository, lines.PurchasePriceID, lines.Id, true);
                    await _uow.PurchasePriceLinesRepository.DeleteAsync(id);
                    await _uow.SaveChanges();
                    return new SuccessResult("Silme işlemi başarılı.");
                }
                else
                {
                    await _manager.DeleteControl(_uow.PurchasePricesRepository, id, Guid.Empty, false);
                    var list = (await _uow.PurchasePriceLinesRepository.GetListAsync(t => t.PurchasePriceID == id));
                    foreach (var line in list)
                    {
                        await _uow.PurchasePriceLinesRepository.DeleteAsync(line.Id);
                    }
                    await _uow.PurchasePricesRepository.DeleteAsync(id);
                    var log = LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, "PurchasePrices", LogType.Delete, id);
                    await _uow.LogsRepository.InsertAsync(log);
                    await _uow.SaveChanges();
                    return new SuccessResult("Silme işlemi başarılı.");
                }
            }
        }

        public async Task<IDataResult<SelectPurchasePricesDto>> GetAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.PurchasePricesRepository.GetAsync(t => t.Id == id,
                t => t.PurchasePriceLines,
                t => t.Branches,
                t => t.Warehouses,
                t => t.Currencies,
                t => t.CurrentAccountCards);

                var mappedEntity = ObjectMapper.Map<PurchasePrices, SelectPurchasePricesDto>(entity);

                mappedEntity.SelectPurchasePriceLines = ObjectMapper.Map<List<PurchasePriceLines>, List<SelectPurchasePriceLinesDto>>(entity.PurchasePriceLines.ToList());

                foreach (var item in mappedEntity.SelectPurchasePriceLines)
                {
                    item.ProductCode = (await _uow.ProductsRepository.GetAsync(t => t.Id == item.ProductID)).Code;
                    item.ProductName = (await _uow.ProductsRepository.GetAsync(t => t.Id == item.ProductID)).Name;
                    item.CurrencyCode = (await _uow.CurrenciesRepository.GetAsync(t => t.Id == item.CurrencyID)).Code;
                }
                var log = LogsAppService.InsertLogToDatabase(mappedEntity, mappedEntity, LoginedUserService.UserId, "PurchasePrices", LogType.Get, id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectPurchasePricesDto>(mappedEntity);
            }
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListPurchasePricesDto>>> GetListAsync(ListPurchasePricesParameterDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var list = await _uow.PurchasePricesRepository.GetListAsync(t => t.IsActive == input.IsActive,
                t => t.PurchasePriceLines,
                t => t.Branches,
                t => t.Warehouses,
                t => t.Currencies,
                t => t.CurrentAccountCards);

                var mappedEntity = ObjectMapper.Map<List<PurchasePrices>, List<ListPurchasePricesDto>>(list.ToList());

                return new SuccessDataResult<IList<ListPurchasePricesDto>>(mappedEntity);
            }
        }

        [ValidationAspect(typeof(UpdatePurchasePricesValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPurchasePricesDto>> UpdateAsync(UpdatePurchasePricesDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.PurchasePricesRepository.GetAsync(x => x.Id == input.Id);

                await _manager.UpdateControl(_uow.PurchasePricesRepository, input.Code, input.Id, entity);

                var mappedEntity = ObjectMapper.Map<UpdatePurchasePricesDto, PurchasePrices>(input);

                await _uow.PurchasePricesRepository.UpdateAsync(mappedEntity);

                foreach (var item in input.SelectPurchasePriceLines)
                {
                    var lineEntity = ObjectMapper.Map<SelectPurchasePriceLinesDto, PurchasePriceLines>(item);
                    lineEntity.PurchasePriceID = mappedEntity.Id;

                    if (lineEntity.Id == Guid.Empty)
                    {
                        await _uow.PurchasePriceLinesRepository.InsertAsync(lineEntity);
                    }
                    else
                    {
                        await _uow.PurchasePriceLinesRepository.UpdateAsync(lineEntity);
                    }
                }
                var before = ObjectMapper.Map<PurchasePrices, UpdatePurchasePricesDto>(entity);
                before.SelectPurchasePriceLines = ObjectMapper.Map<List<PurchasePriceLines>, List<SelectPurchasePriceLinesDto>>(entity.PurchasePriceLines.ToList());
                var log = LogsAppService.InsertLogToDatabase(before, input, LoginedUserService.UserId, "PurchasePrices", LogType.Update, mappedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectPurchasePricesDto>(ObjectMapper.Map<PurchasePrices, SelectPurchasePricesDto>(mappedEntity));
            }
        }

        public async Task<IDataResult<IList<SelectPurchasePriceLinesDto>>> GetSelectLineListAsync(Guid productId)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var list = await _uow.PurchasePriceLinesRepository.GetListAsync(t => t.ProductID == productId, t => t.Products);

                var mappedEntity = ObjectMapper.Map<List<PurchasePriceLines>, List<SelectPurchasePriceLinesDto>>(list.ToList());

                return new SuccessDataResult<IList<SelectPurchasePriceLinesDto>>(mappedEntity);
            }
        }

        public async Task<IDataResult<SelectPurchasePricesDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.PurchasePricesRepository.GetAsync(x => x.Id == id);

                var updatedEntity = await _uow.PurchasePricesRepository.LockRow(entity.Id, lockRow, userId);

                await _uow.SaveChanges();

                var mappedEntity = ObjectMapper.Map<PurchasePrices, SelectPurchasePricesDto>(updatedEntity);

                return new SuccessDataResult<SelectPurchasePricesDto>(mappedEntity);
            }
        }
    }
}

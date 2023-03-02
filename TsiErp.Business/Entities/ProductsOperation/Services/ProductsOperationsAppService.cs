using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results; 
using TsiErp.Localizations.Resources.ProductsOperations.Page;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.ProductsOperation.BusinessRules;
using TsiErp.Business.Entities.ProductsOperation.Validations;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.ProductsOperation;
using TsiErp.Entities.Entities.ProductsOperation.Dtos;
using TsiErp.Entities.Entities.ProductsOperationLine;
using TsiErp.Entities.Entities.ProductsOperationLine.Dtos;
using Microsoft.Extensions.Localization;

namespace TsiErp.Business.Entities.ProductsOperation.Services
{
    [ServiceRegistration(typeof(IProductsOperationsAppService), DependencyInjectionType.Scoped)]

    public class ProductsOperationsAppService : ApplicationService<ProductsOperationsResource>, IProductsOperationsAppService
    {
        public ProductsOperationsAppService(IStringLocalizer<ProductsOperationsResource> l) : base(l)
        {
        }

        ProductsOperationManager _manager { get; set; } = new ProductsOperationManager();

        [ValidationAspect(typeof(CreateProductsOperationsValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectProductsOperationsDto>> CreateAsync(CreateProductsOperationsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.CodeControl(_uow.ProductsOperationsRepository, input.Code,L);

                var entity = ObjectMapper.Map<CreateProductsOperationsDto, ProductsOperations>(input);

                var addedEntity = await _uow.ProductsOperationsRepository.InsertAsync(entity);

                foreach (var item in input.SelectProductsOperationLines)
                {
                    var lineEntity = ObjectMapper.Map<SelectProductsOperationLinesDto, ProductsOperationLines>(item);
                    lineEntity.ProductsOperationID = addedEntity.Id;
                    await _uow.ProductsOperationLinesRepository.InsertAsync(lineEntity);
                }
                input.Id = addedEntity.Id;
                var log = LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, "ProductsOperations", LogType.Insert, addedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);

                await _uow.SaveChanges();
                return new SuccessDataResult<SelectProductsOperationsDto>(ObjectMapper.Map<ProductsOperations, SelectProductsOperationsDto>(addedEntity));
            }
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var lines = (await _uow.ProductsOperationLinesRepository.GetAsync(t => t.Id == id));

                if (lines != null)
                {
                    await _manager.DeleteControl(_uow.ProductsOperationsRepository, lines.ProductsOperationID, lines.Id, true);
                    await _uow.ProductsOperationLinesRepository.DeleteAsync(id);
                    await _uow.SaveChanges();
                    return new SuccessResult(L["DeleteSuccessMessage"]);
                }
                else
                {
                    await _manager.DeleteControl(_uow.ProductsOperationsRepository, id, Guid.Empty, false);

                    var list = (await _uow.ProductsOperationLinesRepository.GetListAsync(t => t.ProductsOperationID == id));
                    foreach (var line in list)
                    {
                        await _uow.ProductsOperationLinesRepository.DeleteAsync(line.Id);
                    }
                    await _uow.ProductsOperationsRepository.DeleteAsync(id);
                    var log = LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, "ProductsOperations", LogType.Delete, id);
                    await _uow.LogsRepository.InsertAsync(log);
                    await _uow.SaveChanges();
                    return new SuccessResult(L["DeleteSuccessMessage"]);
                }
            }
        }

        public async Task<IDataResult<SelectProductsOperationsDto>> GetAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.ProductsOperationsRepository.GetAsync(t => t.Id == id,
                t => t.ProductsOperationLines,
                t => t.Products);

                var mappedEntity = ObjectMapper.Map<ProductsOperations, SelectProductsOperationsDto>(entity);

                mappedEntity.SelectProductsOperationLines = ObjectMapper.Map<List<ProductsOperationLines>, List<SelectProductsOperationLinesDto>>(entity.ProductsOperationLines.ToList());

                foreach (var item in mappedEntity.SelectProductsOperationLines)
                {
                    item.StationCode = (await _uow.StationsRepository.GetAsync(t => t.Id == item.StationID)).Code;
                    item.StationName = (await _uow.StationsRepository.GetAsync(t => t.Id == item.StationID)).Name;
                }
                var log = LogsAppService.InsertLogToDatabase(mappedEntity, mappedEntity, LoginedUserService.UserId, "ProductsOperations", LogType.Get, id);
                await _uow.LogsRepository.InsertAsync(log);
                return new SuccessDataResult<SelectProductsOperationsDto>(mappedEntity);
            }
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListProductsOperationsDto>>> GetListAsync(ListProductsOperationsParameterDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var list = await _uow.ProductsOperationsRepository.GetListAsync(null,
                t => t.ProductsOperationLines,
                t => t.Products);

                var mappedEntity = ObjectMapper.Map<List<ProductsOperations>, List<ListProductsOperationsDto>>(list.ToList());

                return new SuccessDataResult<IList<ListProductsOperationsDto>>(mappedEntity);
            }
        }

        [ValidationAspect(typeof(UpdateProductsOperationsValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectProductsOperationsDto>> UpdateAsync(UpdateProductsOperationsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.ProductsOperationsRepository.GetAsync(x => x.Id == input.Id);

                await _manager.UpdateControl(_uow.ProductsOperationsRepository, input.Code, input.Id, entity,L);

                var mappedEntity = ObjectMapper.Map<UpdateProductsOperationsDto, ProductsOperations>(input);

                await _uow.ProductsOperationsRepository.UpdateAsync(mappedEntity);

                foreach (var item in input.SelectProductsOperationLines)
                {
                    var lineEntity = ObjectMapper.Map<SelectProductsOperationLinesDto, ProductsOperationLines>(item);
                    lineEntity.ProductsOperationID = mappedEntity.Id;

                    if (lineEntity.Id == Guid.Empty)
                    {
                        await _uow.ProductsOperationLinesRepository.InsertAsync(lineEntity);
                    }
                    else
                    {
                        await _uow.ProductsOperationLinesRepository.UpdateAsync(lineEntity);
                    }
                }

                var before = ObjectMapper.Map<ProductsOperations, UpdateProductsOperationsDto>(entity);
                before.SelectProductsOperationLines = ObjectMapper.Map<List<ProductsOperationLines>, List<SelectProductsOperationLinesDto>>(entity.ProductsOperationLines.ToList());
                var log = LogsAppService.InsertLogToDatabase(before, input, LoginedUserService.UserId, "ProductsOperations", LogType.Update, mappedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);

                await _uow.SaveChanges();

                return new SuccessDataResult<SelectProductsOperationsDto>(ObjectMapper.Map<ProductsOperations, SelectProductsOperationsDto>(mappedEntity));
            }
        }

        public async Task<IDataResult<SelectProductsOperationsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.ProductsOperationsRepository.GetAsync(x => x.Id == id);

                var updatedEntity = await _uow.ProductsOperationsRepository.LockRow(entity.Id, lockRow, userId);

                await _uow.SaveChanges();

                var mappedEntity = ObjectMapper.Map<ProductsOperations, SelectProductsOperationsDto>(updatedEntity);

                return new SuccessDataResult<SelectProductsOperationsDto>(mappedEntity);
            }
        }
    }
}

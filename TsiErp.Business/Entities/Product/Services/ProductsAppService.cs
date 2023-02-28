using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results; using TsiErp.Localizations.Resources.Branches.Page;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Product.BusinessRules;
using TsiErp.Business.Entities.Product.Validations;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.Product;
using TsiErp.Entities.Entities.Product.Dtos;
using Microsoft.Extensions.Localization;

namespace TsiErp.Business.Entities.Product.Services
{
    [ServiceRegistration(typeof(IProductsAppService), DependencyInjectionType.Scoped)]
    public class ProductsAppService : ApplicationService<BranchesResource>, IProductsAppService
    {
        public ProductsAppService(IStringLocalizer<BranchesResource> l) : base(l)
        {
        }

        ProductManager _manager { get; set; } = new ProductManager();

        [ValidationAspect(typeof(CreateProductsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectProductsDto>> CreateAsync(CreateProductsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.CodeControl(_uow.ProductsRepository, input.Code);

                var entity = ObjectMapper.Map<CreateProductsDto, Products>(input);

                var addedEntity = await _uow.ProductsRepository.InsertAsync(entity);
                input.Id = addedEntity.Id;
                var log = LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, "Products", LogType.Insert, addedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);

                await _uow.SaveChanges();

                return new SuccessDataResult<SelectProductsDto>(ObjectMapper.Map<Products, SelectProductsDto>(addedEntity));
            }
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.DeleteControl(_uow.ProductsRepository, id);
                await _uow.ProductsRepository.DeleteAsync(id);
                var log = LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, "Products", LogType.Delete, id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();
                return new SuccessResult("Silme işlemi başarılı.");
            }
        }


        public async Task<IDataResult<SelectProductsDto>> GetAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.ProductsRepository.GetAsync(t => t.Id == id, t => t.ProductGroups, y => y.UnitSets, y => y.SalesPropositionLines, y => y.BillsofMaterialLines, y => y.BillsofMaterials);
                var mappedEntity = ObjectMapper.Map<Products, SelectProductsDto>(entity);
                var log = LogsAppService.InsertLogToDatabase(mappedEntity, mappedEntity, LoginedUserService.UserId, "Products", LogType.Get, id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();
                return new SuccessDataResult<SelectProductsDto>(mappedEntity);
            }
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListProductsDto>>> GetListAsync(ListProductsParameterDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var list = await _uow.ProductsRepository.GetListAsync(t => t.IsActive == input.IsActive, x => x.ProductGroups, y => y.UnitSets, y => y.SalesPropositionLines, y => y.BillsofMaterialLines, y => y.BillsofMaterials);

                var mappedEntity = ObjectMapper.Map<List<Products>, List<ListProductsDto>>(list.ToList());

                return new SuccessDataResult<IList<ListProductsDto>>(mappedEntity);
            }
        }


        [ValidationAspect(typeof(UpdateProductsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectProductsDto>> UpdateAsync(UpdateProductsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.ProductsRepository.GetAsync(x => x.Id == input.Id);

                await _manager.UpdateControl(_uow.ProductsRepository, input.Code, input.Id, entity);

                var mappedEntity = ObjectMapper.Map<UpdateProductsDto, Products>(input);

                await _uow.ProductsRepository.UpdateAsync(mappedEntity);
                var before = ObjectMapper.Map<Products, UpdateProductsDto>(entity);
                var log = LogsAppService.InsertLogToDatabase(before, input, LoginedUserService.UserId, "Products", LogType.Update, mappedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectProductsDto>(ObjectMapper.Map<Products, SelectProductsDto>(mappedEntity));
            }
        }

        public async Task<IDataResult<SelectProductsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.ProductsRepository.GetAsync(x => x.Id == id);

                var updatedEntity = await _uow.ProductsRepository.LockRow(entity.Id, lockRow, userId);

                await _uow.SaveChanges();

                var mappedEntity = ObjectMapper.Map<Products, SelectProductsDto>(updatedEntity);

                return new SuccessDataResult<SelectProductsDto>(mappedEntity);
            }
        }
    }
}

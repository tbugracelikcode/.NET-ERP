using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results; 
using TsiErp.Localizations.Resources.ProductGroups.Page;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.ProductGroup.BusinessRules;
using TsiErp.Business.Entities.ProductGroup.Validations;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.ProductGroup;
using TsiErp.Entities.Entities.ProductGroup.Dtos;
using Microsoft.Extensions.Localization;

namespace TsiErp.Business.Entities.ProductGroup.Services
{
    [ServiceRegistration(typeof(IProductGroupsAppService), DependencyInjectionType.Scoped)]
    public class ProductGroupsAppService : ApplicationService<ProductGroupsResource>, IProductGroupsAppService
    {
        public ProductGroupsAppService(IStringLocalizer<ProductGroupsResource> l) : base(l)
        {
        }

        ProductGroupManager _manager { get; set; } = new ProductGroupManager();


        [ValidationAspect(typeof(CreateProductGroupsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectProductGroupsDto>> CreateAsync(CreateProductGroupsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.CodeControl(_uow.ProductGroupsRepository, input.Code,L);

                var entity = ObjectMapper.Map<CreateProductGroupsDto, ProductGroups>(input);

                var addedEntity = await _uow.ProductGroupsRepository.InsertAsync(entity);
                input.Id = addedEntity.Id;
                var log = LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, "ProductGroups", LogType.Insert, addedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectProductGroupsDto>(ObjectMapper.Map<ProductGroups, SelectProductGroupsDto>(addedEntity));
            }
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.DeleteControl(_uow.ProductGroupsRepository, id,L);
                await _uow.ProductGroupsRepository.DeleteAsync(id);
                var log = LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, "ProductGroups", LogType.Delete, id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();
                return new SuccessResult(L["DeleteSuccessMessage"]);
            }
        }


        public async Task<IDataResult<SelectProductGroupsDto>> GetAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.ProductGroupsRepository.GetAsync(t => t.Id == id, t => t.Products);
                var mappedEntity = ObjectMapper.Map<ProductGroups, SelectProductGroupsDto>(entity);
                var log = LogsAppService.InsertLogToDatabase(mappedEntity, mappedEntity, LoginedUserService.UserId, "ProductGroups", LogType.Get, id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectProductGroupsDto>(mappedEntity);
            }
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListProductGroupsDto>>> GetListAsync(ListProductGroupsParameterDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var list = await _uow.ProductGroupsRepository.GetListAsync(t => t.IsActive == input.IsActive, x => x.Products);

                var mappedEntity = ObjectMapper.Map<List<ProductGroups>, List<ListProductGroupsDto>>(list.ToList());

                return new SuccessDataResult<IList<ListProductGroupsDto>>(mappedEntity);
            }
        }


        [ValidationAspect(typeof(UpdateProductGroupsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectProductGroupsDto>> UpdateAsync(UpdateProductGroupsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.ProductGroupsRepository.GetAsync(x => x.Id == input.Id);

                await _manager.UpdateControl(_uow.ProductGroupsRepository, input.Code, input.Id, entity,L);

                var mappedEntity = ObjectMapper.Map<UpdateProductGroupsDto, ProductGroups>(input);

                await _uow.ProductGroupsRepository.UpdateAsync(mappedEntity);
                var before = ObjectMapper.Map<ProductGroups, UpdateProductGroupsDto>(entity);
                var log = LogsAppService.InsertLogToDatabase(before, input, LoginedUserService.UserId, "ProductGroups", LogType.Update, mappedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectProductGroupsDto>(ObjectMapper.Map<ProductGroups, SelectProductGroupsDto>(mappedEntity));
            }
        }

        public async Task<IDataResult<SelectProductGroupsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.ProductGroupsRepository.GetAsync(x => x.Id == id);

                var updatedEntity = await _uow.ProductGroupsRepository.LockRow(entity.Id, lockRow, userId);

                await _uow.SaveChanges();

                var mappedEntity = ObjectMapper.Map<ProductGroups, SelectProductGroupsDto>(updatedEntity);

                return new SuccessDataResult<SelectProductGroupsDto>(mappedEntity);
            }
        }
    }
}

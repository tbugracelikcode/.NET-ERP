using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.ProductGroup;
using TsiErp.Business.Entities.ProductGroup.Validations;
using TsiErp.Entities.Entities.ProductGroup.Dtos;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Entities.Entities.ProductGroup;
using Tsi.Core.Services.BusinessCoreServices;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.Entities.ProductGroup.BusinessRules;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.Entities.Entities.WorkOrder.Dtos;
using TsiErp.Entities.Entities.WorkOrder;

namespace TsiErp.Business.Entities.ProductGroup.Services
{
    [ServiceRegistration(typeof(IProductGroupsAppService), DependencyInjectionType.Scoped)]
    public class ProductGroupsAppService : ApplicationService, IProductGroupsAppService
    {

        ProductGroupManager _manager { get; set; } = new ProductGroupManager();


        [ValidationAspect(typeof(CreateProductGroupsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectProductGroupsDto>> CreateAsync(CreateProductGroupsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.CodeControl(_uow.ProductGroupsRepository, input.Code);

                var entity = ObjectMapper.Map<CreateProductGroupsDto, ProductGroups>(input);

                var addedEntity = await _uow.ProductGroupsRepository.InsertAsync(entity);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectProductGroupsDto>(ObjectMapper.Map<ProductGroups, SelectProductGroupsDto>(addedEntity));
            }
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.DeleteControl(_uow.ProductGroupsRepository, id);
                await _uow.ProductGroupsRepository.DeleteAsync(id);
                await _uow.SaveChanges();
                return new SuccessResult("Silme işlemi başarılı.");
            }
        }


        public async Task<IDataResult<SelectProductGroupsDto>> GetAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.ProductGroupsRepository.GetAsync(t => t.Id == id, t => t.Products);
                var mappedEntity = ObjectMapper.Map<ProductGroups, SelectProductGroupsDto>(entity);
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

                await _manager.UpdateControl(_uow.ProductGroupsRepository, input.Code, input.Id, entity);

                var mappedEntity = ObjectMapper.Map<UpdateProductGroupsDto, ProductGroups>(input);

                await _uow.ProductGroupsRepository.UpdateAsync(mappedEntity);
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

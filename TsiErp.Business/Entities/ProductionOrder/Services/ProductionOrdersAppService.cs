using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.Entities.ProductionOrder.BusinessRules;
using TsiErp.Business.Entities.ProductionOrder.Validations;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.ProductionOrder;
using TsiErp.Entities.Entities.ProductionOrder;
using TsiErp.Entities.Entities.ProductionOrder.Dtos;
using TsiErp.Entities.Entities.WorkOrder.Dtos;
using TsiErp.Entities.Entities.WorkOrder;

namespace TsiErp.Business.Entities.ProductionOrder.Services
{
    [ServiceRegistration(typeof(IProductionOrdersAppService), DependencyInjectionType.Scoped)]
    public class ProductionOrdersAppService : IProductionOrdersAppService
    {
        ProductionOrderManager _manager { get; set; } = new ProductionOrderManager();


        [ValidationAspect(typeof(CreateProductionOrdersValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectProductionOrdersDto>> CreateAsync(CreateProductionOrdersDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.CodeControl(_uow.ProductionOrdersRepository, input.FicheNo);

                var entity = ObjectMapper.Map<CreateProductionOrdersDto, ProductionOrders>(input);

                var addedEntity = await _uow.ProductionOrdersRepository.InsertAsync(entity);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectProductionOrdersDto>(ObjectMapper.Map<ProductionOrders, SelectProductionOrdersDto>(addedEntity));
            }
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _uow.ProductionOrdersRepository.DeleteAsync(id);
                await _uow.SaveChanges();
                return new SuccessResult("Silme işlemi başarılı.");
            }
        }

        public async Task<IDataResult<SelectProductionOrdersDto>> GetAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.ProductionOrdersRepository.GetAsync(t => t.Id == id,
                t => t.SalesOrders,
                t => t.SalesOrderLines,
                t => t.Products,
                t => t.UnitSets,
                t => t.BillsofMaterials,
                t => t.Routes,
                t => t.SalesPropositions,
                t => t.SalesPropositionLines,
                t => t.CurrentAccountCards);
                var mappedEntity = ObjectMapper.Map<ProductionOrders, SelectProductionOrdersDto>(entity);
                return new SuccessDataResult<SelectProductionOrdersDto>(mappedEntity);
            }
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListProductionOrdersDto>>> GetListAsync(ListProductionOrdersParameterDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var list = await _uow.ProductionOrdersRepository.GetListAsync(null,
                t => t.SalesOrders,
                t => t.SalesOrderLines,
                t => t.Products,
                t => t.UnitSets,
                t => t.BillsofMaterials,
                t => t.Routes,
                t => t.SalesPropositions,
                t => t.SalesPropositionLines,
                t => t.CurrentAccountCards);

                var mappedEntity = ObjectMapper.Map<List<ProductionOrders>, List<ListProductionOrdersDto>>(list.ToList());

                return new SuccessDataResult<IList<ListProductionOrdersDto>>(mappedEntity);
            }
        }

        [ValidationAspect(typeof(UpdateProductionOrdersValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectProductionOrdersDto>> UpdateAsync(UpdateProductionOrdersDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.ProductionOrdersRepository.GetAsync(x => x.Id == input.Id);

                await _manager.UpdateControl(_uow.ProductionOrdersRepository, input.FicheNo, input.Id, entity);

                var mappedEntity = ObjectMapper.Map<UpdateProductionOrdersDto, ProductionOrders>(input);

                await _uow.ProductionOrdersRepository.UpdateAsync(mappedEntity);
                await _uow.SaveChanges();
                return new SuccessDataResult<SelectProductionOrdersDto>(ObjectMapper.Map<ProductionOrders, SelectProductionOrdersDto>(mappedEntity));
            }
        }

        public async Task<IDataResult<SelectProductionOrdersDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.ProductionOrdersRepository.GetAsync(x => x.Id == id);

                var updatedEntity = await _uow.ProductionOrdersRepository.LockRow(entity.Id, lockRow, userId);

                await _uow.SaveChanges();

                var mappedEntity = ObjectMapper.Map<ProductionOrders, SelectProductionOrdersDto>(updatedEntity);

                return new SuccessDataResult<SelectProductionOrdersDto>(mappedEntity);
            }
        }
    }
}

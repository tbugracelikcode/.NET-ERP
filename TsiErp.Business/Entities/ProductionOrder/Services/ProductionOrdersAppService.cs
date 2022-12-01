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
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.ProductionOrder;
using TsiErp.Entities.Entities.ProductionOrder;
using TsiErp.Entities.Entities.ProductionOrder.Dtos;

namespace TsiErp.Business.Entities.ProductionOrder.Services
{
    [ServiceRegistration(typeof(IProductionOrdersAppService), DependencyInjectionType.Scoped)]
    public class ProductionOrdersAppService : IProductionOrdersAppService
    {
        private readonly IProductionOrdersRepository _repository;

        ProductionOrderManager _manager { get; set; } = new ProductionOrderManager();

        public ProductionOrdersAppService(IProductionOrdersRepository repository)
        {
            _repository = repository;
        }


        [ValidationAspect(typeof(CreateProductionOrdersValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectProductionOrdersDto>> CreateAsync(CreateProductionOrdersDto input)
        {
            await _manager.CodeControl(_repository, input.FicheNo);

            var entity = ObjectMapper.Map<CreateProductionOrdersDto, ProductionOrders>(input);

            var addedEntity = await _repository.InsertAsync(entity);
            await _repository.SaveChanges();

            return new SuccessDataResult<SelectProductionOrdersDto>(ObjectMapper.Map<ProductionOrders, SelectProductionOrdersDto>(addedEntity));
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
            await _repository.SaveChanges();
            return new SuccessResult("Silme işlemi başarılı.");
        }

        public async Task<IDataResult<SelectProductionOrdersDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id,
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

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListProductionOrdersDto>>> GetListAsync(ListProductionOrdersParameterDto input)
        {
            var list = await _repository.GetListAsync(null,
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

        [ValidationAspect(typeof(UpdateProductionOrdersValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectProductionOrdersDto>> UpdateAsync(UpdateProductionOrdersDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            await _manager.UpdateControl(_repository, input.FicheNo, input.Id, entity);

            var mappedEntity = ObjectMapper.Map<UpdateProductionOrdersDto, ProductionOrders>(input);

            await _repository.UpdateAsync(mappedEntity);
            await _repository.SaveChanges();
            return new SuccessDataResult<SelectProductionOrdersDto>(ObjectMapper.Map<ProductionOrders, SelectProductionOrdersDto>(mappedEntity));
        }
    }
}

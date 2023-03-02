using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using TsiErp.Localizations.Resources.ProductionOrders.Page;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.ProductionOrder.BusinessRules;
using TsiErp.Business.Entities.ProductionOrder.Validations;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.ProductionOrder;
using TsiErp.Entities.Entities.ProductionOrder.Dtos;
using TsiErp.Business.BusinessCoreServices;
using Microsoft.Extensions.Localization;

namespace TsiErp.Business.Entities.ProductionOrder.Services
{
    [ServiceRegistration(typeof(IProductionOrdersAppService), DependencyInjectionType.Scoped)]
    public class ProductionOrdersAppService :ApplicationService<ProductionOrdersResource>, IProductionOrdersAppService
    {
        public ProductionOrdersAppService(IStringLocalizer<ProductionOrdersResource> l) : base(l)
        {
        }

        ProductionOrderManager _manager { get; set; } = new ProductionOrderManager();


        [ValidationAspect(typeof(CreateProductionOrdersValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectProductionOrdersDto>> CreateAsync(CreateProductionOrdersDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.CodeControl(_uow.ProductionOrdersRepository, input.FicheNo,L);

                var entity = ObjectMapper.Map<CreateProductionOrdersDto, ProductionOrders>(input);

                var addedEntity = await _uow.ProductionOrdersRepository.InsertAsync(entity);
                input.Id = addedEntity.Id;
                var log = LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, "ProductionOrders", LogType.Insert, addedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);
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
                var log = LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, "ProductionOrders", LogType.Delete, id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();
                return new SuccessResult(L["DeleteSuccessMessage"]);
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
                var log = LogsAppService.InsertLogToDatabase(mappedEntity, mappedEntity, LoginedUserService.UserId, "ProductionOrders", LogType.Get, id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();
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

                await _manager.UpdateControl(_uow.ProductionOrdersRepository, input.FicheNo, input.Id, entity,L);

                var mappedEntity = ObjectMapper.Map<UpdateProductionOrdersDto, ProductionOrders>(input);

                await _uow.ProductionOrdersRepository.UpdateAsync(mappedEntity);
                var before = ObjectMapper.Map<ProductionOrders, UpdateProductionOrdersDto>(entity);
                var log = LogsAppService.InsertLogToDatabase(before, input, LoginedUserService.UserId, "ProductionOrders", LogType.Update, mappedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);

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

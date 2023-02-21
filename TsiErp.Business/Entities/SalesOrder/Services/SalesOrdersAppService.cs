using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.SalesOrder.BusinessRules;
using TsiErp.Business.Entities.SalesOrder.Validations;
using TsiErp.Business.Entities.SalesProposition.Services;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.Entities.Entities.SalesOrder;
using TsiErp.Entities.Entities.SalesOrder.Dtos;
using TsiErp.Entities.Entities.SalesOrderLine;
using TsiErp.Entities.Entities.SalesOrderLine.Dtos;

namespace TsiErp.Business.Entities.SalesOrder.Services
{
    [ServiceRegistration(typeof(ISalesOrdersAppService), DependencyInjectionType.Scoped)]
    public class SalesOrdersAppService : ApplicationService, ISalesOrdersAppService
    {
        private readonly ISalesPropositionsAppService _salesPropositionsAppService;

        SalesOrderManager _manager { get; set; } = new SalesOrderManager();
        public SalesOrdersAppService(ISalesPropositionsAppService salesPropositionsAppService)
        {
            _salesPropositionsAppService = salesPropositionsAppService;
        }


        [ValidationAspect(typeof(CreateSalesOrderValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectSalesOrderDto>> CreateAsync(CreateSalesOrderDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.CodeControl(_uow.SalesOrdersRepository, input.FicheNo);

                var entity = ObjectMapper.Map<CreateSalesOrderDto, SalesOrders>(input);

                var addedEntity = await _uow.SalesOrdersRepository.InsertAsync(entity);

                foreach (var item in input.SelectSalesOrderLines)
                {
                    var lineEntity = ObjectMapper.Map<SelectSalesOrderLinesDto, SalesOrderLines>(item);
                    lineEntity.Id = GuidGenerator.CreateGuid();
                    lineEntity.SalesOrderID = addedEntity.Id;
                    await _uow.SalesOrderLinesRepository.InsertAsync(lineEntity);
                }

                await _uow.SaveChanges();
                return new SuccessDataResult<SelectSalesOrderDto>(ObjectMapper.Map<SalesOrders, SelectSalesOrderDto>(addedEntity));
            }
        }

        [ValidationAspect(typeof(CreateSalesOrderValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectSalesOrderDto>> ConvertToSalesOrderAsync(CreateSalesOrderDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.CodeControl(_uow.SalesOrdersRepository, input.FicheNo);

                var entity = ObjectMapper.Map<CreateSalesOrderDto, SalesOrders>(input);

                var addedEntity = await _uow.SalesOrdersRepository.InsertAsync(entity);

                if (input.SelectSalesOrderLines != null)
                {
                    foreach (var item in input.SelectSalesOrderLines)
                    {
                        var lineEntity = ObjectMapper.Map<SelectSalesOrderLinesDto, SalesOrderLines>(item);
                        lineEntity.Id = GuidGenerator.CreateGuid();
                        lineEntity.SalesOrderID = addedEntity.Id;
                        await _uow.SalesOrderLinesRepository.InsertAsync(lineEntity);

                    }

                    await _salesPropositionsAppService.UpdateSalesPropositionLineState(input.SelectSalesOrderLines, TsiErp.Entities.Enums.SalesPropositionLineStateEnum.Siparis);
                }



                await _uow.SaveChanges();
                return new SuccessDataResult<SelectSalesOrderDto>(ObjectMapper.Map<SalesOrders, SelectSalesOrderDto>(addedEntity));
            }
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var lines = (await _uow.SalesOrderLinesRepository.GetAsync(t => t.Id == id));

                if (lines != null)
                {
                    await _manager.DeleteControl(_uow.SalesOrdersRepository, lines.SalesOrderID, lines.Id, true);
                    await _uow.SalesOrderLinesRepository.DeleteAsync(id);
                    await _uow.SaveChanges();
                    return new SuccessResult("Silme işlemi başarılı.");
                }
                else
                {
                    await _manager.DeleteControl(_uow.SalesOrdersRepository, id, Guid.Empty, false);
                    await _uow.SalesOrdersRepository.DeleteAsync(id);
                    await _uow.SaveChanges();
                    return new SuccessResult("Silme işlemi başarılı.");
                }
            }
        }

        public async Task<IDataResult<SelectSalesOrderDto>> GetAsync(Guid id)
            {
                using (UnitOfWork _uow = new UnitOfWork())
                {
                    var entity = await _uow.SalesOrdersRepository.GetAsync(t => t.Id == id,
                    t => t.SalesOrderLines,
                    t => t.CurrentAccountCards,
                    t => t.Warehouses,
                    t => t.Branches,
                    t => t.Currencies,
                    t => t.PaymentPlan);

                var mappedEntity = ObjectMapper.Map<SalesOrders, SelectSalesOrderDto>(entity);

                mappedEntity.SelectSalesOrderLines = ObjectMapper.Map<List<SalesOrderLines>, List<SelectSalesOrderLinesDto>>(entity.SalesOrderLines.ToList());

                return new SuccessDataResult<SelectSalesOrderDto>(mappedEntity);
            }
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListSalesOrderDto>>> GetListAsync(ListSalesOrderParameterDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var list = await _uow.SalesOrdersRepository.GetListAsync(null,
                t => t.SalesOrderLines,
                t => t.CurrentAccountCards,
                t => t.Warehouses,
                t => t.Branches,
                t => t.Currencies,
                t => t.PaymentPlan);

                var mappedEntity = ObjectMapper.Map<List<SalesOrders>, List<ListSalesOrderDto>>(list.ToList());

                return new SuccessDataResult<IList<ListSalesOrderDto>>(mappedEntity);
            }
        }

        [ValidationAspect(typeof(UpdateSalesOrderValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectSalesOrderDto>> UpdateAsync(UpdateSalesOrderDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.SalesOrdersRepository.GetAsync(x => x.Id == input.Id);

                await _manager.UpdateControl(_uow.SalesOrdersRepository, input.FicheNo, input.Id, entity);

                var mappedEntity = ObjectMapper.Map<UpdateSalesOrderDto, SalesOrders>(input);

                await _uow.SalesOrdersRepository.UpdateAsync(mappedEntity);

                foreach (var item in input.SelectSalesOrderLines)
                {
                    var lineEntity = ObjectMapper.Map<SelectSalesOrderLinesDto, SalesOrderLines>(item);
                    lineEntity.SalesOrderID = mappedEntity.Id;

                    if (lineEntity.Id == Guid.Empty)
                    {
                        lineEntity.Id = GuidGenerator.CreateGuid();
                        await _uow.SalesOrderLinesRepository.InsertAsync(lineEntity);
                    }
                    else
                    {
                        await _uow.SalesOrderLinesRepository.UpdateAsync(lineEntity);
                    }
                }

                await _uow.SaveChanges();

                return new SuccessDataResult<SelectSalesOrderDto>(ObjectMapper.Map<SalesOrders, SelectSalesOrderDto>(mappedEntity));
            }
        }

        public async Task<IDataResult<SelectSalesOrderDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.SalesOrdersRepository.GetAsync(x => x.Id == id);

                var updatedEntity = await _uow.SalesOrdersRepository.LockRow(entity.Id, lockRow, userId);

                await _uow.SaveChanges();

                var mappedEntity = ObjectMapper.Map<SalesOrders, SelectSalesOrderDto>(updatedEntity);

                return new SuccessDataResult<SelectSalesOrderDto>(mappedEntity);
            }
        }
    }
}

using Tsi.Core.Services.BusinessCoreServices;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.Entities.PurchaseOrder.BusinessRules;
using TsiErp.Business.Entities.PurchaseOrder.Validations;
using TsiErp.Business.Entities.PurchaseRequest.Services;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.PurchaseOrder;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.PurchaseOrderLine;
using TsiErp.Entities.Entities.PurchaseOrder;
using TsiErp.Entities.Entities.PurchaseOrder.Dtos;
using TsiErp.Entities.Entities.PurchaseOrderLine;
using TsiErp.Entities.Entities.PurchaseOrderLine.Dtos;
using TsiErp.Entities.Entities.WorkOrder.Dtos;
using TsiErp.Entities.Entities.WorkOrder;

namespace TsiErp.Business.Entities.PurchaseOrder.Services
{
    [ServiceRegistration(typeof(IPurchaseOrdersAppService), DependencyInjectionType.Scoped)]
    public class PurchaseOrdersAppService : ApplicationService, IPurchaseOrdersAppService
    {
        private readonly IPurchaseRequestsAppService _PurchaseRequestsAppService;

        PurchaseOrderManager _manager { get; set; } = new PurchaseOrderManager();
        public PurchaseOrdersAppService(IPurchaseRequestsAppService PurchaseRequestsAppService)
        {
            _PurchaseRequestsAppService = PurchaseRequestsAppService;
        }


        [ValidationAspect(typeof(CreatePurchaseOrdersValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPurchaseOrdersDto>> CreateAsync(CreatePurchaseOrdersDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.CodeControl(_uow.PurchaseOrdersRepository, input.FicheNo);

                var entity = ObjectMapper.Map<CreatePurchaseOrdersDto, PurchaseOrders>(input);

                var addedEntity = await _uow.PurchaseOrdersRepository.InsertAsync(entity);

                foreach (var item in input.SelectPurchaseOrderLinesDto)
                {
                    var lineEntity = ObjectMapper.Map<SelectPurchaseOrderLinesDto, PurchaseOrderLines>(item);
                    lineEntity.Id = GuidGenerator.CreateGuid();
                    lineEntity.PurchaseOrderID = addedEntity.Id;
                    await _uow.PurchaseOrderLinesRepository.InsertAsync(lineEntity);
                }

                await _uow.SaveChanges();
                return new SuccessDataResult<SelectPurchaseOrdersDto>(ObjectMapper.Map<PurchaseOrders, SelectPurchaseOrdersDto>(addedEntity));
            }
        }

        [ValidationAspect(typeof(CreatePurchaseOrdersValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPurchaseOrdersDto>> ConvertToPurchaseOrderAsync(CreatePurchaseOrdersDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.CodeControl(_uow.PurchaseOrdersRepository, input.FicheNo);

                var entity = ObjectMapper.Map<CreatePurchaseOrdersDto, PurchaseOrders>(input);

                var addedEntity = await _uow.PurchaseOrdersRepository.InsertAsync(entity);

                if (input.SelectPurchaseOrderLinesDto != null)
                {
                    foreach (var item in input.SelectPurchaseOrderLinesDto)
                    {
                        var lineEntity = ObjectMapper.Map<SelectPurchaseOrderLinesDto, PurchaseOrderLines>(item);
                        lineEntity.Id = GuidGenerator.CreateGuid();
                        lineEntity.PurchaseOrderID = addedEntity.Id;
                        await _uow.PurchaseOrderLinesRepository.InsertAsync(lineEntity);

                    }

                    await _PurchaseRequestsAppService.UpdatePurchaseRequestLineState(input.SelectPurchaseOrderLinesDto, TsiErp.Entities.Enums.PurchaseRequestLineStateEnum.SatinAlma);
                }



                await _uow.SaveChanges();
                return new SuccessDataResult<SelectPurchaseOrdersDto>(ObjectMapper.Map<PurchaseOrders, SelectPurchaseOrdersDto>(addedEntity));
            }
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var lines = (await _uow.PurchaseOrderLinesRepository.GetAsync(t => t.Id == id));

                if (lines != null)
                {
                    await _manager.DeleteControl(_uow.PurchaseOrdersRepository, lines.PurchaseOrderID, lines.Id, true);
                    await _uow.PurchaseOrderLinesRepository.DeleteAsync(id);
                    await _uow.SaveChanges();
                    return new SuccessResult("Silme işlemi başarılı.");
                }
                else
                {
                    await _manager.DeleteControl(_uow.PurchaseOrdersRepository, id, Guid.Empty, false);
                    await _uow.PurchaseOrdersRepository.DeleteAsync(id);
                    await _uow.SaveChanges();
                    return new SuccessResult("Silme işlemi başarılı.");
                }
            }
        }

        public async Task<IDataResult<SelectPurchaseOrdersDto>> GetAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.PurchaseOrdersRepository.GetAsync(t => t.Id == id,
                t => t.PurchaseOrderLines,
                t => t.CurrentAccountCards,
                t => t.Warehouses,
                t => t.Branches,
                t => t.Currencies,
                t => t.PaymentPlan);

                var mappedEntity = ObjectMapper.Map<PurchaseOrders, SelectPurchaseOrdersDto>(entity);

                mappedEntity.SelectPurchaseOrderLinesDto = ObjectMapper.Map<List<PurchaseOrderLines>, List<SelectPurchaseOrderLinesDto>>(entity.PurchaseOrderLines.ToList());

                return new SuccessDataResult<SelectPurchaseOrdersDto>(mappedEntity);
            }
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListPurchaseOrdersDto>>> GetListAsync(ListPurchaseOrdersParameterDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var list = await _uow.PurchaseOrdersRepository.GetListAsync(null,
                t => t.PurchaseOrderLines,
                t => t.CurrentAccountCards,
                t => t.Warehouses,
                t => t.Branches,
                t => t.Currencies,
                t => t.PaymentPlan);

                var mappedEntity = ObjectMapper.Map<List<PurchaseOrders>, List<ListPurchaseOrdersDto>>(list.ToList());

                return new SuccessDataResult<IList<ListPurchaseOrdersDto>>(mappedEntity);
            }
        }

        [ValidationAspect(typeof(UpdatePurchaseOrdersValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPurchaseOrdersDto>> UpdateAsync(UpdatePurchaseOrdersDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.PurchaseOrdersRepository.GetAsync(x => x.Id == input.Id);

                await _manager.UpdateControl(_uow.PurchaseOrdersRepository, input.FicheNo, input.Id, entity);

                var mappedEntity = ObjectMapper.Map<UpdatePurchaseOrdersDto, PurchaseOrders>(input);

                await _uow.PurchaseOrdersRepository.UpdateAsync(mappedEntity);

                foreach (var item in input.SelectPurchaseOrderLinesDto)
                {
                    var lineEntity = ObjectMapper.Map<SelectPurchaseOrderLinesDto, PurchaseOrderLines>(item);
                    lineEntity.PurchaseOrderID = mappedEntity.Id;

                    if (lineEntity.Id == Guid.Empty)
                    {
                        lineEntity.Id = GuidGenerator.CreateGuid();
                        await _uow.PurchaseOrderLinesRepository.InsertAsync(lineEntity);
                    }
                    else
                    {
                        await _uow.PurchaseOrderLinesRepository.UpdateAsync(lineEntity);
                    }
                }

                await _uow.SaveChanges();

                return new SuccessDataResult<SelectPurchaseOrdersDto>(ObjectMapper.Map<PurchaseOrders, SelectPurchaseOrdersDto>(mappedEntity));
            }
        }

        public async Task<IDataResult<SelectPurchaseOrdersDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.PurchaseOrdersRepository.GetAsync(x => x.Id == id);

                var updatedEntity = await _uow.PurchaseOrdersRepository.LockRow(entity.Id, lockRow, userId);

                await _uow.SaveChanges();

                var mappedEntity = ObjectMapper.Map<PurchaseOrders, SelectPurchaseOrdersDto>(updatedEntity);

                return new SuccessDataResult<SelectPurchaseOrdersDto>(mappedEntity);
            }
        }
    }
}

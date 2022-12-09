using Tsi.Application.Contract.Services.EntityFrameworkCore;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.Entities.PurchaseOrder.BusinessRules;
using TsiErp.Business.Entities.PurchaseOrder.Validations;
using TsiErp.Business.Entities.PurchaseRequest.Services;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.PurchaseOrder;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.PurchaseOrderLine;
using TsiErp.Entities.Entities.PurchaseOrder;
using TsiErp.Entities.Entities.PurchaseOrder.Dtos;
using TsiErp.Entities.Entities.PurchaseOrderLine;
using TsiErp.Entities.Entities.PurchaseOrderLine.Dtos;

namespace TsiErp.Business.Entities.PurchaseOrder.Services
{
    [ServiceRegistration(typeof(IPurchaseOrdersAppService), DependencyInjectionType.Scoped)]
    public class PurchaseOrdersAppService : ApplicationService, IPurchaseOrdersAppService
    {
        private readonly IPurchaseOrdersRepository _repository;
        private readonly IPurchaseOrderLinesRepository _lineRepository;

        private readonly IPurchaseRequestsAppService _PurchaseRequestsAppService;

        PurchaseOrderManager _manager { get; set; } = new PurchaseOrderManager();
        public PurchaseOrdersAppService(IPurchaseOrdersRepository repository, IPurchaseOrderLinesRepository lineRepository, IPurchaseRequestsAppService PurchaseRequestsAppService)
        {
            _repository = repository;
            _lineRepository = lineRepository;
            _PurchaseRequestsAppService = PurchaseRequestsAppService;
        }


        [ValidationAspect(typeof(CreatePurchaseOrdersValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPurchaseOrdersDto>> CreateAsync(CreatePurchaseOrdersDto input)
        {
            await _manager.CodeControl(_repository, input.FicheNo);

            var entity = ObjectMapper.Map<CreatePurchaseOrdersDto, PurchaseOrders>(input);

            var addedEntity = await _repository.InsertAsync(entity);

            foreach (var item in input.SelectPurchaseOrderLinesDto)
            {
                var lineEntity = ObjectMapper.Map<SelectPurchaseOrderLinesDto, PurchaseOrderLines>(item);
                lineEntity.Id = GuidGenerator.CreateGuid();
                lineEntity.PurchaseOrderID = addedEntity.Id;
                await _lineRepository.InsertAsync(lineEntity);
            }

            await _repository.SaveChanges();
            await _lineRepository.SaveChanges();
            return new SuccessDataResult<SelectPurchaseOrdersDto>(ObjectMapper.Map<PurchaseOrders, SelectPurchaseOrdersDto>(addedEntity));
        }

        [ValidationAspect(typeof(CreatePurchaseOrdersValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPurchaseOrdersDto>> ConvertToPurchaseOrderAsync(CreatePurchaseOrdersDto input)
        {
            await _manager.CodeControl(_repository, input.FicheNo);

            var entity = ObjectMapper.Map<CreatePurchaseOrdersDto, PurchaseOrders>(input);

            var addedEntity = await _repository.InsertAsync(entity);

            if (input.SelectPurchaseOrderLinesDto != null)
            {
                foreach (var item in input.SelectPurchaseOrderLinesDto)
                {
                    var lineEntity = ObjectMapper.Map<SelectPurchaseOrderLinesDto, PurchaseOrderLines>(item);
                    lineEntity.Id = GuidGenerator.CreateGuid();
                    lineEntity.PurchaseOrderID = addedEntity.Id;
                    await _lineRepository.InsertAsync(lineEntity);

                }

                await _PurchaseRequestsAppService.UpdatePurchaseRequestLineState(input.SelectPurchaseOrderLinesDto, TsiErp.Entities.Enums.PurchaseRequestLineStateEnum.SatinAlma);
            }



            await _repository.SaveChanges();
            await _lineRepository.SaveChanges();
            return new SuccessDataResult<SelectPurchaseOrdersDto>(ObjectMapper.Map<PurchaseOrders, SelectPurchaseOrdersDto>(addedEntity));
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var lines = (await _lineRepository.GetAsync(t => t.Id == id));

            if (lines != null)
            {
                await _manager.DeleteControl(_repository, lines.PurchaseOrderID, lines.Id, true);
                await _lineRepository.DeleteAsync(id);
                await _repository.SaveChanges();
                await _lineRepository.SaveChanges();
                return new SuccessResult("Silme işlemi başarılı.");
            }
            else
            {
                await _manager.DeleteControl(_repository, id, Guid.Empty, false);
                await _repository.DeleteAsync(id);
                await _repository.SaveChanges();
                await _lineRepository.SaveChanges();
                return new SuccessResult("Silme işlemi başarılı.");
            }
        }

        public async Task<IDataResult<SelectPurchaseOrdersDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id,
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

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListPurchaseOrdersDto>>> GetListAsync(ListPurchaseOrdersParameterDto input)
        {
            var list = await _repository.GetListAsync(null,
                t => t.PurchaseOrderLines,
                t => t.CurrentAccountCards,
                t => t.Warehouses,
                t => t.Branches,
                t => t.Currencies,
                t => t.PaymentPlan);

            var mappedEntity = ObjectMapper.Map<List<PurchaseOrders>, List<ListPurchaseOrdersDto>>(list.ToList());

            return new SuccessDataResult<IList<ListPurchaseOrdersDto>>(mappedEntity);
        }

        [ValidationAspect(typeof(UpdatePurchaseOrdersValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPurchaseOrdersDto>> UpdateAsync(UpdatePurchaseOrdersDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            await _manager.UpdateControl(_repository, input.FicheNo, input.Id, entity);

            var mappedEntity = ObjectMapper.Map<UpdatePurchaseOrdersDto, PurchaseOrders>(input);

            await _repository.UpdateAsync(mappedEntity);

            foreach (var item in input.SelectPurchaseOrderLinesDto)
            {
                var lineEntity = ObjectMapper.Map<SelectPurchaseOrderLinesDto, PurchaseOrderLines>(item);
                lineEntity.PurchaseOrderID = mappedEntity.Id;

                if (lineEntity.Id == Guid.Empty)
                {
                    lineEntity.Id = GuidGenerator.CreateGuid();
                    await _lineRepository.InsertAsync(lineEntity);
                }
                else
                {
                    await _lineRepository.UpdateAsync(lineEntity);
                }
            }

            await _repository.SaveChanges();
            await _lineRepository.SaveChanges();

            return new SuccessDataResult<SelectPurchaseOrdersDto>(ObjectMapper.Map<PurchaseOrders, SelectPurchaseOrdersDto>(mappedEntity));
        }
    }
}

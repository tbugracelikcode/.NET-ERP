using Tsi.Application.Contract.Services.EntityFrameworkCore;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.Entities.SalesOrder.BusinessRules;
using TsiErp.Business.Entities.SalesOrder.Validations;
using TsiErp.Business.Entities.SalesProposition.Services;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.SalesOrder;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.SalesOrderLine;
using TsiErp.Entities.Entities.SalesOrder;
using TsiErp.Entities.Entities.SalesOrder.Dtos;
using TsiErp.Entities.Entities.SalesOrderLine;
using TsiErp.Entities.Entities.SalesOrderLine.Dtos;

namespace TsiErp.Business.Entities.SalesOrder.Services
{
    [ServiceRegistration(typeof(ISalesOrdersAppService), DependencyInjectionType.Scoped)]
    public class SalesOrdersAppService : ApplicationService, ISalesOrdersAppService
    {
        private readonly ISalesOrdersRepository _repository;
        private readonly ISalesOrderLinesRepository _lineRepository;

        private readonly ISalesPropositionsAppService _salesPropositionsAppService;

        SalesOrderManager _manager { get; set; } = new SalesOrderManager();
        public SalesOrdersAppService(ISalesOrdersRepository repository, ISalesOrderLinesRepository lineRepository, ISalesPropositionsAppService salesPropositionsAppService)
        {
            _repository = repository;
            _lineRepository = lineRepository;
            _salesPropositionsAppService = salesPropositionsAppService;
        }


        [ValidationAspect(typeof(CreateSalesOrderValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectSalesOrderDto>> CreateAsync(CreateSalesOrderDto input)
        {
            await _manager.CodeControl(_repository, input.FicheNo);

            var entity = ObjectMapper.Map<CreateSalesOrderDto, SalesOrders>(input);

            var addedEntity = await _repository.InsertAsync(entity);

            foreach (var item in input.SelectSalesOrderLines)
            {
                var lineEntity = ObjectMapper.Map<SelectSalesOrderLinesDto, SalesOrderLines>(item);
                lineEntity.Id = GuidGenerator.CreateGuid();
                lineEntity.SalesOrderID = addedEntity.Id;
                await _lineRepository.InsertAsync(lineEntity);
            }

            await _repository.SaveChanges();
            await _lineRepository.SaveChanges();
            return new SuccessDataResult<SelectSalesOrderDto>(ObjectMapper.Map<SalesOrders, SelectSalesOrderDto>(addedEntity));
        }

        [ValidationAspect(typeof(CreateSalesOrderValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectSalesOrderDto>> ConvertToSalesOrderAsync(CreateSalesOrderDto input)
        {
            await _manager.CodeControl(_repository, input.FicheNo);

            var entity = ObjectMapper.Map<CreateSalesOrderDto, SalesOrders>(input);

            var addedEntity = await _repository.InsertAsync(entity);

            if(input.SelectSalesOrderLines != null)
            {
                foreach (var item in input.SelectSalesOrderLines)
                {
                    var lineEntity = ObjectMapper.Map<SelectSalesOrderLinesDto, SalesOrderLines>(item);
                    lineEntity.Id = GuidGenerator.CreateGuid();
                    lineEntity.SalesOrderID = addedEntity.Id;
                    await _lineRepository.InsertAsync(lineEntity);

                }

                await _salesPropositionsAppService.UpdateSalesPropositionLineState(input.SelectSalesOrderLines, TsiErp.Entities.Enums.SalesPropositionLineStateEnum.Siparis);
            }



            await _repository.SaveChanges();
            await _lineRepository.SaveChanges();
            return new SuccessDataResult<SelectSalesOrderDto>(ObjectMapper.Map<SalesOrders, SelectSalesOrderDto>(addedEntity));
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var lines = (await _lineRepository.GetAsync(t => t.Id == id));

            if (lines != null)
            {
                await _manager.DeleteControl(_repository, lines.SalesOrderID, lines.Id, true);
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

        public async Task<IDataResult<SelectSalesOrderDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id,
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

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListSalesOrderDto>>> GetListAsync(ListSalesOrderParameterDto input)
        {
            var list = await _repository.GetListAsync(null,
                t => t.SalesOrderLines,
                t => t.CurrentAccountCards,
                t => t.Warehouses,
                t => t.Branches,
                t => t.Currencies,
                t => t.PaymentPlan);

            var mappedEntity = ObjectMapper.Map<List<SalesOrders>, List<ListSalesOrderDto>>(list.ToList());

            return new SuccessDataResult<IList<ListSalesOrderDto>>(mappedEntity);
        }

        [ValidationAspect(typeof(UpdateSalesOrderValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectSalesOrderDto>> UpdateAsync(UpdateSalesOrderDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            await _manager.UpdateControl(_repository, input.FicheNo, input.Id, entity);

            var mappedEntity = ObjectMapper.Map<UpdateSalesOrderDto, SalesOrders>(input);

            await _repository.UpdateAsync(mappedEntity);

            foreach (var item in input.SelectSalesOrderLines)
            {
                var lineEntity = ObjectMapper.Map<SelectSalesOrderLinesDto, SalesOrderLines>(item);
                lineEntity.SalesOrderID = mappedEntity.Id;

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

            return new SuccessDataResult<SelectSalesOrderDto>(ObjectMapper.Map<SalesOrders, SelectSalesOrderDto>(mappedEntity));
        }
    }
}

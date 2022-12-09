using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.Entities.Period.Services;
using TsiErp.Business.Entities.Period.Validations;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.PurchaseRequest;
using TsiErp.Entities.Entities.Period.Dtos;
using TsiErp.Entities.Entities.Period;
using TsiErp.Entities.Entities.PurchaseRequest.Dtos;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Entities.Entities.PurchaseRequest;
using TsiErp.Business.Entities.PurchaseRequest.Validations;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.PurchaseRequestLine;
using TsiErp.Entities.Entities.PurchaseRequestLine.Dtos;
using TsiErp.Entities.Entities.PurchaseRequestLine;
using Tsi.Core.Entities;
using TsiErp.Business.Entities.PurchaseRequest.BusinessRules;
using Microsoft.EntityFrameworkCore;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using Tsi.Core.Utilities.Guids;
using TsiErp.Entities.Enums;
using TsiErp.Entities.Entities.PurchaseOrderLine.Dtos;

namespace TsiErp.Business.Entities.PurchaseRequest.Services
{
    [ServiceRegistration(typeof(IPurchaseRequestsAppService), DependencyInjectionType.Scoped)]
    public class PurchaseRequestsAppService : ApplicationService, IPurchaseRequestsAppService
    {
        private readonly IPurchaseRequestsRepository _repository;
        private readonly IPurchaseRequestLinesRepository _lineRepository;

        PurchaseRequestManager _manager { get; set; } = new PurchaseRequestManager();

        public PurchaseRequestsAppService(IPurchaseRequestsRepository repository, IPurchaseRequestLinesRepository lineRepository)
        {
            _repository = repository;
            _lineRepository = lineRepository;
        }

        [ValidationAspect(typeof(CreatePurchaseRequestsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPurchaseRequestsDto>> CreateAsync(CreatePurchaseRequestsDto input)
        {
            await _manager.CodeControl(_repository, input.FicheNo);

            var entity = ObjectMapper.Map<CreatePurchaseRequestsDto, PurchaseRequests>(input);

            var addedEntity = await _repository.InsertAsync(entity);

            foreach (var item in input.SelectPurchaseRequestLines)
            {
                var lineEntity = ObjectMapper.Map<SelectPurchaseRequestLinesDto, PurchaseRequestLines>(item);
                lineEntity.Id = GuidGenerator.CreateGuid();
                lineEntity.PurchaseRequestID = addedEntity.Id;
                await _lineRepository.InsertAsync(lineEntity);
            }

            await _repository.SaveChanges();
            await _lineRepository.SaveChanges();
            return new SuccessDataResult<SelectPurchaseRequestsDto>(ObjectMapper.Map<PurchaseRequests, SelectPurchaseRequestsDto>(addedEntity));
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var lines = (await _lineRepository.GetAsync(t => t.Id == id));

            if (lines != null)
            {
                await _manager.DeleteControl(_repository, lines.PurchaseRequestID, lines.Id, true);
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

        public async Task<IDataResult<SelectPurchaseRequestsDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id,
                t => t.PurchaseRequestLines,
                t => t.CurrentAccountCards,
                t => t.Warehouses,
                t => t.Branches,
                t => t.Currencies,
                t => t.PaymentPlan);

            var mappedEntity = ObjectMapper.Map<PurchaseRequests, SelectPurchaseRequestsDto>(entity);

            mappedEntity.SelectPurchaseRequestLines = ObjectMapper.Map<List<PurchaseRequestLines>, List<SelectPurchaseRequestLinesDto>>(entity.PurchaseRequestLines.ToList());

            return new SuccessDataResult<SelectPurchaseRequestsDto>(mappedEntity);
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListPurchaseRequestsDto>>> GetListAsync(ListPurchaseRequestsParameterDto input)
        {
            var list = await _repository.GetListAsync(null,
                t => t.PurchaseRequestLines,
                t => t.CurrentAccountCards,
                t => t.Warehouses,
                t => t.Branches,
                t => t.Currencies,
                t => t.PaymentPlan);

            var mappedEntity = ObjectMapper.Map<List<PurchaseRequests>, List<ListPurchaseRequestsDto>>(list.ToList());

            return new SuccessDataResult<IList<ListPurchaseRequestsDto>>(mappedEntity);
        }

        [ValidationAspect(typeof(UpdatePurchaseRequestsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPurchaseRequestsDto>> UpdateAsync(UpdatePurchaseRequestsDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            await _manager.UpdateControl(_repository, input.FicheNo, input.Id, entity);

            var mappedEntity = ObjectMapper.Map<UpdatePurchaseRequestsDto, PurchaseRequests>(input);

            await _repository.UpdateAsync(mappedEntity);

            foreach (var item in input.SelectPurchaseRequestLines)
            {
                var lineEntity = ObjectMapper.Map<SelectPurchaseRequestLinesDto, PurchaseRequestLines>(item);
                lineEntity.PurchaseRequestID = mappedEntity.Id;

                if (lineEntity.Id == Guid.Empty)
                {
                    lineEntity.Id = GuidGenerator.CreateGuid();
                    await _lineRepository.InsertAsync(lineEntity);
                    await _lineRepository.SaveChanges();
                }
                else
                {
                    await _lineRepository.UpdateAsync(lineEntity);
                    await _lineRepository.SaveChanges();
                }
            }

            await _repository.SaveChanges();

            return new SuccessDataResult<SelectPurchaseRequestsDto>(ObjectMapper.Map<PurchaseRequests, SelectPurchaseRequestsDto>(mappedEntity));
        }

        public async Task UpdatePurchaseRequestLineState(List<SelectPurchaseOrderLinesDto> orderLineList, PurchaseRequestLineStateEnum lineState)
        {

            foreach (var item in orderLineList)
            {
                var lineEntity = (await GetAsync(item.LikedPurchaseRequestLineID.GetValueOrDefault())).Data.SelectPurchaseRequestLines;

                if (lineEntity.Count > 0)
                {
                    foreach (var line in lineEntity)
                    {
                        var mappedLineEntity = ObjectMapper.Map<SelectPurchaseRequestLinesDto, PurchaseRequestLines>(line);
                        mappedLineEntity.PurchaseRequestLineState = lineState;
                        mappedLineEntity.OrderConversionDate = DateTime.Now;
                        await _lineRepository.UpdateAsync(mappedLineEntity);
                        await _lineRepository.SaveChanges();
                    }
                }
            }
        }
    }

}

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
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.SalesProposition;
using TsiErp.Entities.Entities.Period.Dtos;
using TsiErp.Entities.Entities.Period;
using TsiErp.Entities.Entities.SalesProposition.Dtos;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Entities.Entities.SalesProposition;
using TsiErp.Business.Entities.SalesProposition.Validations;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.SalesPropositionLine;
using TsiErp.Entities.Entities.SalesPropositionLine.Dtos;
using TsiErp.Entities.Entities.SalesPropositionLine;
using Tsi.Core.Entities;
using TsiErp.Business.Entities.SalesProposition.BusinessRules;
using Microsoft.EntityFrameworkCore;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using Tsi.Core.Utilities.Guids;
using TsiErp.Entities.Enums;
using TsiErp.Entities.Entities.SalesOrderLine.Dtos;

namespace TsiErp.Business.Entities.SalesProposition.Services
{
    [ServiceRegistration(typeof(ISalesPropositionsAppService), DependencyInjectionType.Scoped)]
    public class SalesPropositionsAppService : ApplicationService, ISalesPropositionsAppService
    {
        private readonly ISalesPropositionsRepository _repository;
        private readonly ISalesPropositionLinesRepository _lineRepository;

        SalesPropositionManager _manager { get; set; } = new SalesPropositionManager();

        public SalesPropositionsAppService(ISalesPropositionsRepository repository, ISalesPropositionLinesRepository lineRepository)
        {
            _repository = repository;
            _lineRepository = lineRepository;
        }

        [ValidationAspect(typeof(CreateSalesPropositionsValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectSalesPropositionsDto>> CreateAsync(CreateSalesPropositionsDto input)
        {
            await _manager.CodeControl(_repository, input.FicheNo);

            var entity = ObjectMapper.Map<CreateSalesPropositionsDto, SalesPropositions>(input);

            var addedEntity = await _repository.InsertAsync(entity);

            foreach (var item in input.SelectSalesPropositionLines)
            {
                var lineEntity = ObjectMapper.Map<SelectSalesPropositionLinesDto, SalesPropositionLines>(item);
                lineEntity.Id = GuidGenerator.CreateGuid();
                lineEntity.SalesPropositionID = addedEntity.Id;
                await _lineRepository.InsertAsync(lineEntity);
            }

            await _repository.SaveChanges();
            await _lineRepository.SaveChanges();
            return new SuccessDataResult<SelectSalesPropositionsDto>(ObjectMapper.Map<SalesPropositions, SelectSalesPropositionsDto>(addedEntity));
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var lines = (await _lineRepository.GetAsync(t => t.Id == id));

            if (lines != null)
            {
                await _manager.DeleteControl(_repository, lines.SalesPropositionID, lines.Id, true);
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

        public async Task<IDataResult<SelectSalesPropositionsDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id,
                t => t.SalesPropositionLines,
                t => t.CurrentAccountCards,
                t => t.Warehouses,
                t => t.Branches,
                t => t.Currencies,
                t => t.PaymentPlan);

            var mappedEntity = ObjectMapper.Map<SalesPropositions, SelectSalesPropositionsDto>(entity);

            mappedEntity.SelectSalesPropositionLines = ObjectMapper.Map<List<SalesPropositionLines>, List<SelectSalesPropositionLinesDto>>(entity.SalesPropositionLines.ToList());

            return new SuccessDataResult<SelectSalesPropositionsDto>(mappedEntity);
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListSalesPropositionsDto>>> GetListAsync(ListSalesPropositionsParamaterDto input)
        {
            var list = await _repository.GetListAsync(null,
                t => t.SalesPropositionLines,
                t => t.CurrentAccountCards,
                t => t.Warehouses,
                t => t.Branches,
                t => t.Currencies,
                t => t.PaymentPlan);

            var mappedEntity = ObjectMapper.Map<List<SalesPropositions>, List<ListSalesPropositionsDto>>(list.ToList());

            return new SuccessDataResult<IList<ListSalesPropositionsDto>>(mappedEntity);
        }

        [ValidationAspect(typeof(UpdateSalesPropositionsValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectSalesPropositionsDto>> UpdateAsync(UpdateSalesPropositionsDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            await _manager.UpdateControl(_repository, input.FicheNo, input.Id, entity);

            var mappedEntity = ObjectMapper.Map<UpdateSalesPropositionsDto, SalesPropositions>(input);

            await _repository.UpdateAsync(mappedEntity);

            foreach (var item in input.SelectSalesPropositionLines)
            {
                var lineEntity = ObjectMapper.Map<SelectSalesPropositionLinesDto, SalesPropositionLines>(item);
                lineEntity.SalesPropositionID = mappedEntity.Id;

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

            return new SuccessDataResult<SelectSalesPropositionsDto>(ObjectMapper.Map<SalesPropositions, SelectSalesPropositionsDto>(mappedEntity));
        }

        public async Task UpdateSalesPropositionLineState(List<SelectSalesOrderLinesDto> orderLineList, SalesPropositionLineStateEnum lineState)
        {

            foreach (var item in orderLineList)
            {
                var lineEntity = (await GetAsync(item.LinkedSalesPropositionID.GetValueOrDefault())).Data.SelectSalesPropositionLines;

                if (lineEntity.Count > 0)
                {
                    foreach (var line in lineEntity)
                    {
                        var mappedLineEntity = ObjectMapper.Map<SelectSalesPropositionLinesDto, SalesPropositionLines>(line);
                        mappedLineEntity.SalesPropositionLineState = lineState;
                        mappedLineEntity.OrderConversionDate = DateTime.Now;
                        await _lineRepository.UpdateAsync(mappedLineEntity);
                        await _lineRepository.SaveChanges();
                    }
                }
            }


        }
    }

}

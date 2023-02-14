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
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;

namespace TsiErp.Business.Entities.SalesProposition.Services
{
    [ServiceRegistration(typeof(ISalesPropositionsAppService), DependencyInjectionType.Scoped)]
    public class SalesPropositionsAppService : ApplicationService, ISalesPropositionsAppService
    {
        SalesPropositionManager _manager { get; set; } = new SalesPropositionManager();

        [ValidationAspect(typeof(CreateSalesPropositionsValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectSalesPropositionsDto>> CreateAsync(CreateSalesPropositionsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.CodeControl(_uow.SalesPropositionsRepository, input.FicheNo);

                var entity = ObjectMapper.Map<CreateSalesPropositionsDto, SalesPropositions>(input);

                var addedEntity = await _uow.SalesPropositionsRepository.InsertAsync(entity);

                foreach (var item in input.SelectSalesPropositionLines)
                {
                    var lineEntity = ObjectMapper.Map<SelectSalesPropositionLinesDto, SalesPropositionLines>(item);
                    lineEntity.Id = GuidGenerator.CreateGuid();
                    lineEntity.SalesPropositionID = addedEntity.Id;
                    await _uow.SalesPropositionLinesRepository.InsertAsync(lineEntity);
                }

                await _uow.SaveChanges();
                return new SuccessDataResult<SelectSalesPropositionsDto>(ObjectMapper.Map<SalesPropositions, SelectSalesPropositionsDto>(addedEntity));
            }
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var lines = (await _uow.SalesPropositionLinesRepository.GetAsync(t => t.Id == id));

                if (lines != null)
                {
                    await _manager.DeleteControl(_uow.SalesPropositionsRepository, lines.SalesPropositionID, lines.Id, true);
                    await _uow.SalesPropositionLinesRepository.DeleteAsync(id);
                    await _uow.SaveChanges();
                    return new SuccessResult("Silme işlemi başarılı.");
                }
                else
                {
                    await _manager.DeleteControl(_uow.SalesPropositionsRepository, id, Guid.Empty, false);
                    await _uow.SalesPropositionsRepository.DeleteAsync(id);
                    await _uow.SaveChanges();
                    return new SuccessResult("Silme işlemi başarılı.");
                }
            }
        }

        public async Task<IDataResult<SelectSalesPropositionsDto>> GetAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.SalesPropositionsRepository.GetAsync(t => t.Id == id,
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
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListSalesPropositionsDto>>> GetListAsync(ListSalesPropositionsParamaterDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var list = await _uow.SalesPropositionsRepository.GetListAsync(null,
                t => t.SalesPropositionLines,
                t => t.CurrentAccountCards,
                t => t.Warehouses,
                t => t.Branches,
                t => t.Currencies,
                t => t.PaymentPlan);

                var mappedEntity = ObjectMapper.Map<List<SalesPropositions>, List<ListSalesPropositionsDto>>(list.ToList());

                return new SuccessDataResult<IList<ListSalesPropositionsDto>>(mappedEntity);
            }
        }

        [ValidationAspect(typeof(UpdateSalesPropositionsValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectSalesPropositionsDto>> UpdateAsync(UpdateSalesPropositionsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.SalesPropositionsRepository.GetAsync(x => x.Id == input.Id);

                await _manager.UpdateControl(_uow.SalesPropositionsRepository, input.FicheNo, input.Id, entity);

                var mappedEntity = ObjectMapper.Map<UpdateSalesPropositionsDto, SalesPropositions>(input);

                await _uow.SalesPropositionsRepository.UpdateAsync(mappedEntity);

                foreach (var item in input.SelectSalesPropositionLines)
                {
                    var lineEntity = ObjectMapper.Map<SelectSalesPropositionLinesDto, SalesPropositionLines>(item);
                    lineEntity.SalesPropositionID = mappedEntity.Id;

                    if (lineEntity.Id == Guid.Empty)
                    {
                        lineEntity.Id = GuidGenerator.CreateGuid();
                        await _uow.SalesPropositionLinesRepository.InsertAsync(lineEntity);
                    }
                    else
                    {
                        await _uow.SalesPropositionLinesRepository.UpdateAsync(lineEntity);
                    }
                }

                await _uow.SaveChanges();

                return new SuccessDataResult<SelectSalesPropositionsDto>(ObjectMapper.Map<SalesPropositions, SelectSalesPropositionsDto>(mappedEntity));
            }
        }

        public async Task UpdateSalesPropositionLineState(List<SelectSalesOrderLinesDto> orderLineList, SalesPropositionLineStateEnum lineState)
        {
            using (UnitOfWork _uow = new UnitOfWork())
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
                            await _uow.SalesPropositionLinesRepository.UpdateAsync(mappedLineEntity);
                            await _uow.SaveChanges();
                        }
                    }
                }
            }
        }
    }

}

using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.SalesProposition.BusinessRules;
using TsiErp.Business.Entities.SalesProposition.Validations;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.SalesOrderLine.Dtos;
using TsiErp.Entities.Entities.SalesProposition;
using TsiErp.Entities.Entities.SalesProposition.Dtos;
using TsiErp.Entities.Entities.SalesPropositionLine;
using TsiErp.Entities.Entities.SalesPropositionLine.Dtos;
using TsiErp.Entities.Enums;

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
                    lineEntity.SalesPropositionID = addedEntity.Id;
                    await _uow.SalesPropositionLinesRepository.InsertAsync(lineEntity);
                }

                input.Id = addedEntity.Id;
                var log = LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, "SalesPropositions", LogType.Insert, addedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);

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

                    var list = (await _uow.SalesPropositionLinesRepository.GetListAsync(t => t.SalesPropositionID == id));
                    foreach (var line in list)
                    {
                        await _uow.SalesPropositionLinesRepository.DeleteAsync(line.Id);
                    }


                    await _uow.SalesPropositionsRepository.DeleteAsync(id);
                    var log = LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, "SalesPropositions", LogType.Delete, id);
                    await _uow.LogsRepository.InsertAsync(log);
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

                foreach (var item in mappedEntity.SelectSalesPropositionLines)
                {
                    item.ProductCode = (await _uow.ProductsRepository.GetAsync(t => t.Id == item.ProductID)).Code;
                    item.ProductName = (await _uow.ProductsRepository.GetAsync(t => t.Id == item.ProductID)).Name;
                    item.UnitSetCode = (await _uow.UnitSetsRepository.GetAsync(t => t.Id == item.UnitSetID)).Code;
                }

                var log = LogsAppService.InsertLogToDatabase(mappedEntity, mappedEntity, LoginedUserService.UserId, "SalesPropositions", LogType.Get, id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();

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
                        await _uow.SalesPropositionLinesRepository.InsertAsync(lineEntity);
                    }
                    else
                    {
                        await _uow.SalesPropositionLinesRepository.UpdateAsync(lineEntity);
                    }
                }

                var before = ObjectMapper.Map<SalesPropositions, UpdateSalesPropositionsDto>(entity);
                before.SelectSalesPropositionLines = ObjectMapper.Map<List<SalesPropositionLines>, List<SelectSalesPropositionLinesDto>>(entity.SalesPropositionLines.ToList());
                var log = LogsAppService.InsertLogToDatabase(before, input, LoginedUserService.UserId, "SalesPropositions", LogType.Update, mappedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);

                await _uow.SaveChanges();

                return new SuccessDataResult<SelectSalesPropositionsDto>(ObjectMapper.Map<SalesPropositions, SelectSalesPropositionsDto>(mappedEntity));
            }
        }

        public async Task<IDataResult<SelectSalesPropositionsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.SalesPropositionsRepository.GetAsync(x => x.Id == id);

                var updatedEntity = await _uow.SalesPropositionsRepository.LockRow(entity.Id, lockRow, userId);

                await _uow.SaveChanges();

                var mappedEntity = ObjectMapper.Map<SalesPropositions, SelectSalesPropositionsDto>(updatedEntity);

                return new SuccessDataResult<SelectSalesPropositionsDto>(mappedEntity);
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

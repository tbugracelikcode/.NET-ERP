using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Forecast.BusinessRules;
using TsiErp.Business.Entities.Forecast.Validations;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.Forecast;
using TsiErp.Entities.Entities.Forecast.Dtos;
using TsiErp.Entities.Entities.ForecastLine;
using TsiErp.Entities.Entities.ForecastLine.Dtos;

namespace TsiErp.Business.Entities.Forecast.Services
{
    [ServiceRegistration(typeof(IForecastsAppService), DependencyInjectionType.Scoped)]
    public class ForecastsAppService : ApplicationService, IForecastsAppService
    {
        ForecastManager _manager { get; set; } = new ForecastManager();

        [ValidationAspect(typeof(CreateForecastsValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectForecastsDto>> CreateAsync(CreateForecastsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.CodeControl(_uow.ForecastsRepository, input.Code);

                var entity = ObjectMapper.Map<CreateForecastsDto, Forecasts>(input);

                var addedEntity = await _uow.ForecastsRepository.InsertAsync(entity);

                foreach (var item in input.SelectForecastLines)
                {
                    var lineEntity = ObjectMapper.Map<SelectForecastLinesDto, ForecastLines>(item);
                    lineEntity.ForecastID = addedEntity.Id;
                    await _uow.ForecastLinesRepository.InsertAsync(lineEntity);
                }
                input.Id = addedEntity.Id;
                var log = LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, "Forecasts", LogType.Insert, addedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);

                await _uow.SaveChanges();
                return new SuccessDataResult<SelectForecastsDto>(ObjectMapper.Map<Forecasts, SelectForecastsDto>(addedEntity));
            }
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var lines = (await _uow.ForecastLinesRepository.GetAsync(t => t.Id == id));

                if (lines != null)
                {
                    await _manager.DeleteControl(_uow.ForecastsRepository, lines.ForecastID, lines.Id, true);
                    await _uow.ForecastLinesRepository.DeleteAsync(id);
                    await _uow.SaveChanges();
                    return new SuccessResult("Silme işlemi başarılı.");
                }
                else
                {
                    await _manager.DeleteControl(_uow.ForecastsRepository, id, Guid.Empty, false);

                    var list = (await _uow.ForecastLinesRepository.GetListAsync(t => t.ForecastID == id));
                    foreach (var line in list)
                    {
                        await _uow.ForecastLinesRepository.DeleteAsync(line.Id);
                    }
                    await _uow.ForecastsRepository.DeleteAsync(id);
                    var log = LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, "Forecasts", LogType.Delete, id);
                    await _uow.LogsRepository.InsertAsync(log);
                    await _uow.SaveChanges();
                    return new SuccessResult("Silme işlemi başarılı.");
                }
            }
        }

        public async Task<IDataResult<SelectForecastsDto>> GetAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.ForecastsRepository.GetAsync(t => t.Id == id,
                t => t.ForecastLines,
                t => t.CurrentAccountCards,
                t => t.Periods,
                t => t.Branches);

                var mappedEntity = ObjectMapper.Map<Forecasts, SelectForecastsDto>(entity);

                mappedEntity.SelectForecastLines = ObjectMapper.Map<List<ForecastLines>, List<SelectForecastLinesDto>>(entity.ForecastLines.ToList());

                foreach (var item in mappedEntity.SelectForecastLines)
                {
                    item.ProductCode = (await _uow.ProductsRepository.GetAsync(t => t.Id == item.ProductID)).Code;
                    item.ProductName = (await _uow.ProductsRepository.GetAsync(t => t.Id == item.ProductID)).Name;
                }

                var log = LogsAppService.InsertLogToDatabase(mappedEntity, mappedEntity, LoginedUserService.UserId, "Forecasts", LogType.Get, id);
                await _uow.LogsRepository.InsertAsync(log);

                await _uow.SaveChanges();

                return new SuccessDataResult<SelectForecastsDto>(mappedEntity);
            }
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListForecastsDto>>> GetListAsync(ListForecastsParameterDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var list = await _uow.ForecastsRepository.GetListAsync(null,
                t => t.ForecastLines,
                t => t.CurrentAccountCards,
                t => t.Periods,
                t => t.Branches);

                var mappedEntity = ObjectMapper.Map<List<Forecasts>, List<ListForecastsDto>>(list.ToList());

                return new SuccessDataResult<IList<ListForecastsDto>>(mappedEntity);
            }
        }

        [ValidationAspect(typeof(UpdateForecastsValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectForecastsDto>> UpdateAsync(UpdateForecastsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.ForecastsRepository.GetAsync(x => x.Id == input.Id);

                await _manager.UpdateControl(_uow.ForecastsRepository, input.Code, input.Id, entity);

                var mappedEntity = ObjectMapper.Map<UpdateForecastsDto, Forecasts>(input);

                await _uow.ForecastsRepository.UpdateAsync(mappedEntity);

                foreach (var item in input.SelectForecastLines)
                {
                    var lineEntity = ObjectMapper.Map<SelectForecastLinesDto, ForecastLines>(item);
                    lineEntity.ForecastID = mappedEntity.Id;

                    if (lineEntity.Id == Guid.Empty)
                    {
                        await _uow.ForecastLinesRepository.InsertAsync(lineEntity);
                    }
                    else
                    {
                        await _uow.ForecastLinesRepository.UpdateAsync(lineEntity);
                    }
                }

                var before = ObjectMapper.Map<Forecasts, UpdateForecastsDto>(entity);
                before.SelectForecastLines = ObjectMapper.Map<List<ForecastLines>, List<SelectForecastLinesDto>>(entity.ForecastLines.ToList());
                var log = LogsAppService.InsertLogToDatabase(before, input, LoginedUserService.UserId, "Forecasts", LogType.Update, mappedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);

                await _uow.SaveChanges();

                return new SuccessDataResult<SelectForecastsDto>(ObjectMapper.Map<Forecasts, SelectForecastsDto>(mappedEntity));
            }
        }

        public async Task<IDataResult<SelectForecastsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.ForecastsRepository.GetAsync(x => x.Id == id);

                var updatedEntity = await _uow.ForecastsRepository.LockRow(entity.Id, lockRow, userId);

                await _uow.SaveChanges();

                var mappedEntity = ObjectMapper.Map<Forecasts, SelectForecastsDto>(updatedEntity);

                return new SuccessDataResult<SelectForecastsDto>(mappedEntity);
            }
        }
    }
}

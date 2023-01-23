using Tsi.Application.Contract.Services.EntityFrameworkCore;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.Entities.Forecast.BusinessRules;
using TsiErp.Business.Entities.Forecast.Validations;
using TsiErp.Business.Entities.SalesProposition.Services;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Forecast;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.ForecastLine;
using TsiErp.Entities.Entities.Forecast;
using TsiErp.Entities.Entities.Forecast.Dtos;
using TsiErp.Entities.Entities.ForecastLine;
using TsiErp.Entities.Entities.ForecastLine.Dtos;

namespace TsiErp.Business.Entities.Forecast.Services
{
    [ServiceRegistration(typeof(IForecastsAppService), DependencyInjectionType.Scoped)]
    public class ForecastsAppService : ApplicationService, IForecastsAppService
    {
        private readonly IForecastsRepository _repository;
        private readonly IForecastLinesRepository _lineRepository;

        private readonly ISalesPropositionsAppService _salesPropositionsAppService;

        ForecastManager _manager { get; set; } = new ForecastManager();
        public ForecastsAppService(IForecastsRepository repository, IForecastLinesRepository lineRepository, ISalesPropositionsAppService salesPropositionsAppService)
        {
            _repository = repository;
            _lineRepository = lineRepository;
            _salesPropositionsAppService = salesPropositionsAppService;
        }


        [ValidationAspect(typeof(CreateForecastsValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectForecastsDto>> CreateAsync(CreateForecastsDto input)
        {
            await _manager.CodeControl(_repository, input.Code);

            var entity = ObjectMapper.Map<CreateForecastsDto, Forecasts>(input);

            var addedEntity = await _repository.InsertAsync(entity);

            foreach (var item in input.SelectForecastLines)
            {
                var lineEntity = ObjectMapper.Map<SelectForecastLinesDto, ForecastLines>(item);
                lineEntity.Id = GuidGenerator.CreateGuid();
                lineEntity.ForecastID = addedEntity.Id;
                await _lineRepository.InsertAsync(lineEntity);
            }

            await _repository.SaveChanges();
            await _lineRepository.SaveChanges();
            return new SuccessDataResult<SelectForecastsDto>(ObjectMapper.Map<Forecasts, SelectForecastsDto>(addedEntity));
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var lines = (await _lineRepository.GetAsync(t => t.Id == id));

            if (lines != null)
            {
                await _manager.DeleteControl(_repository, lines.ForecastID, lines.Id, true);
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

        public async Task<IDataResult<SelectForecastsDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id,
                t => t.ForecastLines,
                t => t.CurrentAccountCards,
                t => t.Periods,
                t => t.Branches);

            var mappedEntity = ObjectMapper.Map<Forecasts, SelectForecastsDto>(entity);

            mappedEntity.SelectForecastLines = ObjectMapper.Map<List<ForecastLines>, List<SelectForecastLinesDto>>(entity.ForecastLines.ToList());

            return new SuccessDataResult<SelectForecastsDto>(mappedEntity);
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListForecastsDto>>> GetListAsync(ListForecastsParameterDto input)
        {
            var list = await _repository.GetListAsync(null,
                t => t.ForecastLines,
                t => t.CurrentAccountCards,
                t => t.Periods,
                t => t.Branches);

            var mappedEntity = ObjectMapper.Map<List<Forecasts>, List<ListForecastsDto>>(list.ToList());

            return new SuccessDataResult<IList<ListForecastsDto>>(mappedEntity);
        }

        [ValidationAspect(typeof(UpdateForecastsValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectForecastsDto>> UpdateAsync(UpdateForecastsDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            await _manager.UpdateControl(_repository, input.Code, input.Id, entity);

            var mappedEntity = ObjectMapper.Map<UpdateForecastsDto, Forecasts>(input);

            await _repository.UpdateAsync(mappedEntity);

            foreach (var item in input.SelectForecastLines)
            {
                var lineEntity = ObjectMapper.Map<SelectForecastLinesDto, ForecastLines>(item);
                lineEntity.ForecastID = mappedEntity.Id;

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

            return new SuccessDataResult<SelectForecastsDto>(ObjectMapper.Map<Forecasts, SelectForecastsDto>(mappedEntity));
        }
    }
}

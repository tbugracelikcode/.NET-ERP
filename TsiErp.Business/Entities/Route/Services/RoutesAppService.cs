using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Route;
using TsiErp.Entities.Entities.Route.Dtos;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Entities.Entities.Route;
using TsiErp.Business.Entities.Route.Validations;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.RouteLine;
using TsiErp.Entities.Entities.RouteLine.Dtos;
using TsiErp.Entities.Entities.RouteLine;
using Tsi.Core.Utilities.Guids;
using TsiErp.Business.Entities.Route.BusinessRules;

namespace TsiErp.Business.Entities.Route.Services
{
    [ServiceRegistration(typeof(IRoutesAppService), DependencyInjectionType.Scoped)]
    public class RoutesAppService : ApplicationService, IRoutesAppService
    {
        private readonly IRoutesRepository _repository;
        private readonly IRouteLinesRepository _lineRepository;

        RouteManager _manager { get; set; } = new RouteManager();

        public RoutesAppService(IRoutesRepository repository, IRouteLinesRepository lineRepository)
        {
            _repository = repository;
            _lineRepository = lineRepository;
        }

        [ValidationAspect(typeof(CreateRoutesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectRoutesDto>> CreateAsync(CreateRoutesDto input)
        {
            await _manager.CodeControl(_repository, input.Code);

            var entity = ObjectMapper.Map<CreateRoutesDto, Routes>(input);

            var addedEntity = await _repository.InsertAsync(entity);

            foreach (var item in input.SelectRouteLines)
            {
                var lineEntity = ObjectMapper.Map<SelectRouteLinesDto, RouteLines>(item);
                lineEntity.Id = GuidGenerator.CreateGuid();
                lineEntity.RouteID = addedEntity.Id;
                await _lineRepository.InsertAsync(lineEntity);
            }

            await _repository.SaveChanges();
            await _lineRepository.SaveChanges();
            return new SuccessDataResult<SelectRoutesDto>(ObjectMapper.Map<Routes, SelectRoutesDto>(addedEntity));
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var lines = (await _lineRepository.GetAsync(t => t.Id == id));

            if (lines != null)
            {
                await _manager.DeleteControl(_repository, lines.RouteID, lines.Id, true);
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

        public async Task<IDataResult<SelectRoutesDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id,
                t => t.RouteLines,
                t=>t.Products);

            var mappedEntity = ObjectMapper.Map<Routes, SelectRoutesDto>(entity);

            mappedEntity.SelectRouteLines = ObjectMapper.Map<List<RouteLines>, List<SelectRouteLinesDto>>(entity.RouteLines.ToList());

            return new SuccessDataResult<SelectRoutesDto>(mappedEntity);
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListRoutesDto>>> GetListAsync(ListRoutesParameterDto input)
        {
            var list = await _repository.GetListAsync(null,
                t => t.RouteLines,
                t => t.Products);

            var mappedEntity = ObjectMapper.Map<List<Routes>, List<ListRoutesDto>>(list.ToList());

            return new SuccessDataResult<IList<ListRoutesDto>>(mappedEntity);
        }

        [ValidationAspect(typeof(UpdateRoutesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectRoutesDto>> UpdateAsync(UpdateRoutesDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            await _manager.UpdateControl(_repository, input.Code, input.Id, entity);

            var mappedEntity = ObjectMapper.Map<UpdateRoutesDto, Routes>(input);

            await _repository.UpdateAsync(mappedEntity);

            foreach (var item in input.SelectRouteLines)
            {
                var lineEntity = ObjectMapper.Map<SelectRouteLinesDto, RouteLines>(item);
                lineEntity.RouteID = mappedEntity.Id;

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

            return new SuccessDataResult<SelectRoutesDto>(ObjectMapper.Map<Routes, SelectRoutesDto>(mappedEntity));
        }
    }
}

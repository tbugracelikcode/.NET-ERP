using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Aspects.Autofac.Caching;
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
using Tsi.Core.Entities;

namespace TsiErp.Business.Entities.Route.Services
{
    [ServiceRegistration(typeof(IRoutesAppService), DependencyInjectionType.Scoped)]
    public class RoutesAppService : IRoutesAppService
    {
        private readonly IRoutesRepository _repository;
        private readonly IRouteLinesRepository _lineRepository;

        public RoutesAppService(IRoutesRepository repository, IRouteLinesRepository lineRepository)
        {
            _repository = repository;
            _lineRepository = lineRepository;
        }


        [ValidationAspect(typeof(CreateRoutesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectRoutesDto>> CreateAsync(CreateRoutesDto input)
        {
            var entity = ObjectMapper.Map<CreateRoutesDto, Routes>(input);

            var addedEntity = await _repository.InsertAsync(entity);

            foreach (var item in input.SelectRouteLines)
            {
                var lineEntity = ObjectMapper.Map<SelectRouteLinesDto, RouteLines>(item);
                lineEntity.RouteID = addedEntity.Id;
                await _lineRepository.InsertAsync(lineEntity);
            }

            return new SuccessDataResult<SelectRoutesDto>(ObjectMapper.Map<Routes, SelectRoutesDto>(addedEntity));
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);

            var lines = (await _lineRepository.GetListAsync(t => t.RouteID == id)).ToList();

            foreach (var line in lines)
            {
                await _lineRepository.DeleteAsync(line.Id);
            }

            return new SuccessResult("Silme işlemi başarılı.");
        }

        public async Task<IDataResult<SelectRoutesDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id, t => t.RouteLines, x=>x.Products);
            var mappedEntity = ObjectMapper.Map<Routes, SelectRoutesDto>(entity);

            //var lines = (await _lineRepository.GetListAsync(t => t.SalesPropositionID == id)).ToList();
            mappedEntity.SelectRouteLines = ObjectMapper.Map<List<RouteLines>, List<SelectRouteLinesDto>>(entity.RouteLines.ToList());

            return new SuccessDataResult<SelectRoutesDto>(mappedEntity);
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListRoutesDto>>> GetListAsync(ListRoutesParameterDto input)
        {
            var list = await _repository.GetListAsync(null, t => t.RouteLines, x => x.Products);

            var mappedEntity = ObjectMapper.Map<List<Routes>, List<ListRoutesDto>>(list.ToList());

            return new SuccessDataResult<IList<ListRoutesDto>>(mappedEntity);
        }

        [ValidationAspect(typeof(UpdateRoutesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectRoutesDto>> UpdateAsync(UpdateRoutesDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            var mappedEntity = ObjectMapper.Map<UpdateRoutesDto, Routes>(input);

            await _repository.UpdateAsync(mappedEntity);

            foreach (var item in input.SelectRouteLines)
            {
                var lineEntity = ObjectMapper.Map<SelectRouteLinesDto, RouteLines>(item);
                lineEntity.RouteID = mappedEntity.Id;
                await _lineRepository.UpdateAsync(lineEntity);
            }

            return new SuccessDataResult<SelectRoutesDto>(ObjectMapper.Map<Routes, SelectRoutesDto>(mappedEntity));
        }
    }
}

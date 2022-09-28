using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Operation;
using TsiErp.Entities.Entities.Operation.Dtos;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Entities.Entities.Operation;
using TsiErp.Business.Entities.Operation.Validations;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.OperationLine;
using TsiErp.Entities.Entities.OperationLine.Dtos;
using TsiErp.Entities.Entities.OperationLine;
using Tsi.Core.Entities;

namespace TsiErp.Business.Entities.Operation.Services
{
    [ServiceRegistration(typeof(IOperationsAppService), DependencyInjectionType.Scoped)]
    public class OperationsAppService : IOperationsAppService
    {

        private readonly IOperationsRepository _repository;
        private readonly IOperationLinesRepository _lineRepository;

        public OperationsAppService(IOperationsRepository repository, IOperationLinesRepository lineRepository)
        {
            _repository = repository;
            _lineRepository = lineRepository;
        }


        [ValidationAspect(typeof(CreateOperationsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectOperationsDto>> CreateAsync(CreateOperationsDto input)
        {
            var entity = ObjectMapper.Map<CreateOperationsDto, Operations>(input);

            var addedEntity = await _repository.InsertAsync(entity);

            foreach (var item in input.SelectOperationLines)
            {
                var lineEntity = ObjectMapper.Map<SelectOperationLinesDto, OperationLines>(item);
                lineEntity.OperationID = addedEntity.Id;
                await _lineRepository.InsertAsync(lineEntity);
            }

            return new SuccessDataResult<SelectOperationsDto>(ObjectMapper.Map<Operations, SelectOperationsDto>(addedEntity));
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);

            var lines = (await _lineRepository.GetListAsync(t => t.OperationID == id)).ToList();

            foreach (var line in lines)
            {
                await _lineRepository.DeleteAsync(line.Id);
            }

            return new SuccessResult("Silme işlemi başarılı.");
        }

        public async Task<IDataResult<SelectOperationsDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id, t => t.OperationLines, y=>y.RouteLines);
            var mappedEntity = ObjectMapper.Map<Operations, SelectOperationsDto>(entity);

            //var lines = (await _lineRepository.GetListAsync(t => t.SalesPropositionID == id)).ToList();
            mappedEntity.SelectOperationLines = ObjectMapper.Map<List<OperationLines>, List<SelectOperationLinesDto>>(entity.OperationLines.ToList());

            return new SuccessDataResult<SelectOperationsDto>(mappedEntity);
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListOperationsDto>>> GetListAsync(ListOperationsParameterDto input)
        {
            var list = await _repository.GetListAsync(null, t => t.OperationLines, y => y.RouteLines);

            var mappedEntity = ObjectMapper.Map<List<Operations>, List<ListOperationsDto>>(list.ToList());

            return new SuccessDataResult<IList<ListOperationsDto>>(mappedEntity);
        }

        [ValidationAspect(typeof(UpdateOperationsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectOperationsDto>> UpdateAsync(UpdateOperationsDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            var mappedEntity = ObjectMapper.Map<UpdateOperationsDto, Operations>(input);

            await _repository.UpdateAsync(mappedEntity);

            foreach (var item in input.SelectOperationLines)
            {
                var lineEntity = ObjectMapper.Map<SelectOperationLinesDto, OperationLines>(item);
                lineEntity.OperationID = mappedEntity.Id;
                await _lineRepository.UpdateAsync(lineEntity);
            }

            return new SuccessDataResult<SelectOperationsDto>(ObjectMapper.Map<Operations, SelectOperationsDto>(mappedEntity));
        }
    }
}

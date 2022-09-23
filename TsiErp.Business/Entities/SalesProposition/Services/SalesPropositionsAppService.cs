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

namespace TsiErp.Business.Entities.SalesProposition.Services
{
    [ServiceRegistration(typeof(ISalesPropositionsAppService), DependencyInjectionType.Scoped)]
    public class SalesPropositionsAppService : ISalesPropositionsAppService
    {
        private readonly ISalesPropositionsRepository _repository;
        private readonly ISalesPropositionLinesRepository _lineRepository;

        public SalesPropositionsAppService(ISalesPropositionsRepository repository, ISalesPropositionLinesRepository lineRepository)
        {
            _repository = repository;
            _lineRepository = lineRepository;
        }

        [ValidationAspect(typeof(CreateSalesPropositionsValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectSalesPropositionsDto>> CreateAsync(CreateSalesPropositionsDto input)
        {
            var entity = ObjectMapper.Map<CreateSalesPropositionsDto, SalesPropositions>(input);

            var addedEntity = await _repository.InsertAsync(entity);

            foreach (var item in input.SelectSalesPropositionLines)
            {
                var lineEntity = ObjectMapper.Map<SelectSalesPropositionLinesDto, SalesPropositionLines>(item);
                lineEntity.SalesPropositionID = addedEntity.Id;
                await _lineRepository.InsertAsync(lineEntity);
            }

            return new SuccessDataResult<SelectSalesPropositionsDto>(ObjectMapper.Map<SalesPropositions, SelectSalesPropositionsDto>(addedEntity));
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);

            var lines = (await _lineRepository.GetListAsync(t => t.SalesPropositionID == id)).ToList();

            foreach (var line in lines)
            {
                await _lineRepository.DeleteAsync(line.Id);
            }

            return new SuccessResult("Silme işlemi başarılı.");
        }

        public async Task<IDataResult<SelectSalesPropositionsDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id, t => t.SalesPropositionLines);
            var mappedEntity = ObjectMapper.Map<SalesPropositions, SelectSalesPropositionsDto>(entity);

            //var lines = (await _lineRepository.GetListAsync(t => t.SalesPropositionID == id)).ToList();
            mappedEntity.SelectSalesPropositionLines = ObjectMapper.Map<List<SalesPropositionLines>, List<SelectSalesPropositionLinesDto>>(entity.SalesPropositionLines.ToList());

            return new SuccessDataResult<SelectSalesPropositionsDto>(mappedEntity);
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListSalesPropositionsDto>>> GetListAsync(ListSalesPropositionsParamaterDto input)
        {
            var list = await _repository.GetListAsync(null, t => t.SalesPropositionLines);

            var mappedEntity = ObjectMapper.Map<List<SalesPropositions>, List<ListSalesPropositionsDto>>(list.ToList());

            return new SuccessDataResult<IList<ListSalesPropositionsDto>>(mappedEntity);
        }

        [ValidationAspect(typeof(UpdateSalesPropositionsValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectSalesPropositionsDto>> UpdateAsync(UpdateSalesPropositionsDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            var mappedEntity = ObjectMapper.Map<UpdateSalesPropositionsDto, SalesPropositions>(input);

            await _repository.UpdateAsync(mappedEntity);

            foreach (var item in input.SelectSalesPropositionLines)
            {
                var lineEntity = ObjectMapper.Map<SelectSalesPropositionLinesDto, SalesPropositionLines>(item);
                lineEntity.SalesPropositionID = mappedEntity.Id;
                await _lineRepository.UpdateAsync(lineEntity);
            }

            return new SuccessDataResult<SelectSalesPropositionsDto>(ObjectMapper.Map<SalesPropositions, SelectSalesPropositionsDto>(mappedEntity));
        }
    }
}

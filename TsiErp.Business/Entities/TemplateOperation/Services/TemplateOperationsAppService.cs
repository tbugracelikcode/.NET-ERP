using AutoMapper.Internal.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.Entities.TemplateOperation.BusinessRules;
using TsiErp.Business.Entities.TemplateOperation.Validations;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.TemplateOperation;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.TemplateOperationLine;
using TsiErp.Entities.Entities.TemplateOperation;
using TsiErp.Entities.Entities.TemplateOperation.Dtos;
using TsiErp.Entities.Entities.TemplateOperationLine;
using TsiErp.Entities.Entities.TemplateOperationLine.Dtos;

namespace TsiErp.Business.Entities.TemplateOperation.Services
{
    [ServiceRegistration(typeof(ITemplateOperationsAppService), DependencyInjectionType.Scoped)]
    public class TemplateOperationsAppService : ApplicationService, ITemplateOperationsAppService
    {
        private readonly ITemplateOperationsRepository _repository;
        private readonly ITemplateOperationLinesRepository _lineRepository;

        TemplateOperationManager _manager { get; set; } = new TemplateOperationManager();

        public TemplateOperationsAppService(ITemplateOperationsRepository repository, ITemplateOperationLinesRepository lineRepository)
        {
            _repository = repository;
            _lineRepository = lineRepository;
        }

        [ValidationAspect(typeof(CreateTemplateOperationsValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectTemplateOperationsDto>> CreateAsync(CreateTemplateOperationsDto input)
        {
            await _manager.CodeControl(_repository, input.Code);

            var entity = ObjectMapper.Map<CreateTemplateOperationsDto, TemplateOperations>(input);

            var addedEntity = await _repository.InsertAsync(entity);

            foreach (var item in input.SelectTemplateOperationLines)
            {
                var lineEntity = ObjectMapper.Map<SelectTemplateOperationLinesDto, TemplateOperationLines>(item);
                lineEntity.Id = GuidGenerator.CreateGuid();
                lineEntity.TemplateOperationID = addedEntity.Id;
                await _lineRepository.InsertAsync(lineEntity);
            }

            await _repository.SaveChanges();
            await _lineRepository.SaveChanges();
            return new SuccessDataResult<SelectTemplateOperationsDto>(ObjectMapper.Map<TemplateOperations, SelectTemplateOperationsDto>(addedEntity));
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var lines = (await _lineRepository.GetAsync(t => t.Id == id));

            if (lines != null)
            {
                await _manager.DeleteControl(_repository, lines.TemplateOperationID, lines.Id, true);
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

        public async Task<IDataResult<SelectTemplateOperationsDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id,
                t => t.TemplateOperationLines);

            var mappedEntity = ObjectMapper.Map<TemplateOperations, SelectTemplateOperationsDto>(entity);

            mappedEntity.SelectTemplateOperationLines = ObjectMapper.Map<List<TemplateOperationLines>, List<SelectTemplateOperationLinesDto>>(entity.TemplateOperationLines.ToList());

            return new SuccessDataResult<SelectTemplateOperationsDto>(mappedEntity);
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListTemplateOperationsDto>>> GetListAsync(ListTemplateOperationsParameterDto input)
        {
            var list = await _repository.GetListAsync(null,
                t => t.TemplateOperationLines);

            var mappedEntity = ObjectMapper.Map<List<TemplateOperations>, List<ListTemplateOperationsDto>>(list.ToList());

            return new SuccessDataResult<IList<ListTemplateOperationsDto>>(mappedEntity);
        }

        [ValidationAspect(typeof(UpdateTemplateOperationsValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectTemplateOperationsDto>> UpdateAsync(UpdateTemplateOperationsDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            await _manager.UpdateControl(_repository, input.Code, input.Id, entity);

            var mappedEntity = ObjectMapper.Map<UpdateTemplateOperationsDto, TemplateOperations>(input);

            await _repository.UpdateAsync(mappedEntity);

            foreach (var item in input.SelectTemplateOperationLines)
            {
                var lineEntity = ObjectMapper.Map<SelectTemplateOperationLinesDto, TemplateOperationLines>(item);
                lineEntity.TemplateOperationID = mappedEntity.Id;

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

            return new SuccessDataResult<SelectTemplateOperationsDto>(ObjectMapper.Map<TemplateOperations, SelectTemplateOperationsDto>(mappedEntity));
        }
    }
}

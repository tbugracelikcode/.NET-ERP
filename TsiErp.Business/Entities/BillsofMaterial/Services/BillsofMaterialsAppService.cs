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
using TsiErp.Business.Entities.BillsofMaterial.BusinessRules;
using TsiErp.Business.Entities.BillsofMaterial.Validations;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.BillsofMaterial;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.BillsofMaterialLine;
using TsiErp.Entities.Entities.BillsofMaterial;
using TsiErp.Entities.Entities.BillsofMaterial.Dtos;
using TsiErp.Entities.Entities.BillsofMaterialLine;
using TsiErp.Entities.Entities.BillsofMaterialLine.Dtos;

namespace TsiErp.Business.Entities.BillsofMaterial.Services
{
    [ServiceRegistration(typeof(IBillsofMaterialsAppService), DependencyInjectionType.Scoped)]
    public class BillsofMaterialsAppService : ApplicationService, IBillsofMaterialsAppService
    {
        private readonly IBillsofMaterialsRepository _repository;
        private readonly IBillsofMaterialLinesRepository _lineRepository;

        BillsofMaterialManager _manager { get; set; } = new BillsofMaterialManager();

        public BillsofMaterialsAppService(IBillsofMaterialsRepository repository, IBillsofMaterialLinesRepository lineRepository)
        {
            _repository = repository;
            _lineRepository = lineRepository;
        }

        [ValidationAspect(typeof(CreateBillsofMaterialsValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectBillsofMaterialsDto>> CreateAsync(CreateBillsofMaterialsDto input)
        {
            await _manager.CodeControl(_repository, input.Code);

            var entity = ObjectMapper.Map<CreateBillsofMaterialsDto, BillsofMaterials>(input);

            var addedEntity = await _repository.InsertAsync(entity);

            foreach (var item in input.SelectBillsofMaterialLines)
            {
                var lineEntity = ObjectMapper.Map<SelectBillsofMaterialLinesDto, BillsofMaterialLines>(item);
                lineEntity.Id = GuidGenerator.CreateGuid();
                lineEntity.BoMID = addedEntity.Id;
                await _lineRepository.InsertAsync(lineEntity);
            }

            await _repository.SaveChanges();
            await _lineRepository.SaveChanges();
            return new SuccessDataResult<SelectBillsofMaterialsDto>(ObjectMapper.Map<BillsofMaterials, SelectBillsofMaterialsDto>(addedEntity));
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var lines = (await _lineRepository.GetAsync(t => t.Id == id));

            if (lines != null)
            {
                await _manager.DeleteControl(_repository, lines.BoMID, lines.Id, true);
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

        public async Task<IDataResult<SelectBillsofMaterialsDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id,
                t => t.BillsofMaterialLines,
                t => t.Products);

            var mappedEntity = ObjectMapper.Map<BillsofMaterials, SelectBillsofMaterialsDto>(entity);

            mappedEntity.SelectBillsofMaterialLines = ObjectMapper.Map<List<BillsofMaterialLines>, List<SelectBillsofMaterialLinesDto>>(entity.BillsofMaterialLines.ToList());

            return new SuccessDataResult<SelectBillsofMaterialsDto>(mappedEntity);
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListBillsofMaterialsDto>>> GetListAsync(ListBillsofMaterialsParameterDto input)
        {
            var list = await _repository.GetListAsync(null,
                t => t.BillsofMaterialLines,
                t => t.Products);

            var mappedEntity = ObjectMapper.Map<List<BillsofMaterials>, List<ListBillsofMaterialsDto>>(list.ToList());

            return new SuccessDataResult<IList<ListBillsofMaterialsDto>>(mappedEntity);
        }

        [ValidationAspect(typeof(UpdateBillsofMaterialsValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectBillsofMaterialsDto>> UpdateAsync(UpdateBillsofMaterialsDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            await _manager.UpdateControl(_repository, input.Code, input.Id, entity);

            var mappedEntity = ObjectMapper.Map<UpdateBillsofMaterialsDto, BillsofMaterials>(input);

            await _repository.UpdateAsync(mappedEntity);

            foreach (var item in input.SelectBillsofMaterialLines)
            {
                var lineEntity = ObjectMapper.Map<SelectBillsofMaterialLinesDto, BillsofMaterialLines>(item);
                lineEntity.BoMID = mappedEntity.Id;

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

            return new SuccessDataResult<SelectBillsofMaterialsDto>(ObjectMapper.Map<BillsofMaterials, SelectBillsofMaterialsDto>(mappedEntity));
        }
    }
}

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
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
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

        BillsofMaterialManager _manager { get; set; } = new BillsofMaterialManager();


        [ValidationAspect(typeof(CreateBillsofMaterialsValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectBillsofMaterialsDto>> CreateAsync(CreateBillsofMaterialsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.CodeControl(_uow.BillsofMaterialsRepository, input.Code);

                var entity = ObjectMapper.Map<CreateBillsofMaterialsDto, BillsofMaterials>(input);

                var addedEntity = await _uow.BillsofMaterialsRepository.InsertAsync(entity);

                foreach (var item in input.SelectBillsofMaterialLines)
                {
                    var lineEntity = ObjectMapper.Map<SelectBillsofMaterialLinesDto, BillsofMaterialLines>(item);
                    lineEntity.Id = GuidGenerator.CreateGuid();
                    lineEntity.BoMID = addedEntity.Id;
                    await _uow.BillsofMaterialLinesRepository.InsertAsync(lineEntity);
                }

                await _uow.SaveChanges();
                return new SuccessDataResult<SelectBillsofMaterialsDto>(ObjectMapper.Map<BillsofMaterials, SelectBillsofMaterialsDto>(addedEntity));
            }
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var lines = (await _uow.BillsofMaterialLinesRepository.GetAsync(t => t.Id == id));

                if (lines != null)
                {
                    await _manager.DeleteControl(_uow.BillsofMaterialsRepository, lines.BoMID, lines.Id, true);
                    await _uow.BillsofMaterialLinesRepository.DeleteAsync(id);
                    await _uow.SaveChanges();
                    return new SuccessResult("Silme işlemi başarılı.");
                }
                else
                {
                    await _manager.DeleteControl(_uow.BillsofMaterialsRepository, id, Guid.Empty, false);
                    await _uow.BillsofMaterialsRepository.DeleteAsync(id);
                    await _uow.SaveChanges();
                    return new SuccessResult("Silme işlemi başarılı.");
                }
            }
        }

        public async Task<IDataResult<SelectBillsofMaterialsDto>> GetAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.BillsofMaterialsRepository.GetAsync(t => t.Id == id,
                t => t.BillsofMaterialLines,
                t => t.Products);

                var mappedEntity = ObjectMapper.Map<BillsofMaterials, SelectBillsofMaterialsDto>(entity);

                mappedEntity.SelectBillsofMaterialLines = ObjectMapper.Map<List<BillsofMaterialLines>, List<SelectBillsofMaterialLinesDto>>(entity.BillsofMaterialLines.ToList());

                return new SuccessDataResult<SelectBillsofMaterialsDto>(mappedEntity);
            }
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListBillsofMaterialsDto>>> GetListAsync(ListBillsofMaterialsParameterDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var list = await _uow.BillsofMaterialsRepository.GetListAsync(null,
                t => t.BillsofMaterialLines,
                t => t.Products);

                var mappedEntity = ObjectMapper.Map<List<BillsofMaterials>, List<ListBillsofMaterialsDto>>(list.ToList());

                return new SuccessDataResult<IList<ListBillsofMaterialsDto>>(mappedEntity);
            }
        }

        [ValidationAspect(typeof(UpdateBillsofMaterialsValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectBillsofMaterialsDto>> UpdateAsync(UpdateBillsofMaterialsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.BillsofMaterialsRepository.GetAsync(x => x.Id == input.Id);

                await _manager.UpdateControl(_uow.BillsofMaterialsRepository, input.Code, input.Id, entity);

                var mappedEntity = ObjectMapper.Map<UpdateBillsofMaterialsDto, BillsofMaterials>(input);

                await _uow.BillsofMaterialsRepository.UpdateAsync(mappedEntity);

                foreach (var item in input.SelectBillsofMaterialLines)
                {
                    var lineEntity = ObjectMapper.Map<SelectBillsofMaterialLinesDto, BillsofMaterialLines>(item);
                    lineEntity.BoMID = mappedEntity.Id;

                    if (lineEntity.Id == Guid.Empty)
                    {
                        lineEntity.Id = GuidGenerator.CreateGuid();
                        await _uow.BillsofMaterialLinesRepository.InsertAsync(lineEntity);
                    }
                    else
                    {
                        await _uow.BillsofMaterialLinesRepository.UpdateAsync(lineEntity);
                    }
                }

                await _uow.SaveChanges();
                return new SuccessDataResult<SelectBillsofMaterialsDto>(ObjectMapper.Map<BillsofMaterials, SelectBillsofMaterialsDto>(mappedEntity));
            }
        }

        public async Task<IDataResult<SelectBillsofMaterialsDto>> GetSelectListAsync(Guid finishedproductId)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.BillsofMaterialsRepository.GetAsync(t => t.FinishedProductID == finishedproductId,
               t => t.BillsofMaterialLines,
               t => t.Products);

                var mappedEntity = ObjectMapper.Map<BillsofMaterials, SelectBillsofMaterialsDto>(entity);

                mappedEntity.SelectBillsofMaterialLines = ObjectMapper.Map<List<BillsofMaterialLines>, List<SelectBillsofMaterialLinesDto>>(entity.BillsofMaterialLines.ToList());

                return new SuccessDataResult<SelectBillsofMaterialsDto>(mappedEntity);
            }
        }
    }
}

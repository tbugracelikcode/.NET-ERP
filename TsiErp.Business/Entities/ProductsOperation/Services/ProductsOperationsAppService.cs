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
using TsiErp.Business.Entities.ProductsOperation.BusinessRules;
using TsiErp.Business.Entities.ProductsOperation.Validations;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.ProductsOperation;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.ProductsOperationLine;
using TsiErp.Entities.Entities.ProductsOperation;
using TsiErp.Entities.Entities.ProductsOperation.Dtos;
using TsiErp.Entities.Entities.ProductsOperationLine;
using TsiErp.Entities.Entities.ProductsOperationLine.Dtos;

namespace TsiErp.Business.Entities.ProductsOperation.Services
{
    [ServiceRegistration(typeof(IProductsOperationsAppService), DependencyInjectionType.Scoped)]

    public class ProductsOperationsAppService : ApplicationService, IProductsOperationsAppService
    {
        private readonly IProductsOperationsRepository _repository;
        private readonly IProductsOperationLinesRepository _lineRepository;

        ProductsOperationManager _manager { get; set; } = new ProductsOperationManager();

        public ProductsOperationsAppService(IProductsOperationsRepository repository, IProductsOperationLinesRepository lineRepository)
        {
            _repository = repository;
            _lineRepository = lineRepository;
        }

        [ValidationAspect(typeof(CreateProductsOperationsValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectProductsOperationsDto>> CreateAsync(CreateProductsOperationsDto input)
        {
            await _manager.CodeControl(_repository, input.Code);

            var entity = ObjectMapper.Map<CreateProductsOperationsDto, ProductsOperations>(input);

            var addedEntity = await _repository.InsertAsync(entity);

            foreach (var item in input.SelectProductsOperationLines)
            {
                var lineEntity = ObjectMapper.Map<SelectProductsOperationLinesDto, ProductsOperationLines>(item);
                lineEntity.Id = GuidGenerator.CreateGuid();
                lineEntity.ProductsOperationID = addedEntity.Id;
                await _lineRepository.InsertAsync(lineEntity);
            }

            await _repository.SaveChanges();
            await _lineRepository.SaveChanges();
            return new SuccessDataResult<SelectProductsOperationsDto>(ObjectMapper.Map<ProductsOperations, SelectProductsOperationsDto>(addedEntity));
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var lines = (await _lineRepository.GetAsync(t => t.Id == id));

            if (lines != null)
            {
                await _manager.DeleteControl(_repository, lines.ProductsOperationID, lines.Id, true);
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

        public async Task<IDataResult<SelectProductsOperationsDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id,
                t => t.ProductsOperationLines,
                t=>t.Products);

            var mappedEntity = ObjectMapper.Map<ProductsOperations, SelectProductsOperationsDto>(entity);

            mappedEntity.SelectProductsOperationLines = ObjectMapper.Map<List<ProductsOperationLines>, List<SelectProductsOperationLinesDto>>(entity.ProductsOperationLines.ToList());

            return new SuccessDataResult<SelectProductsOperationsDto>(mappedEntity);
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListProductsOperationsDto>>> GetListAsync(ListProductsOperationsParameterDto input)
        {
            var list = await _repository.GetListAsync(null,
                t => t.ProductsOperationLines,
                t=>t.Products);

            var mappedEntity = ObjectMapper.Map<List<ProductsOperations>, List<ListProductsOperationsDto>>(list.ToList());

            return new SuccessDataResult<IList<ListProductsOperationsDto>>(mappedEntity);
        }

        [ValidationAspect(typeof(UpdateProductsOperationsValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectProductsOperationsDto>> UpdateAsync(UpdateProductsOperationsDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            await _manager.UpdateControl(_repository, input.Code, input.Id, entity);

            var mappedEntity = ObjectMapper.Map<UpdateProductsOperationsDto, ProductsOperations>(input);

            await _repository.UpdateAsync(mappedEntity);

            foreach (var item in input.SelectProductsOperationLines)
            {
                var lineEntity = ObjectMapper.Map<SelectProductsOperationLinesDto, ProductsOperationLines>(item);
                lineEntity.ProductsOperationID = mappedEntity.Id;

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

            return new SuccessDataResult<SelectProductsOperationsDto>(ObjectMapper.Map<ProductsOperations, SelectProductsOperationsDto>(mappedEntity));
        }
    }
}

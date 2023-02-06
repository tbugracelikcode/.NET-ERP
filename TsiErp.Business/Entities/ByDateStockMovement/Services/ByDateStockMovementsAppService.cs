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
using TsiErp.Business.Entities.Branch.Services;
using TsiErp.Business.Entities.Branch.Validations;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Branch;
using TsiErp.Entities.Entities.Branch.Dtos;
using TsiErp.Entities.Entities.Branch;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.ByDateStockMovement;
using TsiErp.Business.Entities.ByDateStockMovement.Validations;
using TsiErp.Entities.Entities.ByDateStockMovement.Dtos;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Entities.Entities.ByDateStockMovement;
using TsiErp.Business.Entities.ByDateStockMovement.BusinessRules;

namespace TsiErp.Business.Entities.ByDateStockMovement.Services
{
    [ServiceRegistration(typeof(IByDateStockMovementsAppService), DependencyInjectionType.Scoped)]
    public class ByDateStockMovementsAppService : ApplicationService, IByDateStockMovementsAppService
    {
        private readonly IByDateStockMovementsRepository _repository;

        ByDateStockMovementManager _manager { get; set; } = new ByDateStockMovementManager();

        public ByDateStockMovementsAppService(IByDateStockMovementsRepository repository)
        {
            _repository = repository;
        }


        [ValidationAspect(typeof(CreateByDateStockMovementsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectByDateStockMovementsDto>> CreateAsync(CreateByDateStockMovementsDto input)
        {
            var entity = ObjectMapper.Map<CreateByDateStockMovementsDto, ByDateStockMovements>(input);

            var addedEntity = await _repository.InsertAsync(entity);
            await _repository.SaveChanges();

            return new SuccessDataResult<SelectByDateStockMovementsDto>(ObjectMapper.Map<ByDateStockMovements, SelectByDateStockMovementsDto>(addedEntity));
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
            await _repository.SaveChanges();
            return new SuccessResult("Silme işlemi başarılı.");
        }


        public async Task<IDataResult<SelectByDateStockMovementsDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id, t => t.Branches, t => t.Warehouses, t => t.Products);
            var mappedEntity = ObjectMapper.Map<ByDateStockMovements, SelectByDateStockMovementsDto>(entity);
            return new SuccessDataResult<SelectByDateStockMovementsDto>(mappedEntity);
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListByDateStockMovementsDto>>> GetListAsync(ListByDateStockMovementsParameterDto input)
        {
            var list = await _repository.GetListAsync(null, t => t.Branches, t => t.Warehouses, t => t.Products);

            var mappedEntity = ObjectMapper.Map<List<ByDateStockMovements>, List<ListByDateStockMovementsDto>>(list.ToList());

            return new SuccessDataResult<IList<ListByDateStockMovementsDto>>(mappedEntity);
        }


        [ValidationAspect(typeof(UpdateByDateStockMovementsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectByDateStockMovementsDto>> UpdateAsync(UpdateByDateStockMovementsDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            var mappedEntity = ObjectMapper.Map<UpdateByDateStockMovementsDto, ByDateStockMovements>(input);

            await _repository.UpdateAsync(mappedEntity);
            await _repository.SaveChanges();

            return new SuccessDataResult<SelectByDateStockMovementsDto>(ObjectMapper.Map<ByDateStockMovements, SelectByDateStockMovementsDto>(mappedEntity));
        }
    }
}

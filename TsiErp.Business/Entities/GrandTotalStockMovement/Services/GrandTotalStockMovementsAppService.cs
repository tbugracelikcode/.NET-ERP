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
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.GrandTotalStockMovement;
using TsiErp.Business.Entities.GrandTotalStockMovement.Validations;
using TsiErp.Entities.Entities.GrandTotalStockMovement.Dtos;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Entities.Entities.GrandTotalStockMovement;
using TsiErp.Business.Entities.GrandTotalStockMovement.BusinessRules;

namespace TsiErp.Business.Entities.GrandTotalStockMovement.Services
{
    [ServiceRegistration(typeof(IGrandTotalStockMovementsAppService), DependencyInjectionType.Scoped)]
    public class GrandTotalStockMovementsAppService : ApplicationService, IGrandTotalStockMovementsAppService
    {
        private readonly IGrandTotalStockMovementsRepository _repository;

        GrandTotalStockMovementManager _manager { get; set; } = new GrandTotalStockMovementManager();

        public GrandTotalStockMovementsAppService(IGrandTotalStockMovementsRepository repository)
        {
            _repository = repository;
        }


        [ValidationAspect(typeof(CreateGrandTotalStockMovementsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectGrandTotalStockMovementsDto>> CreateAsync(CreateGrandTotalStockMovementsDto input)
        {
            var entity = ObjectMapper.Map<CreateGrandTotalStockMovementsDto, GrandTotalStockMovements>(input);

            var addedEntity = await _repository.InsertAsync(entity);
            await _repository.SaveChanges();

            return new SuccessDataResult<SelectGrandTotalStockMovementsDto>(ObjectMapper.Map<GrandTotalStockMovements, SelectGrandTotalStockMovementsDto>(addedEntity));
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
            await _repository.SaveChanges();
            return new SuccessResult("Silme işlemi başarılı.");
        }


        public async Task<IDataResult<SelectGrandTotalStockMovementsDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id, t => t.Branches, t => t.Warehouses, t => t.Products);
            var mappedEntity = ObjectMapper.Map<GrandTotalStockMovements, SelectGrandTotalStockMovementsDto>(entity);
            return new SuccessDataResult<SelectGrandTotalStockMovementsDto>(mappedEntity);
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListGrandTotalStockMovementsDto>>> GetListAsync(ListGrandTotalStockMovementsParameterDto input)
        {
            var list = await _repository.GetListAsync(null, t => t.Branches, t => t.Warehouses, t => t.Products);

            var mappedEntity = ObjectMapper.Map<List<GrandTotalStockMovements>, List<ListGrandTotalStockMovementsDto>>(list.ToList());

            return new SuccessDataResult<IList<ListGrandTotalStockMovementsDto>>(mappedEntity);
        }


        [ValidationAspect(typeof(UpdateGrandTotalStockMovementsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectGrandTotalStockMovementsDto>> UpdateAsync(UpdateGrandTotalStockMovementsDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            var mappedEntity = ObjectMapper.Map<UpdateGrandTotalStockMovementsDto, GrandTotalStockMovements>(input);

            await _repository.UpdateAsync(mappedEntity);
            await _repository.SaveChanges();

            return new SuccessDataResult<SelectGrandTotalStockMovementsDto>(ObjectMapper.Map<GrandTotalStockMovements, SelectGrandTotalStockMovementsDto>(mappedEntity));
        }
    }
}

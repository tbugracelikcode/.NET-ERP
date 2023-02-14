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
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;

namespace TsiErp.Business.Entities.ByDateStockMovement.Services
{
    [ServiceRegistration(typeof(IByDateStockMovementsAppService), DependencyInjectionType.Scoped)]
    public class ByDateStockMovementsAppService : ApplicationService, IByDateStockMovementsAppService
    {
        ByDateStockMovementManager _manager { get; set; } = new ByDateStockMovementManager();


        [ValidationAspect(typeof(CreateByDateStockMovementsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectByDateStockMovementsDto>> CreateAsync(CreateByDateStockMovementsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = ObjectMapper.Map<CreateByDateStockMovementsDto, ByDateStockMovements>(input);

                var addedEntity = await _uow.ByDateStockMovementsRepository.InsertAsync(entity);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectByDateStockMovementsDto>(ObjectMapper.Map<ByDateStockMovements, SelectByDateStockMovementsDto>(addedEntity));
            }
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _uow.ByDateStockMovementsRepository.DeleteAsync(id);
                await _uow.SaveChanges();
                return new SuccessResult("Silme işlemi başarılı.");
            }
        }


        public async Task<IDataResult<SelectByDateStockMovementsDto>> GetAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.ByDateStockMovementsRepository.GetAsync(t => t.Id == id, t => t.Branches, t => t.Warehouses, t => t.Products);
                var mappedEntity = ObjectMapper.Map<ByDateStockMovements, SelectByDateStockMovementsDto>(entity);
                return new SuccessDataResult<SelectByDateStockMovementsDto>(mappedEntity);
            }
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListByDateStockMovementsDto>>> GetListAsync(ListByDateStockMovementsParameterDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var list = await _uow.ByDateStockMovementsRepository.GetListAsync(null, t => t.Branches, t => t.Warehouses, t => t.Products);

                var mappedEntity = ObjectMapper.Map<List<ByDateStockMovements>, List<ListByDateStockMovementsDto>>(list.ToList());

                return new SuccessDataResult<IList<ListByDateStockMovementsDto>>(mappedEntity);
            }
        }


        [ValidationAspect(typeof(UpdateByDateStockMovementsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectByDateStockMovementsDto>> UpdateAsync(UpdateByDateStockMovementsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.ByDateStockMovementsRepository.GetAsync(x => x.Id == input.Id);

                var mappedEntity = ObjectMapper.Map<UpdateByDateStockMovementsDto, ByDateStockMovements>(input);

                await _uow.ByDateStockMovementsRepository.UpdateAsync(mappedEntity);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectByDateStockMovementsDto>(ObjectMapper.Map<ByDateStockMovements, SelectByDateStockMovementsDto>(mappedEntity));
            }
        }
    }
}

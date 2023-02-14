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
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.WorkOrder;
using TsiErp.Business.Entities.WorkOrder.Validations;
using TsiErp.Entities.Entities.WorkOrder.Dtos;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Entities.Entities.WorkOrder;
using TsiErp.Business.Entities.WorkOrder.BusinessRules;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;

namespace TsiErp.Business.Entities.WorkOrder.Services
{
    [ServiceRegistration(typeof(IWorkOrdersAppService), DependencyInjectionType.Scoped)]
    public class WorkOrdersAppService : ApplicationService, IWorkOrdersAppService
    {
        WorkOrderManager _manager { get; set; } = new WorkOrderManager();

        [ValidationAspect(typeof(CreateWorkOrdersValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectWorkOrdersDto>> CreateAsync(CreateWorkOrdersDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.CodeControl(_uow.WorkOrdersRepository, input.Code);

                var entity = ObjectMapper.Map<CreateWorkOrdersDto, WorkOrders>(input);

                var addedEntity = await _uow.WorkOrdersRepository.InsertAsync(entity);
                await _uow.SaveChanges();


                return new SuccessDataResult<SelectWorkOrdersDto>(ObjectMapper.Map<WorkOrders, SelectWorkOrdersDto>(addedEntity));
            }
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _uow.WorkOrdersRepository.DeleteAsync(id);
                await _uow.SaveChanges();
                return new SuccessResult("Silme işlemi başarılı.");
            }
        }


        public async Task<IDataResult<SelectWorkOrdersDto>> GetAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.WorkOrdersRepository.GetAsync(t => t.Id == id,
                t => t.ProductionOrders,
                t => t.SalesPropositions,
                t => t.Routes,
                t => t.ProductsOperations,
                t => t.Stations,
                t => t.StationGroups,
                t => t.Products,
                t => t.CurrentAccountCards);
                var mappedEntity = ObjectMapper.Map<WorkOrders, SelectWorkOrdersDto>(entity);
                return new SuccessDataResult<SelectWorkOrdersDto>(mappedEntity);
            }
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListWorkOrdersDto>>> GetListAsync(ListWorkOrdersParameterDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var list = await _uow.WorkOrdersRepository.GetListAsync(null,
                t => t.ProductionOrders,
                t => t.SalesPropositions,
                t => t.Routes,
                t => t.ProductsOperations,
                t => t.Stations,
                t => t.StationGroups,
                t => t.Products,
                t => t.CurrentAccountCards);

                var mappedEntity = ObjectMapper.Map<List<WorkOrders>, List<ListWorkOrdersDto>>(list.ToList());

                return new SuccessDataResult<IList<ListWorkOrdersDto>>(mappedEntity);
            }
        }


        [ValidationAspect(typeof(UpdateWorkOrdersValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectWorkOrdersDto>> UpdateAsync(UpdateWorkOrdersDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.WorkOrdersRepository.GetAsync(x => x.Id == input.Id);

                await _manager.UpdateControl(_uow.WorkOrdersRepository, input.Code, input.Id, entity);

                var mappedEntity = ObjectMapper.Map<UpdateWorkOrdersDto, WorkOrders>(input);

                await _uow.WorkOrdersRepository.UpdateAsync(mappedEntity);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectWorkOrdersDto>(ObjectMapper.Map<WorkOrders, SelectWorkOrdersDto>(mappedEntity));
            }
        }
    }
}

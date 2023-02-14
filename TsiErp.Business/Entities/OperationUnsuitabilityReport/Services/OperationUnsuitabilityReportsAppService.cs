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
using TsiErp.Business.Entities.OperationUnsuitabilityReport.Validations;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.OperationUnsuitabilityReport;
using TsiErp.Entities.Entities.Branch.Dtos;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Entities.Entities.OperationUnsuitabilityReport.Dtos;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Entities.Entities.OperationUnsuitabilityReport;
using TsiErp.Business.Entities.OperationUnsuitabilityReport.BusinessRules;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.Entities.Entities.WorkOrder.Dtos;
using TsiErp.Entities.Entities.WorkOrder;

namespace TsiErp.Business.Entities.OperationUnsuitabilityReport.Services
{
    [ServiceRegistration(typeof(IOperationUnsuitabilityReportsAppService), DependencyInjectionType.Scoped)]
    public class OperationUnsuitabilityReportsAppService : ApplicationService, IOperationUnsuitabilityReportsAppService
    {
        OperationUnsuitabilityReportManager _manager { get; set; } = new OperationUnsuitabilityReportManager();

        [ValidationAspect(typeof(CreateOperationUnsuitabilityReportsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectOperationUnsuitabilityReportsDto>> CreateAsync(CreateOperationUnsuitabilityReportsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.CodeControl(_uow.OperationUnsuitabilityReportsRepository, input.FicheNo);

                var entity = ObjectMapper.Map<CreateOperationUnsuitabilityReportsDto, OperationUnsuitabilityReports>(input);

                var addedEntity = await _uow.OperationUnsuitabilityReportsRepository.InsertAsync(entity);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectOperationUnsuitabilityReportsDto>(ObjectMapper.Map<OperationUnsuitabilityReports, SelectOperationUnsuitabilityReportsDto>(addedEntity));
            }
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.DeleteControl(_uow.OperationUnsuitabilityReportsRepository, id);
                await _uow.OperationUnsuitabilityReportsRepository.DeleteAsync(id);
                await _uow.SaveChanges();
                return new SuccessResult("Silme işlemi başarılı.");
            }
        }

        public async Task<IDataResult<SelectOperationUnsuitabilityReportsDto>> GetAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.OperationUnsuitabilityReportsRepository.GetAsync(t => t.Id == id,
                t => t.Products,
                t => t.ProductionOrders,
                t => t.ProductsOperations,
                t => t.WorkOrders,
                t => t.StationGroups,
                t => t.Employees,
                t => t.Stations);
                var mappedEntity = ObjectMapper.Map<OperationUnsuitabilityReports, SelectOperationUnsuitabilityReportsDto>(entity);
                return new SuccessDataResult<SelectOperationUnsuitabilityReportsDto>(mappedEntity);
            }
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListOperationUnsuitabilityReportsDto>>> GetListAsync(ListOperationUnsuitabilityReportsParameterDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var list = await _uow.OperationUnsuitabilityReportsRepository.GetListAsync(null,
                t => t.Products,
                t => t.ProductionOrders,
                t => t.ProductsOperations,
                t => t.WorkOrders,
                t => t.StationGroups,
                t => t.Employees,
                t => t.Stations);

                var mappedEntity = ObjectMapper.Map<List<OperationUnsuitabilityReports>, List<ListOperationUnsuitabilityReportsDto>>(list.ToList());

                return new SuccessDataResult<IList<ListOperationUnsuitabilityReportsDto>>(mappedEntity);
            }
        }


        [ValidationAspect(typeof(UpdateOperationUnsuitabilityReportsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectOperationUnsuitabilityReportsDto>> UpdateAsync(UpdateOperationUnsuitabilityReportsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.OperationUnsuitabilityReportsRepository.GetAsync(x => x.Id == input.Id);

                await _manager.UpdateControl(_uow.OperationUnsuitabilityReportsRepository, input.FicheNo, input.Id, entity);

                var mappedEntity = ObjectMapper.Map<UpdateOperationUnsuitabilityReportsDto, OperationUnsuitabilityReports>(input);

                await _uow.OperationUnsuitabilityReportsRepository.UpdateAsync(mappedEntity);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectOperationUnsuitabilityReportsDto>(ObjectMapper.Map<OperationUnsuitabilityReports, SelectOperationUnsuitabilityReportsDto>(mappedEntity));
            }
        }

        public async Task<IDataResult<SelectOperationUnsuitabilityReportsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.OperationUnsuitabilityReportsRepository.GetAsync(x => x.Id == id);

                var updatedEntity = await _uow.OperationUnsuitabilityReportsRepository.LockRow(entity.Id, lockRow, userId);

                await _uow.SaveChanges();

                var mappedEntity = ObjectMapper.Map<OperationUnsuitabilityReports, SelectOperationUnsuitabilityReportsDto>(updatedEntity);

                return new SuccessDataResult<SelectOperationUnsuitabilityReportsDto>(mappedEntity);
            }
        }
    }
}

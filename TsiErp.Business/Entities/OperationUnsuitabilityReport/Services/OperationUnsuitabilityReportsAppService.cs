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

namespace TsiErp.Business.Entities.OperationUnsuitabilityReport.Services
{
    [ServiceRegistration(typeof(IOperationUnsuitabilityReportsAppService), DependencyInjectionType.Scoped)]
    public class OperationUnsuitabilityReportsAppService : ApplicationService, IOperationUnsuitabilityReportsAppService
    {
        private readonly IOperationUnsuitabilityReportsRepository _repository;

        OperationUnsuitabilityReportManager _manager { get; set; } = new OperationUnsuitabilityReportManager();

        public OperationUnsuitabilityReportsAppService(IOperationUnsuitabilityReportsRepository repository)
        {
            _repository = repository;
        }

        [ValidationAspect(typeof(CreateOperationUnsuitabilityReportsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectOperationUnsuitabilityReportsDto>> CreateAsync(CreateOperationUnsuitabilityReportsDto input)
        {
            await _manager.CodeControl(_repository, input.FicheNo);

            var entity = ObjectMapper.Map<CreateOperationUnsuitabilityReportsDto, OperationUnsuitabilityReports>(input);

            var addedEntity = await _repository.InsertAsync(entity);
            await _repository.SaveChanges();

            return new SuccessDataResult<SelectOperationUnsuitabilityReportsDto>(ObjectMapper.Map<OperationUnsuitabilityReports, SelectOperationUnsuitabilityReportsDto>(addedEntity));
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            await _manager.DeleteControl(_repository, id);
            await _repository.DeleteAsync(id);
            await _repository.SaveChanges();
            return new SuccessResult("Silme işlemi başarılı.");
        }

        public async Task<IDataResult<SelectOperationUnsuitabilityReportsDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id,
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

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListOperationUnsuitabilityReportsDto>>> GetListAsync(ListOperationUnsuitabilityReportsParameterDto input)
        {
            var list = await _repository.GetListAsync(null,
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


        [ValidationAspect(typeof(UpdateOperationUnsuitabilityReportsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectOperationUnsuitabilityReportsDto>> UpdateAsync(UpdateOperationUnsuitabilityReportsDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            await _manager.UpdateControl(_repository, input.FicheNo, input.Id, entity);

            var mappedEntity = ObjectMapper.Map<UpdateOperationUnsuitabilityReportsDto, OperationUnsuitabilityReports>(input);

            await _repository.UpdateAsync(mappedEntity);
            await _repository.SaveChanges();

            return new SuccessDataResult<SelectOperationUnsuitabilityReportsDto>(ObjectMapper.Map<OperationUnsuitabilityReports, SelectOperationUnsuitabilityReportsDto>(mappedEntity));
        }
    }
}

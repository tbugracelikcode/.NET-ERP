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
using TsiErp.Business.Entities.FinalControlUnsuitabilityReport.Validations;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.FinalControlUnsuitabilityReport;
using TsiErp.Entities.Entities.Branch.Dtos;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Entities.Entities.FinalControlUnsuitabilityReport.Dtos;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Entities.Entities.FinalControlUnsuitabilityReport;
using TsiErp.Business.Entities.FinalControlUnsuitabilityReport.BusinessRules;

namespace TsiErp.Business.Entities.FinalControlUnsuitabilityReport.Services
{
    [ServiceRegistration(typeof(IFinalControlUnsuitabilityReportsAppService), DependencyInjectionType.Scoped)]
    public class FinalControlUnsuitabilityReportsAppService : ApplicationService, IFinalControlUnsuitabilityReportsAppService
    {
        private readonly IFinalControlUnsuitabilityReportsRepository _repository;

        FinalControlUnsuitabilityReportManager _manager { get; set; } = new FinalControlUnsuitabilityReportManager();

        public FinalControlUnsuitabilityReportsAppService(IFinalControlUnsuitabilityReportsRepository repository)
        {
            _repository = repository;
        }

        [ValidationAspect(typeof(CreateFinalControlUnsuitabilityReportsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectFinalControlUnsuitabilityReportsDto>> CreateAsync(CreateFinalControlUnsuitabilityReportsDto input)
        {
            await _manager.CodeControl(_repository, input.FicheNo);

            var entity = ObjectMapper.Map<CreateFinalControlUnsuitabilityReportsDto, FinalControlUnsuitabilityReports>(input);

            var addedEntity = await _repository.InsertAsync(entity);
            await _repository.SaveChanges();

            return new SuccessDataResult<SelectFinalControlUnsuitabilityReportsDto>(ObjectMapper.Map<FinalControlUnsuitabilityReports, SelectFinalControlUnsuitabilityReportsDto>(addedEntity));
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            await _manager.DeleteControl(_repository, id);
            await _repository.DeleteAsync(id);
            await _repository.SaveChanges();
            return new SuccessResult("Silme işlemi başarılı.");
        }

        public async Task<IDataResult<SelectFinalControlUnsuitabilityReportsDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id,
                t => t.Products,
                t => t.Employees);
            var mappedEntity = ObjectMapper.Map<FinalControlUnsuitabilityReports, SelectFinalControlUnsuitabilityReportsDto>(entity);
            return new SuccessDataResult<SelectFinalControlUnsuitabilityReportsDto>(mappedEntity);
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListFinalControlUnsuitabilityReportsDto>>> GetListAsync(ListFinalControlUnsuitabilityReportsParameterDto input)
        {
            var list = await _repository.GetListAsync(null,
                t => t.Products,
                t => t.Employees);

            var mappedEntity = ObjectMapper.Map<List<FinalControlUnsuitabilityReports>, List<ListFinalControlUnsuitabilityReportsDto>>(list.ToList());

            return new SuccessDataResult<IList<ListFinalControlUnsuitabilityReportsDto>>(mappedEntity);
        }


        [ValidationAspect(typeof(UpdateFinalControlUnsuitabilityReportsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectFinalControlUnsuitabilityReportsDto>> UpdateAsync(UpdateFinalControlUnsuitabilityReportsDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            await _manager.UpdateControl(_repository, input.FicheNo, input.Id, entity);

            var mappedEntity = ObjectMapper.Map<UpdateFinalControlUnsuitabilityReportsDto, FinalControlUnsuitabilityReports>(input);

            await _repository.UpdateAsync(mappedEntity);
            await _repository.SaveChanges();

            return new SuccessDataResult<SelectFinalControlUnsuitabilityReportsDto>(ObjectMapper.Map<FinalControlUnsuitabilityReports, SelectFinalControlUnsuitabilityReportsDto>(mappedEntity));
        }
    }
}

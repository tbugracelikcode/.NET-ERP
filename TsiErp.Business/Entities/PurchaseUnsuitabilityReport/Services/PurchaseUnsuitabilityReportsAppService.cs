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
using TsiErp.Business.Entities.PurchaseUnsuitabilityReport.Validations;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.PurchaseUnsuitabilityReport;
using TsiErp.Entities.Entities.Branch.Dtos;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Entities.Entities.PurchaseUnsuitabilityReport.Dtos;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Entities.Entities.PurchaseUnsuitabilityReport;
using TsiErp.Business.Entities.PurchaseUnsuitabilityReport.BusinessRules;

namespace TsiErp.Business.Entities.PurchaseUnsuitabilityReport.Services
{
    [ServiceRegistration(typeof(IPurchaseUnsuitabilityReportsAppService), DependencyInjectionType.Scoped)]
    public class PurchaseUnsuitabilityReportsAppService : ApplicationService, IPurchaseUnsuitabilityReportsAppService
    {
        private readonly IPurchaseUnsuitabilityReportsRepository _repository;

        PurchaseUnsuitabilityReportManager _manager { get; set; } = new PurchaseUnsuitabilityReportManager();

        public PurchaseUnsuitabilityReportsAppService(IPurchaseUnsuitabilityReportsRepository repository)
        {
            _repository = repository;
        }

        [ValidationAspect(typeof(CreatePurchaseUnsuitabilityReportsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPurchaseUnsuitabilityReportsDto>> CreateAsync(CreatePurchaseUnsuitabilityReportsDto input)
        {
            await _manager.CodeControl(_repository, input.FicheNo);

            var entity = ObjectMapper.Map<CreatePurchaseUnsuitabilityReportsDto, PurchaseUnsuitabilityReports>(input);

            var addedEntity = await _repository.InsertAsync(entity);
            await _repository.SaveChanges();

            return new SuccessDataResult<SelectPurchaseUnsuitabilityReportsDto>(ObjectMapper.Map<PurchaseUnsuitabilityReports, SelectPurchaseUnsuitabilityReportsDto>(addedEntity));
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            await _manager.DeleteControl(_repository, id);
            await _repository.DeleteAsync(id);
            await _repository.SaveChanges();
            return new SuccessResult("Silme işlemi başarılı.");
        }

        public async Task<IDataResult<SelectPurchaseUnsuitabilityReportsDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id,
                t => t.Products, 
                t => t.CurrentAccountCards, 
                t => t.PurchaseOrders);
            var mappedEntity = ObjectMapper.Map<PurchaseUnsuitabilityReports, SelectPurchaseUnsuitabilityReportsDto>(entity);
            return new SuccessDataResult<SelectPurchaseUnsuitabilityReportsDto>(mappedEntity);
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListPurchaseUnsuitabilityReportsDto>>> GetListAsync(ListPurchaseUnsuitabilityReportsParameterDto input)
        {
            var list = await _repository.GetListAsync(null,
                t => t.Products,
                 t => t.CurrentAccountCards,
                t => t.PurchaseOrders);

            var mappedEntity = ObjectMapper.Map<List<PurchaseUnsuitabilityReports>, List<ListPurchaseUnsuitabilityReportsDto>>(list.ToList());

            return new SuccessDataResult<IList<ListPurchaseUnsuitabilityReportsDto>>(mappedEntity);
        }


        [ValidationAspect(typeof(UpdatePurchaseUnsuitabilityReportsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPurchaseUnsuitabilityReportsDto>> UpdateAsync(UpdatePurchaseUnsuitabilityReportsDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            await _manager.UpdateControl(_repository, input.FicheNo, input.Id, entity);

            var mappedEntity = ObjectMapper.Map<UpdatePurchaseUnsuitabilityReportsDto, PurchaseUnsuitabilityReports>(input);

            await _repository.UpdateAsync(mappedEntity);
            await _repository.SaveChanges();

            return new SuccessDataResult<SelectPurchaseUnsuitabilityReportsDto>(ObjectMapper.Map<PurchaseUnsuitabilityReports, SelectPurchaseUnsuitabilityReportsDto>(mappedEntity));
        }
    }
}

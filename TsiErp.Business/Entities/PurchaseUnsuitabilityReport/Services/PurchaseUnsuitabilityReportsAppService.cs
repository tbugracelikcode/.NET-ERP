using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using TsiErp.Localizations.Resources.PurchaseUnsuitabilityReports.Page;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.PurchaseUnsuitabilityReport.BusinessRules;
using TsiErp.Business.Entities.PurchaseUnsuitabilityReport.Validations;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.PurchaseUnsuitabilityReport;
using TsiErp.Entities.Entities.PurchaseUnsuitabilityReport.Dtos;
using Microsoft.Extensions.Localization;

namespace TsiErp.Business.Entities.PurchaseUnsuitabilityReport.Services
{
    [ServiceRegistration(typeof(IPurchaseUnsuitabilityReportsAppService), DependencyInjectionType.Scoped)]
    public class PurchaseUnsuitabilityReportsAppService : ApplicationService<PurchaseUnsuitabilityReportsResource>, IPurchaseUnsuitabilityReportsAppService
    {
        public PurchaseUnsuitabilityReportsAppService(IStringLocalizer<PurchaseUnsuitabilityReportsResource> l) : base(l)
        {
        }

        PurchaseUnsuitabilityReportManager _manager { get; set; } = new PurchaseUnsuitabilityReportManager();

        [ValidationAspect(typeof(CreatePurchaseUnsuitabilityReportsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPurchaseUnsuitabilityReportsDto>> CreateAsync(CreatePurchaseUnsuitabilityReportsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.CodeControl(_uow.PurchaseUnsuitabilityReportsRepository, input.FicheNo,L);

                var entity = ObjectMapper.Map<CreatePurchaseUnsuitabilityReportsDto, PurchaseUnsuitabilityReports>(input);

                var addedEntity = await _uow.PurchaseUnsuitabilityReportsRepository.InsertAsync(entity);
                input.Id = addedEntity.Id;
                var log = LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, "PurchaseUnsuitabilityReports", LogType.Insert, addedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);

                await _uow.SaveChanges();

                return new SuccessDataResult<SelectPurchaseUnsuitabilityReportsDto>(ObjectMapper.Map<PurchaseUnsuitabilityReports, SelectPurchaseUnsuitabilityReportsDto>(addedEntity));
            }
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.DeleteControl(_uow.PurchaseUnsuitabilityReportsRepository, id);
                await _uow.PurchaseUnsuitabilityReportsRepository.DeleteAsync(id);
                var log = LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, "PurchaseUnsuitabilityReports", LogType.Delete, id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();
                return new SuccessResult(L["DeleteSuccessMessage"]);
            }
        }

        public async Task<IDataResult<SelectPurchaseUnsuitabilityReportsDto>> GetAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.PurchaseUnsuitabilityReportsRepository.GetAsync(t => t.Id == id);
                var mappedEntity = ObjectMapper.Map<PurchaseUnsuitabilityReports, SelectPurchaseUnsuitabilityReportsDto>(entity);
                var log = LogsAppService.InsertLogToDatabase(mappedEntity, mappedEntity, LoginedUserService.UserId, "PurchaseUnsuitabilityReports", LogType.Get, id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();
                return new SuccessDataResult<SelectPurchaseUnsuitabilityReportsDto>(mappedEntity);
            }
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListPurchaseUnsuitabilityReportsDto>>> GetListAsync(ListPurchaseUnsuitabilityReportsParameterDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var list = await _uow.PurchaseUnsuitabilityReportsRepository.GetListAsync(null);

                var mappedEntity = ObjectMapper.Map<List<PurchaseUnsuitabilityReports>, List<ListPurchaseUnsuitabilityReportsDto>>(list.ToList());

                return new SuccessDataResult<IList<ListPurchaseUnsuitabilityReportsDto>>(mappedEntity);
            }
        }


        [ValidationAspect(typeof(UpdatePurchaseUnsuitabilityReportsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectPurchaseUnsuitabilityReportsDto>> UpdateAsync(UpdatePurchaseUnsuitabilityReportsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.PurchaseUnsuitabilityReportsRepository.GetAsync(x => x.Id == input.Id);

                await _manager.UpdateControl(_uow.PurchaseUnsuitabilityReportsRepository, input.FicheNo, input.Id, entity,L);

                var mappedEntity = ObjectMapper.Map<UpdatePurchaseUnsuitabilityReportsDto, PurchaseUnsuitabilityReports>(input);

                await _uow.PurchaseUnsuitabilityReportsRepository.UpdateAsync(mappedEntity);
                var before = ObjectMapper.Map<PurchaseUnsuitabilityReports, UpdatePurchaseUnsuitabilityReportsDto>(entity);
                var log = LogsAppService.InsertLogToDatabase(before, input, LoginedUserService.UserId, "PurchaseUnsuitabilityReports", LogType.Update, mappedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectPurchaseUnsuitabilityReportsDto>(ObjectMapper.Map<PurchaseUnsuitabilityReports, SelectPurchaseUnsuitabilityReportsDto>(mappedEntity));
            }
        }

        public async Task<IDataResult<SelectPurchaseUnsuitabilityReportsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.PurchaseUnsuitabilityReportsRepository.GetAsync(x => x.Id == id);

                var updatedEntity = await _uow.PurchaseUnsuitabilityReportsRepository.LockRow(entity.Id, lockRow, userId);

                await _uow.SaveChanges();

                var mappedEntity = ObjectMapper.Map<PurchaseUnsuitabilityReports, SelectPurchaseUnsuitabilityReportsDto>(updatedEntity);

                return new SuccessDataResult<SelectPurchaseUnsuitabilityReportsDto>(mappedEntity);
            }
        }
    }
}

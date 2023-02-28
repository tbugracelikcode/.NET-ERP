using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results; using TsiErp.Localizations.Resources.Branches.Page;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.FinalControlUnsuitabilityReport.BusinessRules;
using TsiErp.Business.Entities.FinalControlUnsuitabilityReport.Validations;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinalControlUnsuitabilityReport;
using TsiErp.Entities.Entities.FinalControlUnsuitabilityReport.Dtos;
using Microsoft.Extensions.Localization;

namespace TsiErp.Business.Entities.FinalControlUnsuitabilityReport.Services
{
    [ServiceRegistration(typeof(IFinalControlUnsuitabilityReportsAppService), DependencyInjectionType.Scoped)]
    public class FinalControlUnsuitabilityReportsAppService : ApplicationService<BranchesResource>, IFinalControlUnsuitabilityReportsAppService
    {
        public FinalControlUnsuitabilityReportsAppService(IStringLocalizer<BranchesResource> l) : base(l)
        {
        }

        FinalControlUnsuitabilityReportManager _manager { get; set; } = new FinalControlUnsuitabilityReportManager();

        [ValidationAspect(typeof(CreateFinalControlUnsuitabilityReportsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectFinalControlUnsuitabilityReportsDto>> CreateAsync(CreateFinalControlUnsuitabilityReportsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.CodeControl(_uow.FinalControlUnsuitabilityReportsRepository, input.FicheNo);

                var entity = ObjectMapper.Map<CreateFinalControlUnsuitabilityReportsDto, FinalControlUnsuitabilityReports>(input);

                var addedEntity = await _uow.FinalControlUnsuitabilityReportsRepository.InsertAsync(entity);
                input.Id = addedEntity.Id;
                var log = LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, "FinalControlUnsuitabilityReports", LogType.Insert, addedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectFinalControlUnsuitabilityReportsDto>(ObjectMapper.Map<FinalControlUnsuitabilityReports, SelectFinalControlUnsuitabilityReportsDto>(addedEntity));
            }
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.DeleteControl(_uow.FinalControlUnsuitabilityReportsRepository, id);
                await _uow.FinalControlUnsuitabilityReportsRepository.DeleteAsync(id);
                var log = LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, "FinalControlUnsuitabilityReports", LogType.Delete, id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();
                return new SuccessResult("Silme işlemi başarılı.");
            }
        }

        public async Task<IDataResult<SelectFinalControlUnsuitabilityReportsDto>> GetAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.FinalControlUnsuitabilityReportsRepository.GetAsync(t => t.Id == id,
                t => t.Products,
                t => t.Employees);
                var mappedEntity = ObjectMapper.Map<FinalControlUnsuitabilityReports, SelectFinalControlUnsuitabilityReportsDto>(entity);
                var log = LogsAppService.InsertLogToDatabase(mappedEntity, mappedEntity, LoginedUserService.UserId, "FinalControlUnsuitabilityReports", LogType.Get, id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectFinalControlUnsuitabilityReportsDto>(mappedEntity);
            }
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListFinalControlUnsuitabilityReportsDto>>> GetListAsync(ListFinalControlUnsuitabilityReportsParameterDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var list = await _uow.FinalControlUnsuitabilityReportsRepository.GetListAsync(null,
                t => t.Products,
                t => t.Employees);

                var mappedEntity = ObjectMapper.Map<List<FinalControlUnsuitabilityReports>, List<ListFinalControlUnsuitabilityReportsDto>>(list.ToList());

                return new SuccessDataResult<IList<ListFinalControlUnsuitabilityReportsDto>>(mappedEntity);
            }
        }


        [ValidationAspect(typeof(UpdateFinalControlUnsuitabilityReportsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectFinalControlUnsuitabilityReportsDto>> UpdateAsync(UpdateFinalControlUnsuitabilityReportsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.FinalControlUnsuitabilityReportsRepository.GetAsync(x => x.Id == input.Id);

                await _manager.UpdateControl(_uow.FinalControlUnsuitabilityReportsRepository, input.FicheNo, input.Id, entity);

                var mappedEntity = ObjectMapper.Map<UpdateFinalControlUnsuitabilityReportsDto, FinalControlUnsuitabilityReports>(input);

                await _uow.FinalControlUnsuitabilityReportsRepository.UpdateAsync(mappedEntity);
                var before = ObjectMapper.Map<FinalControlUnsuitabilityReports, UpdateFinalControlUnsuitabilityReportsDto>(entity);
                var log = LogsAppService.InsertLogToDatabase(before, input, LoginedUserService.UserId, "FinalControlUnsuitabilityReports", LogType.Update, mappedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectFinalControlUnsuitabilityReportsDto>(ObjectMapper.Map<FinalControlUnsuitabilityReports, SelectFinalControlUnsuitabilityReportsDto>(mappedEntity));
            }
        }

        public async Task<IDataResult<SelectFinalControlUnsuitabilityReportsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.FinalControlUnsuitabilityReportsRepository.GetAsync(x => x.Id == id);

                var updatedEntity = await _uow.FinalControlUnsuitabilityReportsRepository.LockRow(entity.Id, lockRow, userId);

                await _uow.SaveChanges();

                var mappedEntity = ObjectMapper.Map<FinalControlUnsuitabilityReports, SelectFinalControlUnsuitabilityReportsDto>(updatedEntity);

                return new SuccessDataResult<SelectFinalControlUnsuitabilityReportsDto>(mappedEntity);
            }
        }
    }
}

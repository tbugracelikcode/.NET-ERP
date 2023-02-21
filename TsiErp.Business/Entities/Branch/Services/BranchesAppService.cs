using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Entities;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Branch.BusinessRules;
using TsiErp.Business.Entities.Branch.Validations;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Entities.Entities.Branch.Dtos;

namespace TsiErp.Business.Entities.Branch.Services
{
    [ServiceRegistration(typeof(IBranchesAppService), DependencyInjectionType.Scoped)]
    public class BranchesAppService : ApplicationService, IBranchesAppService
    {
        BranchesManager _manager { get; set; } = new BranchesManager();

        [ValidationAspect(typeof(CreateBranchesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectBranchesDto>> CreateAsync(CreateBranchesDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.CodeControl(_uow.BranchRepository, input.Code);

                var entity = ObjectMapper.Map<CreateBranchesDto, Branches>(input);

                var addedEntity = await _uow.BranchRepository.InsertAsync(entity);

                var beforeLogEntity = ObjectMapper.Map<Branches,SelectBranchesDto >(addedEntity);

                //var log = LogsAppService.CreateLogObject(beforeLogEntity, beforeLogEntity, LoginedUserService.UserId, "Branches", addedEntity.Id);
                var log = LogsAppService.InsertLogToDatabase<Branches, SelectBranchesDto>(addedEntity, LoginedUserService.UserId, "Branches", LogType.Insert, addedEntity.Id);

                await _uow.LogsRepository.InsertAsync(log);

                await _uow.SaveChanges();

                return new SuccessDataResult<SelectBranchesDto>(ObjectMapper.Map<Branches, SelectBranchesDto>(addedEntity));
            }
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.DeleteControl(_uow.BranchRepository, id);
                await _uow.BranchRepository.DeleteAsync(id);
                //var log = LogsAppService.DeleteLogObject(null, null, LoginedUserService.UserId, "Branches", id);
                var log = LogsAppService.InsertLogToDatabase<Branches, SelectBranchesDto>(null, LoginedUserService.UserId, "Branches", LogType.Delete, id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();
                return new SuccessResult("Silme işlemi başarılı.");
            }
        }


        public async Task<IDataResult<SelectBranchesDto>> GetAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.BranchRepository.GetAsync(t => t.Id == id, t => t.Periods, t => t.SalesPropositions);
                var mappedEntity = ObjectMapper.Map<Branches, SelectBranchesDto>(entity);
                //var log = LogsAppService.GetLogObject(mappedEntity, mappedEntity, LoginedUserService.UserId, "Branches", id);
                var log = LogsAppService.InsertLogToDatabase<Branches, SelectBranchesDto>(entity, LoginedUserService.UserId, "Branches", LogType.Get, mappedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();
                return new SuccessDataResult<SelectBranchesDto>(mappedEntity);
            }
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListBranchesDto>>> GetListAsync(ListBranchesParameterDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var list = await _uow.BranchRepository.GetListAsync(t => t.IsActive == input.IsActive, t => t.Periods, t => t.SalesPropositions);

                var mappedEntity = ObjectMapper.Map<List<Branches>, List<ListBranchesDto>>(list.ToList());

                return new SuccessDataResult<IList<ListBranchesDto>>(mappedEntity);
            }
        }


        [ValidationAspect(typeof(UpdateBranchesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectBranchesDto>> UpdateAsync(UpdateBranchesDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {

                var entity = await _uow.BranchRepository.GetAsync(x => x.Id == input.Id);

                await _manager.UpdateControl(_uow.BranchRepository, input.Code, input.Id, entity);

                var mappedEntity = ObjectMapper.Map<UpdateBranchesDto, Branches>(input);

                await _uow.BranchRepository.UpdateAsync(mappedEntity);

                //var log = LogsAppService.UpdateLogObject(entity, input, LoginedUserService.UserId, "Branches", mappedEntity.Id);

                var log = LogsAppService.InsertLogToDatabase<Branches, SelectBranchesDto>(mappedEntity, LoginedUserService.UserId, "Branches", LogType.Update, mappedEntity.Id);

                await _uow.LogsRepository.InsertAsync(log);

                await _uow.SaveChanges();

                return new SuccessDataResult<SelectBranchesDto>(ObjectMapper.Map<Branches, SelectBranchesDto>(mappedEntity));
            }
        }

        public async Task<IDataResult<SelectBranchesDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.BranchRepository.GetAsync(x => x.Id == id);

                var updatedEntity = await _uow.BranchRepository.LockRow(entity.Id, lockRow, userId);

                await _uow.SaveChanges();

                var mappedEntity = ObjectMapper.Map<Branches, SelectBranchesDto>(updatedEntity);

                return new SuccessDataResult<SelectBranchesDto>(mappedEntity);
            }
        }
    }
}

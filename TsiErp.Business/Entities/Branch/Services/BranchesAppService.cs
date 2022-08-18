using Tsi.Application.Contract.Services.EntityFrameworkCore;
using Tsi.Caching.Aspect;
using Tsi.Guids;
using Tsi.IoC.Tsi.DependencyResolvers;
using Tsi.Logging.Tsi.Services;
using Tsi.Results;
using Tsi.Validation.Validations.FluentValidation.Aspect;
using TsiErp.Business.Entities.Branch.Validations;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Branch;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Entities.Entities.Branch.Dtos;

namespace TsiErp.Business.Entities.Branch.Services
{
    [ServiceRegistration(typeof(IBranchesAppService), DependencyInjectionType.Scoped)]
    public class BranchesAppService : ApplicationService, IBranchesAppService
    {
        private readonly IBranchesRepository _repository;

        private readonly ILogsAppService _logger;

        public BranchesAppService(IBranchesRepository repository, ILogsAppService logger)
        {
            _repository = repository;
            _logger = logger;
        }

        //[TransactionScopeAspect(Priority = 2)]
        [ValidationAspect(typeof(CreateBranchesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectBranchesDto>> CreateAsync(CreateBranchesDto input)
        {
            var entity = ObjectMapper.Map<CreateBranchesDto, Branches>(input);

            entity.Id = GuidGenerator.CreateGuid();
            entity.CreatorId = Guid.NewGuid();
            entity.CreationTime = DateTime.Now;
            entity.IsDeleted = false;
            entity.DeleterId = null;
            entity.DeletionTime = null;
            entity.LastModifierId = null;
            entity.LastModificationTime = null;

            var addedEntity = await _repository.InsertAsync(entity);

            ObjectMapper.Map<Branches, SelectBranchesDto>(addedEntity);

            return new SuccessDataResult<SelectBranchesDto>(ObjectMapper.Map<Branches, SelectBranchesDto>(addedEntity));
        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
            return new SuccessResult("Silme işlemi başarılı.");
        }


        public async Task<IDataResult<SelectBranchesDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id, t => t.Periods);
            var mappedEntity = ObjectMapper.Map<Branches, SelectBranchesDto>(entity);
            return new SuccessDataResult<SelectBranchesDto>(mappedEntity);
        }


        [CacheAspect(duration: 10)]
        public async Task<IDataResult<IList<ListBranchesDto>>> GetListAsync()
        {
            var list = await _repository.GetListAsync(null, t => t.Periods);

            var mappedEntity = ObjectMapper.Map<List<Branches>, List<ListBranchesDto>>(list.ToList());

            return new SuccessDataResult<IList<ListBranchesDto>>(mappedEntity);
        }

        [ValidationAspect(typeof(UpdateBranchesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectBranchesDto>> UpdateAsync(UpdateBranchesDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            var mappedEntity = ObjectMapper.Map<UpdateBranchesDto, Branches>(input);

            mappedEntity.Id = input.Id;
            mappedEntity.LastModifierId = Guid.NewGuid();
            mappedEntity.LastModificationTime = DateTime.Now;
            mappedEntity.CreatorId = entity.CreatorId;
            mappedEntity.CreationTime = entity.CreationTime;
            mappedEntity.IsDeleted = false;
            mappedEntity.DeleterId = null;
            mappedEntity.DeletionTime = null;

            await _repository.UpdateAsync(mappedEntity);

            await _logger.InsertAsync(new Tsi.Logging.Tsi.Dtos.CreateLogsDto
            {
                AfterValues = mappedEntity,
                BeforeValues = entity,
                Date_ = DateTime.Now,
                LogLevel_ = "Update",
                MethodName_ = "UpdateAsync",
                UserId = Guid.NewGuid()
            });

            return new SuccessDataResult<SelectBranchesDto>(ObjectMapper.Map<Branches, SelectBranchesDto>(mappedEntity));
        }

    }
}

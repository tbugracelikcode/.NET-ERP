using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using Tsi.Authentication.Dtos.Roles;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using Tsi.Logging.Tsi.Services;
using TsiErp.Business.Entities.Authentication.Roles.Validators;
using TsiErp.Business.Entities.Branch.Validations;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Authentication.Roles;
using TsiErp.Entities.Entities.Branch.Dtos;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Business.Extensions.ObjectMapping;
using Tsi.Authentication.Entities.Roles;

namespace TsiErp.Business.Entities.Authentication.Roles.Services
{
    [ServiceRegistration(typeof(IRolesAppService), DependencyInjectionType.Singleton)]
    public class RolesAppService : ApplicationService, IRolesAppService
    {
        private readonly IRolesRepository _repository;

        private readonly ILogsAppService _logger;

        public RolesAppService(IRolesRepository repository, ILogsAppService logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [ValidationAspect(typeof(CreateRolesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectRolesDto>> CreateAsync(CreateRolesDto input)
        {
            var entity = ObjectMapper.Map<CreateRolesDto, TsiRoles>(input);

            entity.Id = GuidGenerator.CreateGuid();
            entity.CreatorId = Guid.NewGuid();
            entity.CreationTime = DateTime.Now;
            entity.IsDeleted = false;
            entity.DeleterId = null;
            entity.DeletionTime = null;
            entity.LastModifierId = null;
            entity.LastModificationTime = null;

            var addedEntity = await _repository.InsertAsync(entity);

            //ObjectMapper.Map<TsiRoles, SelectRolesDto>(addedEntity);

            return new SuccessDataResult<SelectRolesDto>(ObjectMapper.Map<TsiRoles, SelectRolesDto>(addedEntity));
        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
            return new SuccessResult("Silme işlemi başarılı.");
        }

        public async Task<IDataResult<SelectRolesDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id);
            var mappedEntity = ObjectMapper.Map<TsiRoles, SelectRolesDto>(entity);
            return new SuccessDataResult<SelectRolesDto>(mappedEntity);
        }

        public async Task<IDataResult<IList<ListRolesDto>>> GetListAsync()
        {
            var list = await _repository.GetListAsync(null);

            var mappedEntity = ObjectMapper.Map<List<TsiRoles>, List<ListRolesDto>>(list.ToList());

            return new SuccessDataResult<IList<ListRolesDto>>(mappedEntity);
        }

        [ValidationAspect(typeof(UpdateRolesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectRolesDto>> UpdateAsync(UpdateRolesDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            var mappedEntity = ObjectMapper.Map<UpdateRolesDto, TsiRoles>(input);

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

            return new SuccessDataResult<SelectRolesDto>(ObjectMapper.Map<TsiRoles, SelectRolesDto>(mappedEntity));
        }
    }
}

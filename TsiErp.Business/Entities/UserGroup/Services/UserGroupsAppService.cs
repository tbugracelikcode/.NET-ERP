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
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.UserGroup;
using TsiErp.EntityContracts.UserGroup;
using TsiErp.Entities.Entities.UserGroup.Dtos;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Entities.Entities.UserGroup;
using TsiErp.Business.Entities.UserGroup.BusinessRules;

namespace TsiErp.Business.Entities.UserGroup.Services
{
    [ServiceRegistration(typeof(IUserGroupsAppService), DependencyInjectionType.Scoped)]
    public class UserGroupsAppService : ApplicationService, IUserGroupsAppService
    {

        private readonly IUserGroupsRepository _repository;

        UserGroupManager _manager { get; set; } = new UserGroupManager();

        public UserGroupsAppService(IUserGroupsRepository repository)
        {
            _repository = repository;
        }

        [ValidationAspect(typeof(CreateUserGroupsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectUserGroupsDto>> CreateAsync(CreateUserGroupsDto input)
        {
            await _manager.CodeControl(_repository, input.Code);

            var entity = ObjectMapper.Map<CreateUserGroupsDto, UserGroups>(input);

            var addedEntity = await _repository.InsertAsync(entity);
            await _repository.SaveChanges();

            return new SuccessDataResult<SelectUserGroupsDto>(ObjectMapper.Map<UserGroups, SelectUserGroupsDto>(addedEntity));
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            await _manager.DeleteControl(_repository, id);
            await _repository.DeleteAsync(id);
            await _repository.SaveChanges();
            return new SuccessResult("Silme işlemi başarılı.");
        }

        public async Task<IDataResult<SelectUserGroupsDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id, t => t.Users);
            var mappedEntity = ObjectMapper.Map<UserGroups, SelectUserGroupsDto>(entity);
            return new SuccessDataResult<SelectUserGroupsDto>(mappedEntity);
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListUserGroupsDto>>> GetListAsync(ListUserGroupsParameterDto input)
        {
            var list = await _repository.GetListAsync(t => t.IsActive == input.IsActive, t => t.Users);

            var mappedEntity = ObjectMapper.Map<List<UserGroups>, List<ListUserGroupsDto>>(list.ToList());

            return new SuccessDataResult<IList<ListUserGroupsDto>>(mappedEntity);
        }


        [ValidationAspect(typeof(UpdateUserGroupsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectUserGroupsDto>> UpdateAsync(UpdateUserGroupsDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            await _manager.UpdateControl(_repository, input.Code, input.Id, entity);

            var mappedEntity = ObjectMapper.Map<UpdateUserGroupsDto, UserGroups>(input);

            await _repository.UpdateAsync(mappedEntity);
            await _repository.SaveChanges();

            return new SuccessDataResult<SelectUserGroupsDto>(ObjectMapper.Map<UserGroups, SelectUserGroupsDto>(mappedEntity));
        }
    }
}

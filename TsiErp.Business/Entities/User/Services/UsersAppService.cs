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
using TsiErp.Business.Entities.Branch.Validations;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.User;
using TsiErp.Entities.Entities.Branch.Dtos;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Entities.Entities.User.Dtos;
using TsiErp.EntityContracts.User;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Entities.Entities.User;
using TsiErp.Business.Entities.User.BusinessRules;

namespace TsiErp.Business.Entities.User.Services
{
    [ServiceRegistration(typeof(IUsersAppService), DependencyInjectionType.Scoped)]
    public class UsersAppService : ApplicationService, IUsersAppService
    {
        private readonly IUsersRepository _repository;

        UserManager _manager { get; set; } = new UserManager();

        public UsersAppService(IUsersRepository repository)
        {
            _repository = repository;
        }

        [ValidationAspect(typeof(CreateUsersValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectUsersDto>> CreateAsync(CreateUsersDto input)
        {
            await _manager.CodeControl(_repository, input.Code);

            var entity = ObjectMapper.Map<CreateUsersDto, Users>(input);

            var addedEntity = await _repository.InsertAsync(entity);
            await _repository.SaveChanges();

            return new SuccessDataResult<SelectUsersDto>(ObjectMapper.Map<Users, SelectUsersDto>(addedEntity));
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            await _manager.DeleteControl(_repository, id);
            await _repository.DeleteAsync(id);
            await _repository.SaveChanges();
            return new SuccessResult("Silme işlemi başarılı.");
        }

        public async Task<IDataResult<SelectUsersDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id, t => t.UserGroups);
            var mappedEntity = ObjectMapper.Map<Users, SelectUsersDto>(entity);
            return new SuccessDataResult<SelectUsersDto>(mappedEntity);
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListUsersDto>>> GetListAsync(ListUsersParameterDto input)
        {
            var list = await _repository.GetListAsync(t => t.IsActive == input.IsActive, t => t.UserGroups);

            var mappedEntity = ObjectMapper.Map<List<Users>, List<ListUsersDto>>(list.ToList());

            return new SuccessDataResult<IList<ListUsersDto>>(mappedEntity);
        }


        [ValidationAspect(typeof(UpdateUsersValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectUsersDto>> UpdateAsync(UpdateUsersDto input)
        {
            var entity = await _repository.GetAsync(x => x.Id == input.Id);

            await _manager.UpdateControl(_repository, input.Code, input.Id, entity);

            var mappedEntity = ObjectMapper.Map<UpdateUsersDto, Users>(input);

            await _repository.UpdateAsync(mappedEntity);
            await _repository.SaveChanges();

            return new SuccessDataResult<SelectUsersDto>(ObjectMapper.Map<Users, SelectUsersDto>(mappedEntity));
        }
    }
}

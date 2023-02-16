using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Services.BusinessCoreServices;
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
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.Entities.Entities.WorkOrder.Dtos;
using TsiErp.Entities.Entities.WorkOrder;

namespace TsiErp.Business.Entities.User.Services
{
    [ServiceRegistration(typeof(IUsersAppService), DependencyInjectionType.Scoped)]
    public class UsersAppService : ApplicationService, IUsersAppService
    {
        UserManager _manager { get; set; } = new UserManager();

        [ValidationAspect(typeof(CreateUsersValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectUsersDto>> CreateAsync(CreateUsersDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.CodeControl(_uow.UsersRepository, input.Code);

                var entity = ObjectMapper.Map<CreateUsersDto, Users>(input);

                var addedEntity = await _uow.UsersRepository.InsertAsync(entity);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectUsersDto>(ObjectMapper.Map<Users, SelectUsersDto>(addedEntity));
            }
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.DeleteControl(_uow.UsersRepository, id);
                await _uow.UsersRepository.DeleteAsync(id);
                await _uow.SaveChanges();
                return new SuccessResult("Silme işlemi başarılı.");
            }
        }

        public async Task<IDataResult<SelectUsersDto>> GetAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.UsersRepository.GetAsync(t => t.Id == id, t => t.UserGroups);
                var mappedEntity = ObjectMapper.Map<Users, SelectUsersDto>(entity);
                return new SuccessDataResult<SelectUsersDto>(mappedEntity);
            }
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListUsersDto>>> GetListAsync(ListUsersParameterDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var list = await _uow.UsersRepository.GetListAsync(t => t.IsActive == input.IsActive, t => t.UserGroups);

                var mappedEntity = ObjectMapper.Map<List<Users>, List<ListUsersDto>>(list.ToList());

                return new SuccessDataResult<IList<ListUsersDto>>(mappedEntity);
            }
        }


        [ValidationAspect(typeof(UpdateUsersValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectUsersDto>> UpdateAsync(UpdateUsersDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.UsersRepository.GetAsync(x => x.Id == input.Id);

                await _manager.UpdateControl(_uow.UsersRepository, input.Code, input.Id, entity);

                var mappedEntity = ObjectMapper.Map<UpdateUsersDto, Users>(input);

                await _uow.UsersRepository.UpdateAsync(mappedEntity);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectUsersDto>(ObjectMapper.Map<Users, SelectUsersDto>(mappedEntity));
            }
        }

        public async Task<IDataResult<SelectUsersDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.UsersRepository.GetAsync(x => x.Id == id);

                var updatedEntity = await _uow.UsersRepository.LockRow(entity.Id, lockRow, userId);

                await _uow.SaveChanges();

                var mappedEntity = ObjectMapper.Map<Users, SelectUsersDto>(updatedEntity);

                return new SuccessDataResult<SelectUsersDto>(mappedEntity);
            }
        }
    }
}

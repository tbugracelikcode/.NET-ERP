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
using TsiErp.Business.Entities.Authentication.Roles.Validators;
using TsiErp.Business.Entities.Branch.Validations;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Authentication.Roles;
using TsiErp.Entities.Entities.Branch.Dtos;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Business.Extensions.ObjectMapping;
using Tsi.Authentication.Entities.Roles;

namespace TsiErp.Business.Entities.Authentication.Roles.Services
{
    [ServiceRegistration(typeof(IRolesAppService), DependencyInjectionType.Transient)]
    public class RolesAppService :  IRolesAppService
    {
        private readonly IRolesRepository _repository;

        public RolesAppService(IRolesRepository repository)
        {
            _repository = repository;
        }

        [ValidationAspect(typeof(CreateRolesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectRolesDto>> CreateAsync(CreateRolesDto input)
        {
            var entity = ObjectMapper.Map<CreateRolesDto, TsiRoles>(input);

            var addedEntity = await _repository.InsertAsync(entity);

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

            await _repository.UpdateAsync(mappedEntity);

            return new SuccessDataResult<SelectRolesDto>(ObjectMapper.Map<TsiRoles, SelectRolesDto>(mappedEntity));
        }
    }
}

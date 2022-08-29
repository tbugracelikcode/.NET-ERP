using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Authentication.Dtos.Menus;
using Tsi.Authentication.Dtos.Roles;
using Tsi.Authentication.Entities.Menus;
using Tsi.Authentication.Entities.Roles;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.Entities.Authentication.Roles.Services;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Authentication.Menus;

namespace TsiErp.Business.Entities.Authentication.Menus
{
    [ServiceRegistration(typeof(IMenusAppService), DependencyInjectionType.Singleton)]
    public class MenusAppService : IMenusAppService
    {
        private readonly IMenusRepository _repository;

        public MenusAppService(IMenusRepository repository)
        {
            _repository = repository;
        }

        public async Task<IDataResult<IList<ListMenusDto>>> GetListAsync()
        {
            var list = await _repository.GetListAsync(null);

            var mappedEntity = ObjectMapper.Map<List<TsiMenus>, List<ListMenusDto>>(list.ToList());

            return new SuccessDataResult<IList<ListMenusDto>>(mappedEntity);
        }
    }
}

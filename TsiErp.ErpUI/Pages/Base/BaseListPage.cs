using Microsoft.AspNetCore.Components;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using Tsi.Core.Utilities.Results;
using TsiErp.Entities.Entities.Branch.Dtos;

namespace TsiErp.ErpUI.Pages.Base
{
    public class BaseListPage<TEntity, TGetOutputDto, TGetListOutputDto, TCreateInput, TUpdateInput, TGetListInput> : ComponentBase
    {
        public ICrudAppService<TEntity, TGetOutputDto, TGetListOutputDto, TCreateInput, TUpdateInput, TGetListInput> BaseCrudService { get; set; }

        public virtual async Task<IDataResult<IList<TGetListOutputDto>>> GetListAsync(TGetListInput input)
        {
            return await BaseCrudService.GetListAsync(input);
        }
    }
}

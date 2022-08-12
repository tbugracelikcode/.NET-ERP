using System;
using System.Collections.Generic;
using System.Text;

namespace Tsi.Application.Contract.Services.EntityFrameworkCore
{
    public interface ICrudAppService<TEntity, TGetOutputDto, TGetListOutputDto, in TCreateInput, in TUpdateInput> :
        ICreateAppService<TGetOutputDto, TCreateInput>,
        IUpdateAppService<TGetOutputDto, TUpdateInput>,
        IDeleteAppService,
        IReadOnlyAppService<TGetOutputDto,TGetListOutputDto>
    {
    }
}
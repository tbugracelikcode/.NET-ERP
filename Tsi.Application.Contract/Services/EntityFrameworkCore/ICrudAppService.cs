using System;
using System.Collections.Generic;
using System.Text;

namespace Tsi.Application.Contract.Services.EntityFrameworkCore
{
    public interface ICrudAppService< TGetOutputDto, TGetListOutputDto, in TCreateInput, in TUpdateInput, in TGetListInput> :
        ICreateAppService<TGetOutputDto, TCreateInput>,
        IUpdateAppService<TGetOutputDto, TUpdateInput>,
        IDeleteAppService,
        IReadOnlyAppService<TGetOutputDto,TGetListOutputDto,TGetListInput>
    {
    }
}
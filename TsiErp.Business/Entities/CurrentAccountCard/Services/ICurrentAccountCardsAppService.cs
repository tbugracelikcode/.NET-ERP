using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Entities.Entities.CurrentAccountCard.Dtos;
using Tsi.Application.Contract.Services.EntityFrameworkCore;

namespace TsiErp.Business.Entities.CurrentAccountCard.Services
{
    public interface ICurrentAccountCardsAppService : ICrudAppService<SelectCurrentAccountCardsDto, ListCurrentAccountCardsDto, CreateCurrentAccountCardsDto, UpdateCurrentAccountCardsDto, ListCurrentAccountCardsParameterDto>
    {
    }
}

﻿using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard.Dtos;

namespace TsiErp.Business.Entities.CurrentAccountCard.Services
{
    public interface ICurrentAccountCardsAppService : ICrudAppService<SelectCurrentAccountCardsDto, ListCurrentAccountCardsDto, CreateCurrentAccountCardsDto, UpdateCurrentAccountCardsDto, ListCurrentAccountCardsParameterDto>
    {
    }
}

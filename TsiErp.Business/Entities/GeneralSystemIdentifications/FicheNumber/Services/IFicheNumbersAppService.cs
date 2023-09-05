using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.FicheNumber.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.FinanceManagementParameter.Dtos;

namespace TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services
{
    public interface IFicheNumbersAppService : ICrudAppService<SelectFicheNumbersDto, ListFicheNumbersDto, CreateFicheNumbersDto, UpdateFicheNumbersDto, ListFicheNumbersParameterDto>
    {
    }
}

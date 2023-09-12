using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.Results;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.FicheNumber.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.FinanceManagementParameter.Dtos;

namespace TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services
{
    public interface IFicheNumbersAppService : ICrudAppService<SelectFicheNumbersDto, ListFicheNumbersDto, CreateFicheNumbersDto, UpdateFicheNumbersDto, ListFicheNumbersParameterDto>
    {
        string GetFicheNumberAsync(string menu);

        Task UpdateFicheNumberAsync(string menu,string progFicheNumber);
    }
}

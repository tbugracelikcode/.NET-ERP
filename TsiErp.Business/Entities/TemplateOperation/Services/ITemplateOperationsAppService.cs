using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Services.BusinessCoreServices;
using TsiErp.Entities.Entities.TemplateOperation.Dtos;

namespace TsiErp.Business.Entities.TemplateOperation.Services
{
    public interface ITemplateOperationsAppService : ICrudAppService<SelectTemplateOperationsDto, ListTemplateOperationsDto, CreateTemplateOperationsDto, UpdateTemplateOperationsDto, ListTemplateOperationsParameterDto>
    {
    }
}

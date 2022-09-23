using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using TsiErp.Entities.Entities.CustomerComplaintItem;
using TsiErp.Entities.Entities.CustomerComplaintItem.Dtos;

namespace TsiErp.Business.Entities.CustomerComplaintItem.Services
{
    public interface ICustomerComplaintItemsAppService : ICrudAppService<SelectCustomerComplaintItemsDto, ListCustomerComplaintItemsDto, CreateCustomerComplaintItemsDto, UpdateCustomerComplaintItemsDto, ListCustomerComplaintItemsParameterDto>
    {
    }
}

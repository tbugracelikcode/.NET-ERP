using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Services.BusinessCoreServices;
using TsiErp.Entities.Entities.Period.Dtos;
using TsiErp.Entities.Entities.Period;
using TsiErp.Entities.Entities.PurchaseUnsuitabilityReport;
using TsiErp.Entities.Entities.PurchaseUnsuitabilityReport.Dtos;

namespace TsiErp.Business.Entities.PurchaseUnsuitabilityReport.Services
{
    public interface IPurchaseUnsuitabilityReportsAppService : ICrudAppService<SelectPurchaseUnsuitabilityReportsDto, ListPurchaseUnsuitabilityReportsDto, CreatePurchaseUnsuitabilityReportsDto, UpdatePurchaseUnsuitabilityReportsDto, ListPurchaseUnsuitabilityReportsParameterDto>
    {
    }
}

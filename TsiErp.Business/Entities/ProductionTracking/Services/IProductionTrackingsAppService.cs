using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Services.BusinessCoreServices;
using TsiErp.Entities.Entities.ProductionTracking.Dtos;

namespace TsiErp.Business.Entities.ProductionTracking.Services
{
    public interface IProductionTrackingsAppService : ICrudAppService<SelectProductionTrackingsDto, ListProductionTrackingsDto, CreateProductionTrackingsDto, UpdateProductionTrackingsDto, ListProductionTrackingsParameterDto>
    {
    }
}

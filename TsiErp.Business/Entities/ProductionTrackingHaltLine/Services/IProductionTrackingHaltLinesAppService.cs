using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using TsiErp.Entities.Entities.ProductionTracking.Dtos;
using TsiErp.Entities.Entities.ProductionTrackingHaltLine.Dtos;

namespace TsiErp.Business.Entities.ProductionTrackingHaltLine.Services
{
    public interface IProductionTrackingHaltLinesAppService : ICrudAppService<SelectProductionTrackingHaltLinesDto, ListProductionTrackingHaltLinesDto, CreateProductionTrackingHaltLinesDto, UpdateProductionTrackingHaltLinesDto, ListProductionTrackingHaltLinesParameterDto>
    {
    }
}

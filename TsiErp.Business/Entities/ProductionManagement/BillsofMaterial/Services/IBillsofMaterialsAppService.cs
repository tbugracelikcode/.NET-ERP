﻿using Tsi.Core.Utilities.Results;
using TsiErp.Localizations.Resources.Branches.Page;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.ProductionManagement.BillsofMaterial.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.BillsofMaterialLine.Dtos;

namespace TsiErp.Business.Entities.BillsofMaterial.Services
{
    public interface IBillsofMaterialsAppService : ICrudAppService<SelectBillsofMaterialsDto, ListBillsofMaterialsDto, CreateBillsofMaterialsDto, UpdateBillsofMaterialsDto, ListBillsofMaterialsParameterDto>
    {
        Task<IDataResult<SelectBillsofMaterialsDto>> GetbyCurrentAccountIDAsync(Guid currentAccountID, Guid finishedProductId);
        Task<IDataResult<SelectBillsofMaterialsDto>> GetbyProductIDAsync( Guid finishedProductId);
        Task<IDataResult<SelectBillsofMaterialsDto>> GetListbyProductIDAsync(Guid finishedProductId);
        Task<IDataResult<IList<SelectBillsofMaterialLinesDto>>> GetLineListbyProductIDAsync(Guid productID);
        Task<IDataResult<IList<SelectBillsofMaterialLinesDto>>> GetLineListbyFinishedProductIDAsync(Guid finishedProductID);
    }
}

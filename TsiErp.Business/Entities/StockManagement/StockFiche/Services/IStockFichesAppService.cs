﻿using Tsi.Core.Utilities.Results;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.ProductionManagement.ContractTrackingFicheAmountEntryLine.Dtos;
using TsiErp.Entities.Entities.StockManagement.StockFiche.Dtos;
using TsiErp.Entities.Entities.StockManagement.StockFicheLine.Dtos;

namespace TsiErp.Business.Entities.StockFiche.Services
{
    public interface IStockFichesAppService : ICrudAppService<SelectStockFichesDto, ListStockFichesDto, CreateStockFichesDto, UpdateStockFichesDto, ListStockFichesParameterDto>
    {
        Task<List<SelectStockFicheLinesDto>> GetInputList(Guid productId, DateTime? startDate = null, DateTime? endDate = null);
        Task<List<SelectStockFicheLinesDto>> GetOutputList(Guid productId, DateTime? startDate = null, DateTime? endDate = null);
        //Task<IDataResult<SelectStockFichesDto>> GetbyProductionOrderAsync(Guid ProductionOrderID);
        Task<List<SelectStockFicheLinesDto>> GetbyProductionOrderAsync(Guid ProductionOrderID);
        Task<List<SelectStockFicheLinesDto>> GetbyProductionOrderDateReferenceAsync(string DateReference, Guid ProductionOrderID);
        Task<IDataResult<IList<ListStockFichesDto>>> GetListbyProductionOrderAsync(Guid productionOrderID);
        Task<IDataResult<IList<ListStockFicheLinesDto>>> GetLineConsumeListbyProductIDAsync(Guid productID);
        Task<IDataResult<IList<ListStockFicheLinesDto>>> GetLineWastageListbyProductIDAsync(Guid productID);
        Task<IDataResult<IList<ListStockFichesDto>>> GetListbyPurchaseOrderAsync(Guid purchaseOrderID);

        Task<IDataResult<IList<ProductMovementsDto>>> GetProductMovementsByProductIDAsync(Guid ProductID);

        Task<decimal> CalculateProductFIFOCostAsync(Guid productId, List<SelectStockFicheLinesDto> outputList);

        Task<decimal> CalculateProductLIFOCostAsync(Guid productId, List<SelectStockFicheLinesDto> outputList);

        Task<IDataResult<SelectStockFichesDto>> GetbyPurchaseInvoiceAsync(Guid purchaseInvoiceID);
        Task<IDataResult<SelectStockFichesDto>> GetbySalesInvoiceAsync(Guid salesInvoiceID);


        Task<IDataResult<IList<SelectStockFichesDto>>> GetListbyStartEndDateAsync(DateTime startDate, DateTime endDate);


    }
}

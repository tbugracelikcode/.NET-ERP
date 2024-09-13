using DevExpress.Export;
using DevExpress.Utils.Filtering;
using Syncfusion.Blazor.Grids;
using System.Dynamic;
using TsiErp.Business.Entities.BillsofMaterial.Services;
using TsiErp.Business.Entities.PlanningManagement.ShipmentPlanning.Services;
using TsiErp.Business.Entities.ProductGroup.Services;
using TsiErp.Business.Entities.ProductionOrder.Services;
using TsiErp.Business.Entities.WorkOrder.Services;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.WorkOrder.Dtos;
using TsiErp.Entities.Entities.StockManagement.ProductGroup;
using TsiErp.Entities.Enums;
using TsiErp.ErpUI.Models.Dashboard.OperationalDashboard.OpenOrderAnalysis;

namespace TsiErp.ErpUI.Services.Dashboard.OperationalDashboard.OpenOrderAnalysis
{
    public class OpenOrderAnalysisAppService : IOpenOrderAnalysisAppService
    {
        private readonly IProductionOrdersAppService _productionOrdersAppService;
        private readonly IWorkOrdersAppService _workOrdersAppService;
        private readonly IBillsofMaterialsAppService _billsofMaterialsAppService;
        private readonly IProductGroupsAppService _productGroupsAppService;
        private readonly IShipmentPlanningsAppService _shipmentPlanningsAppService;

        public OpenOrderAnalysisAppService(IProductionOrdersAppService productionOrdersAppService, IWorkOrdersAppService workOrdersAppService, IBillsofMaterialsAppService billsofMaterialsAppService, IProductGroupsAppService productGroupsAppService, IShipmentPlanningsAppService shipmentPlanningsAppService)
        {
            _productionOrdersAppService = productionOrdersAppService;
            _workOrdersAppService = workOrdersAppService;
            _billsofMaterialsAppService = billsofMaterialsAppService;
            _productGroupsAppService = productGroupsAppService;
            _shipmentPlanningsAppService = shipmentPlanningsAppService;
        }

        public async Task<List<CurrentBalanceAndQuantityTableDto>> GetCurrentBalanceAndQuantityListAsync()
        {
            List<CurrentBalanceAndQuantityTableDto> result = new List<CurrentBalanceAndQuantityTableDto>();

            List<ProductionOrderStateEnum> orderStates = new List<ProductionOrderStateEnum>()
            {
                ProductionOrderStateEnum.Baslamadi,
                ProductionOrderStateEnum.Durduruldu,
                ProductionOrderStateEnum.Iptal,
                ProductionOrderStateEnum.DevamEdiyor,
                ProductionOrderStateEnum.Tamamlandi
            };


            var productionOrders = (await _productionOrdersAppService.GetListAsync(new ListProductionOrdersParameterDto()))
                .Data
                .Where(t => t.ProductType == ProductTypeEnum.MM && orderStates.Contains(t.ProductionOrderState))
                .OrderBy(t => t.ConfirmedLoadingDate)
                .ToList();



            for (int i = 0; i < productionOrders.Count; i++)
            {

                CurrentBalanceAndQuantityTableDto dto = new CurrentBalanceAndQuantityTableDto
                {
                    ProductGroupName = productionOrders[i].ProductGroupName,
                    LoadingDate = productionOrders[i].ConfirmedLoadingDate,
                    NumberofProductionOrder = productionOrders.Where(t => t.ProductGroupName == productionOrders[i].ProductGroupName).Count() / productionOrders.Where(t => t.ProductGroupName == productionOrders[i].ProductGroupName).Count(),
                    PlannedQuantitySum = (int)productionOrders[i].PlannedQuantity
                };


                result.Add(dto);

            }

            await Task.CompletedTask;
            return result;
        }

        public async Task<List<ProductionOrdersDetailDto>> GetProductionOrdersDetailListAsync(string productGroupName, DateTime confirmedLoadingDate)
        {
            List<ProductionOrdersDetailDto> result = new List<ProductionOrdersDetailDto>();

            var productGroups = (await _productGroupsAppService.GetByNameAsync(productGroupName)).Data;

            var productionOrders = (await _productionOrdersAppService.GetCurrentBalanceAndQuantityDetailListAsync(productGroups.Id, confirmedLoadingDate)).Data.ToList();


            foreach (var productionOrder in productionOrders)
            {

                DateTime? plannedLoadingDate = null;

                var shipmentPlanningDataSource = (await _shipmentPlanningsAppService.GetbyProductionOrderAsync(productionOrder.Id)).Data;

                if(shipmentPlanningDataSource != null && shipmentPlanningDataSource.Id != Guid.Empty)
                {
                    plannedLoadingDate = shipmentPlanningDataSource.PlannedLoadingTime;
                }

                ProductionOrdersDetailDto line = new ProductionOrdersDetailDto
                {
                    ConfirmedLoadingDate = confirmedLoadingDate,
                    CustomerOrderNo = productionOrder.CustomerOrderNo,
                    PlannedLoadingDate = plannedLoadingDate.GetValueOrDefault(),
                    FinishedProductCode = productionOrder.FinishedProductCode,
                    FinishedProductName = productionOrder.FinishedProductName,
                    PlannedQuantity = (int)productionOrder.PlannedQuantity,
                    ProductionOrderFicheNo = productionOrder.FicheNo,
                    ProductGroupName = productGroupName,
                    AS = "",
                    GV = "",
                    ML = "",
                    BR = "",
                    PL = "",
                    SC = ""
                };


                var ymProductionOrders = (await _productionOrdersAppService.GetListAsync(new ListProductionOrdersParameterDto()))
                .Data
                .Where(t => t.LinkedProductionOrderID == productionOrder.Id)
                .ToList();

                foreach (var ymProductionOrder in ymProductionOrders)
                {

                    #region AS
                    if (ymProductionOrder.FinishedProductCode.StartsWith("AS"))
                    {
                        var workOrders = (await _workOrdersAppService.GetSelectListbyProductionOrderAsync(ymProductionOrder.Id)).Data.ToList();

                        if(workOrders != null && workOrders.Count > 0)
                        {
                            if(workOrders.Where(t=>t.WorkOrderState == WorkOrderStateEnum.Tamamlandi).Count() == workOrders.Count())
                            {
                                line.AS = "Hazır";
                            }
                            else
                            {
                                string ymProductCode = ymProductionOrder.FinishedProductCode;
                                string productOperationName = "";

                                for (int i = 0; i < workOrders.Count; i++)
                                {
                                    switch (workOrders[i].WorkOrderState)
                                    {
                                        case WorkOrderStateEnum.Baslamadi:
                                            break;
                                        case WorkOrderStateEnum.Durduruldu:
                                            productOperationName = workOrders[i].ProductsOperationName;
                                            break;
                                        case WorkOrderStateEnum.Iptal:
                                            productOperationName = workOrders[i].ProductsOperationName;
                                            break;
                                        case WorkOrderStateEnum.DevamEdiyor:
                                            productOperationName = workOrders[i].ProductsOperationName;
                                            break;
                                        case WorkOrderStateEnum.Tamamlandi:
                                            productOperationName = workOrders[i + 1].ProductsOperationName;
                                            break;
                                        case WorkOrderStateEnum.FasonaGonderildi:
                                            productOperationName = workOrders[i].ProductsOperationName;
                                            break;
                                        default:
                                            break;
                                    }
                                }

                                if (string.IsNullOrEmpty(productOperationName))
                                {
                                    productOperationName = workOrders[0].ProductsOperationName;
                                }

                                if (line.AS.Contains("--"))
                                {
                                    line.AS = line.AS + " -- " + productOperationName;
                                }
                                else
                                {
                                    line.AS = productOperationName;
                                }
                            }
                            
                        }
                        else
                        {
                            line.AS = "Stoktan Kullanım";
                        }

                       
                    }
                    #endregion

                    #region GV
                    if (ymProductionOrder.FinishedProductCode.StartsWith("GV"))
                    {
                        var workOrders = (await _workOrdersAppService.GetSelectListbyProductionOrderAsync(ymProductionOrder.Id)).Data.ToList();

                        if(workOrders != null && workOrders.Count > 0)
                        {
                            if (workOrders.Where(t => t.WorkOrderState == WorkOrderStateEnum.Tamamlandi).Count() == workOrders.Count())
                            {
                                line.GV = "Hazır";
                            }
                            else
                            {
                                string ymProductCode = ymProductionOrder.FinishedProductCode;
                                string productOperationName = "";

                                for (int i = 0; i < workOrders.Count; i++)
                                {
                                    switch (workOrders[i].WorkOrderState)
                                    {
                                        case WorkOrderStateEnum.Baslamadi:
                                            break;
                                        case WorkOrderStateEnum.Durduruldu:
                                            productOperationName = workOrders[i].ProductsOperationName;
                                            break;
                                        case WorkOrderStateEnum.Iptal:
                                            productOperationName = workOrders[i].ProductsOperationName;
                                            break;
                                        case WorkOrderStateEnum.DevamEdiyor:
                                            productOperationName = workOrders[i].ProductsOperationName;
                                            break;
                                        case WorkOrderStateEnum.Tamamlandi:
                                            productOperationName = workOrders[i + 1].ProductsOperationName;
                                            break;
                                        case WorkOrderStateEnum.FasonaGonderildi:
                                            productOperationName = workOrders[i].ProductsOperationName;
                                            break;
                                        default:
                                            break;
                                    }
                                }

                                if (string.IsNullOrEmpty(productOperationName))
                                {
                                    productOperationName = workOrders[0].ProductsOperationName;
                                }

                                if (line.GV.Contains("--"))
                                {
                                    line.GV = line.GV + " -- " + productOperationName;
                                }
                                else
                                {
                                    line.GV = productOperationName;
                                }
                            }
                           
                        }
                        else
                        {
                            line.GV = "Stoktan Kullanım";
                        }

                       
                    }
                    #endregion

                    #region ML
                    if (ymProductionOrder.FinishedProductCode.StartsWith("ML"))
                    {
                        var workOrders = (await _workOrdersAppService.GetSelectListbyProductionOrderAsync(ymProductionOrder.Id)).Data.ToList();

                        if (workOrders != null && workOrders.Count > 0)
                        {
                            if (workOrders.Where(t => t.WorkOrderState == WorkOrderStateEnum.Tamamlandi).Count() == workOrders.Count())
                            {
                                line.ML = "Hazır";
                            }
                            else
                            {
                                string ymProductCode = ymProductionOrder.FinishedProductCode;
                                string productOperationName = "";

                                for (int i = 0; i < workOrders.Count; i++)
                                {
                                    switch (workOrders[i].WorkOrderState)
                                    {
                                        case WorkOrderStateEnum.Baslamadi:
                                            break;
                                        case WorkOrderStateEnum.Durduruldu:
                                            productOperationName = workOrders[i].ProductsOperationName;
                                            break;
                                        case WorkOrderStateEnum.Iptal:
                                            productOperationName = workOrders[i].ProductsOperationName;
                                            break;
                                        case WorkOrderStateEnum.DevamEdiyor:
                                            productOperationName = workOrders[i].ProductsOperationName;
                                            break;
                                        case WorkOrderStateEnum.Tamamlandi:
                                            productOperationName = workOrders[i + 1].ProductsOperationName;
                                            break;
                                        case WorkOrderStateEnum.FasonaGonderildi:
                                            productOperationName = workOrders[i].ProductsOperationName;
                                            break;
                                        default:
                                            break;
                                    }
                                }

                                if (string.IsNullOrEmpty(productOperationName))
                                {
                                    productOperationName = workOrders[0].ProductsOperationName;
                                }

                                if (line.ML.Contains("--"))
                                {
                                    line.ML = line.ML + " -- " + productOperationName;
                                }
                                else
                                {
                                    line.ML = productOperationName;
                                }
                            }
                            
                        }
                        else
                        {
                            line.ML = "Stoktan Kullanım";
                        }

                      
                    }
                    #endregion

                    #region BR
                    if (ymProductionOrder.FinishedProductCode.StartsWith("BR"))
                    {
                        var workOrders = (await _workOrdersAppService.GetSelectListbyProductionOrderAsync(ymProductionOrder.Id)).Data.ToList();

                        if (workOrders != null && workOrders.Count > 0)
                        {
                            if (workOrders.Where(t => t.WorkOrderState == WorkOrderStateEnum.Tamamlandi).Count() == workOrders.Count())
                            {
                                line.BR = "Hazır";
                            }
                            else
                            {
                                string ymProductCode = ymProductionOrder.FinishedProductCode;
                                string productOperationName = "";

                                for (int i = 0; i < workOrders.Count; i++)
                                {
                                    switch (workOrders[i].WorkOrderState)
                                    {
                                        case WorkOrderStateEnum.Baslamadi:
                                            break;
                                        case WorkOrderStateEnum.Durduruldu:
                                            productOperationName = workOrders[i].ProductsOperationName;
                                            break;
                                        case WorkOrderStateEnum.Iptal:
                                            productOperationName = workOrders[i].ProductsOperationName;
                                            break;
                                        case WorkOrderStateEnum.DevamEdiyor:
                                            productOperationName = workOrders[i].ProductsOperationName;
                                            break;
                                        case WorkOrderStateEnum.Tamamlandi:
                                            productOperationName = workOrders[i + 1].ProductsOperationName;
                                            break;
                                        case WorkOrderStateEnum.FasonaGonderildi:
                                            productOperationName = workOrders[i].ProductsOperationName;
                                            break;
                                        default:
                                            break;
                                    }
                                }

                                if (string.IsNullOrEmpty(productOperationName))
                                {
                                    productOperationName = workOrders[0].ProductsOperationName;
                                }

                                if (line.BR.Contains("--"))
                                {
                                    line.BR = line.BR + " -- " + productOperationName;
                                }
                                else
                                {
                                    line.BR = productOperationName;
                                }
                            }
                           
                        }
                        else
                        {
                            line.BR = "Stoktan Kullanım";
                        }
                      
                    }
                    #endregion

                    #region PL

                    if (ymProductionOrder.FinishedProductCode.StartsWith("PL"))
                    {
                        var workOrders = (await _workOrdersAppService.GetSelectListbyProductionOrderAsync(ymProductionOrder.Id)).Data.ToList();

                        if (workOrders != null && workOrders.Count > 0)
                        {
                            if (workOrders.Where(t => t.WorkOrderState == WorkOrderStateEnum.Tamamlandi).Count() == workOrders.Count())
                            {
                                line.PL = "Hazır";
                            }
                            else
                            {
                                string ymProductCode = ymProductionOrder.FinishedProductCode;
                                string productOperationName = "";

                                for (int i = 0; i < workOrders.Count; i++)
                                {
                                    switch (workOrders[i].WorkOrderState)
                                    {
                                        case WorkOrderStateEnum.Baslamadi:
                                            break;
                                        case WorkOrderStateEnum.Durduruldu:
                                            productOperationName = workOrders[i].ProductsOperationName;
                                            break;
                                        case WorkOrderStateEnum.Iptal:
                                            productOperationName = workOrders[i].ProductsOperationName;
                                            break;
                                        case WorkOrderStateEnum.DevamEdiyor:
                                            productOperationName = workOrders[i].ProductsOperationName;
                                            break;
                                        case WorkOrderStateEnum.Tamamlandi:
                                            productOperationName = workOrders[i + 1].ProductsOperationName;
                                            break;
                                        case WorkOrderStateEnum.FasonaGonderildi:
                                            productOperationName = workOrders[i].ProductsOperationName;
                                            break;
                                        default:
                                            break;
                                    }
                                }

                                if (string.IsNullOrEmpty(productOperationName))
                                {
                                    productOperationName = workOrders[0].ProductsOperationName;
                                }

                                if (line.PL.Contains("--"))
                                {
                                    line.PL = line.PL + " -- " + productOperationName;
                                }
                                else
                                {
                                    line.PL = productOperationName;
                                }
                            }
                           
                        }
                        else
                        {
                            line.PL = "Stoktan Kullanım";
                        }
                           
                    }
                    #endregion

                    #region SC

                    if (ymProductionOrder.FinishedProductCode.StartsWith("SC"))
                    {
                        var workOrders = (await _workOrdersAppService.GetSelectListbyProductionOrderAsync(ymProductionOrder.Id)).Data.ToList();

                        if (workOrders != null && workOrders.Count > 0)
                        {
                            if (workOrders.Where(t => t.WorkOrderState == WorkOrderStateEnum.Tamamlandi).Count() == workOrders.Count())
                            {
                                line.SC = "Hazır";
                            }
                            else
                            {
                                string ymProductCode = ymProductionOrder.FinishedProductCode;
                                string productOperationName = "";

                                for (int i = 0; i < workOrders.Count; i++)
                                {
                                    switch (workOrders[i].WorkOrderState)
                                    {
                                        case WorkOrderStateEnum.Baslamadi:
                                            break;
                                        case WorkOrderStateEnum.Durduruldu:
                                            productOperationName = workOrders[i].ProductsOperationName;
                                            break;
                                        case WorkOrderStateEnum.Iptal:
                                            productOperationName = workOrders[i].ProductsOperationName;
                                            break;
                                        case WorkOrderStateEnum.DevamEdiyor:
                                            productOperationName = workOrders[i].ProductsOperationName;
                                            break;
                                        case WorkOrderStateEnum.Tamamlandi:
                                            productOperationName = workOrders[i + 1].ProductsOperationName;
                                            break;
                                        case WorkOrderStateEnum.FasonaGonderildi:
                                            productOperationName = workOrders[i].ProductsOperationName;
                                            break;
                                        default:
                                            break;
                                    }
                                }

                                if (string.IsNullOrEmpty(productOperationName))
                                {
                                    productOperationName = workOrders[0].ProductsOperationName;
                                }

                                if (line.SC.Contains("--"))
                                {
                                    line.SC = line.SC + " -- " + productOperationName;
                                }
                                else
                                {
                                    line.SC = productOperationName;
                                }
                            }
                            
                        }
                        else
                        {
                            line.SC = "Stoktan Kullanım";
                        }

                    }
                    #endregion
                }

                result.Add(line);
            }

            await Task.CompletedTask;
            return result;
        }
    }
}

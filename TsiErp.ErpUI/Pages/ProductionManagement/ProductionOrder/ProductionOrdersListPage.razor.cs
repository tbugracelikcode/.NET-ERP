using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Business.Entities.BillsofMaterial.Services;
using TsiErp.Business.Entities.Branch.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.PlanningManagementParameter.Services;
using TsiErp.Business.Entities.Warehouse.Services;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder.Dtos;
using TsiErp.Entities.Entities.StockManagement.StockFiche.Dtos;
using TsiErp.Entities.Entities.StockManagement.StockFicheLine.Dtos;
using TsiErp.Entities.Entities.StockManagement.WareHouse.Dtos;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.ProductionManagement.ProductionOrder
{
    public partial class ProductionOrdersListPage
    {

        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        [Inject]
        ModalManager ModalManager { get; set; }

        List<SelectStockFicheLinesDto> StockFicheLineList = new List<SelectStockFicheLinesDto>();

        public bool OccuredAmountPopup = false;
        public string productionDateReferance = string.Empty;
        public decimal quantity = 0;
        Guid? BranchIDParameter;
        Guid? WarehouseIDParameter;

        protected override async void OnInitialized()
        {
            BaseCrudService = ProductionOrdersAppService;
            CreateMainContextMenuItems();
            _L = L;


            var purchaseParameter = (await PurchaseManagementParametersAppService.GetPurchaseManagementParametersAsync()).Data;
            BranchIDParameter = purchaseParameter.BranchID;
            WarehouseIDParameter = purchaseParameter.WarehouseID;

        }

        protected void CreateMainContextMenuItems()
        {
            if (MainGridContextMenu.Count() == 0)
            {
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductionOrderContextWorkOrders"], Id = "workorders" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductionOrderContextOccuredAmountEntry"], Id = "occuredamountentry" });
                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductionOrderContextConsumptionReceipt"], Id = "consumptionreceipt" });
            }
        }

        public async void MainContextMenuClick(ContextMenuClickEventArgs<ListProductionOrdersDto> args)
        {
            switch (args.Item.Id)
            {
                case "workorders":

                    break;

                case "occuredamountentry":

                    DataSource = (await ProductionOrdersAppService.GetAsync(args.RowInfo.RowData.Id)).Data;

                    if (DataSource.WarehouseID == Guid.Empty || DataSource.WarehouseID == null)
                    {
                        var warehouse = (await WarehousesAppService.GetAsync(WarehouseIDParameter.GetValueOrDefault())).Data;
                        DataSource.WarehouseID = warehouse.Id;
                        DataSource.WarehouseCode = warehouse.Code;
                    }

                    if (DataSource.BranchID == Guid.Empty || DataSource.BranchID == null)
                    {
                        var branch = (await BranchesAppService.GetAsync(BranchIDParameter.GetValueOrDefault())).Data;
                        DataSource.BranchID = branch.Id;
                        DataSource.BranchCode = branch.Code;
                    }

                    OccuredAmountPopup = true;

                    break;

                case "consumptionreceipt":

                    break;



                default:
                    break;
            }
        }

        public void HideOccuredAmountPopup()
        {
            OccuredAmountPopup = false;
            quantity = 0;
            productionDateReferance = string.Empty;
        }

        public async Task CreateFiche()
        {
            if (string.IsNullOrEmpty(productionDateReferance) || quantity! > 0)
            {
                await ModalManager.WarningPopupAsync(L["UIWarningCreateOccuredAmountFicheTitle"], L["UIWarningCreateOccuredAmountFicheMessage"]);
            }
            else
            {
                DataSource.ProducedQuantity = DataSource.ProducedQuantity + quantity;

                var updateInput = ObjectMapper.Map<SelectProductionOrdersDto, UpdateProductionOrdersDto>(DataSource);

                await UpdateAsync(updateInput);

                SelectStockFicheLinesDto stockFicheLineModel = new SelectStockFicheLinesDto
                {
                    UnitSetID = DataSource.UnitSetID,
                    UnitSetCode = DataSource.UnitSetCode,
                    UnitPrice = 0,
                    Quantity = quantity,
                    PurchaseOrderLineID = Guid.Empty,
                    PurchaseOrderID = Guid.Empty,
                    PurchaseOrderFicheNo = string.Empty,
                    ProductName = DataSource.FinishedProductName,
                    ProductID = DataSource.FinishedProductID,
                    ProductCode = DataSource.FinishedProductCode,
                    ProductionDateReferance = productionDateReferance,
                    LineNr = 0,
                    LineDescription = string.Empty,
                    LineAmount = 0,
                    FicheType = Entities.Enums.StockFicheTypeEnum.UretimdenGirisFisi,
                };

                StockFicheLineList.Add(stockFicheLineModel);

                CreateStockFichesDto stockFicheModel = new CreateStockFichesDto
                {
                    WarehouseID = DataSource.WarehouseID,
                    Time_ = null,
                    SpecialCode = string.Empty,
                    PurchaseOrderID = Guid.Empty,
                    ProductionOrderID = DataSource.Id,
                    ProductionDateReferance = productionDateReferance,
                    NetAmount = 0,
                    InputOutputCode = 0,
                    FicheNo = FicheNumbersAppService.GetFicheNumberAsync("StockFichesChildMenu"),
                    FicheType = 13,
                    BranchID = DataSource.BranchID,
                    CurrencyID = Guid.Empty,
                    Date_ = DateTime.Now,
                    Description_ = string.Empty,
                    ExchangeRate = 0,



                };

                stockFicheModel.SelectStockFicheLines = StockFicheLineList;

                await StockFichesAppService.CreateAsync(stockFicheModel);

                HideOccuredAmountPopup();
                await InvokeAsync(StateHasChanged);

            }
        }


        #region Depo ButtonEdit 

        SfTextBox WarehousesButtonEdit;
        bool SelectWarehousesPopupVisible = false;
        List<ListWarehousesDto> WarehousesList = new List<ListWarehousesDto>();

        public async Task WarehousesOnCreateIcon()
        {
            var WarehousesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, WarehousesButtonClickEvent);
            await WarehousesButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", WarehousesButtonClick } });
        }

        public async void WarehousesButtonClickEvent()
        {
            SelectWarehousesPopupVisible = true;
            WarehousesList = (await WarehousesAppService.GetListAsync(new ListWarehousesParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public async void WarehousesOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {

                var warehouse = (await WarehousesAppService.GetAsync(WarehouseIDParameter.GetValueOrDefault())).Data;

                DataSource.WarehouseID = warehouse.Id;
                DataSource.WarehouseCode = warehouse.Code;
            }
        }

        public async void WarehousesDoubleClickHandler(RecordDoubleClickEventArgs<ListWarehousesDto> args)
        {
            var selectedWarehouse = args.RowData;

            if (selectedWarehouse != null)
            {
                DataSource.WarehouseID = selectedWarehouse.Id;
                DataSource.WarehouseCode = selectedWarehouse.Code;
                SelectWarehousesPopupVisible = false;


                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Şube ButtonEdit

        SfTextBox BranchesButtonEdit;
        bool SelectBranchesPopupVisible = false;
        List<ListBranchesDto> BranchesList = new List<ListBranchesDto>();

        public async Task BranchesOnCreateIcon()
        {
            var BranchesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, BranchesButtonClickEvent);
            await BranchesButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", BranchesButtonClick } });
        }

        public async void BranchesButtonClickEvent()
        {
            SelectBranchesPopupVisible = true;
            BranchesList = (await BranchesAppService.GetListAsync(new ListBranchesParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public async void BranchesOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {

                var branch = (await BranchesAppService.GetAsync(BranchIDParameter.GetValueOrDefault())).Data;

                DataSource.BranchID = branch.Id;
                DataSource.BranchCode = branch.Code;
            }
        }

        public async void BranchesDoubleClickHandler(RecordDoubleClickEventArgs<ListBranchesDto> args)
        {
            var selectedBranch = args.RowData;

            if (selectedBranch != null)
            {
                DataSource.BranchID = selectedBranch.Id;
                DataSource.BranchCode = selectedBranch.Code;


                SelectBranchesPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

    }
}

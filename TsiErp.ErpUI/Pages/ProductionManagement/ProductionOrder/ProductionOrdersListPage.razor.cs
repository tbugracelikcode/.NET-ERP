using DevExpress.Blazor.Reporting;
using DevExpress.CodeParser;
using DevExpress.DataAccess.ObjectBinding;
using DevExpress.XtraReports.UI;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using Syncfusion.Blazor.Navigations;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using TsiErp.Business.Entities.BankAccount.Services;
using TsiErp.Business.Entities.Currency.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.StockManagementParameter.Services;
using TsiErp.Business.Entities.Product.Services;
using TsiErp.Business.Entities.UnitSet.Services;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Currency.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.BillsofMaterialLine.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.WorkOrder.Dtos;
using TsiErp.Entities.Entities.QualityControl.ProductionOrderChangeReport.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.PackingList.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.Entities.Entities.StockManagement.StockFiche.Dtos;
using TsiErp.Entities.Entities.StockManagement.StockFicheLine.Dtos;
using TsiErp.Entities.Entities.StockManagement.TechnicalDrawing.Dtos;
using TsiErp.Entities.Entities.StockManagement.UnitSet.Dtos;
using TsiErp.Entities.Entities.StockManagement.WareHouse.Dtos;
using TsiErp.Entities.Enums;
using TsiErp.ErpUI.Helpers;
using TsiErp.ErpUI.Pages.ShippingManagement.PackingList;
using TsiErp.ErpUI.Reports.ProductionManagement;
using TsiErp.ErpUI.Reports.ShippingManagement.PackingListReports.ShippingInstruction;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.ProductionManagement.ProductionOrder
{
    public partial class ProductionOrdersListPage : IDisposable
    {

        #region Stock Parameters

        bool futureDateParameter;

        #endregion

        #region HM-YM İzleme Değişkenleri

        public class SPFPTracking
        {
            public ProductTypeEnum LineType { get; set; }
            public string ProductCode { get; set; }
            public string ProductName { get; set; }
            public string UnitSet { get; set; }
            public decimal Quantity { get; set; }
            public decimal TotalCounsume { get; set; }
            public decimal TotalWastage { get; set; }
            public decimal TotalOutput { get; set; }
            public decimal Size { get; set; }
        }

        public List<SPFPTracking> TrackingList = new List<SPFPTracking>();

        private SfGrid<SPFPTracking> _TrackingGrid;

        public bool TrackingModalVisible = false;
        public List<ItemModel> TrackingToolbarItems { get; set; } = new List<ItemModel>();

        #endregion

        public List<ContextMenuItemModel> MainGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> StockFicheGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> StockFicheLineGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> ProductionOrderChangeReportGridContextMenu { get; set; } = new List<ContextMenuItemModel>();

        [Inject]
        ModalManager ModalManager { get; set; }

        List<SelectStockFicheLinesDto> StockFicheLineList;
        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();

        #region Malzeme Fişleri Değişkenleri

        public List<ListStockFichesDto> StockFichesList = new List<ListStockFichesDto>();
        public List<SelectStockFicheLinesDto> StockFicheLinesList = new List<SelectStockFicheLinesDto>();
        SelectStockFichesDto StockFicheDataSource;
        SelectStockFicheLinesDto StockFicheLineDataSource;
        private SfGrid<SelectStockFicheLinesDto> _StockFicheLineGrid;
        private SfGrid<ListStockFichesDto> _StockFicheGrid;
        public bool StockFicheModalVisible = false;
        public bool StockFicheEditPageVisible = false;
        public bool StockFicheLineCrudPopupVisible = false;

        #endregion

        #region Üretim Emri Değişiklik Raporları Değişkenleri


        public List<ListProductionOrderChangeReportsDto> ProductionOrderChangeReportsList = new List<ListProductionOrderChangeReportsDto>();
        private SfGrid<ListProductionOrderChangeReportsDto> _ProductionOrderChangeReportGrid;
        SelectProductionOrderChangeReportsDto SelectProductionOrderChangeReportDataSource;
        public bool ProductionOrderChangeReportModalVisible = false;
        public bool ProductionOrderChangeReportViewModalVisible = false;


        #endregion

        #region Teknik Resim Değiştirme Değişkenleri

        SelectTechnicalDrawingsDto TechDrawingDataSource;
        public bool TechDrawingModalVisible = false;
        public string OldTechDrawingNo = string.Empty;
        public string NewTechDrawingNo = string.Empty;

        #endregion

        public bool OccuredAmountPopup = false;
        public string productionDateReferance = string.Empty;
        public decimal quantity = 0;
        Guid? BranchIDParameter;
        Guid? WarehouseIDParameter;

        protected override async void OnInitialized()
        {
            BaseCrudService = ProductionOrdersAppService;
            _L = L;

            var purchaseParameter = (await PurchaseManagementParametersAppService.GetPurchaseManagementParametersAsync()).Data;
            BranchIDParameter = purchaseParameter.BranchID;
            WarehouseIDParameter = purchaseParameter.WarehouseID;

            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "ProductionOrdersChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();
            #endregion

            CreateMainContextMenuItems();
            CreateStockFichesContextMenuItems();
            CreateStockFicheLinesContextMenuItems();
            CreateProductionOrderChangeContextMenuItems();

            var stockParameterDataSource = (await StockManagementParametersAppService.GetStockManagementParametersAsync()).Data;

            futureDateParameter = stockParameterDataSource.FutureDateParameter;

        }

        protected void CreateMainContextMenuItems()
        {
            if (GridContextMenu.Count == 0)
            {

                foreach (var context in contextsList)
                {
                    var permission = UserPermissionsList.Where(t => t.MenuId == context.Id).Select(t => t.IsUserPermitted).FirstOrDefault();
                    if (permission)
                    {
                        switch (context.MenuName)
                        {
                            case "ProductionOrderContextWorkOrders":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductionOrderContextWorkOrders"], Id = "workorders" }); break;
                            case "ProductionOrderContextOccuredAmountEntry":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductionOrderContextOccuredAmountEntry"], Id = "occuredamountentry" }); break;
                            case "ProductionOrderContextConsumptionReceipt":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductionOrderContextConsumptionReceipt"], Id = "consumptionreceipt" }); break;
                            case "ProductionOrderContextCancel":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductionOrderContextCancel"], Id = "cancel" }); break;
                            case "ProductionOrderContextMaterialSupplyStatus":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductionOrderContextMaterialSupplyStatus"], Id = "materialsupplystatus" }); break;
                            case "ProductionOrderContextWrite":

                                List<MenuItem> subMenus = new List<MenuItem>();

                                var subList = MenusList.Where(t => t.ParentMenuId == context.Id).OrderBy(t => t.ContextOrderNo).ToList();

                                foreach (var subMenu in subList)
                                {
                                    var subPermission = UserPermissionsList.Where(t => t.MenuId == subMenu.Id).Select(t => t.IsUserPermitted).FirstOrDefault();

                                    if (subPermission)
                                    {
                                        switch (subMenu.MenuName)
                                        {
                                            case "ProductionOrderContextRequestForm":
                                                subMenus.Add(new MenuItem { Text = L["ProductionOrderContextRequestForm"], Id = "requestform" }); break;
                                            case "ProductionOrderContextPrintProdOrder":
                                                subMenus.Add(new MenuItem { Text = L["ProductionOrderContextPrintProdOrder"], Id = "printprodorder" }); break;
                                            case "ProductionOrderContextViewProdOrder":
                                                subMenus.Add(new MenuItem { Text = L["ProductionOrderContextViewProdOrder"], Id = "viewprodorder" }); break;
                                            default:
                                                break;
                                        }
                                    }
                                }


                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductionOrderContextWrite"], Id = "write", Items = subMenus }); break;


                            case "ProductionOrderContextMaterialFiches":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductionOrderContextMaterialFiches"], Id = "materialfiches" }); break;
                            case "ProductionOrderContextRefresh":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductionOrderContextRefresh"], Id = "refresh" }); break;
                            case "ProductionOrderContextUnsuitabilityRecords":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductionOrderContextUnsuitabilityRecords"], Id = "unsrecords" }); break;
                            case "ProductionOrderContextTrackingChart":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductionOrderContextTrackingChart"], Id = "trackingchart" }); break;
                            case "ProductionOrderContextChangeTechnicalDrawing":
                                MainGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductionOrderContextChangeTechnicalDrawing"], Id = "changetechdrawing" }); break;


                            default: break;
                        }
                    }
                }
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


                case "cancel":

                    DataSource = (await ProductionOrdersAppService.GetAsync(args.RowInfo.RowData.Id)).Data;

                    DataSource.Cancel_ = true;

                    var updatedEntity = ObjectMapper.Map<SelectProductionOrdersDto, UpdateProductionOrdersDto>(DataSource);

                    await ProductionOrdersAppService.UpdateAsync(updatedEntity);

                    var workOrdersList = (await WorkOrdersAppService.GetSelectListbyProductionOrderAsync(DataSource.Id)).Data.ToList();

                    if (workOrdersList != null && workOrdersList.Count > 0)
                    {
                        foreach (var workOrder in workOrdersList)
                        {
                            workOrder.IsCancel = true;

                            var updatedWorkOrder = ObjectMapper.Map<SelectWorkOrdersDto, UpdateWorkOrdersDto>(workOrder);

                            await WorkOrdersAppService.UpdateAsync(updatedWorkOrder);
                        }
                    }

                    await GetListDataSourceAsync();

                    await _grid.Refresh();

                    await InvokeAsync(StateHasChanged);

                    break;


                case "materialsupplystatus":

                    break;


                case "requestform":
                    RawMaterialRequestFormDynamicReport = new XtraReport();
                    RawMaterialRequestFormReportVisible = true;
                    await CreateRawMaterialRequestFormReport(args.RowInfo.RowData.Id);

                    await InvokeAsync(StateHasChanged);
                    break;


                case "printprodorder":

                    break;


                case "viewprodorder":

                    break;


                case "materialfiches":


                    DataSource = (await ProductionOrdersAppService.GetAsync(args.RowInfo.RowData.Id)).Data;

                    StockFichesList = (await StockFichesAppService.GetListbyProductionOrderAsync(DataSource.Id)).Data.ToList();

                    StockFicheModalVisible = true;

                    await InvokeAsync(StateHasChanged);


                    break;


                case "refresh":

                    await GetListDataSourceAsync();

                    await _grid.Refresh();

                    await InvokeAsync(StateHasChanged);

                    break;


                case "unsrecords":

                    DataSource = (await ProductionOrdersAppService.GetAsync(args.RowInfo.RowData.Id)).Data;

                    ProductionOrderChangeReportsList = (await ProductionOrderChangeReportsAppService.GetListAsync(new ListProductionOrderChangeReportsParameterDto())).Data.Where(t => t.ProductionOrderID == DataSource.Id).ToList();

                    ProductionOrderChangeReportModalVisible = true;

                    await InvokeAsync(StateHasChanged);


                    break;


                case "trackingchart":

                    DataSource = (await ProductionOrdersAppService.GetAsync(args.RowInfo.RowData.Id)).Data;

                    TrackingToolbarItems.Clear();

                    var bomDataSource = (await BillsofMaterialsAppService.GetListbyProductIDAsync(DataSource.FinishedProductID.GetValueOrDefault())).Data;

                    if (bomDataSource != null && bomDataSource.Id != Guid.Empty)
                    {
                        if (bomDataSource.SelectBillsofMaterialLines != null && bomDataSource.SelectBillsofMaterialLines.Count > 0)
                        {
                            TrackingList.Clear();

                            foreach (var bomline in bomDataSource.SelectBillsofMaterialLines)
                            {
                                var consumeQuantity = (await StockFichesAppService.GetLineConsumeListbyProductIDAsync(bomline.ProductID.GetValueOrDefault())).Data.Sum(t => t.Quantity);
                                var wastageQuantity = (await StockFichesAppService.GetLineWastageListbyProductIDAsync(bomline.ProductID.GetValueOrDefault())).Data.Sum(t => t.Quantity);

                                SPFPTracking sPFPTrackingModel = new SPFPTracking
                                {
                                    LineType = bomline.MaterialType,
                                    ProductCode = bomline.ProductCode,
                                    ProductName = bomline.ProductName,
                                    UnitSet = bomline.UnitSetCode,
                                    Quantity = DataSource.PlannedQuantity * bomline.Quantity,
                                    TotalCounsume = consumeQuantity,
                                    TotalWastage = wastageQuantity,
                                    TotalOutput = wastageQuantity + consumeQuantity,
                                    Size = 0
                                };

                                TrackingList.Add(sPFPTrackingModel);
                            }

                            TrackingModalVisible = true;

                            TrackingToolbarItems.Add(new ItemModel() { Id = "ExcelExport", CssClass = "TSIExcelButton", Type = ItemType.Button, PrefixIcon = "TSIExcelIcon", TooltipText = L["UIExportTracking"] });

                            await InvokeAsync(StateHasChanged);
                        }

                    }
                    else
                    {
                        await ModalManager.MessagePopupAsync(L["UIMessageNullBomTitle"], L["UIMessageNullBomMessage"]);
                    }
                    break;


                case "changetechdrawing":

                    DataSource = (await ProductionOrdersAppService.GetAsync(args.RowInfo.RowData.Id)).Data;


                    NewTechDrawingNo = string.Empty;

                    TechDrawingDataSource = (await TechnicalDrawingsAppService.GetSelectListAsync(DataSource.FinishedProductID.GetValueOrDefault())).Data.Where(t => t.IsApproved && t.CustomerApproval).FirstOrDefault();

                    if (TechDrawingDataSource != null && TechDrawingDataSource.Id != Guid.Empty)
                    {

                        OldTechDrawingNo = TechDrawingDataSource.RevisionNo;
                        TechDrawingModalVisible = true;
                    }
                    else
                    {
                        OldTechDrawingNo = string.Empty;
                        await ModalManager.MessagePopupAsync(L["UIMessageNullTechDrawTitle"], L["UIMessageNullTechDrawMessage"]);
                    }



                    await InvokeAsync(StateHasChanged);

                    break;


                default:
                    break;
            }
        }

        public async Task CreateFiche()
        {
            if (quantity == 0)
            {
                await ModalManager.WarningPopupAsync(L["UIWarningCreateOccuredAmountFicheTitle"], L["UIWarningCreateOccuredAmountFicheMessageQuantity"]);
                return;
            }
            else if (string.IsNullOrEmpty(productionDateReferance))
            {
                await ModalManager.WarningPopupAsync(L["UIWarningCreateOccuredAmountFicheTitle"], L["UIWarningCreateOccuredAmountFicheMessageRefNo"]);
                return;
            }
            else
            {
                DataSource.ProducedQuantity = DataSource.ProducedQuantity + quantity;

                var updateInput = ObjectMapper.Map<SelectProductionOrdersDto, UpdateProductionOrdersDto>(DataSource);

                await ProductionOrdersAppService.UpdateAsync(updateInput);

                StockFicheLineList = new List<SelectStockFicheLinesDto>();

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
                    ExchangeRate = 0
                };

                stockFicheModel.SelectStockFicheLines = StockFicheLineList;

                await StockFichesAppService.CreateAsync(stockFicheModel);

                HideOccuredAmountPopup();

                await InvokeAsync(StateHasChanged);
            }
        }

        protected override async Task GetListDataSourceAsync()
        {
            ListDataSource = (await ProductionOrdersAppService.GetNotCanceledListAsync(new ListProductionOrdersParameterDto())).Data.ToList();

            IsLoaded = true;
        }

        public async void HideOccuredAmountPopup()
        {
            OccuredAmountPopup = false;
            quantity = 0;
            productionDateReferance = string.Empty;
            await GetListDataSourceAsync();
        }

        #region Malzeme Fişleri Metotları

        public void CreateStockFichesContextMenuItems()
        {
            if (StockFicheGridContextMenu.Count() == 0)
            {

                List<MenuItem> subMenus = new List<MenuItem>();


                subMenus.Add(new MenuItem { Text = L["StockFicheContextAddStockIncome"], Id = "income" });
                subMenus.Add(new MenuItem { Text = L["StockFicheContextAddStockOutput"], Id = "output" });
                subMenus.Add(new MenuItem { Text = L["StockFicheContextAddConsume"], Id = "consume" });
                subMenus.Add(new MenuItem { Text = L["StockFicheContextAddWastege"], Id = "wastage" });
                subMenus.Add(new MenuItem { Text = L["StockFicheContextAddProductionIncome"], Id = "proincome" });
                subMenus.Add(new MenuItem { Text = L["StockFicheContextAddWarehouse"], Id = "warehouse" });
                subMenus.Add(new MenuItem { Text = L["StockFicheContextAddReserved"], Id = "reserved" });

                StockFicheGridContextMenu.Add(new ContextMenuItemModel { Text = L["StockFichesGeneralConAdd"], Id = "add", Items = subMenus });
                StockFicheGridContextMenu.Add(new ContextMenuItemModel { Text = L["StockFicheContextChange"], Id = "changed" });
                StockFicheGridContextMenu.Add(new ContextMenuItemModel { Text = L["StockFicheContextDelete"], Id = "delete" });
                StockFicheGridContextMenu.Add(new ContextMenuItemModel { Text = L["StockFicheContextRefresh"], Id = "refresh" });

            }

        }

        public void CreateStockFicheLinesContextMenuItems()
        {
            if (StockFicheLineGridContextMenu.Count() == 0)
            {


                StockFicheLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["StockFicheLineContextAdd"], Id = "new" });
                StockFicheLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["StockFicheLineContextChange"], Id = "changed" });
                StockFicheLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["StockFicheLineContextDelete"], Id = "delete" });
                StockFicheLineGridContextMenu.Add(new ContextMenuItemModel { Text = L["StockFicheLineContextRefresh"], Id = "refresh" });

            }

        }

        protected async Task StockFicheBeforeInsertAsync()
        {
            StockFicheDataSource = new SelectStockFichesDto()
            {
                Date_ = DateTime.Now,
                Time_ = DateTime.Now.TimeOfDay,
                FicheNo = FicheNumbersAppService.GetFicheNumberAsync("StockFichesChildMenu"),
                ProductionOrderID = Guid.Empty,
                CurrencyID = Guid.Empty
            };

            StockFicheDataSource.SelectStockFicheLines = new List<SelectStockFicheLinesDto>();
            StockFicheLinesList = StockFicheDataSource.SelectStockFicheLines;

            await Task.CompletedTask;
        }

        public async void StockFicheShowEditPage()
        {

            if (StockFicheDataSource != null)
            {

                if (StockFicheDataSource.DataOpenStatus == true && StockFicheDataSource.DataOpenStatus != null)
                {
                    StockFicheEditPageVisible = false;

                    string MessagePopupInformationDescriptionBase = L["MessagePopupInformationDescriptionBase"];

                    MessagePopupInformationDescriptionBase = MessagePopupInformationDescriptionBase.Replace("{0}", LoginedUserService.UserName);

                    await ModalManager.MessagePopupAsync(L["MessagePopupInformationTitleBase"], MessagePopupInformationDescriptionBase);
                    await InvokeAsync(StateHasChanged);
                }
                else
                {
                    StockFicheEditPageVisible = true;
                    await InvokeAsync(StateHasChanged);
                }
            }
        }

        public async void StockFicheMainContextMenuClick(ContextMenuClickEventArgs<ListStockFichesDto> args)
        {
            foreach (var item in types)
            {
                item.FicheTypeName = L[item.FicheTypeName];
            }

            switch (args.Item.Id)
            {
                case "wastage":

                    await StockFicheBeforeInsertAsync();
                    StockFicheDataSource.FicheType = StockFicheTypeEnum.FireFisi;
                    StockFicheEditPageVisible = true;
                    break;

                case "consume":

                    await StockFicheBeforeInsertAsync();
                    StockFicheDataSource.FicheType = StockFicheTypeEnum.SarfFisi;
                    StockFicheEditPageVisible = true;
                    break;

                case "proincome":

                    await StockFicheBeforeInsertAsync();
                    StockFicheDataSource.FicheType = StockFicheTypeEnum.UretimdenGirisFisi;
                    StockFicheEditPageVisible = true;
                    break;

                case "warehouse":

                    await StockFicheBeforeInsertAsync();
                    StockFicheDataSource.FicheType = StockFicheTypeEnum.DepoSevkFisi;
                    StockFicheEditPageVisible = true;
                    break;
                case "reserved":

                    await StockFicheBeforeInsertAsync();
                    StockFicheDataSource.FicheType = StockFicheTypeEnum.StokRezerveFisi;
                    StockFicheEditPageVisible = true;
                    break;

                case "income":

                    await StockFicheBeforeInsertAsync();
                    StockFicheDataSource.FicheType = StockFicheTypeEnum.StokGirisFisi;
                    StockFicheEditPageVisible = true;
                    break;

                case "output":

                    await StockFicheBeforeInsertAsync();
                    StockFicheDataSource.FicheType = StockFicheTypeEnum.StokCikisFisi;
                    StockFicheEditPageVisible = true;
                    break;

                case "changed":
                    IsChanged = true;
                    StockFicheDataSource = (await StockFichesAppService.GetAsync(args.RowInfo.RowData.Id)).Data;
                    StockFicheLineList = StockFicheDataSource.SelectStockFicheLines;


                    StockFicheShowEditPage();
                    await InvokeAsync(StateHasChanged);
                    break;

                case "delete":
                    var res = await ModalManager.ConfirmationAsync(L["UIConfirmationPopupTitleBase"], L["UIConfirmationPopupStockFicheMessageBase"]);
                    if (res == true)
                    {
                        await StockFichesAppService.DeleteAsync(args.RowInfo.RowData.Id);
                        StockFichesList = (await StockFichesAppService.GetListbyProductionOrderAsync(DataSource.Id)).Data.ToList();
                        await _StockFicheGrid.Refresh();
                        await InvokeAsync(StateHasChanged);
                    }
                    break;

                case "refresh":
                    StockFichesList = (await StockFichesAppService.GetListbyProductionOrderAsync(DataSource.Id)).Data.ToList();
                    await _StockFicheGrid.Refresh();
                    await InvokeAsync(StateHasChanged);
                    break;

                default:
                    break;
            }
        }

        public async void OnListStockFicheLineContextMenuClick(ContextMenuClickEventArgs<SelectStockFicheLinesDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    if (StockFicheDataSource.FicheType == 0)
                    {
                        await ModalManager.WarningPopupAsync(L["UIWarningFicheTypeTitleBase"], L["UIWarningFicheTypeMessageBase"]);
                    }
                    else
                    {
                        StockFicheLineDataSource = new SelectStockFicheLinesDto();
                        StockFicheLineCrudPopupVisible = true;
                        StockFicheLineDataSource.FicheType = StockFicheDataSource.FicheType;
                        StockFicheLineDataSource.LineNr = StockFicheLinesList.Count + 1;
                        await InvokeAsync(StateHasChanged);
                    }

                    break;

                case "changed":
                    StockFicheLineDataSource = args.RowInfo.RowData;
                    StockFicheLineCrudPopupVisible = true;
                    await InvokeAsync(StateHasChanged);
                    break;

                case "delete":

                    var res = await ModalManager.ConfirmationAsync(L["UIConfirmationPopupTitleBase"], L["UIConfirmationPopupMessageLineBase"]);

                    if (res == true)
                    {
                        var line = args.RowInfo.RowData;

                        if (line.Id == Guid.Empty)
                        {
                            StockFicheDataSource.SelectStockFicheLines.Remove(args.RowInfo.RowData);
                        }
                        else
                        {
                            if (line != null)
                            {
                                await StockFichesAppService.DeleteAsync(args.RowInfo.RowData.Id);
                                StockFicheDataSource.SelectStockFicheLines.Remove(line);
                                StockFichesList = (await StockFichesAppService.GetListbyProductionOrderAsync(DataSource.Id)).Data.ToList();
                            }
                            else
                            {
                                StockFicheDataSource.SelectStockFicheLines.Remove(line);
                            }
                        }

                        await _StockFicheLineGrid.Refresh();
                        GetTotal();
                        await InvokeAsync(StateHasChanged);
                    }

                    break;

                case "refresh":
                    StockFichesList = (await StockFichesAppService.GetListbyProductionOrderAsync(DataSource.Id)).Data.ToList();
                    await _StockFicheLineGrid.Refresh();
                    await InvokeAsync(StateHasChanged);
                    break;

                default:
                    break;
            }
        }

        public override async void LineCalculate()
        {
            StockFicheLineDataSource.LineAmount = StockFicheLineDataSource.Quantity * StockFicheLineDataSource.UnitPrice;

            await Task.CompletedTask;
        }

        public async void LineCalculate2()
        {

            StockFicheLineDataSource.TransactionExchangeLineAmount = StockFicheLineDataSource.Quantity * StockFicheLineDataSource.TransactionExchangeUnitPrice;

            await Task.CompletedTask;
        }

        protected async Task OnStockFicheLineLineSubmit()
        {

            if (StockFicheLineDataSource.UnitSetID == Guid.Empty)
            {
                await ModalManager.WarningPopupAsync(L["UIWarningPopupTitleBase"], L["UIWarningPopupMessageBase1"]);
            }
            else if (StockFicheLineDataSource.ProductID == Guid.Empty)
            {
                await ModalManager.WarningPopupAsync(L["UIWarningPopupTitleBase"], L["UIWarningPopupMessageBase2"]);
            }
            else if (StockFicheLineDataSource.Quantity == 0)
            {
                await ModalManager.WarningPopupAsync(L["UIWarningPopupTitleBase"], L["UIWarningPopupMessageBase3"]);
            }
            else
            {
                if (StockFicheLineDataSource.Id == Guid.Empty)
                {
                    if (StockFicheDataSource.SelectStockFicheLines.Contains(StockFicheLineDataSource))
                    {
                        int selectedLineIndex = StockFicheDataSource.SelectStockFicheLines.FindIndex(t => t.LineNr == StockFicheLineDataSource.LineNr);

                        if (selectedLineIndex > -1)
                        {
                            StockFicheDataSource.SelectStockFicheLines[selectedLineIndex] = StockFicheLineDataSource;
                        }
                    }
                    else
                    {
                        StockFicheDataSource.SelectStockFicheLines.Add(StockFicheLineDataSource);
                    }
                }
                else
                {
                    int selectedLineIndex = StockFicheDataSource.SelectStockFicheLines.FindIndex(t => t.Id == StockFicheLineDataSource.Id);

                    if (selectedLineIndex > -1)
                    {
                        StockFicheDataSource.SelectStockFicheLines[selectedLineIndex] = StockFicheLineDataSource;
                    }
                }

                StockFicheLinesList = StockFicheDataSource.SelectStockFicheLines;
                GetTotal();
                await _StockFicheLineGrid.Refresh();

                HideStockFicheLineCrudModal();
                await InvokeAsync(StateHasChanged);
            }

        }

        public async void OnStockFicheSubmit()
        {

            if (StockFicheDataSource.Id == Guid.Empty)
            {
                var createInput = ObjectMapper.Map<SelectStockFichesDto, CreateStockFichesDto>(StockFicheDataSource);

                await StockFichesAppService.CreateAsync(createInput);
            }
            else
            {
                var updateInput = ObjectMapper.Map<SelectStockFichesDto, UpdateStockFichesDto>(StockFicheDataSource);

                await StockFichesAppService.UpdateAsync(updateInput);
            }



            StockFichesList = (await StockFichesAppService.GetListbyProductionOrderAsync(DataSource.Id)).Data.ToList();

            await _StockFicheGrid.Refresh();

            var savedEntityIndex = ListDataSource.FindIndex(x => x.Id == DataSource.Id);

            HideStockFicheEditPage();

            await InvokeAsync(StateHasChanged);

        }

        #region Combobox İşlemleri

        public IEnumerable<SelectStockFichesDto> types = GetEnumDisplayTypeNames<StockFicheTypeEnum>();

        public static List<SelectStockFichesDto> GetEnumDisplayTypeNames<T>()
        {
            var type = typeof(T);
            return Enum.GetValues(type)
                       .Cast<StockFicheTypeEnum>()
                       .Select(x => new SelectStockFichesDto
                       {
                           FicheType = x,
                           FicheTypeName = type.GetMember(x.ToString())
                       .First()
                       .GetCustomAttribute<DisplayAttribute>()?.Name ?? x.ToString()

                       }).ToList();
        }

        #endregion

        #region GetList Metotları

        private async Task GetBranchesList()
        {
            StockFicheBranchesList = (await BranchesAppService.GetListAsync(new ListBranchesParameterDto())).Data.ToList();
        }

        private async Task GetWarehousesList()
        {
            StockFicheWarehousesList = (await WarehousesAppService.GetListAsync(new ListWarehousesParameterDto())).Data.ToList();
        }

        private async Task GetCurrenciesList()
        {
            CurrenciesList = (await CurrenciesAppService.GetListAsync(new ListCurrenciesParameterDto())).Data.ToList();
        }

        private async Task GetProductsList()
        {
            ProductsList = (await ProductsAppService.GetListAsync(new ListProductsParameterDto())).Data.ToList();
        }

        private async Task GetUnitSetsList()
        {
            UnitSetsList = (await UnitSetsAppService.GetListAsync(new ListUnitSetsParameterDto())).Data.ToList();
        }

        #endregion

        #region Şube ButtonEdit

        SfTextBox StockFicheBranchesButtonEdit;
        bool SelectStockFicheBranchesPopupVisible = false;
        List<ListBranchesDto> StockFicheBranchesList = new List<ListBranchesDto>();

        public async Task StockFicheBranchesOnCreateIcon()
        {
            var StockFicheBranchesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, StockFicheBranchesButtonClickEvent);
            await StockFicheBranchesButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", StockFicheBranchesButtonClick } });
        }

        public async void StockFicheBranchesButtonClickEvent()
        {
            SelectStockFicheBranchesPopupVisible = true;
            await GetBranchesList();
            await InvokeAsync(StateHasChanged);
        }

        public void StockFicheBranchesOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                StockFicheDataSource.BranchID = Guid.Empty;
                StockFicheDataSource.BranchCode = string.Empty;
            }
        }

        public async void StockFicheBranchesDoubleClickHandler(RecordDoubleClickEventArgs<ListBranchesDto> args)
        {
            var selectedUnitSet = args.RowData;

            if (selectedUnitSet != null)
            {
                StockFicheDataSource.BranchID = selectedUnitSet.Id;
                StockFicheDataSource.BranchCode = selectedUnitSet.Code;
                SelectStockFicheBranchesPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Para Birimleri ButtonEdit

        SfTextBox CurrenciesButtonEdit;
        bool SelectCurrencyPopupVisible = false;
        List<ListCurrenciesDto> CurrenciesList = new List<ListCurrenciesDto>();

        public async Task CurrenciesOnCreateIcon()
        {
            var CurrenciesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, CurrenciesButtonClickEvent);
            await CurrenciesButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", CurrenciesButtonClick } });
        }

        public async void CurrenciesButtonClickEvent()
        {
            SelectCurrencyPopupVisible = true;
            await GetCurrenciesList();
            await InvokeAsync(StateHasChanged);
        }

        public void CurrenciesOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                StockFicheDataSource.CurrencyID = Guid.Empty;
                StockFicheDataSource.CurrencyCode = string.Empty;
            }
        }

        public async void CurrenciesDoubleClickHandler(RecordDoubleClickEventArgs<ListCurrenciesDto> args)
        {
            var selectedCurrency = args.RowData;

            if (selectedCurrency != null)
            {
                StockFicheDataSource.CurrencyID = selectedCurrency.Id;
                StockFicheDataSource.CurrencyCode = selectedCurrency.Name;
                SelectCurrencyPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Depo ButtonEdit

        SfTextBox StockFicheWarehousesButtonEdit;
        bool SelecStockFichetWarehousesPopupVisible = false;
        List<ListWarehousesDto> StockFicheWarehousesList = new List<ListWarehousesDto>();

        public async Task StockFicheWarehousesOnCreateIcon()
        {
            var StockFicheWarehousesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, StockFicheWarehousesButtonClickEvent);
            await StockFicheWarehousesButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", StockFicheWarehousesButtonClick } });
        }

        public async void StockFicheWarehousesButtonClickEvent()
        {
            SelecStockFichetWarehousesPopupVisible = true;
            await GetWarehousesList();
            await InvokeAsync(StateHasChanged);
        }

        public void SelecStockFicheWarehousesOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                StockFicheDataSource.WarehouseID = Guid.Empty;
                StockFicheDataSource.WarehouseCode = string.Empty;
            }
        }

        public async void StockFicheWarehousesDoubleClickHandler(RecordDoubleClickEventArgs<ListWarehousesDto> args)
        {
            var selectedWarehouse = args.RowData;

            if (selectedWarehouse != null)
            {
                StockFicheDataSource.WarehouseID = selectedWarehouse.Id;
                StockFicheDataSource.WarehouseCode = selectedWarehouse.Code;
                SelecStockFichetWarehousesPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Birim Setleri ButtonEdit

        SfTextBox UnitSetsButtonEdit;
        bool SelectUnitSetsPopupVisible = false;
        List<ListUnitSetsDto> UnitSetsList = new List<ListUnitSetsDto>();

        public async Task UnitSetsOnCreateIcon()
        {
            var UnitSetsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, UnitSetsButtonClickEvent);
            await UnitSetsButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", UnitSetsButtonClick } });
        }

        public async void UnitSetsButtonClickEvent()
        {
            SelectUnitSetsPopupVisible = true;
            await GetUnitSetsList();
            await InvokeAsync(StateHasChanged);
        }

        public void UnitSetsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                StockFicheLineDataSource.UnitSetID = Guid.Empty;
                StockFicheLineDataSource.UnitSetCode = string.Empty;
            }
        }

        public async void UnitSetsDoubleClickHandler(RecordDoubleClickEventArgs<ListUnitSetsDto> args)
        {
            var selectedUnitSet = args.RowData;

            if (selectedUnitSet != null)
            {
                StockFicheLineDataSource.UnitSetID = selectedUnitSet.Id;
                StockFicheLineDataSource.UnitSetCode = selectedUnitSet.Name;
                SelectUnitSetsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Stok Kartı Button Edit

        SfTextBox ProductsCodeButtonEdit;
        SfTextBox ProductsNameButtonEdit;
        bool SelectProductsPopupVisible = false;
        List<ListProductsDto> ProductsList = new List<ListProductsDto>();
        public async Task ProductsCodeOnCreateIcon()
        {
            var ProductsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, ProductsCodeButtonClickEvent);
            await ProductsCodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", ProductsButtonClick } });
        }

        public async void ProductsCodeButtonClickEvent()
        {
            SelectProductsPopupVisible = true;
            await GetProductsList();
            await InvokeAsync(StateHasChanged);
        }
        public async Task ProductsNameOnCreateIcon()
        {
            var ProductsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, ProductsNameButtonClickEvent);
            await ProductsNameButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", ProductsButtonClick } });
        }

        public async void ProductsNameButtonClickEvent()
        {
            SelectProductsPopupVisible = true;
            await GetProductsList();
            await InvokeAsync(StateHasChanged);
        }

        public void ProductsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                StockFicheLineDataSource.ProductID = Guid.Empty;
                StockFicheLineDataSource.ProductCode = string.Empty;
                StockFicheLineDataSource.ProductName = string.Empty;
                StockFicheLineDataSource.UnitSetID = Guid.Empty;
                StockFicheLineDataSource.UnitSetCode = string.Empty;
            }
        }

        public async void ProductsDoubleClickHandler(RecordDoubleClickEventArgs<ListProductsDto> args)
        {
            var selectedProduct = args.RowData;

            if (selectedProduct != null)
            {
                StockFicheLineDataSource.ProductID = selectedProduct.Id;
                StockFicheLineDataSource.ProductCode = selectedProduct.Code;
                StockFicheLineDataSource.ProductName = selectedProduct.Name;
                StockFicheLineDataSource.UnitSetID = selectedProduct.UnitSetID;
                StockFicheLineDataSource.UnitSetCode = selectedProduct.UnitSetCode;
                SelectProductsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        #region İşlem Dövizi Para Birimleri ButtonEdit

        SfTextBox TransactionExchangeCurrenciesButtonEdit;
        bool SelectTransactionExchangeCurrencyPopupVisible = false;
        List<ListCurrenciesDto> TransactionExchangeCurrenciesList = new List<ListCurrenciesDto>();

        public async Task TransactionExchangeCurrenciesOnCreateIcon()
        {
            var TransactionExchangeCurrenciesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, TransactionExchangeCurrenciesButtonClickEvent);
            await TransactionExchangeCurrenciesButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", TransactionExchangeCurrenciesButtonClick } });
        }

        public async void TransactionExchangeCurrenciesButtonClickEvent()
        {
            SelectTransactionExchangeCurrencyPopupVisible = true;
            TransactionExchangeCurrenciesList = (await CurrenciesAppService.GetListAsync(new ListCurrenciesParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public void TransactionExchangeCurrenciesOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                StockFicheDataSource.TransactionExchangeCurrencyID = Guid.Empty;
                StockFicheDataSource.TransactionExchangeCurrencyCode = string.Empty;
            }
        }

        public async void TransactionExchangeCurrenciesDoubleClickHandler(RecordDoubleClickEventArgs<ListCurrenciesDto> args)
        {
            var selectedCurrency = args.RowData;

            if (selectedCurrency != null)
            {
                StockFicheDataSource.TransactionExchangeCurrencyID = selectedCurrency.Id;
                StockFicheDataSource.TransactionExchangeCurrencyCode = selectedCurrency.Name;
                SelectTransactionExchangeCurrencyPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Stok Fiş Kod ButtonEdit

        SfTextBox StockFicheCodeButtonEdit;

        public async Task StockFicheCodeOnCreateIcon()
        {
            var StockFicheCodesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, StockFicheCodeButtonClickEvent);
            await StockFicheCodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", StockFicheCodesButtonClick } });
        }

        public async void StockFicheCodeButtonClickEvent()
        {
            StockFicheDataSource.FicheNo = FicheNumbersAppService.GetFicheNumberAsync("StockFichesChildMenu");
            await InvokeAsync(StateHasChanged);
        }
        #endregion


        public void HideStockFichesModal()
        {
            StockFicheModalVisible = false;
            StockFichesList.Clear();
        }

        public void HideStockFicheEditPage()
        {
            StockFicheEditPageVisible = false;
        }

        public void HideStockFicheLineCrudModal()
        {
            StockFicheLineCrudPopupVisible = false;
        }

        #endregion

        #region Uygunsuzluk Kayıtları Modal İşlemleri

        protected void CreateProductionOrderChangeContextMenuItems()
        {
            if (ProductionOrderChangeReportGridContextMenu.Count() == 0)
            {
                ProductionOrderChangeReportGridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductionOrderChangeReportContextView"], Id = "review" });
            }
        }


        public async void ProductionOrderChangeContextMenuClick(ContextMenuClickEventArgs<ListProductionOrderChangeReportsDto> args)
        {

            switch (args.Item.Id)
            {
                case "review":

                    SelectProductionOrderChangeReportDataSource = (await ProductionOrderChangeReportsAppService.GetAsync(args.RowInfo.RowData.Id)).Data;

                    ProductionOrderChangeReportViewModalVisible = true;

                    await InvokeAsync(StateHasChanged);

                    break;



                default:
                    break;
            }
        }

        public void HideProductionOrderChangeReportModal()
        {
            ProductionOrderChangeReportModalVisible = false;

            ProductionOrderChangeReportsList.Clear();
        }

        public void HideProductionOrderChangeReportViewModal()
        {
            ProductionOrderChangeReportViewModalVisible = false;
        }

        #endregion

        #region Teknik Resim Değiştirme Modalı İşlemleri

        public void HideTechnicalDrawingUpdateModal()
        {
            TechDrawingModalVisible = false;
        }

        public async void UpdateTechnicalDrawing()
        {
            TechDrawingDataSource.RevisionNo = NewTechDrawingNo;

            var updatedTechnicalDrawing = ObjectMapper.Map<SelectTechnicalDrawingsDto, UpdateTechnicalDrawingsDto>(TechDrawingDataSource);

            await TechnicalDrawingsAppService.UpdateAsync(updatedTechnicalDrawing);

            DataSource.TechnicalDrawingID = TechDrawingDataSource.Id;
            DataSource.TechnicalDrawingNo = TechDrawingDataSource.RevisionNo;

            var updatedEntity = ObjectMapper.Map<SelectProductionOrdersDto, UpdateProductionOrdersDto>(DataSource);

            await ProductionOrdersAppService.UpdateAsync(updatedEntity);

            HideTechnicalDrawingUpdateModal();

            await InvokeAsync(StateHasChanged);


        }


        #endregion

        #region HM YM İzleme Metotları

        public void HideTrackingModal()
        {
            TrackingModalVisible = false;
            TrackingList.Clear();
        }

        public async void TrackingToolbarClickHandler(Syncfusion.Blazor.Navigations.ClickEventArgs args)
        {
            ExcelExportProperties ExcelExportProperties = new ExcelExportProperties();
            ExcelExportProperties.FileName = args.Item.TooltipText + ".xlsx";
            await this._TrackingGrid.ExportToExcelAsync(ExcelExportProperties);


        }

        #endregion

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

        #region Yazdır

        #region HM ve YM İstek Formu
        bool RawMaterialRequestFormReportVisible { get; set; }

        DxReportViewer RawMaterialRequestFormReportViewer { get; set; }

        XtraReport RawMaterialRequestFormDynamicReport { get; set; }

        async Task CreateRawMaterialRequestFormReport(Guid productionOrderId)
        {
            RawMaterialRequestFormDynamicReport.ShowPrintMarginsWarning = false;
            RawMaterialRequestFormDynamicReport.CreateDocument();

            if (productionOrderId != Guid.Empty)
            {
                var list = (await ProductionOrdersAppService.CreateRawMaterialRequestFormReportAsync(productionOrderId)).Data;

                RawMaterialRequestFormReport report = new RawMaterialRequestFormReport();
                report.DataSource = list;
                report.ShowPrintMarginsWarning = false;
                report.CreateDocument();

                RawMaterialRequestFormDynamicReport.Pages.AddRange(report.Pages);

                RawMaterialRequestFormDynamicReport.PrintingSystem.ContinuousPageNumbering = true;
            }

            await Task.CompletedTask;
        }


        #endregion

        #endregion

        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }

    }
}
